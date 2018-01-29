namespace JobApplicationSpam.Test

open NUnit.Framework
open FsUnit
open JobApplicationSpam
open System.IO
open Types
open Variables
open Chessie.ErrorHandling

module MyTests =
    open System.Transactions

    [<Test>]
    let parseVariable00 () =
        let expected = parse """ $hallo = "we" + "lt" + $a"""
        let actual = ok ["$hallo", VariableExpression "welt$a"] : Result<list<string * Expression>, string>
        expected |> should equal actual

    [<Test>]
    let parseVariable01 () =
        let expected = parse """$hallo = "welt" """
        let actual = ok ["$hallo", VariableExpression "welt"] : Result<list<string * Expression>, string>
        expected |> should equal actual

    [<Test>]
    let parseVariable02 () =
        let expected = parse """ $hallo = "welt" + "da" + $bist + "du" """
        let actual = ok [("$hallo", VariableExpression "weltda$bistdu")] : Result<list<string * Expression>, string>
        expected |> should equal actual

    [<Test>]
    let parseVariable03 () =
        let expected = parse """  $hallo = "welt" +$da +"bist du"
        """
        let actual = ok ["$hallo", VariableExpression "welt$dabist du"] : Result<list<string * Expression>, string>
        expected |> should equal actual

    [<Test>]
    let parseVariable04 () =
        let expected = parse """  $hallo = $welt   """
        let actual = ok ["$hallo", VariableExpression "$welt"] : Result<list<string * Expression>, string>
        expected |> should equal actual

    [<Test>]
    let parseVariable05 () =
        let expected = parse """    $hallo = match $welt with
                              | "a" -> "b"
                              | "c" -> "d"
            """
        let actual = ok ["$hallo", (MatchExpression ("$welt", ["a", "b"; "c", "d"]))] : Result<list<string * Expression>, string>
        expected |> should equal actual

    [<Test>]
    let parseVariable06 () =
        let expected = parse """$hallo =match $welt with
                               | "a" -> "b"
                               | "c" -> "d"
                    $a = $b
                    $c = "d" + "e" + $f
                   $g = match $h with
                                | "i" -> "j"
                                | "k" -> "l" """

        let actual = ok [ "$hallo", (MatchExpression ("$welt", ["a", "b"; "c", "d"]))
                          "$a", VariableExpression "$b"
                          "$c", VariableExpression "de$f"
                          "$g", (MatchExpression ("$h", ["i", "j"; "k", "l"])) ] : Result<list<string * Expression>, string>
        expected |> should equal actual


    [<Test>]
    let parseVariable07 () =
         let expected = parse "$datumHeute = $tag + \".\" + $monat + \".\" + $jahr\n\n$anredeZeile =\n\tmatch $chefGeschlecht with\n\t| \"m\" -> \"Sehr geehrter Herr $chefTitel $chefNachname,\"\n\t| \"f\" -> \"Sehr geehrte Frau $chefTitel $chefNachname,\"\n\t| \"u\" -> \"Sehr geehrte Damen und Herren,\"\n\n$chefAnrede =\n\tmatch $chefGeschlecht with\n\t| \"m\" -> \"Herr\"\n\t| \"f\" -> \"Frau\"\n\n$chefAnredeBriefkopf =\n\tmatch $chefGeschlecht with\n\t| \"m\" -> \"Herrn\"\n\t| \"f\" -> \"Frau\"\n\n"
         let actual =
             ok
                [ "$datumHeute", VariableExpression "$tag.$monat.$jahr"
                  "$anredeZeile", (MatchExpression
                                    ( "$chefGeschlecht"
                                    , [ "m", "Sehr geehrter Herr $chefTitel $chefNachname,"
                                        "f", "Sehr geehrte Frau $chefTitel $chefNachname,"
                                        "u", "Sehr geehrte Damen und Herren,"]))
                  "$chefAnrede", (MatchExpression ("$chefGeschlecht", ["m", "Herr"; "f", "Frau"]))
                  "$chefAnredeBriefkopf", (MatchExpression ("$chefGeschlecht", ["m", "Herrn"; "f", "Frau"]))
                ] : Result<list<string * Expression>, string>
         expected |> should equal actual

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
    let replaceInString00 () =
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
        Odt.replaceInString """<?xml ?><text:p name="hallo">$a_ $b</text:p>welt""" (["$a", ""; "$b", "hallo"]) Ignore |> should equal "<?xml ?>welt"
    [<Test>]
    let replaceInString15 () =
        Odt.replaceInString """<?xml ?><text:p name="hallo">$a_</text:p>welt""" (["$a", "hallo"]) Ignore |> should equal """<?xml ?><text:p name="hallo">hallo</text:p>welt"""
    [<Test>]
    let replaceInString16 () =
        Odt.replaceInString """<?xml ?><text:p name="hallo">$a_</text:p>welt""" (["$a", ""]) Ignore |> should equal "<?xml ?>welt"
    [<Test>]
    let replaceInString17 () =
        Odt.replaceInString """<?xml ?><text:p name="hallo">$a $b</text:p>""" (["$a", "hallo"; "$b", "welt"]) Ignore |> should equal """<?xml ?><text:p name="hallo">hallo welt</text:p>"""
    [<Test>]
    let replaceInString18 () =
        Odt.replaceInString """<?xml ?><text:p name="hallo">$a_</text:p>""" (["$a", "hallo"]) Ignore |> should equal """<?xml ?><text:p name="hallo">hallo</text:p>"""
    [<Test>]
    let replaceInString19 () =
        Odt.replaceInString """<?xml ?><text:p name="hallo">$a_</text:p>""" (["$a", ""; "$b", ""]) Ignore |> should equal "<?xml ?>"
    [<Test>]
    let replaceInString20 () =
        Odt.replaceInString """<?xml ?><text:p name="hallo">abc$a_</text:p>$b def""" (["$a", ""; "$b", ""]) Ignore |> should equal "<?xml ?>def"
    [<Test>]
    let replaceInString21 () =
        Odt.replaceInString """<?xml ?><text:p name="hallo">$a_</text:p>welt""" (["$a", ""]) Ignore |> should equal "<?xml ?>welt"
    [<Test>]
    let replaceInString22 () =
        Odt.replaceInString """<?xml ?><text:p name="hallo">$a_</text:p>$b""" (["$a", ""; "$b", ""]) Ignore |> should equal "<?xml ?>"
    [<Test>]
    let replaceInString23 () =
        Odt.replaceInString """<?xml ?>hallo<text:p name="hallo">$a_</text:p>, grausame <text:p>$b_</text:p>welt""" (["$a", ""; "$b", ""]) Ignore |> should equal "<?xml ?>hallo, grausame welt"
    [<Test>]
    let replaceInString24 () =
        Odt.replaceInString """<?xml ?><text:p name="hallo">$a_$a welt</text:p>""" (["$a", ""]) Ignore |> should equal "<?xml ?>"
    [<Test>]
    let replaceInString25 () =
        Odt.replaceInString """<?xml ?><text:p name="hallo">$a_ $b welt</text:p>""" (["$a", ""; "$b", "hallo"]) Ignore |> should equal "<?xml ?>"
    [<Test>]
    let replaceInString26 () =
        Odt.replaceInString """<?xml ?><text:p name="hallo">$a_ $b</text:p>welt""" (["$a", ""; "$b", "hallo"]) Ignore |> should equal "<?xml ?>welt"
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
    [<Test>]
    let replaceInString28 () =
        (Odt.replaceInString
            "<?xml ?><text:p>$meinTitel $meinVorname $meinNachname</text:p><text:p>$meineStrasse</text:p><text:p>$meinePlz_ $meineStadt</text:p><text:p>Telefon: $meineTelefonnr</text:p><text:p>Mobil: $meineMobilnr_</text:p>"
            [ "$meinTitel", "Dr."
              "$meinVorname", "René"
              "$meinNachname", "Ederer"
              "$meineStrasse", "Raabstr. 24A"
              "$meinePlz", ""
              "$meineStadt", "Nürnberg"
              "$meineTelefonnr", "0911 12345"
              "$meineMobilnr", ""]
            Ignore
        ) |> should equal "<?xml ?><text:p>Dr. René Ederer</text:p><text:p>Raabstr. 24A</text:p><text:p>Telefon: 0911 12345</text:p>"



    log4net.Config.XmlConfigurator.Configure(new FileInfo(@"log4net.config")) |> ignore
    
    [<SetUp>]
    let setup () =
        ()
    
    [<TearDown>]
    let tearDown () =
        ()
    
(*

    [<Test; CategoryAttribute("Database")>]
    let ``login existing userId`` () =
        Server.deleteUserWithIdOne() |> Async.RunSynchronously
        Server.login "rene.ederer.nbg@gmail.com" "1234"
        |> Async.RunSynchronously
        |> should equal (ok () : Result<unit, string>)

    [<Test; CategoryAttribute("Database")>]
    let ``getEmail 404 (nonexisting) should return None`` () =
        Server.getEmailByUserId (UserId 404) |> Async.RunSynchronously |> should equal None

    [<Test; CategoryAttribute("Database")>]
    let ``getEmailByUserId 1 (existing) should return Some "rene.ederer.nbg (at) gmail.com"`` () =
        Server.getEmailByUserId (UserId 1) |> Async.RunSynchronously |> should equal <| Some "rene.ederer.nbg@gmail.com"

    [<Test; CategoryAttribute("Database")>]
    let ``Database.getUserValues' 1 (existing) should return Some {...}`` () =
        let userValues = { gender = Gender.Male; degree = ""; firstName = "René"; lastName = "Ederer"; street = "Raabstr. 24A"; postcode = "90429"; city = "Nürnberg"; phone = "kein Telefon"; mobilePhone = "kein Handy"; }
        Server.getUserValues' (UserId 1) |> should equal userValues

    [<Test; CategoryAttribute("Database")>]
    let ``setUserValues should set the UserValues`` () =
        let userValues = { gender = Gender.Female; degree = "Prof."; firstName = "Katja"; lastName = "Großjohann"; street = "Fürther Str. 22"; postcode = "90429"; city = "Nürnberg"; phone = "0911 91821"; mobilePhone = "0151 147111" }
        Server.setUserValues' userValues (UserId 1)
        Server.getUserValues' (UserId 1) |> should equal userValues

        (*

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


*)
