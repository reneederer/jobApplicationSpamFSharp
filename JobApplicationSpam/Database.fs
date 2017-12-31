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
        log.Debug(sprintf "%i" userId)
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
        log.Debug(sprintf "%i" employerId)
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
        log.Debug(sprintf "%A %i" employer userId)
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


    let getUserValues (dbConn : NpgsqlConnection) (userId : int) =
        log.Debug(sprintf "%i" userId)
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
        log.Debug(sprintf "%A %i" userValues userId)
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
        log.Debug(sprintf "%s" email)
        use command = new NpgsqlCommand("select count(*) from users where email = :email limit 1", dbConn)
        command.Parameters.Add(new NpgsqlParameter("email", email)) |> ignore
        let emailExists = (command.ExecuteScalar() |> string |> Int32.Parse) = 1
        log.Debug(sprintf "%s = %b" email emailExists)
        emailExists

    let getIdPasswordSaltAndGuid (dbConn : NpgsqlConnection) (email : string) =
        log.Debug(sprintf "%s" email)
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
        log.Debug(sprintf "%s %s %s %s" email password salt guid)
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
        log.Debug(sprintf "%s" email)
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
        log.Debug(sprintf "%s" email)
        use command = new NpgsqlCommand("update users set guid = null where email = :email and guid is not null", dbConn)
        command.Parameters.Add(new NpgsqlParameter("email", email)) |> ignore
        let affectedRowCount = command.ExecuteNonQuery()
        if affectedRowCount <> 1
        then
            log.Error("Email does not exist or guid was already null: " + email)
            failwith <| "Email does not exist or guid was already null: " + email
        log.Debug(sprintf "%s = ()" email)

    let insertJobApplication (dbConn : NpgsqlConnection) (userId : int) (employerId : int) (documentId : int) =
        log.Debug(sprintf "%i %i %i" userId employerId documentId)
        use command = new NpgsqlCommand("insert into jobApplication(userId, employerId, documentId) values(:userId, :employerId, :documentId) returning id", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("employerId", employerId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        let jobApplicationId = command.ExecuteScalar() |> string |> Int32.Parse
        command.Dispose()

        use command = new NpgsqlCommand("""insert into jobApplicationStatus(jobApplicationId, statusCjangedOn, dueOn, statusValueId, statusMessage) values(:jobApplicationId, to_timestamp('26.10.2017', '%d.%m.%Y'), null, 1, '') """, dbConn)
        command.Parameters.Add(new NpgsqlParameter("jobApplicationId", jobApplicationId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("employerId", employerId)) |> ignore
        command.ExecuteNonQuery() |> ignore
        command.Dispose()
        log.Debug(sprintf "%i %i %i = ()" userId employerId documentId)


    
    let saveDocument (dbConn : NpgsqlConnection) (document : Document) (userId : int) =
        log.Debug(sprintf "%A %i" document userId)
        use command = new NpgsqlCommand("insert into document (userId, name, emailSubject, emailBody) values (:userId, :name, '', '') returning id", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("name", document.name)) |> ignore
        let documentId = command.ExecuteScalar() |> string |> Int32.Parse
        command.Dispose()
        for page in document.items do
            match page with
            | DocumentPage page ->
                use command = new NpgsqlCommand("insert into page(documentId, pageTemplateId, name) values (2, 1, 'Anschreiben') returning id", dbConn)
                command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
                let pageId = command.ExecuteScalar() |> string |> Int32.Parse
                command.Dispose()
            | DocumentFile _ -> ()
        log.Debug(sprintf "%A %i = %i" document userId documentId)
        documentId
       
    let getDocument (dbConn : NpgsqlConnection) (documentId : int) =
        use command = new NpgsqlCommand("select name from document where id = :documentId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        use reader = command.ExecuteReader()
        reader.Read() |> ignore
        let documentName = reader.GetString(0)
        reader.Dispose()
        command.Dispose()

        use command = new NpgsqlCommand("select id, pageTemplateId, pageIndex, name from page where documentId = :documentId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        use reader = command.ExecuteReader()
        let pageData =
            [ while reader.Read() do
                yield reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(3) 
            ]
        reader.Dispose()
        command.Dispose()

        use command = new NpgsqlCommand("select path, pageIndex, name from files where documentId = :documentId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        use reader = command.ExecuteReader()
        let files =
            [ while reader.Read() do
                yield
                  DocumentFile
                    { path = reader.GetString(0)
                      pageIndex = reader.GetInt32(1)
                      name = reader.GetString(2)
                    }
            ]
        reader.Dispose()
        command.Dispose()

        
        let pages =
            pageData
            |> List.map
                (fun (pageId, pageTemplateId, pageIndex, pageName) ->
                    DocumentPage
                      { name = pageName
                        templateId = pageTemplateId
                        pageIndex = pageIndex
                      }
                )
        { name = documentName
          items = pages @ files
        }
    
    let getDocumentOffset (dbConn : NpgsqlConnection) (userId : int) (documentOffset : int) =
        use command = new NpgsqlCommand("select id from document where userId = :userId offset :documentOffset limit 1", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("documentOffset", documentOffset)) |> ignore
        let documentId = command.ExecuteScalar() |> string |> Int32.Parse
        command.Dispose()
        getDocument dbConn documentId

    let getDocumentNames (dbConn : NpgsqlConnection) (userId : int) =
        log.Debug(sprintf "%i" userId)
        use command = new NpgsqlCommand("select name from document where userId = :userId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        use reader = command.ExecuteReader()
        let ret =
            [ while reader.Read() do
                yield reader.GetString(0)
            ]
        log.Debug(sprintf "%i = %A" userId ret)
        ret

    let getPageTemplatePath (dbConn : NpgsqlConnection) (pageTemplateId : int) =
        use command = new NpgsqlCommand("select odtPath from pageTemplate where id = :pageTemplateId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("pageTemplateId", pageTemplateId)) |> ignore
        command.ExecuteScalar() |> string


    let getPageTemplates (dbConn : NpgsqlConnection) =
        use command = new NpgsqlCommand("select id, html, name from pageTemplate", dbConn)
        use reader = command.ExecuteReader()
        [ while reader.Read() do
            yield { id = reader.GetInt32(0); html = reader.GetString(1); name = reader.GetString(2) } ]

    let getPageTemplate (dbConn : NpgsqlConnection) (templateIndex : int) =
        log.Debug(sprintf "%i" templateIndex)
        use command = new NpgsqlCommand("select html from pageTemplate where id = :templateIndex limit 1", dbConn)
        command.Parameters.Add(new NpgsqlParameter("templateIndex", templateIndex)) |> ignore
        use reader = command.ExecuteReader()
        reader.Read() |> ignore
        let html = reader.GetString(0)
        log.Debug(sprintf "%i = %s" templateIndex html)
        html

    let getPages (dbConn : NpgsqlConnection) (documentId : int) =
        use command = new NpgsqlCommand("select name, pageTemplateId from page where documentId = :documentId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        use reader = command.ExecuteReader()
        [ while reader.Read() do
            yield { name = reader.GetString(0); pageTemplateId = reader.GetInt32(1) } ]

    (*let getDocumentMapOffset (dbConn : NpgsqlConnection) (userId : int) (documentIndex : int) =
        use command = new NpgsqlCommand("select pageIndex, key, value from documentMap where userId = :userId and documentId = (select documentId from document where userId = :userId offset :documentIndex limit 1) order by pageIndex", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("documentIndex", documentIndex)) |> ignore
        use reader = command.ExecuteReader()
        let mutable lastPageIndex = 1
        let mutable (l:array<array<string*string>>) = [| [||] |]
        let mutable currentPageIndex = reader.GetInt32(0)
        while reader.Read() do
            if currentPageIndex <> lastPageIndex && currentPageIndex <> lastPageIndex + 1
            then
                failwith "Something was wrong with the page indices"
            elif currentPageIndex = lastPageIndex + 1
            then
                l <- Array.append l [||]
                currentPageIndex <- lastPageIndex
            let last = Array.append (Array.last l) [| reader.GetString 1, reader.GetString 2 |]
            let init = Array.take (currentPageIndex - 1) l
            l <- Array.append init [|last|]
        l
        *)

    let getDocumentMapOffset (dbConn : NpgsqlConnection) (userId : int) (pageIndex : int) (documentIndex : int) =
        use command = new NpgsqlCommand("select key, value from documentMap join document on documentMap.documentId = document.id where userId = :userId and documentId = (select documentId from document where userId = :userId offset :documentIndex limit 1) and pageIndex = :pageIndex", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("pageIndex", pageIndex)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("documentIndex", documentIndex)) |> ignore
        use reader = command.ExecuteReader()
        [ while reader.Read() do
            yield reader.GetString(0), reader.GetString(1)
        ]
        |> Map.ofList


