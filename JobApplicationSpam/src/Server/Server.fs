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


module Server =
    open System.Net.Mail
    open System.IO
    open WebSharper.Web.Remoting
    open System.Transactions

    let log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().GetType())

    [<Remote>]
    let getCurrentUserId () =
        GetContext().UserSession.GetLoggedInUser()
        |> Async.map (
             Option.map (Int32.TryParse)
             >> Option.bind (fun (parsed, v) -> if parsed then Some v else None)
        )

    let withCurrentUserFail (f : UserId -> 'a) (ifNoneFun) =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with
            | Some userId ->
                    return f (UserId userId)
            | None -> return ifNoneFun()
        }

    let withCurrentUser f  =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        match oUserId with
        | Some userId ->
            async { return fun () -> f (UserId userId) }
        | None -> failwith "Nobody is logged in"

    let doWithCurrentUser (f : UserId -> 'a)  =
        f |> withCurrentUser |> (Async.map (fun g -> g()))
    
    let readDb f =
        let r = f()
        r
    let writeToDb f =
        use dbScope = new TransactionScope()
        f()
        dbScope.Complete()
    let withTransaction f =
        Async.map writeToDb f
    
    
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
                    |> List.map (fun (k : AssignedVariable, v : Expression) -> 
                              (k
                            , (tryGetValue v predefinedVariables |> Option.defaultValue ""))
                        )

            return (predefinedVariables @ customVariables) |> List.sortByDescending (fun (k, v) -> k.Length)
        }


    [<Remote>]
    let getUserIdByEmail (email : string) =
        async {
            return Database.getUserIdByEmail db email
        }

    [<Remote>]
    let getEmailByUserId (userId : UserId) : Async<option<string>> =
        async {
            return Database.getEmailByUserId db userId
        }
    
    let getUserEmail' userId =
        Database.getEmailByUserId db userId
    [<Remote>]
    let getUserEmail () : Async<option<string>> =
        withCurrentUserFail getUserEmail' (fun () -> None)


    
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
        Database.getUserValues db |> doWithCurrentUser
    
    [<Remote>]
    let setUserValues (userValues : UserValues) =
        withCurrentUser (Database.setUserValues db userValues)
        |> withTransaction


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
        withCurrentUser (Database.setPasswordSaltAndConfirmEmailGuid db hashedPassword salt None)
        |> withTransaction

    [<Remote>]
    let login (email : string) (password : string) =
        async {
            log.Debug (sprintf "email = %s, password = password")
            try
                use dbConn = new NpgsqlConnection(Settings.DbConnStr)
                dbConn.Open()
                match Database.getValidateLoginData db email with
                | Some v ->
                    match v.confirmEmailGuid with
                    | None ->
                        let hash = generateHashWithSalt password v.salt 1000 64
                        if hash = v.hashedPassword
                        then
                            Database.insertLastLogin db v.userId
                            return ok ()
                        else return fail "Email oder Passwort ist falsch."
                    | Some _ -> return fail "Bitte bestätige deine Email-Adresse."
                | None -> return fail  "Email oder Passwort ist falsch."
            with
            | e ->
                log.Error("", e)
                return fail "An error occurred."
        }
    
    [<Remote>]
    let setUserEmail (email : string) =
        if email = "geprueftundokund nicht doppelt" || true //TODO
        then
            withCurrentUser (fun userId -> Database.setUserEmail db email userId)
            |> withTransaction
            |> Async.map (fun _ -> ok())
        else
            async { return fail "Email is not valid" }
    
    [<Remote>]
    let loginUserBySessionGuid sessionGuid =
        log.Debug(sprintf "(sessionGuid = %s)" sessionGuid)
        match Database.getUserIdBySessionGuid db sessionGuid with
        | None ->
            log.Debug(sprintf "(sessionGuid = %s) = %b" sessionGuid false)
            async { return false }
        | Some (UserId userId) ->
            GetContext().UserSession.LoginUser (userId |> string) |> Async.RunSynchronously
            log.Debug(sprintf "(sessionGuid = %s) = true" sessionGuid)
            async { return true }
    
    [<Remote>]
    let setSessionGuid oSessionGuid =
        withCurrentUser (Database.setSessionGuid db oSessionGuid)
        |> withTransaction
    
    [<Remote>]
    let loginAsGuest (sessionGuid : string) =
        let (UserId userId) = Database.insertNewUser db None "" "" (Some <| Guid.NewGuid().ToString("N")) (Some sessionGuid) DateTime.Now
        GetContext().UserSession.LoginUser (string userId) |> Async.RunSynchronously
        withCurrentUser (Database.insertLastLogin db)
        |> withTransaction

    [<Remote>]
    let isUserLoggedIn () =
        let loggedInUser = (GetContext().UserSession.GetLoggedInUser() |> Async.RunSynchronously)
        async {
            return loggedInUser.IsSome
        }
    
    [<Remote>]
    let isLoggedInAsGuest () =
        withCurrentUserFail
            (fun userId ->  Database.getConfirmEmailGuidByUserId db userId |> Option.isSome)
            (fun () -> false)

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
                    use dbConn = new NpgsqlConnection(Settings.DbConnStr)
                    dbConn.Open()
                    if Database.userEmailExists db email
                    then
                        return fail "Diese Email-Adresse ist schon registriert."
                    else
                        let salt = generateSalt 64
                        let hashedPassword = generateHashWithSalt password salt 1000 64
                        let confirmEmailGuid = Guid.NewGuid().ToString("N")
                        Database.insertNewUser db (Some email) hashedPassword salt (Some confirmEmailGuid) |> ignore
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
            log.Debug (sprintf "(email = %s, guid = %s)" email confirmEmailGuid)
            let oConfirmEmailGuid = Database.getConfirmEmailGuid db email
            match oConfirmEmailGuid with
            | None -> return ok "Email already confirmed"
            | Some guid when guid = confirmEmailGuid ->
                let recordCount = Database.setConfirmEmailGuidToNull db email
                if recordCount <> 1
                then
                    log.Error("Email does not exist or confirmEmailGuid was already null: " + email)
                    return fail "An error occurred."
                else return ok "Email has been confirmed."
            | Some _ ->
                return fail "Unknown confirmEmailGuid"
        }

    [<Remote>]
    let getDocumentNames () =
        log.Debug "()"
        doWithCurrentUser (Database.getDocumentNames db)

    [<Remote>]
    let overwriteDocument (document : Document) =
        fun userId ->
            try
                Database.overwriteDocument db document userId
            with
            | e ->
                dbContext.ClearUpdates() |> ignore
                log.Error("Saving Document failed", e)
                failwith ("Saving Document failed" + e.ToString())
        |> withCurrentUser
        |> withTransaction

    [<Remote>]
    let saveNewDocument (document : Document) =
        match getCurrentUserId() |> Async.RunSynchronously with
        | None -> failwith "Nobody logged in"
        | Some userId ->
            try
                async {
                    use dbScope = new TransactionScope()
                    let documentId = Database.saveNewDocument db document (UserId userId)
                    dbScope.Complete()
                    return documentId
                }
            with
            | e ->
                dbContext.ClearUpdates() |> ignore
                log.Error("Saving Document failed", e)
                failwith "Saving Document failed"

    [<Remote>]
    let deleteDocument (DocumentId documentId) =
        async {
            use dbConn = new NpgsqlConnection(Settings.DbConnStr)
            dbConn.Open()
            Database.deleteDocument db documentId
            let filePaths = Database.getDeletableFilePaths db documentId
            for filePath in filePaths do
                if Path.IsPathRooted filePath
                then File.Delete filePath
                else File.Delete <| toRootedPath filePath
            Database.deleteDeletableDocumentFilePages db documentId |> ignore
            dbContext.SubmitUpdates()
        }

    let getDocumentOffset' (htmlJobApplicationOffset : int) (userId : UserId) =
        Database.getDocumentOffset db userId htmlJobApplicationOffset
    [<Remote>]
    let getDocumentOffset (htmlJobApplicationOffset : int) =
        doWithCurrentUser (getDocumentOffset' htmlJobApplicationOffset)

    [<Remote>]
    let replaceVariables
            (filePath : string)
            (userValues : UserValues)
            (employer : Employer)
            (document : Document) =
        let oUserEmail = getUserEmail () |> Async.RunSynchronously
        async {
            try
                let tmpDirectory = Path.Combine(Settings.DataDirectory, "tmp", Guid.NewGuid().ToString("N"))
                let! map = toCV employer userValues (oUserEmail |> Option.defaultValue "") document.jobName document.customVariables
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
                    Odt.replaceInFile newFilePath map Ignore
                    return newFilePath
            with
            | e ->
                log.Error ("", e)
                return failwith "An error occurred"
        }
         
    let emailSentApplicationToUser' (sentApplicationOffset : int) (customVariablesString : string) userId =
        try
            let userEmail = Database.getEmailByUserId db userId |> Option.defaultValue ""
            match Database.getSentApplicationOffset db sentApplicationOffset userId with
            | None -> fail "The requested application could not be not found"
            | Some sentApplication ->
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
                                        Ignore
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
                    (Odt.replaceInString sentApplication.email.subject myList Ignore)
                    (Odt.replaceInString (sentApplication.email.body.Replace("\\r\\n", "\n").Replace("\\n", "\n")) myList Ignore)
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
    let emailSentApplicationToUser (sentApplicationOffset : int) (customVariablesString : string) : Async<Result<unit, string>> =
        doWithCurrentUser (emailSentApplicationToUser' sentApplicationOffset customVariablesString)
    
    [<Remote>]
    let sendNotYetSentApplication sentApplicationId =
        try
            let oSentApp = Database.getSentApplication db sentApplicationId
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
                                    Ignore
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


                db.Sentstatus.Create(Sentapplicationid = sentApplicationId, Statuschangedon = DateTime.Today, Dueon = None, 
                    Sentstatusvalueid = 2, Statusmessage = "") |> ignore
                dbContext.SubmitUpdates()

                sendEmail
                    sentApp.user.email
                    (sentApp.user.values.firstName + " " + sentApp.user.values.lastName)
                    sentApp.employer.email
                    (Odt.replaceInString sentApp.email.subject myList Ignore)
                    (Odt.replaceInString (sentApp.email.body.Replace("\\r\\n", "\n").Replace("\\n", "\n")) myList Ignore)

                    (if pdfPaths = []
                     then []
                     else [mergedPdfPath, (sprintf "Bewerbung_%s_%s.pdf" sentApp.user.values.firstName sentApp.user.values.lastName)]
                     )
                sendEmail
                    sentApp.user.email
                    (sentApp.user.values.firstName + " " + sentApp.user.values.lastName)
                    sentApp.user.email
                    ("Deine Bewerbung wurde versandt - " + Odt.replaceInString sentApp.email.subject myList Ignore)
                    ((sprintf
                        "Deine Bewerbung wurde am %s an %s versandt.\n\n"
                        (DateTime.Now.ToShortDateString())
                        sentApp.employer.email)
                            + Odt.replaceInString (sentApp.email.body.Replace("\\r\\n", "\n").Replace("\\n", "\n")) myList Ignore)

                    (if pdfPaths = []
                     then []
                     else [mergedPdfPath, (sprintf "Bewerbung_%s_%s.pdf" sentApp.user.values.firstName sentApp.user.values.lastName)]
                     )

                let oConfirmEmailGuid = Database.getConfirmEmailGuid db (sentApp.user.email)
                if oConfirmEmailGuid.IsSome
                then
                    sendEmail
                        Settings.EmailUsername
                        "Bewerbungsspam"
                        sentApp.user.email
                        (t German PleaseConfirmYourEmailAddressEmailSubject)
                        (String.Format((t German PleaseConfirmYourEmailAddressEmailBody), sentApp.user.email, oConfirmEmailGuid.Value))
                        []
        with
        | e ->
            log.Error ("", e)
            dbContext.ClearUpdates() |> ignore


    let applyNow'
            (employer : Employer)
            (document : Document)
            (userValues : UserValues)
            (url : string)
            (userId : UserId) =
        try
            Database.setUserValues db userValues userId |> ignore
            let oUserEmail = Database.getEmailByUserId db userId
            match oUserEmail with
            | None ->
                log.Error("UserEmail was None")
                fail "User email was None"
            | Some userEmail ->
                let dbEmployer = Database.addEmployer db employer userId
                Database.insertNotYetSentApplication
                    db
                    userId
                    dbEmployer.Id
                    document.email
                    (userEmail, userValues)
                    (document.pages |> List.choose(fun x -> match x with FilePage p -> Some (p.path, p.pageIndex) | HtmlPage _ -> None))
                    document.jobName
                    url
                    document.customVariables
                    DateTime.Today
                dbContext.SubmitUpdates()
                ok()
        with
        | e ->
            log.Error ("", e)
            dbContext.ClearUpdates() |> ignore
            fail "Couldn't send the application"
    [<Remote>]
    let applyNow
            (employer : Employer)
            (document : Document)
            (userValues : UserValues)
            (url : string) =
        doWithCurrentUser (applyNow' employer document userValues url)

    [<Remote>]
    let addFilePage (DocumentId documentId) path pageIndex name =
        async {
            Database.addFilePage db documentId path pageIndex name
            dbContext.SubmitUpdates()
        }

    
    [<Remote>]
    let getHtmlPageTemplates () =
        async {
            return Database.getHtmlPageTemplates db ()
        }
    
    [<Remote>]
    let getHtmlPageTemplate (templateId : int) =
        async {
            return Database.getHtmlPageTemplate db templateId
        }
    
    [<Remote>]
    let getHtmlPages documentId =
        async {
            return Database.getHtmlPages documentId
        }

    [<Remote>]
    let getPageMapOffset (pageIndex : int) (documentIndex : int)  =
        withCurrentUser (Database.getPageMapOffset db) |> Async.map (fun f -> f () pageIndex documentIndex)
     
    [<Remote>]
    let getLastEditedDocumentOffset () =
        doWithCurrentUser (Database.getLastEditedDocumentOffset db)

    [<Remote>]
    let setLastEditedDocumentId (userId : UserId) (documentId : DocumentId) =
        async { return fun () -> Database.setLastEditedDocumentId db documentId userId }
        |> withTransaction

    [<Remote>]
    let addNewDocument name =
        withCurrentUser (Database.addNewDocument db name)
        |> withTransaction


    [<Remote>]
    let addHtmlPage (documentId : int) (oTemplateId : option<int>) (pageIndex : int) (name : string) =
        async {
            return Database.addHtmlPage db documentId oTemplateId pageIndex name
        }

    [<Remote>]
    let getDocumentIdOffset documentIndex =
        withCurrentUser (Database.tryGetDocumentIdOffset db) |> Async.map (fun f -> f documentIndex)
    
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
            use dbConn = new NpgsqlConnection(Settings.DbConnStr)
            dbConn.Open()
            return Database.createLink db filePath name
        }

    [<Remote>]
    let tryGetPathAndNameByLinkGuid linkGuid =
        async {
            return Database.tryGetPathAndNameByLinkGuid db linkGuid
        }

    [<Remote>]
    let deleteLink linkGuid =
        async {
            use dbConn = new NpgsqlConnection(Settings.DbConnStr)
            dbConn.Open()
            Database.deleteLink db linkGuid
        }


    [<Remote>]
    let getSentApplications () =
        doWithCurrentUser (Database.getSentApplications db)

    let getFilesWithExtension extension userId =
        async {
            return Database.getFilesWithExtension db extension userId
        }

    [<Remote>]
    let getNotYetSentApplicationIds () = 
        async {
            return Database.getNotYetSentApplicationIds db
        }

    let getFilePageNames (documentId : DocumentId) =
        async {
            return Database.getFilePageNames db documentId
        }

    [<Remote>]
    let tryFindSentApplication (employer : Employer) =
        doWithCurrentUser (Database.tryFindSentApplication db employer)
    
    let sendNotYetSentApplications () =
        let sendNotYetSentApplications () =
            async {
                printfn "Sending unsent applications: "
                let notYetSentApplicationIds = Database.getNotYetSentApplicationIds db
                printfn "notYetSentApplicationIds: %A" notYetSentApplicationIds
                if notYetSentApplicationIds = []
                then do! Async.Sleep 10000
                notYetSentApplicationIds
                |> List.iter (fun appId -> printfn "Sending %i" appId; sendNotYetSentApplication appId)
            }
        async {
            while true do
                try
                    do! sendNotYetSentApplications()
                with
                | e -> log.Error("", e)
                do! Async.Sleep 5000
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
                try
                    Directory.EnumerateDirectories tmpDir
                    |> Seq.iter deleteOldFiles
                with
                | e -> log.Error("", e)
                do! Async.Sleep (int (TimeSpan.FromMinutes 10.).TotalMilliseconds)
        }
    
    [ sendNotYetSentApplications(); clearTmpFolder() ]
    |> Async.Parallel
    |> Async.Ignore
    |> Async.Start