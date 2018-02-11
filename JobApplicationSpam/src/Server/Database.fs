namespace JobApplicationSpam
module Database =
    open System
    open Npgsql
    open log4net
    open System.Linq
    open System.Reflection
    open FSharp.Data.Sql
    open Types
    open DBTypes

    [<Literal>]
    let private connectionString = "Server=localhost; Port=5432; User Id=spam; Password=Steinmetzstr9!@#$; Database=jobapplicationspam"
    [<Literal>]
    let private resolutionPath = "bin"
    
    type D = DB.dataContext.publicSchema

    let getValueOrDBNull oV =
        match oV with
        | Some v -> v :> obj
        | None -> DBNull.Value :> obj


    let log = LogManager.GetLogger(MethodBase.GetCurrentMethod().GetType())

    let getEmailByUserId (db : DB.dataContext.publicSchema) (UserId userId) : option<string> =
        let oEmail = db.Users.Where(fun x -> x.Id = userId).Select(fun x -> x.Email).SingleOrDefault()
        oEmail
        
    let getUserIdByEmail (db : D) (email : string) : option<int> =
        let oUserId = db.Users.Where(fun x -> x.Email.IsSome && x.Email.Value = email).Select(fun x -> Some x.Id).SingleOrDefault()
        oUserId
    
    let insertLastLogin (db : D) (UserId userId) =
        let dbLogin = db.Login.Create()
        dbLogin.Userid <- userId
        dbLogin.Loggedinat <- DateTime.Now
        dbContext.SubmitUpdates()
    
    let addEmployer (db : D) (employer : Employer) (UserId userId) =
        let dbEmployer = db.Employer.Create()
        dbEmployer.Userid <- userId
        dbEmployer.Company <- employer.company
        dbEmployer.Street <- employer.street
        dbEmployer.Postcode <- employer.postcode
        dbEmployer.City <- employer.city
        dbEmployer.Gender <- employer.gender.ToString()
        dbEmployer.Degree <- employer.degree
        dbEmployer.Firstname <- employer.firstName
        dbEmployer.Lastname <- employer.lastName
        dbEmployer.Email <- employer.email
        dbEmployer.Phone <- employer.phone
        dbEmployer.Mobilephone <- employer.mobilePhone
        dbContext.SubmitUpdates()
        dbEmployer

    let getUserValues (db : D) (UserId userId) =
        query {
            for userValues in db.Uservalues do
            join user in db.Users on (userValues.Userid = user.Id)
            where (userValues.Userid = userId)
            select
                (Some
                    { gender = Gender.fromString userValues.Gender
                      degree = userValues.Degree

                      firstName = userValues.Firstname
                      lastName = userValues.Lastname
                      street = userValues.Street
                      postcode = userValues.Postcode
                      city = userValues.City
                      phone = userValues.Phone
                      mobilePhone = userValues.Mobilephone
                    })
        } |> fun x -> x.SingleOrDefault()

    let setUserValues (db : D) (userValues : UserValues) (UserId userId) =
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
        |> ignore
        dbContext.SubmitUpdates()
    
    let setUserEmail (db : D) (email : string) (UserId userId) =
        db.Users.Where(fun x -> x.Id = userId).Single().Email <- Some email
        dbContext.SubmitUpdates()
    
    let userEmailExists (db : D) (email : string) =
        let emailExists = db.Users.Any(fun x -> x.Email.IsSome && x.Email.Value = email)
        emailExists

    let getUserIdBySessionGuid (db : D) (sessionGuid : string) : option<UserId> =
        db.Users.Where(fun x -> x.Sessionguid.IsSome && x.Sessionguid.Value = sessionGuid) |> Seq.iter (fun x -> x.Sessionguid |> (printfn "%A"))
        let oUserId =
            query {
                for user in db.Users do
                where (user.Sessionguid.IsSome && user.Sessionguid.Value = sessionGuid)
                select (Some user.Id)
            } |> fun x -> x.FirstOrDefault()
        oUserId |> Option.map UserId
    
    let setSessionGuid (db : D) (oSessionGuid : option<string>) (UserId userId) =
        db.Users.Where(fun x -> x.Id = userId).Single().Sessionguid <- oSessionGuid
        dbContext.SubmitUpdates()

    let getValidateLoginData (db : D) (email : string) : option<ValidateLoginData> =
        let ret =
            db.Users
              .Where(fun x -> x.Email.IsSome && x.Email.Value = email)
              .Select(fun x ->
                    Some { userId = UserId x.Id
                           userEmail = email
                           hashedPassword = x.Password
                           salt = x.Salt
                           confirmEmailGuid = x.Confirmemailguid}).SingleOrDefault()
        ret
    
    let insertNewUser
            (db : D)
            (oEmail : option<string>)
            (password : string)
            (salt : string)
            (oConfirmEmailGuid : option<string>)
            (oSessionGuid : option<string>)
            (createdOn : DateTime) =
        let user =
            db.Users.Create(
                Email = oEmail
              , Password = password
              , Salt = salt
              , Confirmemailguid = oConfirmEmailGuid
              , Sessionguid = oSessionGuid
              , Createdon = createdOn)
        dbContext.SubmitUpdates()
        let userValues =
            db.Uservalues.Create(
                Userid = user.Id
              , Gender = "u"
              , Degree = ""
              , Firstname = ""
              , Lastname = ""
              , Street = ""
              , Postcode = ""
              , City = ""
              , Phone = ""
              , Mobilephone = "")
        dbContext.SubmitUpdates()
        UserId user.Id

    let getConfirmEmailGuid (db : D) (email : string) : option<string> =
        let oConfirmEmailGuid =
            db.Users
              .Where(fun x -> x.Email.IsSome && x.Email.Value = email)
              .Select(fun x -> x.Confirmemailguid)
              .SingleOrDefault()
        oConfirmEmailGuid

    let getConfirmEmailGuidByUserId (db : D) (UserId userId) : option<string> =
        let oConfirmEmailGuid = db.Users.Where(fun x -> x.Id = userId).Select(fun x -> x.Confirmemailguid).SingleOrDefault()
        oConfirmEmailGuid

    let setConfirmEmailGuidToNull (db : D) (email : string) =
        let records = db.Users.Where(fun x -> x.Email.IsSome && x.Email.Value = email)
        records |> Seq.iter (fun x -> x.Confirmemailguid <- None)
        dbContext.SubmitUpdates()
        records.Count()

    let insertNotYetSentApplication
            (db : D)
            (UserId userId)
            (employerId : int)
            (email : DocumentEmail)
            (userEmail, userValues : UserValues)
            (sentFilePages : list<string * int>) // path * pageIndex
            (jobName : string)
            (url : string)
            (customVariables : string)
            (statusChangedOn : DateTime) =
        let sentDocumentEmail = db.Sentdocumentemail.Create(Subject = email.subject, Body = email.body)
        dbContext.SubmitUpdates()
        let sentUserValues =
            db.Sentuservalues.Create(
                Email = userEmail
              , Gender = userValues.gender.ToString()
              , Degree = userValues.degree
              , Firstname = userValues.firstName
              , Lastname = userValues.lastName
              , Street = userValues.street
              , Postcode = userValues.postcode
              , City = userValues.city
              , Phone = userValues.phone
              , Mobilephone = userValues.mobilePhone)
        dbContext.SubmitUpdates()
        let sentDocument =
            db.Sentdocument.Create(
                Employerid = employerId
              , Sentdocumentemailid = sentDocumentEmail.Id
              , Sentuservaluesid = sentUserValues.Id
              , Jobname = jobName
              , Customvariables = customVariables)
        dbContext.SubmitUpdates()
        for (path, pageIndex) in sentFilePages do
            db.Sentfilepage.Create(Sentdocumentid = sentDocument.Id, Path = path, Pageindex = pageIndex) |> ignore
        dbContext.SubmitUpdates()
        let sentApplication = db.Sentapplication.Create(Userid = userId, Sentdocumentid = sentDocument.Id, Url = url)
        dbContext.SubmitUpdates()
        db.Sentstatus.Create(
            Sentapplicationid = sentApplication.Id
          , Statuschangedon = statusChangedOn
          , Dueon = None
          , Sentstatusvalueid = 1
          , Statusmessage = "") |> ignore
        dbContext.SubmitUpdates()

    let getSentApplications (db : D) (UserId userId) =
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
        sentApplications
    
    
    let getSentApplication (db : D) (sentApplicationId : int) =
        query {
            for sentApplication in db.Sentapplication do
            join sentDocument in db.Sentdocument on (sentApplication.Sentdocumentid = sentDocument.Id)
            join employer in db.Employer on (sentDocument.Employerid = employer.Id)
            join sentDocumentEmail in db.Sentdocumentemail on (sentDocument.Sentdocumentemailid = sentDocumentEmail.Id)
            join sentUserValues in db.Sentuservalues on (sentDocument.Sentuservaluesid = sentUserValues.Id)
            join sentStatus in db.Sentstatus on (sentApplication.Id = sentStatus.Sentapplicationid)
            where (sentApplication.Id = sentApplicationId)
            select
                ( sentDocument.Jobname
                , sentApplication.Url
                , sentStatus.Statuschangedon
                , sentDocumentEmail.Subject
                , sentDocumentEmail.Body
                , sentDocument.Customvariables
                , sentUserValues.Email
                , sentApplication.Userid
                , sentUserValues.Gender
                , sentUserValues.Degree
                , sentUserValues.Firstname
                , sentUserValues.Lastname
                , sentUserValues.Street
                , sentUserValues.Postcode
                , sentUserValues.City
                , sentUserValues.Phone
                , sentUserValues.Mobilephone
                , employer.Company
                , employer.Street
                , employer.Postcode
                , employer.City
                , employer.Gender
                , employer.Degree
                , employer.Firstname
                , employer.Lastname
                , employer.Email
                , employer.Phone
                , employer.Mobilephone
                , sentDocument.Id
                )
        }
        |> Seq.map
            (fun ( jobName
                 , url
                 , statusChangedOn
                 , emailSubject
                 , emailBody
                 , customvariables
                 , userEmail
                 , userId
                 , userGender
                 , userDegree
                 , userFirstname
                 , userLastname
                 , userStreet
                 , userPostcode
                 , userCity
                 , userPhone
                 , userMobilephone
                 , employerCompany
                 , employerStreet
                 , employerPostcode
                 , employerCity
                 , employerGender
                 , employerDegree
                 , employerFirstname
                 , employerLastname
                 , employerEmail
                 , employerPhone
                 , employerMobilephone
                 , sentDocumentId
                 ) ->
                     Some
                         { jobName = jobName
                           url = url
                           sentDate = statusChangedOn.ToString("dd.MM.yyyy")
                           email = { subject = emailSubject; body = emailBody }
                           customVariables = customvariables
                           user =
                             { email = userEmail
                               id = UserId userId
                               values =
                                 { gender = Gender.fromString userGender
                                   degree = userDegree
                                   firstName = userFirstname
                                   lastName = userLastname
                                   street = userStreet
                                   postcode = userPostcode
                                   city = userCity
                                   phone = userPhone
                                   mobilePhone = userMobilephone
                                 }
                             }
                           employer =
                             { company = employerCompany
                               street = employerStreet
                               postcode = employerPostcode
                               city = employerCity
                               gender = Gender.fromString employerGender
                               degree = employerDegree
                               firstName = employerFirstname
                               lastName = employerLastname
                               email = employerEmail
                               phone = employerPhone
                               mobilePhone = employerMobilephone
                             }
                           filePages =
                                 db.Sentfilepage
                                   .Where(fun x -> x.Sentdocumentid = sentDocumentId)
                                   .Select(fun x -> x.Path, x.Pageindex)
                                 |> List.ofSeq
                         }
         )
         |> fun x -> x.SingleOrDefault()

    let getSentApplicationOffset (db : D) (sentApplicationOffset : int) (UserId userId) =
        query {
            for sentStatus in db.Sentstatus do
            join sentApplication in db.Sentapplication on
                (sentStatus.Sentapplicationid = sentApplication.Id)
            where (sentApplication.Userid = userId)
            skip sentApplicationOffset
            select (getSentApplication db sentApplication.Id)
        } |> fun x -> x.SingleOrDefault()

    let getNotYetSentApplicationIds (db : D) =
        query {
            for sentStatus1 in db.Sentstatus do
            where (sentStatus1.Sentapplicationid |<>|
                    query {
                        for sentStatus2 in db.Sentstatus do
                        where (sentStatus2.Sentstatusvalueid >= 2)
                        select sentStatus2.Sentapplicationid
                        distinct
                    }
            )
            select sentStatus1.Sentapplicationid
        } |> List.ofSeq


    let overwriteDocument (db : D) (document : Document) (UserId userId) =
        match document.oId with
        | None -> log.Error("Documentid was None")
        | Some (DocumentId documentId) ->
            let dbDocument = db.Document.Where(fun x -> x.Id = documentId).Single()
            dbDocument.Jobname <- document.jobName
            dbDocument.Customvariables <- document.customVariables
            dbDocument.Name <- document.name
            dbContext.SubmitUpdates()
            match db.Documentemail.Where(fun x -> x.Documentid = documentId) |> List.ofSeq with
            | [] -> db.Documentemail.Create(Documentid = documentId, Subject = document.email.subject, Body = document.email.body) |> ignore
            | [x] ->
                x.Subject <- document.email.subject
                x.Body <- document.email.body
            | _ -> failwith "There should only be one email for a document"
            dbContext.SubmitUpdates()

            db.Htmlpage.Where(fun x -> x.Documentid = documentId) |> Seq.iter (fun x -> x.Delete())
            dbContext.SubmitUpdates()
            db.Filepage.Where(fun x -> x.Documentid = documentId) |> Seq.iter (fun x -> x.Delete())
            dbContext.SubmitUpdates()
            for page in document.pages do
                db.Pagemap.Where(fun x -> x.Documentid = documentId && x.Pageindex = page.PageIndex()) |> Seq.iter (fun x -> x.Delete())
                dbContext.SubmitUpdates()
                match page with
                | HtmlPage htmlPage ->
                    db.Htmlpage.Create(
                        Documentid = documentId
                      , Templateid = htmlPage.oTemplateId
                      , Pageindex = htmlPage.pageIndex
                      , Name = htmlPage.name) |> ignore
                    dbContext.SubmitUpdates()

                    for mapItem in htmlPage.map do
                        db.Pagemap.Create(
                            Documentid = documentId
                          , Pageindex = htmlPage.pageIndex
                          , Key = fst mapItem
                          , Value = snd mapItem) |> ignore
                        dbContext.SubmitUpdates()
                | FilePage filePage ->
                    db.Filepage.Create(
                        Documentid = documentId
                      , Path = filePage.path
                      , Pageindex = filePage.pageIndex
                      , Name = filePage.name) |> ignore
                    dbContext.SubmitUpdates()
    
    let saveNewDocument (db : D) (document : Document) (UserId userId) =
        let dbDocument =
            db.Document.Create( Userid = userId, Name = document.name,
                                Jobname = document.jobName, Customvariables = document.customVariables)
        dbContext.SubmitUpdates()
        db.Documentemail.Create(Documentid=dbDocument.Id, Subject=document.email.subject, Body=document.email.body) |> ignore
        for page in document.pages do
            match page with
            | HtmlPage htmlPage ->
                db.Htmlpage.Create(
                    Documentid = dbDocument.Id
                  , Templateid = htmlPage.oTemplateId
                  , Pageindex = htmlPage.pageIndex
                  , Name = htmlPage.name) |> ignore
                dbContext.SubmitUpdates()
                for mapItem in htmlPage.map do
                    db.Pagemap.Create( Documentid = dbDocument.Id, Pageindex = htmlPage.pageIndex
                                     , Key = fst mapItem, Value = snd mapItem) |> ignore
                    dbContext.SubmitUpdates()
            | FilePage filePage ->
                    db.Filepage.Create( Documentid = dbDocument.Id, Path = filePage.path
                                      , Pageindex = filePage.pageIndex, Name = filePage.name)
                    |> ignore
                    dbContext.SubmitUpdates()
        DocumentId dbDocument.Id

    let deleteDocument (db : D) (documentId : int) =
        db.Pagemap.Where(fun x -> x.Documentid = documentId) |> Seq.iter (fun x -> x.Delete())
        dbContext.SubmitUpdates()
        db.Htmlpage.Where(fun x -> x.Documentid = documentId) |> Seq.iter (fun x -> x.Delete())
        dbContext.SubmitUpdates()
        db.Filepage.Where(fun x -> x.Documentid = documentId) |> Seq.iter (fun x -> x.Delete())
        dbContext.SubmitUpdates()
        db.Documentemail.Where(fun x -> x.Documentid = documentId).Single().Delete()
        dbContext.SubmitUpdates()
        db.Lastediteddocumentid.Where(fun x -> x.Documentid = documentId) |> Seq.iter (fun x -> x.Delete())
        dbContext.SubmitUpdates()
        db.Document.Where(fun x -> x.Id = documentId).Single().Delete()
        dbContext.SubmitUpdates()
       
    let getDocument (db : D) (documentId : int) =
        let oDocumentValues = 
            (query {
                for document in db.Document do
                join documentEmail in db.Documentemail on (document.Id = documentEmail.Documentid)
                where (document.Id = documentId)
                select (Some (document.Name, document.Jobname, documentEmail.Subject, documentEmail.Body, document.Customvariables))
            }).SingleOrDefault()
        match oDocumentValues with
        | None ->
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
            Some document
    
    let getDocumentOffset (db : D) (UserId userId) (documentOffset : int) =
        try
            db.Document
              .Where(fun x -> x.Userid = userId)
              .OrderBy(fun x -> x.Id)
              .Skip(documentOffset)
              .Select(fun x -> Some x.Id)
              .FirstOrDefault()
            |> Option.bind (getDocument db)
        with
        | e ->
            log.Error ("", e)
            None

    let getDocumentNames (db : D) (UserId userId) =
        let documentNames =
            db.Document
              .Where(fun x -> x.Userid = userId)
              .OrderBy(fun x -> x.Id)
              .Select(fun x -> x.Name)
            |> List.ofSeq
        documentNames


    let getHtmlPageTemplatePath (db : D) (htmlPageTemplateId : int) =
        let oOdtPath = db.Htmlpagetemplate.Where(fun x -> x.Id = htmlPageTemplateId).Select(fun x -> Some x.Odtpath).SingleOrDefault()
        oOdtPath

    let getHtmlPageTemplates  (db : D)() =
        let htmlPageTemplates = db.Htmlpagetemplate.OrderBy(fun x -> x.Id).Select(fun x -> { id = x.Id; html = x.Html; name = x.Name }) |> List.ofSeq
        htmlPageTemplates

    let getHtmlPageTemplate (db : D) (templateId : int) =
        let oHtml = db.Htmlpagetemplate.Where(fun x -> x.Id = templateId).Select(fun x -> Some x.Html).SingleOrDefault()
        oHtml

    let getHtmlPages (db : D) (documentId : int) =
        let htmlPages = db.Htmlpage.Where(fun x -> x.Documentid = documentId).Select(fun x -> { name = x.Name; oTemplateId = x.Templateid }) |> List.ofSeq
        ()

    
    let getLastEditedDocumentOffset (db : D) (UserId userId) =
        let documentOffset =
            query {
                for document in db.Document do
                join lastEditedDocumentId in db.Lastediteddocumentid on (document.Userid = lastEditedDocumentId.Userid)
                where (lastEditedDocumentId.Userid = userId)
                select (document.Id, lastEditedDocumentId.Documentid)
            }
            |> Seq.filter(fun (x1, x2) -> x1 < x2)
            |> Seq.length
        documentOffset

    let setLastEditedDocumentId (db : D) (DocumentId documentId) (UserId userId) =
        match db.Lastediteddocumentid
                .Where(fun x -> x.Userid = userId)
                .Select(fun x -> Some x)
                .SingleOrDefault() with
        | None ->
            db.Lastediteddocumentid.Create(Documentid = documentId, Userid = userId) |> ignore
        | Some v ->
            v.Documentid <- documentId
            v.Userid <- userId
        dbContext.SubmitUpdates()

    let getPageMapOffset (db : D) (UserId userId) (pageIndex : int) (documentIndex : int) =
        query {
            for pageMap in db.Pagemap do
            join document in db.Document on (pageMap.Documentid = document.Id)
            where ((document.Userid = userId)
                   && (document.Id = db.Document.Where(fun x -> x.Userid = userId).Select(fun x -> x.Id).Skip(documentIndex).Single()))
            select (pageMap.Key, pageMap.Value)
        } |> List.ofSeq
    
    let addFilePage (db : D) (documentId : int) (path : string) (pageIndex : int) (name : string) =
        db.Filepage.Where(fun x -> x.Pageindex >= pageIndex && x.Documentid = documentId)
        |> Seq.iter (fun x -> x.Pageindex <- x.Pageindex + 1)
        db.Filepage.Create(Documentid = documentId, Path = path, Pageindex = pageIndex, Name = name) |> ignore
        dbContext.SubmitUpdates()

    let addNewDocument (db : D) (name : string) (UserId userId) =
        db.Document.Create(Userid = userId, Name = name, Jobname = "", Customvariables = "") |> ignore
        dbContext.SubmitUpdates()

    let addHtmlPage (db : D) (documentId : int) (oTemplateId : option<int>) (pageIndex : int) (name : string) =
        db.Htmlpage.Where(fun x -> x.Pageindex >= pageIndex && x.Documentid = documentId)
        |> Seq.iter (fun x -> x.Pageindex <- x.Pageindex + 1)
        dbContext.SubmitUpdates()
        db.Htmlpage.Create(Documentid = documentId, Templateid = oTemplateId, Pageindex = pageIndex, Name = name)
        dbContext.SubmitUpdates()

    let tryGetDocumentIdOffset (db : D) (UserId userId) (documentIndex : int) =
        let documentIds = db.Document.Where(fun x -> x.Userid = userId).Select(fun x -> x.Id)
        let oDocumentId =
            if documentIds.Count() >= documentIndex
            then Some (documentIds.Skip(documentIndex).First())
            else None
        oDocumentId

    let createLink (db : D) (filePath : string) (name : string) =
        let linkGuid = Guid.NewGuid().ToString("N")
        db.Link.Create(Path = filePath, Guid = linkGuid, Name = name) |> ignore
        dbContext.SubmitUpdates()
        linkGuid

    let getDeletableFilePaths (db : D) (documentId : int) =
        query {
            for filePage in db.Filepage do
            where ((filePage.Documentid = documentId)
                  && not <| db.Filepage.Any(fun x -> x.Path = filePage.Path && x.Id <> filePage.Id))
            select filePage.Path
        } |> List.ofSeq

    let deleteDeletableDocumentFilePages (db : D) (documentId : int) =
        query {
            for filePage in db.Filepage do
            where ((filePage.Documentid = documentId)
                  && not <| db.Filepage.Any(fun x -> x.Path = filePage.Path && x.Id <> filePage.Id))
        } |> Seq.iter (fun x -> x.Delete())
        dbContext.SubmitUpdates()

    let tryGetPathAndNameByLinkGuid (db : D) (linkGuid : string) =
        let oPathAndName = db.Link.Where(fun x -> x.Guid = linkGuid).Select(fun x -> Some (x.Path, x.Name)).SingleOrDefault()
        oPathAndName

    let deleteLink (db : D) (linkGuid : string) =
        db.Link.Where(fun x -> x.Guid = linkGuid) |> Seq.iter (fun x -> x.Delete())
        dbContext.SubmitUpdates()

    let getFilesWithExtension (db : D) (extension : string) (UserId userId) =
        let files =
            query {
                for filePage in db.Filepage do
                join document in db.Document on (filePage.Documentid = document.Id)
                where (filePage.Path.EndsWith("." + extension) && document.Userid = userId)
                select filePage.Path
            } |> List.ofSeq
        files


    let getFilePageNames (db : D) (DocumentId documentId) =
        let filePageNames = db.Filepage.Where(fun x -> x.Documentid = documentId).Select(fun x -> x.Name) |> List.ofSeq
        filePageNames

    let tryFindSentApplication (db : D) (employer : Employer) (UserId userId) =
        let employers =
                db.Employer
                  .Where(fun x -> (x.Email = employer.email) && x.Userid = userId)
                  .OrderByDescending(fun x -> x.Id)
                |> List.ofSeq
        if employers |> List.isEmpty
        then None
        else Some 1

    
    let setPasswordSaltAndConfirmEmailGuid (db : D) (password : string) (salt : string) (oConfirmEmailGuid : option<string>) (UserId userId) =
        let user = db.Users.Where(fun x -> x.Id = userId).Single()
        user.Password <- password
        user.Salt <- salt
        user.Confirmemailguid <- oConfirmEmailGuid
        dbContext.SubmitUpdates()
