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
    open Phrases
    open Translation

    [<JavaScript>]
    let login () =
        div
          [ formAttr
              [ attr.action "/login"; attr.method "POST" ]
              [ h3 [text (t German Login) ]
                divAttr
                  [ attr.``class`` "form-group" ]
                  [ labelAttr
                      [ attr.``for`` "txtLoginEmail" ] 
                      [text "Email"]
                    inputAttr
                      [ attr.``class`` "form-control"; attr.name "txtLoginEmail"; attr.id "txtLoginEmail" ]
                      []
                  ]
                divAttr
                  [ attr.``class`` "form-group" ]
                  [ labelAttr
                      [ attr.``for`` "txtLoginPassword" ] 
                      [text "Password"]
                    inputAttr
                      [ attr.``type`` "password"
                        attr.``class`` "form-control"
                        attr.name "txtLoginPassword"
                        attr.id "txtLoginPassword" ]
                      []
                  ]
                inputAttr
                  [ attr.``type`` "submit"
                    attr.value "Login"
                    attr.name "btnLogin"
                  ]
                  []
                inputAttr
                  [ attr.``type`` "submit"
                    attr.style "margin-left: 30px;"
                    attr.name "btnRegister"
                    attr.value "Register"
                  ]
                  []
              ]
          ]


    [<JavaScript>]
    let templates () = 
        let varDocument = Var.Create emptyDocument
        let documentEmailSubject : IRef<string> = varDocument.Lens (fun x -> x.email.subject) (fun x v -> { x with email = {x.email with subject = v }})
        let documentEmailBody : IRef<string> = varDocument.Lens (fun x -> x.email.body) (fun x v -> { x with email = {x.email with body = v }})
        let documentJobName : IRef<string> = varDocument.Lens (fun x -> x.jobName) (fun x v -> { x with jobName = v })
        let customVariables : IRef<string> = varDocument.Lens (fun x -> x.customVariables) (fun x v -> { x with customVariables = v })

        let varUserValues : Var<UserValues> = Var.Create emptyUserValues
        let userGender : IRef<Gender> = varUserValues.Lens (fun x -> x.gender) (fun x v -> { x with gender = v })
        let userDegree : IRef<string> = varUserValues.Lens (fun x -> x.degree) (fun x v -> { x with degree = v })
        let userFirstName : IRef<string> = varUserValues.Lens (fun x -> x.firstName) (fun x v -> { x with firstName = v })
        let userLastName : IRef<string> = varUserValues.Lens (fun x -> x.lastName) (fun x v -> { x with lastName = v })
        let userStreet : IRef<string> = varUserValues.Lens (fun x -> x.street) (fun x v -> { x with street = v })
        let userPostcode : IRef<string> = varUserValues.Lens (fun x -> x.postcode) (fun x v -> { x with postcode = v })
        let userCity : IRef<string> = varUserValues.Lens (fun x -> x.city) (fun x v -> { x with city = v })
        let userPhone : IRef<string> = varUserValues.Lens (fun x -> x.phone) (fun x v -> { x with phone = v })
        let userMobilePhone : IRef<string> = varUserValues.Lens (fun x -> x.mobilePhone) (fun x v -> { x with mobilePhone = v })

        let varUserEmail = Var.CreateWaiting<string>()

        let varEmployer = Var.Create emptyEmployer
        let employerCompany : IRef<string> = varEmployer.Lens (fun x -> x.company) (fun x v -> { x with company = v })
        let employerGender : IRef<Gender> = varEmployer.Lens (fun x -> x.gender) (fun x v -> { x with gender = v })
        let employerDegree : IRef<string> = varEmployer.Lens (fun x -> x.degree) (fun x v -> { x with degree = v })
        let employerFirstName : IRef<string> = varEmployer.Lens (fun x -> x.firstName) (fun x v -> { x with firstName = v })
        let employerLastName : IRef<string> = varEmployer.Lens (fun x -> x.lastName) (fun x v -> { x with lastName = v })
        let employerStreet : IRef<string> = varEmployer.Lens (fun x -> x.street) (fun x v -> { x with street = v })
        let employerPostcode : IRef<string> = varEmployer.Lens (fun x -> x.postcode) (fun x v -> { x with postcode = v })
        let employerCity : IRef<string> = varEmployer.Lens (fun x -> x.city) (fun x v -> { x with city = v })
        let employerEmail : IRef<string> = varEmployer.Lens (fun x -> x.email) (fun x v -> { x with email = v })
        let employerPhone : IRef<string> = varEmployer.Lens (fun x -> x.phone) (fun x v -> { x with phone = v })
        let employerMobilePhone : IRef<string> = varEmployer.Lens (fun x -> x.mobilePhone) (fun x v -> { x with mobilePhone = v })

        let varDisplayedDocument = Var.Create(div [] :> Doc)
        let varLanguage = Var.Create English
        let varDivSentApplications = Var.Create(div [] :> Doc)

        let getCurrentPageIndex () =
            let index = JQuery("#divAttachmentButtons").Find(".mainButton").Index(JQuery(".active")) + 1
            Math.Max (index, 1)

        
        let getSentApplications () =
            async {
                let! sentApplications = Server.getSentApplications DateTime.Now DateTime.Now
                varDivSentApplications.Value <-
                    divAttr
                      [ attr.style "width: 100%; height: 100%; overflow: auto"
                      ]
                      [ tableAttr
                          [ attr.style "border-spacing: 10px; border-collapse: separate" ]
                          [ thead
                              [ tr
                                  [ th [ text (t German CompanyName) ]
                                    th [ text (t German AppliedOnDate) ]
                                    th [ text (t German AppliedAs) ]
                                    th [ text "an dich mailen" ]
                                  ]
                              ]
                            tbody
                              [ let emailSentApplicationToUserFun =
                                    fun (el : Dom.Element) (ev : Dom.MouseEvent) ->
                                        async {
                                            let! result = Server.emailSentApplicationToUser (el.ParentElement.ParentElement?rowIndex - 1) ""
                                            match result with
                                            | Ok _ -> ()
                                            | Bad _ -> JS.Alert("Entschuldigung, es trat ein Fehler auf")
                                        } |> Async.Start

                                for (company, jobName, (appliedOn : DateTime), url) in sentApplications do
                                   yield!
                                     [ tr
                                         [ td
                                             [ text company ]
                                           td
                                             [ text (sprintf "%02i.%02i.%04i" appliedOn.Day appliedOn.Month appliedOn.Year) ]
                                           td
                                             [ text jobName ]
                                           td
                                             [ buttonAttr
                                                 [ on.click emailSentApplicationToUserFun
                                                 ]
                                                 [ iAttr
                                                     [ attr.``class`` "fa fa-envelope"; (Attr.Create "aria-hidden" "true")
                                                     ]
                                                     []
                                                 ]
                                             ]
                                         ]
                                       :> Doc
                                     ]
                              ]
                          ]
                      ]
            }
        


        let createInput labelText (ref : IRef<string>) (validFun : string -> string) =
          let guid = Guid.NewGuid().ToString("N")
          divAttr
            [ attr.``class`` "form-group bottom-distanced"]
            [ labelAttr
                [ attr.``for`` guid
                  attr.style "font-weight: bold"
                ]
                [ text labelText ]
              Doc.Input
                [ attr.id guid
                  attr.``class`` "form-control"
                  attr.``type`` "text"
                  on.blur (fun el _ ->
                    let validResult = validFun el?value
                    ()
                    (*
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
                        *)
                  )

                ]
                ref
            ]

        let createInputWithPlaceholder (placeholder : string) labelText (ref : IRef<string>) (validFun : string -> string) =
            let d = createInput labelText ref validFun
            d.Dom.ChildNodes.[1]?placeholder <- placeholder
            d


        let createRadio (labelText : string) (radioValuesList : list<string * 'a * (IRef<'a>) * string>) =
          let radioGroup = Guid.NewGuid().ToString("N")    
          divAttr
            [attr.``class`` "bottom-distanced"]
            ([ labelAttr
                 [ attr.style "font-weight: bold" ]
                 [ text labelText ]
             ]
            @
            (radioValuesList |> List.mapi (fun i (radioText, value, ref, ``checked``) ->
                let guid = Guid.NewGuid().ToString("N")
                divAttr
                  [ attr.``class`` "form-group"]
                  [ Doc.Radio
                      [ attr.id guid
                        attr.``type`` "radio"
                        attr.name radioGroup
                        attr.``checked`` ``checked``
                      ]
                      value
                      ref
                    labelAttr
                      [ attr.``for`` guid]
                      [ text radioText ]
                    
                  ] :> Doc
                )
            )
            )

        let createTextarea labelText ref minHeight =
          let guid = Guid.NewGuid().ToString("N")
          divAttr
            [attr.``class`` "form-group bottom-distanced"]
            [ labelAttr
                [attr.``for`` guid; attr.style "font-weight: bold" ]
                [text labelText]
              Doc.InputArea
                [ attr.id guid
                  attr.``class`` "form-control"
                  attr.style ("wrap: soft; white-space: nowrap; overflow: auto; min-height: " + minHeight)
                ]
                ref
            ]

        let fillDocumentValues() =
            async {
                if varDocument.Value.pages <> []
                then
                    let pageMapElements = JS.Document.QuerySelectorAll("[data-page-key]")
                    match varDocument.Value.pages.[getCurrentPageIndex() - 1] with
                    | HtmlPage htmlPage ->
                        let myMap = htmlPage.map |> Map.ofList
                        JQuery(pageMapElements).Each
                            (fun (_, el) ->
                                let key = el.GetAttribute("data-page-key")
                                if myMap.ContainsKey key
                                then
                                    el?value <- myMap.[key]
                                else
                                    el?value <- el.GetAttribute("data-page-value")
                        ) |> ignore
                    | FilePage filePage ->
                        ()

                let refMap : Map<string, DataBinding> =
                    [ "userGender", GenderBinding userGender
                      "userDegree", TextBinding userDegree
                      "userFirstName", TextBinding userFirstName
                      "userLastName", TextBinding userLastName
                      "userStreet", TextBinding userStreet
                      "userPostcode", TextBinding userPostcode
                      "userCity", TextBinding userCity
                      "userPhone",TextBinding userPhone
                      "userMobilePhone", TextBinding userMobilePhone
                      "employerCompany", TextBinding employerCompany
                      "employerStreet", TextBinding employerStreet
                      "employerPostcode", TextBinding employerPostcode
                      "employerCity", TextBinding employerCity
                      "employerDegree", TextBinding employerDegree
                      "employerFirstName", TextBinding employerFirstName
                      "employerLastName", TextBinding employerLastName
                      "employerEmail", TextBinding employerEmail
                      "employerPhone", TextBinding employerPhone
                      "employerMobilePhone", TextBinding employerMobilePhone
                      "documentEmailSubject", TextBinding documentEmailSubject
                      "documentEmailBody", TextBinding documentEmailBody
                      "documentJobName", TextBinding documentJobName
                    ]
                    |> Map.ofList

                
                JQuery("[data-bind-ref]").Each(
                    fun i el ->
                        let attributes =
                            [ for i = 0 to el.Attributes.Length - 1 do
                                if not <| (el.Attributes.[i].Name).StartsWith "data-bind"
                                then
                                    yield Attr.Create el.Attributes.[i].Name el.Attributes.[i].Value
                            ]
                        match refMap.[el.GetAttribute("data-bind-ref")] with
                        | TextBinding ref ->
                            Doc.RunReplace
                                el
                                (Doc.Input
                                   attributes
                                   ref
                                )
                        | GenderBinding ref ->
                            Doc.RunReplace
                                el
                                (Doc.Radio
                                   attributes
                                   (Gender.fromString (el.GetAttribute "data-bind-value"))
                                   ref
                                )
                ) |> ignore
            }

        let loadPageTemplate() : Async<unit> =
            async {
                JQuery("#divInsert").Remove() |> ignore
                while JS.Document.GetElementById("divInsert") <> null do
                    do! Async.Sleep 10
                let pageTemplateIndex = JS.Document.GetElementById("slctHtmlPageTemplate")?selectedIndex
                let! template = Server.getHtmlPageTemplate (pageTemplateIndex + 1)
                varDisplayedDocument.Value <- (template |> Option.defaultValue "" |> Doc.Verbatim)
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
              "divAddDocument"
              "divEditUserValues"
              "divAddEmployer"
              "divDisplayedDocument"
              "divAttachments"
              "divUploadedFileDownload"
              "divSentApplications"
              "divVariables"
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
                    JS.Document.GetElementById("btnAddPage")?style?visibility <- "visible"
                    JS.Document.GetElementById("btnApplyNowTop")?disabled <- false
                    JS.Document.GetElementById("btnApplyNowBottom")?disabled <- false
                    show ["divAttachments"]
                | None ->
                    varDocument.Value <- emptyDocument
                    JS.Document.GetElementById("btnAddPage")?style?visibility <- "hidden"
                    JS.Document.GetElementById("btnApplyNowTop")?disabled <- true
                    JS.Document.GetElementById("btnApplyNowBottom")?disabled <- true
                    show ["divAddDocument"]
            }
        

        let rec setPageButtons () =
            async {
                JQuery("#divAttachmentButtons").Children("div").Remove() |> ignore
                for page in varDocument.Value.pages do
                    let deleteButton =
                        JQuery("""<button class="distanced"><i class="fa fa-trash" aria-hidden="true"></i></button>""").On(
                            "click"
                          , (fun _ _ ->
                                if JS.Confirm(String.Format(t German ReallyDeletePage, page.Name()))
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
                                        let! _ = Server.overwriteDocument varDocument.Value
                                        do! setDocument()
                                        do! setPageButtons()
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
                                    let! _ = Server.overwriteDocument varDocument.Value
                                    do! setDocument()
                                    do! setPageButtons()
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
                                    let! _ = Server.overwriteDocument varDocument.Value
                                    do! setDocument()
                                    do! setPageButtons()
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
                                        varDisplayedDocument.Value <- (template |> Option.defaultValue "" |> Doc.Verbatim)
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
        
        let btnApplyNowClicked () =
            async {
                let emailRegexStr = """^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$"""
                let regex = RegExp(emailRegexStr)
                if not <| regex?test(employerEmail.Value)
                then
                    JS.Alert(t German TheEmailOfYourEmployerDoesNotLookValid)
                elif documentJobName.Value.Trim() = ""
                then
                    JS.Alert(String.Format(t German FieldIsRequired, (t German JobName)))
                else
                    let! sentApplication (*TODO this is an option<int> instead of option<SentApplication> as placeholder*) =
                        Server.tryFindSentApplication varEmployer.Value
                    if sentApplication.IsNone || (sentApplication.IsSome && JS.Confirm("Du hast dich schon einmal bei dieser Firmen-Email-Adresse beworben.\nBewerbung trotzdem abschicken?"))
                    then
                        let btnLoadFromWebsite = JQuery("#btnLoadFromWebsite")
                        let fontAwesomeEls =
                            [ JQuery("#faBtnApplyNowBottom")
                              JQuery("#faBtnApplyNowTop")
                            ]
                        fontAwesomeEls
                        |> List.iter (fun faEl ->
                            faEl.Css("color", "black") |> ignore
                            faEl.AddClass("fa-spinner fa-spin") |> ignore
                            )
                        btnLoadFromWebsite.Prop("disabled", true) |> ignore
                        JQuery("#divJobApplicationContent").Find("input,textarea,button,select").Prop("disabled", true) |> ignore

                        let! applyResult =
                            Server.applyNow
                                varEmployer.Value
                                varDocument.Value
                                varUserValues.Value
                                (JS.Document.GetElementById("txtReadEmployerFromWebsite")?value)

                        fontAwesomeEls
                        |> List.iter (fun faEl ->
                            faEl.RemoveClass("fa-spinner fa-spin") |> ignore
                        )
                        btnLoadFromWebsite.Prop("disabled", false) |> ignore
                        JQuery("#divJobApplicationContent").Find("input,textarea,button,select").Prop("disabled", false) |> ignore
                        match applyResult with
                        | Bad xs ->
                            do! Async.Sleep 700
                            JS.Alert(t German SorryAnErrorOccurred + "\n" + t German YourApplicationHasNotBeenSent)
                        | Ok _ ->
                            fontAwesomeEls
                            |> List.iter (fun faEl ->
                                faEl.Css("color", "#08a81b") |> ignore
                                faEl.AddClass("fa-check") |> ignore
                            )

                            varEmployer.Value <- emptyEmployer
                            JS.Document.GetElementById("txtReadEmployerFromWebsite")?value <- ""

                            do! Async.Sleep 4500

                            fontAwesomeEls
                            |> List.iter (fun faEl ->
                                faEl.RemoveClass("fa-check") |> ignore
                            )
            }
            
        async {
            let divMenu = JS.Document.GetElementById("divSidebarMenu")
            while divMenu = null do
                do! Async.Sleep 10
            let addMenuEntry entry (f : Dom.Element -> Event -> unit) = 
                let li = JQuery(sprintf """<li><button class="btnLikeLink1">%s</button></li>""" entry).On("click", f)
                JQuery(divMenu).Append(li)

            addMenuEntry (t German SentApplications) (fun _ _ ->
                async {
                    do! getSentApplications()
                    show ["divSentApplications"]
                } |> Async.Start
            ) |> ignore
            addMenuEntry (t German EditYourValues) (fun _ _ -> show ["divEditUserValues"]) |> ignore
            addMenuEntry "Variablen" (fun _ _ -> show ["divVariables"]) |> ignore
            addMenuEntry (t German EditEmail) (fun _ _ -> show ["divEmail"]) |> ignore
            addMenuEntry (t German EditAttachments) (fun _ _ -> show ["divAttachments"]) |> ignore
            addMenuEntry (t German AddEmployerAndApply) (fun _ _ -> show ["divAddEmployer"]) |> ignore

            let! oUserEmail = Server.getCurrentUserEmail()
            varUserEmail.Value <- oUserEmail |> Option.defaultValue ""
        
            let! userValues = Server.getCurrentUserValues()
            varUserValues.Value <- userValues

            let! documentNames = Server.getDocumentNames()

            let slctDocumentNameEl = JS.Document.GetElementById("slctDocumentName")
            while JS.Document.GetElementById("slctDocumentName") = null do
                do! Async.Sleep 10
            for documentName in documentNames do
                addSelectOption slctDocumentNameEl documentName

            let! lastEditedDocumentOffset = Server.getLastEditedDocumentOffset()
            slctDocumentNameEl?selectedIndex <- lastEditedDocumentOffset
            do! setDocument()
            do! setPageButtons()

            //Set slctHtmlPageTemplate
            let! htmlPageTemplates = Server.getHtmlPageTemplates()
            let slctHtmlPageTemplateEl = JS.Document.GetElementById("slctHtmlPageTemplate")
            while slctHtmlPageTemplateEl = null do
                do! Async.Sleep 10
            for htmlPageTemplate in htmlPageTemplates do
                addSelectOption slctHtmlPageTemplateEl htmlPageTemplate.name
        } |> Async.Start

        let readFromWebsite () =
            async {
                JS.Document.GetElementById("btnReadFromWebsite")?disabled <- true
                JS.Document.GetElementById("faReadFromWebsite")?style?visibility <- "visible"
                do! Async.Sleep 200
                let! employerResult = Server.readWebsite (JS.Document.GetElementById("txtReadEmployerFromWebsite")?value)
                JS.Document.GetElementById("btnReadFromWebsite")?disabled <- false
                JS.Document.GetElementById("faReadFromWebsite")?style?visibility <- "hidden"
                match employerResult with
                | Ok (employer, _) ->
                    varEmployer.Value <- employer
                | Bad (xs) ->
                    JS.Alert(List.fold (fun state x -> state + x + "\n") "" xs)
            }


        divAttr
          [ attr.id "divJobApplicationContent"
          ]
          [ divAttr
              [ attr.style "width : 100%"
              ]
              [ h3 [text (t German YourApplicationDocuments)]
                selectAttr
                  [ attr.id "slctDocumentName";
                    on.change
                      (fun el _ ->
                          async {
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
                    on.click(fun _ _ -> show ["divAddDocument"])
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
                            if slctEl?selectedIndex >= 0 && JS.Confirm(String.Format(t German ReallyDeleteDocument, varDocument.Value.name))
                            then
                                do! Server.deleteDocument varDocument.Value.id
                                let slctEl = JS.Document.GetElementById("slctDocumentName")
                                slctEl.RemoveChild(slctEl?(slctEl?selectedIndex)) |> ignore
                                if slctEl?length = 0
                                then
                                    el?style?display <- "none"
                                    varDocument.Value <- emptyDocument
                                    show ["divAddDocument"]
                                else
                                    show ["divAttachments"]
                                do! setDocument()
                                do! setPageButtons()
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
              [ attr.id "divVariables"
                attr.style "display: none"
              ]
              [ h3Attr
                  [ attr.``class`` "distanced-bottom" ]
                    [ text "Variablen" ]
                h4Attr
                  [ attr.``class`` "distanced-bottom" ]
                  [ text "Vordefiniert" ]
                b [ text "Arbeitgeber" ]
                br []
                text "$firmaName"
                br []
                text "$firmaStrasse"
                br []
                text "$firmaPlz"
                br []
                text "$firmaStadt"
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
                hr[]
                b [text (t German YourValues)]
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
                text "$meineMobilnr"
                br []
                text "$meineTelefonnr"
                br []
                hr []
                b [ text "Datum" ]
                br []
                text "$tag"
                br []
                text "$monat"
                br []
                text "$jahr"
                br []
                hr []
                b [ text "Sonstige" ]
                br []
                text "$jobName"
                br []
                br []
                hr []
                br[]
                h4Attr
                  [attr.``class`` "distanced-bottom"]
                  [ text "Benutzerdefiniert" ]
                Doc.InputArea
                    [ attr.style "width: 100%; min-height: 500px"
                      on.keyDown (fun el ev ->
                          if ev?keyCode=9
                          then
                            let v = el?value |> string
                            let s = el?selectionStart |> int
                            let e = el?selectionEnd |> int
                            el?value <- v.Substring(0, s) + "\t" + v.Substring(e)
                            el?selectionStart <- s + 1
                            el?selectionEnd <- s + 1
                            ev.StopPropagation()
                            ev.PreventDefault()
                      )
                    ]
                    customVariables
              ]
            divAttr
              [ attr.id "divAttachments"; attr.style "display: none"
              ]
              [ h3 [ text (t German YourAttachments) ]
                divAttr
                  [ attr.id "divAttachmentButtons"
                  ]
                  [ buttonAttr
                      [ attr.id "btnAddPage"
                        attr.style "margin:0; visibility : hidden"
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
              [ attr.id "divAddDocument"; attr.style "display: none" ]
              [ br []
                h3 [ text (t German AddDocument)]
                br []
                labelAttr
                  [ attr.``for`` "txtNewDocumentName" ]
                  [ text <| t German DocumentName ]
                inputAttr
                  [ attr.id "txtNewDocumentName"
                    attr.``class`` "form-control"
                  ]
                  []
                inputAttr
                  [ attr.``type`` "button"
                    attr.``class`` "btnLikeLink"
                    attr.value (t German AddDocument)
                    on.click (fun _ _ ->
                        async {
                            let newDocumentName = JS.Document.GetElementById("txtNewDocumentName")?value |> string
                            if newDocumentName.Trim() = ""
                            then
                                JS.Alert(String.Format(t German FieldIsRequired, t German DocumentName))
                            else
                                varDocument.Value <- { varDocument.Value with name = newDocumentName }
                                let! newDocumentId = Server.saveNewDocument varDocument.Value
                                varDocument.Value <- { varDocument.Value with id = newDocumentId }
                                let slctEl = JS.Document.GetElementById("slctDocumentName")
                                addSelectOption slctEl newDocumentName
                                JS.Document.GetElementById("divAddDocument")?style?display <- "none"
                                JS.Document.GetElementById("btnDeleteDocument")?style?display <- "inline"
                                slctEl?selectedIndex <- slctEl?options?length - 1
                                do! setDocument()
                                do! setPageButtons()
                                show ["divAttachments"]
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
              [ inputAttr
                  [ attr.``type`` "checkbox"
                    attr.value "false"
                    attr.id "chkReplaceVariables"
                  ]
                  []
                labelAttr
                  [ attr.``for`` "chkReplaceVariables" ]
                  [ text (t German ReplaceVariables)
                  ]
                br []
                buttonAttr
                  [ attr.``type`` "button"
                    on.click
                        (fun _ _ ->
                                let chkReplaceVariables = JQuery("#chkReplaceVariables")
                                match varDocument.Value.pages |> List.tryItem (getCurrentPageIndex() - 1) with
                                | Some (FilePage filePage) ->
                                    async {
                                        let! path =
                                            if chkReplaceVariables.Prop("checked")
                                            then
                                                Server.replaceVariables
                                                    filePage.path
                                                    varUserValues.Value
                                                    varEmployer.Value
                                                    varDocument.Value
                                            else async { return filePage.path }
                                        let fileName =
                                            let extension = filePage.path.Substring(filePage.path.LastIndexOf('.') + 1)
                                            if filePage.name.EndsWith ("." + extension)
                                            then filePage.name
                                            else filePage.name + "." + extension
                                        let! guid = Server.createLink path fileName
                                        JS.Window.Location.Href <- sprintf "download/%s" guid
                                    } |> Async.Start
                                | Some (HtmlPage _) -> ()
                                | None -> ()
                        )
                  ]
                  [ text (t German Download)
                  ]
              ]
            divAttr
              [ attr.id "divEmail"; attr.style "display: none"]
              [ h3 [text (t German Email) ]
                createInput (t German EmailSubject) documentEmailSubject (fun s -> "")
                (createTextarea (t German EmailBody) documentEmailBody "400px")
              ]
            divAttr
              [ attr.id "divChoosePageType"; attr.style "display: none" ]
              [ inputAttr
                  [ attr.``type`` "radio"
                    attr.disabled "true"
                    attr.name "rbgrpPageType"
                    attr.id "rbHtmlPage"
                    on.click (fun _ _ -> show ["divAttachments";"divChoosePageType"; "divCreateHtmlPage"]) ]
                  []
                labelAttr
                  [ attr.``for`` "rbHtmlPage"
                  ]
                  [ text (t German CreateOnline) ]
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
                  [ text (t German UploadFile)]
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
                  [text (t German AddHtmlAttachment)]
              ]
            divAttr
              [ attr.id "divCreateFilePage"; attr.style "display: none" ]
              [ formAttr
                  [ attr.method "POST"; attr.action "upload"; attr.enctype "multipart/form-data"
                  ]
                  [ text (t German PleaseChooseAFile)
                    br []
                    inputAttr
                      [ attr.``type`` "file"
                        attr.name "myFile"
                        attr.id "myFile"
                        on.change (fun el _ ->
                            async {
                                let file = (el?files)?item(0)
                                let fileName = file?name |> string
                                let fileExtension = fileName.Substring(fileName.LastIndexOf(".") + 1)
                                if (el?files)?item(0)?size > maxUploadSize
                                then
                                    JS.Alert(t German FileIsTooBig + "\n"
                                             + String.Format(t German UploadLimit, (maxUploadSize / 1000000) |> string))
                                elif not (supportedUnoconvFileTypes |> List.contains fileExtension)
                                then
                                    JS.Alert(String.Format("Entschuldigung.\n*.{0} Dateien können zur Zeit nicht ins PDF-Format verwandelt werden.
                                                          \nTypische Dateitypen zum Uploaden sind *.odt, *.docx und *.pdf.", fileExtension))
                                else
                                    el.ParentElement?submit()
                            } |> Async.Start
                        )
                      ]
                      []
                    inputAttr [ attr.``type`` "hidden"; attr.id "hiddenDocumentId"; attr.name "documentId"; attr.value "1" ] []
                    inputAttr [ attr.``type`` "hidden"; attr.id "hiddenNextPageIndex"; attr.name "pageIndex"; attr.value "1" ] []
                  ]
                br []
                hr []
                br []
                h3 [ text (t German YouMightWantToReplaceSomeWordsInYourFileWithVariables) ]
                text <| t German VariablesWillBeReplacedWithTheRightValuesEveryTimeYouSendYourApplication
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
                br []
                text "$jobName"
              ]
            divAttr
              [ attr.id "divSentApplications"
                attr.style "display: none"
              ]
              [ Doc.EmbedView varDivSentApplications.View
              ]
            divAttr
              [ attr.id "divEditUserValues"; attr.style "display: none" ]
              [ h3 [ text (t German YourValues) ]
                b
                  [ text "Tipp: Dies sind keine Pflichtangaben."
                  ]
                br []
                text "Lass Felder, die du nicht als Variablen verwenden willst einfach leer."
                br []
                br []
                createInput (t German Degree) userDegree (fun s -> "")
                createRadio
                  (t German Gender)
                  [ (t German Male), Gender.Male, userGender, ""
                    (t German Female), Gender.Female, userGender, ""
                  ]
                createInput (t German FirstName) userFirstName (fun s -> "")
                createInput (t German LastName) userLastName (fun s -> "")
                createInput (t German Street) userStreet (fun s -> "")
                createInput (t German Postcode) userPostcode (fun s -> "")
                createInput (t German City) userCity (fun s -> "")
                createInput (t German Phone) userPhone (fun s -> "")
                createInput (t German MobilePhone) userMobilePhone (fun s -> "")
              ]
            divAttr
              [ attr.id "divAddEmployer"
                attr.style "display: none"
              ]
              [ createInput (t German JobName) documentJobName (fun s -> "")
                h3 [text (t German Employer)]
                divAttr
                  [ attr.``class`` "form-group row" ]
                  [ divAttr
                      [ attr.``class`` "col-lg-3"
                      ]
                      [ buttonAttr
                          [ attr.``type`` "button"
                            attr.``class`` "btn-block"
                            attr.id "btnReadFromWebsite"
                            on.click (fun _ _ ->
                                readFromWebsite () |> Async.Start
                            )
                          ]
                          [ iAttr
                              [ attr.``class`` "fa fa-spinner fa-spin"
                                attr.id "faReadFromWebsite"
                                attr.style "color: black; margin-right: 10px; visibility: hidden"
                              ]
                              []
                            text <| t German LoadFromWebsite
                          ]
                    ]
                    divAttr
                      [attr.``class`` "col-lg-9"]
                      [ inputAttr
                          [ attr.id "txtReadEmployerFromWebsite"
                            attr.``type`` "text"
                            on.paste (fun el _ ->
                                readFromWebsite () |> Async.Start
                            )
                            attr.``class`` "form-control"
                            attr.placeholder "URL oder Referenznummer"
                            on.focus (fun el _ -> el?select())
                          ]
                          []
                      ]
                  ] 
                divAttr
                  [ attr.``class`` "form-group row col-12" ]
                  [ buttonAttr
                      [ attr.``type`` "button"
                        attr.``class`` "btnLikeLink btn-block"
                        attr.style "min-height: 40px; font-size: 20px"
                        attr.id "btnApplyNowTop"
                        on.click (fun _ _ ->
                          btnApplyNowClicked () |> Async.Start
                        )
                      ]
                      [ iAttr
                          [ attr.``class`` "fa fa-icon"
                            attr.id "faBtnApplyNowTop"
                            attr.style "color: #08a81b; margin-right: 10px"
                          ]
                          []
                        text <| t German ApplyNow
                      ]
                  ]
                createInput (t German CompanyName) employerCompany (fun (s : string) -> "")
                createInput (t German Street) employerStreet (fun (s : string) -> "")
                createInput (t German Postcode) employerPostcode (fun (s : string) -> "")
                createInput (t German City) employerCity (fun (s : string) -> "")
                createRadio
                    (t German Gender)
                    [ (t German Male), Gender.Male, employerGender, ""
                      (t German Female), Gender.Female, employerGender, ""
                      (t German UnknownGender), Gender.Unknown, employerGender, "checked"
                    ]
                createInput (t German Degree) employerDegree (fun s -> "")
                createInput (t German FirstName) employerFirstName (fun s -> "")
                createInput (t German LastName) employerLastName (fun (s : string) -> "")
                createInputWithPlaceholder "Tipp: Zum Testen eigene Email eintragen" (t German Email) employerEmail (fun (s : string) -> "")
                createInput (t German Phone) employerPhone (fun s -> "")
                createInput (t German MobilePhone) employerMobilePhone (fun s -> "")
                buttonAttr
                  [ attr.``type`` "button"
                    attr.``class`` "btnLikeLink btn-block"
                    attr.style "min-height: 40px; font-size: 20px"
                    attr.id "btnApplyNowBottom"
                    on.click (fun _ _ ->
                         btnApplyNowClicked () |> Async.Start
                    )
                  ]
                  [ iAttr
                      [ attr.``class`` "fa fa-icon"
                        attr.id "faBtnApplyNowBottom"
                        attr.style "color: #08a81b; margin-right: 10px"
                      ]
                      []
                    text <| t German ApplyNow
                  ]
              ]
          ]

