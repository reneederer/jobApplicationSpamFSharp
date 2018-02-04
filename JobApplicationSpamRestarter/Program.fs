open System.Diagnostics
open System.Threading

let rec observeUnoconv () : Async<unit> =
    let rec restartUnoconv'() =
        if Process.GetProcessesByName("soffice.bin").Length = 0
        then
            use p = new Process()
            p.StartInfo.FileName <- "cmd"
            p.StartInfo.Arguments <-
                """cmd /C ""C:/Program Files/LibreOffice 5/program/python.exe" "c:/Program Files/unoconv/unoconv" --listener" """
            p.Start() |> ignore
        Thread.Sleep(500)
        restartUnoconv' ()
    async { restartUnoconv'() }


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
    p.Id


let observeJobApplicationSpam () =
    let rec observeJobApplicationSpam' processId =
        Thread.Sleep 500
        try
            observeJobApplicationSpam'
                (if (Process.GetProcessById processId).HasExited
                then restartJobApplicationSpam()
                else processId)
        with
        | _ ->
            observeJobApplicationSpam' (restartJobApplicationSpam())
    async { observeJobApplicationSpam' (-1) }

[<EntryPoint>]
let main argv =
    [ observeUnoconv(); observeJobApplicationSpam()]
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
    0

