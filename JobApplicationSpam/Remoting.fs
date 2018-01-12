namespace JobApplicationSpam

open WebSharper
open Npgsql
open Chessie.ErrorHandling
open System
open JobApplicationSpam.Types
open System.Configuration
open Website
open Microsoft.Owin.Cors


module Server =
    open Deutsch
    open System.Web
    open System.Net.Mail
    open System.IO
    open WebSharper.Web.Remoting

    let log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().GetType())

    [<Remote>]
    let getMaxUploadSize () =
        async {
            return 5000000
        }

    [<Remote>]
    let getEmailByUserId userId =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.getEmailByUserId dbConn userId
        }

    [<Remote>]
    let getUserIdByEmail (email : string) =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.getUserIdByEmail dbConn email
        }

    [<Remote>]
    let getCurrentUserId () =
        GetContext().UserSession.GetLoggedInUser()
        |> Async.map (
            Option.map (Int32.TryParse)
            >> Option.bind (fun (parsed, v) -> if parsed then Some v else None)
        )
    
    [<Remote>]
    let getLanguageDict language : Async<list<Word * string>>=
        async {
            return
                match language with
                | English ->
                    English.dict
                | Deutsch ->
                    Deutsch.dict
                | _ ->
                    English.dict
        }
    
    [<Remote>]
    let getAvailableLanguages =
        async {
            return Directory.EnumerateFiles("Internationalization") |> List.ofSeq
        }


    [<Remote>]
    let getCurrentUserEmail () =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with
            | Some userId ->
                let! userEmail = getEmailByUserId userId
                return userEmail |> Option.get
            | None -> return failwith "Nobody is logged in"
        }



    let sendEmail fromAddress fromName toAddress subject body (attachmentPathsAndNames : list<string * string>) =
        use smtpClient = new SmtpClient(ConfigurationManager.AppSettings.["email_server"], ConfigurationManager.AppSettings.["email_port"] |> Int32.TryParse |> snd)
        smtpClient.EnableSsl <- true
        smtpClient.Credentials <- new System.Net.NetworkCredential(ConfigurationManager.AppSettings.["email_username"], ConfigurationManager.AppSettings.["email_password"])
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
                return Database.getUserValues dbConn userId
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
                    let insertedUserValuesId = Database.setUserValues dbConn userValues userId
                    transaction.Commit()
                    return ok "User values have been updated."
                with
                | e ->
                    transaction.Rollback()
                    return fail "Setting user values failed"
            | None -> return fail "Please login."
        }

    
    [<Remote>]
    let addEmployer (employer : Employer) =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with
            | Some userId ->
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                use transaction = dbConn.BeginTransaction()
                try
                    let addedEmployerId = Database.addEmployer dbConn employer userId
                    transaction.Commit()
                    return ok addedEmployerId
                with
                | e ->
                    transaction.Rollback()
                    return fail "Adding employer failed"
            | None -> return fail "Please login first"
        }


    open System.Security.Cryptography
    open WebSharper.Html.Server.Tags
    open System.Net.Mime
    open System.Security.Claims

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
        use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
        dbConn.Open()
        match Database.getIdPasswordSaltAndGuid dbConn email with
        | Some (userId, hashedPassword, salt, None) ->
            if generateHash password salt 1000 64 = hashedPassword
            then
                GetContext().UserSession.LoginUser(string userId) |> Async.RunSynchronously
                async {
                    return ok <| string userId
                }
            else
                async {
                    return fail "Email or password wrong."
                }
        |  Some (_, _, _, Some guid) ->
            async {
                return fail "Please confirm your email"
            }
        | None ->
            async {
                return fail "Email is unknown"
            }


    [<Remote>]
    let register (email : string) (password1 : string) =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            if Database.userEmailExists dbConn email
            then
                return fail "Email already exists"
            else
                let salt = generateSalt(64)
                let hashedPassword = generateHash password1 salt 1000 64
                let guid = Guid.NewGuid().ToString()
                let dict = Deutsch.dict |> Map.ofList
                sendEmail
                    "rene.ederer.nbg@gmail.com"
                    "bewerbungsspam.de"
                    email
                    dict.[PleaseConfirmYourEmailAddressEmailSubject]
                    (String.Format(dict.[PleaseConfirmYourEmailAddressEmailBody], email, guid))
                    []
                Database.insertNewUser dbConn email hashedPassword salt guid |> ignore
                return ok "Please confirm your email"
        }

    [<Remote>]
    let confirmEmail email guid =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            let oDbGuidOpt = Database.getGuid dbConn email
            match oDbGuidOpt with
            | None -> return ok "Email already confirmed"
            | Some dbGuid when guid = dbGuid ->
                use transaction = dbConn.BeginTransaction()
                try
                    Database.setGuidToNull dbConn email
                    transaction.Commit()
                    return ok "Email confirmed"
                with
                | e ->
                    transaction.Rollback()
                    return failwith "Setting guid to null failed"
            | Some _ ->
                return fail "Unknown guid"

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
                let result =  Database.getDocumentNames dbConn userId
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
                File.Delete filePath
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
                return Database.getDocumentOffset dbConn userId htmlJobApplicationOffset
            | None -> return failwith "No user logged in"
        }
    
    [<Remote>]
    let replaceMap (userEmail : string) (userValues : UserValues) (employer : Employer) (document : Document) =
        async {
            return
                [ ("$firmaName", employer.company)
                  ("$firmaStrasse", employer.street)
                  ("$firmaPlz", employer.postcode)
                  ("$firmaStadt", employer.city)
                  ("$chefAnredeBriefkopf", match employer.gender with Gender.Male -> "Herrn" | Gender.Female -> "Frau" | Gender.Unknown -> "")
                  ("$chefAnrede", match employer.gender with Gender.Male -> "Herr" | Gender.Female -> "Frau" | Gender.Unknown -> "")
                  ("$geehrter", match employer.gender with Gender.Male -> "geehrter" | Gender.Female -> "geehrte" | Gender.Unknown -> "")
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
                  ("$meinTelefon", userValues.phone)
                  ("$meinMobilTelefon", userValues.mobilePhone)
                  ("$meineMobilnummer", userValues.mobilePhone)
                  ("$meineMobilnr", userValues.mobilePhone)
                  ("$datumHeute", DateTime.Today.ToString("dd.MM.yyyy"))
                  ("$beruf", document.jobName)
                ]
        }

    [<Remote>]
    let replaceVariables (filePath : string) (userValues : UserValues) (employer : Employer) (document : Document)=
        match getCurrentUserId() |> Async.RunSynchronously with
        | Some userId ->
            async {
                let! userEmail = getEmailByUserId userId
                let guid = Guid.NewGuid().ToString("N")
                let! map = replaceMap (userEmail |> Option.defaultValue "") userValues employer document
                Directory.CreateDirectory(sprintf "tmp/%s" guid) |> ignore
                if filePath.EndsWith(".odt") || filePath.EndsWith(".docx")
                then
                    return Odt.replaceInOdt filePath (sprintf "tmp/%s/extracted/" guid) (sprintf "tmp/%s/replaced/" guid) map
                else
                    let newFilePath = sprintf "tmp/%s/%s" guid (Path.GetFileName(filePath))
                    File.Copy(filePath, newFilePath, true)
                    Odt.replaceInFile newFilePath map Ignore
                    return newFilePath
            }

        | None -> async { return "" }
         


    [<Remote>]
    let applyNow (employer : Employer) (document : Document) (userValues : UserValues) =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with 
            | None -> return failwith "No user logged in"
            | Some userId ->
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                use transaction = dbConn.BeginTransaction()
                try
                    let documentId = Database.overwriteDocument dbConn document userId
                    let employerId = Database.addEmployer dbConn employer userId
                    Database.insertSentApplication dbConn userId employerId document.jobName
                    let userEmail = Database.getEmailByUserId dbConn userId |> Option.defaultValue ""
                    Database.setUserValues dbConn userValues userId |> ignore
                    let! myList = replaceMap userEmail userValues employer document
                    let tmpPath = Path.Combine("tmp", Guid.NewGuid().ToString("N"))
                    let odtPaths =
                        [ for item in document.pages do
                            match item with
                            | HtmlPage htmlPage ->
                                let oPageTemplatePath = Option.map (Database.getHtmlPageTemplatePath dbConn) htmlPage.oTemplateId
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
                                        filePage.path
                                    elif filePage.path.EndsWith(".odt") || filePage.path.EndsWith(".docx")
                                    then
                                        let directoryGuid = Guid.NewGuid().ToString("N")
                                        Odt.replaceInOdt
                                            filePage.path
                                            (Path.Combine(tmpPath, directoryGuid, "replacedOdt"))
                                            (Path.Combine(tmpPath, directoryGuid, "extractedOdt"))
                                            myList
                                    else
                                        let copiedPath = Path.Combine(tmpPath, Guid.NewGuid().ToString("N"), Path.GetFileName(filePage.path))
                                        Directory.CreateDirectory(Path.GetDirectoryName copiedPath) |> ignore
                                        File.Copy(filePage.path, copiedPath)
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
                        "rene.ederer.nbg@gmail.com" //employer.email
                        (Odt.replaceInString document.email.subject myList Ignore)
                        (Odt.replaceInString (document.email.body.Replace("\\r\\n", "\r\n").Replace("\\n", "\n")) myList Ignore)
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
            return Database.getHtmlPageTemplates dbConn
        }
    
    [<Remote>]
    let getHtmlPageTemplate (templateId : int) =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.getHtmlPageTemplate dbConn templateId
        }
    
    [<Remote>]
    let getHtmlPages documentId =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.getHtmlPages dbConn documentId
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
    let getLastEditedDocumentId () =
        match getCurrentUserId () |> Async.RunSynchronously with
        | None -> failwith "Nobody logged in"
        | Some userId ->
            async {
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                return Database.getLastEditedDocumentId dbConn userId
            }

    [<Remote>]
    let setLastEditedDocumentId (userId) (documentId : int) =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.setLastEditedDocumentId dbConn userId documentId
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
                return Database.getDocumentIdOffset dbConn userId documentIndex
            }
    
    [<Remote>]
    let readWebsite url =
        async {
            return Website.read url
        }
    
    [<Remote>]
    let createLink filePath name =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.createLink dbConn filePath name
        }

    [<Remote>]
    let getPathAndNameByGuid guid =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.getPathAndNameByGuid dbConn guid
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
                return Database.getSentApplications dbConn userId startDate endDate
            | None -> 
                return failwith "Nobody is logged in"
        }

    let filesWithSameExtension filePath userId =
        async {
            let extension = Path.GetExtension(filePath)
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.getFilesWithExtension dbConn extension userId
        }

    let getFilePageNames (documentId : int) =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.getFilePageNames dbConn documentId
        }
