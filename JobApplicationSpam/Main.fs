namespace JobApplicationSpam

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Server
open System.IO

type EndPoint =
    | [<EndPoint "/">] Home
    | [<EndPoint "/login">] Login
    | [<EndPoint "/edituservalues">] EditUserValues
    | [<EndPoint "/uploadtemplate">] UploadTemplate
    | [<EndPoint "/addEmployer">] AddEmployer
    | [<EndPoint "/applynow">] ApplyNow
    | [<EndPoint "/showjobapplicationlist">] ShowJobApplicationList
    | [<EndPoint "/about">] About

module Templating =
    open WebSharper.UI.Next.Html


    type MainTemplate = Templating.Template<"Main.html">

    let MenuBar (ctx: Context<EndPoint>) endpoint : Doc list =
        let ( => ) txt act =
             liAttr [if endpoint = act then yield attr.``class`` "active"] [
                aAttr [attr.href (ctx.Link act)] [text txt]
             ]
        [
            li ["Home" => EndPoint.Home]
            li ["Upload" => EndPoint.UploadTemplate]
            li ["Add employer" => EndPoint.AddEmployer]
            li ["Apply now" => EndPoint.ApplyNow]
            li ["About" => EndPoint.About]
        ]

    let main ctx action (title: string) (body: Doc list) =
        Content.Page(
            MainTemplate()
                .Title(title)
                .MenuBar(MenuBar ctx action)
                .Body(body)
                .Doc()
        )

module Site =
    open WebSharper.UI.Next.Html
    open System.Web
    open System.Net.Mime
    open Chessie.ErrorHandling.Trial
    open Chessie.ErrorHandling
    open System.Web
    open WebSharper.UI.Next.Client

    let homePage ctx =
        Templating.main ctx EndPoint.Home "Home" [
            h1 [text "Say Hi to the server!"]
            div [client <@ Client.editUserValues () @>]
        ]
    
    let loginPage ctx =
        Templating.main ctx EndPoint.Home "Home" [
            h1 [text "Say Hi to the server!"]
            div [client <@ Client.editUserValues () @>]
        ]
    

    let uploadTemplatePage (ctx : Context<EndPoint>) =
        let dir = "./reneupload1/"
        if not <| Directory.Exists dir then Directory.CreateDirectory dir |> ignore
        ctx.Request.Files
        |> Seq.iteri
            (fun i (x : HttpPostedFileBase) ->
                if i = 0 && x.FileName <> ""
                then x.SaveAs(dir + x.FileName)
                elif i >= 1  && x.FileName <> ""
                then x.SaveAs(dir + x.FileName)
            )
        let uploadResult = 
            if (not <| Seq.isEmpty ctx.Request.Files)
            then
                Server.uploadTemplate 1
                    (Option.defaultValue "" ctx.Request.Post.["templateName"])
                    (Option.defaultValue "" ctx.Request.Post.["emailSubject"])
                    (Option.defaultValue "" ctx.Request.Post.["emailBody"])
                    (Option.defaultValue "" ctx.Request.Post.["templateName"])
                    (Seq.item 0 ctx.Request.Files |> fun (x : HttpPostedFileBase) -> x.FileName)
                    (Seq.skip 1 ctx.Request.Files |> Seq.map (fun (x : HttpPostedFileBase) -> x.FileName))
                |> Async.Ignore
                |> Async.Start
         (*
      (if not (ctx.Request.Files |> Seq.isEmpty) then text "Files have been uploaded" else text "")
      myText
      *)
        Templating.main ctx EndPoint.UploadTemplate "UploadTemplate"
          [ div
              [ client <@ Client.uploadTemplate() @>
                h1 [text (Seq.length ctx.Request.Files |> string)]
              ]
          ]
    let addEmployerPage ctx =
        Templating.main ctx EndPoint.AddEmployer "AddEmployer" [
            div [client <@ Client.addEmployer () @>]
        ]
    let applyNowPage ctx =
        Templating.main ctx EndPoint.About "Apply now" [
            h1 [text "Apply now"]
            div [client <@ Client.applyNow () @>]
        ]
    let showJobApplicationListPage ctx =
        Templating.main ctx EndPoint.About "About" [
            h1 [text "About"]
            p [text "This is a template WebSharper client-server application."]
        ]

    let aboutPage ctx =
        Templating.main ctx EndPoint.About "About" [
            h1 [text "About"]
            p [text "This is a template WebSharper client-server application."]
        ]

    let main =
        Application.MultiPage (fun ctx endpoint ->
            match endpoint with
            | EndPoint.Home -> homePage ctx
            | EndPoint.Login -> loginPage ctx
            | EndPoint.EditUserValues -> loginPage ctx
            | EndPoint.UploadTemplate -> uploadTemplatePage ctx
            | EndPoint.AddEmployer -> addEmployerPage ctx
            | EndPoint.ApplyNow -> applyNowPage ctx
            | EndPoint.ShowJobApplicationList -> showJobApplicationListPage ctx
            | EndPoint.About -> aboutPage ctx
        )

module SelfHostedServer =

    open global.Owin
    open Microsoft.Owin.Hosting
    open Microsoft.Owin.StaticFiles
    open Microsoft.Owin.FileSystems
    open WebSharper.Owin

    [<EntryPoint>]
    let main args =
        let rootDirectory, url =
            match args with
            | [| rootDirectory; url |] -> rootDirectory, url
            | [| url |] -> "..", url
            | [| |] -> "..", "http://localhost:9000/"
            | _ -> eprintfn "Usage: JobApplicationSpam ROOT_DIRECTORY URL"; exit 1
        use server = WebApp.Start(url, fun appB ->
            appB.UseStaticFiles(
                    StaticFileOptions(
                        FileSystem = PhysicalFileSystem(rootDirectory)))
                .UseSitelet(rootDirectory, Site.main)
            |> ignore)
        stdout.WriteLine("Serving {0}", url)
        stdin.ReadLine() |> ignore
        0