namespace JobApplicationSpam.Client
module UserValues =
    open WebSharper.UI.Next.Html
    open ClientHelpers
    open ClientTypes
    open JobApplicationSpam
    open Types
    open WebSharper
    open JobApplicationSpam.I18n

    [<JavaScript>]
    let getDivUserValues (refUserValues : RefUserValues) =
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
            createInput (t German Degree) refUserValues.degree (fun s -> "")
            createRadio
              (t German Gender)
              [ (t German Male), Gender.Male, refUserValues.gender, ""
                (t German Female), Gender.Female, refUserValues.gender, ""
              ]
            createInput (t German FirstName) refUserValues.firstName (fun s -> "")
            createInput (t German LastName) refUserValues.lastName (fun s -> "")
            createInput (t German Street) refUserValues.street (fun s -> "")
            createInput (t German Postcode) refUserValues.postcode (fun s -> "")
            createInput (t German City) refUserValues.city (fun s -> "")
            createInput (t German Phone) refUserValues.phone (fun s -> "")
            createInput (t German MobilePhone) refUserValues.mobilePhone (fun s -> "")
          ]

