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
open System.Linq
open Translation
open Phrases
open Variables


module Server =
    open System.Net.Mail
    open System.IO
    open WebSharper.Web.Remoting

    let log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().GetType())

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
    let getEmailByUserId userId =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.getEmailByUserId userId
        }

    [<Remote>]
    let getUserIdByEmail (email : string) =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.getUserIdByEmail email
        }

    [<Remote>]
    let getCurrentUserId () =
        GetContext().UserSession.GetLoggedInUser()
        |> Async.map (
            Option.map (Int32.TryParse)
            >> Option.bind (fun (parsed, v) -> if parsed then Some v else None)
        )
    
    [<Remote>]
    let getCurrentUserEmail () =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with
            | Some userId ->
                let! oUserEmail = getEmailByUserId userId
                return oUserEmail
            | None -> return None
        }
    




    let sendEmail fromAddress fromName toAddress subject body (attachmentPathsAndNames : list<string * string>) =
        use smtpClient = new SmtpClient(ConfigurationManager.AppSettings.["emailServer"], ConfigurationManager.AppSettings.["emailPort"] |> Int32.TryParse |> snd)
        smtpClient.EnableSsl <- true
        smtpClient.Credentials <- new System.Net.NetworkCredential(ConfigurationManager.AppSettings.["emailUsername"], ConfigurationManager.AppSettings.["emailPassword"])
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
    let getCurrentUserValues () =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with
            | Some userId ->
                    use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                    dbConn.Open()
                    return Some (Database.getUserValues userId)
            | None -> return None
        }

    [<Remote>]
    let setUserValues (userValues : UserValues) =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with
            | Some userId ->
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                Database.setUserValues userValues userId
            | None -> return failwith "Nobody is logged in."
        }

    let generateSalt length =
        let (bytes : array<byte>) = Array.replicate length (0uy)
        use rng = new RNGCryptoServiceProvider()
        rng.GetBytes(bytes)
        bytes |> Convert.ToBase64String

    let generateHashWithSalt (password : string) (salt : string) iterations length =
        async {
            use deriveBytes = new Rfc2898DeriveBytes(password |> System.Text.Encoding.UTF8.GetBytes, salt |> Convert.FromBase64String, iterations)
            return deriveBytes.GetBytes(length) |> Convert.ToBase64String
        }

    [<Remote>]
    let generateHash (word : string) : Async<string> =
        async {
            use sha256Managed = new SHA256Managed()
            let bytes = sha256Managed.ComputeHash(word |> System.Text.Encoding.UTF8.GetBytes)
            return bytes |> Convert.ToBase64String
        }

    [<Remote>]
    let setPassword password =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with
            | Some userId ->
                let salt =  generateSalt 64
                let! hashedPassword = generateHashWithSalt password salt 1000 64
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                Database.setPasswordSaltAndConfirmEmailGuid dbConn userId hashedPassword salt None
            | None -> return failwith "Nobody is logged in"
        }

    [<Remote>]
    let login (email : string) (password : string) =
        async {
            log.Debug (sprintf "email = %s, password = password")
            try
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                match Database.getValidateLoginData email with
                | Some v when v.confirmEmailGuid.IsNone ->
                    let! hash = generateHashWithSalt password v.salt 1000 64
                    if hash = v.hashedPassword
                    then
                        Database.insertLastLogin dbConn v.userId
                        return ok <| string v.userId
                    else return fail "Email oder Passwort ist falsch."
                |  Some v when v.confirmEmailGuid.IsSome -> return fail "Bitte bestätige deine Email-Adresse."
                | None -> return fail  "Email oder Passwort ist falsch."
            with
            | e ->
                log.Error("", e)
                return fail "An error occurred"
        }
    
    [<Remote>]
    let setUserEmail (email : string) =
        match GetContext().UserSession.GetLoggedInUser() |> Async.RunSynchronously with
        | None -> async {return failwith "Nobody logged in"}
        | Some userId ->
            async {
                try
                    use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                    dbConn.Open()
                    return ok <| Database.setUserEmail dbConn (userId |> Int32.Parse) email
                with
                | e -> return fail "Diese Email existiert bereits"
            }
    
    [<Remote>]
    let loginUserBySessionGuid sessionGuid =
            match Database.getUserIdBySessionGuid sessionGuid with
            | None -> async {return fail "Session guid not found"}
            | Some userId ->
                GetContext().UserSession.LoginUser (userId |> string) |> Async.RunSynchronously
                async {
                    return ok ()
                }
    [<Remote>]
    let setSessionGuid guid =
        let oUserIdStr = GetContext().UserSession.GetLoggedInUser() |> Async.RunSynchronously
        async {
            match oUserIdStr with
            | Some userIdStr -> 
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                Database.setSessionGuid dbConn (userIdStr |> Int32.Parse) guid
            | None -> return failwith "Nobody logged in"
        }
    
    [<Remote>]
    let loginAsGuest (sessionGuid : string) =
        use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
        dbConn.Open()
        let userId = Database.insertNewUser dbConn None "" "" (Some <| Guid.NewGuid().ToString("N")) (Some sessionGuid)
        GetContext().UserSession.LoginUser (string userId) |> Async.RunSynchronously
        async { return () }

    [<Remote>]
    let isUserLoggedIn () =
        let loggedInUser = (GetContext().UserSession.GetLoggedInUser() |> Async.RunSynchronously)
        async {
            return loggedInUser.IsSome
        }

    
    [<Remote>]
    let isLoggedInAsGuest () =
        match GetContext().UserSession.GetLoggedInUser() |> Async.RunSynchronously with
        | Some userIdStr -> 
            async {
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                return
                    Database.getConfirmEmailGuidByUserId (Int32.Parse userIdStr)
                    |> (fun x -> ok <| Option.isSome x)
            }
        | None -> async { return fail "Nobody is logged in." }



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
                    use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                    dbConn.Open()
                    if Database.userEmailExists email
                    then
                        return fail "Diese Email-Adresse ist schon registriert."
                    else
                        let salt = generateSalt 64
                        let! hashedPassword = generateHashWithSalt password salt 1000 64
                        let confirmEmailGuid = Guid.NewGuid().ToString("N")
                        Database.insertNewUser dbConn (Some email) hashedPassword salt (Some confirmEmailGuid) |> ignore
                        sendEmail
                            ConfigurationManager.AppSettings.["emailUsername"]
                            ConfigurationManager.AppSettings.["domainName"]
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
            try
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
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
            with
            | e ->
                log.Error("", e)
                return fail "An error occured."
        }

    [<Remote>]
    let getDocumentNames () =
        log.Debug "()"
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with
            | Some userId ->
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                let result =  Database.getDocumentNames userId
                log.Debug "() = ()"
                return result
            | None ->
                log.Error "No user logged in"
                return  failwith "No user logged in"
        }

    [<Remote>]
    let overwriteDocument (document : Document) =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with
            | Some userId ->
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                use transaction = dbConn.BeginTransaction()
                try
                    Database.overwriteDocument dbConn document userId
                    transaction.Commit()
                with
                | e ->
                    transaction.Rollback()
                    log.Error("Saving Document failed", e)
                    return failwith "Saving Document failed"
            | None -> return failwith "User is not logged in"
        }

    [<Remote>]
    let saveNewDocument (document : Document) =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with
            | Some userId ->
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                use transaction = dbConn.BeginTransaction()
                try
                    let documentId = Database.saveNewDocument dbConn document userId
                    transaction.Commit()
                    return documentId
                with
                | e ->
                    transaction.Rollback()
                    log.Error("Saving Document failed", e)
                    return failwith "Saving Document failed"
            | None -> return failwith "User is not logged in"
        }

    [<Remote>]
    let deleteDocument (documentId : int) =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            Database.deleteDocument dbConn documentId
            let filePaths = Database.getDeletableFilePaths dbConn documentId
            for filePath in filePaths do
                if Path.IsPathRooted filePath
                then File.Delete filePath
                else File.Delete <| toRootedPath filePath
            Database.deleteDeletableDocumentFilePages dbConn documentId |> ignore
        }

    [<Remote>]
    let getDocumentOffset (htmlJobApplicationOffset : int) =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with
            | Some userId ->
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                return Database.getDocumentOffset userId htmlJobApplicationOffset
            | None -> return failwith "No user logged in"
        }

    [<Remote>]
    let replaceVariables
            (filePath : string)
            (userValues : UserValues)
            (employer : Employer)
            (document : Document) =
        match getCurrentUserId() |> Async.RunSynchronously with
        | Some userId ->
            async {
                try
                    let! oUserEmail = getEmailByUserId userId
                    let tmpDirectory = Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], "tmp", Guid.NewGuid().ToString("N"))
                    let! map = toCV employer userValues (oUserEmail |> Option.defaultValue "") document.jobName document.customVariables
                    Directory.CreateDirectory(tmpDirectory) |> ignore
                    if filePath.EndsWith(".odt") || filePath.EndsWith(".docx")
                    then
                        return
                            Odt.replaceInOdt
                                (toRootedPath filePath)
                                (Path.Combine(tmpDirectory, "extracted"))
                                (Path.Combine(tmpDirectory, "replaced"))
                                map
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

        | None -> async { return "" }
         
    [<Remote>]
    let emailSentApplicationToUser (sentApplicationOffset : int) (customVariablesString : string)=
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with 
            | None -> return failwith "No user logged in"
            | Some userId ->
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                try
                    let userEmail = Database.getEmailByUserId userId |> Option.defaultValue ""
                    match Database.getSentApplication dbConn sentApplicationOffset userId with
                    | None -> return fail "The requested application could not be not found"
                    | Some sentApplication ->
                        let! myList =
                            toCV
                                sentApplication.employer
                                sentApplication.userValues
                                sentApplication.userEmail
                                sentApplication.jobName
                                customVariablesString
                        let tmpDirectory = Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], "tmp", Guid.NewGuid().ToString("N"))
                        let odtPaths =
                            [ for (path, pageIndex) in sentApplication.filePages do
                                    yield
                                        if path.EndsWith(".pdf")
                                        then toRootedPath path
                                        elif path.EndsWith(".odt") || path.EndsWith(".docx")
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
                            ConfigurationManager.AppSettings.["emailUsername"]
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
                        return ok ()
                with
                | e ->
                    log.Error ("", e)
                    return fail "Couldn't email the application to the user"
        }

    [<Remote>]
    let applyNow
            (employer : Employer)
            (document : Document)
            (userValues : UserValues)
            (url : string) =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        let isGuestResult = isLoggedInAsGuest () |> Async.RunSynchronously

        async {
            match oUserId, isGuestResult with 
            | None, _ -> return failwith "No user logged in"
            | Some _, Bad _ -> return failwith "An error occurred"
            | Some userId, Ok (isGuest, _) ->
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
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
                        return fail "User email was None"
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
                        let! myList =
                                toCV
                                    employer
                                    userValues
                                    userEmail
                                    document.jobName
                                    document.customVariables
                        let tmpDirectory = Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], "tmp", Guid.NewGuid().ToString("N"))
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
                                        if filePage.path.EndsWith(".pdf")
                                        then
                                            toRootedPath filePage.path
                                        elif filePage.path.EndsWith(".odt") || filePage.path.EndsWith(".docx")
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

                        let oConfirmEmailGuid = Database.getConfirmEmailGuid userEmail

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
                            ((sprintf "Deine Bewerbung wurde am %A an %s versandt.\n\n" DateTime.Today employer.email) + Odt.replaceInString (document.email.body.Replace("\\r\\n", "\n").Replace("\\n", "\n")) myList Ignore)

                            (if pdfPaths = []
                             then []
                             else [mergedPdfPath, (sprintf "Bewerbung_%s_%s.pdf" userValues.firstName userValues.lastName)]
                             )
                        transaction.Commit()
                        if isGuest && oConfirmEmailGuid.IsSome
                        then
                            sendEmail
                                "registration@bewerbungsspam.de"
                                "Bewerbungsspam"
                                userEmail
                                (t German PleaseConfirmYourEmailAddressEmailSubject)
                                (String.Format((t German PleaseConfirmYourEmailAddressEmailBody), userEmail, oConfirmEmailGuid.Value))
                                []

                        return ok ()
                with
                | e ->
                    log.Error ("", e)
                    transaction.Rollback()
                    return fail "Couldn't send the application"
        }

    [<Remote>]
    let addFilePage documentId path pageIndex name =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.addFilePage dbConn documentId path pageIndex name
        }

    
    [<Remote>]
    let getHtmlPageTemplates () =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.getHtmlPageTemplates ()
        }
    
    [<Remote>]
    let getHtmlPageTemplate (templateId : int) =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.getHtmlPageTemplate templateId
        }
    
    [<Remote>]
    let getHtmlPages documentId =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.getHtmlPages documentId
        }

    [<Remote>]
    let getPageMapOffset pageIndex documentIndex  =
        match getCurrentUserId () |> Async.RunSynchronously with
        | None -> failwith "Nobody logged in"
        | Some userId ->
            async {
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                return Database.getPageMapOffset dbConn userId pageIndex documentIndex
            }
     
    [<Remote>]
    let getLastEditedDocumentOffset () =
        match getCurrentUserId () |> Async.RunSynchronously with
        | None -> failwith "Nobody logged in"
        | Some userId ->
            async {
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                return Database.getLastEditedDocumentOffset userId
            }

    [<Remote>]
    let setLastEditedDocumentId (userId : int) (documentId : int) =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            Database.setLastEditedDocumentId dbConn userId documentId
        }


    [<Remote>]
    let addNewDocument name =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        match oUserId with
        | None -> failwith "No user logged in"
        | Some userId ->
            async {
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                return Database.addNewDocument dbConn userId name
            }


    [<Remote>]
    let addHtmlPage (documentId : int) (oTemplateId : option<int>) (pageIndex : int) (name : string) =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        match oUserId with
        | None -> failwith "No user logged in"
        | Some userId ->
            async {
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                return Database.addHtmlPage dbConn documentId oTemplateId pageIndex name
            }

    [<Remote>]
    let getDocumentIdOffset documentIndex =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        match oUserId with
        | None -> failwith "No user logged in"
        | Some userId ->
            async {
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                return Database.tryGetDocumentIdOffset userId documentIndex
            }
    
    [<Remote>]
    let readWebsite (identifier : string) : Async<Result<Employer, string>> =
        async {
            return Website.read identifier
        }
    
    [<Remote>]
    let createLink filePath name =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.createLink dbConn filePath name
        }

    [<Remote>]
    let tryGetPathAndNameByLinkGuid linkGuid =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.tryGetPathAndNameByLinkGuid linkGuid
        }

    [<Remote>]
    let deleteLink linkGuid =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            Database.deleteLink dbConn linkGuid
        }


    [<Remote>]
    let getSentApplications (startDate : DateTime) (endDate : DateTime) =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with
            | Some userId ->
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                return Database.getSentApplications userId startDate endDate
            | None -> 
                return failwith "Nobody is logged in"
        }

    let getFilesWithSameExtension filePath userId =
        async {
            let extension = Path.GetExtension(filePath)
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.getFilesWithExtension extension userId
        }

    let getFilePageNames (documentId : int) =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.getFilePageNames documentId
        }
    
    [<Remote>]
    let tryFindSentApplication (employer : Employer) =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with
            | Some userId ->
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                return Database.tryFindSentApplication userId employer
            | None -> return failwith "Nobody is logged in"
        }
    
    [<Remote>]
    let convertToOdt filePath =
        async {
            return Odt.convertToOdt filePath
        }

