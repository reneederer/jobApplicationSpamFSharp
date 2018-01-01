
namespace JobApplicationSpam

open WebSharper
open WebSharper.UI.Next
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Html
open WebSharper.JavaScript


module Client =
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
    open WebSharper.UI.Next.CSharp.Client.Html.SvgElements

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
        let varSelectHtmlPageTemplate = Var.Create(div [])
        let varNewDocument = Var.Create(div [])
        let varPageButtonsDiv = Var.Create(div [])
        let varPageButtons = Var.Create(div [])
        let varCurrentPageIndex = Var.Create(1)
        let varPageCount = Var.Create(1)
        let varDisplayedDocument = Var.Create(div [] :> Doc)
        let varAddPage = Var.Create (div [] :> Doc)
        

        let rec setSelectDocumentName() =
            async {
                let! documentNames = Server.getDocumentNames()
                varSelectDocumentName.Value <-
                    div
                      [ text "Your application documents: "
                        selectAttr
                          [attr.id "selectDocumentName"; on.change (fun _ _ -> indexChanged_selectDocumentName() |> Async.Start)]
                          [ for documentName in documentNames do
                                yield optionAttr [] [text documentName] :> Doc
                          ]
                        inputAttr
                          [ attr.``type`` "button"
                            attr.style "margin-left: 20px"
                            attr.value "+"
                            on.click(fun _ _ -> setNewDocument())
                          ]
                          []
                        inputAttr
                          [ attr.``type`` "button"
                            attr.style "margin-left: 20px"
                            attr.value "-"
                            on.click(fun _ _ -> setNewDocumentEmpty() |> Async.Start)
                          ]
                          []
                      ]
            }
        
        and addSelectDocumentName value =
            let mutable documentNames : list<string> = []
            JQuery("#selectDocumentName option").Each (fun i el -> documentNames <- (el?text |> string) :: documentNames) |> ignore
            documentNames <- documentNames @ [value]

            varSelectDocumentName.Value <-
                div
                  [ text "Your application documents: "
                    selectAttr
                      [attr.id "selectDocumentName"; on.change (fun _ _ -> indexChanged_selectDocumentName() |> Async.Start)]
                      [ for documentName in documentNames do
                            yield optionAttr [] [text documentName] :> Doc
                      ]
                    inputAttr
                      [ attr.``type`` "button"
                        attr.style "margin-left: 20px"
                        attr.value "+"
                        on.click(fun _ _ -> setNewDocument())
                      ]
                      []
                    inputAttr
                      [ attr.``type`` "button"
                        attr.style "margin-left: 20px"
                        attr.value "-"
                        on.click(fun _ _ -> setNewDocumentEmpty() |> Async.Start)
                      ]
                      []
                  ]

        and setSelectHtmlPageTemplate() =
            async {
                let! htmlPageTemplates = Server.getHtmlPageTemplates()
                varSelectHtmlPageTemplate.Value <-
                    selectAttr
                      [ attr.id "selectHtmlPageTemplate"; on.change (fun _ _ -> indexChanged_selectHtmlPageTemplate() |> Async.Start) ]
                      [ for htmlPageTemplate in htmlPageTemplates do
                          yield optionAttr [] [text htmlPageTemplate.name] :> Doc
                      ]
            }






        
        and setPageButtons () =
            async {
                let rec setPageNameDiv () =
                    let addHtmlPage (pageName : string) =
                        async {
                            let! documentId = Server.getDocumentIdOffset (JS.Document.GetElementById("selectDocumentName")?selectedIndex |> string |> Int32.Parse) 
                            do! Server.addHtmlPage documentId None (varPageCount.Value + 1) pageName
                            varPageButtons.Value <-
                                 div
                                   (
                                   [ varPageButtons.Value ]
                                   @
                                   [ buttonAttr [on.click (fun _ _ ->
                                       async {
                                           JS.Document.GetElementById("selectHtmlPageTemplate")?selectedIndex <- (Option.defaultValue 1 None) - 1
                                           varCurrentPageIndex.Value <- varPageCount.Value
                                           do! loadPageTemplate()
                                           do! fillDocumentValues()
                                       } |> Async.Start)] [text (pageName)] :> Doc
                                   ])
                        }
                    let htmlPageDiv ()=
                        div
                          [
                            text "Page name: "
                            br []
                            inputAttr [ attr.id "txtAddPageName" ] []
                            br []
                            br []
                            buttonAttr [ on.click (fun _ _ -> addHtmlPage (JS.Document.GetElementById("txtAddPageName")?value |> string) |> Async.Start; varAddPage.Value <- div [] )] [text "Add page"]
                            buttonAttr [ attr.style "margin-left: 20px"; on.click (fun _ _ -> varAddPage.Value <- div []) ] [text "Abort"]
                          ]
                    let filePageDiv () =
                        div
                          [ formAttr [attr.enctype "multipart/form-data"; attr.method "POST"; attr.action ""]
                              [ text "Please choose a file: "
                                br []
                                inputAttr
                                  [ attr.``type`` "file"
                                    attr.name "file"
                                  ]
                                  []
                                inputAttr [ attr.``type`` "hidden"; attr.name "documentId"; attr.value ((JS.Document.GetElementById("selectDocumentName")?selectedIndex + 1).ToString()); ] []
                                inputAttr [ attr.``type`` "hidden"; attr.name "pageIndex"; attr.value ((varPageCount.Value + 1) |> string); ] []
                                br []
                                br []
                                buttonAttr [attr.``type`` "submit" ] [text "Add page"]
                                buttonAttr [ attr.style "margin-left: 20px"; on.click (fun _ _ -> varAddPage.Value <- div []) ] [text "Abort"]
                              ]
                          ]
                    let varPageDiv = Var.Create(div [])
                    div
                      [
                        inputAttr
                          [ attr.``type`` "radio"; attr.name "rbgrpPageType"; attr.id "rbHtmlPage"; on.click (fun _ _ -> varPageDiv.Value <- htmlPageDiv ()) ]
                          []
                        labelAttr
                          [ attr.``for`` "rbHtmlPage" ]
                          [ text "Create online" ]
                        br []
                        inputAttr
                          [ attr.``type`` "radio"; attr.id "rbFilePage"; attr.name "rbgrpPageType"; on.click (fun _ _ -> varPageDiv.Value <- filePageDiv ()) ]
                          []
                        labelAttr
                          [ attr.``for`` "rbFilePage" ]
                          [ text "Upload" ]
                        br []
                        br []
                        Doc.EmbedView varPageDiv.View
                      ]

                varAddPage.Value <- div []
                varPageCount.Value <- varDocument.Value.pages.Length
                varPageButtonsDiv.Value <-
                    divAttr
                      [attr.id "ulPageButtons"]
                      ([ Doc.EmbedView varPageButtons.View
                      ]
                      @
                      [ div
                          [ buttonAttr
                              [ on.click
                                  (fun el _ ->
                                      varAddPage.Value <- setPageNameDiv ()
                                  )
                              ]
                              [ text "+"]
                          ]
                      ])
                varPageButtons.Value <-
                    div
                      [
                        for page in varDocument.Value.pages |> List.sortBy (fun x -> x.PageIndex()) do
                           match page with
                           | DocumentPage htmlPage ->
                               yield
                                 div
                                   [ buttonAttr [on.click (fun _ _ ->
                                       async {
                                           JS.Document.GetElementById("selectHtmlPageTemplate")?selectedIndex <- (Option.defaultValue 1 htmlPage.oTemplateId) - 1
                                           varCurrentPageIndex.Value <- htmlPage.pageIndex
                                           do! loadPageTemplate()
                                           do! fillDocumentValues()
                                       } |> Async.Start)] [text (page.Name())] :> Doc
                                   ]
                                 :> Doc
                           | DocumentFile filePage ->
                               yield
                                 div
                                   [ buttonAttr [on.click (fun _ _ -> varCurrentPageIndex.Value <- filePage.pageIndex; loadFileUploadTemplate();)] [text (page.Name())]
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
                let! oDocument = Server.getDocumentOffset documentIndex
                match oDocument with
                | Some document ->
                    varDocument.Value <- document
                | None ->
                    ()
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

        and setNewDocument() =
            varNewDocument.Value <-
                div
                  [ text "Name: "
                    br []
                    inputAttr [attr.id "txtNewTemplateName"] []
                    br []
                    br []
                    text "Email-Subject: "
                    br []
                    inputAttr [attr.id "txtNewTemplateEmailSubject"] []
                    br []
                    br []
                    text "Email-Body: "
                    br []
                    textareaAttr [attr.id "txtNewTemplateEmailBody"; attr.style "min-height: 300px; min-width: 100%"] []
                    br []
                    br []
                    inputAttr
                      [ attr.``type`` "button"
                        attr.value "Add"
                        on.click (fun _ _ ->
                            async {
                                let newDocumentName = JS.Document.GetElementById("txtNewTemplateName")?value |> string
                                let newDocumentEmailSubject = JS.Document.GetElementById("txtNewTemplateEmailSubject")?value |> string
                                let newDocumentEmailBody = JS.Document.GetElementById("txtNewTemplateEmailBody")?value |> string
                                do! Server.addNewDocument newDocumentName newDocumentEmailSubject newDocumentEmailBody
                                addSelectDocumentName newDocumentName
                                do! setNewDocumentEmpty()
                            } |> Async.Start
                          )
                      ]
                      []
                  ]
            varPageButtonsDiv.Value <- div []
            varSelectHtmlPageTemplate.Value <- div []
            varDisplayedDocument.Value <- div []

        and setNewDocumentEmpty() =
            async {
                varNewDocument.Value <- div []
                do! setPageButtons()
                do! setSelectHtmlPageTemplate()
            }

        and indexChanged_selectDocumentName() =
            async {
                do! setDocument()
                do! setPageButtons()
            }

        and indexChanged_selectHtmlPageTemplate() =
            async {
                do! loadPageTemplate()
                do! fillDocumentValues()
            }

        and loadPageTemplate() : Async<unit> =
            async {
                JQuery("#insertDiv").Remove() |> ignore
                while JS.Document.GetElementById("insertDiv") <> null do
                    do! Async.Sleep 10
                let pageTemplateIndex = JS.Document.GetElementById("selectHtmlPageTemplate")?selectedIndex
                let! template = Server.getHtmlPageTemplate (pageTemplateIndex + 1)
                varDisplayedDocument.Value <- template |> Doc.Verbatim
                while JS.Document.GetElementById("insertDiv") = null do
                    do! Async.Sleep 10
            }
        
        and loadFileUploadTemplate() =
            varDisplayedDocument.Value <-
                formAttr [attr.enctype "multipart/form-data"; attr.method "POST"; attr.action ""]
                  [ inputAttr [ attr.``type`` "file"; attr.name "file" ] []
                    inputAttr [ attr.``type`` "hidden"; attr.name "documentId"; attr.value ((JS.Document.GetElementById("selectDocumentName")?selectedIndex + 1).ToString()); ] []
                    inputAttr [ attr.``type`` "hidden"; attr.name "pageIndex"; attr.value (varCurrentPageIndex |> string); ] []
                    inputAttr [ attr.``type`` "submit" ] []
                  ]
        
        let itemToDoc (documentPage : DocumentPage) = 
            match documentPage with
            | DocumentPage htmlPage -> 
                  div [text "page " ] :> Doc
            | DocumentFile filePage ->
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
            do! setSelectHtmlPageTemplate()
            for i = 0 to 1000 do
                if JS.Document.GetElementById("selectHtmlPageTemplate") = null
                then do! Async.Sleep 10
            do! fillDocumentValues()
        } |> Async.Start

        div
          [ Doc.EmbedView varSelectDocumentName.View
            Doc.EmbedView varNewDocument.View
            Doc.EmbedView varPageButtonsDiv.View
            Doc.EmbedView varAddPage.View
            Doc.EmbedView varSelectHtmlPageTemplate.View
            Doc.EmbedView varDisplayedDocument.View
          ]

