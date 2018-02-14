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
    open Chessie.ErrorHandling
    open System.Transactions

    type D = DB.dataContext

    let log = LogManager.GetLogger(MethodBase.GetCurrentMethod().GetType())

    let single (xs : seq<'a>) =
        match xs.Select(Some).SingleOrDefault() with
        | Some v -> v
        | None ->
            failwith
                (if xs.Any()
                then 
                    sprintf "Expected one result, found %i" (xs.Count())
                else
                    "Expected one result, found 0")
    

    let getEmailByUserId (UserId userId) (dbContext : D) =
        dbContext.Public.Users
          .Where(fun x -> x.Id = userId)
          .Select(fun x -> Option.map id x.Email)
        |> single

        
    let tryGetUserIdByEmail (email : string) (dbContext : D) = 
        dbContext.Public.Users.Where(fun x -> x.Email.IsSome && x.Email.Value = email).Select(fun x -> Some x.Id).SingleOrDefault()
    
    let insertLastLogin (time : DateTime) (UserId userId) (dbContext : D) =
        let dbLogin = dbContext.Public.Login.Create()
        dbLogin.Userid <- userId
        dbLogin.Loggedinat <- time
    
    let insertEmployer (employer : Employer) (UserId userId) (dbContext : D) =
        let dbEmployer =
            dbContext.Public.Employer.Create(
                Userid = userId
              , Company = employer.company
              , Street = employer.street
              , Postcode = employer.postcode
              , City = employer.city
              , Gender = employer.gender.ToString()
              , Degree = employer.degree
              , Firstname = employer.firstName
              , Lastname = employer.lastName
              , Email = employer.email
              , Phone = employer.phone
              , Mobilephone = employer.mobilePhone)
        dbEmployer

    let getUserValues (UserId userId) (dbContext : D) =
        query {
            for userValues in dbContext.Public.Uservalues do
            join user in dbContext.Public.Users on (userValues.Userid = user.Id)
            where (userValues.Userid = userId)
            select
                { gender  = Gender.fromString userValues.Gender
                  degree = userValues.Degree

                  firstName = userValues.Firstname
                  lastName = userValues.Lastname
                  street = userValues.Street
                  postcode = userValues.Postcode
                  city = userValues.City
                  phone = userValues.Phone
                  mobilePhone = userValues.Mobilephone
                }
        } |> single

    let setUserValues (userValues : UserValues) (UserId userId) (dbContext : D) =
        dbContext.Public.Uservalues.Where(fun x -> x.Userid = userId)
        |> single
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
    
    let insertUserValues (userValues : UserValues) (UserId userId) (dbContext : D) =
        dbContext.Public.Uservalues.Create(
              Userid = userId
            , Gender = userValues.gender.ToString()
            , Degree = userValues.degree
            , Firstname = userValues.firstName
            , Lastname = userValues.lastName
            , Street = userValues.street
            , Postcode = userValues.postcode
            , City = userValues.city
            , Phone = userValues.phone
            , Mobilephone = userValues.mobilePhone)
        |> ignore

    let setUserEmail (oEmail : option<string>) (UserId userId) (dbContext : D) =
        dbContext.Public.Users.Where(fun x -> x.Id = userId)
        |> single
        |> fun x -> x.Email <- oEmail
    
    let userEmailExists (email : string) (dbContext : D) =
        let emailExists = dbContext.Public.Users.Any(fun x -> x.Email.IsSome && x.Email.Value = email)
        emailExists

    let tryGetUserIdBySessionGuid (sessionGuid : string) (dbContext : D) =
        dbContext.Public.Users
          .Where(fun x -> x.Sessionguid.IsSome && x.Sessionguid.Value = sessionGuid)
          .Select(fun x -> Some (UserId x.Id))
          .SingleOrDefault()
    
    let setSessionGuid (oSessionGuid : option<string>) (UserId userId) (dbContext : D) =
        dbContext.Public.Users.Where(fun x -> x.Id = userId)
        |> single
        |> fun x -> x.Sessionguid <- oSessionGuid

    let tryGetValidateLoginData (email : string) (dbContext : D) =
            dbContext.Public.Users
              .Where(fun x -> x.Email.IsSome && x.Email.Value = email)
              .Select(fun x ->
                    Some
                        { userId = UserId x.Id
                          userEmail = email
                          hashedPassword = x.Password
                          salt = x.Salt
                          confirmEmailGuid = x.Confirmemailguid
                        })
            |> single
    
    let insertUser
            (oEmail : option<string>)
            (password : string)
            (salt : string)
            (oConfirmEmailGuid : option<string>)
            (oSessionGuid : option<string>)
            (createdOn : DateTime)
            (dbContext : D) =
        dbContext.Public.Users.Create(
            Email = oEmail
          , Password = password
          , Salt = salt
          , Confirmemailguid = oConfirmEmailGuid
          , Sessionguid = oSessionGuid
          , Createdon = createdOn)

    let tryGetConfirmEmailGuid (email : string) (dbContext : D) =
        dbContext.Public.Users
          .Where(fun x -> x.Email.IsSome && x.Email.Value = email)
          .Select(fun x -> Option.map id x.Confirmemailguid)
        |> single

    let tryGetConfirmEmailGuidByUserId (UserId userId) (dbContext : D) =
            dbContext.Public.Users
              .Where(fun x -> x.Id = userId)
              .Select(fun x -> Option.map id x.Confirmemailguid)
            |> single

    let setConfirmEmailGuidToNull (dbContext : D) (email : string) =
        let users =
            dbContext.Public.Users
              .Where(fun x -> Option.map id x.Email = Some email)
              .Select(fun x -> x)
        users |> Seq.iter (fun x -> x.Confirmemailguid <- None)
        users.Count()

    let insertNotYetSentApplication
            (employerId : int)
            (email : DocumentEmail)
            (userEmail, userValues : UserValues)
            (sentFilePages : list<string * int>) // path * pageIndex
            (jobName : string)
            (url : string)
            (customVariables : string)
            (statusChangedOn : DateTime)
            (UserId userId)
            (dbContext : D) =
        let sentDocumentEmail = dbContext.Public.Sentdocumentemail.Create(Subject = email.subject, Body = email.body)
        dbContext.SubmitUpdates()
        let sentUserValues =
            dbContext.Public.Sentuservalues.Create(
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
            dbContext.Public.Sentdocument.Create(
                Employerid = employerId
              , Sentdocumentemailid = sentDocumentEmail.Id
              , Sentuservaluesid = sentUserValues.Id
              , Jobname = jobName
              , Customvariables = customVariables)
        dbContext.SubmitUpdates()
        for (path, pageIndex) in sentFilePages do
            dbContext.Public.Sentfilepage.Create(Sentdocumentid = sentDocument.Id, Path = path, Pageindex = pageIndex) |> ignore
            dbContext.SubmitUpdates()
        let sentApplication = dbContext.Public.Sentapplication.Create(Userid = userId, Sentdocumentid = sentDocument.Id, Url = url)
        dbContext.SubmitUpdates()
        dbContext.Public.Sentstatus.Create(
            Sentapplicationid = sentApplication.Id
          , Statuschangedon = statusChangedOn
          , Dueon = None
          , Sentstatusvalueid = 1
          , Statusmessage = "") |> ignore
        dbContext.SubmitUpdates()

    let getSentApplications (UserId userId) (dbContext : D) =
        let sentApplications =
            query {
                for sentApplication in dbContext.Public.Sentapplication do
                join sentStatus in dbContext.Public.Sentstatus on (sentApplication.Id = sentStatus.Sentapplicationid)
                join sentDocument in dbContext.Public.Sentdocument on (sentApplication.Sentdocumentid = sentDocument.Id)
                join employer in dbContext.Public.Employer on (sentDocument.Employerid = employer.Id)
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
    
    let getSentApplication (sentApplicationId : int) (dbContext : D) =
        query {
            for sentApplication in dbContext.Public.Sentapplication do
            join sentDocument in dbContext.Public.Sentdocument on (sentApplication.Sentdocumentid = sentDocument.Id)
            join employer in dbContext.Public.Employer on (sentDocument.Employerid = employer.Id)
            join sentDocumentEmail in dbContext.Public.Sentdocumentemail on (sentDocument.Sentdocumentemailid = sentDocumentEmail.Id)
            join sentUserValues in dbContext.Public.Sentuservalues on (sentDocument.Sentuservaluesid = sentUserValues.Id)
            join sentStatus in dbContext.Public.Sentstatus on (sentApplication.Id = sentStatus.Sentapplicationid)
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
                                 dbContext.Public.Sentfilepage
                                   .Where(fun x -> x.Sentdocumentid = sentDocumentId)
                                   .Select(fun x -> x.Path, x.Pageindex)
                                 |> List.ofSeq
                         }
         )
         |> fun x -> x.SingleOrDefault()

    let getSentApplicationOffset (sentApplicationOffset : int) (UserId userId) (dbContext : D) =
        query {
            for sentStatus in dbContext.Public.Sentstatus do
            join sentApplication in dbContext.Public.Sentapplication on
                (sentStatus.Sentapplicationid = sentApplication.Id)
            where (sentApplication.Userid = userId)
            skip sentApplicationOffset
            select (getSentApplication sentApplication.Id dbContext)
        } |> fun x -> x.SingleOrDefault()

    let getNotYetSentApplicationIds (dbContext : D) =
        query {
            for sentStatus1 in dbContext.Public.Sentstatus do
            where (sentStatus1.Sentapplicationid |<>|
                    query {
                        for sentStatus2 in dbContext.Public.Sentstatus do
                        where (sentStatus2.Sentstatusvalueid >= 2)
                        select sentStatus2.Sentapplicationid
                        distinct
                    }
            )
            select sentStatus1.Sentapplicationid
        } |> List.ofSeq

    let overwriteDocument (document : Document) (UserId userId) (dbContext : D) =
        match document.oId with
        | None -> log.Error("Documentid was None")
        | Some (DocumentId documentId) ->
            let dbDocument =
                dbContext.Public.Document.Where(fun x -> x.Id = documentId) |> single
            dbDocument.Jobname <- document.jobName
            dbDocument.Customvariables <- document.customVariables
            dbDocument.Name <- document.name
            dbContext.SubmitUpdates()
            match dbContext.Public.Documentemail.Where(fun x -> x.Documentid = documentId) |> List.ofSeq with
            | [] -> dbContext.Public.Documentemail.Create(Documentid = documentId, Subject = document.email.subject, Body = document.email.body) |> ignore
            | [x] ->
                x.Subject <- document.email.subject
                x.Body <- document.email.body
            | _ -> failwith "There should only be one email for a document"
            dbContext.SubmitUpdates()

            dbContext.Public.Htmlpage.Where(fun x -> x.Documentid = documentId) |> Seq.iter (fun x -> x.Delete())
            dbContext.SubmitUpdates()
            dbContext.Public.Filepage.Where(fun x -> x.Documentid = documentId) |> Seq.iter (fun x -> x.Delete())
            dbContext.SubmitUpdates()
            for page in document.pages do
                dbContext.Public.Pagemap.Where(fun x -> x.Documentid = documentId && x.Pageindex = page.PageIndex()) |> Seq.iter (fun x -> x.Delete())
                dbContext.SubmitUpdates()
                match page with
                | HtmlPage htmlPage ->
                    dbContext.Public.Htmlpage.Create(
                        Documentid = documentId
                      , Templateid = htmlPage.oTemplateId
                      , Pageindex = htmlPage.pageIndex
                      , Name = htmlPage.name) |> ignore
                    dbContext.SubmitUpdates()

                    for mapItem in htmlPage.map do
                        dbContext.Public.Pagemap.Create(
                            Documentid = documentId
                          , Pageindex = htmlPage.pageIndex
                          , Key = fst mapItem
                          , Value = snd mapItem) |> ignore
                        dbContext.SubmitUpdates()
                | FilePage filePage ->
                    dbContext.Public.Filepage.Create(
                        Documentid = documentId
                      , Path = filePage.path
                      , Pageindex = filePage.pageIndex
                      , Name = filePage.name) |> ignore
                    dbContext.SubmitUpdates()
    
    let insertDocument (document : Document) (UserId userId) (dbContext : D) =
        let dbDocument =
            dbContext.Public.Document.Create( Userid = userId, Name = document.name,
                                Jobname = document.jobName, Customvariables = document.customVariables)
        dbContext.SubmitUpdates()
        dbContext.Public.Documentemail.Create(Documentid=dbDocument.Id, Subject=document.email.subject, Body=document.email.body) |> ignore
        for page in document.pages do
            match page with
            | HtmlPage htmlPage ->
                dbContext.Public.Htmlpage.Create(
                    Documentid = dbDocument.Id
                  , Templateid = htmlPage.oTemplateId
                  , Pageindex = htmlPage.pageIndex
                  , Name = htmlPage.name) |> ignore
                for mapItem in htmlPage.map do
                    dbContext.Public.Pagemap.Create( Documentid = dbDocument.Id, Pageindex = htmlPage.pageIndex
                                     , Key = fst mapItem, Value = snd mapItem) |> ignore
            | FilePage filePage ->
                    dbContext.Public.Filepage.Create( Documentid = dbDocument.Id, Path = filePage.path
                                      , Pageindex = filePage.pageIndex, Name = filePage.name)
                    |> ignore
        DocumentId dbDocument.Id

    let deleteDocument (documentId : int) (dbContext : D) =
        dbContext.Public.Pagemap.Where(fun x -> x.Documentid = documentId) |> Seq.iter (fun x -> x.Delete())
        dbContext.Public.Htmlpage.Where(fun x -> x.Documentid = documentId) |> Seq.iter (fun x -> x.Delete())
        dbContext.Public.Filepage.Where(fun x -> x.Documentid = documentId) |> Seq.iter (fun x -> x.Delete())
        dbContext.Public.Documentemail.Where(fun x -> x.Documentid = documentId).Single().Delete()
        dbContext.Public.Lastediteddocumentid.Where(fun x -> x.Documentid = documentId) |> Seq.iter (fun x -> x.Delete())
        dbContext.Public.Document.Where(fun x -> x.Id = documentId).Single().Delete()
       
    let getDocument (documentId : int) (dbContext : D) =
        let oDocumentValues = 
            (query {
                for document in dbContext.Public.Document do
                join documentEmail in dbContext.Public.Documentemail on (document.Id = documentEmail.Documentid)
                where (document.Id = documentId)
                select (Some (document.Name, document.Jobname, documentEmail.Subject, documentEmail.Body, document.Customvariables))
            }).SingleOrDefault()
        match oDocumentValues with
        | None ->
            None
        | Some (documentName, jobName, emailSubject, emailBody, customVariables) ->
            let htmlPages = 
                dbContext.Public.Htmlpage
                  .Where(fun x -> x.Documentid = documentId)
                  .OrderBy(fun x -> x.Pageindex)
                  .Select(fun x ->
                      HtmlPage
                        { map = 
                              dbContext.Public.Pagemap
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
                dbContext.Public.Filepage
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
    
    let getDocumentOffset (documentOffset : int) (UserId userId) (dbContext : D) =
        try
            dbContext.Public.Document
              .Where(fun x -> x.Userid = userId)
              .OrderBy(fun x -> x.Id)
              .Skip(documentOffset)
              .Select(fun x -> Some x.Id)
            |> single
            |> Option.bind (fun documentId -> getDocument documentId dbContext)
        with
        | e ->
            log.Error ("", e)
            None

    let getDocumentNames (UserId userId) (dbContext : D) =
        let documentNames =
            dbContext.Public.Document
              .Where(fun x -> x.Userid = userId)
              .OrderBy(fun x -> x.Id)
              .Select(fun x -> x.Name)
            |> List.ofSeq
        documentNames

    let getHtmlPageTemplatePath (htmlPageTemplateId : int) (dbContext : D) =
        let oOdtPath = dbContext.Public.Htmlpagetemplate.Where(fun x -> x.Id = htmlPageTemplateId).Select(fun x -> Some x.Odtpath).SingleOrDefault()
        oOdtPath

    let getHtmlPageTemplates (dbContext : D) =
        let htmlPageTemplates = dbContext.Public.Htmlpagetemplate.OrderBy(fun x -> x.Id).Select(fun x -> { id = x.Id; html = x.Html; name = x.Name }) |> List.ofSeq
        htmlPageTemplates

    let getHtmlPageTemplate (templateId : int) (dbContext : D) =
        let oHtml = dbContext.Public.Htmlpagetemplate.Where(fun x -> x.Id = templateId).Select(fun x -> Some x.Html).SingleOrDefault()
        oHtml

    let getHtmlPages (documentId : int) (dbContext : D) =
        let htmlPages = dbContext.Public.Htmlpage.Where(fun x -> x.Documentid = documentId).Select(fun x -> { name = x.Name; oTemplateId = x.Templateid }) |> List.ofSeq
        ()

    let getLastEditedDocumentOffset (UserId userId)  (dbContext : D)=
        let documentOffset =
            query {
                for document in dbContext.Public.Document do
                join lastEditedDocumentId in dbContext.Public.Lastediteddocumentid on (document.Userid = lastEditedDocumentId.Userid)
                where (lastEditedDocumentId.Userid = userId)
                select (document.Id, lastEditedDocumentId.Documentid)
            }
            |> Seq.filter(fun (x1, x2) -> x1 < x2)
            |> Seq.length
        documentOffset

    let setLastEditedDocumentId (DocumentId documentId) (UserId userId) (dbContext : D) =
        match dbContext.Public.Lastediteddocumentid
                .Where(fun x -> x.Userid = userId)
                .Select(fun x -> Some x)
                .SingleOrDefault() with
        | None ->
            dbContext.Public.Lastediteddocumentid.Create(Documentid = documentId, Userid = userId) |> ignore
        | Some v ->
            v.Documentid <- documentId
            v.Userid <- userId

    let getPageMapOffset (pageIndex : int) (documentIndex : int) (UserId userId) (dbContext : D) =
        query {
            for pageMap in dbContext.Public.Pagemap do
            join document in dbContext.Public.Document on (pageMap.Documentid = document.Id)
            where ((document.Userid = userId)
                   && (document.Id = dbContext.Public.Document.Where(fun x -> x.Userid = userId).Select(fun x -> x.Id).Skip(documentIndex).Single()))
            select (pageMap.Key, pageMap.Value)
        } |> List.ofSeq
    
    let insertFilePage (documentId : int) (path : string) (pageIndex : int) (name : string) (dbContext : D) =
        dbContext.Public.Filepage.Where(fun x -> x.Pageindex >= pageIndex && x.Documentid = documentId)
        |> Seq.iter (fun x -> x.Pageindex <- x.Pageindex + 1)
        dbContext.Public.Filepage.Create(Documentid = documentId, Path = path, Pageindex = pageIndex, Name = name) |> ignore

    let insertHtmlPage (documentId : int) (oTemplateId : option<int>) (pageIndex : int) (name : string) (dbContext : D) =
        dbContext.Public.Htmlpage.Where(fun x -> x.Pageindex >= pageIndex && x.Documentid = documentId)
        |> Seq.iter (fun x -> x.Pageindex <- x.Pageindex + 1)
        dbContext.Public.Htmlpage.Create(Documentid = documentId, Templateid = oTemplateId, Pageindex = pageIndex, Name = name) |> ignore

    let tryGetDocumentIdOffset (documentIndex : int) (UserId userId) (dbContext : D) =
        let documentIds = dbContext.Public.Document.Where(fun x -> x.Userid = userId).Select(fun x -> x.Id)
        let oDocumentId =
            if documentIds.Count() >= documentIndex
            then Some (documentIds.Skip(documentIndex).First())
            else None
        oDocumentId

    let insertLink (filePath : string) (name : string) (linkGuid : string) (dbContext : D) =
        dbContext.Public.Link.Create(Path = filePath, Guid = linkGuid, Name = name) |> ignore

    let getDeletableFilePaths (documentId : int) (dbContext : D) =
        query {
            for filePage in dbContext.Public.Filepage do
            where ((filePage.Documentid = documentId)
                  && not <| dbContext.Public.Filepage.Any(fun x -> x.Path = filePage.Path && x.Id <> filePage.Id))
            select filePage.Path
        } |> List.ofSeq

    let deleteDeletableDocumentFilePages (documentId : int) (dbContext : D) =
        query {
            for filePage in dbContext.Public.Filepage do
            where ((filePage.Documentid = documentId)
                  && not <| dbContext.Public.Filepage.Any(fun x -> x.Path = filePage.Path && x.Id <> filePage.Id))
        } |> Seq.iter (fun x -> x.Delete())

    let tryGetPathAndNameByLinkGuid (linkGuid : string) (dbContext : D) =
        let oPathAndName = dbContext.Public.Link.Where(fun x -> x.Guid = linkGuid).Select(fun x -> Some (x.Path, x.Name)).SingleOrDefault()
        oPathAndName

    let deleteLink (linkGuid : string) (dbContext : D) =
        dbContext.Public.Link.Where(fun x -> x.Guid = linkGuid) |> Seq.iter (fun x -> x.Delete())

    let getFilesWithExtension (extension : string) (UserId userId) (dbContext : D) =
        let files =
            query {
                for filePage in dbContext.Public.Filepage do
                join document in dbContext.Public.Document on (filePage.Documentid = document.Id)
                where (filePage.Path.EndsWith("." + extension) && document.Userid = userId)
                select filePage.Path
            } |> List.ofSeq
        files

    let getFilePageNames (DocumentId documentId) (dbContext : D) =
        let filePageNames = dbContext.Public.Filepage.Where(fun x -> x.Documentid = documentId).Select(fun x -> x.Name) |> List.ofSeq
        filePageNames

    let tryGetSentApplication (employer : Employer) (UserId userId) (dbContext : D) =
        let employers =
                dbContext.Public.Employer
                  .Where(fun x -> (x.Email = employer.email) && x.Userid = userId)
                  .OrderByDescending(fun x -> x.Id)
                |> List.ofSeq
        if employers |> List.isEmpty
        then None
        else Some 1
    
    let setPasswordSaltAndConfirmEmailGuid (password : string) (salt : string) (oConfirmEmailGuid : option<string>) (UserId userId) (dbContext : D) =
        let user = dbContext.Public.Users.Where(fun x -> x.Id = userId).Single()
        user.Password <- password
        user.Salt <- salt
        user.Confirmemailguid <- oConfirmEmailGuid



