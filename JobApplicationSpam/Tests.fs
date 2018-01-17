namespace JobApplicationSpam.Test

open NUnit.Framework
open FsUnit
open JobApplicationSpam
open Database
open Npgsql
open System
open System.IO
open Types

module MyTests =
    open WebSharper.UI.Next.CSharp.Client.Html.on

    [<Test>]
    let deleteLine00 () =
        Odt.deleteLine "" 0 |> should equal ""
    [<Test>]
    let deleteLine01 () =
        Odt.deleteLine "\n" 0 |> should equal ""
    [<Test>]
    let deleteLine02 () =
        Odt.deleteLine "\n" 0 |> should equal ""
    [<Test>]
    let deleteLine03 () =
        Odt.deleteLine "\n\n" 1 |> should equal "\n"
    [<Test>]
    let deleteLine04 () =
        Odt.deleteLine "hallo\n" 3 |> should equal ""
    [<Test>]
    let deleteLine05 () =
        Odt.deleteLine "\nhallo\n" 3 |> should equal "\n"
    [<Test>]
    let deleteLine06 () =
        Odt.deleteLine "hallo\nwelt\n" 10 |> should equal "hallo\n"

    [<Test>]
    let deleteLine07 () =
        Odt.deleteLine "$a_\nwelt" 0 |> should equal "welt"


    [<Test>]
    let replaceInString () =
        Odt.replaceInString "$a welt" (["$a", "hallo"]) Ignore |> should equal "hallo welt"

    [<Test>]
    let replaceInString01 () =
        Odt.replaceInString "$a_ welt" (["$a", "hallo"]) Ignore |> should equal "hallo welt"

    [<Test>]
    let replaceInString02 () =
        Odt.replaceInString "$a_ $a welt" (["$a", "hallo"]) Ignore |> should equal "hallo hallo welt"


    [<Test>]
    let replaceInString03 () =
        Odt.replaceInString "$a_\nwelt" (["$a", "hallo"]) Ignore |> should equal "hallo\nwelt"

    [<Test>]
    let replaceInString04 () =
        Odt.replaceInString "$a_\nwelt" (["$a", ""]) Ignore |> should equal "welt"
    [<Test>]
    let replaceInString05 () =
        Odt.replaceInString "$a $b" (["$a", "hallo"; "$b", "welt"]) Ignore |> should equal "hallo welt"
    [<Test>]
    let replaceInString06 () =
        Odt.replaceInString "$a_" (["$a", "hallo"]) Ignore |> should equal "hallo"
    [<Test>]
    let replaceInString07 () =
        Odt.replaceInString "$a_\n" (["$a", ""; "$b", ""]) Ignore |> should equal ""
    [<Test>]
    let replaceInString08 () =
        Odt.replaceInString "abc$a_\n$b\ndef" (["$a", ""; "$b", ""]) Ignore |> should equal "\ndef"
    [<Test>]
    let replaceInString10 () =
        Odt.replaceInString "$a_\n$b" (["$a", ""; "$b", ""]) Ignore |> should equal ""
    [<Test>]
    let replaceInString11 () =
        Odt.replaceInString "$a_\n$b_" (["$a", ""; "$b", ""]) Ignore |> should equal ""
    [<Test>]
    let replaceInString12 () =
        Odt.replaceInString "$a_ $a welt" (["$a", ""]) Ignore |> should equal ""
    [<Test>]
    let replaceInString13 () =
        Odt.replaceInString "$a_ $b welt" (["$a", ""; "$b", "hallo"]) Ignore |> should equal ""
    [<Test>]
    let replaceInString14 () =
        Odt.replaceInString """<text:p name="hallo">$a_ $b</text:p>welt""" (["$a", ""; "$b", "hallo"]) Ignore |> should equal "welt"
    [<Test>]
    let replaceInString15 () =
        Odt.replaceInString """<text:p name="hallo">$a_</text:p>welt""" (["$a", "hallo"]) Ignore |> should equal """<text:p name="hallo">hallo</text:p>welt"""
    [<Test>]
    let replaceInString16 () =
        Odt.replaceInString """<text:p name="hallo">$a_</text:p>welt""" (["$a", ""]) Ignore |> should equal "welt"
    [<Test>]
    let replaceInString17 () =
        Odt.replaceInString """<text:p name="hallo">$a $b</text:p>""" (["$a", "hallo"; "$b", "welt"]) Ignore |> should equal """<text:p name="hallo">hallo welt</text:p>"""
    [<Test>]
    let replaceInString18 () =
        Odt.replaceInString """<text:p name="hallo">$a_</text:p>""" (["$a", "hallo"]) Ignore |> should equal """<text:p name="hallo">hallo</text:p>"""
    [<Test>]
    let replaceInString19 () =
        Odt.replaceInString """<text:p name="hallo">$a_</text:p>""" (["$a", ""; "$b", ""]) Ignore |> should equal ""
    [<Test>]
    let replaceInString20 () =
        Odt.replaceInString """<text:p name="hallo">abc$a_</text:p>$b def""" (["$a", ""; "$b", ""]) Ignore |> should equal "def"
    [<Test>]
    let replaceInString21 () =
        Odt.replaceInString """<text:p name="hallo">$a_</text:p>welt""" (["$a", ""]) Ignore |> should equal "welt"
    [<Test>]
    let replaceInString22 () =
        Odt.replaceInString """<text:p name="hallo">$a_</text:p>$b""" (["$a", ""; "$b", ""]) Ignore |> should equal ""
    [<Test>]
    let replaceInString23 () =
        Odt.replaceInString """<text:p name="hallo">$a_</text:p>$b_""" (["$a", ""; "$b", ""]) Ignore |> should equal ""
    [<Test>]
    let replaceInString24 () =
        Odt.replaceInString """<text:p name="hallo">$a_$a welt</text:p>""" (["$a", ""]) Ignore |> should equal ""
    [<Test>]
    let replaceInString25 () =
        Odt.replaceInString """<text:p name="hallo">$a_ $b welt</text:p>""" (["$a", ""; "$b", "hallo"]) Ignore |> should equal ""
    [<Test>]
    let replaceInString26 () =
        Odt.replaceInString """<text:p name="hallo">$a_ $b</text:p>welt""" (["$a", ""; "$b", "hallo"]) Ignore |> should equal "welt"
    [<Test>]
    let replaceInString27 () =
        (Odt.replaceInString
            "$meinTitel $meinVorname $meinNachname\n$meineStrasse\n$meinePlz_ $meineStadt\nTelefon: $meineTelefonnr\nMobil: $meineMobilnr_"
            [ "$meinTitel", "Dr."
              "$meinVorname", "René"
              "$meinNachname", "Ederer"
              "$meineStrasse", "Raabstr. 24A"
              "$meinePlz", ""
              "$meineStadt", "Nürnberg"
              "$meineTelefonnr", "0911 12345"
              "$meineMobilnr", ""]
            Ignore
        ) |> should equal "Dr. René Ederer\nRaabstr. 24A\nTelefon: 0911 12345\n"
    (*
    log4net.Config.XmlConfigurator.Configure(new FileInfo(@"log4net.config")) |> ignore
    
    let mutable dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
    dbConn.Open()
    let mutable transaction = dbConn.BeginTransaction()

    [<SetUp>]
    let setup () =
        dbConn <- new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
        dbConn.Open()
        transaction <- dbConn.BeginTransaction()
    
    [<TearDown>]
    let tearDown () =
        transaction.Rollback()
        transaction.Dispose()
        dbConn.Dispose()

    [<Test; CategoryAttribute("Database")>]
    let ``getEmail 404 (nonexisting) should return None`` () =
        Database.getEmailByUserId dbConn 443 |> should equal None

    [<Test; CategoryAttribute("Database")>]
    let ``getEmail 1 (existing) should return Some "rene.ederer.nbg (at) gmail.com"`` () =
        Database.getEmailByUserId dbConn 1 |> should equal <| Some "rene.ederer.nbg@gmail.com"

    [<Test; CategoryAttribute("Database")>]
    let ``getUserValues 1 (existing) should return Some {...}`` () =
        let userValues = { gender = Gender.Male; degree = ""; firstName = "René"; lastName = "Ederer"; street = "Raabstr. 24A"; postcode = "90429"; city = "Nürnberg"; phone = "kein Telefon"; mobilePhone = "kein Handy"; }
        Database.getUserValues dbConn 1 |> should equal (Some userValues)

    [<Test; CategoryAttribute("Database")>]
    let ``setUserValues should add the UserValues`` () =
        let userValues = { gender = Gender.Female; degree = "Prof."; firstName = "Katja"; lastName = "Großjohann"; street = "Fürther Str. 22"; postcode = "90429"; city = "Nürnberg"; phone = "0911 91821"; mobilePhone = "0151 147111" }
        let userId = 1
        let addedUserValuesId =
            Database.setUserValues
                dbConn
                userValues
                userId
        Database.getUserValues dbConn userId |> should equal (Some userValues)

    [<Test; CategoryAttribute("Database")>]
    let ``userEmailExists "rene.ederer(at)gmail.com (existing) should return true`` () =
        Database.userEmailExists dbConn "rene.ederer.nbg@gmail.com" |> should equal true


    [<Test; CategoryAttribute("Database")>]
    let ``userEmailExists "some.nonexisting(at)gmail.com (nonexisting) should return false`` () =
        Database.userEmailExists dbConn "some.nonexisting.nbg@gmail.com" |> should equal false


    [<Test; CategoryAttribute("Database")>]
    let ``getIdPasswordSaltAndGuid "rene.ederer.nbg(at)gmail.com" (existing) should return a tuple`` () =
        let expected =
            ( 1
            , "r99n/4/4NGGeD7pn4I1STI2rI+BFweUmzAqkxwLUzFP9aB7g4zR5CBHx+Nz2yn3NbiY7/plf4ZRGPaXXnQvFsA=="
            , "JjjYQTWgutm4pv/VnzgHf6r4NjNrAVcTq+xnR7/JsRGAIHRdrcw3IMVrzngn2KPRakfX/S1kl9VrqwAT+T02Og=="
            , (None : option<string>)
            )
        Database.getIdPasswordSaltAndGuid dbConn "rene.ederer.nbg@gmail.com" |> should equal (Some expected)

    [<Test; CategoryAttribute("Database")>]
    let ``getIdPasswordSaltAndGuid "some.nonexisting(at)gmail.com" (nonexisting) should return a tuple`` () =
        Database.getIdPasswordSaltAndGuid dbConn "some.nonexisting@gmail.com" |> should equal None


    [<Test; CategoryAttribute("Database")>]
    let ``insertNewUser "some.nonexisting(at)gmail.com" (existing) should throw an Exception`` () =
        let insertedNewUserId = Database.insertNewUser dbConn "some.nonexisting@gmail.com" "password" "salt" "guid"
        let email = Database.getEmailByUserId dbConn insertedNewUserId
        email |> should equal (Some "some.nonexisting@gmail.com")

    [<Test; CategoryAttribute("Database")>]
    let ``insertNewUser "rene.ederer.nbg(at)gmail.com" (existing) should throw a PostgresException`` () =
        (fun () -> Database.insertNewUser dbConn "rene.ederer.nbg@gmail.com" "password" "salt" "guid" |> ignore)
        |> should throw typeof<PostgresException>

    [<Test; CategoryAttribute("Database")>]
    let ``getGuid "some.nonexisting(at)gmail.com" (nonexisting) should throw an exception`` () =
        (fun () -> Database.getGuid dbConn "some.nonexisting@gmail.com" |> ignore)
        |> should throw typeof<Exception>


    [<Test; CategoryAttribute("Database")>]
    let ``getGuid "rene.ederer.nbg(at)gmail.com" (existing with guid=null) should return None`` () =
        Database.getGuid dbConn "rene.ederer.nbg@gmail.com" |> should equal None

    [<Test; CategoryAttribute("Database")>]
    let ``getGuid "helmut.goerke(at)gmail.com" (existing with guid=someGuid) should return Some "..."`` () =
        (Database.getGuid dbConn "helmut.goerke@gmail.com" |> Option.isSome) |> should be True


    [<Test; CategoryAttribute("Database")>]
    let ``setGuidToNull "rene.ederer.nbg(at)gmail.com" (existing, guid is null) should throw an Exception`` () =
        (fun () -> Database.setGuidToNull dbConn "rene.ederer.nbg@gmail.com" |> ignore)
        |> should throw typeof<Exception>

    [<Test; CategoryAttribute("Database")>]
    let ``setGuidToNull "some.nonexisting(at)gmail.com" (nonexisting) should throw an Exception`` () =
        (fun () -> Database.setGuidToNull dbConn "some.nonexisting@gmail.com" |> ignore)
        |> should throw typeof<Exception>

    [<Test; CategoryAttribute("Database")>]
    let ``setGuidToNull "helmut.goerke(at)gmail.com" (existing, guid not null) should set guid to null`` () =
        Database.setGuidToNull dbConn "helmut.goerke@gmail.com"
        Database.getGuid dbConn "helmut.goerke@gmail.com" |> Option.isNone |> should be True







*)
