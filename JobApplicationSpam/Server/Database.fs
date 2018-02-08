namespace JobApplicationSpam


module Database =
    open System
    open Npgsql
    open log4net
    open System.Linq
    open System.Reflection
    open FSharp.Data.Sql
    open Types
    open System.Linq


    [<Literal>]
    let connectionString = "Server=localhost; Port=5432; User Id=spam; Password=Steinmetzstr9!@#$; Database=jobapplicationspam"

    [<Literal>]
    let resolutionPath = "bin"


    type DB =
        SqlDataProvider<
            DatabaseVendor = FSharp.Data.Sql.Common.DatabaseProviderTypes.POSTGRESQL,
            ConnectionString = connectionString,
            Owner = "",
            ResolutionPath = resolutionPath,
            IndividualsAmount = 1000,
            UseOptionTypes = true>

     
    let getValueOrDBNull oV =
        match oV with
        | Some v -> v :> obj
        | None -> DBNull.Value :> obj


    let dbContext = DB.GetDataContext("Server=localhost; Port=5432; User Id=spam; Password=Steinmetzstr9!@#$; Database=jobapplicationspam")
    let db = dbContext.Public
    let log = LogManager.GetLogger(MethodBase.GetCurrentMethod().GetType())

    let getEmailByUserId (UserId userId) : option<string> =
        log.Debug(sprintf "(userId = %i)" userId)
        let oEmail = db.Users.Where(fun x -> x.Id = userId).Select(fun x -> x.Email).SingleOrDefault()
        log.Debug(sprintf "(userId = %i) = %A" userId oEmail)
        oEmail
        
    let getUserIdByEmail (email : string) : option<int> =
        log.Debug(sprintf "(email = %s)" email)
        let oUserId = db.Users.Where(fun x -> x.Email.IsSome && x.Email.Value = email).Select(fun x -> Some x.Id).SingleOrDefault()
        log.Debug(sprintf "(email = %s) = %A" email oUserId)
        oUserId

    let insertLastLogin dbConn (UserId userId) =
        log.Debug(sprintf "(userId = %i)" userId)
        use command = new NpgsqlCommand("insert into login (userId, loggedInAt) values (:userId, current_timestamp)", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.ExecuteNonQuery() |> ignore
        log.Debug(sprintf "(userId = %i) = ()" userId)
    
    let addEmployer (dbConn : NpgsqlConnection) (employer : Employer) (UserId userId) =
        log.Debug(sprintf "(employer = %A, userId = %i)" employer userId)
        use command =
            new NpgsqlCommand(
                """insert into employer
                (userId, company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone)
                values(:userId, :company, :street, :postcode, :city, :gender, :degree, :firstName, :lastName, :email, :phone, :mobilePhone)
                returning id""", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("company", employer.company)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("street", employer.street)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("postcode", employer.postcode)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("city", employer.city)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("gender", employer.gender.ToString())) |> ignore
        command.Parameters.Add(new NpgsqlParameter("degree", employer.degree)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("firstName", employer.firstName)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("lastName", employer.lastName)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("email", employer.email)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("phone", employer.phone)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("mobilePhone", employer.mobilePhone)) |> ignore
        let addedEmployerId = command.ExecuteScalar() |> string |> Int32.Parse
        log.Debug(sprintf "(employer = %A, userId = %i) = %i" employer userId addedEmployerId)
        addedEmployerId


    let getUserValues (UserId userId) =
        log.Debug(sprintf "(userId = %i)" userId)
        let userValues =
            query {
                for userValues in db.Uservalues do
                join user in db.Users on (userValues.Userid = user.Id)
                where (userValues.Userid = userId)
                select
                    { gender = Gender.fromString userValues.Gender
                      degree = userValues.Degree
                      firstName = userValues.Firstname
                      lastName = userValues.Lastname
                      street = userValues.Street
                      postcode = userValues.Postcode
                      city = userValues.City
                      phone = userValues.Phone
                      mobilePhone = userValues.Mobilephone
                    }
            } |> fun x -> x.Single()
        log.Debug(sprintf "(userId = %i) = %A" userId userValues)
        userValues

    let setUserValues (userValues : UserValues) (UserId userId) =
        log.Debug(sprintf "(userValues = %A, userId = %i)" userValues userId)
        db.Uservalues.Where(fun x -> x.Userid = userId).Single()
        |> fun x -> 
            x.Gender <- userValues.gender.ToString()
            x.Degree <- userValues.degree
            x.Firstname <- userValues.firstName
            x.Lastname <- userValues.lastName
            x.Street <- userValues.street
            x.Postcode <- userValues.postcode
            x.City <- userValues.city
            x.Phone <- userValues.phone
            x.Mobilephone <- userValues.mobilePhone
        dbContext.SubmitUpdates()
        log.Debug(sprintf "(userValues = %A, userId = %i) = ()" userValues userId)
        ()
    
    let setUserEmail dbConn (UserId userId) (email : string) =
        use command = new NpgsqlCommand("update users set email = :email where id = :userId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("email", email)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.ExecuteNonQuery() |> ignore
    
    let userEmailExists (email : string) =
        log.Debug(sprintf "(email = %s)" email)
        let emailExists = db.Users.Any(fun x -> x.Email.IsSome && x.Email.Value = email)
        log.Debug(sprintf "(email = %s) = %b" email emailExists)
        emailExists

    let getUserIdBySessionGuid (sessionGuid : string) : option<UserId> =
        log.Debug(sprintf "(sessionGuid = %s)" sessionGuid)
        db.Users.Where(fun x -> x.Sessionguid.IsSome && x.Sessionguid.Value = sessionGuid) |> Seq.iter (fun x -> x.Sessionguid |> (printfn "%A"))
        let oUserId =
            query {
                for user in db.Users do
                where (user.Sessionguid.IsSome && user.Sessionguid.Value = sessionGuid)
                select (Some user.Id)
            } |> fun x -> x.FirstOrDefault()
        log.Debug(sprintf "(sessionGuid = %s) = %A" sessionGuid oUserId)
        oUserId |> Option.map UserId
    
    let setSessionGuid dbConn (UserId userId) (sessionGuid : string) =
        log.Debug(sprintf "(userId = %i, sessionGuid = %s)" userId sessionGuid)
        use command = new NpgsqlCommand("update users set sessionGuid = :sessionGuid where id = :userId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("sessionGuid", sessionGuid)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.ExecuteNonQuery() |> ignore
        log.Debug(sprintf "(userId = %i, sessionGuid = %s) = ()" userId sessionGuid)

    let getValidateLoginData (email : string) : option<ValidateLoginData> =
        log.Debug(sprintf "(email = %s)" email)
        let ret =
            db.Users
              .Where(fun x -> x.Email.IsSome && x.Email.Value = email)
              .Select(fun x ->
                    Some { userId = UserId x.Id
                           userEmail = email
                           hashedPassword = x.Password
                           salt = x.Salt
                           confirmEmailGuid = x.Confirmemailguid}).SingleOrDefault()
        log.Debug(sprintf "(email = %s) = %A" email ret)
        ret
    
    let insertNewUser
            (dbConn : NpgsqlConnection)
            (oEmail : option<string>)
            (password : string)
            (salt : string)
            (oConfirmEmailGuid : option<string>)
            (oSessionGuid : option<string>) =
        log.Debug(sprintf "(oEmail = %A, password = %s, salt = %s, guid = %A, oSessionGuid = %A)"
                           oEmail
                           password
                           salt
                           oConfirmEmailGuid
                           oSessionGuid)
        use command = new NpgsqlCommand("""insert into users(email, password, salt, confirmEmailGuid, sessionGuid, createdOn)
                                         values(:email, :password, :salt, :confirmEmailGuid, :sessionGuid, current_date) returning id""", dbConn)
        command.Parameters.Add(new NpgsqlParameter("email", getValueOrDBNull oEmail)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("password", password)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("salt", salt)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("confirmEmailGuid", getValueOrDBNull oConfirmEmailGuid)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("sessionGuid", getValueOrDBNull oSessionGuid)) |> ignore
        let userId = command.ExecuteScalar() |> string |> Int32.Parse
        command.Dispose()
        use command = new NpgsqlCommand("""insert into userValues(userId, gender, degree, firstName,
                                         lastName, street, postcode, city, phone, mobilePhone)
                                         values(:userId, 'u', '', '', '', '', '', '', '', '')""", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.ExecuteNonQuery() |> ignore
        log.Debug
            (sprintf "(oEmail = %A, password = %s, salt = %s, oConfirmEmailGuid = %A, oSessionGuid = %A) = %i"
                     oEmail
                     password
                     salt
                     oConfirmEmailGuid
                     oSessionGuid
                     userId)
        UserId userId

    let getConfirmEmailGuid (email : string) : option<string> =
        log.Debug(sprintf "(email = %s)" email)
        let oConfirmEmailGuid = db.Users.Where(fun x -> x.Email.IsSome && x.Email.Value = email).Select(fun x -> x.Confirmemailguid).SingleOrDefault()
        log.Debug(sprintf "(email = %s) = %A" email oConfirmEmailGuid)
        oConfirmEmailGuid

    let getConfirmEmailGuidByUserId (UserId userId) : option<string> =
        log.Debug(sprintf "(userId = %i)" userId)
        let oConfirmEmailGuid = db.Users.Where(fun x -> x.Id = userId).Select(fun x -> x.Confirmemailguid).SingleOrDefault()
        log.Debug(sprintf "(userId = %i) = %A" userId oConfirmEmailGuid)
        oConfirmEmailGuid

    let setConfirmEmailGuidToNull (email : string) =
        log.Debug(sprintf "(email = %s)" email)
        let records = db.Users.Where(fun x -> x.Email.IsSome && x.Email.Value = email)
        records |> Seq.iter (fun x -> x.Confirmemailguid <- None)
        let recordCount = records.Count()
        log.Debug(sprintf "(email = %s) = %i" email recordCount)
        recordCount

    let insertNotYetSentApplication
            dbConn
            (UserId userId)
            (employerId : int)
            (email : DocumentEmail)
            (userEmailAndValues : string * UserValues)
            (sentFilePages : list<string * int>) // path * pageIndex
            (jobName : string)
            (url : string)
            (customVariables : string) =
        log.Debug(
            sprintf
                "(userId = %i ,employerId = %i ,email = %A, userEmailAndValues = %A, sentFilePages = %A, jobName = %s, url = %s, customVariables = %s)"
                userId
                employerId
                email
                userEmailAndValues
                sentFilePages
                jobName
                url
                customVariables
        )
        use command = new NpgsqlCommand("""insert into sentDocumentEmail(subject, body) values(:subject, :body) returning id""", dbConn)
        command.Parameters.Add(new NpgsqlParameter("subject", email.subject)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("body", email.body)) |> ignore
        let sentDocumentEmailId = command.ExecuteScalar() |> string |> Int32.Parse
        command.Dispose()

        let userValues = snd userEmailAndValues
        use command = new NpgsqlCommand("""insert into sentUserValues
                                           ( email, gender, degree, firstName, lastName,
                                             street, postcode, city, phone, mobilePhone)
                                           values(:email, :gender, :degree, :firstName, :lastName,
                                                  :street, :postcode, :city, :phone, :mobilePhone) returning id""", dbConn)
        command.Parameters.Add(new NpgsqlParameter("email", fst userEmailAndValues)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("gender", userValues.gender.ToString())) |> ignore
        command.Parameters.Add(new NpgsqlParameter("degree", userValues.degree)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("firstName", userValues.firstName)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("lastName", userValues.lastName)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("street", userValues.street)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("postcode", userValues.postcode)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("city", userValues.city)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("phone", userValues.phone)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("mobilePhone", userValues.mobilePhone)) |> ignore
        let sentUserValuesId = command.ExecuteScalar() |> string |> Int32.Parse
        command.Dispose()

        use command =
            new NpgsqlCommand("""insert into sentDocument(employerId, sentDocumentEmailId, sentUserValuesId, jobName, customVariables)
                                 values(:employerId, :sentDocumentEmailId, :sentUserValuesId, :jobName, :customVariables) returning id""", dbConn)
        command.Parameters.Add(new NpgsqlParameter("employerId",employerId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("sentDocumentEmailId", sentDocumentEmailId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("sentUserValuesId", sentUserValuesId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("jobName", jobName)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("customVariables", customVariables)) |> ignore
        let sentDocumentId = command.ExecuteScalar() |> string |> Int32.Parse
        command.Dispose()


        for (path, pageIndex) in sentFilePages do
            use command = new NpgsqlCommand("""insert into sentFilePage(sentDocumentId, path, pageIndex)
                                               values (:sentDocumentId, :path, :pageIndex)""", dbConn)
            command.Parameters.Add(new NpgsqlParameter("sentDocumentId", sentDocumentId)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("path", path)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("pageIndex", pageIndex)) |> ignore
            command.ExecuteNonQuery() |> ignore
            command.Dispose()

        use command = new NpgsqlCommand("insert into sentApplication(userId, sentDocumentId, url) values(:userId, :sentDocumentId, :url) returning id", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("sentDocumentId", sentDocumentId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("url", url)) |> ignore
        let sentApplicationId = command.ExecuteScalar() |> string |> Int32.Parse
        command.Dispose()

        use command =
            new NpgsqlCommand(
                """insert into sentStatus(sentApplicationId, statusChangedOn, dueOn, sentStatusValueId, statusMessage)
                values(:sentApplicationId, current_date, null, 1, '') """, dbConn)
        command.Parameters.Add(new NpgsqlParameter("sentApplicationId", sentApplicationId)) |> ignore
        command.ExecuteNonQuery() |> ignore
        command.Dispose()

        log.Debug(
            sprintf
                "(userId = %i, employerId = %i, email = %A, userEmailAndValues = %A, sentFilePages = %A, jobName = %s, url = %s, customVariables = %s) = ()"
                userId
                employerId
                email
                userEmailAndValues
                sentFilePages
                jobName
                url
                customVariables
        )

    let getSentApplications (UserId userId) =
        log.Debug(sprintf "(userId = %i)" userId)
        let sentApplications =
            query {
                for sentApplication in db.Sentapplication do
                join sentStatus in db.Sentstatus on (sentApplication.Id = sentStatus.Sentapplicationid)
                join sentDocument in db.Sentdocument on (sentApplication.Sentdocumentid = sentDocument.Id)
                join employer in db.Employer on (sentDocument.Employerid = employer.Id)
                where (sentApplication.Userid = userId)
                sortByDescending sentStatus.Statuschangedon
                thenByDescending sentStatus.Id
                select ( { employer =
                               { company = employer.Company
                                 gender = Gender.fromString employer.Gender
                                 degree = employer.Degree
                                 firstName = employer.Firstname
                                 lastName = employer.Lastname
                                 street = employer.Street
                                 postcode = employer.Postcode
                                 email = employer.Email
                                 city = employer.City
                                 phone =  employer.Phone
                                 mobilePhone = employer.Mobilephone
                               }
                           jobName = sentDocument.Jobname
                           appliedOn = sentStatus.Statuschangedon
                           url = sentApplication.Url
                         }
                )
            } |> List.ofSeq
        log.Debug(sprintf "(userId = %i) = %A" userId sentApplications)
        sentApplications
    
    
    let getSentApplication dbConn (sentApplicationId : int) =
        log.Debug (sprintf "(sentApplicationId = %i)" sentApplicationId)
        use command =
            new NpgsqlCommand("""
                select
                      sentStatus.statusChangedOn
                    , employer.company
                    , employer.street
                    , employer.postcode
                    , employer.city
                    , employer.gender
                    , employer.degree
                    , employer.firstName
                    , employer.lastName
                    , employer.email
                    , employer.phone
                    , employer.mobilePhone
                    , sentDocumentEmail.subject
                    , sentDocumentEmail.body
                    , sentUserValues.email
                    , sentUserValues.gender
                    , sentUserValues.degree
                    , sentUserValues.firstName
                    , sentUserValues.lastName
                    , sentUserValues.street
                    , sentUserValues.postcode
                    , sentUserValues.city
                    , sentUserValues.phone
                    , sentUserValues.mobilePhone
                    , sentDocument.jobName
                    , sentDocument.id
                    , sentDocument.customVariables
                    , sentApplication.url
                    , sentApplication.userId
                from sentApplication
                join sentDocument on sentApplication.sentDocumentId = sentDocument.id
                join employer on sentDocument.employerId = employer.id
                join sentDocumentEmail on sentDocument.sentDocumentEmailId = sentDocumentEmail.id
                join sentUserValues on sentDocument.sentUserValuesId = sentUserValues.id
                join sentStatus on sentApplication.id = sentStatus.sentApplicationId
                where sentApplication.id = :sentApplicationId
                order by sentApplication.id
                """, dbConn)
        command.Parameters.Add(new NpgsqlParameter("sentApplicationId", sentApplicationId)) |> ignore
        use reader = command.ExecuteReader()
        try
            reader.Read () |> ignore
            let statusChangedOn =
                let d = reader.GetDate(0)
                DateTime(d.Year, d.Month, d.Day)
            let employerCompany = reader.GetString(1)
            let employerStreet = reader.GetString(2)
            let employerPostcode = reader.GetString(3)
            let employerCity = reader.GetString(4)
            let employerGender = reader.GetString(5)
            let employerDegree = reader.GetString(6)
            let employerFirstName = reader.GetString(7)
            let employerLastName = reader.GetString(8)
            let employerEmail = reader.GetString(9)
            let employerPhone = reader.GetString(10)
            let employerMobilePhone = reader.GetString(11)
            let subject = reader.GetString(12)
            let body = reader.GetString(13)
            let userEmail = reader.GetString(14)
            let userGender = reader.GetString(15)
            let userDegree = reader.GetString(16)
            let userFirstName = reader.GetString(17)
            let userLastName = reader.GetString(18)
            let userStreet = reader.GetString(19)
            let userPostcode = reader.GetString(20)
            let userCity = reader.GetString(21)
            let userPhone = reader.GetString(22)
            let userMobilePhone = reader.GetString(23)
            let jobName = reader.GetString(24)
            let sentDocumentId = reader.GetInt32(25)
            let sentCustomVariables = reader.GetString(26)
            let url = reader.GetString(27)
            let userId = reader.GetInt32(28)
            let oSentApplication =
                Some
                    { jobName = jobName
                      url = url
                      sentDate = statusChangedOn.ToString("dd.MM.yyyy")
                      email = { subject = subject; body = body }
                      customVariables = sentCustomVariables
                      user =
                        { email = userEmail
                          id = UserId userId
                          values =
                            { gender = Gender.fromString userGender
                              degree = userDegree
                              firstName = userFirstName
                              lastName = userLastName
                              street = userStreet
                              postcode = userPostcode
                              city = userCity
                              phone = userPhone
                              mobilePhone = userMobilePhone
                            }
                        }
                      employer =
                        { company = employerCompany
                          street = employerStreet
                          postcode = employerPostcode
                          city = employerCity
                          gender = Gender.fromString employerGender
                          degree = employerDegree
                          firstName = employerFirstName
                          lastName = employerLastName
                          email = employerEmail
                          phone = employerPhone
                          mobilePhone = employerMobilePhone
                        }
                      filePages =
                            command.Dispose()
                            reader.Dispose()
                            use command = new NpgsqlCommand("""select path, pageIndex from sentFilePage where sentDocumentId = :sentDocumentId order by pageIndex""", dbConn)
                            command.Parameters.Add(new NpgsqlParameter("sentDocumentId", sentDocumentId)) |> ignore
                            use reader = command.ExecuteReader()
                            [ while reader.Read() do
                                yield
                                      reader.GetString(0)
                                    , reader.GetInt32(1)
                            ]
                    }
            log.Debug (sprintf "(sentApplicationId = %i) = %A" sentApplicationId oSentApplication)
            oSentApplication
        with
        | e ->
            log.Error("", e)
            None

    let getSentApplicationOffset dbConn (sentApplicationOffset : int) (UserId userId) =
        query {
            for sentStatus in db.Sentstatus do
            join sentApplication in db.Sentapplication on
                (sentStatus.Sentapplicationid = sentApplication.Id)
            where (sentApplication.Userid = userId)
            skip sentApplicationOffset
            select (getSentApplication dbConn sentApplication.Id)
        } |> fun x -> x.SingleOrDefault()

    let getNotYetSentApplicationIds dbConn =
        use command =
            new NpgsqlCommand(
                """select sentApplicationId
                   from sentStatus s1
                   where not exists
                        (select sentApplicationId
                         from sentStatus s2
                         where s2.sentApplicationId = s1.sentApplicationId
                           and sentStatusValueId >= 2)
                   limit 50""", dbConn)
        use reader = command.ExecuteReader()
        let sentApplicationIds = 
            [ while reader.Read() do
                yield reader.GetInt32(0)
            ]
        command.Dispose()
        sentApplicationIds


    let overwriteDocument (dbConn : NpgsqlConnection) (document : Document) (UserId userId) =
        match document.oId with
        | None -> log.Error("Documentid was None")
        | Some (DocumentId documentId) ->
            log.Debug(sprintf "(document = %A, userId = %i)" document userId)
            use command = new NpgsqlCommand("""update document
                                               set (name, jobName, customVariables) = (:name, :jobName, :customVariables)
                                               where id = :documentId""", dbConn)
            command.Parameters.Add(new NpgsqlParameter("name", document.name)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("jobName", document.jobName)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("customVariables", document.customVariables)) |> ignore
            command.ExecuteNonQuery() |> ignore
            command.Dispose()

            use command =
                new NpgsqlCommand(
                    """insert into documentEmail(documentId, subject, body) values (:documentId, :subject, :body)
                    on conflict(documentId) do update set (subject, body) = (:subject, :body)""", dbConn)
            command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("subject", document.email.subject)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("body", document.email.body)) |> ignore
            command.ExecuteNonQuery() |> ignore
            command.Dispose()
            use command = new NpgsqlCommand("delete from htmlPage where documentId = :documentId", dbConn)
            command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
            command.ExecuteNonQuery() |> ignore
            command.Dispose()
            use command = new NpgsqlCommand("delete from filePage where documentId = :documentId", dbConn)
            command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
            command.ExecuteNonQuery() |> ignore
            command.Dispose()
            for page in document.pages do
                use command = new NpgsqlCommand("delete from pageMap where documentId = :documentId and pageIndex = :pageIndex", dbConn)
                command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("pageIndex", page.PageIndex())) |> ignore
                command.ExecuteNonQuery() |> ignore
                command.Dispose()
                match page with
                | HtmlPage htmlPage ->
                    use command =
                        new NpgsqlCommand(
                            """insert into htmlPage(documentId, templateId, pageIndex, name)
                            values (:documentId, :templateId, :pageIndex, :name)""", dbConn)
                    command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
                    command.Parameters.Add(new NpgsqlParameter("templateId", htmlPage.oTemplateId |> Option.get)) |> ignore
                    command.Parameters.Add(new NpgsqlParameter("pageIndex", htmlPage.pageIndex)) |> ignore
                    command.Parameters.Add(new NpgsqlParameter("name", htmlPage.name)) |> ignore
                    command.ExecuteNonQuery() |> ignore
                    command.Dispose()
                    for mapItem in htmlPage.map do
                        use command =
                            new NpgsqlCommand(
                                """insert into pageMap(documentId, pageIndex, key, value)
                                values (:documentId, :pageIndex, :key, :value)""", dbConn)
                        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
                        command.Parameters.Add(new NpgsqlParameter("pageIndex", htmlPage.pageIndex)) |> ignore
                        command.Parameters.Add(new NpgsqlParameter("key", fst mapItem)) |> ignore
                        command.Parameters.Add(new NpgsqlParameter("value", snd mapItem)) |> ignore
                        command.ExecuteNonQuery() |> ignore
                        command.Dispose()
                | FilePage filePage ->
                    use command =
                        new NpgsqlCommand(
                            "insert into filePage
                                (documentId, path, pageIndex, name)
                                values (:documentId, :path, :pageIndex, :name)", dbConn)
                    command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
                    command.Parameters.Add(new NpgsqlParameter("path", filePage.path)) |> ignore
                    command.Parameters.Add(new NpgsqlParameter("pageIndex", filePage.pageIndex)) |> ignore
                    command.Parameters.Add(new NpgsqlParameter("name", filePage.name)) |> ignore
                    command.ExecuteNonQuery() |> ignore
                    command.Dispose()
            log.Debug(sprintf "(document = %A, userId = %i) = %A" document userId documentId)
    
    let saveNewDocument (dbConn : NpgsqlConnection) (document : Document) (UserId userId) =
        log.Debug(sprintf "(document = %A, userId = %i)" document userId)
        use command = new NpgsqlCommand("insert into document (userId, name, jobName, customVariables)
                                         values (:userId, :name, :jobName, :customVariables) returning id", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("name", document.name)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("jobName", document.jobName)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("customVariables", document.customVariables)) |> ignore
        let documentId = command.ExecuteScalar() |> string |> Int32.Parse
        command.Dispose()

        use command = new NpgsqlCommand("insert into documentEmail (documentId, subject, body) values (:documentId, :subject, :body)", dbConn)
        command.Parameters.Add(new NpgsqlParameter("subject", document.email.subject)) |> ignore
        command.Parameters.Add(new NpgsqlParameter(":body", document.email.body)) |> ignore
        command.Parameters.Add(new NpgsqlParameter(":documentId", documentId)) |> ignore
        command.ExecuteNonQuery() |> ignore
        command.Dispose()
        for page in document.pages do
            match page with
            | HtmlPage htmlPage ->
                use command =
                    new NpgsqlCommand(
                        """insert into htmlPage(documentId, templateId, pageIndex, name)
                        values (:documentId, :templateId, :pageIndex, :name)""", dbConn)
                command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("templateId", htmlPage.oTemplateId |> Option.get)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("pageIndex", htmlPage.pageIndex)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("name", htmlPage.name)) |> ignore
                command.ExecuteNonQuery() |> ignore
                command.Dispose()
                for mapItem in htmlPage.map do
                    use command =
                        new NpgsqlCommand(
                            """insert into pageMap(documentId, pageIndex, key, value)
                            values (:documentId, :pageIndex, :key, :value)""", dbConn)
                    command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
                    command.Parameters.Add(new NpgsqlParameter("pageIndex", htmlPage.pageIndex)) |> ignore
                    command.Parameters.Add(new NpgsqlParameter("key", fst mapItem)) |> ignore
                    command.Parameters.Add(new NpgsqlParameter("value", snd mapItem)) |> ignore
                    command.ExecuteNonQuery() |> ignore
                    command.Dispose()
            | FilePage filePage ->
                use command =
                    new NpgsqlCommand(
                        """insert into FilePage(documentId, path, pageIndex, name)
                        values (:documentId, :path, :pageIndex, :name) returning id""", dbConn)
                command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("path", filePage.path)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("pageIndex", filePage.pageIndex)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("name", filePage.name)) |> ignore
                let pageId = command.ExecuteScalar() |> string |> Int32.Parse
                command.Dispose()
        log.Debug(sprintf "(document = %A, userId = %i) = %i" document userId documentId)
        DocumentId documentId

    let deleteDocument (documentId : int) =
        log.Debug(sprintf "(documentId = %i)" documentId)
        db.Pagemap.Where(fun x -> x.Documentid = documentId) |> Seq.iter (fun x -> x.Delete())
        db.Htmlpage.Where(fun x -> x.Documentid = documentId) |> Seq.iter (fun x -> x.Delete())
        db.Filepage.Where(fun x -> x.Documentid = documentId) |> Seq.iter (fun x -> x.Delete())
        db.Documentemail.Where(fun x -> x.Documentid = documentId).Single().Delete()
        db.Lastediteddocumentid.Where(fun x -> x.Documentid = documentId) |> Seq.iter (fun x -> x.Delete())
        db.Document.Where(fun x -> x.Id = documentId).Single().Delete()
        dbContext.SubmitUpdates()
        log.Debug(sprintf "(documentId = %i) = ()" documentId)
       
    let getDocument (documentId : int) =
        log.Debug(sprintf "(documentId = %i)" documentId)
        let oDocumentValues = 
            (query {
                for document in db.Document do
                join documentEmail in db.Documentemail on (document.Id = documentEmail.Documentid)
                where (document.Id = documentId)
                select (Some (document.Name, document.Jobname, documentEmail.Subject, documentEmail.Body, document.Customvariables))
            }).SingleOrDefault()
        match oDocumentValues with
        | None ->
            log.Debug(sprintf "(documentId = %i) = None" documentId)
            None
        | Some (documentName, jobName, emailSubject, emailBody, customVariables) ->
            let htmlPages = 
                db.Htmlpage
                  .Where(fun x -> x.Documentid = documentId)
                  .OrderBy(fun x -> x.Pageindex)
                  .Select(fun x ->
                      HtmlPage
                        { map = 
                              db.Pagemap
                                .Where(fun x -> x.Documentid = documentId && x.Pageindex = x.Pageindex)
                                .Select(fun y -> (y.Key, y.Value))
                              |> List.ofSeq
                          oTemplateId = x.Templateid
                          pageIndex = x.Pageindex
                          name = x.Name
                        }
                  )
                |> List.ofSeq
            
            let filePages =
                db.Filepage
                  .Where(fun x -> x.Documentid = documentId)
                  .OrderBy(fun x -> x.Pageindex)
                  .Select(fun x -> FilePage { path = x.Path; pageIndex = x.Pageindex; name = x.Name })
                |> List.ofSeq

            let document =
                { oId = Some (DocumentId documentId)
                  name = documentName
                  pages = (htmlPages @ filePages) |> List.sortBy (fun x -> x.PageIndex())
                  email = {subject = emailSubject; body = emailBody}
                  jobName = jobName
                  customVariables = customVariables
                }
            log.Debug(sprintf "(documentId = %i) = %A" documentId document)
            Some document
    
    let getDocumentOffset (UserId userId) (documentOffset : int) =
        log.Debug(sprintf "(userId = %i, documentOffset = %i)" userId documentOffset)
        try
            db.Document
              .Where(fun x -> x.Userid = userId)
              .OrderBy(fun x -> x.Id)
              .Skip(documentOffset)
              .Select(fun x -> Some x.Id)
              .FirstOrDefault()
            |> Option.bind getDocument
        with
        | e ->
            log.Error ("", e)
            None

    let getDocumentNames (UserId userId) =
        log.Debug(sprintf "(userId = %i)" userId)
        let documentNames =
            db.Document
              .Where(fun x -> x.Userid = userId)
              .OrderBy(fun x -> x.Id)
              .Select(fun x -> x.Name)
            |> List.ofSeq
        log.Debug(sprintf "(userId = %i) = %A" userId documentNames)
        documentNames

    let getHtmlPageTemplatePath (htmlPageTemplateId : int) =
        log.Debug(sprintf "(htmlPageTemplateId = %i)" htmlPageTemplateId)
        let oOdtPath = db.Htmlpagetemplate.Where(fun x -> x.Id = htmlPageTemplateId).Select(fun x -> Some x.Odtpath).SingleOrDefault()
        log.Debug(sprintf "(htmlPageTemplateId = %i) = %A" htmlPageTemplateId oOdtPath)
        oOdtPath

    let getHtmlPageTemplates () =
        log.Debug "()"
        let htmlPageTemplates = db.Htmlpagetemplate.OrderBy(fun x -> x.Id).Select(fun x -> { id = x.Id; html = x.Html; name = x.Name }) |> List.ofSeq
        log.Debug(sprintf "() = %A" htmlPageTemplates)
        htmlPageTemplates

    let getHtmlPageTemplate (templateId : int) =
        log.Debug(sprintf "(templateId = %i)" templateId)
        let oHtml = db.Htmlpagetemplate.Where(fun x -> x.Id = templateId).Select(fun x -> Some x.Html).SingleOrDefault()
        log.Debug(sprintf "(templateId = %i) = %A" templateId oHtml)
        oHtml

    let getHtmlPages (documentId : int) =
        log.Debug(sprintf "(documentId = %i)" documentId)
        let htmlPages = db.Htmlpage.Where(fun x -> x.Documentid = documentId).Select(fun x -> { name = x.Name; oTemplateId = x.Templateid }) |> List.ofSeq
        log.Debug(sprintf "(documentId = %i) = %A" documentId htmlPages)

    
    let getLastEditedDocumentOffset (UserId userId) =
        log.Debug(sprintf "(userId = %i)" userId)
        let documentOffset =
            query {
                for document in db.Document do
                join lastEditedDocumentId in db.Lastediteddocumentid on (document.Userid = lastEditedDocumentId.Userid)
                where (lastEditedDocumentId.Userid = userId)
                select (document.Id, lastEditedDocumentId.Documentid)
            }
            |> Seq.filter(fun (x1, x2) -> x1 < x2)
            |> Seq.length
        log.Debug(sprintf "(userId = %i)" userId)
        documentOffset

    let setLastEditedDocumentId (dbConn : NpgsqlConnection) (UserId userId) (DocumentId documentId) =
        use command =
            new NpgsqlCommand("""insert into lastEditedDocumentId (documentId, userId) values (:documentId, :userId)
                               on conflict (userId) do update 
                               set (documentId, userId) = (:documentId, :userId)""", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        command.ExecuteNonQuery() |> ignore

    let getPageMapOffset dbConn (UserId userId) (pageIndex : int) (documentIndex : int) =
        use command =
            new NpgsqlCommand(
                """select key, value
                from pageMap
                join document on pageMap.documentId = document.id
                where userId = :userId and
                documentId =

                  (select documentId from document where userId = :userId offset :documentIndex limit 1)
                and pageIndex = :pageIndex""", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("pageIndex", pageIndex)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("documentIndex", documentIndex)) |> ignore
        use reader = command.ExecuteReader()
        [ while reader.Read() do
            yield reader.GetString(0), reader.GetString(1)
        ]
        |> Map.ofList
    
    let addFilePage (dbConn : NpgsqlConnection) (documentId : int) (path : string) (pageIndex : int) (name : string) =
        log.Debug (sprintf "(documentId: %i, path: %s, pageIndex: %i)" documentId path pageIndex)
        use command = new NpgsqlCommand("update filePage set pageIndex = pageIndex + 1 where pageIndex >= :pageIndex and documentId = :documentId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("pageIndex", pageIndex)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        command.ExecuteNonQuery() |> ignore
        command.Dispose()

        use command = new NpgsqlCommand("insert into filePage (documentId, path, pageIndex, name) values(:documentId, :path, :pageIndex, :name)", dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("path", path)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("pageIndex", pageIndex)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("name", name)) |> ignore
        command.ExecuteNonQuery() |> ignore
        log.Debug (sprintf "(documentId: %i, path: %s, pageIndex: %i) = ()" documentId path pageIndex)

    let addNewDocument (dbConn : NpgsqlConnection) (UserId userId) (name : string) =
        use command = new NpgsqlCommand("insert into document (userId, name, jobName, customVariables)
                                         values (:userId, :name, '', :customVariables)", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("name", name)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("customVariables", """$anrede = \
        hallo""")) |> ignore
        command.ExecuteNonQuery() |> ignore

    let addHtmlPage dbConn (documentId : int) (oTemplateId : option<int>) (pageIndex : int) (name : string) =
        log.Debug (sprintf "(documentId: %i, oTemplateId: %A, pageIndex: %i, name: %s)" documentId oTemplateId pageIndex name)
        use command = new NpgsqlCommand("update htmlPage set pageIndex = pageIndex + 1 where pageIndex >= :pageIndex and documentId = :documentId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("pageIndex", pageIndex)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        command.ExecuteNonQuery() |> ignore
        command.Dispose()

        use command = new NpgsqlCommand("insert into htmlpage (documentId, templateId, pageIndex, name) values (:documentId, :templateId, :pageIndex, :name)", dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        match oTemplateId with
        | None ->
            command.Parameters.Add(new NpgsqlParameter("templateId", DBNull.Value)) |> ignore
        | Some templateId ->
            command.Parameters.Add(new NpgsqlParameter("templateId", templateId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("pageIndex", pageIndex)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("name", name)) |> ignore
        command.ExecuteNonQuery() |> ignore

    let tryGetDocumentIdOffset (UserId userId) (documentIndex : int) =
        log.Debug(sprintf "(userId = %i, documentIndex = %i)" userId documentIndex)
        let documentIds = db.Document.Where(fun x -> x.Userid = userId).Select(fun x -> x.Id)
        let oDocumentId =
            if documentIds.Count() >= documentIndex
            then Some (documentIds.Skip(documentIndex).First())
            else None
        log.Debug(sprintf "(userId = %i, documentIndex = %i) = %A" userId documentIndex oDocumentId)
        oDocumentId

    let createLink dbConn (filePath : string) (name : string) =
        let linkGuid = Guid.NewGuid().ToString("N")
        use command = new NpgsqlCommand("insert into link (path, guid, name) values(:path, :linkGuid, :name)", dbConn)
        command.Parameters.Add(new NpgsqlParameter("path", filePath)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("linkGuid", linkGuid)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("name", name)) |> ignore
        command.ExecuteNonQuery() |> ignore
        linkGuid

    let getDeletableFilePaths dbConn (documentId : int) =
        use command =
            new NpgsqlCommand(
                """select f1.path
                   from filepage f1
                   where f1.documentid = :documentId
                   and not exists
                     (select f2.path
                      from filepage f2
                      where f1.path = f2.path
                      and f1.id <> f2.id)"""
             , dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        use reader = command.ExecuteReader()
        [ while reader.Read() do
            yield reader.GetString(0)
        ]

    let deleteDeletableDocumentFilePages dbConn (documentId : int) =
        use command =
            new NpgsqlCommand(
                """delete from filepage f1
                   where f1.documentid = :documentId
                   and not exists
                     (select f2.path
                      from filepage f2
                      where f1.path = f2.path
                      and f1.id <> f2.id)"""
             , dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        command.ExecuteNonQuery()


    let tryGetPathAndNameByLinkGuid (linkGuid : string) =
        log.Debug(sprintf "(linkGuid = %s)" linkGuid)
        let oPathAndName = db.Link.Where(fun x -> x.Guid = linkGuid).Select(fun x -> Some (x.Path, x.Name)).SingleOrDefault()
        log.Debug(sprintf "(linkGuid = %s) = %A" linkGuid oPathAndName)
        oPathAndName

    let deleteLink dbConn (linkGuid : string) =
        use command = new NpgsqlCommand("delete from link where guid = :linkGuid", dbConn)
        command.Parameters.Add(new NpgsqlParameter("linkGuid", linkGuid)) |> ignore
        command.ExecuteNonQuery() |> ignore

    let getFilesWithExtension (extension : string) (UserId userId) =
        log.Debug(sprintf "(extension = %s, userId = %i)" extension userId)
        let files =
            query {
                for filePage in db.Filepage do
                join document in db.Document on (filePage.Documentid = document.Id)
                where (filePage.Path.EndsWith("." + extension) && document.Userid = userId)
                select filePage.Path
            } |> List.ofSeq
        log.Debug(sprintf "(extension = %s, userId = %i) = %A" extension userId files)
        files


    let getFilePageNames (DocumentId documentId) =
        log.Debug(sprintf "(documentId = %i)" documentId)
        let filePageNames = db.Filepage.Where(fun x -> x.Documentid = documentId).Select(fun x -> x.Name) |> List.ofSeq
        log.Debug(sprintf "(documentId = %i) = %A" documentId filePageNames)
        filePageNames

    let tryFindSentApplication (UserId userId) (employer : Employer) =
        log.Debug(sprintf "(userId = %i, employer = %A)" userId employer)
        let employers =
                db.Employer
                  .Where(fun x -> (x.Email = employer.email) && x.Userid = userId)
                  .OrderByDescending(fun x -> x.Id)
                |> List.ofSeq
        log.Debug(sprintf "(userId = %i, employer = %A) = %b" userId employer (employers |> (not << List.isEmpty)))
        if employers |> List.isEmpty
        then None
        else Some 1

    
    let setPasswordSaltAndConfirmEmailGuid dbConn (password : string) (salt : string) (confirmEmailGuid : option<string>) (UserId userId) =
        use command = new NpgsqlCommand("update users set (password, salt, confirmEmailGuid) =
                                         (:password, :salt, :confirmEmailGuid) where id = :userId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("password", password)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("salt", salt)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("confirmEmailGuid", getValueOrDBNull confirmEmailGuid)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.ExecuteNonQuery() |> ignore
