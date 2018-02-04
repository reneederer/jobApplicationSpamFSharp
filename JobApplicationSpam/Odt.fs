﻿namespace JobApplicationSpam
module Odt =
    open System
    open System.IO
    open System.IO.Compression
    open System.Configuration
    open PdfSharp.Pdf
    open PdfSharp.Pdf.IO
    open System.Text.RegularExpressions
    open Types
    open System.Threading

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
        let chunkSize = 10
        let mutable (b1 : array<byte>) = Array.zeroCreate (chunkSize)
        let mutable (b2 : array<byte>) = Array.zeroCreate (chunkSize)

        let rec areStreamsEqual' () =

            let length1 = fs1.Read(b1, 0, chunkSize)
            let length2 = fs2.Read(b2, 0, chunkSize)
            if length1 <> length2
            then
                false
            elif (Array.take length1 b1) <> (Array.take length2 b2) then
                Console.WriteLine(fs1.Position |> string)
                Array.zip b1 b2 |> Array.skipWhile(fun (x1, x2) -> x1 = x2)
                |> Array.iter
                    (fun x -> printf "%A " x)
                false
            elif length1 <> chunkSize then
                true
            else
                areStreamsEqual' ()

        areStreamsEqual' ()


    let rec applyRec path f : list<'a> =
        let directories = Directory.EnumerateDirectories(path)
        let xs =
            [ for directory in directories do
                yield! applyRec directory f
            ]

        let files = Directory.EnumerateFiles(path)
        xs
        @
        [ for file in files do
            yield f file ]


    let areOdtFilesEqual odtPath1 odtPath2 =
        printfn "o1: %s, o2: %s" odtPath1 odtPath2
        let guid1 = Guid.NewGuid().ToString("N")
        let extractedOdtDirectory1 =
            Path.Combine(
                Settings.DataDirectory
                , "tmp"
                , guid1)

        let guid2 = Guid.NewGuid().ToString("N")
        let extractedOdtDirectory2 =
            Path.Combine(
                Settings.DataDirectory
                , "tmp"
                , guid2)

        ZipFile.ExtractToDirectory(odtPath1, extractedOdtDirectory1)
        ZipFile.ExtractToDirectory(odtPath2, extractedOdtDirectory2)


        applyRec
            extractedOdtDirectory1
            (fun path1 ->
                if path1.ToLower().EndsWith("meta.xml")
                then true
                else
                    let path2 = Path.Combine(extractedOdtDirectory2, path1.Substring(extractedOdtDirectory1.Length + 1))

                    if not <| File.Exists path1 || not <| File.Exists path2
                    then false
                    else
                        use fs1 = 
                            new FileStream(
                                  path1
                                , FileMode.Open
                                , FileAccess.Read)
                        use fs2 = 
                            new FileStream(
                                  path2
                                , FileMode.Open
                                , FileAccess.Read)

                        areStreamsEqual fs1 fs2
            )
        |> List.forall id



        //log.Debug (sprintf "(odtPath=%s, extractedOdtDirectory=%s, replacedOdtDirectory=%s) = %s" odtPath extractedOdtDirectory replacedOdtDirectory replacedOdtPath)




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
                                    | Ignore -> System.Security.SecurityElement.Escape(replacedV)
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

    

    let rec replaceInExtractedOdtDirectory path map emptyTextTagAction =
        applyRec
            path
            (fun fileName ->
                if fileName.ToLower().EndsWith (".xml")
                then replaceInFile fileName map Replace
                else ()
            ) |> ignore


    
    let replaceInOdt odtPath extractedOdtDirectory replacedOdtDirectory map =
        log.Debug (sprintf "(odtPath=%s, extractedOdtDirectory=%s, replacedOdtDirectory=%s)" odtPath extractedOdtDirectory replacedOdtDirectory)
        let odtFileName = Path.GetFileName(odtPath)
        if not <| Directory.Exists(replacedOdtDirectory) then Directory.CreateDirectory(replacedOdtDirectory) |> ignore
        let replacedOdtPath = Path.Combine(replacedOdtDirectory, odtFileName)
        if Directory.Exists extractedOdtDirectory then Directory.Delete(extractedOdtDirectory, true)
        if File.Exists(replacedOdtPath) then File.Delete(replacedOdtPath)
        ZipFile.ExtractToDirectory(odtPath, extractedOdtDirectory)
        replaceInExtractedOdtDirectory extractedOdtDirectory map Replace
        ZipFile.CreateFromDirectory(extractedOdtDirectory, replacedOdtPath)
        Directory.Delete(extractedOdtDirectory, true)
        log.Debug (sprintf "(odtPath=%s, extractedOdtDirectory=%s, replacedOdtDirectory=%s) = %s" odtPath extractedOdtDirectory replacedOdtDirectory replacedOdtPath)
        replacedOdtPath
    
    let odtToPdf (odtPath : string) =
        let rec odtToPdf' n =
            log.Debug (sprintf "(odtPath = %s)" odtPath)
            let outputPath = Path.ChangeExtension(odtPath, ".pdf")
            File.Delete(outputPath)
            use process1 = new System.Diagnostics.Process()
            process1.StartInfo.FileName <- Settings.Python
            process1.StartInfo.UseShellExecute <- false
            process1.StartInfo.Arguments <-
                sprintf
                    """ "%s" --format pdf -eUseLossLessCompression=true "%s" """
                    Settings.Unoconv
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
                if n < 10
                then
                    Thread.Sleep 5000
                    odtToPdf' (n + 1)
                else
                    log.Error (sprintf "(odtPath = %s) failed to Convert" odtPath)
                    failwith "Could not convert odt file to pdf: " + odtPath
        odtToPdf' 0
    
    let convertToOdt filePath =
        let rec convertToOdt' n =
            log.Debug (sprintf "(filePath = %s)" filePath)
            let outputPath = Path.ChangeExtension(filePath, ".odt")
            if File.Exists(outputPath) then File.Delete(outputPath)
            use process1 = new System.Diagnostics.Process()
            process1.StartInfo.FileName <- Settings.Python
            process1.StartInfo.UseShellExecute <- false
            process1.StartInfo.Arguments <-
                sprintf
                    """ "%s" --format odt --output="%s" "%s" """
                    Settings.Unoconv
                    outputPath
                    filePath
            printfn "%s" process1.StartInfo.Arguments
            process1.StartInfo.CreateNoWindow <- true
            process1.Start() |> ignore
            process1.WaitForExit()
            if File.Exists outputPath
            then
                log.Debug (sprintf "(filePath = %s) = %s" filePath outputPath)
                outputPath
            else
                if n < 5
                then
                    Thread.Sleep 5000
                    convertToOdt' (n + 1)
                else
                    log.Error (sprintf "(filePath = %s) failed to Convert" filePath)
                    failwith "Could not convert file to odt: " + filePath
        convertToOdt' 0
    
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

