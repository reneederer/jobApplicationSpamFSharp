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

    [<JavaScript>]
    let varLanguageDict = Var.Create<Map<Word, string>>(Deutsch.dict |> Map.ofList)

    [<JavaScript>]
    let t (w : Word) =
        varLanguageDict.Value.[w]

    [<JavaScript>]
    let login () =
        formAttr
          [ attr.action "/login"; attr.method "POST" ]
          [ h4 [text (t Login) ]
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
                  [ attr.``type`` "password"; attr.``class`` "form-control"; attr.name "txtLoginPassword"; attr.id "txtLoginPassword" ]
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
    

    [<JavaScript>]
    let templates () = 
        let varDocument = Var.Create emptyDocument
        let documentEmailSubject : IRef<string> = varDocument.Lens (fun x -> x.email.subject) (fun x v -> { x with email = {x.email with subject = v }})
        let documentEmailBody : IRef<string> = varDocument.Lens (fun x -> x.email.body) (fun x v -> { x with email = {x.email with body = v }})
        let documentJobName : IRef<string> = varDocument.Lens (fun x -> x.jobName) (fun x v -> { x with jobName = v })

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

        let varEmployer = Var.Create<Employer>({company="";gender=Gender.Unknown;degree="";firstName="";lastName="";street="";postcode="";city="";email="";phone="";mobilePhone=""})
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
                    tableAttr
                      [ attr.style "border-spacing: 10px; border-collapse: separate" ]
                      [ thead
                          [ tr
                              [ th [ text (t CompanyName) ]
                                th [ text (t AppliedOnDate) ]
                                th [ text (t AppliedAs) ]
                              ]
                          ]
                        tbody
                          [ for app in sentApplications do
                               yield!
                                 [ tr
                                     [ td
                                         [ text app.companyName ]
                                       td
                                         [ text (sprintf "%02i.%02i.%04i" app.statusChangedOn.Day app.statusChangedOn.Month app.statusChangedOn.Year) ]
                                       td
                                         [ text app.appliedAs ]
                                     ]
                                   :> Doc
                                 ]
                          ]
                      ]
            }
        


        let createInputWithColumnSizes1 column1Size column2Size labelText (ref : IRef<string>) (validFun : string -> string) =
          let guid = Guid.NewGuid().ToString("N")
          divAttr
            [attr.``class`` "form-group row"]
            [ labelAttr
                [attr.``class`` (column1Size + " col-form-label"); attr.``for`` guid]
                [text labelText]
              divAttr
                [ attr.``class`` column2Size]
                [ Doc.Input
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


        let createRadioWithColumnSizes1 column1Size column2Size (labelText : string) (radioValuesList : list<string * 'a * (IRef<'a>) * string>) =
          let radioGroup = Guid.NewGuid().ToString("N")    
          div
            (radioValuesList |> List.mapi (fun i (radioText, value, ref, ``checked``) ->
                let guid = Guid.NewGuid().ToString("N")
                divAttr
                  [attr.``class`` "form-group row"]
                  [ labelAttr
                       [attr.``class`` (column1Size + " col-form-label")]
                       [text (if i = 0 then labelText else "")]
                    divAttr
                      [ attr.``class`` column2Size
                      ]
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
                      ]
                  ] :> Doc
            )
            )
        let createTextareaWithColumnSizes column1Size column2Size labelText ref minHeight =
          let guid = Guid.NewGuid().ToString("N")
          divAttr
            [attr.``class`` "form-group row"]
            [ labelAttr
                [attr.``class`` (column1Size + " col-form-label"); attr.``for`` guid]
                [text labelText]
              divAttr
                [attr.``class`` column2Size]
                [ Doc.InputArea
                    [ attr.id guid
                      attr.``class`` "form-control"
                      attr.style ("wrap: soft; white-space: nowrap; overflow: auto; min-height: " + minHeight)
                    ]
                    ref
                ]
            ]

        let createInput = createInputWithColumnSizes1 "col-lg-3" "col-lg-9"
        let createRadio = createRadioWithColumnSizes1 "col-lg-3" "col-lg-9"
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
              "divSentApplications"
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
                    varDocument.Value <- emptyDocument
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
                                        let! _ = Server.overwriteDocument varDocument.Value
                                        do! setDocument()
                                        do! setPageButtons()
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
                                    let! _ = Server.overwriteDocument varDocument.Value
                                    do! setDocument()
                                    do! setPageButtons()
                                    show ["divAttachments"]
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
                                    show ["divAttachments"]
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
        
        let btnApplyNowClicked () =
            async {
                let emailRegexStr = """^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$"""
                let regex = RegExp(emailRegexStr)
                if not <| regex?test(employerEmail.Value)
                then
                    JS.Alert(t TheEmailOfYourEmployerDoesNotLookValid)
                elif documentJobName.Value.Trim() = ""
                then
                    JS.Alert(String.Format(t FieldIsRequired, (t JobName)))
                else
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

                    let! applyResult = Server.applyNow varEmployer.Value varDocument.Value varUserValues.Value

                    fontAwesomeEls
                    |> List.iter (fun faEl ->
                        faEl.RemoveClass("fa-spinner fa-spin") |> ignore
                    )
                    btnLoadFromWebsite.Prop("disabled", false) |> ignore
                    JQuery("#divJobApplicationContent").Find("input,textarea,button,select").Prop("disabled", false) |> ignore
                    match applyResult with
                    | Bad xs ->
                        do! Async.Sleep 700
                        JS.Alert(t SorryAnErrorOccurred + "\n" + t YourApplicationHasNotBeenSent)
                    | Ok _ ->
                        fontAwesomeEls
                        |> List.iter (fun faEl ->
                            faEl.Css("color", "#08a81b") |> ignore
                            faEl.AddClass("fa-check") |> ignore
                        )

                        JQuery("#divAddEmployer input[type='text'][data-bind]").Val("") |> ignore
                        JQuery("#divAddEmployer input[type='radio'][data-bind='bossGender'][value='u']").Prop("checked", "checked") |> ignore

                        do! Async.Sleep 3500

                        fontAwesomeEls
                        |> List.iter (fun faEl ->
                            faEl.RemoveClass("fa-check") |> ignore
                        )
            }
            
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

            addMenuEntry (t SentApplications) (fun _ _ ->
                async {
                    do! getSentApplications()
                    show ["divSentApplications"]
                } |> Async.Start
            ) |> ignore
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
            show [ "divAttachments" ]
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
                                    varDocument.Value <- emptyDocument
                                do! setDocument()
                                do! setPageButtons()
                                show ["divAttachments"]
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
              [ br []
                h4 [ text (t AddDocument)]
                br []
                labelAttr
                  [ attr.``for`` "txtNewDocumentName" ]
                  [ text <| t DocumentName ]
                inputAttr
                  [ attr.id "txtNewDocumentName"
                    attr.``class`` "form-control"
                  ]
                  []
                inputAttr
                  [ attr.``type`` "button"
                    attr.``class`` "btnLikeLink"
                    attr.value (t AddDocument)
                    on.click (fun _ _ ->
                        async {
                            let newDocumentName = JS.Document.GetElementById("txtNewDocumentName")?value |> string
                            if newDocumentName.Trim() = ""
                            then
                                JS.Alert(String.Format(t FieldIsRequired, t DocumentName))
                            else
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
                    //attr.``checked`` "false"
                    attr.value "false"
                    attr.id "chkReplaceVariables"
                  ]
                  []
                labelAttr
                  [ attr.``for`` "chkReplaceVariables" ]
                  [ text (t ReplaceVariables)
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
                                                Server.replaceVariables filePage.path varUserValues.Value varEmployer.Value varDocument.Value
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
                  [ text (t Download)
                  ]
              ]
            divAttr
              [ attr.id "divEmail"; attr.style "display: none"]
              [ h4 [text (t Email) ]
                createInput (t EmailSubject) documentEmailSubject (fun s -> "")
                (createTextArea (t EmailBody) documentEmailBody "400px")
              ]
            divAttr
              [ attr.id "divChoosePageType"; attr.style "display: none" ]
              [ inputAttr
                  [ attr.``type`` "radio"
                    //attr.disabled "true"
                    attr.name "rbgrpPageType"
                    attr.id "rbHtmlPage"
                    on.click (fun _ _ -> show ["divAttachments";"divChoosePageType"; "divCreateHtmlPage"]) ]
                  []
                labelAttr
                  [ attr.``for`` "rbHtmlPage"
                  ]
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
              [ formAttr
                  [ attr.method "POST"; attr.action "upload"; attr.enctype "multipart/form-data"
                  ]
                  [ text (t PleaseChooseAFile)
                    br []
                    inputAttr
                      [ attr.``type`` "file"
                        attr.name "myFile"
                        attr.id "myFile"
                        on.change(fun el _ ->
                            async {
                                let! maxUploadSize = Server.getMaxUploadSize ()
                                if (el?files)?item(0)?size > maxUploadSize
                                then
                                    JS.Document.GetElementById("btnUpload")?style?visibility <- "hidden"
                                    JS.Alert(t FileIsTooBig + "\n" + String.Format(t UploadLimit, (maxUploadSize / 1000000) |> string))
                                else
                                    JS.Document.GetElementById("btnUpload")?style?visibility <- "visible"
                            } |> Async.Start
                        )
                      ]
                      []
                    inputAttr [ attr.``type`` "hidden"; attr.id "hiddenDocumentId"; attr.name "documentId"; attr.value "1" ] []
                    inputAttr [ attr.``type`` "hidden"; attr.id "hiddenNextPageIndex"; attr.name "pageIndex"; attr.value "1" ] []
                    br []
                    br []
                    buttonAttr [attr.``type`` "submit"; attr.id "btnUpload"; attr.style "visibility: hidden" ] [text (t AddAttachment)]
                  ]
                br []
                hr []
                br []
                h4 [ text (t YouMightWantToReplaceSomeWordsInYourFileWithVariables) ]
                text <| t VariablesWillBeReplacedWithTheRightValuesEveryTimeYouSendYourApplication
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
              [ h4 [ text (t YourValues) ]
                createInput (t Degree) userDegree (fun s -> "")
                createRadio
                  (t Gender)
                  [ (t Male), Gender.Male, userGender, ""
                    (t Female), Gender.Female, userGender, ""
                  ]
                createInput (t FirstName) userFirstName (fun s -> "")
                createInput (t LastName) userLastName (fun s -> "")
                createInput (t Street) userStreet (fun s -> "")
                createInput (t Postcode) userPostcode (fun s -> "")
                createInput (t City) userCity (fun s -> "")
                createInput (t Phone) userPhone (fun s -> "")
                createInput (t MobilePhone) userMobilePhone (fun s -> "")
              ]
            divAttr
              [ attr.id "divAddEmployer"
                attr.style "display: none"
              ]
              [ createInput (t JobName) documentJobName (fun s -> "")
                h4 [text (t Employer)]
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
                            text <| t LoadFromWebsite
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
                  [ attr.``class`` "form-group row"
                  ]
                  [ divAttr
                      [ attr.``class`` "col-12"
                      ]
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
                            text <| t ApplyNow
                          ]
                      ]
                  ]
                createInput (t CompanyName) employerCompany (fun (s : string) -> "")
                createInput (t Street) employerStreet (fun (s : string) -> "")
                createInput (t Postcode) employerPostcode (fun (s : string) -> "")
                createInput (t City) employerCity (fun (s : string) -> "")
                createRadio
                    (t Gender)
                    [ (t Male), Gender.Male, employerGender, ""
                      (t Female), Gender.Female, employerGender, ""
                      (t UnknownGender), Gender.Unknown, employerGender, "checked"
                    ]
                createInput (t Degree) employerDegree (fun s -> "")
                createInput (t FirstName) employerFirstName (fun s -> "")
                createInput (t LastName) employerLastName (fun (s : string) -> "")
                createInput (t Email) employerEmail (fun (s : string) -> "")
                createInput (t Phone) employerPhone (fun s -> "")
                createInput (t MobilePhone) employerMobilePhone (fun s -> "")
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
                    text <| t ApplyNow
                  ]
              ]
          ]

