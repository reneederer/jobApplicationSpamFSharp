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


module Server =
    open System.Web
    open System.Net.Mail
    open System.IO
    open WebSharper.Web.Remoting
    open WebSharper.Core
    open System.Transactions

    let log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().GetType())


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
                let! userEmail = getEmailByUserId userId
                return userEmail
            | None -> return failwith "Nobody is logged in"
        }



    let sendEmail fromAddress fromName toAddress subject body (attachmentPathsAndNames : list<string * string>) =
        use smtpClient = new SmtpClient(ConfigurationManager.AppSettings.["emailServer"], ConfigurationManager.AppSettings.["emailPort"] |> Int32.TryParse |> snd)
        smtpClient.EnableSsl <- true
        //smtpClient.UseDefaultCredentials <- false
        //smtpClient.DeliveryMethod <- SmtpDeliveryMethod.Network
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
        match getCurrentUserId() |> Async.RunSynchronously with
        | Some userId ->
            async {
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                return Database.getUserValues userId
            }
        | None -> failwith "There is no user logged in."



    [<Remote>]
    let addUserValues (userValues : UserValues) =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with
            | Some userId ->
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                use transaction = dbConn.BeginTransaction()
                try
                    let insertedUserValuesId = Database.setUserValues userValues userId
                    transaction.Commit()
                    return ok "User values have been updated."
                with
                | e ->
                    transaction.Rollback()
                    return fail "Setting user values failed"
            | None -> return fail "Please login."
        }



    let generateSalt length =
        let (bytes : array<byte>) = Array.replicate length (0uy)
        use rng = new RNGCryptoServiceProvider()
        rng.GetBytes(bytes)
        bytes |> Convert.ToBase64String

    let generateHash (password : string) (salt : string) iterations length =
        use deriveBytes = new Rfc2898DeriveBytes(password |> System.Text.Encoding.UTF8.GetBytes, salt |> Convert.FromBase64String, iterations)
        deriveBytes.GetBytes(length) |> Convert.ToBase64String


    [<Remote>]
    let login (email : string) (password : string) =
        async {
            log.Debug (sprintf "email = %s, password = password")
            try
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                match Database.getIdPasswordSaltAndGuid email with
                | Some (userId, hashedPassword, salt, None) ->
                    if generateHash password salt 1000 64 = hashedPassword
                    then
                        Database.insertLastLogin dbConn userId
                        return ok <| string userId
                    else return fail "Email oder Passwort ist falsch."
                |  Some (_, _, _, Some guid) -> return fail "Bitte bestätige deine Email-Adresse."
                | None -> return fail  "Email oder Passwort ist falsch."
            with
            | e ->
                log.Error("", e)
                return fail "An error occurred"
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
                    use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                    dbConn.Open()
                    if Database.userEmailExists email
                    then
                        return fail "Diese Email-Adresse ist schon registriert."
                    else
                        let salt = generateSalt(64)
                        let hashedPassword = generateHash password salt 1000 64
                        let guid = Guid.NewGuid().ToString("N")
                        Database.insertNewUser dbConn email hashedPassword salt guid |> ignore
                        sendEmail
                            ConfigurationManager.AppSettings.["emailUsername"]
                            ConfigurationManager.AppSettings.["domainName"]
                            email
                            (t German PleaseConfirmYourEmailAddressEmailSubject)
                            (String.Format(t German PleaseConfirmYourEmailAddressEmailBody, email, guid))
                            []
                        return ok "Please confirm your email"
            with
            | e ->
                log.Error("", e)
                return fail "An error occured."
        }

    [<Remote>]
    let confirmEmail email guid =
        async {
            log.Debug (sprintf "(email = %s, guid = %s)" email guid)
            try
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                let oDbGuid = Database.getGuid email
                match oDbGuid with
                | None -> return ok "Email already confirmed"
                | Some dbGuid when guid = dbGuid ->
                    use transaction = dbConn.BeginTransaction()
                    let recordCount = Database.setGuidToNull email
                    if recordCount <> 1
                    then
                        log.Error("Email does not exist or guid was already null: " + email)
                        return fail "An error occurred."
                    else return ok "Email has been confirmed."
                | Some _ ->
                    return fail "Unknown guid"

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
                    let documentId = Database.overwriteDocument dbConn document userId
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
                File.Delete <| Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], filePath)
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
    let replaceMap (userEmail : string) (userValues : UserValues) (employer : Employer) (jobName : string) =
        async {
            return
                [ ("$firmaName", employer.company)
                  ("$firmaStrasse", employer.street)
                  ("$firmaPlz", employer.postcode)
                  ("$firmaStadt", employer.city)
                  ("$chefAnredeBriefkopf", match employer.gender with Gender.Male -> "Herrn" | Gender.Female -> "Frau" | Gender.Unknown -> "")
                  ("$chefAnrede", match employer.gender with Gender.Male -> "Herr" | Gender.Female -> "Frau" | Gender.Unknown -> "")
                  ("$geehrter", match employer.gender with Gender.Male -> "geehrter" | Gender.Female -> "geehrte" | Gender.Unknown -> "")
                  ("$anredeZeile",
                        match (employer.gender, employer.lastName) with
                        | Gender.Male, _ -> "Sehr geehrter Herr $chefTitel $chefNachname,"
                        | Gender.Female, _ -> "Sehr geehrte Frau $chefTitel $chefNachname,"
                        | _, s when s.Trim() = "" -> "Sehr geehrte Damen und Herren,"
                        | Gender.Unknown, _ -> "Sehr geehrte Damen und Herren,")
                  ("$telefonZeile", "Telefon: $meineTelefonnr")
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
                  ("$meineTelefonnummer", userValues.phone)
                  ("$meineTelefonnr", userValues.phone)
                  ("$meinTelefon", userValues.phone)
                  ("$meinMobilTelefon", userValues.mobilePhone)
                  ("$meineMobilnummer", userValues.mobilePhone)
                  ("$meineMobilnr", userValues.mobilePhone)
                  ("$datumHeute", DateTime.Today.ToString("dd.MM.yyyy"))
                  ("$beruf", jobName)
                ]
                |> fun xs ->
                        (xs.OrderByDescending(fun (k, v) -> k.Length))
                           .ThenBy(fun (k, v) -> if k.EndsWith("_") then 1 else -1)
                |> List.ofSeq
        }

    [<Remote>]
    let replaceVariables (filePath : string) (userValues : UserValues) (employer : Employer) (document : Document)=
        match getCurrentUserId() |> Async.RunSynchronously with
        | Some userId ->
            async {
                let! userEmail = getEmailByUserId userId
                let guid = Guid.NewGuid().ToString("N")
                let! map = replaceMap (userEmail |> Option.defaultValue "") userValues employer document.jobName
                Directory.CreateDirectory(Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], "tmp", guid)) |> ignore
                if filePath.EndsWith(".odt") || filePath.EndsWith(".docx")
                then
                    return
                        Odt.replaceInOdt
                            (Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], filePath))
                            (Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], sprintf "tmp/%s/extracted/" guid))
                            (Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], sprintf "tmp/%s/replaced/" guid))
                            map
                else
                    let newFilePath = Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], "tmp", guid, (Path.GetFileName(filePath)))
                    File.Copy(Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], filePath), newFilePath, true)
                    Odt.replaceInFile newFilePath map Ignore
                    return newFilePath
            }

        | None -> async { return "" }
         
    [<Remote>]
    let emailSentApplicationToUser (sentApplicationOffset : int) =
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
                        let! myList = replaceMap sentApplication.userEmail sentApplication.userValues sentApplication.employer sentApplication.jobName
                        let tmpPath = Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], "tmp", Guid.NewGuid().ToString("N"))
                        let odtPaths =
                            [ for (path, pageIndex) in sentApplication.filePages do
                                    yield
                                        if path.EndsWith(".pdf")
                                        then
                                            Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], path)
                                        elif path.EndsWith(".odt") || path.EndsWith(".docx")
                                        then
                                            let directoryGuid = Guid.NewGuid().ToString("N")
                                            Odt.replaceInOdt
                                                (Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], path))
                                                (Path.Combine(tmpPath, directoryGuid, "extractedOdt"))
                                                (Path.Combine(tmpPath, directoryGuid, "replacedOdt"))
                                                myList
                                        else
                                            let copiedPath = Path.Combine(tmpPath, Guid.NewGuid().ToString("N"), Path.GetFileName(path))
                                            Directory.CreateDirectory(Path.GetDirectoryName copiedPath) |> ignore
                                            File.Copy(Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], path), copiedPath)
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
                        let mergedPdfPath = Path.Combine(tmpPath, (sprintf "Bewerbung_%s_%s.pdf" sentApplication.userValues.firstName sentApplication.userValues.lastName))
                        if pdfPaths <> [] then Odt.mergePdfs pdfPaths mergedPdfPath
                        sendEmail
                            ConfigurationManager.AppSettings.["emailUsername"]
                            "Bewerbungsspam"
                            userEmail
                            (Odt.replaceInString sentApplication.email.subject myList Ignore)
                            (Odt.replaceInString (sentApplication.email.body.Replace("\\r\\n", "\n").Replace("\\n", "\n")) myList Ignore)
                            (if pdfPaths = []
                             then []
                             else [mergedPdfPath, sprintf "Bewerbung_%s_%s.pdf" sentApplication.userValues.firstName sentApplication.userValues.lastName]
                             )
                        return ok ()
                with
                | e ->
                    log.Error ("", e)
                    return fail "Couldn't email the application to the user"
        }

    [<Remote>]
    let applyNow (employer : Employer) (document : Document) (userValues : UserValues) (url : string) =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with 
            | None -> return failwith "No user logged in"
            | Some userId ->
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                Database.setUserValues userValues userId |> ignore
                Database.overwriteDocument dbConn document userId |> ignore
                use transaction = dbConn.BeginTransaction()
                use command = new NpgsqlCommand("set constraints all deferred", dbConn)
                command.ExecuteNonQuery() |> ignore
                command.Dispose()
                try
                    let userEmail = Database.getEmailByUserId userId |> Option.get
                    let employerId = Database.addEmployer dbConn employer userId
                    if userEmail <> employer.email || true
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
                    let! myList = replaceMap userEmail userValues employer document.jobName
                    let tmpPath = Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], "tmp", Guid.NewGuid().ToString("N"))
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
                                yield Odt.replaceInOdt pageTemplatePath "c:/users/rene/myodt/" tmpPath (myList @ lines)
                            | FilePage filePage ->
                                yield
                                    if filePage.path.EndsWith(".pdf")
                                    then
                                        Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], filePage.path)
                                    elif filePage.path.EndsWith(".odt") || filePage.path.EndsWith(".docx")
                                    then
                                        let directoryGuid = Guid.NewGuid().ToString("N")
                                        Odt.replaceInOdt
                                            (Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], filePage.path))
                                            (Path.Combine(tmpPath, directoryGuid, "extractedOdt"))
                                            (Path.Combine(tmpPath, directoryGuid, "replacedOdt"))
                                            myList
                                    else
                                        let copiedPath = Path.Combine(tmpPath, Guid.NewGuid().ToString("N"), Path.GetFileName(filePage.path))
                                        Directory.CreateDirectory(Path.GetDirectoryName copiedPath) |> ignore
                                        File.Copy(Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], filePage.path), copiedPath)
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
                    let mergedPdfPath = Path.Combine(tmpPath, (sprintf "Bewerbung_%s_%s.pdf" userValues.firstName userValues.lastName))
                    if pdfPaths <> [] then Odt.mergePdfs pdfPaths mergedPdfPath
                    sendEmail
                        userEmail
                        (userValues.firstName + " " + userValues.lastName)
                        employer.email
                        (Odt.replaceInString document.email.subject myList Ignore)
                        (Odt.replaceInString (document.email.body.Replace("\\r\\n", "\n").Replace("\\n", "\n")) myList Ignore)
                        (if pdfPaths = []
                         then []
                         else [mergedPdfPath, sprintf "Bewerbung_%s_%s.pdf" userValues.firstName userValues.lastName]
                         )
                    transaction.Commit()
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
            return Database.createLink dbConn (Path.Combine(ConfigurationManager.AppSettings.["dataDirectory"], filePath)) name
        }

    [<Remote>]
    let tryGetPathAndNameByGuid guid =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.tryGetPathAndNameByGuid guid
        }

    [<Remote>]
    let deleteLink guid =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            Database.deleteLink dbConn guid
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

