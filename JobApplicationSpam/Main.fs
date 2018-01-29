namespace JobApplicationSpam

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Server
open System.IO
open Chessie.ErrorHandling
open Types

type EndPoint =
    | [<EndPoint "/ghi">] Login
    | [<EndPoint "/about">] About
    | [<EndPoint "/confirmemail">] ConfirmEmail
    | [<EndPoint "/templates">] Templates
    | [<EndPoint "/upload">] Upload
    | [<EndPoint "/abc">] Logout
    | [<EndPoint "/download">] Download of guid:string
    | [<EndPoint "/changepassword">] ChangePassword
    | [<EndPoint "/dologin">] DoLogin
    | [<EndPoint "/">] Home

module Templating =
    open WebSharper.UI.Next.Html
    open System
    open Client


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
        let oUserId = ctx.UserSession.GetLoggedInUser() |> Async.RunSynchronously |> Option.map (Int32.Parse >> UserId)
        match oUserId with
        | Some userId ->
            match Server.getEmailByUserId userId |> Async.RunSynchronously with
            | Some email -> email
            | None -> sprintf "Gast %A" userId
        | None -> "Gast"


    let atSign (ctx: Context<EndPoint>) =
        if ctx.UserSession.GetLoggedInUser()
            |> Async.RunSynchronously
            |> Option.isSome
        then "@"
        else "@"

    let btnLoginOrOut (ctx: Context<EndPoint>) : Doc =
        client <@ Client.loginOrOutButton() @>

    let main (ctx : Context<EndPoint>) (action : EndPoint) (title: string) (body: Doc list) : Async<Content<'a>>=
        Content.Page(
            MainTemplate()
                .Title(title)
                .AtSign(atSign ctx)
                .MenuBar(MenuBar ctx action)
                .Body(body)
                .BtnLoginOrOut(btnLoginOrOut ctx)
                .LoggedInUserEmail(loggedInUserEmail ctx)
                .Doc()
        )


module Site =
    open WebSharper.UI.Next.Html
    open System.Web
    open System
    open System.Configuration
    open Client
    open WebSharper.Sitelets.Content


    let homePage (ctx : Context<EndPoint>) =
        Templating.main ctx EndPoint.Home "Home" [
            h1 [text "Say Hi to the server!"]
            //div [client <@ Client.editUserValues () @>]
        ]

    let doLoginPage (ctx : Context<EndPoint>) =
        Templating.main ctx EndPoint.Home "Home" [
            client <@ Client.doLogin() @>
        ]
    
    let loginPage (ctx : Context<EndPoint>) =
        match ctx.Request.Post.["btnLogin"], ctx.Request.Post.["btnRegister"], ctx.Request.Post.["txtLoginEmail"], ctx.Request.Post.["txtLoginPassword"] with
        | Some _, None, Some email, Some password -> //login
            let loginResult = Server.login email password |> Async.RunSynchronously
            match loginResult with
            | Ok (_, _) ->
                let userId = Server.getUserIdByEmail email |> Async.RunSynchronously |> Option.get
                ctx.UserSession.LoginUser (string userId, true) |> Async.RunSynchronously
                Templating.main ctx EndPoint.Login "Login failed" [
                    div 
                      [ client <@ Client.setSessionCookie() @>
                        client <@ Client.templates() @>
                      ]
                ]
            | Bad xs ->
                Templating.main ctx EndPoint.Login "Login failed" [
                    h1 [text "Login"]
                    client <@ Client.login () @>
                    br []
                    br []
                    text (xs |> String.concat ", ")
                ]
        | None, Some _, Some email, Some password -> //register
            let registerResult = Server.register email password |> Async.RunSynchronously
            match registerResult with
            | Ok (_, _) ->
                Templating.main ctx EndPoint.Login "Login" [
                    h1 [text "Login"]
                    client <@ Client.login () @>
                    br []
                    br []
                    text "Wir haben dir eine Email geschickt."
                    br []
                    text "Bitte bestaetige deine Email-Adresse."
                ]
            | Bad xs ->
                Templating.main ctx EndPoint.Login "Registration failed" [
                    h1 [text "Login"]
                    client <@ Client.login () @>
                    br []
                    br []
                    text (xs |> String.concat ", ")
                ]
        | _ ->
            Templating.main ctx EndPoint.Login "Login" [
                client <@ Client.login () @>
            ]
    
    let aboutPage ctx =
        Templating.main ctx EndPoint.About "About" [
            h1 [text "About"]
            p [text "This is a template WebSharper client-server application."]
        ]

    let logoutPage (ctx : Context<EndPoint>) =
        ctx.UserSession.Logout() |> Async.RunSynchronously
        Templating.main ctx EndPoint.Logout "Logout" [
            client <@ Client.logout() @>
            client <@ Client.doLogin() @>
        ]
    
    let uploadPage (ctx : Context<EndPoint>) =
        match ctx.Request.Post.["documentId"] |> Option.map Int32.TryParse |> Option.map (fun (b, v) -> (b, DocumentId v))
            , ctx.UserSession.GetLoggedInUser()
                  |> Async.RunSynchronously
                  |> Option.map Int32.Parse
                  |> Option.map UserId
            , ctx.Request.Post.["pageIndex"] |> Option.map Int32.TryParse
            with
        | Some (true, documentId), Some userId, Some (true, pageIndex) ->
            let relativeDir = Path.Combine("users", userId.ToString())
            if not <| Directory.Exists (toRootedPath relativeDir) then Directory.CreateDirectory (toRootedPath relativeDir) |> ignore
            let findFreeFileName file documentId =
                let fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file)
                let extension = Path.GetExtension(file)
                let files =
                    Server.getFilePageNames documentId |> Async.RunSynchronously
                    |> List.filter (fun x -> Path.GetExtension(x) = Path.GetExtension(extension))
                    |> List.filter (fun x -> Path.GetFileNameWithoutExtension(x).StartsWith(fileNameWithoutExtension))

                let rec findFreeFileName' i =
                    let name = sprintf "%s%s%s" fileNameWithoutExtension (if i = 0 then "" else sprintf " (%i)" i) extension
                    if List.contains name files
                    then findFreeFileName' (i + 1)
                    else name
                findFreeFileName' 0

            ctx.Request.Files
            |> Seq.iter
                (fun (x : HttpPostedFileBase) ->
                    if     x.FileName <> ""
                        && x.ContentLength < maxUploadSize
                        && List.contains (Path.GetExtension(x.FileName).Substring(1)) supportedUnoconvFileTypes
                    then
                        let convertedFilePath, hasBeenConverted =
                            if List.contains (Path.GetExtension(x.FileName).Substring(1)) convertibleToOdtFormats
                            then
                                let tmpFilePath =
                                    Path.Combine(
                                          Settings.DataDirectory
                                        , "tmp"
                                        , Guid.NewGuid().ToString()
                                        , x.FileName)
                                Directory.CreateDirectory(Path.GetDirectoryName(tmpFilePath)) |> ignore
                                x.SaveAs(tmpFilePath)
                                Server.convertToOdt tmpFilePath |> Async.RunSynchronously, true
                            elif x.FileName.ToLower().EndsWith(".odt")
                            then
                                let tmpFilePath =
                                    Path.Combine
                                        ( Settings.DataDirectory
                                        , "tmp"
                                        , Guid.NewGuid().ToString()
                                        , x.FileName)
                                Directory.CreateDirectory(Path.GetDirectoryName(tmpFilePath)) |> ignore
                                x.SaveAs tmpFilePath
                                tmpFilePath, false
                            else Path.Combine(Settings.DataDirectory, "users", userId |> string, x.FileName), false

                        let (filePath, name) =
                            let filesWithSameExtension =
                                Server.getFilesWithSameExtension convertedFilePath userId
                                |> Async.RunSynchronously
                            let oSameFile =
                                filesWithSameExtension
                                |> Seq.tryFind
                                    (fun file ->
                                        if convertedFilePath.ToLower().EndsWith(".odt")
                                        then
                                            Odt.areOdtFilesEqual
                                                convertedFilePath
                                                (toRootedPath file)
                                        else
                                            use fileStream =
                                                new FileStream
                                                    ( toRootedPath file
                                                    , FileMode.Open
                                                    , FileAccess.Read)
                                            Odt.areStreamsEqual x.InputStream fileStream
                                    )
                            match oSameFile with
                            | Some file ->
                                (file, findFreeFileName convertedFilePath documentId)
                            | None ->
                                let fileName =
                                    if File.Exists(Path.Combine(toRootedPath relativeDir, convertedFilePath))
                                    then findFreeFileName (Path.GetFileName(convertedFilePath)) documentId
                                    else Path.GetFileName(convertedFilePath)
                                printfn "%s, %s" fileName convertedFilePath
                                if hasBeenConverted
                                then
                                    if File.Exists (Path.Combine(toRootedPath relativeDir, fileName))
                                    then
                                        File.Replace(convertedFilePath, Path.Combine(toRootedPath relativeDir, fileName), "")
                                    else
                                        File.Move(convertedFilePath, Path.Combine(toRootedPath relativeDir, fileName))
                                    (Path.Combine(relativeDir, fileName), fileName)
                                else
                                    x.SaveAs(Path.Combine(toRootedPath relativeDir, fileName))
                                    (Path.Combine(relativeDir, fileName), fileName)
                        async {
                            do! Server.addFilePage documentId filePath pageIndex name
                            do! Server.setLastEditedDocumentId userId documentId
                        } |> Async.RunSynchronously
                )
        | _ -> ()
        Content.RedirectPermanentToUrl "/"

    let templatesPage (ctx : Context<EndPoint>) =
        Templating.main ctx EndPoint.Templates "Bewerbungsspam" [
            client <@ Client.templates() @>
        ]

    let changePasswordPage (ctx : Context<EndPoint>) =
        Templating.main ctx EndPoint.Templates "Bewerbungsspam" [
            div [client <@ Client.changePassword () @>]
        ]
    
    let confirmEmailPage (ctx : Context<EndPoint>) =
        async {
            try
                match ctx.Request.Get.["email"], ctx.Request.Get.["guid"] with
                | Some email, Some guid->
                    match Server.confirmEmail email guid |> Async.RunSynchronously with
                    | Ok _ -> 
                        let! oUserId = Server.getUserIdByEmail email
                        match oUserId with
                        | Some userId ->
                            do! ctx.UserSession.LoginUser (userId |> string)
                            return changePasswordPage ctx
                        | None ->
                            return loginPage ctx
                    | Bad vs -> return loginPage ctx
                | Some _, None
                | None, Some _
                | None, None ->
                    return loginPage ctx
            with
            | e ->
                return loginPage ctx
        } |> Async.RunSynchronously


    let downloadPage (ctx : Context<EndPoint>) (linkGuid : string) =
        async {
            let! oFilePathAndName = Server.tryGetPathAndNameByLinkGuid linkGuid
            match oFilePathAndName with
            | None -> return Content.NotFound
            | Some (filePath, name) ->
                let file = FileInfo(Path.Combine(Settings.DataDirectory, filePath))
                return
                    if file.Exists then
                        Content.File(file.FullName, true, "application/pdf")
                        |> Content.MapResponse (fun response ->
                            { response with
                                Headers = Seq.append response.Headers 
                                    [Http.Header.Custom "Content-Disposition" ("attachment; filename=" + name)]
                            }
                        )
                    else Content.NotFound
        } |> Async.RunSynchronously

    let main =
        Application.MultiPage (fun (ctx : Context<EndPoint>) endpoint ->
            match (ctx.UserSession.GetLoggedInUser() |> Async.RunSynchronously, endpoint) with
            | (_ , EndPoint.Logout) -> logoutPage ctx
            | None, EndPoint.DoLogin -> doLoginPage ctx
            | None , EndPoint.Home ->
                doLoginPage ctx
            | None , EndPoint.Templates ->
                doLoginPage ctx
            | Some _, EndPoint.Home -> templatesPage ctx
            | Some _, EndPoint.Upload -> uploadPage ctx
            | Some _, EndPoint.ConfirmEmail -> confirmEmailPage ctx
            | None, EndPoint.ConfirmEmail -> confirmEmailPage ctx
            | Some _ , EndPoint.Templates -> templatesPage ctx
            | Some _ , EndPoint.ChangePassword -> changePasswordPage ctx
            | None , EndPoint.ChangePassword -> changePasswordPage ctx
            | _ , EndPoint.Login -> loginPage ctx
            | Some _ , EndPoint.Download guid ->
                downloadPage ctx guid
            | a, b ->
                templatesPage ctx
        )
    
    let redirectHttpToHttps =
        Application.MultiPage (fun (ctx : Context<EndPoint>) endpoint ->
            Content.RedirectPermanentToUrl "https://www.bewerbungsspam.de"
        )

module SelfHostedServer =
    open global.Owin
    open Microsoft.Owin.Hosting
    open Microsoft.Owin.StaticFiles
    open Microsoft.Owin.FileSystems
    open WebSharper.Owin
    open LetsEncrypt.Owin
    open log4net
    open JobApplicationSpam
    open System.IO

    [<EntryPoint>]
    let main args =
        log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config")) |> ignore

        let rootDirectory, url, port =
            match args with
            | [| rootDirectory; url; port |] ->
                rootDirectory, url, port
            | [| url; port |] -> "..", url, port
            | [| |] -> "..", "http://localhost", "9000"
            | _ -> eprintfn "Usage: JobApplicationSpam ROOT_DIRECTORY URL"; exit 1

        use server = WebApp.Start(url + ":" + port, fun appB ->
            appB
                .UseAcmeChallenge()
                .UseStaticFiles(
                    StaticFileOptions(
                        FileSystem = PhysicalFileSystem(rootDirectory)))
                .UseSitelet(rootDirectory, Site.main)
            |> ignore)
        
        use server1 =
            if url.StartsWith("localhost") || url.StartsWith("http://localhost")
            then
                WebApp.Start(url + ":" + "9001", fun appB ->
                appB
                    .UseStaticFiles(
                        StaticFileOptions(
                            FileSystem = PhysicalFileSystem(rootDirectory)))
                    .UseSitelet(rootDirectory, Site.redirectHttpToHttps)
                |> ignore)
            else
                let url = System.Text.RegularExpressions.Regex.Replace(url, "https://", "http://")
                WebApp.Start(url + ":" + "80", fun appB ->
                appB
                    .UseStaticFiles(
                        StaticFileOptions(
                            FileSystem = PhysicalFileSystem(rootDirectory)))
                    .UseSitelet(rootDirectory, Site.redirectHttpToHttps)
                |> ignore)
        while true do
            System.Threading.Thread.Sleep(60000)
        0


