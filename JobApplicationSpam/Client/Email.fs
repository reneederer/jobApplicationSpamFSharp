namespace JobApplicationSpam.Client
module Email =
    open WebSharper.UI.Next.Html
    open WebSharper
    open JobApplicationSpam
    open ClientHelpers
    open Translation
    open Phrases

    [<JavaScript>]
    let getDivEmail refSubject refBody =
        divAttr
          [ attr.id "divEmail"; attr.style "display: none"]
          [ h3 [text (t German Email) ]
            createInput (t German EmailSubject) refSubject (fun s -> "")
            (createTextarea (t German EmailBody) refBody "400px")
          ]

