
namespace JobApplicationSpam

open WebSharper
open WebSharper.UI.Next
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Html
open WebSharper.JavaScript


module Client =
    open Chessie.ErrorHandling
    open JobApplicationSpam.Types
    open WebSharper.JQuery
    open System
    open WebSharper.UI.Next.Client.HtmlExtensions
    open WebSharper.UI.Html.Tags

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
          [ on.submit (fun _ ev ->
                async {
                  let! loginResult = Server.login (varTxtLoginEmail.Value) (varTxtLoginPassword.Value)
                  match loginResult with
                  | Ok (v, _) ->
                    JS.Window.Location.Href <- "/templates"
                    ()
                  | Bad xs -> JS.Alert(String.concat ", " xs)
                } |> Async.Start
                ev.PreventDefault()
                ev.StopImmediatePropagation()
                ev.StopPropagation()
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
        let varUserValues : Var<UserValues> = Var.Create<UserValues>({gender=Gender.Male;degree="dr l";firstName="ren";lastName="ederer";street="";postcode="";city="";phone="";mobilePhone=""})
        let varUserEmail = Var.CreateWaiting<string>()
        let varEmployer = Var.Create<Employer>({company="";gender=Gender.Male;degree="";firstName="empfirstl";lastName="";street="";postcode="";city="";email="";phone="";mobilePhone=""})
        let varCurrentPageIndex = Var.Create(1)
        let varDisplayedDocument = Var.Create(div [] :> Doc)
        let varAddPage = Var.Create (div [] :> Doc)

        let createInput t d =
          div
            [ text t
              br []
              inputAttr [attr.``data-`` "bind" d ] []
              br[]
              br[]
            ]
          :> Doc

            

        let fillDocumentValues() =
            async {
                let pageMapElements = JS.Document.QuerySelectorAll("[data-page-key]")
                match varDocument.Value.pages.[varCurrentPageIndex.Value - 1] with
                | HtmlPage htmlPage ->
                    let myMap = htmlPage.map |> Map.ofList
                    JQuery(pageMapElements).Each
                        (fun (n, (el : Dom.Element)) ->
                            let jEl = JQuery(el)
                            let key = el.GetAttribute("data-page-key")
                            if myMap.ContainsKey key
                            then
                                jEl.Val(myMap.[key]) |> ignore
                            else
                                jEl.Val(el.GetAttribute("data-page-value") |> string) |> ignore
                    ) |> ignore
                | FilePage filePage ->
                    ()

                let map =
                    [ "userDegree", ((fun () -> varUserValues.Value.degree), (fun v -> varUserValues.Value <- { varUserValues.Value with degree = v }))
                      "userFirstName", ((fun () -> varUserValues.Value.firstName), (fun v -> varUserValues.Value <- { varUserValues.Value with firstName = v }))
                      "userLastName", ((fun () -> varUserValues.Value.lastName), (fun v -> varUserValues.Value <- { varUserValues.Value with lastName = v }))
                      "userStreet", ((fun () -> varUserValues.Value.street), (fun v -> varUserValues.Value <- { varUserValues.Value with street= v }))
                      "userPostcode", ((fun () -> varUserValues.Value.postcode), (fun v -> varUserValues.Value <- { varUserValues.Value with postcode = v }))
                      "userCity", ((fun () -> varUserValues.Value.city), (fun v -> varUserValues.Value <- { varUserValues.Value with city = v }))
                      "userEmail", ((fun () -> varUserEmail.Value), (fun v -> varUserEmail.Value <- v))
                      "userPhone", ((fun () -> varUserValues.Value.phone), (fun v -> varUserValues.Value <- { varUserValues.Value with phone = v }))
                      "userMobilePhone", ((fun () -> varUserValues.Value.mobilePhone), (fun v -> varUserValues.Value <- { varUserValues.Value with mobilePhone = v }))
                      "company", ((fun () -> varEmployer.Value.company), (fun v -> varEmployer.Value <- { varEmployer.Value with company = v }))
                      "companyStreet", ((fun () -> varEmployer.Value.street), (fun v -> varEmployer.Value <- { varEmployer.Value with street = v }))
                      "companyPostcode", ((fun () -> varEmployer.Value.postcode), (fun v -> varEmployer.Value <- { varEmployer.Value with postcode = v }))
                      "companyCity", ((fun () -> varEmployer.Value.city), (fun v -> varEmployer.Value <- { varEmployer.Value with city = v }))
                      "bossGender", ((fun () -> varEmployer.Value.gender.ToString()), (fun v -> varEmployer.Value <- { varEmployer.Value with gender = Gender.fromString(v) }))
                      "bossDegree", ((fun () -> varEmployer.Value.degree), (fun v -> varEmployer.Value <- { varEmployer.Value with degree = v }))
                      "bossFirstName", ((fun () -> varEmployer.Value.firstName), (fun v -> varEmployer.Value <- { varEmployer.Value with firstName = v }))
                      "bossLastName", ((fun () -> varEmployer.Value.lastName), (fun v -> varEmployer.Value <- { varEmployer.Value with lastName = v }))
                      "bossEmail", ((fun () -> varEmployer.Value.email), (fun v -> varEmployer.Value <- { varEmployer.Value with email = v }))
                      "bossPhone", ((fun () -> varEmployer.Value.phone), (fun v -> varEmployer.Value <- { varEmployer.Value with phone = v }))
                      "bossMobilePhone", ((fun () -> varEmployer.Value.mobilePhone), (fun v -> varEmployer.Value <- { varEmployer.Value with mobilePhone = v }))
                      //"today", sprintf "%i-%i-%i" DateTime.Now.Year DateTime.Now.Month DateTime.Now.Day
                    ]
                    |> Map.ofList
                
                for item in map do
                    JQuery(sprintf "[data-bind='%s']" item.Key).Val(fst item.Value ()) |> ignore

                JQuery(JS.Document.QuerySelectorAll("[data-bind]"))
                    .Each
                        (fun (n, (el : Dom.Element)) ->
                            let eventAction =
                                (fun () ->
                                    let bindValue = JQuery(el).Data("bind").ToString()
                                    let updateElements = JQuery(sprintf "[data-bind='%s']" bindValue)
                                    updateElements.Each
                                        (fun (n, updateElement) ->
                                            let elValue = JQuery(el).Val() |> string
                                            snd map.[bindValue] elValue
                                            if updateElement <> el
                                            then JQuery(updateElement).Val(elValue) |> ignore
                                            ()
                                        ) |> ignore
                                )
                            el.RemoveEventListener("input", eventAction, true)
                            el.AddEventListener("input", eventAction, true)
                        ) |> ignore
            }

        let loadPageTemplate() : Async<unit> =
            async {
                JS.Alert("hallo")
                JQuery("#insertDiv").Remove() |> ignore
                while JS.Document.GetElementById("insertDiv") <> null do
                    do! Async.Sleep 10
                let pageTemplateIndex = JS.Document.GetElementById("selectHtmlPageTemplate")?selectedIndex
                let! template = Server.getHtmlPageTemplate (pageTemplateIndex + 1)
                varDisplayedDocument.Value <- template |> Doc.Verbatim
                while JS.Document.GetElementById("insertDiv") = null do
                    do! Async.Sleep 10
                let pageMapElements = JS.Document.QuerySelectorAll("[data-page-key]")
                JQuery(pageMapElements).
                    Each(fun i el ->
                        let key = el.GetAttribute("data-page-key")
                        let eventAction = 
                            (fun () ->
                                  let beforePages, currentPage, afterPages =
                                      (List.splitAt (varCurrentPageIndex.Value - 1) varDocument.Value.pages)
                                      |> (fun (before, currentAndAfter) ->
                                             let current, after =
                                                  match currentAndAfter with
                                                  | [HtmlPage htmlPage] ->
                                                        let myMap = htmlPage.map |> Map.ofList
                                                        HtmlPage { htmlPage with map =  Map.add key (JQuery(el).Val() |> string) myMap |> Map.toList }, []
                                                  | (HtmlPage htmlPage)::xs ->
                                                        let myMap = htmlPage.map |> Map.ofList
                                                        HtmlPage { htmlPage with map = Map.add key (JQuery(el).Val() |> string) myMap |> Map.toList }, xs
                                                  | [] -> failwith "pageList was empty"
                                                  | (FilePage filePage)::_ -> FilePage filePage, []
                                             before, current, after
                                         )
                                  varDocument.Value <- { varDocument.Value with pages = beforePages @ (currentPage :: afterPages) }
                                  //fillDocumentValues() |> Async.Start
                            )
                        el.RemoveEventListener("input", eventAction, true)
                        el.AddEventListener("input", eventAction, true)
                    ) |> ignore
                do! fillDocumentValues()
            }
        
        let showHideMutualElements =
            [ "createFilePageDiv"
              "createHtmlPageDiv"
              "choosePageTypeDiv"
              "emailDiv"
              "newDocumentDiv"
              "editUserValuesDiv"
              "editEmployerDiv"
              "displayedDocumentDiv"
            ]
        
        let hideAll () =
            for elId in showHideMutualElements do
                JS.Document.GetElementById(elId)?style?display <- "none"
              
        let show elId =
            JS.Document.GetElementById(elId)?style?display <- "block"
            for currentElId in showHideMutualElements do
                if currentElId <> elId
                then JS.Document.GetElementById(currentElId)?style?display <- "none"


        let setDocument () =
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
        

        let setPageButtons () =
            async {
                JQuery("#pageButtonsUl li:not(:last-child)").Remove() |> ignore
                for page in varDocument.Value.pages do
                    JQuery(sprintf """<li><button id="pageButton%i">%s</button</li>""" (page.PageIndex()) (page.Name())).InsertBefore("#addPageButton").On(
                          "click"
                        , (fun el ev ->
                                match page with
                                | HtmlPage htmlPage ->
                                    async {
                                        JQuery("#insertDiv").Remove() |> ignore
                                        while JS.Document.GetElementById("insertDiv") <> null do
                                            do! Async.Sleep 10
                                        let pageTemplateIndex = htmlPage.oTemplateId |> Option.defaultValue 1
                                        let! template = Server.getHtmlPageTemplate pageTemplateIndex
                                        varDisplayedDocument.Value <- template |> Doc.Verbatim
                                        while JS.Document.GetElementById("insertDiv") = null do
                                            do! Async.Sleep 10
                                        do! fillDocumentValues()
                                        show "displayedDocumentDiv"
                                    } |> Async.Start
                                | FilePage filePage -> 
                                    () 
                        )
                    ) |> ignore
            }
        
        let addSelectOption el value =
            let optionEl = JS.Document.CreateElement("option")
            optionEl.TextContent <- value
            el?add(optionEl)

            
        async {
            let! documentNames = Server.getDocumentNames()

            let selectDocumentNameEl = JS.Document.GetElementById("selectDocumentName")
            for documentName in documentNames do
                addSelectOption selectDocumentNameEl documentName

            let! oLastEditedDocumentId = Server.getLastEditedDocumentId()
            match oLastEditedDocumentId with
            | None ->
                selectDocumentNameEl?selectedIndex <- 0
            | Some lastEditedDocumentId ->
                selectDocumentNameEl?selectedIndex <- lastEditedDocumentId - 1
            do! setDocument()
            do! setPageButtons()

            //Set selectHtmlPageTemplate
            let! htmlPageTemplates = Server.getHtmlPageTemplates()
            let selectHtmlPageTemplateEl = JS.Document.GetElementById("selectHtmlPageTemplate")
            for htmlPageTemplate in htmlPageTemplates do
                addSelectOption selectHtmlPageTemplateEl htmlPageTemplate.name

            do! fillDocumentValues()
            hideAll()
        } |> Async.Start


        div
          [ h3 [text "Your application documents: "]
            selectAttr
              [ attr.id "selectDocumentName";
                on.change
                  (fun _ _ ->
                      async {
                        do! setDocument()
                        do! setPageButtons()
                      } |> Async.Start)
              ]
              []
            inputAttr
              [ attr.``type`` "button"
                attr.style "margin-left: 20px"
                attr.value "+"
                on.click(fun _ _ -> show "newDocumentDiv")
              ]
              []
            inputAttr
              [ attr.``type`` "button"
                attr.style "margin-left: 20px"
                attr.value "-"
                on.click(fun _ _ ->())
              ]
              []
            div
              [ h3 [ text "Your pages:" ]
                ulAttr
                  [ attr.id "pageButtonsUl" ]
                  [ li
                      [ buttonAttr
                          [ attr.id "addPageButton"
                            on.click
                              (fun el _ ->
                                  ()//varAddPage.Value <- showAddPageDiv()
                              )
                          ]
                          [ text "+"]
                      ]
                  ]
              ]
            hr []
            inputAttr
              [ attr.``type`` "button"
                attr.value "Email data"
                on.click(fun _ _ -> show "emailDiv")
              ]
              []
            br []
            inputAttr
              [ attr.``type`` "button"
                attr.value "Your values"
                on.click(fun _ _ -> show "editUserValuesDiv")
              ]
              []
            br []
            inputAttr
              [ attr.``type`` "button"
                attr.value "Edit employer values"
                on.click(fun _ _ -> show "editEmployerDiv")
              ]
              []
            hr []
            divAttr
              [ attr.id "newDocumentDiv" ]
              [ text "Document name: "
                br []
                inputAttr [attr.id "txtNewTemplateName"] []
                br []
                br []
                inputAttr
                  [ attr.``type`` "button"
                    attr.value "Add document"
                    on.click (fun _ _ ->
                        async {
                            let newDocumentName = JS.Document.GetElementById("txtNewDocumentName")?value |> string
                            do! Server.addNewDocument newDocumentName
                            addSelectOption "selectDocumentName" newDocumentName
                        } |> Async.Start
                      )
                  ]
                  []
              ]
            divAttr
              [attr.id "displayedDocumentDiv"]
              [ selectAttr
                  [ attr.id "selectHtmlPageTemplate";
                    on.change
                      (fun _ _ ->
                          async {
                            do! loadPageTemplate()
                            do! fillDocumentValues()
                          } |> Async.Start)
                  ]
                  []
                Doc.EmbedView varDisplayedDocument.View
              ]
            br []
            br []
            br []
            divAttr
              [ attr.id "emailDiv"]
              [ h3 [text "Email" ]
                text "Email subject:"
                br []
                inputAttr
                  [ on.input (fun el _ -> varDocument.Value <- { varDocument.Value with email = {varDocument.Value.email with subject = el?value }})]
                  []
                br []
                br []
                text "Email body:"
                br []
                textareaAttr
                  [ on.input (fun el _ -> varDocument.Value <- { varDocument.Value with email = {varDocument.Value.email with body = el?value }})
                    attr.style "min-height:300px"
                  ]
                  []
              ]
            divAttr
              [ attr.id "choosePageTypeDiv" ]
              [ inputAttr
                  [ attr.``type`` "radio"; attr.name "rbgrpPageType"; attr.id "rbHtmlPage"; on.click (fun _ _ -> ()) ]
                  []
                labelAttr
                  [ attr.``for`` "rbHtmlPage" ]
                  [ text "Create online" ]
                br []
                inputAttr
                  [ attr.``type`` "radio"; attr.id "rbFilePage"; attr.name "rbgrpPageType"; on.click (fun _ _ -> ()) ]
                  []
                labelAttr
                  [ attr.``for`` "rbFilePage" ]
                  [ text "Upload" ]
                br []
                br []
              ]
            divAttr
              [ attr.id "createHtmlPageDiv" ]
              [ inputAttr [attr.id ""] []
                br []
                br []
                buttonAttr [attr.``type`` "submit" ] [text "Add html page"]
                buttonAttr [ attr.style "margin-left: 20px"; on.click (fun _ _ -> varAddPage.Value <- div []) ] [text "Abort"]
              ]
            divAttr
              [ attr.id "createFilePageDiv"; ]
              [ formAttr [attr.enctype "multipart/form-data"; attr.method "POST"; attr.action ""]
                  [ text "Please choose a file: "
                    br []
                    inputAttr
                      [ attr.``type`` "file"
                        attr.name "file"
                      ]
                      []
                    //inputAttr [ attr.``type`` "hidden"; attr.id "hiddenDocumentId"; attr.name "documentId"; attr.value ((JS.Document.GetElementById("selectDocumentName")?selectedIndex + 1).ToString()); ] []
                    //inputAttr [ attr.``type`` "hidden"; attr.id "hiddenPageIndex"; attr.name "pageIndex"; attr.value ((varPageCount.Value + 1) |> string); ] []
                    br []
                    br []
                    buttonAttr [attr.``type`` "submit" ] [text "Upload"]
                    buttonAttr [ attr.style "margin-left: 20px"; on.click (fun _ _ -> varAddPage.Value <- div []) ] [text "Abort"]
                  ]
              ]
            divAttr
              [ attr.id "editUserValuesDiv" ]
              [ h3 [ text "Your values" ]
                createInput "Degree" "userDegree"
                createInput "First name" "userFirstName"
                createInput "Last name" "userLastName"
                createInput "Street" "userStreet"
                createInput "Postcode" "userPostcode"
                createInput "City" "userCity"
                createInput "Phone" "userPhone"
                createInput "Mobile phone" "userMobilePhone"
              ]
            divAttr
              [ attr.id "editEmployerDiv" ]
              [ h3 [text "Employer"]
                createInput "Company name" "company"
                createInput "Street" "companyStreet"
                createInput "Postcode" "companyPostcode"
                createInput "City" "companyCity"
                createInput "Degree" "bossDegree"
                createInput "First name" "bossFirstName"
                createInput "Last name" "bossLastName"
                createInput "Phone" "bossPhone"
                createInput "Mobile phone" "bossMobilePhone"
              ]
            inputAttr
              [ attr.``type`` "button"
                attr.value "Save as new document"
                on.click
                  (fun _ _ ->
                    async {
                        let! newDocumentId = Server.saveNewDocument varDocument.Value
                        () //TODO
                    }
                    |> Async.Start
                  )
              ]
              []
            inputAttr
              [ attr.``type`` "button"
                attr.value "Save as new document"
                on.click
                  (fun _ _ ->
                    async {
                        let! newDocumentId = Server.saveNewDocument varDocument.Value
                        () //TODO
                    }
                    |> Async.Start
                  )
              ]
              []
            br []
            inputAttr
              [ attr.``type`` "button"
                attr.value "Overwrite document"
                on.click
                  (fun _ _ ->
                    async {
                        let!_ = Server.overwriteDocument varDocument.Value
                        ()
                    } |> Async.Start
                  )
              ]
              []
            br []
            inputAttr [attr.``type`` "button"; attr.value "Apply now"; on.click (fun _ _ -> Server.applyNowWithHtmlTemplate varEmployer.Value varDocument.Value varUserValues.Value |> Async.Start)] []
          ]
