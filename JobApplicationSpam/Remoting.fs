
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
    open WebSharper.Owin.EnvKey.WebSharper
    open System.IO

    let sendEmail fromAddress fromName toAddress subject body attachmentPaths =
        use smtpClient = new SmtpClient(ConfigurationManager.AppSettings.["email_server"], ConfigurationManager.AppSettings.["email_port"] |> Int32.TryParse |> snd)
        smtpClient.EnableSsl <- true
        smtpClient.Credentials <- new System.Net.NetworkCredential(ConfigurationManager.AppSettings.["email_username"], ConfigurationManager.AppSettings.["email_password"])
        let fromAddress = new MailAddress(fromAddress, fromName, System.Text.Encoding.UTF8)
        let toAddress = new MailAddress(toAddress)
        let message = new MailMessage(fromAddress, toAddress, SubjectEncoding = System.Text.Encoding.UTF8, Subject = subject, Body = body, BodyEncoding = System.Text.Encoding.UTF8)
        attachmentPaths
        |> List.iter (fun x -> message.Attachments.Add(new Attachment(x)))
        //smtpClient.Send(message)

    [<Remote>]
    let setSessionEmail (email : string) =
        HttpContext.Current.Session.Add("email", email)

    [<Remote>]
    let setUserValues (userValues : UserValues) (userId : int) : Async<Result<string, string>> =
        async {
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
        }


    let userEmailExists (dbConn : NpgsqlConnection) (email : string) =
        use command = new NpgsqlCommand("select count(*) from users where email = :email", dbConn)
        command.Parameters.Add(new NpgsqlParameter("email", email)) |> ignore
        try
            ok (Int32.Parse(command.ExecuteScalar().ToString()) = 1)
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
    let addUser (email : string) (password : string) (salt : string) (guid : string) =
        ()
        (*
        let addUserDB (dbConn : NpgsqlConnection) (email : string) (password : string) (salt : string) (guid : string) =
            use command = new NpgsqlCommand("insert into users (email, password, salt, guid) values(:email, :password, :salt, :guid)", dbConn)
            command.Parameters.Add(new NpgsqlParameter("email", email)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("password", password)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("salt", salt)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("guid", guid)) |> ignore
            try
                command.ExecuteNonQuery () |> ignore
                ok "User has been added."
            with
            | :? PostgresException
            | _ ->
                fail "An error occured while trying to add user."
        use dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
        dbConn.Open()
        addUserDB dbConn email password salt guid
        *)
    
    [<Remote>]
    let addEmployer (employer : Employer) (userId : int) =
        let addEmployerDB (dbConn : NpgsqlConnection) (employer : Employer) (userId : int) =
            use command = new NpgsqlCommand("insert into employer (userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone) values(:userId, :company, :street, :postcode, :city, :gender, :degree, :firstName, :lastName, :email, :phone, :mobilePhone)", dbConn)
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
                command.ExecuteNonQuery () |> ignore
                ok "Employer has been added."
            with
            | (e : Exception) ->
                fail ("An error occured while trying to add employer." + e.Message)
        async {
            use dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
            dbConn.Open()
            return addEmployerDB dbConn employer userId
        }

    [<Remote>]
    let applyNow (userId : int) (employerId : int) (templateId : int) =
        async {
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
                Odt.replaceInOdt template.odtPath "c:/users/rene/myodt/" "c:/users/rene/myodt1/m1.odt" myMap
                sendEmail "rene.ederer.nbg@gmail.com" "René Ederer" employer.email template.emailSubject template.emailBody template.pdfPaths
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
    let uploadTemplate (userId : int) (templateName : string) (userAppliesAs : string) (emailSubject : string) (emailBody : string) (odtPath : string) (pdfPaths : seq<string>) =
        let addOdt (dbConn : NpgsqlConnection) (userId : int) (templateName : string) (userAppliesAs : string) (emailSubject : string) (emailBody : string) (odtPath : string) =
            try
                use command = new NpgsqlCommand("insert into jobApplicationTemplate (userId, templateName, userAppliesAs, emailSubject, emailBody, odtPath) values(:userId, :templateName, :userAppliesAs, :emailSubject, :emailBody, :odtPath) returning id", dbConn)
                command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("templateName", templateName)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("userAppliesAs", userAppliesAs)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("emailSubject", emailSubject)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("emailBody", emailBody)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("odtPath", odtPath)) |> ignore
                let id = command.ExecuteScalar () |> string |> Int32.Parse
                ok id
            with
            | (e : Exception) ->
                fail ("An error occured while trying to upload template" + e.Message)
        let addPdfPaths (dbConn : NpgsqlConnection) (templateId : int) (pdfPaths : seq<string>) =
            try
                for pdfPath in pdfPaths do
                    use command = new NpgsqlCommand("insert into jobApplicationPdfAppendix (jobApplicationTemplateId, pdfPath) values (:jobApplicationTemplateId, :pdfPath)", dbConn)
                    command.Parameters.Add(new NpgsqlParameter("jobApplicationTemplateId", templateId)) |> ignore
                    command.Parameters.Add(new NpgsqlParameter("pdfPath", pdfPath)) |> ignore
                    command.ExecuteNonQuery () |> ignore
                ok "Employer has been added."
            with
            | (e : Exception) ->
                fail ("An error occured while trying to upload template" + e.Message)
        async {
            use dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
            dbConn.Open()
            use transaction = dbConn.BeginTransaction()
            let result =
                trial {
                    let! addOdtResult = addOdt dbConn userId templateName userAppliesAs emailSubject emailBody odtPath
                    return! addPdfPaths dbConn addOdtResult pdfPaths
                }
            match result with
            | Ok _ ->
                transaction.Commit()
            | Bad _ ->
                transaction.Rollback()
            return result
        }








