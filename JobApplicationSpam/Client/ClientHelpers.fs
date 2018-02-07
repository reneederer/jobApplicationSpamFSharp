namespace JobApplicationSpam.Client
module ClientHelpers =
    open System
    open WebSharper.UI.Next
    open WebSharper.UI.Next.Html
    open WebSharper.UI.Next.Client
    open WebSharper
    open WebSharper.JavaScript

    [<JavaScript>]
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


    [<JavaScript>]
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

    [<JavaScript>]
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
