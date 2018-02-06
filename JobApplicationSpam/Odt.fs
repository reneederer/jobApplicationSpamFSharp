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
    open System.Threading
    open Chessie.ErrorHandling

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


    let saveStreamCompare (sc : StreamCompare) (filePath : string) =
        match sc with
        | OdtStream odtStream ->
            use fs = new FileStream(filePath, FileMode.OpenOrCreate)
            odtStream.Seek(0L, SeekOrigin.Begin) |> ignore
            odtStream.CopyTo(fs)
        | OdtFile path ->
            File.Copy(path, filePath)
        | Stream stream ->
            use fs = new FileStream(filePath, FileMode.OpenOrCreate)
            stream.Seek(0L, SeekOrigin.Begin) |> ignore
            stream.CopyTo(fs)
        | File path ->
            File.Copy(path, filePath)



    let streamCompareToFile (sc : StreamCompare) (extension : string) =
        match sc with
        | OdtStream odtStream ->
            let filePath = Path.Combine(Settings.DataDirectory, "tmp", Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N") + extension)
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)) |> ignore
            use fs = new FileStream(filePath, FileMode.OpenOrCreate)
            odtStream.Seek(0L, SeekOrigin.Begin) |> ignore
            odtStream.CopyTo(fs)
            filePath
        | OdtFile path ->
            path
        | Stream stream ->
            let filePath = Path.Combine(Settings.DataDirectory, "tmp", Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N") + extension)
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)) |> ignore
            use fs = new FileStream(filePath, FileMode.OpenOrCreate)
            stream.Seek(0L, SeekOrigin.Begin) |> ignore
            stream.CopyTo(fs)
            filePath
        | File path ->
            path



    let areStreamsEqual (sc1 : StreamCompare) (sc2 : StreamCompare) : bool =
        (*
        Deletes meta.xml in *.odt files (meta.xml contains values like creation date, which mess up the comparison)
        Removes tags with name Rsid from  settings.xml in *.odt files
        Converts StreamCompare sc to Stream
        *)




        let rec convertToStream (sc : StreamCompare) : Stream =
            match sc with
            | OdtStream odtStream ->
                let tmpDir = Path.Combine(Settings.DataDirectory, "tmp", Guid.NewGuid().ToString("N"))
                Directory.CreateDirectory tmpDir |> ignore
                let zipPath = Path.Combine(tmpDir, "odtbeforedelete.odt")
                use fileStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write)
                odtStream.Seek(0L, SeekOrigin.Begin) |> ignore
                odtStream.CopyTo(fileStream);
                fileStream.Dispose()
                convertToStream (OdtFile zipPath)
            | OdtFile odtFile ->
                let tmpDir = Path.Combine(Settings.DataDirectory, "tmp", Guid.NewGuid().ToString("N"))
                Directory.CreateDirectory tmpDir |> ignore
                let extractedDir = Path.Combine(tmpDir, "extracted")
                Directory.CreateDirectory extractedDir |> ignore
                let completedDir = Path.Combine(tmpDir, "completed")
                Directory.CreateDirectory completedDir |> ignore
                let completedOdtPath = Path.Combine(completedDir, "odtcompleted.odt")
                ZipFile.ExtractToDirectory(odtFile, extractedDir)
                File.Delete(Path.Combine(extractedDir, "meta.xml"))
                let settingsText = File.ReadAllText(Path.Combine(extractedDir, "settings.xml"))
                let pattern = """<config:config-item config:name="Rsid.*?</config:config-item>"""
                let newSettingsText = Regex.Replace(settingsText, pattern, "");
                if newSettingsText = settingsText then failwith "newSettingsText = settingsText!!!"
                File.WriteAllText(Path.Combine(extractedDir, "settings.xml"), newSettingsText)
                File.Delete(Path.Combine(extractedDir, "settings.xml"))
                ZipFile.CreateFromDirectory(extractedDir, completedOdtPath)
                new FileStream(completedOdtPath, FileMode.Open) :> Stream
            | File file ->
                if Path.GetExtension(file).ToLower() = ".odt"
                then convertToStream (OdtFile file)
                else new FileStream(file, FileMode.Open) :> Stream


        use s1 = convertToStream sc1
        use s2 = convertToStream sc2
        s1.Seek(0L, SeekOrigin.Begin) |> ignore
        s2.Seek(0L, SeekOrigin.Begin) |> ignore

        let chunkSize = 2048
        let mutable (b1 : array<byte>) = Array.zeroCreate (chunkSize)
        let mutable (b2 : array<byte>) = Array.zeroCreate (chunkSize)

        let rec areStreamsEqual' () =
            printfn "..."

            let length1 = s1.Read(b1, 0, chunkSize)
            let length2 = s2.Read(b2, 0, chunkSize)
            if length1 <> length2
            then false
            elif (Array.take length1 b1) <> (Array.take length2 b2) then
                Console.WriteLine(s1.Position |> string)
                Array.zip b1 b2
                |> Array.iter
                    (fun x -> printf "%A " x)
                false
            elif length1 <> chunkSize
            then true
            else areStreamsEqual' ()
        areStreamsEqual' ()


    let applyRec path f : list<'a> =
        let rec applyRec' currentDir =
            let directories = Directory.EnumerateDirectories(Path.Combine(path, currentDir))
            let xs =
                [ for directory in directories do
                    yield! applyRec' (Path.Combine(currentDir, Path.GetFileName(directory)))
                ]

            let files = Directory.EnumerateFiles(Path.Combine(path, currentDir))
            xs
            @
            [ for file in files do
                yield f currentDir (Path.GetFileName(file)) ]
        applyRec' ""


    let rec areFilesAndDirectoriesEqual (f1 : string) (f2 : string) : bool =
        let extractOdtForComparison odtFilePath =
            let tmpDir = Path.Combine(Settings.DataDirectory, "tmp", Guid.NewGuid().ToString("N"))
            Directory.CreateDirectory tmpDir |> ignore
            let extractedDir = Path.Combine(tmpDir, "extracted")
            Directory.CreateDirectory extractedDir |> ignore
            ZipFile.ExtractToDirectory(odtFilePath, extractedDir)
            File.Delete(Path.Combine(extractedDir, "meta.xml"))
            let settingsText = File.ReadAllText(Path.Combine(extractedDir, "settings.xml"))
            let pattern = """<config:config-item config:name="Rsid.*?</config:config-item>"""
            let newSettingsText = Regex.Replace(settingsText, pattern, "");
            File.WriteAllText(Path.Combine(extractedDir, "settings.xml"), newSettingsText)
            extractedDir
        
        if Path.GetExtension(f1).ToLower() = ".odt"
        then
            areFilesAndDirectoriesEqual (extractOdtForComparison f1) f2
        elif Path.GetExtension(f2).ToLower() = ".odt"
        then areFilesAndDirectoriesEqual f1 (extractOdtForComparison f2)
        elif Directory.Exists f1 && Directory.Exists f2
        then
            applyRec
                f1
                (fun currentDir currentFile ->
                    let p1 = Path.Combine(f1, currentDir, currentFile)
                    let p2 = Path.Combine(f2, currentDir, currentFile)
                    (   File.Exists p1
                     && File.Exists p2
                     && areStreamsEqual
                          (File (Path.Combine(f1, currentDir, currentFile)))
                          (File (Path.Combine(f2, currentDir, currentFile)))
                    )
                )
            |> List.forall id
        elif File.Exists f1 && File.Exists f2
        then
            let b =
                areStreamsEqual
                    (File f1)
                    (File f2)
            b
        else false
        

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
            (fun currentDir currentFile ->
                if Path.GetExtension(currentFile).ToLower() = (".xml")
                then replaceInFile (Path.Combine(path, currentDir, currentFile)) map Replace
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
        let rec odtToPdf'() =
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
                Some outputPath
            else
                log.Error (sprintf "(odtPath = %s) failed to Convert" odtPath)
                None
        odtToPdf'()
    
    let convertToOdt filePath =
        let rec convertToOdt' n =
            log.Debug (sprintf "(filePath = %s)" filePath)
            let outputPath = Path.ChangeExtension(filePath, ".odt")
            File.Delete(outputPath)
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

