namespace JobApplicationSpam
module Database =
    open System
    open Npgsql
    open Types
    open Chessie.ErrorHandling
    open System.Data
    open log4net.Config
    open log4net
    open System.Reflection
    open System.IO

    let log = LogManager.GetLogger(MethodBase.GetCurrentMethod().GetType())

    let getEmailByUserId (dbConn : NpgsqlConnection) (userId : int) =
        use command = new NpgsqlCommand("select email from users where id = :userId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        use reader = command.ExecuteReader()
        let oEmail =
            if reader.Read()
            then Some <| reader.GetString(0)
            else None
        log.Debug(sprintf "%i = %A" userId oEmail)
        oEmail
        
    let getEmployer (dbConn : NpgsqlConnection) (employerId : int) =
        use command = new NpgsqlCommand("select company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone from employer where id = :employerId limit 1", dbConn)
        command.Parameters.Add(new NpgsqlParameter("employerId", employerId)) |> ignore
        use reader = command.ExecuteReader()
        let oEmployer =
            if reader.Read()
            then
              Some
                  { company = reader.GetString(0)
                    street = reader.GetString(1)
                    postcode = reader.GetString(2)
                    city = reader.GetString(3)
                    gender = Gender.fromString(reader.GetString(4))
                    degree = reader.GetString(5)
                    firstName = reader.GetString(6)
                    lastName = reader.GetString(7)
                    email = reader.GetString(8)
                    phone = reader.GetString(9)
                    mobilePhone = reader.GetString(10)
                  }
            else None
        log.Debug(sprintf "%i = %A" employerId oEmployer)
        oEmployer
    
    let addEmployer (dbConn : NpgsqlConnection) (employer : Employer) (userId : int) =
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
        let addedEmployerId = command.ExecuteScalar() |> string |> Int32.Parse
        log.Debug(sprintf "%A %i = %i" employer userId addedEmployerId)
        addedEmployerId


(*
    let getUserIdByEmail (dbConn : NpgsqlConnection) (email : string) =
        use command = new NpgsqlCommand("select id from users where email = :email", dbConn)
        command.Parameters.Add(new NpgsqlParameter("email", email)) |> ignore
        try
            ok (command.ExecuteScalar() |> string |> Int32.Parse)
        with
        | :? NpgsqlException as e ->
            reraise()
        | e -> fail "Email not found"
        *)
    

    let getTemplateForJobApplication (dbConn : NpgsqlConnection) (templateId : int) =
        use command = new NpgsqlCommand("select emailSubject, emailBody from jobApplicationTemplate where id = :templateId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("templateId", templateId)) |> ignore
        use reader = command.ExecuteReader()
        reader.Read() |> ignore
        let emailSubject = reader.GetString(0)
        let emailBody = reader.GetString(1)
        reader.Dispose()
        command.Dispose()
        use command1 = new NpgsqlCommand("select filePath from jobApplicationTemplateFile where jobApplicationTemplateId = :templateId", dbConn)
        command1.Parameters.Add(new NpgsqlParameter("templateId", templateId)) |> ignore
        use reader1 = command1.ExecuteReader()
        let mutable filePaths = list.Empty
        while reader1.Read() do
            filePaths <- reader1.GetString(0) :: filePaths
        ok
          { emailSubject = emailSubject
            emailBody = emailBody
            filePaths = filePaths
          }



    let getTemplateForJobApplicationByTemplateName (dbConn : NpgsqlConnection) (userId : int) (templateName : string) : Result<int * TemplateForJobApplication, string>=
        use command = new NpgsqlCommand("select id, emailSubject, emailBody from jobApplicationTemplate where userId = :userId and templateName = :templateName", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("templateName", templateName)) |> ignore
        use reader = command.ExecuteReader()
        reader.Read() |> ignore
        let templateId = reader.GetInt32(0)
        let emailSubject = reader.GetString(1)
        let emailBody = reader.GetString(2)
        reader.Dispose()
        command.Dispose()
        use command1 = new NpgsqlCommand("select filePath from jobApplicationTemplateFile where jobApplicationTemplateId = :templateId", dbConn)
        command1.Parameters.Add(new NpgsqlParameter("templateId", templateId)) |> ignore
        use reader1 = command1.ExecuteReader()
        let mutable filePaths = list.Empty
        while reader1.Read() do
            filePaths <- reader1.GetString(0) :: filePaths
        ok
          ( templateId
          , { emailSubject = emailSubject
              emailBody = emailBody
              filePaths = filePaths
            }
          )

    let getTemplateNames (dbConn : NpgsqlConnection) (userId : int) =
        use command = new NpgsqlCommand("select templateName from jobApplicationTemplate where userId = :userId order by templateName desc", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        use reader = command.ExecuteReader()
        let mutable tableNames = []
        while reader.Read() do
            tableNames <- reader.GetString(0) :: tableNames 
        tableNames


    let getTemplateIdWithOffset (dbConn : NpgsqlConnection) (userId : int) (offset : int)=
        use command = new NpgsqlCommand("select id from jobApplicationTemplate where userId = :userId limit 1 offset :offset", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("offset", offset)) |> ignore
        try
            ok (command.ExecuteScalar() |> string |> Int32.Parse)
        with
        | :? PostgresException
        | _ ->
            fail "An error occured while trying to add user."

    let getUserValues (dbConn : NpgsqlConnection) (userId : int) =
        use command =
            new NpgsqlCommand("""
                select gender, degree, firstName, lastName, street, postcode, city, phone, mobilePhone
                from userValues where userId = :userId order by id desc limit 1"""
                , dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        use reader = command.ExecuteReader()
        let oUserValues =
            if reader.Read()
            then
                Some
                    { gender = reader.GetString(0) |> Gender.fromString
                      degree = reader.GetString(1)
                      firstName = reader.GetString(2)
                      lastName = reader.GetString(3)
                      street = reader.GetString(4)
                      postcode = reader.GetString(5)
                      city = reader.GetString(6)
                      phone = reader.GetString(7)
                      mobilePhone = reader.GetString(8)
                    }
            else None
        log.Debug(sprintf "%i = %A" userId oUserValues)
        oUserValues

    let addUserValues (dbConn : NpgsqlConnection) (userValues : UserValues) (userId : int) =
        use command = new NpgsqlCommand("""
            insert into userValues
                (userId, gender, degree, firstName, lastName, street, postcode, city, phone, mobilePhone)
                values (:userId, :gender, :degree, :firstName, :lastName, :street, :postcode, :city, :phone, :mobilePhone) returning id"""
            , dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("gender", userValues.gender.ToString())) |> ignore
        command.Parameters.Add(new NpgsqlParameter("degree", userValues.degree)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("firstName", userValues.firstName)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("lastName", userValues.lastName)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("street", userValues.street)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("postcode", userValues.postcode)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("city", userValues.city)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("phone", userValues.phone)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("mobilePhone", userValues.mobilePhone)) |> ignore
        let addedUserValuesId = command.ExecuteScalar() |> string |> Int32.Parse
        log.Debug(sprintf "%A %i = %i" userValues userId addedUserValuesId)
        addedUserValuesId
    
    let userEmailExists (dbConn : NpgsqlConnection) (email : string) =
        use command = new NpgsqlCommand("select count(*) from users where email = :email limit 1", dbConn)
        command.Parameters.Add(new NpgsqlParameter("email", email)) |> ignore
        let emailExists = (command.ExecuteScalar() |> string |> Int32.Parse) = 1
        log.Debug(sprintf "%s = %b" email emailExists)
        emailExists

    let getIdPasswordSaltAndGuid (dbConn : NpgsqlConnection) (email : string) =
        use command = new NpgsqlCommand("select id, password, salt, guid from users where email = :email limit 1", dbConn)
        command.Parameters.Add(new NpgsqlParameter("email", email)) |> ignore
        use reader = command.ExecuteReader()
        let ret =
            if reader.Read()
            then
                Some
                    ( reader.GetInt32(0)
                    , reader.["password"] |> string
                    , reader.["salt"] |> string
                    , if reader.IsDBNull(3) then None else Some (reader.GetString(3))
                    )
            else None
        log.Debug(sprintf "%s = %A" email ret)
        ret


    let insertNewUser (dbConn : NpgsqlConnection) (email : string) (password : string) (salt : string) (guid : string) =
        use command = new NpgsqlCommand("insert into users(email, password, salt, guid) values(:email, :password, :salt, :guid) returning id", dbConn)
        command.Parameters.AddRange(
            [| new NpgsqlParameter("email", email)
               new NpgsqlParameter("password", password)
               new NpgsqlParameter("salt", salt)
               new NpgsqlParameter("guid", guid) |])
        let insertedNewUserId = command.ExecuteScalar() |> string |> Int32.Parse
        log.Debug(sprintf "%s %s %s %s = %i" email password salt guid insertedNewUserId)
        insertedNewUserId

    let getGuid (dbConn : NpgsqlConnection) (email : string) =
        use command = new NpgsqlCommand("select guid from users where email = :email", dbConn)
        command.Parameters.Add(new NpgsqlParameter("email", email)) |> ignore
        let guid = command.ExecuteScalar()
        if guid = null then raise (new Exception("No record with email: " + email))
        let oGuid =
            match guid |> string with
            | null -> None
            | "" -> None
            | guidStr -> Some guidStr
        log.Debug(sprintf "%s = %A" email oGuid)
        oGuid

    let setGuidToNull (dbConn : NpgsqlConnection) (email : string) =
        use command = new NpgsqlCommand("update users set guid = null where email = :email and guid is not null", dbConn)
        command.Parameters.Add(new NpgsqlParameter("email", email)) |> ignore
        let affectedRowCount = command.ExecuteNonQuery()
        if affectedRowCount <> 1
        then
            failwith <| "Email does not exist or guid was already null: " + email
        log.Debug(sprintf "%s = ()" email)
    
    let saveHtmlJobApplication (dbConn : NpgsqlConnection) (htmlJobApplication : HtmlJobApplication) (userId : int) =
        use transaction = dbConn.BeginTransaction()
        try
            use command = new NpgsqlCommand("insert into htmlJobApplication (userId, name) values (:userId, :name) returning id", dbConn)
            command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("name", htmlJobApplication.name)) |> ignore
            let htmlJobApplicationId = command.ExecuteScalar() |> string |> Int32.Parse
            command.Dispose()
            for page in htmlJobApplication.pages do
                use command = new NpgsqlCommand("insert into htmlJobApplicationPage(htmlJobApplicationId, htmlJobApplicationPageTemplateId, name) values (2, 1, 'Anschreiben') returning id", dbConn)
                command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
                let htmlJobApplicationPageId = command.ExecuteScalar() |> string |> Int32.Parse
                command.Dispose()
                for mapItem in page.map do
                    use command = new NpgsqlCommand("insert into htmlJobApplicationPageValue(htmlJobApplicationPageId, key, value) values (:htmlJobApplicationPageId, :key, :value)", dbConn)
                    command.Parameters.Add(new NpgsqlParameter("htmlJobApplicationPageId", htmlJobApplicationPageId)) |> ignore
                    command.Parameters.Add(new NpgsqlParameter("key", mapItem.Key)) |> ignore
                    command.Parameters.Add(new NpgsqlParameter("value", mapItem.Value)) |> ignore
                    command.ExecuteNonQuery() |> ignore
                    command.Dispose()
            transaction.Commit()
            htmlJobApplicationId
        with
        | e ->
            transaction.Rollback()
            reraise()
       
    let getHtmlJobApplication (dbConn : NpgsqlConnection) (htmlJobApplicationId : int) =
        use command = new NpgsqlCommand("select name from htmlJobApplication where id = :htmlJobApplicationId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("htmlJobApplicationId", htmlJobApplicationId)) |> ignore
        use reader = command.ExecuteReader()
        reader.Read() |> ignore
        let htmlJobApplicationName =
            reader.GetString(0)
        reader.Dispose()
        command.Dispose()

        use command = new NpgsqlCommand("select id, htmlJobApplicationPageTemplateId, name from htmlJobApplicationPage where htmlJobApplicationId = :htmlJobApplicationId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("htmlJobApplicationId", htmlJobApplicationId)) |> ignore
        use reader = command.ExecuteReader()
        let pageData =
            [ while reader.Read() do
                yield reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2) 
            ]
        reader.Dispose()
        command.Dispose()

        
        let pages =
            pageData
            |> List.map
                (fun (htmlJobApplicationPageId, htmlJobApplicationPageTemplateId, htmlJobApplicationPageName) ->
                    use command = new NpgsqlCommand("select key, value from htmlJobApplicationPageValue where htmlJobApplicationPageId = :htmlJobApplicationPageId", dbConn)
                    command.Parameters.Add(new NpgsqlParameter("htmlJobApplicationPageId", htmlJobApplicationPageId)) |> ignore
                    use reader = command.ExecuteReader()
                    let map =
                        [ while reader.Read() do
                            yield (reader.GetString(0), reader.GetString(1))
                        ]
                        |> Map.ofList

                    { name = htmlJobApplicationPageName
                      jobApplicationPageTemplateId = htmlJobApplicationPageTemplateId
                      map = map
                    }
                )
        { name = htmlJobApplicationName
          pages = pages
        }
    
    let getHtmlJobApplicationOffset (dbConn : NpgsqlConnection) (userId : int) (htmlJobApplicationOffset : int) =
        use command = new NpgsqlCommand("select id from htmlJobApplication where userId = :userId offset :htmlJobApplicationOffset limit 1", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("htmlJobApplicationOffset", htmlJobApplicationOffset)) |> ignore
        let htmlJobApplicationId = command.ExecuteScalar() |> string |> Int32.Parse
        command.Dispose()
        getHtmlJobApplication dbConn htmlJobApplicationId

    let getHtmlJobApplicationNames (dbConn : NpgsqlConnection) (userId : int) =
        use command = new NpgsqlCommand("select name from htmlJobApplication where userId = :userId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        use reader = command.ExecuteReader()
        [ while reader.Read() do
            yield reader.GetString(0)
        ]

    let getHtmlJobApplicationPageTemplatePath (dbConn : NpgsqlConnection) (htmlJobApplicationPageTemplateId : int) =
        use command = new NpgsqlCommand("select odtPath from htmlJobApplicationPageTemplate where id = :htmlJobApplicationPageTemplateId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("htmlJobApplicationPageTemplateId", htmlJobApplicationPageTemplateId)) |> ignore
        command.ExecuteScalar() |> string

    let getHtmlJobApplicationPageTemplateNames (dbConn : NpgsqlConnection) =
        use command = new NpgsqlCommand("select name from htmlJobApplicationPageTemplate", dbConn)
        use reader = command.ExecuteReader()
        [ while reader.Read() do
            yield reader.GetString(0) ]
