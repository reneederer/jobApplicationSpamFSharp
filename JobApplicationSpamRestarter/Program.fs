open System.Diagnostics
open System.Threading
open System.IO
open System

let observeUnoconv () : Async<unit> =
    let restartUnoconv'() =
        if Process.GetProcessesByName("soffice.bin").Length = 0
        then
            use p = new Process()
            p.StartInfo.FileName <- "cmd"
            p.StartInfo.Arguments <-
                """cmd /C ""C:/Program Files/LibreOffice 5/program/python.exe" "c:/Program Files/unoconv/unoconv" --listener" """
            p.Start() |> ignore
    async {
        while true do
            Thread.Sleep(500)
            restartUnoconv' ()
    }

let clearTmpFolder () =
    let rec deleteOldFiles dir = 
        let isOld lastWriteTime = DateTime.Now - lastWriteTime > TimeSpan.FromHours 3.
        let directories = Directory.EnumerateDirectories dir
        for directoryIndex = (Seq.length directories - 1) downto 0 do
            deleteOldFiles (directories |> Seq.item directoryIndex)
        let files = Directory.EnumerateFiles dir
        for fileIndex = (Seq.length files - 1) downto 0 do
            if isOld (File.GetLastWriteTime (files |> Seq.item fileIndex))
            then try File.Delete (files |> Seq.item fileIndex) with _ -> ()
        if Directory.EnumerateFileSystemEntries dir |> Seq.isEmpty
        then try Directory.Delete dir with _ -> ()
    async {
        let tmpDir = @"C:\inetpub\JobApplicationSpamFSharpData\tmp"
        while true do
            deleteOldFiles tmpDir
            Thread.Sleep (TimeSpan.FromHours 2.)
    }


let restartJobApplicationSpam() =
    for p in Process.GetProcessesByName("JobApplicationSpam") do
        try
            p.Kill()
            p.WaitForExit()
        with
        | _ -> ()
    use p = new Process()
    p.StartInfo.FileName <-
        "C:/Users/rene/Documents/Visual Studio 2017/Projects/JobApplicationSpamFSharp/JobApplicationSpam/bin/JobApplicationSpam.exe"
    p.StartInfo.Arguments <-
        """ "C:/Users/rene/Documents/Visual Studio 2017/Projects/JobApplicationSpamFSharp/JobApplicationSpam" "http://localhost" "9000" """
    p.Start() |> ignore

let observeJobApplicationSpam () =
    let observeJobApplicationSpam'() =
        Thread.Sleep 2000
        try
                if Process.GetProcessesByName("JobApplicationSpam").Length = 0
                then restartJobApplicationSpam()
        with
        | _ ->
            restartJobApplicationSpam()
    async {
        while true do
            observeJobApplicationSpam' ()
            Thread.Sleep 2000
    }

[<EntryPoint>]
let main argv =
    [ observeUnoconv(); observeJobApplicationSpam(); clearTmpFolder(); ]
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
    0

