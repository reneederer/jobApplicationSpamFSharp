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
    open WebSharper.UI.Next.Html.Tags
    open Server
    open WebSharper.Html.Server.Attr
    open WebSharper.Formlets.Enhance

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
        let varDocument = Var.Create {name="";id=0;pages=[];email={subject="";body=""}}
        let varUserValues : Var<UserValues> = Var.Create<UserValues>({gender=Gender.Female;degree="";firstName="";lastName="";street="";postcode="";city="";phone="";mobilePhone=""})
        let varUserEmail = Var.CreateWaiting<string>()
        let varEmployer = Var.Create<Employer>({company="";gender=Gender.Unknown;degree="";firstName="";lastName="";street="";postcode="";city="";email="";phone="";mobilePhone=""})
        let varDisplayedDocument = Var.Create(div [] :> Doc)
        let varLanguage = Var.Create English
        let varLanguageDict = Var.Create<Map<Word, string>>(Deutsch.dict |> Map.ofList)

        let getCurrentPageIndex () =
            let index = JQuery("#divAttachmentButtons").Find(".mainButton").Index(JQuery(".active")) + 1
            Math.Max (index, 1)

        let getCookie str =
            JS.Document.Cookie.Split([| "; "; ";" |], StringSplitOptions.None)
                |> Array.tryFind(fun (x : string) -> x.StartsWith(str + "="))
                |> Option.map (fun x -> x.Substring(str.Length + 1))


        let setLanguage language =
            JS.Document.Cookie <- JS.Document.Cookie + ";language=" + language.ToString()

        
        let t (w : Word) =
            varLanguageDict.Value.[w]


        let createInputWithColumnSizes column1Size column2Size labelText dataBind (validFun : string -> (bool * string)) =
          divAttr
            [attr.``class`` "form-group row"]
            [ labelAttr
                [attr.``class`` (column1Size + " col-form-label"); attr.``for`` dataBind]
                [text labelText]
              divAttr
                [ attr.``class`` column2Size]
                [ inputAttr
                    [ attr.id dataBind
                      attr.``data-`` "bind" dataBind
                      attr.``class`` "form-control"
                      on.blur (fun el _ ->
                        let (valid, textInvalid) = validFun el?value
                        if valid
                        then
                            JQuery(el).RemoveClass("is-invalid") |> ignore
                            JQuery(el).AddClass("is-valid") |> ignore
                            JQuery(el).Parent().Next().Hide() |> ignore
                        else
                            JQuery(el).RemoveClass("is-valid") |> ignore
                            JQuery(el).AddClass("is-invalid") |> ignore
                            JQuery(el).Parent().Next().Toggle(true) |> ignore
                            JQuery(el).Parent().Next().First().Html(textInvalid) |> ignore
                      )

                    ]
                    []
                ]
              divAttr
                [ attr.``class`` column1Size
                  attr.style "display: none"
                ]
                [ smallAttr
                    [ attr.``class`` "text-danger"
                    ]
                    [ text "Must be 8-20 characters" ]
                ]
            ]

        let createRadioWithColumnSizes column1Size column2Size (labelText : string) (radioValuesList : list<string * string * string * string>) =
          let radioGroup = Guid.NewGuid().ToString()    
          div
            (radioValuesList |> List.mapi (fun i (radioText, bind, value, ``checked``) ->
                let id = Guid.NewGuid().ToString()
                divAttr
                  [attr.``class`` "form-group row"]
                  [ labelAttr
                       [attr.``class`` (column1Size + " col-form-label")]
                       [text (if i = 0 then labelText else "")]
                    divAttr
                      [ attr.``class`` column2Size
                      ]
                      [ inputAttr
                          [ attr.id id
                            attr.``type`` "radio"
                            attr.name radioGroup
                            attr.value value
                            attr.``data-`` "bind" bind
                            attr.``checked`` ``checked``
                          ]
                          []
                        labelAttr
                          [ attr.``for`` id]
                          [ text radioText ]
                      ]
                  ] :> Doc
            )
            )
        let createTextareaWithColumnSizes column1Size column2Size labelText dataBind minHeight =
          divAttr
            [attr.``class`` "form-group row"]
            [ labelAttr
                [attr.``class`` (column1Size + " col-form-label"); attr.``for`` dataBind]
                [text labelText]
              divAttr
                [attr.``class`` column2Size]
                [ textareaAttr [attr.id dataBind; attr.``data-`` "bind" dataBind; attr.``class`` "form-control"; attr.style ("wrap: soft; white-space: nowrap; overflow: auto; min-height: " + minHeight)] []
                ]
            ]

        let createInput = createInputWithColumnSizes "col-lg-3" "col-lg-9"
        let createRadio = createRadioWithColumnSizes "col-lg-3" "col-lg-9"
        let createTextArea = createTextareaWithColumnSizes "col-lg-3" "col-lg-9"
            

        let fillDocumentValues() =
            async {
                if varDocument.Value.pages <> []
                then
                    let pageMapElements = JS.Document.QuerySelectorAll("[data-page-key]")
                    match varDocument.Value.pages.[getCurrentPageIndex() - 1] with
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

                    let map = // itemName * (itemType * getter * setter)
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
                        ]
                        |> Map.ofList
                    
                    for item in map do
                        match item.Value with
                        | "radio", get, set ->
                                JQuery(sprintf "[data-bind='%s']" item.Key).Each
                                   (fun i el -> if get() = el?value then el?``checked`` <- true) |> ignore
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
                                                        updateElement?``checked`` <- (elValue = updateElement?value)
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
                JQuery("#divInsert").Remove() |> ignore
                while JS.Document.GetElementById("divInsert") <> null do
                    do! Async.Sleep 10
                let pageTemplateIndex = JS.Document.GetElementById("slctHtmlPageTemplate")?selectedIndex
                let! template = Server.getHtmlPageTemplate (pageTemplateIndex + 1)
                varDisplayedDocument.Value <- template |> Doc.Verbatim
                while JS.Document.GetElementById("divInsert") = null do
                    do! Async.Sleep 10
                let pageMapElements = JS.Document.QuerySelectorAll("[data-page-key]")
                JQuery(pageMapElements).
                    Each(fun i el ->
                        let key = el.GetAttribute("data-page-key")
                        let eventAction = 
                            (fun () ->
                                  let beforePages, currentPage, afterPages =
                                      (List.splitAt (getCurrentPageIndex() - 1) varDocument.Value.pages)
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
                                  fillDocumentValues() |> Async.Start
                            )
                        el.RemoveEventListener("input", eventAction, true)
                        el.AddEventListener("input", eventAction, true)
                    ) |> ignore
                do! fillDocumentValues()
            }
        
        let showHideMutualElements =
            [ "divCreateFilePage"
              "divCreateHtmlPage"
              "divChoosePageType"
              "divEmail"
              "divNewDocument"
              "divEditUserValues"
              "divAddEmployer"
              "divDisplayedDocument"
              "divAttachments"
              "divUploadedFileDownload"
            ]
        
        let show (elIds : list<string>) =
            for elId in elIds do
                JS.Document.GetElementById(elId)?style?display <- "block"
            for hideElId in showHideMutualElements do
                if not <| List.contains hideElId elIds
                then JS.Document.GetElementById(hideElId)?style?display <- "none"
            


        let setDocument () =
            async {
                let slctDocumentNameEl = JS.Document.GetElementById("slctDocumentName")
                let! oDocument = Server.getDocumentOffset slctDocumentNameEl?selectedIndex
                match oDocument with
                | Some document ->
                    varDocument.Value <- document
                    JS.Document.GetElementById("hiddenDocumentId")?value <- varDocument.Value.id |> string
                | None ->
                    varDocument.Value <- {id=0;name="";pages=[];email={subject="";body=""}}
            }
        

        let rec setPageButtons () =
            async {
                JQuery("#divAttachmentButtons").Children("div").Remove() |> ignore
                for page in varDocument.Value.pages do
                    let deleteButton =
                        JQuery("""<button class="distanced"><i class="fa fa-trash" aria-hidden="true"></i></button>""").On(
                            "click"
                          , (fun _ _ ->
                                if JS.Confirm(String.Format(t ReallyDeletePage, page.Name()))
                                then
                                    varDocument.Value <-
                                        { varDocument.Value
                                            with pages =
                                                    varDocument.Value.pages
                                                    |> List.splitAt (page.PageIndex() - 1)
                                                    |> fun (before, after) ->
                                                        match after with
                                                        | x::xs -> 
                                                            before @ (xs |> List.map (fun p -> p.PageIndex(p.PageIndex() - 1)))
                                                        | [] -> before
                                            
                                        }
                                    async {
                                        let! _ = overwriteDocument varDocument.Value
                                        do! setDocument()
                                        do! setPageButtons()
                                        do! fillDocumentValues()
                                        show ["divAttachments"]
                                    } |> Async.Start
                            )
                          )
                    let pageUpButton =
                        JQuery( """<button class="distanced"><i class="fa fa-arrow-up" aria-hidden="true"></i></button>""").On(
                            "click"
                          , (fun _ _ ->
                                async {
                                    varDocument.Value <-
                                        { varDocument.Value
                                            with pages =
                                                    varDocument.Value.pages
                                                    |> List.splitAt (page.PageIndex() - 2)
                                                    |> fun (before, after) ->
                                                        match after with
                                                        | x1::x2::xs -> 
                                                            before @ (x2.PageIndex(x1.PageIndex())::x1.PageIndex(x2.PageIndex())::xs)
                                                        | [x] -> before @ [x]
                                                        | [] -> before
                                            
                                        }
                                    let! _ = overwriteDocument varDocument.Value
                                    do! setDocument()
                                    do! setPageButtons()
                                    show ["divAttachments"]
                                    do! fillDocumentValues()
                                } |> Async.Start
                            )
                          )


                    let pageDownButton =
                        JQuery("""<button class="distanced"><i class="fa fa-arrow-down" aria-hidden="true"></i></button>""").On(
                            "click"
                          , (fun _ _ ->
                                async {
                                    varDocument.Value <-
                                        { varDocument.Value
                                            with pages =
                                                    varDocument.Value.pages
                                                    |> List.splitAt (page.PageIndex() - 1)
                                                    |> fun (before, after) ->
                                                        match after with
                                                        | x1::x2::xs -> 
                                                            before @ (x2.PageIndex(x1.PageIndex())::x1.PageIndex(x2.PageIndex())::xs)
                                                        | [x] -> before @ [x]
                                                        | [] -> before
                                            
                                        }
                                    let! _ = overwriteDocument varDocument.Value
                                    do! setDocument()
                                    do! setPageButtons()
                                    show ["divAttachments"]
                                    do! fillDocumentValues()
                                } |> Async.Start
                            )
                          )
                    let mainButton =
                        JQuery(sprintf """<button class="distanced btn-block mainButton" style="width: 100%%">%s</button>""" (page.Name())).On(
                              "click"
                            , (fun el _ ->
                                JQuery(el).AddClass("active") |> ignore
                                JQuery(el).Parent().Parent().Parent().Find(".mainButton").Each(fun (i, b) ->
                                    if b <> el then JQuery(b).RemoveClass("active") |> ignore) |> ignore
                                match page with
                                | HtmlPage htmlPage ->
                                    async {
                                        JQuery("#divInsert").Remove() |> ignore
                                        while JS.Document.GetElementById("divInsert") <> null do
                                            do! Async.Sleep 10
                                        let pageTemplateIndex = htmlPage.oTemplateId |> Option.defaultValue 1
                                        let! template = Server.getHtmlPageTemplate pageTemplateIndex
                                        varDisplayedDocument.Value <- template |> Doc.Verbatim
                                        while JS.Document.GetElementById("divInsert") = null do
                                            do! Async.Sleep 10
                                        do! fillDocumentValues()
                                        show ["divDisplayedDocument"; "divAttachments"]
                                    } |> Async.Start
                                | FilePage filePage -> 
                                    show ["divAttachments"; "divUploadedFileDownload"]
                              )
                        )
                    let firstDiv = JQuery("""<div class="col-6 col-sm-7 col-md-8 col-lg-8 col-xl-8" style="float: left; display:inline; padding: 0; margin-left:0;"></div>""").Append(mainButton)
                    let secondDiv =
                        JQuery("""<div class="col-6 col-sm-5 col-md-4 col-lg-4 col-xl-4" style="float: right; display:inline"></div>""")
                            .Append(if page.PageIndex() >= (varDocument.Value.pages |> List.length) then pageDownButton.Css("visibility", "hidden") else pageDownButton)
                            .Append(if page.PageIndex() <= 1 then pageUpButton.Css("visibility", "hidden") else pageUpButton)
                            .Append(deleteButton)
                        
                    JQuery("""<div class="row" style="min-width:100%; margin-bottom: 10px;"></div>""")
                        .Append(firstDiv)
                        .Append(secondDiv)
                        .InsertBefore("#btnAddPage") |> ignore
                JS.Document.GetElementById("hiddenNextPageIndex")?value <- ((JQuery("#divAttachmentButtons").Children("div").Length + 1) |> string)
            }
        
        let addSelectOption el value =
            let optionEl = JS.Document.CreateElement("option")
            optionEl.TextContent <- value
            el?add(optionEl)

            
        async {
            (*
            setLanguage Deutsch
            let! dict = Server.getLanguageDict (Language.fromString(getCookie "language" |> Option.defaultValue "english"))
            *)
            varLanguageDict.Value <- (Deutsch.dict |> Map.ofList)

            let divMenu = JS.Document.GetElementById("divSidebarMenu")
            while divMenu = null do
                do! Async.Sleep 10
            let addMenuEntry entry (f : Dom.Element -> Event -> unit) = 
                let li = JQuery(sprintf """<li><button class="btnLikeLink1">%s</button></li>""" entry).On("click", f)
                JQuery(divMenu).Append(li)

            addMenuEntry (t EditYourValues) (fun _ _ -> show ["divEditUserValues"]) |> ignore
            addMenuEntry (t EditEmail) (fun _ _ -> show ["divEmail"]) |> ignore
            addMenuEntry (t EditAttachments) (fun _ _ -> show ["divAttachments"]) |> ignore
            addMenuEntry (t AddEmployerAndApply) (fun _ _ -> show ["divAddEmployer"]) |> ignore

            let! userEmail = Server.getCurrentUserEmail()
            varUserEmail.Value <- userEmail
        
            let! userValues = Server.getCurrentUserValues()
            varUserValues.Value <- userValues

            let! documentNames = Server.getDocumentNames()

            let slctDocumentNameEl = JS.Document.GetElementById("slctDocumentName")
            while JS.Document.GetElementById("slctDocumentName") = null do
                do! Async.Sleep 10
            for documentName in documentNames do
                addSelectOption slctDocumentNameEl documentName

            let! oLastEditedDocumentId = Server.getLastEditedDocumentId()
            match oLastEditedDocumentId with
            | None ->
                slctDocumentNameEl?selectedIndex <- 0
            | Some lastEditedDocumentId ->
                slctDocumentNameEl?selectedIndex <- lastEditedDocumentId - 1
            do! setDocument()
            do! setPageButtons()

            //Set slctHtmlPageTemplate
            let! htmlPageTemplates = Server.getHtmlPageTemplates()
            let slctHtmlPageTemplateEl = JS.Document.GetElementById("slctHtmlPageTemplate")
            while slctHtmlPageTemplateEl = null do
                do! Async.Sleep 10
            for htmlPageTemplate in htmlPageTemplates do
                addSelectOption slctHtmlPageTemplateEl htmlPageTemplate.name
            show [ "divAttachments" ]
            do! fillDocumentValues()
        } |> Async.Start

        div
          [ divAttr
              [ attr.style "width : 100%" ]
              [ h4 [text (t YourApplicationDocuments)]
                selectAttr
                  [ attr.id "slctDocumentName";
                    on.change
                      (fun _ _ ->
                          async {
                            show ["divAttachments"]
                            do! setDocument()
                            do! setPageButtons()
                            do! fillDocumentValues()
                          } |> Async.Start)
                  ]
                  []
                buttonAttr
                  [ attr.``type`` "button"
                    attr.style "margin-left: 20px"
                    attr.``class`` ".btnLikeLink"
                    on.click(fun _ _ -> show ["divNewDocument"])
                  ]
                  [ iAttr
                      [ attr.``class`` "fa fa-plus-square"
                        Attr.Create "aria-hidden" "true"
                      ]
                      []
                  ]
                buttonAttr
                  [ attr.``type`` "button"
                    attr.id "btnDeleteDocument"
                    attr.``class`` ".btnLikeLink"
                    attr.style "margin-left: 20px"
                    on.click(fun el _ ->
                        async {
                            let slctEl = JS.Document.GetElementById("slctDocumentName")
                            if slctEl?selectedIndex >= 0 && JS.Confirm(String.Format(t ReallyDeleteDocument, varDocument.Value.name))
                            then
                                do! Server.deleteDocument varDocument.Value.id
                                let slctEl = JS.Document.GetElementById("slctDocumentName")
                                slctEl.RemoveChild(slctEl?(slctEl?selectedIndex)) |> ignore
                                if slctEl?length = 0
                                then
                                    el?style?display <- "none"
                                    varDocument.Value <- {name="";id=0;pages=[];email={subject="";body=""}}
                                do! setDocument()
                                do! setPageButtons()
                                do! fillDocumentValues()
                        } |> Async.Start
                    )
                  ]
                  [ iAttr
                      [ attr.``class`` "fa fa-trash"
                        Attr.Create "aria-hidden" "true"
                      ]
                      []
                  ]
              ]
            hr []
            divAttr
              [ attr.id "divAttachments"; attr.style "display: none"
              ]
              [ h4 [ text (t YourAttachments) ]
                divAttr
                  [ attr.id "divAttachmentButtons"
                  ]
                  [ buttonAttr
                      [ attr.id "btnAddPage"
                        attr.style "margin:0;"
                        attr.``class`` "btnLikeLink"
                        on.click
                          (fun el _ ->
                              show ["divChoosePageType"; "divAttachments"; "divCreateFilePage"]
                              JS.Document.GetElementById("rbFilePage")?``checked`` <- true;
                          )
                      ]
                      [ iAttr
                          [ attr.``class`` "fa fa-plus-square"
                            Attr.Create "aria-hidden" "true"
                          ]
                          []
                      ]
                  ]
                br []
                hr []
                br []
              ]
            divAttr
              [ attr.id "divNewDocument"; attr.style "display: none" ]
              [ text (t DocumentName)
                br []
                inputAttr [attr.id "txtNewDocumentName"; attr.autofocus "autofocus" ] []
                br []
                br []
                inputAttr
                  [ attr.``type`` "button"
                    attr.``class`` "btnLikeLink"
                    attr.value (t AddDocument)
                    on.click (fun _ _ ->
                        async {
                            let newDocumentName = JS.Document.GetElementById("txtNewDocumentName")?value |> string
                            varDocument.Value <- { varDocument.Value with name = newDocumentName }
                            let! newDocumentId = Server.saveNewDocument varDocument.Value
                            varDocument.Value <- { varDocument.Value with id = newDocumentId }
                            let slctEl = JS.Document.GetElementById("slctDocumentName")
                            addSelectOption slctEl newDocumentName
                            JS.Document.GetElementById("divNewDocument")?style?display <- "none"
                            JS.Document.GetElementById("btnDeleteDocument")?style?display <- "inline"
                            slctEl?selectedIndex <- slctEl?options?length - 1
                            do! setDocument()
                            do! setPageButtons()
                            do! fillDocumentValues()
                        } |> Async.Start
                      )
                  ]
                  []
              ]
            divAttr
              [attr.id "divDisplayedDocument"; attr.style "display: none"]
              [ selectAttr
                  [ attr.id "slctHtmlPageTemplate";
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
              [ attr.id "divUploadedFileDownload"; attr.style "display: none"]
              [ buttonAttr
                  [ attr.``type`` "button"
                    on.click
                        (fun _ _ ->
                            async {
                                match varDocument.Value.pages |> List.tryItem (getCurrentPageIndex() - 1) with
                                | Some (FilePage filePage) ->
                                    let! fullPath = Server.getFullPath filePage.path varDocument.Value.id
                                    let! guid = Server.createLink fullPath
                                    JS.Window.Location.Href <- sprintf "download/%s" guid
                                | _ -> ()
                            } |> Async.Start
                        )
                  ]
                  [ text (t JustDownload)
                  ]
                br []
                br []
                buttonAttr
                  [ attr.``type`` "button"
                    on.click
                        (fun _ _ ->
                            async {
                                match varDocument.Value.pages |> List.tryItem (getCurrentPageIndex() - 1) with
                                | Some (FilePage filePage) ->
                                    let! path = Server.replaceVariables filePage.path varUserValues.Value varEmployer.Value varDocument.Value
                                    let! guid = Server.createLink path
                                    JS.Window.Location.Href <- sprintf "download/%s" guid
                                | _ -> ()
                            } |> Async.Start
                        )
                  ]
                  [ text (t DownloadWithReplacedVariables)
                  ]
              ]
            divAttr
              [ attr.id "divEmail"; attr.style "display: none"]
              [ h4 [text (t Email) ]
                createInput (t EmailSubject) "emailSubject" (fun (s : string) -> s <> "", "Required")
                createTextArea (t EmailBody) "emailBody" "400px"
              ]
            divAttr
              [ attr.id "divChoosePageType"; attr.style "display: none" ]
              [ inputAttr
                  [ attr.``type`` "radio"; attr.name "rbgrpPageType"; attr.id "rbHtmlPage"; on.click (fun _ _ -> show ["divAttachments";"divChoosePageType"; "divCreateHtmlPage"]) ]
                  []
                labelAttr
                  [ attr.``for`` "rbHtmlPage" ]
                  [ text (t CreateOnline) ]
                br []
                inputAttr
                  [ attr.``type`` "radio"; attr.id "rbFilePage"; attr.name "rbgrpPageType";
                    on.click
                      (fun _ _ ->
                        show ["divAttachments"; "divChoosePageType"; "divCreateFilePage"]
                         )
                  ]
                  []
                labelAttr
                  [ attr.``for`` "rbFilePage" ]
                  [ text (t UploadFile)]
                br []
                br []
              ]
            divAttr
              [ attr.id "divCreateHtmlPage"; attr.style "display: none" ]
              [ inputAttr [attr.id "txtCreateHtmlPage"] []
                br []
                br []
                buttonAttr
                  [ attr.``type`` "submit";
                    on.click
                      (fun _ _ ->
                        let pageIndex = JQuery("#divAttachmentButtons").Children("div").Length
                        varDocument.Value <-
                            { varDocument.Value
                                with pages = (
                                              varDocument.Value.pages
                                                @
                                                [ HtmlPage
                                                    { name = JS.Document.GetElementById("txtCreateHtmlPage")?value
                                                      oTemplateId = Some 1
                                                      pageIndex = pageIndex
                                                      map = []
                                                    }
                                                ]
                                    )
                            }
                        async {
                            do! setPageButtons()
                        } |> Async.Start
                      )
                  ]
                  [text (t AddHtmlAttachment)]
              ]
            divAttr
              [ attr.id "divCreateFilePage"; attr.style "display: none" ]
              [ formAttr [attr.enctype "multipart/form-data"; attr.method "POST"; attr.action ""]
                  [ text (t PleaseChooseAFile)
                    br []
                    inputAttr
                      [ attr.``type`` "file"
                        attr.name "file"
                        on.change(fun _ _ -> JS.Document.GetElementById("flUpload")?style?visibility <- "visible")
                      ]
                      []
                    inputAttr [ attr.``type`` "hidden"; attr.id "hiddenDocumentId"; attr.name "documentId"; ] []
                    inputAttr [ attr.``type`` "hidden"; attr.id "hiddenNextPageIndex"; attr.name "pageIndex"] []
                    br []
                    br []
                    buttonAttr [attr.``type`` "submit"; attr.id "flUpload"; attr.style "visibility: hidden" ] [text (t AddAttachment)]
                    br []
                    hr []
                    br []
                    b [ text
                          ((t YouMightWantToReplaceSomeWordsInYourFileWithVariables) +
                          (t VariablesWillBeReplacedWithTheRightValuesEveryTimeYouSendYourApplication))
                      ]
                    br []
                    br []
                    text "$firmaName"
                    br []
                    text "$firmaStrasse"
                    br []
                    text "$firmaPlz"
                    br []
                    text "$firmaStadt"
                    br []
                    text "$chefAnredeBriefkopf"
                    br []
                    text "$chefAnrede"
                    br []
                    text "$geehrter"
                    br []
                    text "$chefTitel"
                    br []
                    text "$chefVorname"
                    br []
                    text "$chefNachname"
                    br []
                    text "$chefEmail"
                    br []
                    text "$chefTelefon"
                    br []
                    text "$chefMobil"
                    br []
                    text "$meinGeschlecht"
                    br []
                    text "$meinTitel"
                    br []
                    text "$meinVorname"
                    br []
                    text "$meinNachname"
                    br []
                    text "$meineStrasse"
                    br []
                    text "$meinePlz"
                    br []
                    text "$meineStadt"
                    br []
                    text "$meineEmail"
                    br []
                    text "$meinMobilTelefon"
                    br []
                    text "$meineTelefonnr"
                    br []
                    text "$datumHeute"
                  ]
              ]
            divAttr
              [ attr.id "divEditUserValues"; attr.style "display: none" ]
              [ h4 [ text (t YourValues) ]
                createInput (t Degree) "userDegree" (fun s -> true, "")
                createRadio
                  (t Gender)
                  [ (t Male), "userGender", "m", ""
                    (t Female), "userGender", "f", ""
                  ]
                createInput (t FirstName) "userFirstName" (fun s -> s <> "", "This field is required")
                createInput (t LastName) "userLastName" (fun s -> s <> "", "This field is required")
                createInput (t Street) "userStreet" (fun s -> s <> "", "This field is required")
                createInput (t Postcode) "userPostcode" (fun (s : string) -> s <> "", "This field is required")
                createInput (t City) "userCity" (fun (s : string) -> s <> "", "This field is required")
                createInput (t Phone) "userPhone" (fun (s : string) -> s <> "", "This field is required")
                createInput (t MobilePhone) "userMobilePhone" (fun (s : string) -> s <> "", "This field is required")
              ]
            divAttr
              [ attr.id "divAddEmployer"
                attr.style "display: none"
              ]
              [ h4 [text (t Employer)]
                divAttr
                  [ attr.``class`` "form-group row" ]
                  [ divAttr
                      [ attr.``class`` "col-lg-3"
                      ]
                      [ inputAttr
                          [ attr.``type`` "button"
                            attr.``class`` "btn-block"
                            attr.value (t LoadFromWebsite)
                            on.click (fun el _ ->
                                async {
                                    let! employer = Server.readWebsite (JS.Document.GetElementById("txtReadEmployerFromWebsite")?value)
                                    varEmployer.Value <- employer
                                    do! fillDocumentValues()
                                } |> Async.Start
                            )
                          ]
                          []
                    ]
                    divAttr
                      [attr.``class`` "col-lg-9"]
                      [ inputAttr
                          [ attr.id "txtReadEmployerFromWebsite"
                            attr.``type`` "text"
                            attr.``class`` "form-control"
                            attr.value "https://jobboerse.arbeitsagentur.de/vamJB/stellenangeboteFinden.html?execution=e4s1&_eventId_detailView&bencs=ECCL4bGU%2BoeU3dXfDx34zLzb40uikic%2B2KKQU5eGJmbIR%2B7U88EatZPz4c6thxWn&bencs=m4%2BYgQaq%2BX3rqfQIFvibQOfuTdWSRPhHFObxFs%2BMsVl5i8Ha2yIwL1W5WT0iPA4PxFEqmlYn%2F%2BS1r%2FuIRfNrBw%3D%3D&bencs=6PQaRUFDQLZ%2BGNPAPRG8v%2BzbdKHav8zjyetSZpAojmXOPuJQd%2F4O3ojlMh1kXaLryb44mxmmwUNC%2F0m3Nq0xAXci%2FOEbKO0KpeEsoXm%2BGVaRIDnp67LAL434DTMOym9f&bencs=ScHZtBeeBMNt7ILR4tjstoAti5XHVScqFoc6%2FRQffzYt%2FJrTwlVXtA8Y77YD%2Fth0"
                            on.click (fun el _ -> el?select())
                          ]
                          []
                      ]
                  ] 
                divAttr
                  [ attr.``class`` "form-group row"
                  ]
                  [ divAttr
                      [ attr.``class`` "col-12"
                      ]
                      [ buttonAttr
                          [ attr.``type`` "button"
                            attr.``class`` "btn-block"
                            on.click (fun _ _ -> Server.applyNow varEmployer.Value varDocument.Value varUserValues.Value |> Async.Start)
                          ]
                          [ text (t ApplyNow)
                          ]
                      ]
                  ]
                createInput (t CompanyName) "company" (fun (s : string) -> s <> "", "This field is required")
                createInput (t Street) "companyStreet" (fun (s : string) -> s <> "", "This field is required")
                createInput (t Postcode) "companyPostcode" (fun (s : string) -> s <> "", "This field is required")
                createInput (t City) "companyCity" (fun (s : string) -> s <> "", "This field is required")
                createRadio
                    (t Gender)
                    [ (t Male), "bossGender", "m", ""
                      (t Female), "bossGender", "f", ""
                      (t UnknownGender), "bossGender", "u", "checked"
                    ]
                createInput (t Degree) "bossDegree" (fun s -> true, "")
                createInput (t FirstName) "bossFirstName" (fun s -> s <> "", "This field is required")
                createInput (t LastName) "bossLastName" (fun (s : string) -> s <> "", "This field is required")
                createInput (t Email) "bossEmail" (fun (s : string) -> s <> "", "This field is required")
                createInput (t Phone) "bossPhone" (fun s -> true, "")
                createInput (t MobilePhone) "bossMobilePhone" (fun s -> true, "")
                inputAttr
                  [ attr.``type`` "button"
                    attr.``class`` "btnLikeLink btn-block"
                    attr.value (t ApplyNow)
                    on.click (fun _ _ -> Server.applyNow varEmployer.Value varDocument.Value varUserValues.Value |> Async.Start)] []
              ]
          ]

