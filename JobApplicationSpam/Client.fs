
namespace JobApplicationSpam

open WebSharper
open WebSharper.UI.Next
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Html
open WebSharper.JavaScript


module Client =

    open Server
    open Chessie.ErrorHandling
    open JobApplicationSpam.Types
    open System.Web
    open System.IO
    open WebSharper.Sitelets
    open HtmlAgilityPack
    open WebSharper.JQuery
    open System

    [<JavaScript>]
    type Language =
        | English
        | German
    
    [<JavaScript>]
    type Str =
        | StrMale
        | StrFemale
        | StrAddEmployer
        | StrCompanyName
        | StrCompanyStreet
        | StrCompanyPostcode
        | StrCompanyCity
        | StrBossGender
        | StrBossDegree
        | StrBossFirstName
        | StrBossLastName
        | StrBossEmail
        | StrBossPhone
        | StrBossMobilePhone
        | StrUploadTemplate
        | StrTemplateName
        | StrEmailSubject
        | StrEmailBody
        | StrUserAppliesAs
        | StrAddFile
        | StrEditUserValues
        | StrSubmitEditUserValues
        | StrUserValuesGender
        | StrUserValuesDegree
        | StrUserValuesFirstName
        | StrUserValuesLastName
        | StrUserValuesStreet
        | StrUserValuesPostcode
        | StrUserValuesCity
        | StrUserValuesPhone
        | StrUserValuesMobilePhone

    [<JavaScript>]
    let translate language str =
        match language, str with
        | English, StrMale -> "male"
        | English, StrFemale -> "female"
        | English, StrAddEmployer -> "Add Employer"
        | English, StrCompanyName -> "Company name"
        | English, StrBossFirstName -> "First name"
        | English, StrBossLastName -> "Last name"
        | English, StrCompanyStreet -> "Street"
        | English, StrCompanyPostcode -> "Postcode"
        | English, StrCompanyCity -> "City"
        | English, StrBossGender -> "Gender"
        | English, StrBossDegree -> "Degree"
        | English, StrBossEmail -> "Email"
        | English, StrBossPhone -> "Phone"
        | English, StrBossMobilePhone -> "Mobile phone"
        | English, StrUploadTemplate -> "Add template"
        | English, StrEmailSubject -> "Email subject"
        | English, StrEmailBody -> "Email body"
        | English, StrTemplateName -> "Template name"
        | English, StrUserAppliesAs -> "Job"
        | English, StrAddFile -> "Add file"
        | English, StrEditUserValues -> "Edit your values"
        | English, StrSubmitEditUserValues -> "Save values"
        | English, StrUserValuesGender -> "Gender"
        | English, StrUserValuesDegree -> "Degree"
        | English, StrUserValuesFirstName -> "First name"
        | English, StrUserValuesLastName -> "Last name"
        | English, StrUserValuesStreet -> "Street"
        | English, StrUserValuesPostcode -> "Postcode"
        | English, StrUserValuesCity -> "City"
        | English, StrUserValuesPhone -> "Phone"
        | English, StrUserValuesMobilePhone -> "Mobile phone"
        | German, StrMale -> "männlich"
        | German, StrFemale -> "weiblich"
        | German, StrAddEmployer -> "Arbeitgeber eingeben"
        | German, StrCompanyName -> "Firmenname"
        | German, StrCompanyStreet -> "Straße"
        | German, StrCompanyPostcode -> "Postleitzahl"
        | German, StrCompanyCity -> "Stadt"
        | German, StrBossGender -> "Geschlecht"
        | German, StrBossDegree -> "Titel"
        | German, StrBossFirstName -> "Vorname"
        | German, StrBossLastName -> "Nachname"
        | German, StrBossEmail -> "Email"
        | German, StrBossPhone -> "Telefon"
        | German, StrBossMobilePhone -> "Mobiltelefon"
        | German, StrUploadTemplate -> "Vorlage hochladen"
        | German, StrEmailSubject -> "Email-Betreff"
        | German, StrEmailBody -> "Email-Text"
        | German, StrTemplateName -> "Name der Vorlage"
        | German, StrUserAppliesAs -> "Beruf"
        | German, StrAddFile -> "Datei hinzufügen"
        | German, StrEditUserValues -> "Angaben zum Bewerber"
        | German, StrSubmitEditUserValues -> "Speichern"
        | German, StrUserValuesGender -> "Geschlecht"
        | German, StrUserValuesDegree -> "Titel"
        | German, StrUserValuesFirstName -> "Vorname"
        | German, StrUserValuesLastName -> "Nachname"
        | German, StrUserValuesStreet -> "Straße"
        | German, StrUserValuesPostcode -> "PLZ"
        | German, StrUserValuesCity -> "Stadt"
        | German, StrUserValuesPhone -> "Telefon"
        | German, StrUserValuesMobilePhone -> "Mobiltelefon"

    
    [<JavaScript>]
    let currentLanguage = German


    [<JavaScript>]
    type AddEmployerAction =
    | ApplyImmediately
    | SendJobApplicationToUserOnly
    | JustAddEmployer


    [<JavaScript>]
    let editUserValues () : Elt =
        let createUserValues gender degree firstName lastName street postcode city phone mobilePhone =
            { gender = gender
              degree = degree
              firstName = firstName
              lastName = lastName
              street = street
              postcode = postcode
              city = city
              phone = phone
              mobilePhone = mobilePhone
            }
        let varMessage = Var.Create("nothing")
        let subm userValues =
            async {
                Var.Set varMessage ("Setting user values...")
                let! result = Server.setUserValues userValues
                let m =
                    match result with
                    | Ok (v, _) ->  v
                    | Bad v -> sprintf "%A" v
                do! Async.Sleep(1000)
                varMessage.Value <- m
                return ()
            } |> Async.StartImmediate
        let varGender = Var.Create Gender.Male 
        let varDegree = Var.Create("")
        let varFirstName = Var.Create("")
        let varLastName = Var.Create("")
        let varStreet = Var.Create("")
        let varPostcode = Var.Create("")
        let varCity = Var.Create("")
        let varPhone = Var.Create("")
        let varMobilePhone = Var.Create("")
        async {
            let! currentUserValues = Server.getCurrentUserValues()
            match currentUserValues with 
            | Some userValues ->
                varGender.Value <- userValues.gender
                varDegree.Value <- userValues.degree
                varFirstName.Value <- userValues.firstName
                varLastName.Value <- userValues.lastName
                varStreet.Value <- userValues.street
                varPostcode.Value <- userValues.postcode
                varCity.Value <- userValues.city
                varPhone.Value <- userValues.phone
                varMobilePhone.Value <- userValues.mobilePhone
            | None -> ()
        } |> Async.Start
        div [  h1 [text (translate currentLanguage StrEditUserValues)]
               divAttr
                 [ attr.``class`` "form-group" ]
                 [ labelAttr [attr.``class`` "control-label" ] [ text <| translate currentLanguage StrUserValuesGender ]
                   br []
                   divAttr [ attr.``class`` "" ]
                     [ Doc.Radio [ attr.id "male"; attr.radiogroup "gender"; ] Gender.Male varGender
                       labelAttr [ attr.``for`` "male"; ] [text <| translate currentLanguage StrMale]
                       br []
                       Doc.Radio [ attr.id "female"; attr.radiogroup "gender"; ] Gender.Female varGender
                       labelAttr [ attr.``for`` "female"; ] [text <| translate currentLanguage StrFemale]
                     ]
                 ]
               divAttr
                 [ attr.``class`` "form-group" ]
                 [ labelAttr
                     [attr.``for`` "degree"; attr.``class`` "control-label"]
                     [text <| translate currentLanguage StrUserValuesDegree]
                   divAttr [ attr.``class`` "" ]
                     [ Doc.Input
                         [ attr.``class`` "form-control"
                           attr.``type`` "input"
                           attr.placeholder <| translate currentLanguage StrUserValuesDegree
                           attr.id "degree"; attr.value varDegree.Value ] varDegree
                     ]
                 ]
               divAttr
                 [ attr.``class`` "form-group" ]
                 [ labelAttr
                     [attr.``for`` "firstName"; attr.``class`` "control-label"]
                     [text <| translate currentLanguage StrUserValuesFirstName]
                   divAttr [ attr.``class`` "" ]
                     [ Doc.Input
                         [ attr.``class`` "form-control"
                           attr.``type`` "input"
                           attr.placeholder <| translate currentLanguage StrUserValuesFirstName
                           attr.id "firstName"; attr.value varFirstName.Value ] varFirstName
                     ]
                 ]
               divAttr
                 [ attr.``class`` "form-group" ]
                 [ labelAttr
                     [attr.``for`` "lastName"; attr.``class`` "control-label"]
                     [text <| translate currentLanguage StrUserValuesLastName]
                   divAttr [ attr.``class`` "" ]
                     [ Doc.Input
                         [ attr.``class`` "form-control"
                           attr.``type`` "input"
                           attr.placeholder <| translate currentLanguage StrUserValuesLastName
                           attr.id "lastName"; attr.value varLastName.Value ] varLastName
                     ]
                 ]
               divAttr
                 [ attr.``class`` "form-group" ]
                 [ labelAttr
                     [attr.``for`` "street"; attr.``class`` "control-label"]
                     [text <| translate currentLanguage StrUserValuesStreet]
                   divAttr [ attr.``class`` "" ]
                     [ Doc.Input
                         [ attr.``class`` "form-control"
                           attr.``type`` "input"
                           attr.placeholder <| translate currentLanguage StrUserValuesStreet
                           attr.id "street"; attr.value varStreet.Value ] varStreet
                     ]
                 ]
               divAttr
                 [ attr.``class`` "form-group" ]
                 [ labelAttr
                     [attr.``for`` "postcode"; attr.``class`` "control-label"]
                     [text <| translate currentLanguage StrUserValuesPostcode]
                   divAttr [ attr.``class`` "" ]
                     [ Doc.Input
                         [ attr.``class`` "form-control"
                           attr.``type`` "input"
                           attr.placeholder <| translate currentLanguage StrUserValuesPostcode
                           attr.id "postcode"; attr.value varPostcode.Value ] varPostcode
                     ]
                 ]
               divAttr
                 [ attr.``class`` "form-group" ]
                 [ labelAttr
                     [attr.``for`` "city"; attr.``class`` "control-label"]
                     [text <| translate currentLanguage StrUserValuesCity]
                   divAttr [ attr.``class`` "" ]
                     [ Doc.Input
                         [ attr.``class`` "form-control"
                           attr.``type`` "input"
                           attr.placeholder <| translate currentLanguage StrUserValuesCity
                           attr.id "city"; attr.value varCity.Value ] varCity
                     ]
                 ]
               divAttr
                 [ attr.``class`` "form-group" ]
                 [ labelAttr
                     [attr.``for`` "phone"; attr.``class`` "control-label"]
                     [text <| translate currentLanguage StrUserValuesPhone]
                   divAttr [ attr.``class`` "" ]
                     [ Doc.Input
                         [ attr.``class`` "form-control"
                           attr.``type`` "input"
                           attr.placeholder <| translate currentLanguage StrUserValuesPhone
                           attr.id "phone"; attr.value varPhone.Value ] varPhone
                     ]
                 ]
               divAttr
                 [ attr.``class`` "form-group" ]
                 [ labelAttr
                     [attr.``for`` "mobilePhone"; attr.``class`` "control-label"]
                     [text <| translate currentLanguage StrUserValuesMobilePhone]
                   divAttr [ attr.``class`` "" ]
                     [ Doc.Input
                         [ attr.``class`` "form-control"
                           attr.``type`` "input"
                           attr.placeholder <| translate currentLanguage StrUserValuesMobilePhone
                           attr.id "mobilePhone"; attr.value varMobilePhone.Value ] varMobilePhone
                     ]
                 ]

               Doc.Button (translate currentLanguage StrSubmitEditUserValues) [attr.``type`` "submit"; ] (fun () -> subm (createUserValues varGender.Value varDegree.Value varFirstName.Value varLastName.Value varStreet.Value varPostcode.Value varCity.Value varPhone.Value varMobilePhone.Value) |> ignore)
               textView varMessage.View
               //subm <| createUserValues varGender.Value varDegree.Value varFirstName.Value varLastName.Value varStreet.Value varPostcode.Value varCity.Value varPhone.Value varMobilePhone.Value
            ]


    [<JavaScript>]
    let addEmployer () : Elt =
        let createEmployer company street postcode city gender degree firstName lastName email phone mobilePhone =
            { company = company
              street = street
              postcode = postcode
              city = city
              gender = gender
              degree = degree
              firstName = firstName
              lastName = lastName
              email = email
              phone = phone
              mobilePhone = mobilePhone
            }
        let varMessage = Var.Create("")
        let varCanSubmit = Var.Create(true)
        let subm employer templateName =
            let addEmployer123 () : Async<Result<int, string>> =
                async {
                    varMessage.Value <- "Adding employer..."
                    do! Async.Sleep 2000
                    return! Server.addEmployer employer
                }
            let applyNow123 employerId templateName : Async<Result<string, string>> =
                async {
                    varMessage.Value <- "Sending job application..."
                    do! Async.Sleep 2000
                    return! Server.applyNowByTemplateName employerId templateName
                }
            
            
            let b =
                async {
                    let! addEmployerResult = addEmployer123 ()
                    match addEmployerResult with
                    | Ok (employerId, _) ->
                        let! applyNowResult = applyNow123 employerId templateName
                        match applyNowResult with
                        | Ok (v, _) ->
                            varMessage.Value <- "Job application has been sent"
                            return ()
                        | Bad xs ->
                            varMessage.Value <- "Unfortunately, adding employer failed."
                            return ()
                    | Bad xs ->
                        varMessage.Value <- "Unfortunately, adding employer failed."
                        return ()
                }
            b |> Async.Ignore |> Async.StartImmediate
            varCanSubmit.Value <- true
        let varCompany = Var.Create ""
        let varStreet = Var.Create ""
        let varPostcode = Var.Create ""
        let varCity = Var.Create ""
        let varGender = Var.Create Gender.Male 
        let varDegree = Var.Create ""
        let varFirstName = Var.Create ""
        let varLastName = Var.Create ""
        let varEmail = Var.Create ""
        let varPhone = Var.Create ""
        let varMobilePhone = Var.Create ""

        let varTemplate = Var.Create("")
        let varTemplateList = Var.Create([])
        async {
            let! templateList = Server.getTemplateNames ()
            varTemplateList.Value <- templateList
            varTemplate.Value <- Seq.tryItem 0 templateList |> Option.defaultValue ""
        } |> Async.Start
        let varAddEmployerAction = Var.Create JustAddEmployer
        formAttr
          [ attr.``class`` "form-horizontal"; ]
          [ h1 [text <| translate currentLanguage StrAddEmployer]
            divAttr
              [ attr.``class`` "form-group" ]
              [ labelAttr
                  [attr.``for`` "company"; attr.``class`` "control-label"]
                  [text <| translate currentLanguage StrCompanyName]
                divAttr [ attr.``class`` "" ]
                  [ Doc.Input
                      [ attr.``class`` "form-control"
                        attr.``type`` "input"
                        attr.placeholder <| translate currentLanguage StrCompanyName
                        attr.id "company"; attr.value varStreet.Value ] varCompany
                  ]
              ]
            divAttr
              [ attr.``class`` "form-group" ]
              [ labelAttr
                  [attr.``for`` "company"; attr.``class`` "control-label"]
                  [text <| translate currentLanguage StrCompanyStreet]
                divAttr [ attr.``class`` "" ]
                  [ Doc.Input
                      [ attr.``class`` "form-control"
                        attr.``type`` "input"
                        attr.placeholder <| translate currentLanguage StrCompanyStreet
                        attr.id "street"; attr.value varStreet.Value ] varStreet
                  ]
              ]
            divAttr
              [ attr.``class`` "form-group" ]
              [ labelAttr
                  [attr.``for`` "company"; attr.``class`` "control-label"]
                  [text <| translate currentLanguage StrCompanyPostcode]
                divAttr [ attr.``class`` "" ]
                  [ Doc.Input
                      [ attr.``class`` "form-control"
                        attr.``type`` "input"
                        attr.placeholder <| translate currentLanguage StrCompanyPostcode
                        attr.id "postcode"; attr.value varPostcode.Value ] varPostcode
                  ]
              ]
            divAttr
              [ attr.``class`` "form-group" ]
              [ labelAttr
                  [attr.``for`` "city"; attr.``class`` "control-label"]
                  [text <| translate currentLanguage StrCompanyCity]
                divAttr [ attr.``class`` "" ]
                  [ Doc.Input
                      [ attr.``class`` "form-control"
                        attr.``type`` "input"
                        attr.placeholder <| translate currentLanguage StrCompanyCity
                        attr.id "city"; attr.value varCity.Value ] varCity
                  ]
              ]
            divAttr
              [ attr.``class`` "form-group" ]
              [ labelAttr [attr.``class`` "control-label" ] [ text <| translate currentLanguage StrBossGender ]
                br []
                divAttr [ attr.``class`` "" ]
                  [ Doc.Radio [ attr.id "male"; attr.radiogroup "gender"; ] Gender.Male varGender
                    labelAttr [ attr.``for`` "male"; ] [text <| translate currentLanguage StrMale]
                    br []
                    Doc.Radio [ attr.id "female"; attr.radiogroup "gender"; ] Gender.Female varGender
                    labelAttr [ attr.``for`` "female"; ] [text <| translate currentLanguage StrFemale]
                  ]
              ]
            divAttr
              [ attr.``class`` "form-group" ]
              [ labelAttr [attr.``for`` "degree"; attr.``class`` "control-label"] [text <| translate currentLanguage StrBossDegree]
                divAttr [ attr.``class`` "" ]
                  [ Doc.Input
                      [ attr.``class`` "form-control"
                        attr.``type`` "input"
                        attr.placeholder <| translate currentLanguage StrBossDegree
                        attr.id "degree"
                        attr.value varDegree.Value ] varDegree
                  ]
              ]
            divAttr
              [ attr.``class`` "form-group" ]
              [ labelAttr [attr.``for`` "firstName"; attr.``class`` "control-label"] [text <| translate currentLanguage StrBossFirstName]
                divAttr [ attr.``class`` "" ]
                  [ Doc.Input
                      [ attr.``class`` "form-control"
                        attr.``type`` "input"
                        attr.placeholder <| translate currentLanguage StrBossFirstName
                        attr.id "firstName"
                        attr.value varFirstName.Value ] varFirstName
                  ]
              ]
            divAttr
              [ attr.``class`` "form-group" ]
              [ labelAttr [attr.``for`` "lastName"; attr.``class`` "control-label"] [text <| translate currentLanguage StrBossLastName]
                divAttr [ attr.``class`` "" ]
                  [ Doc.Input
                      [ attr.``class`` "form-control"
                        attr.``type`` "input"
                        attr.placeholder <| translate currentLanguage StrBossLastName
                        attr.value varLastName.Value ] varLastName
                  ]
              ]
            divAttr
              [ attr.``class`` "form-group" ]
              [ labelAttr [attr.``for`` "email"; attr.``class`` "control-label"] [text <| translate currentLanguage StrBossEmail]
                divAttr [ attr.``class`` "" ]
                  [ Doc.Input
                      [ attr.``class`` "form-control"
                        attr.``type`` "input"
                        attr.placeholder <| translate currentLanguage StrBossEmail
                        attr.id "email"
                        attr.value varEmail.Value ] varEmail
                  ]
              ]
            divAttr
              [ attr.``class`` "form-group" ]
              [ labelAttr [attr.``for`` "phone"; attr.``class`` "control-label"] [text <| translate currentLanguage StrBossPhone]
                divAttr [ attr.``class`` "" ]
                  [ Doc.Input
                      [ attr.``class`` "form-control"
                        attr.``type`` "input"
                        attr.placeholder <| translate currentLanguage StrBossPhone
                        attr.id "phone"
                        attr.value varPhone.Value ] varPhone
                  ]
              ]
            divAttr
              [ attr.``class`` "form-group" ]
              [ labelAttr [attr.``for`` "mobilePhone"; attr.``class`` "control-label"] [text <| translate currentLanguage StrBossMobilePhone]
                divAttr [ attr.``class`` "" ]
                  [ Doc.Input
                      [ attr.``class`` "form-control"
                        attr.``type`` "input"
                        attr.placeholder <| translate currentLanguage StrBossMobilePhone
                        attr.id "mobilePhone"
                        attr.value varMobilePhone.Value ] varMobilePhone
                  ]
              ]
            divAttr
              [ attr.``class`` "form-group" ]
              [ labelAttr [attr.``class`` "control-label"] [text <| translate currentLanguage StrBossMobilePhone]
                divAttr [ attr.``class`` "" ]
                  [ Doc.Radio
                      [ attr.``class`` "form-control"
                        attr.radiogroup "addEmployerAction"
                        attr.placeholder <| translate currentLanguage StrBossMobilePhone
                        attr.id "mobilePhone"
                        attr.value "hallo"
                      ]
                      ApplyImmediately
                      varAddEmployerAction
                    Doc.Radio
                      [ attr.``class`` "form-control"
                        attr.radiogroup "addEmployerAction"
                        attr.placeholder <| translate currentLanguage StrBossMobilePhone
                        attr.id "mobilePhone"
                        attr.value varMobilePhone.Value
                      ]
                      SendJobApplicationToUserOnly
                      varAddEmployerAction
                    Doc.Radio
                      [ attr.``class`` "form-control"
                        attr.radiogroup "addEmployerAction"
                        attr.placeholder <| translate currentLanguage StrBossMobilePhone
                        attr.id "mobilePhone"
                        attr.value varMobilePhone.Value
                      ]
                      AddEmployerAction.JustAddEmployer
                      varAddEmployerAction
                  ]
              ]
            (if true then
                divAttr
                  [ attr.``class`` "form-group" ]
                  [ labelAttr [attr.``class`` "control-label"] [text <| translate currentLanguage StrBossMobilePhone]
                    divAttr [ attr.``class`` "" ]
                      [ Doc.Select
                          [ attr.``class`` "form-control"
                            attr.``type`` "input"
                            attr.placeholder <| translate currentLanguage StrBossMobilePhone
                            attr.id "mobilePhone"
                            attr.value varMobilePhone.Value
                          ]
                          id
                          varTemplateList.Value
                          varTemplate
                      ]
                  ]
                else div [])
            divAttr
              [ attr.``class`` "form-group" ]
              [ Doc.Button (translate currentLanguage StrAddEmployer) [attr.``type`` "submit"; attr.``class`` "form-control" ] (fun () -> if varCanSubmit.Value then varCanSubmit.Value <- false; subm (createEmployer varCompany.Value varStreet.Value varPostcode.Value varCity.Value varGender.Value varDegree.Value varFirstName.Value varLastName.Value varEmail.Value varPhone.Value varMobilePhone.Value) varTemplate.Value |> ignore)
                            
              ]
            textView varMessage.View
         ]
    open System
    open System.ComponentModel

    [<JavaScript>]
    let applyNow () =
        let varEmployerId = Var.Create("")
        let varTemplateId = Var.Create("")
        let message = Var.Create("abc")

        let submitIt () =
            try
                let employerId = varEmployerId.Value |> Int32.Parse
                let templateId = varTemplateId.Value |> Int32.Parse
                //JS.Document.Body.SetAttribute("style", "cursor:wait")
                async {
                    try
                        let! applyNowResult = Server.applyNow employerId templateId
                        match applyNowResult with
                        | Ok (v, _) -> message.Value <- "Bewerbung wurde versandt."
                        | Bad vs -> message.Value <- "Bewerbung konnte nicht bearbeitet werden."
                        return ()
                    with
                    | e ->
                        message.Value <- "Bewerbung konnte nicht bearbeitet werden." + e.Message
                } |> Async.StartImmediate
            with
            | e ->
                message.Value <- "Es trat ein Fehler auf: " + e.Message
        div
          [ formAttr
              [ attr.action ""; attr.method "post"; on.submit (fun _ ev -> submitIt (); ev.PreventDefault(); ev.StopImmediatePropagation();) ]
              [ labelAttr [] [text "employerId"]
                Doc.Input [attr.``type`` "text"; attr.value varEmployerId.Value] varEmployerId
                br []
                labelAttr [] [text "templateId"]
                Doc.Input [attr.``type`` "text"; attr.value varTemplateId.Value] varTemplateId
                br []
                inputAttr [attr.``type`` "submit"; ] []
              ]
            textView message.View
          ]


    [<JavaScript>]
    let uploadTemplate () =
        let rec createFileDiv n =
                    divAttr
                     [ attr.``class`` "form-group"; ]
                     [ labelAttr
                         [attr.``for`` ("file" + (n.ToString())); attr.``class`` "control-label"]
                         [ text <| translate currentLanguage StrAddFile ]
                       divAttr
                         [attr.``class`` ""]
                         [ inputAttr
                             [ attr.``class`` "form-control"
                               attr.``type`` "file"
                               attr.id ("file" + (n.ToString()))
                               attr.name "file[]"
                               on.change (fun _ _ -> addFileDiv varAddFileDivsDiv ())
                               attr.accept ".odt .pdf .txt"; ]
                             []
                         ]
                     ]
        and varAddFileDivsDiv = Var.Create(div [createFileDiv 0])
                    
        and addFileDiv myDiv () =
            let n = varAddFileDivsDiv.Value.Dom.LastChild.LastChild.LastChild.Attributes.GetNamedItem("id").Value.Substring(4) |> Int32.Parse
            varAddFileDivsDiv.Value.AppendChild (createFileDiv (n + 1))
        
            
        div
            [ h1 [text <| translate currentLanguage StrUploadTemplate]
              formAttr
                 [attr.``class`` "form-horizontal"; attr.enctype "multipart/form-data"; attr.method "POST"; attr.action ""]
                 [ divAttr
                     [ attr.``class`` "form-group"; ]
                     [ labelAttr
                         [attr.``for`` "templateName"; attr.``class`` "control-label" ]
                         [ text <| translate currentLanguage StrTemplateName ]
                       divAttr
                         [attr.``class`` ""]
                         [ inputAttr
                             [ attr.``class`` "form-control"
                               attr.``type`` "text"
                               attr.id "templateName"
                               attr.name "templateName"
                               attr.placeholder <| translate currentLanguage StrTemplateName]
                             []
                         ]
                     ]
                   divAttr
                     [ attr.``class`` "form-group"; ]
                     [ labelAttr
                         [attr.``for`` "emailSubject"; attr.``class`` "control-label" ]
                         [ text <| translate currentLanguage StrUserAppliesAs ]
                       divAttr
                         [attr.``class`` ""]
                         [ inputAttr
                             [ attr.``class`` "form-control"
                               attr.``type`` "text"
                               attr.id "userAppliesAs"
                               attr.name "userAppliesAs"
                               attr.placeholder <| translate currentLanguage StrUserAppliesAs
                             ]
                             []
                         ]
                     ]
                   divAttr
                     [ attr.``class`` "form-group"; ]
                     [ labelAttr
                         [attr.``for`` "emailSubject"; attr.``class`` "control-label" ]
                         [ text <| translate currentLanguage StrEmailSubject ]
                       divAttr
                         [attr.``class`` ""]
                         [ inputAttr
                             [ attr.``class`` "form-control"
                               attr.``type`` "text"
                               attr.id "emailSubject"
                               attr.name "emailSubject"
                               attr.placeholder <| translate currentLanguage StrEmailSubject
                             ]
                             []
                         ]
                     ]
                   divAttr
                     [ attr.``class`` "form-group"; ]
                     [ labelAttr
                         [attr.``for`` "emailBody"; attr.``class`` "control-label"]
                         [ text <| translate currentLanguage StrEmailBody ]
                       divAttr
                         [attr.``class`` ""]
                         [ textareaAttr
                             [ attr.``class`` "form-control"
                               attr.style "min-height:260px"
                               attr.``type`` "text"
                               attr.id "emailBody"
                               attr.name "emailBody"
                               attr.placeholder <| translate currentLanguage StrEmailBody
                             ]
                             []
                         ]
                     ]
                   varAddFileDivsDiv.Value
                   divAttr
                     [ attr.``class`` "form-group"; ]
                     [ labelAttr
                         [attr.``class`` "control-label"]
                         [text ""]
                       divAttr
                         [attr.``class`` ""]
                         [ inputAttr
                             [ attr.``type`` "submit"; attr.value <| translate currentLanguage StrUploadTemplate]
                             []
                         ]
                     ]
                 ]
              (text <| sprintf "logged in as: %A" (Server.getCurrentUserId ()))
            ]
     
     
    [<JavaScript>]
    let login () =
        let varTxtLoginEmail = Var.Create ""
        let varTxtLoginPassword = Var.Create ""
        formAttr
          [ on.submit (fun _ _ ->
              async {
                  let! loginResult = Server.login (varTxtLoginEmail.Value) (varTxtLoginPassword.Value)
                  match loginResult with
                  | Ok (v, _) -> ()
                  | Bad xs -> JS.Alert(String.concat ", " xs)
              } |> Async.Start
              )
          ]
          [ divAttr
              [ attr.``class`` "form-group" ]
              [ labelAttr
                  [ attr.``for`` "txtLoginEmail" ] 
                  [text "Email"]
                Doc.Input
                  [ attr.``class`` "form-control"; attr.id "txtLoginEmail"; attr.placeholder "Email" ] varTxtLoginEmail
              ]
            divAttr
              [ attr.``class`` "form-group" ]
              [ labelAttr
                  [ attr.``for`` "txtLoginPassword" ] 
                  [text "Password"]
                Doc.PasswordBox
                  [ attr.``class`` "form-control"; attr.id "txtLoginPassword"; attr.placeholder "Password" ] varTxtLoginPassword
              ]
            inputAttr [ attr.``type`` "submit"; attr.value "Login" ] []
          ]

    [<JavaScript>]
    let register () =
        let varTxtRegisterEmail = Var.Create ""
        let varTxtRegisterPassword1 = Var.Create ""
        let varTxtRegisterPassword2 = Var.Create ""
        formAttr
          [ on.submit (fun _ _ ->
              async {
                  let x = Server.register (varTxtRegisterEmail.Value) (varTxtRegisterPassword1.Value) (varTxtRegisterPassword2.Value)
                  return ()
              } |> Async.StartImmediate
              )
          ]
          [ divAttr
              [ attr.``class`` "form-group" ]
              [ labelAttr
                  [ attr.``for`` "txtRegisterEmail" ] 
                  [text "Email"]
                Doc.Input
                  [ attr.``class`` "form-control"; attr.id "txtRegisterEmail"; attr.placeholder "Email" ]
                  varTxtRegisterEmail
                labelAttr
                  [ attr.``for`` "txtRegisterPassword1"; ] 
                  [text "Password"]
                Doc.PasswordBox
                  [ attr.``class`` "form-control"; attr.id "txtRegisterPassword1"; attr.placeholder "Password" ]
                  varTxtRegisterPassword1
                labelAttr
                  [ attr.``for`` "txtRegisterPassword2" ] 
                  [text "Password"]
                Doc.PasswordBox [ attr.``class`` "form-control"; attr.id "txtRegisterPassword2"; attr.placeholder "Repeat Password" ] varTxtRegisterPassword2
                inputAttr [ attr.``type`` "submit"; attr.value "Register" ] []
              ]
          ]
    
    [<JavaScript>]
    let showSentJobApplications () =
        h1 [text "hallo"]


    [<JavaScript>]
    let createTemplate () = 
        let varUserTitle = Var.Create("")
        let varUserFirstName = Var.Create("")
        let varUserLastName = Var.Create("")
        let varUserStreet = Var.Create("")
        let varUserPostcode = Var.Create("")
        let varUserCity = Var.Create("")
        let varBossTitulation = Var.Create("")
        let varBossTitle = Var.Create("")
        let varBossFirstName = Var.Create("")
        let varBossLastName = Var.Create("")
        let varCompanyStreet = Var.Create("")
        let varCompanyPostcode = Var.Create("")
        let varCompanyCity = Var.Create("")
        let varSubject = Var.Create("Bewerbung")
        let varTextArea = Var.Create("abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrstuvwxyz1234567890")
        let varCover = Var.Create(Upload)
        let resize (el : JQuery) font fontSize fontWeight (defaultWidth: int) =
            let str = el.Val().ToString().Replace(" ", "&nbsp;")
            if str = ""
            then el.Width(defaultWidth) |> ignore
            else
                let (span : JQuery) = JQuery("<span />").Attr("style", sprintf "font-family:%s; font-size: %s; font-weight: %s; visibility: hidden" font fontSize fontWeight).Html(str)
                span.AppendTo("body") |> ignore
                el.Width(span.Width()) |> ignore
                JQuery("body span").Last().Remove() |> ignore
            ()
        let getWidth (s : string) font fontSize fontWeight =
            let str = s.ToString().Replace(" ", "&nbsp;")
            let span = JQuery("<span />").Attr("style", sprintf "font-family: Arial; font-size: 12pt; font-weight: normal; letter-spacing:0pt; visibility: hidden;").Html(str)
            span.AppendTo("body") |> ignore
            let spanWidth = span.Width()
            JQuery("body span:last").Remove() |> ignore
            spanWidth
        let findLineBreak (str : string) containerWidth font fontSize fontWeight =
            let rec findLineBreak' beginIndex endIndex n =
                if n < 0
                then str.Length
                else
                    let currentIndex = beginIndex + (endIndex - beginIndex + 1) / 2
                    let currentString = str.Substring(0, currentIndex)
                    let width = getWidth currentString font fontSize fontWeight
                    if width > containerWidth
                    then
                        if endIndex = currentIndex
                        then
                            JS.Alert(currentIndex - 1 |> string)
                            currentIndex - 1
                        else
                            let nextEndIndex = currentIndex
                            findLineBreak' beginIndex nextEndIndex (n-1)
                    else
                        let nextBeginIndex = currentIndex
                        findLineBreak' nextBeginIndex endIndex (n-1)
            findLineBreak' 0 (str.Length) 30
        let findLineBreaks (str : string) containerWidth font fontSize fontWeight =
            let lines = str.Split([|'\n'|]) |> Array.map (fun x -> if x.EndsWith(" ") then x.TrimEnd([|' '|]) + "\n" else x) |> List.ofArray
            let splitLines = 
                List.unfold
                    (fun state ->
                        match state with
                        | [] -> None
                        | x::xs ->
                            let splitIndex = findLineBreak x containerWidth font fontSize fontWeight
                            let splitIndexSpace =
                                let ar = x.ToCharArray() |> Array.take splitIndex |> Array.tryFindIndexBack (fun c -> List.contains c [' '; '.'; ','; ';'; '-'])
                                match ar, splitIndex = x.Length with
                                | None, _ -> splitIndex
                                | _, true -> splitIndex
                                | Some v, false -> v + 1
                            JS.Alert(splitIndexSpace |> string)
                            let front = x.Substring(0, splitIndexSpace)
                            let back = x.Substring(splitIndexSpace)
                            match back, xs with
                            | "", [] -> Some (front, [])
                            | _, [] -> Some (front, [back])
                            | "", y::ys ->
                                Some (front, y::ys)
                            | _, y::ys ->
                                Some (front, back:: y::ys)
                    )
                    lines
            JS.Alert(splitLines |> Seq.length |> string)
            for l in splitLines do
                JS.Alert(l)

        div
          [ h1 [ text "Create a template" ]
            div
              [ Doc.Radio [attr.id "uploadCover"; on.click (fun _ _ -> JS.Alert(varCover.Value.ToString())); attr.radiogroup "cover" ] Upload varCover
                labelAttr [ attr.``for`` "uploadCover" ] [text "Hochladen"]
                br []
                Doc.Radio [attr.id "createCover"; on.click (fun _ _ -> JS.Alert(varCover.Value.ToString())); attr.radiogroup "cover" ] Create varCover
                labelAttr [ attr.``for`` "createCover" ] [text "Online erstellen"]
                br []
                Doc.Radio [attr.id "ignoreCover"; on.click (fun _ _ -> JS.Alert(varCover.Value.ToString())); attr.radiogroup "cover" ] Ignore varCover
                labelAttr [ attr.``for`` "ignoreCover" ] [text "Nicht verwenden"]
                br []
              ]
            divAttr [attr.``class`` "page"]
              [ divAttr [attr.style "height: 225pt; width: 100%; background-color: lightblue"]
                  [ Doc.Input [ attr.``class`` "grow-input"; attr.autofocus "autofocus"; on.input (fun el _ -> resize (JQuery el) "Arial" "12pt" "normal" 150; findLineBreaks varTextArea.Value (JQuery("#mainText").Width()) "Arial" "12pt" "normal"); attr.placeholder "Dein Titel" ] varUserTitle
                    Doc.Input [ attr.``class`` "grow-input"; on.input (fun el _ -> resize (JQuery el) "Arial" "12pt" "normal" 150); attr.placeholder "Dein Vorname" ] varUserFirstName
                    Doc.Input [ attr.``class`` "grow-input"; on.input (fun el _ -> resize (JQuery el) "Arial" "12pt" "normal" 150); attr.placeholder "Dein Nachname" ] varUserLastName
                    br []
                    Doc.Input [ attr.``class`` "grow-input"; attr.style "width:150px"; on.input (fun el _ -> resize (JQuery el) "Arial" "12pt" "normal" 150); attr.placeholder "Deine Straße" ] varUserStreet
                    br []
                    Doc.Input [ attr.``class`` "grow-input"; on.input (fun el _ -> resize (JQuery el) "Arial" "12pt" "normal" 150); attr.placeholder "Deine Postleitzahl" ] varUserPostcode
                    Doc.Input [ attr.``class`` "grow-input"; on.input (fun el _ -> resize (JQuery el) "Arial" "12pt" "normal" 150); attr.placeholder "Deine Stadt" ] varUserCity
                    br []
                    br []
                    br []
                    Doc.Input [ attr.``class`` "grow-input"; attr.style "width:150px"; on.input (fun el _ -> resize (JQuery el) "Arial" "12pt" "normal" 150); attr.placeholder "Chef-Anrede" ] varBossTitulation
                    br []
                    Doc.Input [ attr.``class`` "grow-input"; on.input (fun el _ -> resize (JQuery el) "Arial" "12pt" "normal" 150); attr.placeholder "Chef-Titel" ] varBossTitle
                    Doc.Input [ attr.``class`` "grow-input"; on.input (fun el _ -> resize (JQuery el) "Arial" "12pt" "normal" 150); attr.placeholder "Chef-Vorname" ] varBossFirstName
                    Doc.Input [ attr.``class`` "grow-input"; on.input (fun el _ -> resize (JQuery el) "Arial" "12pt" "normal" 150); attr.placeholder "Chef-Nachname" ] varBossLastName
                    br []
                    Doc.Input [ attr.``class`` "grow-input"; on.input (fun el _ -> resize (JQuery el) "Arial" "12pt" "normal" 150); attr.placeholder "Firma-Strasse" ] varCompanyStreet
                    br []
                    Doc.Input [ attr.``class`` "grow-input"; on.input (fun el _ -> resize (JQuery el) "Arial" "12pt" "normal" 150); attr.placeholder "Firma-Postleitzahl" ] varCompanyPostcode
                    Doc.Input [ attr.``class`` "grow-input"; on.input (fun el _ -> resize (JQuery el) "Arial" "12pt" "normal" 150); attr.placeholder "Firma-Stadt" ] varCompanyCity
                    br []
                    spanAttr [ attr.style "float:right" ]
                      [ textView <| View.FromVar varUserCity
                        text <|  ", " + DateTime.Now.ToShortDateString()
                      ]
                    br []
                    br []
                    Doc.Input [ attr.``class`` "grow-input"; attr.style "font-weight: bold;"; on.input (fun el _ -> resize (JQuery el) "Arial" "12pt" "bold" 150); attr.placeholder "Betreff" ] varSubject
                    br []
                    br []
                  ]
                divAttr [attr.style "width:100%; min-height: 322.4645709pt; background-color:red;"]
                  [ Doc.InputArea [ attr.id "mainText"; attr.style "wrap: soft; border: none; outline: none; letter-spacing:0pt; margin: 0px; padding: 0px; background-color: lighblue; overflow: hidden; min-height: 322.4645709pt; min-width:100%; font-family: Arial; font-size: 12pt; font-weight: normal; display: block" ] varTextArea
                  ]
                divAttr [ attr.style "height:96pt; width: 100%;" ]
                  [
                    br []
                    text "Mit freundlichen Grüßen"
                    br []
                    br []
                    br []
                    text "$meinTitel $meinVorname $meinNachname"
                  ]
              ]
            buttonAttr [attr.value "Abschicken"; on.click (fun _ _ -> ())] []
          ]
(*        <textarea style="min-width: 100%; height:100%; font-family: inherit; font-size: inherit">
It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).

Where does it come from?

Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line in section 1.10.32.
        </textarea>
-->
*)

   










