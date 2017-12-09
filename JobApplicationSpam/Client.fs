
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

    
    
    let createUserValues1 gender degree firstName lastName street postcode city phone mobilePhone =
        { //gender = Var.Create<Gender>(gender)
          degree = Var.Create<string>(degree)
          firstName = Var.Create<string>(firstName)
          lastName = Var.Create<string>(lastName)
          street = Var.Create<string>(street)
          postcode= Var.Create<string>(postcode)
          city = Var.Create<string>(city)
          phone = Var.Create<string>(phone)
          mobilePhone = Var.Create<string>(mobilePhone)
        }
    let createUserValues degree =
        {
          degree = degree
        }


    let editUserValues () : Elt =
        let userValues2 = createUserValues1 Gender.Male "dr" "rene" "ederer" "Raabstr. 24A" "90429" "Nuernberg" "noPhone" "noMobilePhone"
        let userValues1 = createUserValues1 Gender.Male "dr" "rene" "ederer" "Raabstr. 24A" "90429" "Nuernberg" "noPhone" "noMobilePhone"
        let subm userValues =
            async {
                let! result = Server.setUserValues userValues 1
                let re =
                    match result with
                    | Ok (v, _) -> JS.Alert(v)
                    | Bad v -> JS.Alert(sprintf "%A" v)
                return ()
            } |> Async.Start
            //userValues.firstName.Value <- ""
            //userValues.lastName.Value <- ""
        let abc userValues =
            async {
                let! r = Server.setUserValues userValues 1
                match r with
                | Ok (v, _) -> return div [text v]
                | Bad vs ->  return div [text <| String.concat ", " vs]
            } |> Doc.Async
        let degreeVar = Var.Create("2")
        div [  h1 [text "Hello"]
               (*Doc.Radio [attr.id "male"; ] Gender.Male userValues.gender
               labelAttr [attr.``for`` "male"; attr.radiogroup "gender"] [text "männlich"]
               br []
               Doc.Radio [attr.id "female"; attr.radiogroup "gender" ] Gender.Female userValues.gender
               labelAttr [attr.``for`` "female"] [text "weiblich"]
               br []
               *)
//               labelAttr [attr.``for`` "degree"] [text "Titel"]
               //Doc.Input [ attr.``type`` "input"; attr.id "degree"; attr.value userValues.degree.Value ] userValues.degree
               Doc.Input [ attr.``type`` "input"; attr.id "degree"; attr.value degreeVar.Value ] degreeVar
               //br []
 //              labelAttr [attr.``for`` "firstName"] [text "Vorname"]
               //Doc.Input [ attr.``type`` "input"; attr.id "firstName"; attr.value userValues.firstName.Value ] userValues.firstName
               //br []
  //             labelAttr [attr.``for`` "lastName"] [text "Nachname"]
               //Doc.Input [ attr.``type`` "input"; attr.name "lastName"; attr.value userValues.lastName.Value ] userValues.lastName
               br []
   //            labelAttr [attr.``for`` "street"] [text "Straße"]
               //Doc.Input [ attr.``type`` "input"; attr.id "street"; attr.value userValues.street.Value ] userValues.street
               br []
    //           labelAttr [attr.``for`` "postcode"] [text "Postleitzahl"]
               //Doc.Input [ attr.``type`` "input"; attr.id "postcode"; attr.value userValues.postcode.Value ] userValues.postcode
               br []
     //          labelAttr [attr.``for`` "city"] [text "Stadt"]
               //Doc.Input [ attr.``type`` "input"; attr.id "city"; attr.value userValues.city.Value ] userValues.city
               br []
      //         labelAttr [attr.``for`` "phone"] [text "Telefon"]
               //Doc.Input [ attr.``type`` "input"; attr.id "phone"; attr.value userValues.phone.Value ] userValues.phone
               br []
       //        labelAttr [attr.``for`` "mobilePhone"] [text "Mobil"]
               //Doc.Input [ attr.``type`` "input"; attr.id "mobilePhone"; attr.value userValues.mobilePhone.Value ] userValues.mobilePhone
               br []
               Doc.Button "myBut" [attr.``type`` "submit"; ] (fun () -> subm <| createUserValues degreeVar.Value)
               abc <| createUserValues degreeVar.Value
            ]







