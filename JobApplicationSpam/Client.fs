
namespace JobApplicationSpam

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Html

[<JavaScript>]
module Client =

    open Server
    open WebSharper
    open WebSharper.JavaScript
    open WebSharper.UI.Next
    open WebSharper.UI.Next.Client
    open WebSharper.UI.Next.Html
    open Chessie.ErrorHandling
    open JobApplicationSpam.Types

    
    
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


    let editUserValues () : Elt =
        let varMessage = Var.Create("nothing")
        let subm userValues =
            Var.Set varMessage ("Doing something")
            async {
                let! result = Server.setUserValues userValues 1
                let m =
                    match result with
                    | Ok (v, _) ->  v
                    | Bad v -> sprintf "%A" v
                do! Async.Sleep(1000)
                Var.Set varMessage m
                JS.Alert("Hallo")
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







