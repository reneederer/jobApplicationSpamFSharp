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

    let private log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().GetType())

    let deleteLine (s : string) (index : int) =
        let deleteLine' (m : Match) =
             s.Substring(0, m.Index) + s.Substring(m.Index + m.Length)
        
        let pattern =
            if s.Trim().StartsWith("<?xml ")
            then
                "((<text:p.*?</text:p>)|(<w:p.*?</w:p>))"
            else "(?<=(\n|^)).*?\r?(\n|$)"
        let matches = Regex.Matches(s, pattern)
        let rec tryFindMatch n =
            if n < 0
            then None
            elif matches.[n].Success && matches.[n].Index <= index
            then Some matches.[n]
            else tryFindMatch (n - 1)
        let oM = tryFindMatch (matches.Count - 1)
        oM |> Option.map (fun m -> deleteLine' m) |> Option.defaultValue ""




    let areStreamsEqual (fs1 : Stream) (fs2 : Stream) =
        let chunkSize = 2048
        let mutable (b1 : array<byte>) = Array.zeroCreate (chunkSize)
        let mutable (b2 : array<byte>) = Array.zeroCreate (chunkSize)

        let rec areFileStreamsEqual () =

            let length = fs1.Read(b1, 0, chunkSize)
            fs2.Read(b2, 0, chunkSize) |> ignore
            if (Array.take length b1) <> (Array.take length b2) then
                false
            elif length <> chunkSize then
                true
            else
                areFileStreamsEqual ()

        if fs1.Length <> fs2.Length
        then false
        else areFileStreamsEqual ()



    let replaceInString (text1 : string) (map : list<string * string>) (emptyTextTagAction1 : EmptyTextTagAction) =
        let rec replaceInString' (text : string) (emptyTextTagAction : EmptyTextTagAction) depth =
            if depth >= 100
            then text
            else
                let t =
                    match emptyTextTagAction with
                    | Replace ->
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
                        let replace (s : string) (k1 : string) (v1: string) = 
                            let replacedV =
                                if v1.Contains "$" && s.Contains k1
                                then replaceInString' v1 Ignore (depth + 1)
                                else v1

                            if replacedV = ""
                            then
                                if k1.EndsWith "_"
                                then
                                    let rec deleteAllLinesWithKey (s2: string) =
                                        let keyIndex = s2.IndexOf(k1)
                                        if keyIndex = -1
                                        then s2
                                        else
                                            deleteAllLinesWithKey (deleteLine s2 keyIndex)
                                    deleteAllLinesWithKey s
                                else s.Replace(k1 + " ", "").Replace(k1, "")
                            else
                                let replaceValue =
                                    match emptyTextTagAction with
                                    | Replace -> System.Security.SecurityElement.Escape(replacedV)
                                    | Ignore -> replacedV
                                s.Replace(k1 + "_", replaceValue).Replace(k1, replaceValue)
                        let stateWithReplacedUderScoreVar = replace state (k + "_") v
                        replace stateWithReplacedUderScoreVar k v
                    )
                    t
                    map
        replaceInString' text1 emptyTextTagAction1 0




















    let replaceInFile path map emptyTextTagAction =
        let content = File.ReadAllText(path)
        let replacedText = replaceInString content map emptyTextTagAction
        File.WriteAllText(path, replacedText)

    let rec private replaceInDirectory path map emptyTextTagAction =
        let files = Directory.EnumerateFiles(path)
        for file in files do
            if file.EndsWith(".xml") then replaceInFile file map Replace
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
        log.Debug (sprintf "(odtPath = %s)" odtPath)
        let outputPath = Path.ChangeExtension(odtPath, ".pdf")
        File.Delete(outputPath)
        use process1 = new System.Diagnostics.Process()
        process1.StartInfo.FileName <- ConfigurationManager.AppSettings.["python"]
        process1.StartInfo.UseShellExecute <- false
        process1.StartInfo.Arguments <-
            sprintf
                """ "%s" --format pdf -eUseLossLessCompression=true "%s" """
                (ConfigurationManager.AppSettings.["unoconv"])
                odtPath
        process1.StartInfo.CreateNoWindow <- true
        process1.Start() |> ignore
        process1.WaitForExit()
        let outputPath = Path.ChangeExtension(odtPath, ".pdf")
        if File.Exists outputPath
        then
            log.Debug (sprintf "(odtPath = %s) = %s" odtPath outputPath)
            outputPath
        else
            log.Error (sprintf "(odtPath = %s) failed to Convert" odtPath)
            failwith "Could not convert odt file to pdf: " + odtPath
    
    let convertToOdt filePath =
        log.Debug (sprintf "(filePath = %s)" filePath)
        let outputPath = Path.ChangeExtension(filePath, ".odt")
        use process1 = new System.Diagnostics.Process()
        process1.StartInfo.FileName <- ConfigurationManager.AppSettings.["python"]
        process1.StartInfo.UseShellExecute <- false
        process1.StartInfo.Arguments <-
            sprintf
                """ "%s" --format odt "%s" """
                (ConfigurationManager.AppSettings.["unoconv"])
                filePath
        process1.StartInfo.CreateNoWindow <- true
        process1.Start() |> ignore
        process1.WaitForExit()
        if File.Exists outputPath
        then
            log.Debug (sprintf "(filePath = %s) = %s" filePath outputPath)
            outputPath
        else
            log.Error (sprintf "(filePath = %s) failed to Convert" filePath)
            failwith "Could not convert file to odt: " + filePath
    
    let mergePdfs (pdfPaths : list<string>) (outputPath : string) =
        log.Debug (sprintf "(pdfPaths = %A, outputPath = %s)" pdfPaths outputPath)
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)) |> ignore
        use outputDocument = new PdfDocument ()
        for pdfPath in pdfPaths do
            let inputDocument = PdfReader.Open(pdfPath, PdfDocumentOpenMode.Import)
            for page in inputDocument.Pages do
                outputDocument.AddPage page |> ignore
        outputDocument.Save outputPath
        log.Debug (sprintf "(pdfPaths = %A, outputPath = %s) = ()" pdfPaths outputPath)

