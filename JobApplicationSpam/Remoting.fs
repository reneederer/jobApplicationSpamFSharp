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
open Translation
open Phrases
open Variables

module Server =
    open System.Net.Mail
    open System.IO
    open WebSharper.Web.Remoting
    open Database

    let log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().GetType())

    [<Remote>]
    let deleteUserWithIdOne () =
        async {
            Database.deleteUserWithIdOne () |> ignore
        }

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

    let withCurrentUser (f : UserId -> 'a)  =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with
            | Some userId ->
                    return f (UserId userId)
            | None -> return failwith "Nobody is logged in"
        }

    let withDB f =
        use dbConn = new NpgsqlConnection(Settings.DbConnStr)
        dbConn.Open()
        f dbConn
    
    let withDBAndCurrentUser f =
        f |> withCurrentUser |> Async.map withDB

    [<Remote>]
    let predefinedVariables =
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
            return Database.getUserIdByEmail email
        }

    [<Remote>]
    let getEmailByUserId userId =
        async {
            return Database.getEmailByUserId userId
        }
    
    let getUserEmail' userId =
        Database.getEmailByUserId userId
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
    


    
    let getUserValues' userId = Database.getUserValues userId
    [<Remote>]
    let getUserValues () = withCurrentUser getUserValues'
    
    let setUserValues' userValues userId = 
        Database.setUserValues userValues userId
    [<Remote>]
    let setUserValues (userValues : UserValues) = withCurrentUser (setUserValues' userValues)


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

    let setPassword' password (userId : UserId) dbConn =
        let salt =  generateSalt 64
        let hashedPassword = generateHashWithSalt password salt 1000 64
        Database.setPasswordSaltAndConfirmEmailGuid dbConn hashedPassword salt None userId 
    [<Remote>]
    let setPassword password = withDBAndCurrentUser (setPassword' password)

    [<Remote>]
    let login (email : string) (password : string) =
        async {
            log.Debug (sprintf "email = %s, password = password")
            try
                use dbConn = new NpgsqlConnection(Settings.DbConnStr)
                dbConn.Open()
                match Database.getValidateLoginData email with
                | Some v when v.confirmEmailGuid.IsNone ->
                    let hash = generateHashWithSalt password v.salt 1000 64
                    if hash = v.hashedPassword
                    then
                        Database.insertLastLogin dbConn v.userId
                        return ok ()
                    else return fail "Email oder Passwort ist falsch."
                |  Some v when v.confirmEmailGuid.IsSome -> return fail "Bitte bestätige deine Email-Adresse."
                | None -> return fail  "Email oder Passwort ist falsch."
            with
            | e ->
                log.Error("", e)
                return fail "An error occurred."
        }
    
    let setUserEmail' (email : string) userId (dbConn : NpgsqlConnection) =
        try
            ok <| Database.setUserEmail dbConn userId email
        with
        | e -> fail "Diese Email-Adresse ist bereits vergeben."
    [<Remote>]
    let setUserEmail (email : string) =
        withDBAndCurrentUser (setUserEmail' email)
    
    [<Remote>]
    let loginUserBySessionGuid sessionGuid =
        log.Debug(sprintf "(sessionGuid = %s)" sessionGuid)
        match Database.getUserIdBySessionGuid sessionGuid with
        | None ->
            log.Debug(sprintf "(sessionGuid = %s) = %A" sessionGuid (fail "Session guid not found"))
            async {return fail "Session guid not found"}
        | Some (UserId userId) ->
            GetContext().UserSession.LoginUser (userId |> string) |> Async.RunSynchronously
            log.Debug(sprintf "(sessionGuid = %s) = ok ()" sessionGuid)
            async {
                return ok ()
            }
    
    let setSessionGuid' guid userId dbConn =
        Database.setSessionGuid dbConn userId guid
    [<Remote>]
    let setSessionGuid guid =
        withDBAndCurrentUser (setSessionGuid' guid)
    
    let loginAsGuest' (sessionGuid : string) (dbConn : NpgsqlConnection) =
        let (UserId userId) = Database.insertNewUser dbConn None "" "" (Some <| Guid.NewGuid().ToString("N")) (Some sessionGuid)
        GetContext().UserSession.LoginUser (string userId) |> Async.RunSynchronously
    [<Remote>]
    let loginAsGuest (sessionGuid : string) =
        withDB (loginAsGuest' sessionGuid)
        async {
            return ()
        }

    [<Remote>]
    let isUserLoggedIn () =
        let loggedInUser = (GetContext().UserSession.GetLoggedInUser() |> Async.RunSynchronously)
        async {
            return loggedInUser.IsSome
        }

    
    [<Remote>]
    let isLoggedInAsGuest () =
        let oUserId = GetContext().UserSession.GetLoggedInUser() |> Async.RunSynchronously |> Option.map (Int32.Parse >> UserId)
        async {
            match oUserId with
            | Some userId ->
                return Database.getConfirmEmailGuidByUserId userId |> Option.isSome
            | None -> return false
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
                    use dbConn = new NpgsqlConnection(Settings.DbConnStr)
                    dbConn.Open()
                    if Database.userEmailExists email
                    then
                        return fail "Diese Email-Adresse ist schon registriert."
                    else
                        let salt = generateSalt 64
                        let hashedPassword = generateHashWithSalt password salt 1000 64
                        let confirmEmailGuid = Guid.NewGuid().ToString("N")
                        Database.insertNewUser dbConn (Some email) hashedPassword salt (Some confirmEmailGuid) |> ignore
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
            let oConfirmEmailGuid = Database.getConfirmEmailGuid email
            match oConfirmEmailGuid with
            | None -> return ok "Email already confirmed"
            | Some guid when guid = confirmEmailGuid ->
                let recordCount = Database.setConfirmEmailGuidToNull email
                if recordCount <> 1
                then
                    log.Error("Email does not exist or confirmEmailGuid was already null: " + email)
                    return fail "An error occurred."
                else return ok "Email has been confirmed."
            | Some _ ->
                return fail "Unknown confirmEmailGuid"
        }

    let getDocumentNames' userId = Database.getDocumentNames userId
    [<Remote>]
    let getDocumentNames () =
        log.Debug "()"
        withCurrentUser getDocumentNames'

    let overwriteDocument' (document : Document) userId (dbConn : NpgsqlConnection) =
        use transaction = dbConn.BeginTransaction()
        try
            Database.overwriteDocument dbConn document userId
            transaction.Commit()
        with
        | e ->
            transaction.Rollback()
            log.Error("Saving Document failed", e)
            failwith "Saving Document failed"
    [<Remote>]
    let overwriteDocument (document : Document) =
        (overwriteDocument' document) |> withCurrentUser |> Async.map withDB

    let saveNewDocument' userId (document : Document) (dbConn : NpgsqlConnection) =
        use transaction = dbConn.BeginTransaction()
        try
            let documentId = Database.saveNewDocument dbConn document userId
            transaction.Commit()
            documentId
        with
        | e ->
            transaction.Rollback()
            log.Error("Saving Document failed", e)
            failwith "Saving Document failed"
    [<Remote>]
    let saveNewDocument (document : Document) =
        let a = withCurrentUser saveNewDocument'
        let b = Async.map (fun x -> x document) a
        let c = Async.map withDB b
        c

    let deleteDocument' documentId dbConn =
        Database.deleteDocument documentId
        let filePaths = Database.getDeletableFilePaths dbConn documentId
        for filePath in filePaths do
            if Path.IsPathRooted filePath
            then File.Delete filePath
            else File.Delete <| toRootedPath filePath
        Database.deleteDeletableDocumentFilePages dbConn documentId |> ignore
    [<Remote>]
    let deleteDocument (DocumentId documentId) =
        async {
            return withDB (deleteDocument' documentId)
        }

    let getDocumentOffset' (htmlJobApplicationOffset : int) (userId : UserId) =
        Database.getDocumentOffset userId htmlJobApplicationOffset
    [<Remote>]
    let getDocumentOffset (htmlJobApplicationOffset : int) =
        withCurrentUser (getDocumentOffset' htmlJobApplicationOffset)

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
         
    let emailSentApplicationToUser' (sentApplicationOffset : int) (customVariablesString : string) userId dbConn =
        try
            let userEmail = Database.getEmailByUserId userId |> Option.defaultValue ""
            match Database.getSentApplication dbConn sentApplicationOffset userId with
            | None -> fail "The requested application could not be not found"
            | Some sentApplication ->
                let myList =
                    toCV
                        sentApplication.employer
                        sentApplication.userValues
                        sentApplication.userEmail
                        sentApplication.jobName
                        customVariablesString
                    |> Async.RunSynchronously
                let tmpDirectory = Path.Combine(Settings.DataDirectory, "tmp", Guid.NewGuid().ToString("N"))
                let odtPaths =
                    [ for (path, pageIndex) in sentApplication.filePages do
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
                    ]
                let mergedPdfPath =
                    Path.Combine(
                        tmpDirectory,
                        (sprintf
                            "Bewerbung_%s_%s.pdf"
                            sentApplication.userValues.firstName
                            sentApplication.userValues.lastName
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
                                sentApplication.userValues.firstName
                                sentApplication.userValues.lastName
                           ).Replace("_.", ".").Replace("_.", ".")]
                     )
                ok ()
        with
        | e ->
            log.Error ("", e)
            fail "Couldn't email the application to the user"

    [<Remote>]
    let emailSentApplicationToUser (sentApplicationOffset : int) (customVariablesString : string)=
        withDBAndCurrentUser (emailSentApplicationToUser' sentApplicationOffset customVariablesString)

    
    let applyNow'
            (isGuest : bool)
            (employer : Employer)
            (document : Document)
            (userValues : UserValues)
            (url : string)
            (userId : UserId)
            (dbConn : NpgsqlConnection) =
        use transaction = dbConn.BeginTransaction()
        try
            use command = new NpgsqlCommand("set constraints all deferred", dbConn)
            command.ExecuteNonQuery() |> ignore
            command.Dispose()
            Database.setUserValues userValues userId |> ignore
            let oUserEmail = Database.getEmailByUserId userId
            match oUserEmail with
            | None ->
                transaction.Rollback()
                fail "User email was None"
            | Some userEmail ->
                let employerId = Database.addEmployer dbConn employer userId
                if (oUserEmail <> Some employer.email) || oUserEmail = (Some "rene.ederer.nbg@gmail.com")
                then
                    Database.insertSentApplication
                        dbConn
                        userId
                        employerId
                        document.email
                        (userEmail, userValues)
                        (document.pages |> List.choose(fun x -> match x with FilePage p -> Some (p.path, p.pageIndex) | HtmlPage _ -> None))
                        document.jobName
                        url
                        document.customVariables
                let myList =
                        toCV
                            employer
                            userValues
                            userEmail
                            document.jobName
                            document.customVariables
                        |> Async.RunSynchronously
                let tmpDirectory = Path.Combine(Settings.DataDirectory, "tmp", Guid.NewGuid().ToString("N"))
                let odtPaths =
                    [ for item in document.pages do
                        match item with
                        | HtmlPage htmlPage ->
                            let oPageTemplatePath = Option.bind (Database.getHtmlPageTemplatePath) htmlPage.oTemplateId
                            let pageTemplatePath =
                                match oPageTemplatePath with
                                | None -> 
                                    failwith "oPageTemplatePath was None"
                                | Some path -> path
                            let lines = []
                                (*let emptyLines = List.init 50 (fun i -> sprintf "$line%i" (i + 1), "")
                                let pageLines = page.map.["mainText"].Split([|'\n'|])
                                let len = pageLines.Length
                                (pageLines
                                |> Array.mapi (fun i x -> sprintf "$line%i" (i + 1), x)
                                |> List.ofArray)
                                @ List.skip len emptyLines
                                |> List.sortByDescending (fun (key, _) -> key.Length)
                                *)
                            yield Odt.replaceInOdt pageTemplatePath "c:/users/rene/myodt/" tmpDirectory (myList @ lines)
                        | FilePage filePage ->
                            yield
                                if unoconvImageTypes |> List.contains(Path.GetExtension(filePage.path).ToLower().Substring(1))
                                then
                                    toRootedPath filePage.path
                                elif filePage.path.ToLower().EndsWith(".odt")
                                then
                                    let directoryGuid = Guid.NewGuid().ToString("N")
                                    Odt.replaceInOdt
                                        (toRootedPath filePage.path)
                                        (Path.Combine(tmpDirectory, directoryGuid, "extractedOdt"))
                                        (Path.Combine(tmpDirectory, directoryGuid, "replacedOdt"))
                                        myList
                                else
                                    let copiedPath = Path.Combine(tmpDirectory, Guid.NewGuid().ToString("N"), Path.GetFileName(filePage.path))
                                    Directory.CreateDirectory(Path.GetDirectoryName copiedPath) |> ignore
                                    File.Copy(toRootedPath filePage.path, copiedPath)
                                    Odt.replaceInFile
                                        copiedPath
                                        myList
                                        Ignore
                                    copiedPath
                    ]

                let pdfPaths =
                    [ for odtPath in odtPaths do
                        yield Odt.odtToPdf odtPath
                    ]
                let mergedPdfPath = Path.Combine(tmpDirectory, Guid.NewGuid().ToString() + ".pdf")
                if pdfPaths <> []
                then Odt.mergePdfs pdfPaths mergedPdfPath

                let oConfirmEmailGuid = Database.getConfirmEmailGuidByUserId userId

                sendEmail
                    userEmail
                    (userValues.firstName + " " + userValues.lastName)
                    employer.email
                    (Odt.replaceInString document.email.subject myList Ignore)
                    (Odt.replaceInString (document.email.body.Replace("\\r\\n", "\n").Replace("\\n", "\n")) myList Ignore)

                    (if pdfPaths = []
                     then []
                     else [mergedPdfPath, (sprintf "Bewerbung_%s_%s.pdf" userValues.firstName userValues.lastName)]
                     )
                sendEmail
                    userEmail
                    (userValues.firstName + " " + userValues.lastName)
                    userEmail
                    ("Deine Bewerbung wurde versandt - " + Odt.replaceInString document.email.subject myList Ignore)
                    ((sprintf "Deine Bewerbung wurde am %s an %s versandt.\n\n" (DateTime.Now.ToShortDateString()) employer.email) + Odt.replaceInString (document.email.body.Replace("\\r\\n", "\n").Replace("\\n", "\n")) myList Ignore)

                    (if pdfPaths = []
                     then []
                     else [mergedPdfPath, (sprintf "Bewerbung_%s_%s.pdf" userValues.firstName userValues.lastName)]
                     )
                transaction.Commit()
                if isGuest && oConfirmEmailGuid.IsSome
                then
                    sendEmail
                        Settings.EmailUsername
                        "Bewerbungsspam"
                        userEmail
                        (t German PleaseConfirmYourEmailAddressEmailSubject)
                        (String.Format((t German PleaseConfirmYourEmailAddressEmailBody), userEmail, oConfirmEmailGuid.Value))
                        []
                ok ()
        with
        | e ->
            log.Error ("", e)
            transaction.Rollback()
            fail "Couldn't send the application"
    [<Remote>]
    let applyNow
            (employer : Employer)
            (document : Document)
            (userValues : UserValues)
            (url : string) =
        let isGuest = isLoggedInAsGuest () |> Async.RunSynchronously
        withDBAndCurrentUser (applyNow' isGuest employer document userValues url)

    let addFilePage' documentId path pageIndex name (dbConn : NpgsqlConnection) =
        Database.addFilePage dbConn documentId path pageIndex name
    [<Remote>]
    let addFilePage (DocumentId documentId) path pageIndex name =
        async {
            return withDB (addFilePage' documentId path pageIndex name)
        }

    
    [<Remote>]
    let getHtmlPageTemplates () =
        async {
            return Database.getHtmlPageTemplates ()
        }
    
    [<Remote>]
    let getHtmlPageTemplate (templateId : int) =
        async {
            return Database.getHtmlPageTemplate templateId
        }
    
    [<Remote>]
    let getHtmlPages documentId =
        async {
            return Database.getHtmlPages documentId
        }

    let getPageMapOffset' pageIndex documentIndex userId dbConn =
        Database.getPageMapOffset dbConn userId pageIndex documentIndex
    [<Remote>]
    let getPageMapOffset pageIndex documentIndex  =
        withDBAndCurrentUser (getPageMapOffset' pageIndex documentIndex)
     
    let getLastEditedDocumentOffset' userId =
        Database.getLastEditedDocumentOffset userId
    [<Remote>]
    let getLastEditedDocumentOffset () =
        withCurrentUser getLastEditedDocumentOffset'

    let setLastEditedDocumentId' documentId userId dbConn =
        Database.setLastEditedDocumentId dbConn userId documentId
    [<Remote>]
    let setLastEditedDocumentId (userId : UserId) (documentId : DocumentId) =
        async {
            return withDB (setLastEditedDocumentId' documentId userId)
        }

    let addNewDocument' name userId (dbConn : NpgsqlConnection) =
        Database.addNewDocument dbConn userId name
    [<Remote>]
    let addNewDocument name =
        withDBAndCurrentUser (addNewDocument' name)


    let addHtmlPage' (documentId : int) (oTemplateId : option<int>) (pageIndex : int) (name : string) userId (dbConn : NpgsqlConnection) =
        Database.addHtmlPage dbConn documentId oTemplateId pageIndex name
    [<Remote>]
    let addHtmlPage (documentId : int) (oTemplateId : option<int>) (pageIndex : int) (name : string) =
        withDBAndCurrentUser (addHtmlPage' documentId oTemplateId pageIndex name)

    let getDocumentIdOffset' documentIndex userId =
        Database.tryGetDocumentIdOffset userId documentIndex
    [<Remote>]
    let getDocumentIdOffset documentIndex =
        withCurrentUser (getDocumentIdOffset' documentIndex)
    
    [<Remote>]
    let readWebsite (identifier : string) : Async<Result<Employer, string>> =
        async {
            return Website.read identifier
        }
    
    [<Remote>]
    let createLink filePath name =
        async {
            use dbConn = new NpgsqlConnection(Settings.DbConnStr)
            dbConn.Open()
            return Database.createLink dbConn filePath name
        }

    [<Remote>]
    let tryGetPathAndNameByLinkGuid linkGuid =
        async {
            return Database.tryGetPathAndNameByLinkGuid linkGuid
        }

    [<Remote>]
    let deleteLink linkGuid =
        async {
            use dbConn = new NpgsqlConnection(Settings.DbConnStr)
            dbConn.Open()
            Database.deleteLink dbConn linkGuid
        }

    let getSentApplications' userId =
        Database.getSentApplications userId
    [<Remote>]
    let getSentApplications () =
        withCurrentUser getSentApplications'

    let getFilesWithSameExtension filePath userId =
        async {
            return Database.getFilesWithExtension (Path.GetExtension(filePath).ToLower().Substring(1)) userId
        }

    let getFilePageNames documentId =
        async {
            return Database.getFilePageNames documentId
        }

    let tryFindSentApplication' (employer : Employer) userId =
        Database.tryFindSentApplication userId employer
    [<Remote>]
    let tryFindSentApplication (employer : Employer) =
        withCurrentUser (tryFindSentApplication' employer)
    
    [<Remote>]
    let convertToOdt filePath =
        async {
            return Odt.convertToOdt filePath
        }

