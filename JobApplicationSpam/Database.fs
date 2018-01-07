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
    open NpgsqlTypes

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
        use command =
            new NpgsqlCommand(
                """select company, street, postcode, city, gender, degree, firstName,
                lastName, email, phone, mobilePhone from employer where id = :employerId limit 1""", dbConn)
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

    let setUserValues (dbConn : NpgsqlConnection) (userValues : UserValues) (userId : int) =
        log.Debug(sprintf "%A %i" userValues userId)
        use command =
            new NpgsqlCommand(
                "insert into userValues
                (userId, gender, degree, firstName, lastName, street, postcode, city, phone, mobilePhone)
                values (:userId, :gender, :degree, :firstName, :lastName, :street, :postcode, :city, :phone, :mobilePhone)
                on conflict(userId) do
                update set (gender, degree, firstName, lastName, street, postcode, city, phone, mobilePhone)
                        = (:gender, :degree, :firstName, :lastName, :street, :postcode, :city, :phone, :mobilePhone)"
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
        let affectedRowCount = command.ExecuteNonQuery()
        log.Debug(sprintf "%A %i = %i" userValues userId affectedRowCount)
        affectedRowCount
    
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

    let insertSentApplication (dbConn : NpgsqlConnection) (userId : int) (employerId : int) (documentId : int) = //TODO
        log.Debug(sprintf "%i %i %i" userId employerId documentId)
        use command = new NpgsqlCommand("insert into sentApplication(userId, employerId, documentId) values(:userId, :employerId, :documentId) returning id", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("employerId", employerId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        let sentApplicationId = command.ExecuteScalar() |> string |> Int32.Parse
        command.Dispose()

        use command =
            new NpgsqlCommand(
                """insert into sentStatus(sentApplicationId, statusChangedOn, dueOn, sentStatusValueId, statusMessage)
                values(:sentApplicationId, to_timestamp('26.10.2017', '%d.%m.%Y'), null, 1, '') """, dbConn)
        command.Parameters.Add(new NpgsqlParameter("sentApplicationId", sentApplicationId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("employerId", employerId)) |> ignore
        command.ExecuteNonQuery() |> ignore
        command.Dispose()
        log.Debug(sprintf "%i %i %i = ()" userId employerId documentId)


    let overwriteDocument (dbConn : NpgsqlConnection) (document : Document) (userId : int) =
        log.Debug(sprintf "%A %i" document userId)
        use command = new NpgsqlCommand("update document set name = :name where id = :documentId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("name", document.name)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("documentId", document.id)) |> ignore
        command.ExecuteNonQuery() |> ignore
        command.Dispose()
        use command =
            new NpgsqlCommand(
                """insert into documentEmail(documentId, subject, body) values (:documentId, :subject, :body)
                on conflict(documentId) do update set (subject, body) = (:subject, :body)""", dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", document.id)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("subject", document.email.subject)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("body", document.email.body)) |> ignore
        command.ExecuteNonQuery() |> ignore
        command.Dispose()
        use command = new NpgsqlCommand("delete from htmlPage where documentId = :documentId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", document.id)) |> ignore
        command.ExecuteNonQuery() |> ignore
        command.Dispose()
        use command = new NpgsqlCommand("delete from filePage where documentId = :documentId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", document.id)) |> ignore
        command.ExecuteNonQuery() |> ignore
        command.Dispose()
        for page in document.pages do
            match page with
            | HtmlPage htmlPage ->
                use command =
                    new NpgsqlCommand(
                        """insert into htmlPage(documentId, templateId, pageIndex, name)
                        values (:documentId, :templateId, :pageIndex, :name)
                        on conflict on constraint htmlPage_unique do update
                        set (templateId, name) = (:templateId, :name)""", dbConn)
                command.Parameters.Add(new NpgsqlParameter("documentId", document.id)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("templateId", htmlPage.oTemplateId |> Option.get)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("pageIndex", htmlPage.pageIndex)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("name", htmlPage.name)) |> ignore
                command.ExecuteNonQuery() |> ignore
                command.Dispose()
                use command = new NpgsqlCommand("delete from pageMap where documentId = :documentId and pageIndex = :pageIndex", dbConn)
                command.Parameters.Add(new NpgsqlParameter("documentId", document.id)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("pageIndex", htmlPage.pageIndex)) |> ignore
                command.ExecuteNonQuery() |> ignore
                command.Dispose()
                for mapItem in htmlPage.map do
                    use command =
                        new NpgsqlCommand(
                            """insert into pageMap(documentId, pageIndex, key, value)
                            values (:documentId, :pageIndex, :key, :value)
                            on conflict on constraint pageMap_unique do update set value = :value""", dbConn)
                    command.Parameters.Add(new NpgsqlParameter("documentId", document.id)) |> ignore
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
                            values (:documentId, :path, :pageIndex, :name)
                         on conflict on constraint filePage_unique do
                         update set
                            (path, name) = (:path, :name)", dbConn)
                command.Parameters.Add(new NpgsqlParameter("documentId", document.id)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("path", Path.GetFileName(filePage.path))) |> ignore
                command.Parameters.Add(new NpgsqlParameter("pageIndex", filePage.pageIndex)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("name", filePage.name)) |> ignore
                command.ExecuteNonQuery() |> ignore
                command.Dispose()
        log.Debug(sprintf "%A %i = %i" document userId document.id)
        document.id
    
    let saveNewDocument (dbConn : NpgsqlConnection) (document : Document) (userId : int) =
        log.Debug(sprintf "%A %i" document userId)
        use command = new NpgsqlCommand("insert into document (userId, name) values (:userId, :name) returning id", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("name", document.name)) |> ignore
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
                command.Parameters.Add(new NpgsqlParameter("path", Path.GetFileName(filePage.path))) |> ignore
                command.Parameters.Add(new NpgsqlParameter("pageIndex", filePage.pageIndex)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("name", filePage.name)) |> ignore
                let pageId = command.ExecuteScalar() |> string |> Int32.Parse
                command.Dispose()
        log.Debug(sprintf "%A %i = %i" document userId documentId)
        documentId
    
    let deleteDocument (dbConn : NpgsqlConnection) (documentId : int) =
        use command = new NpgsqlCommand("delete from pageMap where documentId = :documentId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
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

        use command = new NpgsqlCommand("delete from documentEmail where documentId = :documentId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        command.ExecuteNonQuery() |> ignore
        command.Dispose()

        use command = new NpgsqlCommand("delete from document where id = :documentId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        command.ExecuteNonQuery() |> ignore
        command.Dispose()

       
    let getDocument (dbConn : NpgsqlConnection) (documentId : int) =
        use command =
            new NpgsqlCommand(
                """select d.name, email.subject, email.body
                from document d
                join documentEmail email on d.id = email.documentId
                where d.id = :documentId""", dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        use reader = command.ExecuteReader()
        reader.Read() |> ignore
        let documentName = reader.GetString(0)
        let emailSubject, emailBody = reader.GetString(1), reader.GetString(2)
        reader.Dispose()
        command.Dispose()

        use command = new NpgsqlCommand("select id, templateId, pageIndex, name from htmlPage where documentId = :documentId order by pageIndex", dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        use reader = command.ExecuteReader()
        let htmlPageData =
            [ while reader.Read() do
                yield
                  reader.GetInt32(0)
                , if reader.IsDBNull(1) then None else Some <| reader.GetInt32(1)
                , reader.GetInt32(2)
                , reader.GetString(3) 
            ]
        reader.Dispose()
        command.Dispose()

        use command = new NpgsqlCommand("select path, pageIndex, name from filePage where documentId = :documentId order by pageIndex", dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        use reader = command.ExecuteReader()
        let filePages =
            [ while reader.Read() do
                yield
                  FilePage
                    { path = reader.GetString(0)
                      pageIndex = reader.GetInt32(1)
                      name = reader.GetString(2)
                    }
            ]
        reader.Dispose()
        command.Dispose()

        let htmlPages =
            htmlPageData
            |> List.map
                (fun (_, oTemplateId, pageIndex, pageName) ->
                    HtmlPage
                      { name = pageName
                        oTemplateId = oTemplateId
                        pageIndex = pageIndex
                        map =
                            use command = new NpgsqlCommand("select key, value from pageMap where documentId = :documentId and pageIndex = :pageIndex", dbConn)
                            command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
                            command.Parameters.Add(new NpgsqlParameter("pageIndex", pageIndex)) |> ignore
                            use reader = command.ExecuteReader()
                            [ while reader.Read() do
                                yield reader.GetString(0), reader.GetString(1)
                            ]
                      }
                )


        { id = documentId
          name = documentName
          pages = (htmlPages @ filePages) |> List.sortBy (fun x -> x.PageIndex())
          email = {subject = emailSubject; body = emailBody}
        }
    
    let getDocumentOffset (dbConn : NpgsqlConnection) (userId : int) (documentOffset : int) =
        if documentOffset < 0
        then None
        else
            use command = new NpgsqlCommand("select id from document where userId = :userId order by id offset :documentOffset limit 1", dbConn)
            command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("documentOffset", documentOffset)) |> ignore
            match command.ExecuteScalar() |> string with
            | null -> None
            | documentIdStr ->
                command.Dispose()
                Some <| getDocument dbConn (documentIdStr |> Int32.Parse)

    let getDocumentNames (dbConn : NpgsqlConnection) (userId : int) =
        log.Debug(sprintf "%i" userId)
        use command = new NpgsqlCommand("select name from document where userId = :userId order by id", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        use reader = command.ExecuteReader()
        let ret =
            [ while reader.Read() do
                yield reader.GetString(0)
            ]
        log.Debug(sprintf "%i = %A" userId ret)
        ret

    let getHtmlPageTemplatePath (dbConn : NpgsqlConnection) (htmlPageTemplateId : int) =
        use command = new NpgsqlCommand("select odtPath from htmlPageTemplate where id = :htmlPageTemplateId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("htmlPageTemplateId", htmlPageTemplateId)) |> ignore
        command.ExecuteScalar() |> string


    let getHtmlPageTemplates (dbConn : NpgsqlConnection) =
        use command = new NpgsqlCommand("select id, html, name from htmlPageTemplate order by id", dbConn)
        use reader = command.ExecuteReader()
        [ while reader.Read() do
            yield { id = reader.GetInt32(0); html = reader.GetString(1); name = reader.GetString(2) } ]

    let getHtmlPageTemplate (dbConn : NpgsqlConnection) (templateId : int) =
        log.Debug(sprintf "%i" templateId)
        use command = new NpgsqlCommand("select html from htmlPageTemplate where id = :templateId limit 1", dbConn)
        command.Parameters.Add(new NpgsqlParameter("templateId", templateId)) |> ignore
        use reader = command.ExecuteReader()
        reader.Read() |> ignore
        let html = reader.GetString(0)
        log.Debug(sprintf "%i = %A" templateId html)
        html

    let getHtmlPages (dbConn : NpgsqlConnection) (documentId : int) =
        use command = new NpgsqlCommand("select name, templateId from htmlPage where documentId = :documentId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        use reader = command.ExecuteReader()
        [ while reader.Read() do
            yield { name = reader.GetString(0); oTemplateId = if reader.IsDBNull(0) then None else Some <| reader.GetInt32(1) } ]

    
    let getLastEditedDocumentId (dbConn : NpgsqlConnection) (userId : int) =
        use command = new NpgsqlCommand("select documentId from lastEditedDocumentId where userId = :userId limit 1", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        let oDocumentId =
            match command.ExecuteScalar() with
            | null -> None
            | documentIdStr -> Some (documentIdStr |> string |> Int32.Parse)
        command.Dispose()
        use command = new NpgsqlCommand("delete from lastEditedDocumentId where userId = :userId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.ExecuteNonQuery() |> ignore
        oDocumentId

    let setLastEditedDocumentId (dbConn : NpgsqlConnection) (userId : int) (documentId : int)=
        use command = new NpgsqlCommand("delete from lastEditedDocumentId where userId = :userId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.ExecuteNonQuery() |> ignore
        command.Dispose()
        use command = new NpgsqlCommand("insert into lastEditedDocumentId (documentId, userId) values (:documentId, :userId)", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        command.ExecuteNonQuery() |> ignore

    let getPageMapOffset (dbConn : NpgsqlConnection) (userId : int) (pageIndex : int) (documentIndex : int) =
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
    
    let addFilePage (dbConn : NpgsqlConnection) (documentId : int) (path : string) (pageIndex : int)  =
        log.Debug (sprintf "(documentId: %i, path: %s, pageIndex: %i)" documentId path pageIndex)
        use command = new NpgsqlCommand("update filePage set pageIndex = pageIndex + 1 where pageIndex >= :pageIndex", dbConn)
        command.Parameters.Add(new NpgsqlParameter("pageIndex", pageIndex)) |> ignore
        command.ExecuteNonQuery() |> ignore
        command.Dispose()

        use command = new NpgsqlCommand("update filePage set pageIndex = pageIndex + 1 where pageIndex >= :pageIndex", dbConn)
        command.Parameters.Add(new NpgsqlParameter("pageIndex", pageIndex)) |> ignore
        command.ExecuteNonQuery() |> ignore
        command.Dispose()

        use command = new NpgsqlCommand("insert into filePage (documentId, path, pageIndex, name) values(:documentId, :path, :pageIndex, :name)", dbConn)
        command.Parameters.Add(new NpgsqlParameter("documentId", documentId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("path", path)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("pageIndex", pageIndex)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("name", Path.GetFileName(path))) |> ignore
        command.ExecuteNonQuery() |> ignore
        log.Debug (sprintf "(documentId: %i, path: %s, pageIndex: %i) = ()" documentId path pageIndex)

    let addNewDocument (dbConn : NpgsqlConnection) (userId : int) (name : string) =
        use command = new NpgsqlCommand("insert into document (userId, name) values (:userId, :name)", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("name", name)) |> ignore
        command.ExecuteNonQuery() |> ignore

    let addHtmlPage dbConn (documentId : int) (oTemplateId : option<int>) (pageIndex : int) (name : string) =
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

    let getDocumentIdOffset dbConn (userId : int) (documentIndex : int) =
        use command = new NpgsqlCommand("select id from document where userId = :userId limit 1 offset :documentIndex", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("documentIndex", documentIndex)) |> ignore
        command.ExecuteScalar() |> string |> Int32.Parse
