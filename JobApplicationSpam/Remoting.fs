
namespace JobApplicationSpam

open WebSharper
open Npgsql
open Chessie.ErrorHandling
open System
open JobApplicationSpam.Types
open System.Configuration
module DB = JobApplicationSpam.Database


module Server =
    open System.Web
    open System.Net.Mail
    open System.IO
    open WebSharper.Web.Remoting

    [<Remote>]
    let getEmailByUserId userId =
        async {
            use dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
            dbConn.Open()
            return Database.getEmailByUserId dbConn userId
        }

    [<Remote>]
    let getCurrentUserId () =
        GetContext().UserSession.GetLoggedInUser()
        |> Async.RunSynchronously
        |> Option.map (Int32.TryParse)
        |> Option.bind (fun (parsed, v) -> if parsed then Some v else None)

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
    let setUserValues (userValues : UserValues) =
        let oUserId = getCurrentUserId()
        async {
            match oUserId with
            | Some userId ->
                use dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
                dbConn.Open()
                use command = new NpgsqlCommand("select email from users where id = " + userValues.degree, dbConn)
                try
                    match command.ExecuteScalar() with
                    | null -> return fail "No record found"
                    | v -> return ok <| HttpContext.Current.Session.["email"].ToString()
                with
                | _ ->
                    return fail "An error occured while trying to set user values."
            | None -> return fail "Please login."
        }


    let userEmailExists (dbConn : NpgsqlConnection) (email : string) =
        use command = new NpgsqlCommand("select count(*) from users where email = :email", dbConn)
        command.Parameters.Add(new NpgsqlParameter("email", email)) |> ignore
        try
            ok ((command.ExecuteScalar() |> string |> Int32.Parse) = 1)
        with
        | :? PostgresException
        | _ ->
            fail "An error occured while checking if email exists"
    
    [<Remote>]
    let emailExists email = 
        async {
            use dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
            dbConn.Open()
            let emailExists = userEmailExists dbConn email
            let resultStr =
                match emailExists with
                | Ok (true, _) -> sprintf "Email %s does exist." email
                | Ok (false, _) -> sprintf "Email %s does not exist" email
                | Bad _ -> "an error occurred"
            return resultStr
        }

    
    [<Remote>]
    let addEmployer (employer : Employer) =
        let oUserId = getCurrentUserId()
        let addEmployerDB (dbConn : NpgsqlConnection) (employer : Employer) (userId : int) =
            use command = new NpgsqlCommand("insert into employer (userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(:userId, :company, :street, :postcode, :city, :gender, :degree, :firstName, :lastName, :email, :phone, :mobilePhone) returning id", dbConn)
            command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("company", employer.company)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("street", employer.street)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("postcode", employer.postcode)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("city", employer.city)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("gender", if employer.gender = Gender.Male then 'm' else 'f')) |> ignore
            command.Parameters.Add(new NpgsqlParameter("degree", employer.degree)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("firstName", employer.firstName)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("lastName", employer.lastName)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("email", employer.email)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("phone", employer.phone)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("mobilePhone", employer.mobilePhone)) |> ignore
            try
                ok (command.ExecuteScalar() |> string |> Int32.Parse)
            with
            | (e : Exception) ->
                fail ("An error occured while trying to add employer." + e.Message)
        async {
            use dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
            dbConn.Open()
            match oUserId with
            | Some userId -> return addEmployerDB dbConn employer userId
            | None -> return fail "Please login first"
        }

    [<Remote>]
    let applyNow (employerId : int) (templateId : int) =
        let oUserId = getCurrentUserId() 
        async {
            match oUserId with 
            | None -> return fail "Please login first"
            | Some userId ->
                use dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
                dbConn.Open()
                let employerResult = Database.getEmployer dbConn employerId
                let templateResult = Database.getTemplateForJobApplication dbConn templateId
                match employerResult, templateResult with
                | Ok (employer, _), Ok (template, _) ->
                    use command = new NpgsqlCommand("insert into jobApplication (userId, employerId, jobApplicationTemplateId) values(:userId, :employerId, :jobApplicationTemplateId) returning id", dbConn)
                    command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
                    command.Parameters.Add(new NpgsqlParameter("employerId", employerId)) |> ignore
                    command.Parameters.Add(new NpgsqlParameter("jobApplicationTemplateId", templateId)) |> ignore
                    let jobApplicationId = command.ExecuteScalar() |> string |> Int32.Parse
                    command.Dispose()
                    use command = new NpgsqlCommand("insert into jobApplicationStatus (jobApplicationId, statusChangedOn, dueOn, statusValueId, statusMessage) values(:jobApplicationId, current_date, null, 1, '')", dbConn)
                    command.Parameters.Add(new NpgsqlParameter("jobApplicationId", jobApplicationId)) |> ignore
                    command.ExecuteNonQuery ()|> ignore
                    let myMap =
                        [ ("$firmaName", employer.company)
                          ("$firmaStrasse", employer.street)
                          ("$firmaPlz", employer.postcode)
                          ("$firmaStadt", employer.city)
                          ("$chefAnredeBriefkopf", match employer.gender with Gender.Male -> "Herrn" | Gender.Female -> "Frau")
                          ("$chefAnrede", match employer.gender with Gender.Male -> "Herr" | Gender.Female -> "Frau")
                          ("$geehrter", match employer.gender with Gender.Male -> "geehrter" | Gender.Female -> "geehrte")
                          ("$chefTitel", employer.degree)
                          ("$chefVorname", employer.firstName)
                          ("$chefNachname", employer.lastName)
                          ("$chefEmail", employer.email)
                          ("$chefTelefon", employer.phone)
                          ("$chefMobil", employer.mobilePhone)
                          ("$datumHeute", DateTime.Today.ToString("dd.MM.yyyy"))
                        ] |> Map.ofList
                    Odt.replaceInOdt template.filePaths.[0] "c:/users/rene/myodt/" "c:/users/rene/myodt1/m1.odt" myMap
                    //sendEmail "rene.ederer.nbg@gmail.com" "René Ederer" employer.email template.emailSubject template.emailBody template.pdfPaths
                    return ok (Odt.odtToPdf "c:/users/rene/myodt1/m1.odt")
                | Ok _, Bad vs ->
                    failwith (String.concat ", " vs)
                    return fail "An error occured while trying to upload template"
                | Bad _, Ok _ ->
                    failwith "soeerr"
                    return fail "An error occured while trying to upload template"
                | Bad xs, Bad ys ->
                    failwith ((String.concat ", " xs) +  "::::" + (String.concat ", " ys))
                    return fail "An error occured while trying to upload template"
            }
    
    [<Remote>]
    let applyNowByTemplateName (employerId : int) (templateName : string) =
        let oUserId = getCurrentUserId() 
        async {
            match oUserId with 
            | None -> return fail "Please login first"
            | Some userId ->
                use dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
                dbConn.Open()
                let employerResult = Database.getEmployer dbConn employerId
                let templateResult = Database.getTemplateForJobApplicationByTemplateName dbConn userId templateName
                match employerResult, templateResult with
                | Ok (employer, _), Ok ((templateId, template), _) ->
                    use command = new NpgsqlCommand("insert into jobApplication (userId, employerId, jobApplicationTemplateId) values(:userId, :employerId, :jobApplicationTemplateId) returning id", dbConn)
                    command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
                    command.Parameters.Add(new NpgsqlParameter("employerId", employerId)) |> ignore
                    command.Parameters.Add(new NpgsqlParameter("jobApplicationTemplateId", templateId)) |> ignore
                    let jobApplicationId = command.ExecuteScalar() |> string |> Int32.Parse
                    command.Dispose()
                    use command = new NpgsqlCommand("insert into jobApplicationStatus (jobApplicationId, statusChangedOn, dueOn, statusValueId, statusMessage) values(:jobApplicationId, current_date, null, 1, '')", dbConn)
                    command.Parameters.Add(new NpgsqlParameter("jobApplicationId", jobApplicationId)) |> ignore
                    command.ExecuteNonQuery ()|> ignore
                    let myMap =
                        [ ("$firmaName", employer.company)
                          ("$firmaStrasse", employer.street)
                          ("$firmaPlz", employer.postcode)
                          ("$firmaStadt", employer.city)
                          ("$chefAnredeBriefkopf", match employer.gender with Gender.Male -> "Herrn" | Gender.Female -> "Frau")
                          ("$chefAnrede", match employer.gender with Gender.Male -> "Herr" | Gender.Female -> "Frau")
                          ("$geehrter", match employer.gender with Gender.Male -> "geehrter" | Gender.Female -> "geehrte")
                          ("$chefTitel", employer.degree)
                          ("$chefVorname", employer.firstName)
                          ("$chefNachname", employer.lastName)
                          ("$chefEmail", employer.email)
                          ("$chefTelefon", employer.phone)
                          ("$chefMobil", employer.mobilePhone)
                          ("$datumHeute", DateTime.Today.ToString("dd.MM.yyyy"))
                        ] |> List.sortByDescending (fun (x, _) -> x.Length) |> Map.ofList
                    let replacedOdtPath = Odt.replaceInOdt template.filePaths.[0] (Path.Combine(Environment.CurrentDirectory, "users/tmp_" + Guid.NewGuid().ToString())) (Path.Combine(Environment.CurrentDirectory, "users/" + userId.ToString() + "/" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + "_" + Guid.NewGuid().ToString())) myMap
                    //sendEmail "rene.ederer.nbg@gmail.com" "René Ederer" employer.email template.emailSubject template.emailBody template.pdfPaths
                    return ok (Odt.odtToPdf replacedOdtPath)
                | Ok _, Bad vs ->
                    failwith (String.concat ", " vs)
                    return fail "An error occured while trying to upload template"
                | Bad _, Ok _ ->
                    failwith "soeerr"
                    return fail "An error occured while trying to upload template"
                | Bad xs, Bad ys ->
                    failwith ((String.concat ", " xs) +  "::::" + (String.concat ", " ys))
                    return fail "An error occured while trying to upload template"
            }

    [<Remote>]
    let uploadTemplate (templateName : string) (userAppliesAs : string) (emailSubject : string) (emailBody : string) (filePaths : seq<string>) (oUserId : option<int>) =
        let addTemplate (dbConn : NpgsqlConnection) (userId : int) (templateName : string) (userAppliesAs : string) (emailSubject : string) (emailBody : string) =
            try
                use command = new NpgsqlCommand("insert into jobApplicationTemplate (userId, templateName, userAppliesAs, emailSubject, emailBody) values(:userId, :templateName, :userAppliesAs, :emailSubject, :emailBody) returning id", dbConn)
                command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("templateName", templateName)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("userAppliesAs", userAppliesAs)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("emailSubject", emailSubject)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("emailBody", emailBody)) |> ignore
                let id = command.ExecuteScalar () |> string |> Int32.Parse
                ok id
            with
            | (e : Exception) ->
                fail ("An error occured while trying to upload template" + e.Message)
        let addPdfPaths (dbConn : NpgsqlConnection) (templateId : int) (pdfPaths : seq<string>) =
            try
                for filePath in filePaths do
                    use command = new NpgsqlCommand("insert into jobApplicationTemplateFile (jobApplicationTemplateId, filePath) values (:jobApplicationTemplateId, :filePath)", dbConn)
                    command.Parameters.Add(new NpgsqlParameter("jobApplicationTemplateId", templateId)) |> ignore
                    command.Parameters.Add(new NpgsqlParameter("filePath", filePath)) |> ignore
                    command.ExecuteNonQuery () |> ignore
                ok "Template has been uploaded."
            with
            | (e : Exception) ->
                fail ("An error occured while trying to upload template" + e.Message)
        async {
            match oUserId with
            | None -> return fail "Please login first"
            |Some userId ->
                use dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
                dbConn.Open()
                use transaction = dbConn.BeginTransaction()
                let result =
                    trial {
                        let! addOdtResult = addTemplate dbConn userId templateName userAppliesAs emailSubject emailBody
                        return! addPdfPaths dbConn addOdtResult filePaths
                    }
                match result with
                | Ok _ ->
                    transaction.Commit()
                | Bad _ ->
                    transaction.Rollback()
                return result
        }

    
    open System.Security.Cryptography

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
        use dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
        dbConn.Open()
        match (Database.getIdPasswordSaltAndGuid dbConn email) with
        | Bad v -> 
             fail "Email or password wrong."
        | Ok ((userId, hashedPassword, salt, None), _) ->
            if generateHash password salt 1000 64 = hashedPassword
            then
                GetContext().UserSession.LoginUser(string userId) |> Async.RunSynchronously
                ok <| string userId
            else fail "Email or password wrong."
        | Ok ((_, _, _, Some guid), _) ->
            fail "Please confirm your email"

    [<Remote>]
    let register email (password1 : string) password2 =
        use dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
        dbConn.Open()
        match (Database.userEmailExists dbConn email), password1, password2 with
        | _ when password1 <> password2 -> fail "Passwords are not equal"
        | Bad v, _, _ -> Bad v
        | Ok (true, _), _, _ -> fail "Email already exists"
        | Ok (false, _), _, _ -> 
            let salt = generateSalt(64)
            let hashedPassword = generateHash password1 salt 1000 64
            let guid = Guid.NewGuid().ToString()
            match Database.insertNewUser dbConn email hashedPassword salt guid with
            | Bad v ->
                Bad v
            | Ok _ ->
                sendEmail
                    "rene.ederer.nbg@gmail.com"
                    "bewerbungsspam.de"
                    email
                    "Please confirm your email address"
                    ("Dear user,\n\nplease visit this link to confirm your email address.\nhttp://bewerbungsspam.de/confirmEmail?email=rene.ederer.nbg@gmail.com&guid=" + guid + "\nPlease excuse the inconvenience.\n\nYour team from www.bewerbungsspam.de")
                    []
                ok ()

    [<Remote>]
    let confirmEmail email guid =
        trial {
            use dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
            dbConn.Open()
            let! dbGuidOpt = Database.getGuid dbConn email
            match dbGuidOpt with
            | None -> return! (fail "Email already confirmed")
            | Some dbGuid when guid = dbGuid ->
                return! Database.setGuidToNull dbConn email
            | Some _ ->
                return! fail "Unknown guid"

        }

    [<Remote>]
    let getTemplateNames () =
        use dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
        dbConn.Open()
        match getCurrentUserId () with
        | Some userId ->
            Database.getTemplateNames dbConn userId
        | None -> failwith "User is not logged in"






