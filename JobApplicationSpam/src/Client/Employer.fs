namespace JobApplicationSpam.Client
module Employer =
    open WebSharper.UI.Next.Html
    open WebSharper
    open WebSharper.UI.Next.Client
    open JobApplicationSpam
    open JobApplicationSpam.I18n
    open Types
    open WebSharper.JavaScript
    open ClientHelpers
    open WebSharper.UI.Next
    open ClientTypes

    [<JavaScript>]
    let getDivAddEmployer refJobName (varUserEmailInput : Var<Doc>) (refEmployer : RefEmployer) =
        divAttr
          [ attr.id "divAddEmployer"
            attr.style "display: none"
          ]
          [ createInput (t German JobName) refJobName (fun s -> "")
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
            buttonAttr
              [ attr.``type`` "button"
                attr.``class`` "btnLikeLink btn-block"
                attr.style "min-height: 40px; width: 100%; font-size: 20px"
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
            createInput (t German CompanyName) refEmployer.company (fun (s : string) -> "")
            createInput (t German Street) refEmployer.street (fun (s : string) -> "")
            createInput (t German Postcode) refEmployer.postcode (fun (s : string) -> "")
            createInput (t German City) refEmployer.city (fun (s : string) -> "")
            createRadio
                (t German Gender)
                [ (t German Phrase.Male), Gender.Male, refEmployer.gender , ""
                  (t German Phrase.Female), Gender.Female, refEmployer.gender , ""
                  (t German UnknownGender), Gender.Unknown, refEmployer.gender , "checked"
                ]
            createInput (t German Degree) refEmployer.degree (fun s -> "")
            createInput (t German FirstName) refEmployer.firstName (fun s -> "")
            createInput (t German LastName) refEmployer.lastName (fun (s : string) -> "")
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
                  refEmployer.email
              ]
            createInput (t German Phone) refEmployer.phone (fun s -> "")
            createInput (t German MobilePhone) refEmployer.mobilePhone (fun s -> "")
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

