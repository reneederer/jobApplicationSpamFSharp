namespace JobApplicationSpam.Client

module ShowVariables =
    open WebSharper.UI.Next.Html
    open WebSharper
    open WebSharper.UI.Next.Client
    open JobApplicationSpam.I18n
    open JobApplicationSpam

    [<JavaScript>]
    let getDivVariables refCustomVariables =
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
                    refCustomVariables
              ]
