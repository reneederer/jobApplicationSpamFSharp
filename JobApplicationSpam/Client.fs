
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
    


    [<JavaScript>]
    let editUserValues () : Elt =
        let createUserValues gender degree firstName lastName street postcode city phone mobilePhone =
            { gender = gender
              degree = degree
              firstName = firstName
              lastName = lastName
              street = street
              postcode = postcode
              city = city
              phone = phone
              mobilePhone = mobilePhone
            }
        let varMessage = Var.Create("nothing")
        let subm userValues =
            async {
                Var.Set varMessage ("Setting user values...")
                let! result = Server.setUserValues userValues 1
                let m =
                    match result with
                    | Ok (v, _) ->  v
                    | Bad v -> sprintf "%A" v
                do! Async.Sleep(1000)
                varMessage.Value <- m
                return ()
            } |> Async.StartImmediate
            //userValues.firstName.Value <- ""
            //userValues.lastName.Value <- ""
        let abc userValues =
            async {
                let! r = Server.setUserValues userValues 1
                match r with
                | Ok (v, _) -> return div [text v]
                | Bad vs ->  return div [text <| String.concat ", " vs]
            } |> Doc.Async
        let varGender = Var.Create Gender.Male 
        let varDegree = Var.Create("1")
        let varFirstName = Var.Create("")
        let varLastName = Var.Create("")
        let varStreet = Var.Create("")
        let varPostcode = Var.Create("")
        let varCity = Var.Create("")
        let varPhone = Var.Create("")
        let varMobilePhone = Var.Create("")
        div [  h1 [text "Hello"]
               Doc.Radio [attr.id "male"; ] Gender.Male varGender
               labelAttr [attr.``for`` "male"; attr.radiogroup "gender"] [text "männlich"]
               br []
               Doc.Radio [attr.id "female"; attr.radiogroup "gender" ] Gender.Female varGender
               labelAttr [attr.``for`` "female"] [text "weiblich"]
               br []
               labelAttr [attr.``for`` "degree"] [text "Titel"]
               Doc.Input [ attr.``type`` "input"; attr.id "degree"; attr.value varDegree.Value ] varDegree
               br []
               labelAttr [attr.``for`` "firstName"] [text "Vorname"]
               Doc.Input [ attr.``type`` "input"; attr.id "firstName"; attr.value varFirstName.Value ] varFirstName
               br []
               labelAttr [attr.``for`` "lastName"] [text "Nachname"]
               Doc.Input [ attr.``type`` "input"; attr.name "lastName"; attr.value varLastName.Value ] varLastName
               br []
               labelAttr [attr.``for`` "street"] [text "Straße"]
               Doc.Input [ attr.``type`` "input"; attr.id "street"; attr.value varStreet.Value ] varStreet
               br []
               labelAttr [attr.``for`` "postcode"] [text "Postleitzahl"]
               Doc.Input [ attr.``type`` "input"; attr.id "postcode"; attr.value varPostcode.Value ] varPostcode
               br []
               labelAttr [attr.``for`` "city"] [text "Stadt"]
               Doc.Input [ attr.``type`` "input"; attr.id "city"; attr.value varCity.Value ] varCity
               br []
               labelAttr [attr.``for`` "phone"] [text "Telefon"]
               Doc.Input [ attr.``type`` "input"; attr.id "phone"; attr.value varPhone.Value ] varPhone
               br []
               labelAttr [attr.``for`` "mobilePhone"] [text "Mobil"]
               Doc.Input [ attr.``type`` "input"; attr.id "mobilePhone"; attr.value varMobilePhone.Value ] varMobilePhone
               br []

               Doc.Button "myBut" [attr.``type`` "submit"; ] (fun () -> subm (createUserValues varGender.Value varDegree.Value varFirstName.Value varLastName.Value varStreet.Value varPostcode.Value varCity.Value varPhone.Value varMobilePhone.Value) |> ignore)
               textView varMessage.View
               //subm <| createUserValues varGender.Value varDegree.Value varFirstName.Value varLastName.Value varStreet.Value varPostcode.Value varCity.Value varPhone.Value varMobilePhone.Value
            ]


    [<JavaScript>]
    let uploadTemplate1 () : Elt =
        let varMessage = Var.Create("nothing")
        let subme () =
            Var.Set varMessage ("Uploading")
        div
            [ h1 [text "Hello"]
              formAttr
                 [attr.enctype "multipart/form-data"; attr.method "POST"; on.submit (fun _ _ -> subme ())]
                 [ inputAttr [attr.``type`` "file"; attr.name "myFile"] []
                   inputAttr [attr.``type`` "submit"] []
                   //Doc.Button "myBut" [attr.``type`` "submit"; ] (fun () -> subme ())
                 ]
              textView varMessage.View
            ]

    [<JavaScript>]
    let addEmployer () : Elt =
        let createEmployer company street postcode city gender degree firstName lastName email phone mobilePhone =
            { company = company
              street = street
              postcode = postcode
              city = city
              gender = gender
              degree = degree
              firstName = firstName
              lastName = lastName
              email = email
              phone = phone
              mobilePhone = mobilePhone
            }
        let varMessage = Var.Create("nothing")
        let subm employer =
            async {
                Var.Set varMessage ("Setting user values...")
                let! result = Server.addEmployer employer 1
                let m =
                    match result with
                    | Ok (v, _) ->  v
                    | Bad v -> sprintf "%A" v
                do! Async.Sleep(1000)
                varMessage.Value <- m
                return ()
            } |> Async.StartImmediate
        let varCompany = Var.Create ""
        let varStreet = Var.Create ""
        let varPostcode = Var.Create ""
        let varCity = Var.Create ""
        let varGender = Var.Create Gender.Male 
        let varDegree = Var.Create "1"
        let varFirstName = Var.Create ""
        let varLastName = Var.Create ""
        let varEmail = Var.Create ""
        let varPhone = Var.Create ""
        let varMobilePhone = Var.Create ""
        div [  h1 [text "Add employer"]
               labelAttr [attr.``for`` "company"] [text "Firma"]
               Doc.Input [ attr.``type`` "input"; attr.id "company"; attr.value varStreet.Value ] varCompany
               br []
               labelAttr [attr.``for`` "street"] [text "Straße"]
               Doc.Input [ attr.``type`` "input"; attr.id "street"; attr.value varStreet.Value ] varStreet
               br []
               labelAttr [attr.``for`` "postcode"] [text "Postleitzahl"]
               Doc.Input [ attr.``type`` "input"; attr.id "postcode"; attr.value varPostcode.Value ] varPostcode
               br []
               labelAttr [attr.``for`` "city"] [text "Stadt"]
               Doc.Input [ attr.``type`` "input"; attr.id "city"; attr.value varCity.Value ] varCity
               br []
               Doc.Radio [attr.id "male"; ] Gender.Male varGender
               labelAttr [attr.``for`` "male"; attr.radiogroup "gender"] [text "männlich"]
               br []
               Doc.Radio [attr.id "female"; attr.radiogroup "gender" ] Gender.Female varGender
               labelAttr [attr.``for`` "female"] [text "weiblich"]
               br []
               labelAttr [attr.``for`` "degree"] [text "Titel"]
               Doc.Input [ attr.``type`` "input"; attr.id "degree"; attr.value varDegree.Value ] varDegree
               br []
               labelAttr [attr.``for`` "firstName"] [text "Vorname"]
               Doc.Input [ attr.``type`` "input"; attr.id "firstName"; attr.value varFirstName.Value ] varFirstName
               br []
               labelAttr [attr.``for`` "lastName"] [text "Nachname"]
               Doc.Input [ attr.``type`` "input"; attr.name "lastName"; attr.value varLastName.Value ] varLastName
               br []
               labelAttr [attr.``for`` "email"] [text "Email"]
               Doc.Input [ attr.``type`` "input"; attr.id "email"; attr.value varEmail.Value ] varEmail
               br []
               labelAttr [attr.``for`` "phone"] [text "Telefon"]
               Doc.Input [ attr.``type`` "input"; attr.id "phone"; attr.value varPhone.Value ] varPhone
               br []
               labelAttr [attr.``for`` "mobilePhone"] [text "Mobil"]
               Doc.Input [ attr.``type`` "input"; attr.id "mobilePhone"; attr.value varMobilePhone.Value ] varMobilePhone
               br []

               Doc.Button "Add employer" [attr.``type`` "submit"; ] (fun () -> subm (createEmployer varCompany.Value varStreet.Value varPostcode.Value varCity.Value varGender.Value varDegree.Value varFirstName.Value varLastName.Value varEmail.Value varPhone.Value varMobilePhone.Value) |> ignore)
               textView varMessage.View
               //subm <| createUserValues varGender.Value varDegree.Value varFirstName.Value varLastName.Value varStreet.Value varPostcode.Value varCity.Value varPhone.Value varMobilePhone.Value
            ]


    [<JavaScript>]
    let applyNow () =
        let submitIt () =
            let userId = 1
            let templateId = 1
            let employerId = 1
            async {
                let! applyNowResult = Server.applyNow userId employerId templateId
                return ()
            } |> Async.StartImmediate
        div
          [ formAttr
              [on.submit (fun _ _ -> submitIt ()) ]
              [ inputAttr [attr.``type`` "submit"; ] []
              ]
          ]


    [<JavaScript>]
    let uploadTemplate () =
        div
            [ h1 [text "Hello"]
              formAttr
                 [attr.enctype "multipart/form-data"; attr.method "POST"; attr.action ""]
                 [ inputAttr [attr.``type`` "text"; attr.name "templateName"] []
                   inputAttr [attr.``type`` "text"; attr.name "emailSubject"] []
                   inputAttr [attr.``type`` "text"; attr.name "emailBody"] []
                   inputAttr [attr.``type`` "file"; attr.name "fileOdt" ] []
                   inputAttr [attr.``type`` "file"; attr.name "filePdfs[]" ] []
                   inputAttr [attr.``type`` "file"; attr.name "filePdfs[]" ] []
                   inputAttr [attr.``type`` "submit"; ] []
                 ]
            ]
