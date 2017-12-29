
namespace JobApplicationSpam

open WebSharper
open Npgsql
open Chessie.ErrorHandling
open System
open JobApplicationSpam.Types
open System.Configuration


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
                    let insertedUserValuesId = Database.addUserValues dbConn userValues userId
                    transaction.Commit()
                    return ok "User values have been updated."
                with
                | e ->
                    transaction.Rollback()
                    return fail "Inserting user values failed"
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
                ok <| string userId
            else
                fail "Email or password wrong."
        |  Some (_, _, _, Some guid) ->
            fail "Please confirm your email"
        | None ->
            fail "Email is unknown"


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
    let getHtmlJobApplicationNames () =
        log.Debug "()"
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with
            | Some userId ->
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                let result =  Database.getHtmlJobApplicationNames dbConn userId
                log.Debug "() = ()"
                return result
            | None ->
                log.Error "No user logged in"
                return  failwith "No user logged in"
        }


    [<Remote>]
    let saveHtmlJobApplication (htmlJobApplication : HtmlJobApplication) =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with
            | Some userId ->
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                use transaction = dbConn.BeginTransaction()
                try
                    let htmlJobApplicationId = Database.saveHtmlJobApplication dbConn htmlJobApplication userId
                    transaction.Commit()
                    return htmlJobApplicationId
                with
                | e ->
                    transaction.Rollback()
                    log.Error("Saving HtmlJobApplication failed", e)
                    return failwith "Saving HtmlJobApplication failed"
            | None -> return failwith "User is not logged in"
        }


    [<Remote>]
    let getHtmlJobApplicationOffset (htmlJobApplicationOffset : int) =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        async {
            match oUserId with
            | Some userId ->
                use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
                dbConn.Open()
                return Database.getHtmlJobApplicationOffset dbConn userId htmlJobApplicationOffset
            | None -> return failwith "No user logged in"
        }



    [<Remote>]
    let applyNowWithHtmlTemplate (employer : Employer) (employerId : int) (htmlJobApplication : HtmlJobApplication) (userValues : UserValues) =
        let oUserId = getCurrentUserId() |> Async.RunSynchronously
        //async {
        match oUserId with 
        | None -> failwith "No user loggeg in"
        | Some userId ->
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            use transaction = dbConn.BeginTransaction()
            try
                let htmlJobApplicationId = Database.saveHtmlJobApplication dbConn htmlJobApplication userId
                Database.insertJobApplication dbConn userId employerId htmlJobApplicationId
                let odtPaths =
                    [ for currentPage in htmlJobApplication.pages do
                        let htmlJobApplicationPageTemplatePath = Database.getHtmlJobApplicationPageTemplatePath dbConn currentPage.jobApplicationPageTemplateId
                        let userEmail = Database.getEmailByUserId dbConn userId |> Option.defaultValue ""
                        let lines =
                            let emptyLines = List.init 50 (fun i -> sprintf "$line%i" (i + 1), "")
                            let currentPageLines = currentPage.map.["mainText"].Split([|'\n'|])
                            let len = currentPageLines.Length
                            (currentPageLines
                            |> Array.mapi (fun i x -> sprintf "$line%i" (i + 1), x)
                            |> List.ofArray)
                            @ List.skip len emptyLines

                        let myList =
                            (
                            [ ("$firmaName", employer.company)
                              ("$firmaStrasse", employer.street)
                              ("$firmaPlz", employer.postcode)
                              ("$firmaStadt", employer.city)
                              ("$chefAnredeBriefkopf", match employer.gender with Gender.Male -> "Herrn" | Gender.Female -> "Frau")
                              ("$chefAnrede", employer.gender.ToString())
                              ("$geehrter", match employer.gender with Gender.Male -> "geehrter" | Gender.Female -> "geehrte")
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
                            ] @ lines)
                            |> List.sortByDescending (fun (key, _) -> key.Length)
                        let tmpPath = "c:/users/rene/myodt1/" + Guid.NewGuid().ToString()
                        yield Odt.replaceInOdt htmlJobApplicationPageTemplatePath "c:/users/rene/myodt/" tmpPath myList
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
        //}
    
    [<Remote>]
    let getHtmlJobApplicationPageTemplates () =
        async {
            use dbConn = new NpgsqlConnection(ConfigurationManager.AppSettings.["dbConnStr"])
            dbConn.Open()
            return Database.getHtmlJobApplicationPageTemplates dbConn
        }
