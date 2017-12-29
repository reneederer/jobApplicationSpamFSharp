
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
    let editUserValues () : Elt = div []

    [<JavaScript>]
    let addEmployer () : Elt = div []

    [<JavaScript>]
    let applyNow () = div []

    [<JavaScript>]
    let uploadTemplate () = div []
    (*
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
            *)
     
     
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
    let public a () =
        JS.Alert("halsdf")

    [<JavaScript>]
    let createTemplate () = 
        let varUserGender = Var.Create Gender.Female
        let varUserDegree = Var.Create ""
        let varUserFirstName = Var.Create ""
        let varUserLastName = Var.Create ""
        let varUserStreet = Var.Create ""
        let varUserPostcode = Var.Create ""
        let varUserCity = Var.Create ""
        let varUserPhone = Var.Create ""
        let varUserMobilePhone = Var.Create ""
        let varCompanyName = Var.Create ""
        let varCompanyStreet = Var.Create ""
        let varCompanyPostcode = Var.Create ""
        let varCompanyCity = Var.Create ""
        let varBossGender = Var.Create Gender.Male 
        let varBossDegree = Var.Create ""
        let varBossFirstName = Var.Create ""
        let varBossLastName = Var.Create ""
        let varBossEmail = Var.Create ""
        let varBossPhone = Var.Create ""
        let varBossMobilePhone = Var.Create ""
        let varSubject = Var.Create "Bewerbung als ..."
        let varMainText = Var.Create ""
        let varCover = Var.Create Upload
        let varHtmlJobApplication = Var.Create {name = ""; pages = [] }
        let varHtmlJobApplicationNames = Var.Create [""]
        let varHtmlJobApplicationName = Var.Create ""
        let varHtmlJobApplicationPageTemplateNames = Var.Create [""]
        let varHtmlJobApplicationPageTemplateName = Var.Create ""
        let varTxtSaveAs = Var.Create ""
        let varHtmlJobApplicationId = Var.Create 0
        let varPageTemplate = Var.Create(Doc.Verbatim("<div>nothing here yet</div>"))
        let varPageTemplateMap = Var.Create(Map.empty)
        let fillHtmlJobApplication htmlJobApplicationOffset =
            async {
                let! htmlJobApplication = Server.getHtmlJobApplicationOffset htmlJobApplicationOffset
                varHtmlJobApplication.Value <- htmlJobApplication
                JQuery("#mainText").Val((htmlJobApplication.pages |> Seq.item 0).map.["mainText"].Replace("\\n", "\n")) |> ignore
                return ()
            }
        let setUserValues () =
            async {
                    let! userValues = Server.getCurrentUserValues ()
                    varUserGender.Value <- userValues.gender
                    varUserDegree.Value <- userValues.degree
                    varUserFirstName.Value <- userValues.firstName
                    varUserLastName.Value <- userValues.lastName
                    varUserStreet.Value <- userValues.street
                    varUserPostcode.Value <- userValues.postcode
                    varUserCity.Value <- userValues.city
                    varUserPhone.Value <- userValues.phone
                    varUserMobilePhone.Value <- userValues.mobilePhone
            }
    
        let updateMainText () =
            match varBossGender.Value with
            | Gender.Female ->
                varMainText.Value <- "Sehr geehrte Frau " + varBossDegree.Value + " " + varBossLastName.Value + "\n" + varMainText.Value
            | Gender.Male ->
                varMainText.Value <- "Sehr geehrter Herr " + varBossDegree.Value + " " + varBossLastName.Value + "\n" + varMainText.Value

        let resize (el : Dom.Element) (defaultWidth: int) =
            let jEl = JQuery el
            let str = jEl.Val().ToString().Replace(" ", "&nbsp;")
            if str = ""
            then jEl.Width(defaultWidth) |> ignore
            else
                let (span : JQuery) = JQuery("<span />").Attr("style", sprintf "font-family:%s; font-size: %s; font-weight: %s; visibility: hidden" (el?style?fontFamily) (el?style?fontSize) (el?style?fontWeight)).Html(str)
                span.AppendTo("body") |> ignore
                jEl.Width(span.Width()) |> ignore
                JQuery("body span").Last().Remove() |> ignore
            ()
        let resize1 (el1:JS) font fontSize fontWeight (defaultWidth: int) =
            let el = JQuery(el1)
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
            let span = JQuery("<span />").Attr("style", sprintf "font-family: %s; font-size: %s; font-weight: %s; letter-spacing:0pt; visibility: hidden;" font fontSize fontWeight).Html(str)
            span.AppendTo("body") |> ignore
            let spanWidth = span.Width()
            JQuery("body span:last").Remove() |> ignore
            spanWidth
        let findLineBreak (str : string) textAreaWidth font fontSize fontWeight =
            let rec findLineBreak' beginIndex endIndex n =
                if n < 0
                then str.Length
                else
                    let currentIndex = beginIndex + (endIndex - beginIndex + 1) / 2
                    let currentString = str.Substring(0, currentIndex)
                    let width = getWidth currentString font fontSize fontWeight
                    if width > textAreaWidth
                    then
                        if endIndex = currentIndex
                        then
                            //JS.Alert(currentIndex - 1 |> string)
                            currentIndex - 1
                        else
                            let nextEndIndex = currentIndex
                            findLineBreak' beginIndex nextEndIndex (n-1)
                    else
                        let nextBeginIndex = currentIndex
                        findLineBreak' nextBeginIndex endIndex (n-1)
            findLineBreak' 0 (str.Length) 30
        let getTextAreaLines (str : string) textAreaWidth font fontSize fontWeight =
            let lines = str.Split([|'\n'|]) |> Array.map (fun x -> if x.EndsWith(" ") then x.TrimEnd([|' '|]) + "\n" else x) |> List.ofArray
            let splitLines = 
                List.unfold
                    (fun state ->
                        match state with
                        | [] -> None
                        | x::xs ->
                            let splitIndex = findLineBreak x textAreaWidth font fontSize fontWeight
                            let splitIndexSpace =
                                let ar = x.ToCharArray() |> Array.take splitIndex |> Array.tryFindIndexBack (fun c -> List.contains c [' '; '.'; ','; ';'; '-'])
                                match ar, splitIndex = x.Length with
                                | None, _ -> splitIndex
                                | _, true -> splitIndex
                                | Some v, false -> v + 1
                            //JS.Alert(splitIndexSpace |> string)
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
            splitLines

        let saveHtmlJobApplication htmlJobApplicationName =
            async {
                let mainText =
                    getTextAreaLines (varMainText.Value) (JQuery("#mainText").Width()) "Arial" "12pt" "normal"
                    |> String.concat "\n"
                let htmlJobApplication =
                    { name = htmlJobApplicationName
                      pages =
                        [ { name = "Anschreiben"
                            jobApplicationPageTemplateId = 1
                            map = ["mainText", mainText] |> Map.ofList
                          }
                        ]
                    }
                let! htmlJobApplicationId = Server.saveHtmlJobApplication htmlJobApplication
                return htmlJobApplicationId
            }

        let setUserValues () =
            async {
                let userValues =
                    { gender = varUserGender.Value
                    ; degree = varUserDegree.Value
                    ; firstName = varUserFirstName.Value
                    ; lastName = varUserLastName.Value
                    ; street = varUserStreet.Value
                    ; postcode = varUserPostcode.Value
                    ; city = varUserCity.Value
                    ; phone = varUserPhone.Value
                    ; mobilePhone = varUserMobilePhone.Value
                    }
                let! result = Server.addUserValues userValues
                return ()
            }

        let addEmployer () =
            async {
                let employer =
                    { company = varCompanyName.Value
                      street = varCompanyStreet.Value
                      postcode = varCompanyPostcode.Value
                      city = varCompanyCity.Value
                      gender = varBossGender.Value
                      degree = varBossDegree.Value
                      firstName = varBossFirstName.Value
                      lastName = varBossLastName.Value
                      email = varBossEmail.Value
                      phone = varBossPhone.Value
                      mobilePhone = varBossMobilePhone.Value
                    }
                let! addEmployerResult =  Server.addEmployer employer
                let oAddedEmployerId =
                    match addEmployerResult with
                    | Ok (v, _) -> Some v
                    | Bad _ -> None
                return oAddedEmployerId
            }

        let applyNowWithHtmlTemplate () =
            async {
                let! oAddedEmployerId = addEmployer()
                do! setUserValues ()
                let! htmlJobApplicationId = saveHtmlJobApplication varHtmlJobApplicationName.Value

                match oAddedEmployerId with
                | Some employerId ->
                    let htmlJobApplication =
                        { name = varHtmlJobApplication.Value.name
                          pages =
                            [ { name = "Anschreiben"
                                jobApplicationPageTemplateId = 1
                                map = [("mainText", varMainText.Value)] |> Map.ofList
                              }
                            ]
                        }
                    let userValues =
                        { gender = varUserGender.Value
                          degree = varUserDegree.Value
                          firstName = varUserFirstName.Value
                          lastName = varUserLastName.Value
                          street = varUserStreet.Value
                          postcode = varUserPostcode.Value
                          city = varUserCity.Value
                          phone = varUserPhone.Value
                          mobilePhone = varUserMobilePhone.Value
                        }
                    let employer =
                        { company = varCompanyName.Value
                          street = varCompanyStreet.Value
                          postcode = varCompanyPostcode.Value
                          city = varCompanyCity.Value
                          gender = varBossGender.Value
                          degree = varBossDegree.Value
                          firstName = varBossFirstName.Value
                          lastName = varBossLastName.Value
                          email = varBossEmail.Value
                          phone = varBossPhone.Value
                          mobilePhone = varBossMobilePhone.Value
                        }
                    do Server.applyNowWithHtmlTemplate employer employerId htmlJobApplication userValues
                | None -> ()
            } |> Async.Start
        


        async {
            //do! fillHtmlJobApplication 1
            let! htmlJobApplicationNames = Server.getHtmlJobApplicationNames ()
            if not <| List.isEmpty htmlJobApplicationNames
            then
                varHtmlJobApplicationName.Value <- htmlJobApplicationNames.[0]
                varHtmlJobApplicationNames.Value <- htmlJobApplicationNames

            let! htmlJobApplicationPageTemplates = Server.getHtmlJobApplicationPageTemplates ()
            if not <| List.isEmpty htmlJobApplicationPageTemplates then
                varHtmlJobApplicationPageTemplateName.Value <- htmlJobApplicationPageTemplates |> List.head |> (fun t -> t.name)
                varHtmlJobApplicationPageTemplateNames.Value <- htmlJobApplicationPageTemplates |> List.map (fun t -> t.name)
                varPageTemplateMap.Value <- htmlJobApplicationPageTemplates |> List.map (fun t -> t.name, t.html) |> Map.ofList
                varPageTemplate.Value <- varPageTemplateMap.Value.[varHtmlJobApplicationPageTemplateName.Value] |> Doc.Verbatim
                let el = JS.Document.GetElementById("selectHtmlJobApplicationPageTemplate")
                while el = JS.Undefined || el?length <> htmlJobApplicationPageTemplates.Length do
                    do! Async.Sleep 50
                JS.Document.GetElementById("selectHtmlJobApplicationPageTemplate")?selectedIndex <- 0
                let inputGrowers = JQuery(".resizing")
                inputGrowers.Each(fun (n, el) -> el.AddEventListener("input", (fun () -> resize el 150), true)) |> ignore
                let fieldUpdaters = JQuery(".field-updating")
                fieldUpdaters.Each
                    (fun (n, (el : Dom.Element)) ->
                        el.AddEventListener
                            ( "input"
                            , (fun () ->
                                let updateElements = JQuery(sprintf "[data-update-field='%s']" ((JQuery(el)).Data("update-field").ToString()))
                                updateElements.Each
                                    (fun (n, updateElement) ->
                                        if updateElement <> el then
                                            JQuery(updateElement).Val(JQuery(el).Val() |> string) |> ignore
                                            resize updateElement 150
                                        ()
                                    ) |> ignore
                                ()
                              ), true
                            )
                    ) |> ignore
                JQuery("[data-variable-value]")
                    .Each(fun (n, el) ->
                        let jEl = JQuery el
                        match jEl.Data("variable-value") |> string with
                        | "today" -> jEl.Val(sprintf "%i.%i.%i" DateTime.Now.Day DateTime.Now.Month DateTime.Now.Year) |> ignore
                        | _ -> ()
                    ) |> ignore


            //do! setUserValues ()
        } |> Async.Start

        div
          [ h1 [ text "Create a template" ]
            Doc.SelectDyn [attr.style "min-width: 300px"; attr.id "selectHtmlJobApplicationName"; on.change (fun el _ -> ()(*fillHtmlJobApplication el?selectedIndex |> Async.Start*))] id varHtmlJobApplicationNames.View varHtmlJobApplicationName
            br []
            text "Pages: "
            br []
            ul
              [ li [ button [text "+"] ]
                li [ text "Anschreiben" ]
                li [button [text "+"] ]
              ]
            br []
            hr []
            br []
            div
              [ Doc.Radio [attr.id "upload"; on.click (fun _ _ -> JS.Alert(varCover.Value.ToString())); attr.radiogroup "cover" ] Upload varCover
                labelAttr [ attr.``for`` "upload" ] [text "Hochladen"]
                br []
                Doc.Radio [attr.id "create"; on.click (fun _ _ -> JS.Alert(varCover.Value.ToString())); attr.radiogroup "cover" ] Create varCover
                labelAttr [ attr.``for`` "create" ] [text "Online erstellen"]
                br []
                Doc.Radio [attr.id "useCreated"; on.click (fun _ _ -> JS.Alert(varCover.Value.ToString())); attr.radiogroup "cover" ] UseCreated varCover
                labelAttr [ attr.``for`` "useCreated" ] [text "Online erstellte Seite verwenden"]
                br []
                Doc.Radio [attr.id "ignoreCover"; on.click (fun _ _ -> JS.Alert(varCover.Value.ToString())); attr.radiogroup "cover" ] Ignore varCover
                labelAttr [ attr.``for`` "ignoreCover" ] [text "Nicht verwenden"]
                br []
              ]
            Doc.SelectDyn [attr.style "min-width: 300px"; attr.id "selectHtmlJobApplicationPageTemplate"; on.change (fun _ _ -> if varPageTemplateMap.Value.ContainsKey varHtmlJobApplicationPageTemplateName.Value then varPageTemplate.Value <- Doc.Verbatim <| varPageTemplateMap.Value.[varHtmlJobApplicationPageTemplateName.Value])] id varHtmlJobApplicationPageTemplateNames.View varHtmlJobApplicationPageTemplateName
            br []
            Doc.EmbedView varPageTemplate.View
            br []
            inputAttr [attr.``type`` "button"; attr.value "Abschicken"; on.click (fun _ _ -> applyNowWithHtmlTemplate())] []
          ]








