
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
        match Server.getCurrentUserValues () with 
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
                    JS.Alert(employerId.ToString() + ", " + templateName)
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
        let templateList = Server.getTemplateNames ()
        let varTemplate = Var.Create(Seq.tryItem 0 templateList |> Option.defaultValue "")
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
                          templateList
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
              match Server.login (varTxtLoginEmail.Value) (varTxtLoginPassword.Value) with
              | Ok (v, _) -> ()
              | Bad xs -> JS.Alert(String.concat ", " xs)
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
        let varTextArea = Var.Create("")
        div
          [ h1 [ text "hallo123" ]
            Doc.InputArea [attr.autofocus "autofocus"; attr.style "white-space: nowrap; overflow: hidden; min-height: 400px; font: Arial; font-size: 12pt"; on.mouseDown (fun el _ -> JS.Alert(varTextArea.Value))] varTextArea
          ]
   