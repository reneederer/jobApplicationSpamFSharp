namespace JobApplicationSpam

open WebSharper
open Npgsql
open Chessie.ErrorHandling
open System
open JobApplicationSpam.Types
open System.Configuration
open Website
open System.Security.Cryptography
open System.Text.RegularExpressions
open JobApplicationSpam.I18n
open Variables
open System.Data
open DBTypes
open FSharp.Data.Sql.Transactions
open System.Linq


module Server =

    open System.Net.Mail
    open System.IO
    open WebSharper.Web.Remoting
    open System.Transactions
    open WebSharper.Owin.EnvKey.WebSharper

    let log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().GetType())

    [<Remote>]
    let getCurrentUserId () =
        try
            GetContext().UserSession.GetLoggedInUser()
            |> Async.map (
                 Option.map (Int32.TryParse)
                 >> Option.bind (fun (parsed, v) -> if parsed then Some v else None)
            )
        with
        | e ->
            log.Error("", e)
            Async.singleton None


    let withCurrentUserFail () =
        getCurrentUserId() |> Async.RunSynchronously

    let withCurrentUser() =
        getCurrentUserId() |> Async.RunSynchronously
        |> fun x -> if x.IsNone then failwith "Nobody logged in!" else UserId x.Value

    [<Remote>]
    let predefinedVariables =
        Async.singleton
            [ ( "employer"
                , [ "$firma"
                    "$firmaStrasse"
                    "$firmaPlz"
                    "$firmaStadt"
                    "$chefGeschlecht"
                    "$chefTitel"
                    "$chefVorname"
                    "$chefNachname"
                    "$chefEmail"
                    "$chefTelefon"
                    "$chefMobil"
                  ]
              );
              ( "yourValues"
              , [ "$meinGeschlecht"
                  "$meinTitel"
                  "$meinVorname"
                  "$meinNachname"
                  "$meineStrasse"
                  "$meinePlz"
                  "$meinePostleitzahl"
                  "$meineStadt"
                  "$meineEmail"
                  "$meineTelefonnr"
                  "$meineMobilnr"
                ]
              );
              ( "date"
              , [ "$tagHeute"
                  "$monatHeute"
                  "$jahrHeute"
                ]
              );
              ( "others"
              , [ "$beruf" ]
              )
            ]

    [<Remote>]
    let toCV (employer : Employer) (userValues : UserValues) (userEmail : string) (jobName : string) (customVariablesStr : string) =
        async {
            let predefinedVariables =
                [ ("$firma", employer.company)
                  ("$firmaStrasse", employer.street)
                  ("$firmaPlz", employer.postcode)
                  ("$firmaStadt",employer.city)
                  ("$chefGeschlecht", employer.gender.ToString())
                  ("$chefTitel", employer.degree)
                  ("$chefVorname", employer.firstName)
                  ("$chefNachname", employer.lastName)
                  ("$chefEmail", employer.email)
                  ("$chefTelefon", employer.phone)
                  ("$chefMobil", employer.mobilePhone)

                  ("$meinGeschlecht", userValues.gender.ToString())
                  ("$meinTitel", userValues.degree)
                  ("$meinVorname", userValues.firstName)
                  ("$meinNachname", userValues.lastName)
                  ("$meineStrasse", userValues.street)
                  ("$meinePlz", userValues.postcode)
                  ("$meinePostleitzahl", userValues.postcode)
                  ("$meineStadt", userValues.city)
                  ("$meineEmail", userEmail)
                  ("$meineTelefonnr", userValues.phone)
                  ("$meineMobilnr", userValues.mobilePhone)
                  ("$tagHeute", sprintf "%02i" DateTime.Today.Day)
                  ("$monatHeute", sprintf "%02i" DateTime.Today.Month)
                  ("$jahrHeute", sprintf "%04i" DateTime.Today.Year)
                  ("$beruf", jobName)
                ]
            let customVariables =
                match parse customVariablesStr with
                | Bad xs -> failwith (String.Concat xs)
                | Ok (parsedVariables, _) ->
                    parsedVariables
                    |> List.map (fun (k : AssignedVariable, v : Variables.Expression) -> 
                              (k
                            , (tryGetValue v predefinedVariables |> Option.defaultValue ""))
                        )

            return (predefinedVariables @ customVariables) |> List.sortByDescending (fun (k, v) -> k.Length)
        }


    [<Remote>]
    let tryGetUserIdByEmail (email : string) =
        async {
            return Database.tryGetUserIdByEmail email |> readDB
        }

    [<Remote>]
    let getEmailByUserId (userId : UserId) =
        async {
            return Database.getEmailByUserId userId |> readDB
        }
    
    [<Remote>]
    let tryGetUserEmail () : Async<option<string>> =
         withCurrentUserFail ()
         |> fun oUserId ->
            async {
                return
                    oUserId
                    |> Option.bind
                        (fun x ->
                             Database.getEmailByUserId (UserId x)
                             |> readDB
                        )
            }
    
    let sendEmail fromAddress fromName toAddress subject body (attachmentPathsAndNames : list<string * string>) =
        use smtpClient = new SmtpClient(Settings.EmailServer, Settings.EmailPort)
        smtpClient.EnableSsl <- true
        smtpClient.Credentials <- new System.Net.NetworkCredential(Settings.EmailUsername, Settings.EmailPassword)
        let fromAddress = new MailAddress(fromAddress, fromName, System.Text.Encoding.UTF8)
        let toAddress = new MailAddress(toAddress)
        let message = new MailMessage(fromAddress, toAddress, SubjectEncoding = System.Text.Encoding.UTF8, Subject = subject, Body = body, BodyEncoding = System.Text.Encoding.UTF8)
        let attachments =
            attachmentPathsAndNames
            |> List.map (fun (filePath, fileName) ->
                let attachment = new Attachment(filePath)
                attachment.Name <- fileName
                message.Attachments.Add(attachment)
                attachment)
        smtpClient.Send(message)
        for attachment in attachments do
            attachment.Dispose()
    
    [<Remote>]
    let getUserValues () =
        let userId = withCurrentUser()
        async {
            return
                userId
                |> Database.getUserValues
                |> readDB
        }
    
    [<Remote>]
    let setUserValues (userValues : UserValues) =
        let userId = withCurrentUser ()
        async {
            return
                userId
                |> Database.setUserValues userValues
                |> withTransaction
        }

    let generateSalt length =
        let (bytes : array<byte>) = Array.replicate length (0uy)
        use rng = new RNGCryptoServiceProvider()
        rng.GetBytes(bytes)
        bytes |> Convert.ToBase64String

    let generateHashWithSalt (password : string) (salt : string) iterations length =
        use deriveBytes = new Rfc2898DeriveBytes(password |> System.Text.Encoding.UTF8.GetBytes, salt |> Convert.FromBase64String, iterations)
        deriveBytes.GetBytes(length) |> Convert.ToBase64String

    let generateHash (word : string) =
        use sha256Managed = new SHA256Managed()
        let bytes = sha256Managed.ComputeHash(word |> System.Text.Encoding.UTF8.GetBytes)
        bytes |> Convert.ToBase64String

    [<Remote>]
    let setPassword password =
        let salt =  generateSalt 64
        let hashedPassword = generateHashWithSalt password salt 1000 64
        let userId = withCurrentUser()
        async {
            return
                userId
                |> Database.setPasswordSaltAndConfirmEmailGuid hashedPassword salt None
                |> withTransaction
                }

    [<Remote>]
    let login (email : string) (password : string) =
        async {
            log.Debug (sprintf "email = %s, password = password")
            match Database.tryGetValidateLoginData email |> readDB with
            | Some v ->
                match v.confirmEmailGuid with
                | None ->
                    let hash = generateHashWithSalt password v.salt 1000 64
                    if hash = v.hashedPassword
                    then
                        (Database.insertLastLogin DateTime.Now v.userId) |> withTransaction
                        return ok ()
                    else return fail "Email oder Passwort ist falsch."
                | Some _ -> return fail "Bitte bestätige deine Email-Adresse."
            | None -> return fail  "Email oder Passwort ist falsch."
        }
    
    [<Remote>]
    let setUserEmail (email : string) =
        if email = "geprueftundokund nicht doppelt" || true //TODO
        then
            try
                withCurrentUser ()
                |> (fun userId dbContext ->
                    Database.setUserEmail (Some email) userId dbContext |> ignore
                    Database.setConfirmEmailGuid email (Some <| Guid.NewGuid().ToString("N")) dbContext |> ignore
                   )
                |> withTransaction
                async { return ok () }
            with
            | e -> async { return fail "Email already registered" }
        else
            async { return fail "Email is not valid" }
    
    [<Remote>]
    let loginUserBySessionGuid sessionGuid =
        log.Debug(sprintf "(sessionGuid = %s)" sessionGuid)
        match Database.tryGetUserIdBySessionGuid sessionGuid |> readDB with
        | None ->
            log.Debug(sprintf "(sessionGuid = %s) = %b" sessionGuid false)
            async { return false }
        | Some (UserId userId) ->
            GetContext().UserSession.LoginUser (userId |> string) |> Async.RunSynchronously
            log.Debug(sprintf "(sessionGuid = %s) = true" sessionGuid)
            async { return true }
    
    [<Remote>]
    let setSessionGuid oSessionGuid =
        let userId = withCurrentUser ()
        async {
            return
                userId
                |> Database.setSessionGuid oSessionGuid
                |> withTransaction
                }
    
    [<Remote>]
    let loginAsGuest (sessionGuid : string) =
        log.Debug (sprintf "(sessionGuid = %s)" sessionGuid)
        use dbScope = new TransactionScope(TransactionScopeOption.RequiresNew)
        let dbContext = DB.GetDataContext()
        let user = Database.insertUser None "" "" None (Some sessionGuid) DateTime.Now dbContext
        dbContext.SubmitUpdates()
        Database.insertLastLogin DateTime.Now (UserId user.Id) dbContext
        Database.insertUserValues emptyUserValues (UserId user.Id) dbContext
        dbContext.SubmitUpdates()
        dbScope.Complete()
        GetContext().UserSession.LoginUser (string user.Id)

    [<Remote>]
    let isUserLoggedIn () =
        let loggedInUser = (GetContext().UserSession.GetLoggedInUser() |> Async.RunSynchronously)
        async {
            return loggedInUser.IsSome
        }
    
    [<Remote>]
    let isLoggedInAsGuest () =
        let oUserId = withCurrentUserFail ()
        async {
            return
                oUserId
                |> Option.map (fun userId ->
                        Database.tryGetConfirmEmailGuidByUserId (UserId userId)
                        |> readDB
                        |> Option.isNone)
                |> Option.defaultValue false
        }

    [<Remote>]
    let register (email : string) (password : string) =
        async {
            try
                log.Debug (sprintf "(email = %s, password = %s)" email password)
                let emailRegexStr = """^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$"""
                if not <| Regex.IsMatch(email, emailRegexStr)
                then return fail "Email-Adresse scheint unzulässig zu sein."
                elif password = ""
                then return fail "Passwort darf nicht leer sein."
                else
                    match Database.userEmailExists email |> readDB with
                    | true ->
                        return fail "Diese Email-Adresse ist schon registriert."
                    | false ->
                        let salt = generateSalt 64
                        let hashedPassword = generateHashWithSalt password salt 1000 64
                        let confirmEmailGuid = Guid.NewGuid().ToString("N")
                        Database.insertUser (Some email) hashedPassword salt (Some confirmEmailGuid) None System.DateTime.Now
                        |> andThen (fun user dbContext ->
                            Database.insertUserValues emptyUserValues (UserId user.Id) dbContext
                            Database.insertLastLogin DateTime.Now (UserId user.Id) dbContext)
                        |> withTransaction
                        sendEmail
                            Settings.EmailUsername
                            Settings.DomainName
                            email
                            (t German PleaseConfirmYourEmailAddressEmailSubject)
                            (String.Format(t German PleaseConfirmYourEmailAddressEmailBody, email, confirmEmailGuid))
                            []
                        return ok "Please confirm your email"
            with
            | e ->
                log.Error("", e)
                return fail "An error occured."
        }

    [<Remote>]
    let confirmEmail email confirmEmailGuid =
        async {
            return
                (fun dbContext ->
                    log.Debug (sprintf "(email = %s, guid = %s)" email confirmEmailGuid)
                    match Database.tryGetConfirmEmailGuid email |> readDB with
                    | None -> ok "Email already confirmed"
                    | Some guid when guid = confirmEmailGuid ->
                        Database.setConfirmEmailGuid email None dbContext 
                        ok "Email has been confirmed."
                    | Some _ ->
                        fail "Unknown confirmEmailGuid")
                |> withTransaction
        }

    [<Remote>]
    let getDocumentNames () =
        let userId = withCurrentUser ()
        async {
            return
                userId
                |> Database.getDocumentNames
                |> readDB
                }

    [<Remote>]
    let overwriteDocument (document : Document) =
        let userId = withCurrentUser ()
        async {
            return
                userId
                |> Database.overwriteDocument document
                |> withTransaction
                }

    [<Remote>]
    let saveNewDocument (document : Document) =
        let userId = withCurrentUser ()
        async {
            return
                userId
                |> Database.insertDocument document
                |> withTransaction
                }

    [<Remote>]
    let deleteDocument (DocumentId documentId) =
        (fun dbContext ->
            async {
                return Database.deleteDocument documentId dbContext
                let filePaths = Database.getDeletableFilePaths documentId dbContext
                for filePath in filePaths do
                    if Path.IsPathRooted filePath
                    then File.Delete filePath
                    else File.Delete <| toRootedPath filePath
                Database.deleteDeletableDocumentFilePages documentId dbContext
            }
        ) |> withTransaction

    [<Remote>]
    let getDocumentOffset (htmlJobApplicationOffset : int) =
        let userId = withCurrentUser ()
        async {
            return
                userId
                |> Database.getDocumentOffset htmlJobApplicationOffset
                |> readDB
                }

    [<Remote>]
    let replaceVariables
            (filePath : string)
            (userValues : UserValues)
            (employer : Employer)
            (document : Document) =
        let oUserEmail = tryGetUserEmail () |> Async.RunSynchronously
        match oUserEmail with
        | Some userEmail ->
            async {
                try
                    let tmpDirectory = Path.Combine(Settings.DataDirectory, "tmp", Guid.NewGuid().ToString("N"))
                    let! map = toCV employer userValues userEmail document.jobName document.customVariables
                    Directory.CreateDirectory(tmpDirectory) |> ignore
                    if filePath.ToLower().EndsWith(".odt")
                    then
                        return
                            Odt.replaceInOdt
                                (toRootedPath filePath)
                                (Path.Combine(tmpDirectory, "extracted"))
                                (Path.Combine(tmpDirectory, "replaced"))
                                map
                    elif unoconvImageTypes |> List.contains (Path.GetExtension(filePath).ToLower().Substring(1))
                    then
                        return filePath
                    else
                        let newFilePath = Path.Combine(tmpDirectory, (Path.GetFileName(filePath)))
                        File.Copy(toRootedPath filePath, newFilePath, true)
                        Odt.replaceInFile newFilePath map Types.Ignore
                        return newFilePath
                with
                | e ->
                    log.Error ("", e)
                    return failwith "An error occurred"
            }
        | None ->
            log.Error("User email was None")
            failwith "User email was None"
         
    let emailSentApplicationToUser' (sentApplicationOffset : int) (customVariablesString : string) userId =
        try
            let oUserEmail = Database.getEmailByUserId userId |> readDB
            let oSentApplication =
                withCurrentUser ()
                |> Database.getSentApplicationOffset sentApplicationOffset
                |> readDB
            match oUserEmail, oSentApplication with
            | _, None -> fail "The requested application could not be not found"
            | None, _ -> fail "User email was None"
            | Some userEmail, Some sentApplication ->
                let myList =
                    toCV
                        sentApplication.employer
                        sentApplication.user.values
                        sentApplication.user.email
                        sentApplication.jobName
                        customVariablesString
                    |> Async.RunSynchronously
                let tmpDirectory = Path.Combine(Settings.DataDirectory, "tmp", Guid.NewGuid().ToString("N"))
                let odtPaths =
                    [ for (path, _) in sentApplication.filePages do
                            yield
                                if unoconvImageTypes |> List.contains(Path.GetExtension(path).Substring(1).ToLower())
                                then toRootedPath path
                                elif path.ToLower().EndsWith(".odt")
                                then
                                    let directoryGuid = Guid.NewGuid().ToString("N")
                                    Odt.replaceInOdt
                                        (toRootedPath path)
                                        (Path.Combine(tmpDirectory, directoryGuid, "extractedOdt"))
                                        (Path.Combine(tmpDirectory, directoryGuid, "replacedOdt"))
                                        myList
                                else
                                    let copiedPath = Path.Combine(tmpDirectory, Guid.NewGuid().ToString("N"), Path.GetFileName(path))
                                    Directory.CreateDirectory(Path.GetDirectoryName copiedPath) |> ignore
                                    File.Copy(toRootedPath path, copiedPath)
                                    Odt.replaceInFile
                                        copiedPath
                                        myList
                                        Types.Ignore
                                    copiedPath
                    ]

                let pdfPaths =
                    [ for odtPath in odtPaths do
                        yield Odt.odtToPdf odtPath
                    ] |> List.choose id
                let mergedPdfPath =
                    Path.Combine(
                        tmpDirectory,
                        (sprintf
                            "Bewerbung_%s_%s.pdf"
                            sentApplication.user.values.firstName
                            sentApplication.user.values.lastName
                        ).Replace("_.", ".").Replace("_.", "."))
                if pdfPaths <> [] then Odt.mergePdfs pdfPaths mergedPdfPath
                sendEmail
                    Settings.EmailUsername
                    "www.bewerbungsspam.de"
                    userEmail
                    (Odt.replaceInString sentApplication.email.subject myList Types.Ignore)
                    (Odt.replaceInString (sentApplication.email.body.Replace("\\r\\n", "\n").Replace("\\n", "\n")) myList Types.Ignore)
                    (if pdfPaths = []
                     then []
                     else [mergedPdfPath,
                           (sprintf
                                "Bewerbung_%s_%s.pdf"
                                sentApplication.user.values.firstName
                                sentApplication.user.values.lastName
                           ).Replace("_.", ".").Replace("_.", ".")]
                     )
                ok ()
        with
        | e ->
            log.Error ("", e)
            fail "Couldn't email the application to the user"

    [<Remote>]
    let emailSentApplicationToUser (sentApplicationOffset : int) (customVariablesString : string) =
        let userId = withCurrentUser ()
        async {
            return
                userId
                |> fun userId () -> emailSentApplicationToUser' sentApplicationOffset customVariablesString userId
                |> fun f -> f()
        }
    [<Remote>]
    let sendNotYetSentApplication sentApplicationId =
        try
            let oSentApp = Database.getSentApplication sentApplicationId |> readDB
            match oSentApp with
            | None ->
                log.Error (sprintf "sentApplication was None. Id: %i" sentApplicationId)
            | Some sentApp ->
                let myList =
                        toCV
                            sentApp.employer
                            sentApp.user.values
                            sentApp.user.email
                            sentApp.jobName
                            sentApp.customVariables
                        |> Async.RunSynchronously
                let tmpDirectory = Path.Combine(Settings.DataDirectory, "tmp", Guid.NewGuid().ToString("N"))
                let odtPaths =
                    [ for (filePath, pageIndex) in sentApp.filePages do
                        yield
                            if unoconvImageTypes |> List.contains(Path.GetExtension(filePath).ToLower().Substring(1))
                            then
                                toRootedPath filePath
                            elif filePath.ToLower().EndsWith(".odt")
                            then
                                let directoryGuid = Guid.NewGuid().ToString("N")
                                Odt.replaceInOdt
                                    (toRootedPath filePath)
                                    (Path.Combine(tmpDirectory, directoryGuid, "extractedOdt"))
                                    (Path.Combine(tmpDirectory, directoryGuid, "replacedOdt"))
                                    myList
                            else
                                let copiedPath = Path.Combine(tmpDirectory, Guid.NewGuid().ToString("N"), Path.GetFileName(filePath))
                                Directory.CreateDirectory(Path.GetDirectoryName copiedPath) |> ignore
                                File.Copy(toRootedPath filePath, copiedPath)
                                Odt.replaceInFile
                                    copiedPath
                                    myList
                                    Types.Ignore
                                copiedPath
                    ]
                       

                let pdfPaths =
                    [ for odtPath in odtPaths do
                        yield
                            Odt.odtToPdf odtPath
                    ] |> List.choose id
                let mergedPdfPath = Path.Combine(tmpDirectory, Guid.NewGuid().ToString() + ".pdf")
                if pdfPaths <> []
                then Odt.mergePdfs pdfPaths mergedPdfPath


                //TODO
                (fun (dbContext : DB.dataContext) ->
                    dbContext.Public.Sentstatus.Create(Sentapplicationid = sentApplicationId, Statuschangedon = DateTime.Today, Dueon = None, 
                        Sentstatusvalueid = 2, Statusmessage = ""))
                |> withTransaction
                |> ignore

                sendEmail
                    sentApp.user.email
                    (sentApp.user.values.firstName + " " + sentApp.user.values.lastName)
                    sentApp.employer.email
                    (Odt.replaceInString sentApp.email.subject myList Types.Ignore)
                    (Odt.replaceInString (sentApp.email.body.Replace("\\r\\n", "\n").Replace("\\n", "\n")) myList Types.Ignore)

                    (if pdfPaths = []
                     then []
                     else [mergedPdfPath, (sprintf "Bewerbung_%s_%s.pdf" sentApp.user.values.firstName sentApp.user.values.lastName)]
                     )
                sendEmail
                    sentApp.user.email
                    (sentApp.user.values.firstName + " " + sentApp.user.values.lastName)
                    sentApp.user.email
                    ("Deine Bewerbung wurde versandt - " + Odt.replaceInString sentApp.email.subject myList Types.Ignore)
                    ((sprintf
                        "Deine Bewerbung wurde am %s an %s versandt.\n\n"
                        (DateTime.Now.ToShortDateString())
                        sentApp.employer.email)
                            + Odt.replaceInString (sentApp.email.body.Replace("\\r\\n", "\n").Replace("\\n", "\n")) myList Types.Ignore)

                    (if pdfPaths = []
                     then []
                     else [mergedPdfPath, (sprintf "Bewerbung_%s_%s.pdf" sentApp.user.values.firstName sentApp.user.values.lastName)]
                     )

                let oConfirmEmailGuid = Database.tryGetConfirmEmailGuid sentApp.user.email |> readDB
                match oConfirmEmailGuid with
                | Some confirmEmailGuid ->
                    sendEmail
                        Settings.EmailUsername
                        "Bewerbungsspam"
                        sentApp.user.email
                        (t German PleaseConfirmYourEmailAddressEmailSubject)
                        (String.Format((t German PleaseConfirmYourEmailAddressEmailBody), sentApp.user.email, confirmEmailGuid))
                        []
                | None -> ()
        with
        | e ->
            log.Error ("", e)



    [<Remote>]
    let applyNow
            (employer : Employer)
            (document : Document)
            (userValues : UserValues)
            (url : string) =
        let userId = withCurrentUser()
        async {
            return
                userId
                |>
                fun userId dbContext ->
                    Database.setUserValues userValues userId dbContext |> ignore
                    dbContext.SubmitUpdates()
                    let oUserEmail = Database.getEmailByUserId userId dbContext
                    match oUserEmail with
                    | None ->
                        log.Error("UserEmail was None")
                        fail "User email was None"
                    | Some userEmail ->
                        let dbEmployer = Database.insertEmployer employer userId dbContext
                        dbContext.SubmitUpdates()
                        Database.insertNotYetSentApplication
                            dbEmployer.Id
                            document.email
                            (userEmail, userValues)
                            (document.pages |> List.choose(fun x -> match x with FilePage p -> Some (p.path, p.pageIndex) | HtmlPage _ -> None))
                            document.jobName
                            url
                            document.customVariables
                            DateTime.Today
                            userId
                            dbContext
                        dbContext.SubmitUpdates()
                        ok ()
                |> withTransaction
        }

    [<Remote>]
    let addFilePage (DocumentId documentId) path pageIndex name =
        async {
            return
                Database.insertFilePage documentId path pageIndex name |> withTransaction
        }

    
    [<Remote>]
    let getHtmlPageTemplates () =
        async {
            return Database.getHtmlPageTemplates |> readDB
        }
    
    [<Remote>]
    let getHtmlPageTemplate (templateId : int) =
        async {
            return Database.getHtmlPageTemplate templateId |> readDB
        }
    
    [<Remote>]
    let getHtmlPages documentId =
        async {
            Database.getHtmlPages documentId |> readDB
        }

    [<Remote>]
    let getPageMapOffset (pageIndex : int) (documentIndex : int)  =
        let userId = withCurrentUser  ()
        async {
            return
                userId
                |> Database.getPageMapOffset pageIndex documentIndex
                |> readDB
        }
     
    [<Remote>]
    let getLastEditedDocumentOffset () =
        let userId = withCurrentUser ()
        async {
            return
                userId
                |> Database.getLastEditedDocumentOffset
                |> readDB
        }

    [<Remote>]
    let setLastEditedDocumentId (userId : UserId) (documentId : DocumentId) =
        async {
            return Database.setLastEditedDocumentId documentId userId |> withTransaction
        }

    [<Remote>]
    let addNewDocument name =
        let userId = withCurrentUser  ()
        async {
            return
                userId
                |> Database.insertDocument name
                |> withTransaction
            }

    [<Remote>]
    let addHtmlPage (documentId : int) (oTemplateId : option<int>) (pageIndex : int) (name : string) =
        async {
            Database.insertHtmlPage documentId oTemplateId pageIndex name |> withTransaction
        }

    [<Remote>]
    let getDocumentIdOffset documentIndex =
        let userId = withCurrentUser ()
        async {
            return
                userId
                |> Database.tryGetDocumentIdOffset documentIndex
                |> readDB
        }
    
    [<Remote>]
    let readWebsite (identifier : string) : Async<Result<Employer, string>> =
        async {
            return Website.read identifier
        }
    
    [<Remote>]
    let isLoggedIn() =
        let loggedIn = getCurrentUserId() |> Async.RunSynchronously |> Option.isSome
        async { return loggedIn }
    
    [<Remote>]
    let createLink filePath name =
        async {
            let linkGuid = Guid.NewGuid().ToString("N")
            return 
                Database.insertLink filePath name linkGuid
                |> withTransaction
                |> (fun _ -> linkGuid)
        }

    [<Remote>]
    let tryGetPathAndNameByLinkGuid linkGuid =
        async {
            return Database.tryGetPathAndNameByLinkGuid linkGuid |> readDB
        }

    [<Remote>]
    let deleteLink linkGuid =
        async {
            Database.deleteLink linkGuid |> withTransaction
        }

    [<Remote>]
    let getSentApplications () =
        let userId = withCurrentUser ()
        async {
            return
                userId
                |> Database.getSentApplications
                |> readDB
        }

    let getFilesWithExtension extension userId =
        async {
            return Database.getFilesWithExtension extension userId |> readDB
        }

    [<Remote>]
    let getNotYetSentApplicationIds () = 
        async {
            return Database.getNotYetSentApplicationIds |> readDB
        }

    let getFilePageNames (documentId : DocumentId) =
        async {
            return Database.getFilePageNames documentId |> readDB
        }

    [<Remote>]
    let tryFindSentApplication (employer : Employer) =
        let userId = withCurrentUser ()
        async {
            return
                userId
                |> Database.tryGetSentApplication employer
                |> readDB
        }

    
