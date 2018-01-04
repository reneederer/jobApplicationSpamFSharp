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
                    JS.Window.Location.Href <- ""
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
        let varUserValues : Var<UserValues> = Var.Create<UserValues>({gender=Gender.Female;degree="";firstName="";lastName="";street="";postcode="";city="";phone="";mobilePhone=""})
        let varUserEmail = Var.CreateWaiting<string>()
        let varEmployer = Var.Create<Employer>({company="";gender=Gender.Unknown;degree="";firstName="";lastName="";street="";postcode="";city="";email="";phone="";mobilePhone=""})
        let varCurrentPageIndex = Var.Create(1)
        let varDisplayedDocument = Var.Create(div [] :> Doc)

        let createInput t d =
          divAttr
            [attr.``class`` "form-group row"]
            [ labelAttr
                [attr.``class`` "col-sm-3 col-form-label"; attr.``for`` d]
                [text t]
              divAttr
                [attr.``class`` "col-sm-9"]
                [ inputAttr [attr.id d; attr.``data-`` "bind" d; attr.``class`` "form-control" ] []
                ]
            ]
        let createRadio (header : string) (rs : list<string * string * string>) =
          let radioGroup = Guid.NewGuid().ToString()    
          div
            [ for i = 0 to rs.Length - 1 do
                  match rs.[i] with
                  | radioText, bind, value ->
                      let id = Guid.NewGuid().ToString()
                      yield
                        divAttr
                          [attr.``class`` "form-group row"]
                          [ labelAttr
                               [attr.``class`` "col-sm-3 col-form-label"]
                               [text (if i = 0 then header else "")]
                            divAttr
                              [ attr.``class`` "col-sm-9"
                              ]
                              [ inputAttr
                                  [ attr.id id
                                    attr.``type`` "radio"
                                    attr.name radioGroup
                                    attr.value value
                                    attr.``data-`` "bind" bind
                                  ]
                                  []
                                labelAttr
                                  [ attr.``for`` id]
                                  [ text radioText ]

                              ]
                            :> Doc
                          ] :> Doc
              ]
  
            

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
                do! Async.Sleep 2000

                let map =
                    [ "userGender", ("radio", (fun () -> varUserValues.Value.gender.ToString()), (fun v -> varUserValues.Value <- { varUserValues.Value with gender = Gender.fromString v }))
                      "userDegree", ("text", (fun () -> varUserValues.Value.degree), (fun v -> varUserValues.Value <- { varUserValues.Value with degree = v }))
                      "userFirstName", ("text", (fun () -> varUserValues.Value.firstName), (fun v -> varUserValues.Value <- { varUserValues.Value with firstName = v }))
                      "userLastName", ("text", (fun () -> varUserValues.Value.lastName), (fun v -> varUserValues.Value <- { varUserValues.Value with lastName = v }))
                      "userStreet", ("text", (fun () -> varUserValues.Value.street), (fun v -> varUserValues.Value <- { varUserValues.Value with street= v }))
                      "userPostcode", ("text", (fun () -> varUserValues.Value.postcode), (fun v -> varUserValues.Value <- { varUserValues.Value with postcode = v }))
                      "userCity", ("text", (fun () -> varUserValues.Value.city), (fun v -> varUserValues.Value <- { varUserValues.Value with city = v }))
                      "userEmail", ("text", (fun () -> varUserEmail.Value), (fun v -> varUserEmail.Value <- v))
                      "userPhone", ("text", (fun () -> varUserValues.Value.phone), (fun v -> varUserValues.Value <- { varUserValues.Value with phone = v }))
                      "userMobilePhone", ("text", (fun () -> varUserValues.Value.mobilePhone), (fun v -> varUserValues.Value <- { varUserValues.Value with mobilePhone = v }))
                      "company", ("text", (fun () -> varEmployer.Value.company), (fun v -> varEmployer.Value <- { varEmployer.Value with company = v }))
                      "companyStreet", ("text", (fun () -> varEmployer.Value.street), (fun v -> varEmployer.Value <- { varEmployer.Value with street = v }))
                      "companyPostcode", ("text", (fun () -> varEmployer.Value.postcode), (fun v -> varEmployer.Value <- { varEmployer.Value with postcode = v }))
                      "companyCity", ("text", (fun () -> varEmployer.Value.city), (fun v -> varEmployer.Value <- { varEmployer.Value with city = v }))
                      "bossGender", ("radio", (fun () -> varEmployer.Value.gender.ToString()), (fun v -> varEmployer.Value <- { varEmployer.Value with gender = Gender.fromString(v) }))
                      "bossDegree", ("text", (fun () -> varEmployer.Value.degree), (fun v -> varEmployer.Value <- { varEmployer.Value with degree = v }))
                      "bossFirstName", ("text", (fun () -> varEmployer.Value.firstName), (fun v -> varEmployer.Value <- { varEmployer.Value with firstName = v }))
                      "bossLastName", ("text", (fun () -> varEmployer.Value.lastName), (fun v -> varEmployer.Value <- { varEmployer.Value with lastName = v }))
                      "bossEmail", ("text", (fun () -> varEmployer.Value.email), (fun v -> varEmployer.Value <- { varEmployer.Value with email = v }))
                      "bossPhone", ("text", (fun () -> varEmployer.Value.phone), (fun v -> varEmployer.Value <- { varEmployer.Value with phone = v }))
                      "bossMobilePhone", ("text", (fun () -> varEmployer.Value.mobilePhone), (fun v -> varEmployer.Value <- { varEmployer.Value with mobilePhone = v }))
                      "emailSubject", ("text", (fun () -> varDocument.Value.email.subject), (fun v -> varDocument.Value <- { varDocument.Value with email = { varDocument.Value.email with subject = v} }))
                      "emailBody", ("text", (fun () -> varDocument.Value.email.body), (fun v -> varDocument.Value <- { varDocument.Value with email = {varDocument.Value.email with body  = v } }))
                      //"today", sprintf "%i-%i-%i" DateTime.Now.Year DateTime.Now.Month DateTime.Now.Day
                    ]
                    |> Map.ofList
                
                for item in map do
                    match item.Value with
                    | "radio", get, set ->
                            JQuery(sprintf "[data-bind='%s']" item.Key).Each
                               (fun i el -> if get() = el?value then el?checked <- true) |> ignore
                    | "text", get, set ->
                            JQuery(sprintf "[data-bind='%s']" item.Key).Each
                                (fun i el -> el?value <- get ()) |> ignore
                    | s, _, _ -> failwith ("Unknown input type: " + s)
                        

                JQuery(JS.Document.QuerySelectorAll("[data-bind]"))
                    .Each
                        (fun (n, (el : Dom.Element)) ->
                            let eventAction =
                                (fun () ->
                                    let elValue = JQuery(el).Val() |> string
                                    let bindValue = JQuery(el).Data("bind").ToString()
                                    map.[bindValue] |> fun (_, _, set) -> set elValue |> ignore
                                    let updateElements = JQuery(sprintf "[data-bind='%s']" bindValue)
                                    updateElements.Each
                                        (fun (n, updateElement) ->
                                            if updateElement <> el
                                            then
                                                match map.[bindValue] with
                                                | "radio", get, set ->
                                                    updateElement?checked <- (elValue = updateElement?value)
                                                | "text", get, set ->
                                                    updateElement?value <- elValue
                                                | s, get, set ->
                                                    failwith ("Unknown input type: " + s) 
                                        ) |> ignore
                                )
                            el.RemoveEventListener("input", eventAction, true)
                            el.AddEventListener("input", eventAction, true)
                            el.RemoveEventListener("click", eventAction, true)
                            el.AddEventListener("click", eventAction, true)
                        ) |> ignore
            }

        let loadPageTemplate() : Async<unit> =
            async {
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
              "addEmployerDiv"
              "displayedDocumentDiv"
              "attachmentsDiv"
            ]
        
        let show (elIds : list<string>) =
            for elId in elIds do
                JS.Document.GetElementById(elId)?style?display <- "block"
            for hideElId in showHideMutualElements do
                if not <| List.contains hideElId elIds
                then JS.Document.GetElementById(hideElId)?style?display <- "none"
            


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
                    JS.Document.GetElementById("hiddenDocumentId")?value <- (JS.Document.GetElementById("selectDocumentName")?selectedIndex + 1).ToString()
                | None ->
                    ()
            }
        

        let setPageButtons () =
            async {
                JQuery("#pageButtonsUl li:not(:last-child)").Remove() |> ignore
                for page in varDocument.Value.pages do
                    JQuery(sprintf """<li><button id="pageButton%i" class="btnLikeLink">%s</button</li>""" (page.PageIndex()) (page.Name())).InsertBefore("#addPageButton").On(
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
                                        show ["displayedDocumentDiv"; "attachmentsDiv"]
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
            let menuDiv = JS.Document.GetElementById("sidebarMenuDiv")
            let addMenuEntry entry (f : Dom.Element -> Event -> unit) = 
                let li = JQuery(sprintf """<li><button class="btnLikeLink1">%s</button></li>""" entry).On("click", f)
                JQuery(menuDiv).Append(li)

            addMenuEntry "Add employer and apply" (fun _ _ -> show ["addEmployerDiv"]) |> ignore
            addMenuEntry "Edit your values" (fun _ _ -> show ["editUserValuesDiv"]) |> ignore
            addMenuEntry "Edit email" (fun _ _ -> show ["emailDiv"]) |> ignore
            addMenuEntry "Edit attachments" (fun _ _ -> show ["attachmentsDiv"]) |> ignore
        
            let! userValues = Server.getCurrentUserValues()
            varUserValues.Value <- userValues

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
            show ["addEmployerDiv"]
        } |> Async.Start


        div
          [ h4 [text "Your application documents: "]
            selectAttr
              [ attr.id "selectDocumentName";
                on.change
                  (fun _ _ ->
                      async {
                        do! setDocument()
                        do! setPageButtons()
                        do! fillDocumentValues()
                      } |> Async.Start)
              ]
              []
            inputAttr
              [ attr.``type`` "button"
                attr.style "margin-left: 20px"
                attr.``class`` ".btnLikeLink"
                attr.value "+"
                on.click(fun _ _ -> show ["newDocumentDiv"])
              ]
              []
            inputAttr
              [ attr.``type`` "button"
                attr.``class`` ".btnLikeLink"
                attr.style "margin-left: 20px"
                attr.value "-"
                on.click(fun _ _ ->())
              ]
              []
            hr []
            inputAttr
              [ attr.``type`` "button"
                attr.``class`` "btnLikeLink"
                attr.value "Email data"
                attr.style "display: none"
                on.click(fun _ _ -> show ["emailDiv"])
              ]
              []
            divAttr
              [ attr.id "attachmentsDiv"
              ]
              [ h4 [ text "Your attachments:" ]
                ulAttr
                  [ attr.id "pageButtonsUl"; attr.style "list-style-type: none; padding: 0; margin: 0;" ]
                  [ li
                      [ buttonAttr
                          [ attr.id "addPageButton"
                            attr.``class`` "btnLikeLink"
                            on.click
                              (fun el _ ->
                                  show ["choosePageTypeDiv"; "attachmentsDiv"; "createFilePageDiv"]
                                  JS.Document.GetElementById("rbFilePage")?checked <- true;
                              )
                          ]
                          [ text "+"]
                      ]
                  ]
              ]
            divAttr
              [ attr.id "newDocumentDiv" ]
              [ text "Document name: "
                br []
                inputAttr [attr.id "txtNewTemplateName"] []
                br []
                br []
                inputAttr
                  [ attr.``type`` "button"
                    attr.``class`` "btnLikeLink"
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
            divAttr
              [ attr.id "emailDiv"]
              [ h4 [text "Email" ]
                text "Email subject:"
                br []
                inputAttr
                  [ attr.``data-`` "bind" "emailSubject"
                    attr.style "width:100%"
                  ]
                  []
                br []
                text "Email body:"
                br []
                textareaAttr
                  [ attr.``data-`` "bind" "emailBody"
                    attr.style "min-height:300px; width: 100%"
                  ]
                  []
              ]
            divAttr
              [ attr.id "choosePageTypeDiv" ]
              [ inputAttr
                  [ attr.``type`` "radio"; attr.name "rbgrpPageType"; attr.id "rbHtmlPage"; on.click (fun _ _ -> show ["attachmentsDiv";"choosePageTypeDiv"; "createHtmlPageDiv"]) ]
                  []
                labelAttr
                  [ attr.``for`` "rbHtmlPage" ]
                  [ text "Create online" ]
                br []
                inputAttr
                  [ attr.``type`` "radio"; attr.id "rbFilePage"; attr.name "rbgrpPageType";
                    on.click
                      (fun _ _ ->
                        show ["attachmentsDiv"; "choosePageTypeDiv"; "createFilePageDiv"]
                        JS.Document.GetElementById("hiddenDocumentId")?value <- (JS.Document.GetElementById("selectDocumentName")?selectedIndex + 1).ToString()
                         )
                  ]
                  []
                labelAttr
                  [ attr.``for`` "rbFilePage" ]
                  [ text "File upload" ]
                br []
                br []
              ]
            divAttr
              [ attr.id "createHtmlPageDiv" ]
              [ inputAttr [attr.id ""] []
                br []
                br []
                buttonAttr [attr.``type`` "submit" ] [text "Add html attachment"]
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
                    inputAttr [ attr.``type`` "hidden"; attr.id "hiddenDocumentId"; attr.name "documentId" ] []
                    inputAttr [ attr.``type`` "hidden"; attr.name "pageIndex"; attr.value ((JQuery("#pageButtonsUl li").Length - 1) |> string); ] []
                    br []
                    br []
                    buttonAttr [attr.``type`` "submit" ] [text "Add attachment"]
                  ]
              ]
            divAttr
              [ attr.id "editUserValuesDiv" ]
              [ h4 [ text "Your values" ]
                createInput "Degree" "userDegree"
                createRadio
                  "Gender"
                  [ "male", "userGender", "m"
                    "female", "userGender", "f"
                  ]
                createInput "First name" "userFirstName"
                createInput "Last name" "userLastName"
                createInput "Street" "userStreet"
                createInput "Postcode" "userPostcode"
                createInput "City" "userCity"
                createInput "Phone" "userPhone"
                createInput "Mobile phone" "userMobilePhone"
              ]
            divAttr
              [ attr.id "addEmployerDiv" ]
              [ inputAttr
                  [ attr.``type`` "button"
                    attr.value "Load from website"
                    on.click (fun el _ ->
                        async {
                            let! employer = Server.readWebsite (JS.Document.GetElementById("txtReadEmployerFromWebsite")?value)
                            varEmployer.Value <- employer
                            JS.Alert(varEmployer.Value.company)
                            do! fillDocumentValues()
                            JS.Alert(varEmployer.Value.company)
                        } |> Async.Start
                    )
                  ]
                  []
                inputAttr
                  [ attr.id "txtReadEmployerFromWebsite"
                    attr.``type`` "text"
                    attr.value "https://jobboerse.arbeitsagentur.de/vamJB/stellenangeboteFinden.html?execution=e4s1&_eventId_detailView&bencs=ECCL4bGU%2BoeU3dXfDx34zLzb40uikic%2B2KKQU5eGJmbIR%2B7U88EatZPz4c6thxWn&bencs=m4%2BYgQaq%2BX3rqfQIFvibQOfuTdWSRPhHFObxFs%2BMsVl5i8Ha2yIwL1W5WT0iPA4PxFEqmlYn%2F%2BS1r%2FuIRfNrBw%3D%3D&bencs=6PQaRUFDQLZ%2BGNPAPRG8v%2BzbdKHav8zjyetSZpAojmXOPuJQd%2F4O3ojlMh1kXaLryb44mxmmwUNC%2F0m3Nq0xAXci%2FOEbKO0KpeEsoXm%2BGVaRIDnp67LAL434DTMOym9f&bencs=ScHZtBeeBMNt7ILR4tjstoAti5XHVScqFoc6%2FRQffzYt%2FJrTwlVXtA8Y77YD%2Fth0"
                  ]
                  []
                inputAttr
                  [ attr.``type`` "button"
                    attr.``class`` "btnLikeLink"
                    attr.value "Apply now"
                    on.click (fun _ _ -> Server.applyNowWithHtmlTemplate varEmployer.Value varDocument.Value varUserValues.Value |> Async.Start)
                  ]
                  []
                h4 [text "Employer"]
                createInput "Company name" "company"
                createInput "Street" "companyStreet"
                createInput "Postcode" "companyPostcode"
                createInput "City" "companyCity"
                createRadio
                    "Gender"
                    [ "male", "bossGender", "m"
                      "female", "bossGender", "f"
                    ]
                createInput "Degree" "bossDegree"
                createInput "First name" "bossFirstName"
                createInput "Last name" "bossLastName"
                createInput "Email" "bossEmail"
                createInput "Phone" "bossPhone"
                createInput "Mobile phone" "bossMobilePhone"
                inputAttr
                  [ attr.``type`` "button"
                    attr.``class`` "btnLikeLink"
                    attr.value "Apply now"
                    on.click (fun _ _ -> Server.applyNowWithHtmlTemplate varEmployer.Value varDocument.Value varUserValues.Value |> Async.Start)] []
              ]
          ]
