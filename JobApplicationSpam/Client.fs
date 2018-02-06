﻿namespace JobApplicationSpam

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
    open Phrases
    open Translation


    [<JavaScript>]
    let togglePassword (el : JQuery) =
        if el.Attr("type") = "password"
        then
            el.Attr("type", "text") |> ignore
        else
            el.Attr("type", "password") |> ignore
        el.Next().ToggleClass("fa-eye fa-eye-slash") |> ignore
        el.Focus() |> ignore
        ()


    [<JavaScript>]
    let loginOrOutButton () =
        let varIsGuest = Var.Create false
        async {
            let! isGuest = Server.isLoggedInAsGuest()
            varIsGuest.Value <- isGuest
        } |> Async.Start
        match varIsGuest.Value with
        | true ->
            formAttr
              [ attr.action "/ghi" ]
              [ buttonAttr
                  [ attr.``type`` "submit"
                  ]
                  [text "Login"]
              ]
            :> Doc
        | false ->
            formAttr
              [ attr.action "/logout" ]
              [ buttonAttr
                  [ attr.``type`` "submit"
                  ]
                  [text "Logout"]
              ]
            :> Doc

    [<JavaScript>]
    let logout () =
        Cookies.Expire("user")        
        div
          []

    [<JavaScript>]
    let changePassword() =
        div
          [ formAttr
              [ on.submit (fun _ _ ->
                    async {
                        do! Server.setPassword (JS.Document.GetElementById("txtNewPassword")?value)
                    } |> Async.Start
                )
              ]
              [ h3 [text (t German ChangePassword) ]
                divAttr
                  [ attr.``class`` "form-group" ]
                  [ b
                      [ labelAttr
                          [ attr.``for`` "txtNewPassword" ] 
                          [text "Neues Password"]
                      ]
                    div
                      [ inputAttr
                          [ attr.``type`` "text"
                            attr.``class`` "form-control"
                            attr.name "txtNewPassword"
                            attr.id "txtNewPassword" ]
                          []
                        iAttr
                          [ attr.``class`` "fa fa-eye fa-2x";
                            Attr.Create "aria-hidden" "true"
                            attr.style "float: right; position: relative; margin-top: -36px; margin-right: 15px"
                            on.click (fun _ _ ->
                                togglePassword <| JQuery("#txtNewPassword")
                            )
                          ]
                          []
                      ]
                  ]
                inputAttr
                  [ attr.``type`` "submit"
                    attr.value (t German ChangePassword)
                  ]
                  []
              ]
          ]

    [<JavaScript>]
    let login () =
        div
          [ formAttr
              [ attr.action "/ghi"; attr.method "POST" ]
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
        let varUserEmailInput = Var.CreateWaiting<Doc>()

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
            let varColumns =
                ListModel.FromSeq
                  [ "Firma", employerCompany, true
                    "Vorname", employerFirstName, true
                    "Nachname", employerLastName, false
                    "Straße", employerStreet, true
                    "PLZ", employerPostcode, true
                    "Stadt", employerCity, false
                  ]
            let chooseColumns (employer : Employer) (url : string) =
                divAttr
                  [ attr.``class`` "modal fade in";
                    attr.id "employerModal"
                    attr.tabindex "-1"
                    Attr.Create "role" "dialog"
                    Attr.Create "aria-labelledby" "exampleModalLabel"
                    Attr.Create "aria-hidden" "true"
                  ]
                  [ divAttr
                      [ attr.``class`` "modal-dialog"
                        Attr.Create "role" "document"
                      ]
                      [ divAttr
                          [ attr.``class`` "modal-content"
                          ]
                          [ divAttr
                              [ attr.``class`` "modal-header"
                              ]
                              [ h5Attr
                                  [ attr.``class`` "modal-title"
                                    attr.id "exampleModalLabel"
                                  ]
                                  [ text employer.company ]
                                buttonAttr
                                  [ attr.``type`` "button"
                                    attr.``class`` "close"
                                    Attr.Create "aria-label" "Close"
                                    attr.``data-`` "dismiss" "modal"
                                  ]
                                  [ spanAttr
                                      [ Attr.Create "aria-hidden" "true"
                                      ]
                                      [ text "x" ]
                                  ]
                              ]
                            divAttr
                              [ attr.``class`` "modal-body"
                              ]
                              [ (Doc.BindSeqCached
                                    (fun (name, ref, selected) ->
                                        let guid = Guid.NewGuid().ToString("N")
                                        div
                                          [ inputAttr
                                              [ attr.``type`` "checkbox"
                                                attr.id guid
                                                on.change (fun el _ ->
                                                    varColumns.Value <-
                                                        varColumns.Value
                                                        |> Seq.map (fun (name1, ref1, selected1) -> (name1, ref1, el?``checked``))
                                                )
                                              ]
                                              []
                                            labelAttr
                                              [ attr.``for`` guid
                                              ]
                                              [ text name ]
                                          ]
                                    )
                                    varColumns.View
                                )
                              ]
                          ]
                      ]
                  ]
            let createEmployerModal (employer : Employer) (url : string) =
                divAttr
                  [ attr.``class`` "modal fade in";
                    attr.id "employerModal"
                    attr.tabindex "-1"
                    Attr.Create "role" "dialog"
                    Attr.Create "aria-labelledby" "exampleModalLabel"
                    Attr.Create "aria-hidden" "true"
                  ]
                  [ divAttr
                      [ attr.``class`` "modal-dialog"
                        Attr.Create "role" "document"
                      ]
                      [ divAttr
                          [ attr.``class`` "modal-content"
                          ]
                          [ divAttr
                              [ attr.``class`` "modal-header"
                              ]
                              [ h5Attr
                                  [ attr.``class`` "modal-title"
                                    attr.id "exampleModalLabel"
                                  ]
                                  [ text employer.company ]
                                buttonAttr
                                  [ attr.``type`` "button"
                                    attr.``class`` "close"
                                    Attr.Create "aria-label" "Close"
                                    attr.``data-`` "dismiss" "modal"
                                  ]
                                  [ spanAttr
                                      [ Attr.Create "aria-hidden" "true"
                                      ]
                                      [ text "x" ]
                                  ]
                              ]
                            divAttr
                              [ attr.``class`` "modal-body"
                              ]
                              [ text employer.company
                                br []
                                text employer.street
                                br []
                                text (employer.postcode + " " + employer.city)
                                br []
                                text ( (if employer.degree <>  "" then employer.degree + " " else "")
                                        + employer.firstName + " " + employer.lastName)
                                br []
                                text employer.email
                                br []
                                text employer.phone
                                br []
                                text employer.mobilePhone
                                br []
                                (if url.Contains("://")
                                 then
                                     aAttr
                                       [ attr.href url
                                         attr.target "blank"
                                       ]
                                       [ text url ]
                                 else text url :?> Elt)
                              ]
                            divAttr
                              [ attr.``class`` "modal-footer"
                              ]
                              []
                          ]
                      ]
                  ]
            let varEmployerModal = Var.Create(createEmployerModal emptyEmployer "")

            async {
                let! sentApplications = Server.getSentApplications ()
                let varSentApplications = ListModel.FromSeq sentApplications
                
                let setSentApplicationsFromTo () =
                    let dateFromParsed = DateTime.Parse(JS.Document.GetElementById("dateFrom")?value)
                    let dateFrom = DateTime(dateFromParsed.Year, dateFromParsed.Month, dateFromParsed.Day)
                    let dateToParsed = DateTime.Parse(JS.Document.GetElementById("dateTo")?value)
                    let dateTo = DateTime(dateToParsed.Year, dateToParsed.Month, dateToParsed.Day)
                    let dateFrom, dateTo = 
                        if dateFrom <= dateTo
                        then dateFrom, dateTo
                        else dateTo, dateFrom
                    varSentApplications.Value <-
                        sentApplications
                        |> List.filter (fun (_, _, d : DateTime, _) ->
                            d >= dateFrom && d <= dateTo
                        )
                
                varDivSentApplications.Value <-
                    divAttr
                      [ attr.style "width: 100%; height: 100%; overflow: auto"
                      ]
                      [ text "Von"
                        inputAttr
                          [ attr.``type`` "date"
                            attr.id "dateFrom"
                            attr.value
                                ( let dateFrom = DateTime.Now.AddMonths(-1).AddDays(float (-(DateTime.Now.Day) + 1))
                                  sprintf "%04i-%02i-%02i" dateFrom.Year dateFrom.Month dateFrom.Day)
                            attr.style "margin-left: 15px; margin-right: 15px;"
                            on.change (fun _ _ ->
                                setSentApplicationsFromTo ()
                            )
                          ]
                          []
                        text "bis"
                        inputAttr
                          [ attr.``type`` "date"
                            attr.id "dateTo"
                            attr.value
                                ( let dateTo = DateTime.Now.AddMonths(1).AddDays(float -DateTime.Now.Day)
                                  sprintf "%04i-%02i-%02i" dateTo.Year dateTo.Month dateTo.Day)
                            attr.style "margin-left: 15px;"
                            on.change (fun _ _ ->
                                setSentApplicationsFromTo ()
                            )
                          ]
                          []
                        buttonAttr
                          [ attr.style "float : right;"
                          ]
                          [ text "Liste downloaden" ]
                        Doc.EmbedView varEmployerModal.View
                        tableAttr
                          [ attr.style "border-spacing: 10px; border-collapse: separate" ]
                          [ thead
                              [ tr
                                  [ Doc.BindSeqCached
                                        (fun (name, ref, selected) ->
                                            if selected
                                            then th [ text name ]
                                            else text "" :?> Elt
                                        ) varColumns.View ]
                                  //@ [ th [ text "an dich mailen" ] ])
                              ]
                            tbody
                              [
                                (*
                                Doc.BindSeqCached
                                    (fun (name, ref : IRef<string>, selected) ->
                                        if selected && varSentApplications.Value |> Seq.contains 
                                        then td [ text ref.Value ]
                                        else text "" :?> Elt
                                    )
                                    varColumns.View
                                    *)

                                (*
                                (Doc.BindSeqCached
                                    (fun (employer : Employer, jobName : string, appliedOn : DateTime, url : string) ->
                                        tr
                                          [ td
                                              [ buttonAttr
                                                  [ on.click (fun el _ ->
                                                        async {
                                                            varEmployerModal.Value <- createEmployerModal employer url
                                                            do! Async.Sleep 100
                                                            JQuery("#btnHelperShowEmployerModal").Click() |> ignore
                                                        } |> Async.Start
                                                    )
                                                    attr.``type`` "button"
                                                    attr.``class`` "btn btn-primary"
                                                  ]
                                                  [ text employer.company ]
                                                buttonAttr
                                                  [
                                                    attr.id "btnHelperShowEmployerModal"
                                                    attr.``class`` "btn btn-primary"
                                                    attr.``data-`` "toggle" "modal"
                                                    attr.``data-`` "target" "#employerModal"
                                                    attr.style "visibility: hidden"
                                                  ]
                                                  [ text "" ]
                                              ]
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
                                       
                                    )
                                varSentApplications.View
                                )
                                *)


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
        
        let addSelectOption el value =
            let optionEl = JS.Document.CreateElement("option")
            optionEl.TextContent <- value
            el?add(optionEl)
        

        let setDocument () =
            async {
                let slctDocumentNameEl = JS.Document.GetElementById("slctDocumentName")
                let! oDocument =
                    if slctDocumentNameEl?selectedIndex >= 0
                    then Server.getDocumentOffset slctDocumentNameEl?selectedIndex
                    else async { return None }
                match oDocument with
                | Some document ->
                    varDocument.Value <- document
                | None ->
                    let! documentId = Server.saveNewDocument emptyDocument
                    varDocument.Value <- { emptyDocument with oId = Some documentId }
                    addSelectOption slctDocumentNameEl varDocument.Value.name
                JS.Document.GetElementById("hiddenDocumentId")?value <- varDocument.Value.GetId()
                show ["divAttachments"]
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
                                        do! Server.overwriteDocument varDocument.Value
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
                                    do! Server.overwriteDocument varDocument.Value
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
                                    do! Server.overwriteDocument varDocument.Value
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
        
        let btnApplyNowClicked () =
            async {
                let! isGuest = Server.isLoggedInAsGuest()

                let emailRegexStr = """^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$"""
                let regex = RegExp(emailRegexStr)
                let! sentApplication (*TODO this is an option<int> instead of option<SentApplication> as placeholder*) =
                    Server.tryFindSentApplication varEmployer.Value
                let! setUserEmailResult = Server.setUserEmail varUserEmail.Value

                let userEmailValid() =
                    if isGuest
                    then
                        if not <| regex?test(varUserEmail.Value)
                        then
                            JS.Alert("Deine Email scheint ungültig zu sein.")
                            false
                        else
                            match setUserEmailResult with
                            | Ok _ -> true
                            | Bad xs ->
                                JS.Alert(String.Concat(xs))
                                false
                    else
                        true

                let employerValid() =
                    if not <| regex?test(employerEmail.Value)
                     then
                         JS.Alert(t German TheEmailOfYourEmployerDoesNotLookValid + ", " + employerEmail.Value)
                         false
                     else true
                    
                let jobNameValid () =
                     if documentJobName.Value.Trim() = ""
                     then
                         JS.Alert(String.Format(t German FieldIsRequired, (t German JobName)))
                         false
                     else true
                
                let sentAlreadyValid() =
                    sentApplication.IsNone
                    || (sentApplication.IsSome
                        && JS.Confirm("Du hast dich schon einmal bei dieser Firmen-Email-Adresse beworben.\nBewerbung trotzdem abschicken?"))
                
                if userEmailValid() && employerValid() && jobNameValid() && sentAlreadyValid()
                then
                    do! Server.overwriteDocument varDocument.Value

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
                            (JS.Document.GetElementById("txtReadFromWebsite")?value)

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
                        JS.Document.GetElementById("txtReadFromWebsite")?value <- ""

                        do! Async.Sleep 4500

                        fontAwesomeEls
                        |> List.iter (fun faEl ->
                            faEl.RemoveClass("fa-check") |> ignore
                        )
            }

        let readFromWebsite () =
            async {
                JS.Document.GetElementById("btnReadFromWebsite")?disabled <- true
                JS.Document.GetElementById("faReadFromWebsite")?style?visibility <- "visible"
                do! Async.Sleep 200
                let! employerResult = Server.readWebsite (JS.Document.GetElementById("txtReadFromWebsite")?value)
                JS.Document.GetElementById("btnReadFromWebsite")?disabled <- false
                JS.Document.GetElementById("faReadFromWebsite")?style?visibility <- "hidden"
                match employerResult with
                | Ok (employer, _) ->
                    varEmployer.Value <- employer
                | Bad (xs) ->
                    JS.Alert(List.fold (fun state x -> state + x + "\n") "" xs)
            }



        let registerEvents () =
            JS.Document.GetElementById("slctDocumentName").AddEventListener
                ( "change"
                , (fun () ->
                      async {
                        do! setDocument()
                        do! setPageButtons()
                        do! fillDocumentValues()
                      } |> Async.Start)
                , false)
            JS.Document.GetElementById("btnShowDivNewDocument").AddEventListener
                ( "click"
                , fun () -> show ["divAddDocument"]
                , false)
            JS.Document.GetElementById("btnDeleteDocument").AddEventListener
                ( "click"
                , fun (ev : Dom.Event) ->
                        async {
                            let slctEl = JS.Document.GetElementById("slctDocumentName")
                            if slctEl?selectedIndex >= 0 && JS.Confirm(String.Format(t German ReallyDeleteDocument, varDocument.Value.name))
                            then
                                match varDocument.Value.oId with
                                | Some documentId -> do! Server.deleteDocument documentId
                                | None -> ()
                                let slctDocumentNameEl = JS.Document.GetElementById("slctDocumentName")
                                slctDocumentNameEl.RemoveChild(slctDocumentNameEl?(slctDocumentNameEl?selectedIndex)) |> ignore
                                if slctDocumentNameEl?length = 0
                                then
                                    ev.Target?style?display <- "none"
                                    varDocument.Value <- emptyDocument
                                show ["divAttachments"]
                            else
                                varDocument.Value <- emptyDocument
                            do! setDocument()
                            do! setPageButtons()
                        } |> Async.Start
                , false)
            JS.Document.GetElementById("txtUserDefinedVariables").AddEventListener
                ( "keydown"
                , fun (ev : Dom.Event) ->
                          if ev?keyCode=9
                          then
                            let v = ev.Target?value |> string
                            let s = ev.Target?selectionStart |> int
                            let e = ev.Target?selectionEnd |> int
                            ev.Target?value <- v.Substring(0, s) + "\t" + v.Substring(e)
                            ev.Target?selectionStart <- s + 1
                            ev.Target?selectionEnd <- s + 1
                            ev.StopPropagation()
                            ev.PreventDefault()
                , false)
            JS.Document.GetElementById("btnAddPage").AddEventListener
                ( "click"
                , fun (_ : Dom.Event) ->
                      if JS.Document.GetElementById("slctDocumentName")?selectedIndex >= 0
                      then
                          show ["divChoosePageType"; "divAttachments"; "divCreateFilePage"]
                          JS.Document.GetElementById("rbFilePage")?``checked`` <- true;
                      else
                          JS.Alert("Bitte erst eine neue Bewerbungsmappe anlegen")
                          show ["divAddDocument"]
                , false)
            JS.Document.GetElementById("btnAddDocument").AddEventListener
                ( "click"
                , fun (ev : Dom.Event) ->
                      async {
                          let newDocumentName = JS.Document.GetElementById("txtNewDocumentName")?value |> string
                          if newDocumentName.Trim() = ""
                          then
                              JS.Alert(String.Format(t German FieldIsRequired, t German DocumentName))
                          else
                              varDocument.Value <- { varDocument.Value with name = newDocumentName }
                              let! newDocumentId = Server.saveNewDocument varDocument.Value
                              varDocument.Value <- { varDocument.Value with oId = Some newDocumentId }
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
                , false)
            JS.Document.GetElementById("slctHtmlPageTemplate").AddEventListener
                ( "change"
                , fun (_ : Dom.Event) ->
                      async {
                        do! loadPageTemplate()
                        do! fillDocumentValues()
                      } |> Async.Start
                , false)
            JS.Document.GetElementById("btnDownloadFilePage").AddEventListener
                ( "click"
                , fun (_ : Dom.Event) ->
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
                , false)
            JS.Document.GetElementById("rbHtmlPage").AddEventListener
                ( "click"
                , fun (_ : Dom.Event) -> show ["divAttachments";"divChoosePageType"; "divCreateHtmlPage"]
                , false)
            JS.Document.GetElementById("rbFilePage").AddEventListener
                ( "click"
                , fun (_ : Dom.Event) -> show ["divAttachments"; "divChoosePageType"; "divCreateFilePage"]
                , false)
            JS.Document.GetElementById("btnCreateHtmlPage").AddEventListener
                ( "click"
                , fun (_ : Dom.Event) ->
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
                , false)

            JS.Document.GetElementById("fileToUpload").AddEventListener
                ( "change"
                , fun (ev : Dom.Event) ->
                      async {
                          let file = (ev.Target?files)?item(0)
                          let fileName = file?name |> string
                          let fileExtension = fileName.Substring(fileName.LastIndexOf(".") + 1)
                          if (ev.Target?files)?item(0)?size > maxUploadSize
                          then
                              JS.Alert(t German FileIsTooBig + "\n"
                                       + String.Format(t German UploadLimit, (maxUploadSize / 1000000) |> string))
                          elif not (supportedUnoconvFileTypes |> List.contains fileExtension)
                          then
                              JS.Alert(String.Format("Entschuldigung.\n*.{0} Dateien können zur Zeit nicht ins PDF-Format verwandelt werden.
                                                    \nTypische Dateitypen zum Uploaden sind *.odt, *.doc, *.jpg oder *.pdf.", fileExtension))
                          else
                              ev.Target?parentElement?submit()
                      } |> Async.Start
                , false)
            JS.Document.GetElementById("btnReadFromWebsite").AddEventListener
                ( "click"
                , fun () -> readFromWebsite () |> Async.Start
                , false)
            JS.Document.GetElementById("txtReadFromWebsite").AddEventListener
                ( "paste"
                , fun () -> readFromWebsite () |> Async.Start
                , false)
            JS.Document.GetElementById("txtReadFromWebsite").AddEventListener
                ( "focus"
                , fun (ev : Dom.Event) -> ev.Target?select()
                , false)
            JS.Document.GetElementById("btnSetEmployerEmailToUserEmail").AddEventListener
                ( "click"
                , fun () -> employerEmail.Value <- varUserEmail.Value
                , false)
            JS.Document.GetElementById("btnApplyNowTop").AddEventListener
                ( "click"
                , fun () -> btnApplyNowClicked() |> Async.Start
                , false)
            JS.Document.GetElementById("btnApplyNowBottom").AddEventListener
                ( "click"
                , fun () -> btnApplyNowClicked() |> Async.Start
                , false)

        let saveChanges () =
            async {
                do! Server.overwriteDocument varDocument.Value
                do! Server.setUserValues varUserValues.Value
            }
        
        async {
            let! loggedIn = Server.isLoggedIn()
            if not loggedIn
            then
                do! JobApplicationService.loginWithCookieOrAsGuest()
                JS.Window.Location.Href <- "/"
            let! isGuest = Server.isLoggedInAsGuest()
            if isGuest
            then
                
                varUserEmailInput.Value <- createInput "Deine Email" varUserEmail (fun x -> "")
            else varUserEmailInput.Value <- text ""

            let divMenu = JS.Document.GetElementById("divSidebarMenu")
            while divMenu = null do
                do! Async.Sleep 10

            let addMenuEntry entry (f : Dom.Element -> Event -> unit) = 
                let li = JQuery(sprintf """<li><button class="btnLikeLink1">%s</button></li>""" entry).On("click", ((*saveChanges();*) f))
                JQuery(divMenu).Append(li)

            addMenuEntry (t German SentApplications) (fun _ _ ->
                async {
                    do! getSentApplications()
                    show ["divSentApplications"]
                } |> Async.Start
            ) |> ignore

            addMenuEntry "Variablen" (fun _ _ -> show ["divVariables"]) |> ignore
            //addMenuEntry (t German EditYourValues) (fun _ _ -> show ["divEditUserValues"]) |> ignore
            addMenuEntry (t German EditEmail) (fun _ _ -> show ["divEmail"]) |> ignore
            addMenuEntry (t German EditAttachments) (fun _ _ -> show ["divAttachments"]) |> ignore
            addMenuEntry (t German AddEmployerAndApply) (fun _ _ -> show ["divAddEmployer"]) |> ignore

            let! oUserEmail = Server.getUserEmail()
            varUserEmail.Value <- oUserEmail |> Option.defaultValue ""
        
            let! userValues = Server.getUserValues()
            varUserValues.Value <- emptyUserValues


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
            JQuery(JS.Document).Ready(fun () -> registerEvents()) |> ignore
        } |> Async.Start

        divAttr
          [ attr.id "divJobApplicationContent"
          ]
          [ divAttr
              [ attr.style "width : 100%"
              ]
              [ h3 [text (t German YourApplicationDocuments)]
                selectAttr
                  [ attr.id "slctDocumentName";
                  ]
                  []
                buttonAttr
                  [ attr.``type`` "button"
                    attr.style "margin-left: 20px"
                    attr.``class`` ".btnLikeLink"
                    attr.id "btnShowDivNewDocument"
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
                h5Attr
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
                text "$tagHeute"
                br []
                text "$monatHeute"
                br []
                text "$jahrHeute"
                br []
                hr []
                b [ text "Sonstige" ]
                br []
                text "$jobName"
                br []
                br []
                hr []
                br[]
                h5Attr
                  [attr.``class`` "distanced-bottom"]
                  [ text "Benutzerdefiniert" ]
                Doc.InputArea
                    [ attr.style "width: 100%; min-height: 500px"
                      attr.id "txtUserDefinedVariables"
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
                        attr.style "margin:0;"
                        attr.``class`` "btnLikeLink"
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
                    attr.id "btnAddDocument"
                    attr.value (t German AddDocument)
                  ]
                  []
              ]
            divAttr
              [ attr.id "divDisplayedDocument"; attr.style "display: none"]
              [ selectAttr
                  [ attr.id "slctHtmlPageTemplate" ]
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
                    attr.id "btnDownloadFilePage"
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
                  ]
                  []
                labelAttr
                  [ attr.``for`` "rbHtmlPage"
                  ]
                  [ text (t German CreateOnline) ]
                br []
                inputAttr
                  [ attr.``type`` "radio"; attr.id "rbFilePage"; attr.name "rbgrpPageType";
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
                    attr.id "btnCreateHtmlPage"
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
                        attr.name "fileToUpload"
                        attr.id "fileToUpload"
                      ]
                      []
                    inputAttr [ attr.``type`` "hidden"; attr.id "hiddenDocumentId"; attr.name "documentId"; attr.value "1" ] []
                    inputAttr [ attr.``type`` "hidden"; attr.id "hiddenNextPageIndex"; attr.name "pageIndex"; attr.value "1" ] []
                  ]
                br []
                hr []
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
                  [ text "Dies sind keine Pflichtangaben."
                  ]
                br []
                text """Diese Angaben setzen die Werte von Variablen die mit "$mein" beginnen, zum Beispiel "$meinVorname". Statt hier deine Daten einzutragen kannst du alle Vorkommen dieser Variablen auch direkt ersetzen."""
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
                Doc.EmbedView varUserEmailInput.View
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
                          [ attr.id "txtReadFromWebsite"
                            attr.``type`` "text"
                            attr.``class`` "form-control"
                            attr.placeholder "URL oder Referenznummer"
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
                divAttr
                  [ attr.``class`` "form-group bottom-distanced"]
                  [ div
                      [ labelAttr
                          [ attr.``for`` "txtEmployerEmail"
                            attr.style "font-weight: bold"
                          ]
                          [ text "Email" ]
                        buttonAttr
                          [ attr.id "btnSetEmployerEmailToUserEmail"
                            attr.``class`` "distanced"
                          ]
                          [text "an dich"]
                      ]
                    Doc.Input
                      [ attr.id "txtEmployerEmail"
                        attr.``class`` "form-control"
                        attr.``type`` "text"
                      ]
                      employerEmail
                  ]
                createInput (t German Phone) employerPhone (fun s -> "")
                createInput (t German MobilePhone) employerMobilePhone (fun s -> "")
                buttonAttr
                  [ attr.``type`` "button"
                    attr.``class`` "btnLikeLink btn-block"
                    attr.style "min-height: 40px; font-size: 20px"
                    attr.id "btnApplyNowBottom"
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

