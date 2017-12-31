
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
    open Database
    open WebSharper.JavaScript
    open WebSharper.UI.Next.CSharp.Html.SvgAttributes
    open System.Data.SqlTypes
    open WebSharper.Html.Client.Operators
    open WebSharper.UI.Next.Client.HtmlExtensions
    open WebSharper.JavaScript
    open WebSharper.UI.Next.Html.Tags
    open System.Collections

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
    let login () =
        let varTxtLoginEmail = Var.Create ""
        let varTxtLoginPassword = Var.Create ""
        formAttr
          [ on.submit (fun _ _ ->
              let loginResult = Server.login (varTxtLoginEmail.Value) (varTxtLoginPassword.Value)
              match loginResult with
              | Ok (v, _) ->
                ()
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
              } |> Async.Start
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
    let templates () = 
        let varDocument = Var.CreateWaiting()
        let varSelectDocumentName = Var.Create(div [])
        let varSelectPageTemplate = Var.Create(div [])
        let varPageButtons = Var.Create(div [])
        let varCurrentPageIndex = Var.Create(1)
        let varDisplayedDocument = Var.Create(div [] :> Doc)

        let rec setSelectDocumentName() =
            async {
                let! documentNames = Server.getDocumentNames()
                varSelectDocumentName.Value <-
                    selectAttr
                      [attr.id "selectDocumentName"; on.change (fun _ _ -> indexChanged_selectDocumentName())]
                      [ for documentName in documentNames do
                          yield optionAttr [] [text documentName] :> Doc
                      ]
            }

        and setSelectPageTemplate() =
            async {
                let! pageTemplates = Server.getPageTemplates()
                varSelectPageTemplate.Value <-
                    selectAttr
                      [ attr.id "selectPageTemplate"; on.change (fun _ _ -> indexChanged_selectPageTemplate()) ]
                      [ for pageTemplate in pageTemplates do
                          yield optionAttr [] [text pageTemplate.name] :> Doc
                      ]
            }
        
        and setPageButtons () =
            async {
                varPageButtons.Value <-
                    ul
                      [ for documentItem in varDocument.Value.items |> List.sortBy (fun x -> x.PageIndex()) do
                          match documentItem with
                          | DocumentPage page ->
                              yield
                                li
                                  [ buttonAttr [on.click (fun _ _ ->
                                      async {
                                          JS.Document.GetElementById("selectPageTemplate")?selectedIndex <- page.templateId - 1
                                          varCurrentPageIndex.Value <- page.pageIndex
                                          do! loadPageTemplate()
                                          do! fillDocumentValues()
                                      } |> Async.Start)] [text (documentItem.Name())] :> Doc
                                  ]
                                :> Doc
                          | DocumentFile file ->
                              yield
                                li
                                  [ buttonAttr [on.click (fun _ _ -> varCurrentPageIndex.Value <- file.pageIndex; loadFileUploadTemplate();)] [text (documentItem.Name())]
                                  ]
                                :> Doc
                      ]
            }
            
        and setDocument () =
            async {
                let selectDocumentNameEl = JS.Document.GetElementById("selectDocumentName")
                let documentIndex =
                    if selectDocumentNameEl <> null
                    then JS.Document.GetElementById("selectDocumentName")?selectedIndex
                    else 0
                let! document = Server.getDocumentOffset documentIndex
                varDocument.Value <- document
            }
        
        and fillDocumentValues() =
            async {
                let! userValues = Server.getCurrentUserValues()
                let! userEmail = Server.getCurrentUserEmail()
                let map =
                    [ "userDegree", userValues.degree
                      "userFirstName", userValues.firstName
                      "userLastName", userValues.lastName
                      "userStreet", userValues.street
                      "userPostcode", userValues.postcode
                      "userCity", userValues.city
                      "userEmail", userEmail
                      "userPhone", userValues.phone
                      "userMobilePhone", userValues.mobilePhone
                      "today", sprintf "%i-%i-%i" DateTime.Now.Year DateTime.Now.Month DateTime.Now.Day
                    ]
                    |> Map.ofList

                for item in map do
                    JQuery(sprintf "[data-variable-value='%s']" item.Key).Val(item.Value) |> ignore

                let documentIndex = JS.Document.GetElementById("selectDocumentName")?selectedIndex
                let! documentMap = Server.getDocumentMapOffset varCurrentPageIndex.Value documentIndex
                for i = 0 to 1000 do
                    if JS.Document.GetElementById("insertDiv") = null
                    then do! Async.Sleep 10
                if documentMap.ContainsKey "mainText"
                then
                    JQuery("#mainText").Val(documentMap.["mainText"].Replace("\\n", "\n")) |> ignore
                let fieldUpdaters = JQuery(".field-updating")
                fieldUpdaters.Each
                    (fun (n, (el : Dom.Element)) ->
                        el.AddEventListener
                            ( "input"
                            , (fun () ->
                                let updateField = JQuery(el).Data("update-field").ToString()
                                match updateField with
                                | "userDegree" -> JS.Alert("userDegree")
                                | "userFirstName" -> JS.Alert("FirstName!")
                                | "userLastName" -> JS.Alert("LastName!")
                                | _ -> ()
                                let updateElements = JQuery(sprintf "[data-update-field='%s']" ((JQuery(el)).Data("update-field").ToString()))
                                updateElements.Each
                                    (fun (n, updateElement) ->
                                        if updateElement <> el
                                        then
                                            JQuery(updateElement).Val(JQuery(el).Val() |> string) |> ignore
                          //                  resize updateElement 150
                                            ()
                                    ) |> ignore
                                ()
                              ), true
                            )
                    ) |> ignore
            }


        and indexChanged_selectDocumentName() =
            async {
                do! setDocument()
                do! setPageButtons()
            } |> Async.Start

        and indexChanged_selectPageTemplate() =
            async {
                do! loadPageTemplate()
                do! fillDocumentValues()
            } |> Async.Start

        and loadPageTemplate() : Async<unit> =
            async {
                JQuery("#insertDiv").Remove() |> ignore
                while JS.Document.GetElementById("insertDiv") <> null do
                    do! Async.Sleep 10
                let pageTemplateIndex = JS.Document.GetElementById("selectPageTemplate")?selectedIndex
                let! template = Server.getPageTemplate (pageTemplateIndex + 1)
                varDisplayedDocument.Value <- template |> Doc.Verbatim
                while JS.Document.GetElementById("insertDiv") = null do
                    do! Async.Sleep 10
            }

        and loadFileUploadTemplate() =
            varDisplayedDocument.Value <-
                formAttr [attr.enctype "multipart/form-data"; attr.method "POST"; attr.action ""]
                  [ inputAttr [ attr.``type`` "file"; attr.name "file" ] []
                    inputAttr [ attr.``type`` "hidden"; attr.name "documentId"; attr.value ((JS.Document.GetElementById("selectDocumentName")?selectedIndex + 1).ToString()); ] []
                    inputAttr [ attr.``type`` "hidden"; attr.name "pageIndex"; attr.value (varCurrentPageIndex.Value.ToString()); ] []
                    inputAttr [ attr.``type`` "submit" ] []
                  ]
        
        let itemToDoc (documentItem : DocumentItem) = 
            match documentItem with
            | DocumentPage page -> 
                  div [text "page " ] :> Doc
            | DocumentFile file ->
                  div [text "file " ] :> Doc
            
        async {
            do! setSelectDocumentName()
            while JS.Document.GetElementById("selectDocumentName") = null do
                do! Async.Sleep 10
            let! oLastEditedDocumentId = Server.getLastEditedDocumentId()
            match oLastEditedDocumentId with
            | None ->
                JS.Document.GetElementById("selectDocumentName")?selectedIndex <- 0
            | Some lastEditedDocumentId ->
                JS.Document.GetElementById("selectDocumentName")?selectedIndex <- lastEditedDocumentId - 1
            do! setDocument()
            do! setPageButtons()
            do! setSelectPageTemplate()
            for i = 0 to 1000 do
                if JS.Document.GetElementById("selectPageTemplate") = null
                then do! Async.Sleep 10
            do! fillDocumentValues()
        } |> Async.Start

        div
          [ text "Your application documents: "
            Doc.EmbedView varSelectDocumentName.View
            Doc.EmbedView varPageButtons.View
            Doc.EmbedView varSelectPageTemplate.View
            Doc.EmbedView varDisplayedDocument.View
          ]

