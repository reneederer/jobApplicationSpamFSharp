namespace JobApplicationSpam
module Odt =
    open System
    open System.IO
    open System.IO.Compression
    open System.Configuration
    open PdfSharp.Pdf
    open PdfSharp.Pdf.IO
    open System.Text.RegularExpressions
    open Types
    open WebSharper.UI.V

    let private log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().GetType())

    let replaceInString (text : string) (map : list<string * string>) (emptyTextTagAction : EmptyTextTagAction) =
        let t =
            match emptyTextTagAction with
            | Replace ->
                //let text = """$ha$ha<text:span text:style-name="llo">l</text:span>lo<text:span text:style-name="asf">abc</text:span>welt"""
                let pattern = """\$.*?(<text:span text:style-name="\w*?">).*?(</text:span>)"""
                let rec replaceTags (s : string) beginIndex endIndex =
                    if beginIndex < 0 || endIndex < 0
                    then s
                    else
                        let s1 = s.Substring(beginIndex, endIndex - beginIndex + 1)
                        let m = Regex.Match(s.Substring(beginIndex, endIndex - beginIndex + 1), pattern)
                        if m.Success
                        then
                            let g1 = s.Substring(0, beginIndex)
                            let g2 = s1.Substring(0, m.Groups.[1].Index)
                            let g3begin = m.Groups.[1].Index + m.Groups.[1].Length
                            let g3length = m.Groups.[2].Index - (m.Groups.[1].Index + m.Groups.[1].Length)
                            let g3 = s1.Substring(g3begin, g3length)
                            let g4begin = beginIndex + m.Groups.[2].Index + m.Groups.[2].Length
                            let g4 = s.Substring(g4begin)
                            let nextS = g1 + g2 + g3 + g4
                            let nextDollarIndex =
                                let n = nextS.IndexOf('$', beginIndex + 1)
                                if n < 0 then nextS.Length - 1 else n
                            replaceTags nextS beginIndex nextDollarIndex
                        elif beginIndex <= 0
                        then s
                        else replaceTags s (text.LastIndexOf('$', beginIndex - 1)) (beginIndex)
                replaceTags text (text.LastIndexOf('$')) (text.Length - 1)
            | Ignore -> text

        List.fold
            (fun (state:string) (k: string, v: string) ->
                if v = "" then
                    state.Replace(k + " ", "").Replace(k, "")
                else
                    state.Replace
                        (k
                        , match emptyTextTagAction with
                          | Replace -> System.Security.SecurityElement.Escape(v)
                          | Ignore -> v)
            )
            t
            map

    let replaceInFile path map emptyTextTagAction =
        let content = File.ReadAllText(path)
        let replacedText = replaceInString content map emptyTextTagAction
        File.WriteAllText(path, replacedText)

    let rec private replaceInDirectory path map emptyTextTagAction =
        let files = Directory.EnumerateFiles(path)
        for file in files do
            replaceInFile file map (if file.EndsWith("content.xml") then Replace else Ignore)
        let directories = Directory.EnumerateDirectories(path)
        for directory in directories do
            replaceInDirectory directory map emptyTextTagAction
    
    let replaceInOdt odtPath extractedOdtDirectory replacedOdtDirectory map =
        log.Debug (sprintf "(odtPath=%s, extractedOdtDirectory=%s, replacedOdtDirectory=%s)" odtPath extractedOdtDirectory replacedOdtDirectory)
        let odtFileName = Path.GetFileName(odtPath)
        if not <| Directory.Exists(replacedOdtDirectory) then Directory.CreateDirectory(replacedOdtDirectory) |> ignore
        let replacedOdtPath = Path.Combine(replacedOdtDirectory, odtFileName)
        if Directory.Exists extractedOdtDirectory then Directory.Delete(extractedOdtDirectory, true)
        if File.Exists(replacedOdtPath) then File.Delete(replacedOdtPath)
        ZipFile.ExtractToDirectory(odtPath, extractedOdtDirectory)
        replaceInDirectory extractedOdtDirectory map Replace
        ZipFile.CreateFromDirectory(extractedOdtDirectory, replacedOdtPath)
        Directory.Delete(extractedOdtDirectory, true)
        log.Debug (sprintf "(odtPath=%s, extractedOdtDirectory=%s, replacedOdtDirectory=%s) = %s" odtPath extractedOdtDirectory replacedOdtDirectory replacedOdtPath)
        replacedOdtPath
    
    let odtToPdf (odtPath : string) =
        use process1 = new System.Diagnostics.Process()
        process1.StartInfo.FileName <- ConfigurationManager.AppSettings.["python"]
        process1.StartInfo.UseShellExecute <- false
        process1.StartInfo.Arguments <- sprintf """ "%s" --format pdf -eUseLossLessCompression=true "%s" """  (ConfigurationManager.AppSettings.["unoconv"]) odtPath
        process1.StartInfo.CreateNoWindow <- true
        process1.Start() |> ignore
        process1.WaitForExit()
        if File.Exists(odtPath.Substring(0, odtPath.Length - 4) + ".pdf")
        then
            odtPath.Substring(0, odtPath.Length - 4) + ".pdf"
        else failwith "Could not convert odt file to pdf: " + odtPath
    
    let mergePdfs (pdfPaths : list<string>) (outputPath : string) =
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)) |> ignore
        use outputDocument = new PdfDocument ()
        for pdfPath in pdfPaths do
            let inputDocument = PdfReader.Open(pdfPath, PdfDocumentOpenMode.Import)
            let count = inputDocument.PageCount
            for page in inputDocument.Pages do
                outputDocument.AddPage page |> ignore
        outputDocument.Save outputPath

