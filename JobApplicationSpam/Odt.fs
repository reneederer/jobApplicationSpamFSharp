﻿namespace JobApplicationSpam
module Odt =
    open Chessie.ErrorHandling
    open Chessie.ErrorHandling.Trial
    open System
    open System.IO
    open System.IO.Compression
    open System.Configuration

    let private replaceAll text map =
        Map.fold (fun (state : string) (key : string) (value : string) -> state.Replace(key, value)) text map

    let private replaceInFile path map =
        let content = File.ReadAllText(path)
        let replacedText = replaceAll content map
        File.WriteAllText(path, replacedText)

    let rec private replaceInDirectory path map =
        let files = Directory.EnumerateFiles(path)
        for file in files do
            replaceInFile file map
        let directories = Directory.EnumerateDirectories(path)
        for directory in directories do
            replaceInDirectory (Path.Combine(path, directory)) map
    
    let replaceInOdt odtPath extractedOdtPath replacedOdtPath map =
        let replacedOdtDirectory = Path.GetDirectoryName(replacedOdtPath)
        if not <| Directory.Exists(replacedOdtDirectory) then Directory.CreateDirectory(replacedOdtDirectory) |> ignore
        if Directory.Exists(extractedOdtPath) then Directory.Delete(extractedOdtPath, true)
        if File.Exists(replacedOdtPath) then File.Delete(replacedOdtPath)
        ZipFile.ExtractToDirectory(odtPath, extractedOdtPath)
        replaceInDirectory extractedOdtPath map
        ZipFile.CreateFromDirectory(extractedOdtPath, replacedOdtPath)
        Directory.Delete(extractedOdtPath, true)
    
    let odtToPdf (odtPath : string) =
        use process1 = new System.Diagnostics.Process()
        process1.StartInfo.FileName <- ConfigurationManager.AppSettings.["python"]
        process1.StartInfo.UseShellExecute <- false
        process1.StartInfo.Arguments <- sprintf """ "%s" -f pdf -eUseLossLessCompression=true "%s" """  (ConfigurationManager.AppSettings.["unoconv"]) odtPath
        process1.StartInfo.CreateNoWindow <- true
        process1.Start() |> ignore
        process1.WaitForExit()
        if File.Exists(odtPath.Substring(0, odtPath.Length - 4) + ".pdf")
        then
            odtPath.Substring(0, odtPath.Length - 4) + ".pdf"
        else failwith "Could not convert odt file to pdf."
    (*
    let map = [ ("$chef_vorname", "Hans"); ("$chef_nachname", "Meiser") ] |> Map.ofList
    let zipPath = @"c:\users\rene\myodt.odt"
    let extractPath = @"c:\users\rene\myodt"
    let newZipPath = @"c:\users\rene\myodt1.odt"
    if Directory.Exists(extractPath) then Directory.Delete(extractPath, true)
    if File.Exists(newZipPath) then File.Delete(newZipPath)
    ZipFile.ExtractToDirectory(zipPath, extractPath)
    replaceInDirectory extractPath map
    ZipFile.CreateFromDirectory(extractPath, newZipPath)
    odtToPdf @"C:\UniServerZ\www\jobApplicationSpam\unoconv-master\tests\document-example.odt"
    *)