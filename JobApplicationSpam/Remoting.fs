
namespace JobApplicationSpam

open WebSharper
open Npgsql
open Chessie.ErrorHandling
open System
open JobApplicationSpam.Types
open System.Configuration
open Website


module Server =
    open System.Web
    open System.Net.Mail
    open System.IO
    open WebSharper.Web.Remoting

    let log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().GetType())

    [<Remote>]
    let getEmailByUserId userId =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.getEmailByUserId dbConn userId
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
                return userEmail |> Option.get
            | None -> return failwith "Nobody is logged in"
        }



    let sendEmail fromAddress fromName toAddress subject body attachmentPaths =
        use smtpClient = new SmtpClient(ConfigurationManager.AppSettings.["email_server"], ConfigurationManager.AppSettings.["email_port"] |> Int32.TryParse |> snd)
        smtpClient.EnableSsl <- true
        smtpClient.Credentials <- new System.Net.NetworkCredential(ConfigurationManager.AppSettings.["email_username"], ConfigurationManager.AppSettings.["email_password"])
        let fromAddress = new MailAddress(fromAddress, fromName, System.Text.Encoding.UTF8)
        let toAddress = new MailAddress(toAddress)
        let message = new MailMessage(fromAddress, toAddress, SubjectEncoding = System.Text.Encoding.UTF8, Subject = subject, Body = body, BodyEncoding = System.Text.Encoding.UTF8)
        attachmentPaths
        |> List.iter (fun x -> message.Attachments.Add(new Attachment(x)))
        smtpClient.Send(message)
    
    [<Remote>]
    let getCurrentUserValues () =
        match getCurrentUserId() |> Async.RunSynchronously with
        | Some userId ->
            async {
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                match Database.getUserValues dbConn userId with
                | Some userValues -> return userValues
                | None -> return failwith "An error occured" 
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
    let register email (password1 : string) password2 =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            if password1 <> password2
            then
                return fail "Passwords are not equal"
            elif not <| Database.userEmailExists dbConn email
            then
                return fail "Email already exists"
            else
                let salt = generateSalt(64)
                let hashedPassword = generateHash password1 salt 1000 64
                let guid = Guid.NewGuid().ToString()
                sendEmail
                    "rene.ederer.nbg@gmail.com"
                    "bewerbungsspam.de"
                    email
                    "Please confirm your email address"
                    ("Dear user,\n\nplease visit this link to confirm your email address.\nhttp://bewerbungsspam.de/confirmemail?email=rene.ederer.nbg@gmail.com&guid=" + guid + "\nPlease excuse the inconvenience.\n\nYour team from www.bewerbungsspam.de")
                    []
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
    let applyNowWithHtmlTemplate (employer : Employer) (document : Document) (userValues : UserValues) =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with 
            | None -> failwith "No user loggeg in"
            | Some userId ->
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                use transaction = dbConn.BeginTransaction()
                try
                    let documentId = Database.overwriteDocument dbConn document userId
                    let employerId = Database.addEmployer dbConn employer userId
                    Database.insertSentApplication dbConn userId employerId documentId
                    let userEmail = Database.getEmailByUserId dbConn userId |> Option.defaultValue ""
                    Database.setUserValues dbConn userValues userId |> ignore
                    let myList =
                        [ ("$firmaName", employer.company)
                          ("$firmaStrasse", employer.street)
                          ("$firmaPlz", employer.postcode)
                          ("$firmaStadt", employer.city)
                          ("$chefAnredeBriefkopf", match employer.gender with Gender.Male -> "Herrn" | Gender.Female -> "Frau" | Gender.Unknown -> "")
                          ("$chefAnrede", employer.gender.ToString())
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
                          ("$meineStadt", userValues.city)
                          ("$meineEmail", userEmail)
                          ("$meinMobilTelefon", userValues.mobilePhone)
                          ("$meineTelefonnr", userValues.phone)
                          ("$datumHeute", DateTime.Today.ToString("dd.MM.yyyy"))
                        ]
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
                                let tmpPath = "c:/users/rene/myodt1/" + Guid.NewGuid().ToString()
                                yield Odt.replaceInOdt pageTemplatePath "c:/users/rene/myodt/" tmpPath (myList @ lines)
                            | FilePage filePage ->
                                let tmpPath = "c:/users/rene/myodt1/" + Guid.NewGuid().ToString()
                                yield
                                    if filePage.path.EndsWith ".pdf"
                                    then
                                        filePage.path
                                    else
                                        Odt.replaceInOdt filePage.path "c:/users/rene/myodt/" tmpPath myList
                        ]
                    let pdfPaths =
                        [ for odtPath in odtPaths do
                            yield Odt.odtToPdf odtPath
                        ]
                    Odt.mergePdfs pdfPaths "c:/users/rene/myodt1/mygreatpdf.pdf"
                    transaction.Commit()
                    //return ()
                with
                | e ->
                    log.Error ("", e)
                    transaction.Rollback()
                    //sendEmail "rene.ederer.nbg@gmail.com" "René Ederer" employer.email template.emailSubject template.emailBody template.pdfPaths
                    //return ()
        }

    [<Remote>]
    let addFilePage documentId path pageIndex =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.addFilePage dbConn documentId path pageIndex
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
    let valuesMap userValues employer =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        match oUserId with
        | None -> failwith "No user logged in"
        | Some userId ->
            async {
                let! userEmail = (getEmailByUserId userId)
                return
                  [ "$firmaName", employer.company
                    "$firmaStrasse", employer.street
                    "$firmaPlz", employer.postcode
                    "$firmaStadt", employer.city
                    "$chefAnredeBriefkopf", match employer.gender with Gender.Male -> "Herrn" | Gender.Female -> "Frau" | Gender.Unknown -> ""
                    "$chefAnrede", employer.gender.ToString()
                    "$geehrter", match employer.gender with Gender.Male -> "geehrter" | Gender.Female -> "geehrte" | Gender.Unknown -> ""
                    "$chefTitel", employer.degree
                    "$chefVorname", employer.firstName
                    "$chefNachname", employer.lastName
                    "$chefEmail", employer.email
                    "$chefTelefon", employer.phone
                    "$chefMobil", employer.mobilePhone
                    "$meinGeschlecht", userValues.gender.ToString()
                    "$meinTitel", userValues.degree
                    "$meinVorname", userValues.firstName
                    "$meinNachname", userValues.lastName
                    "$meineStrasse", userValues.street
                    "$meinePlz", userValues.postcode
                    "$meineStadt", userValues.city
                    "$meineEmail", userEmail |> Option.get
                    "$meinMobilTelefon", userValues.mobilePhone
                    "$meineTelefonnr", userValues.phone
                    "$datumHeute", DateTime.Today.ToString("dd.MM.yyyy")
                ]
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
