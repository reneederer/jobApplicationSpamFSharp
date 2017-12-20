namespace JobApplicationSpam

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Server
open System.IO

type EndPoint =
    | [<EndPoint "/">] Home
    | [<EndPoint "/login">] Login
    | [<EndPoint "/register">] Register
    | [<EndPoint "/edituservalues">] EditUserValues
    | [<EndPoint "/uploadtemplate">] UploadTemplate
    | [<EndPoint "/addEmployer">] AddEmployer
    | [<EndPoint "/applynow">] ApplyNow
    | [<EndPoint "/showsentjobapplications">] ShowSentJobApplications
    | [<EndPoint "/about">] About
    | [<EndPoint "/confirmEmail">] ConfirmEmail

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
            li ["Register" => EndPoint.Register]
            li ["Login" => EndPoint.Login ]
            li ["About" => EndPoint.About]
            li ["ShowSentJobApplications" => EndPoint.ShowSentJobApplications]
        ]
    let SideBar (ctx: Context<EndPoint>) endpoint : Doc list =
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

    let main (ctx : Context<EndPoint>) (action : EndPoint) (title: string) (body: Doc list) : Async<Content<'a>>=
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
                Server.uploadTemplate
                    (Option.get ctx.Request.Post.["templateName"])
                    (Option.get ctx.Request.Post.["userAppliesAs"])
                    (Option.get ctx.Request.Post.["emailSubject"])
                    (Option.get ctx.Request.Post.["emailBody"])
                    (ctx.Request.Files |> Seq.choose (fun (x : HttpPostedFileBase) -> if x.FileName <> "" then Some x.FileName else None))
                    (ctx.UserSession.GetLoggedInUser () |> Async.RunSynchronously |> Option.map Int32.Parse)
                |> Async.RunSynchronously
            else ok "Nothing to upload"
         (*
      (if not (ctx.Request.Files |> Seq.isEmpty) then text "Files have been uploaded" else text "")
      myText
      *)
        Templating.main ctx EndPoint.UploadTemplate "UploadTemplate"
          [ div
              [ client <@ Client.uploadTemplate() @>
                h1 [text (sprintf "%A" uploadResult)]
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
                match Server.confirmEmail email guid with
                | Ok _ -> "Great! Your email has been confirmed."
                | Bad vs -> "Email confirmation has failed."
            | Some _, None
            | None, Some _
            | None, None ->
                "Confirmation requires email and guid to be set."
        Templating.main ctx EndPoint.About "About" [
            h1 [text message]
        ]


    let main =
        Application.MultiPage (fun (ctx : Context<EndPoint>) endpoint ->
            match (ctx.UserSession.GetLoggedInUser() |> Async.RunSynchronously, endpoint) with
            | Some _, EndPoint.Home -> homePage ctx
            | Some _, EndPoint.Login -> loginPage ctx
            | Some _, EndPoint.EditUserValues -> loginPage ctx
            | Some _, EndPoint.UploadTemplate -> uploadTemplatePage ctx
            | Some _, EndPoint.AddEmployer -> addEmployerPage ctx
            | Some _, EndPoint.ApplyNow -> applyNowPage ctx
            | Some _, EndPoint.ShowSentJobApplications -> showSentJobApplications ctx
            | Some _, EndPoint.Register -> registerPage ctx
            | None, EndPoint.Register -> registerPage ctx
            | Some _, EndPoint.About -> aboutPage ctx
            | Some _, EndPoint.ConfirmEmail -> confirmEmailPage ctx
            | None, _ -> loginPage ctx
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