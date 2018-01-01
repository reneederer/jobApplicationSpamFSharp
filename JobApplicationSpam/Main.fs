namespace JobApplicationSpam

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Server
open System.IO
open Chessie.ErrorHandling

type EndPoint =
    | [<EndPoint "/">] Home
    | [<EndPoint "/login">] Login
    | [<EndPoint "/register">] Register
    | [<EndPoint "/showsentjobapplications">] ShowSentJobApplications
    | [<EndPoint "/about">] About
    | [<EndPoint "/confirmemail">] ConfirmEmail
    | [<EndPoint "/templates">] Templates

module Templating =
    open WebSharper.UI.Next.Html


    type MainTemplate = Templating.Template<"Main.html">

    let MenuBar (ctx: Context<EndPoint>) endpoint : Doc list =
        let ( => ) txt act =
             liAttr [if endpoint = act then yield attr.``class`` "active"] [
                aAttr [attr.href (ctx.Link act)] [text txt]
             ]
        [
            (*li ["Home" => EndPoint.Home]
            li ["Upload" => EndPoint.UploadTemplate]
            li ["Add employer" => EndPoint.AddEmployer]
            li ["Apply now" => EndPoint.ApplyNow]
            li ["Register" => EndPoint.Register]
            li ["Login" => EndPoint.Login ]
            li ["About" => EndPoint.About]
            li ["ShowSentJobApplications" => EndPoint.ShowSentJobApplications]
            li ["Edit user values" => EndPoint.EditUserValues]
            li ["Create template" => EndPoint.CreateTemplate]
            *)
        ]
    let SideBar (ctx: Context<EndPoint>) endpoint : Doc list =
        let ( => ) txt act =
             liAttr [if endpoint = act then yield attr.``class`` "active"] [
                aAttr [attr.href (ctx.Link act)] [text txt]
             ]
        [
            li ["Home" => EndPoint.Home]
            li ["About" => EndPoint.About]
        ]
    
    let loggedInUserEmail (ctx: Context<EndPoint>) =
        ctx.UserSession.GetLoggedInUser()
        |> Async.RunSynchronously
        |> Option.map System.Int32.Parse
        |> Option.bind (Server.getEmailByUserId >> Async.RunSynchronously)
        |> Option.defaultValue ""

    let main (ctx : Context<EndPoint>) (action : EndPoint) (title: string) (body: Doc list) : Async<Content<'a>>=
        Content.Page(
            MainTemplate()
                .Title(title)
                .MenuBar(MenuBar ctx action)
                .Body(body)
                .LoggedInUserEmail(loggedInUserEmail ctx)
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
    open System


    let homePage (ctx : Context<EndPoint>) =
        Templating.main ctx EndPoint.Home "Home" [
            h1 [text "Say Hi to the server!"]
            //div [client <@ Client.editUserValues () @>]
        ]
    
    let loginPage (ctx : Context<EndPoint>) =
        Templating.main ctx EndPoint.Login "Login" [
            h1 [text "Login"]
            client <@ Client.login () @>
        ]

    let registerPage (ctx : Context<EndPoint>) =
        Templating.main ctx EndPoint.Register "Register" [
            h1 [text "Register"]
            client <@ Client.register () @>
        ]


    let showSentJobApplications ctx =
        Templating.main ctx EndPoint.About "About" [
            h1 [text "About"]
            p [text "This is a template WebSharper client-server application."]
        ]

    let aboutPage ctx =
        Templating.main ctx EndPoint.About "About" [
            h1 [text "About"]
            p [text "This is a template WebSharper client-server application."]
        ]
    
    let confirmEmailPage (ctx : Context<EndPoint>) =
        let message =
            match ctx.Request.Get.["email"], ctx.Request.Get.["guid"] with
            | Some email, Some guid->
                match Server.confirmEmail email guid |> Async.RunSynchronously with
                | Ok _ -> "Great! Your email has been confirmed."
                | Bad vs -> "Email confirmation has failed."
            | Some _, None
            | None, Some _
            | None, None ->
                "Confirmation requires email and guid to be set."
        Templating.main ctx EndPoint.About "About" [
            h1 [text message]
        ]

    let templatesPage (ctx : Context<EndPoint>) =
        let dir = "./reneupload1/"
        if not <| Directory.Exists dir then Directory.CreateDirectory dir |> ignore
        ctx.Request.Files
        |> Seq.iteri
            (fun i (x : HttpPostedFileBase) ->
                if x.FileName <> ""
                then
                    let path = Path.Combine(dir, x.FileName)
                    x.SaveAs path
                    try
                        match ctx.Request.Post.["documentId"], ctx.Request.Post.["pageIndex"], ctx.UserSession.GetLoggedInUser() |> Async.RunSynchronously |> Option.map Int32.Parse with
                        | Some documentIdStr, Some pageIndexStr, Some userId ->
                            let documentId = documentIdStr |> Int32.Parse
                            Server.addFilePage documentId path (pageIndexStr |> Int32.Parse) |> Async.RunSynchronously
                            Server.setLastEditedDocumentId userId documentId |> Async.RunSynchronously
                        | _, _, _ ->
                            failwith "Document id or pageIndex unknown or userId was None"
                    with
                    | e ->
                        File.Delete(path)
                        reraise()
            )
        Templating.main ctx EndPoint.About "Templates" [
            client <@ Client.templates() @>
        ]

    let main =
        Application.MultiPage (fun (ctx : Context<EndPoint>) endpoint ->
            match (ctx.UserSession.GetLoggedInUser() |> Async.RunSynchronously, endpoint) with
            | Some _, EndPoint.Home -> homePage ctx
            | Some _, EndPoint.Login -> loginPage ctx
            | Some _, EndPoint.ShowSentJobApplications -> showSentJobApplications ctx
            | Some _, EndPoint.Register -> registerPage ctx
            | None, EndPoint.Register -> registerPage ctx
            | Some _, EndPoint.About -> aboutPage ctx
            | Some _, EndPoint.ConfirmEmail -> confirmEmailPage ctx
            | Some _ , EndPoint.Templates -> templatesPage ctx
            | None, _ -> loginPage ctx
        )

module SelfHostedServer =

    open global.Owin
    open Microsoft.Owin.Hosting
    open Microsoft.Owin.StaticFiles
    open Microsoft.Owin.FileSystems
    open WebSharper.Owin
    open LetsEncrypt.Owin
    open System.Web.Http

    [<EntryPoint>]
    let main args =
        log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config")) |> ignore
        let rootDirectory, url =
            match args with
            | [| rootDirectory; url |] ->
                rootDirectory, url
            | [| url |] -> "..", url
            | [| |] -> "..", "https://localhost:9000/"
            | _ -> eprintfn "Usage: JobApplicationSpam ROOT_DIRECTORY URL"; exit 1

        use server = WebApp.Start(url, fun appB ->
            appB
                .UseAcmeChallenge()
                .UseStaticFiles(
                    StaticFileOptions(
                        FileSystem = PhysicalFileSystem(rootDirectory)))
                .UseSitelet(rootDirectory, Site.main)
            |> ignore)
        while true do
            System.Threading.Thread.Sleep(60000)
        0