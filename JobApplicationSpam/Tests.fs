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
    let ``getEmployer 404 (nonexisting) should return None`` () =
        Database.getEmployer dbConn 404 |> should equal None

    [<Test; CategoryAttribute("Database")>]
    let ``getEmployer 1 (existing) should return Some {company = "BJC BEST JOB IT SERVICES GmbH"}`` () =
        Database.getEmployer dbConn 1 |> should equal (Some { company = "BJC BEST JOB IT SERVICES GmbH"; street = "Alte Rabenstraße 32"; postcode = "20148"; city = "Hamburg"; gender = Gender.Female; degree = ""; firstName = "Katrin"; lastName = "Thoms"; email = "Katrin.Thoms@bjc-its.de"; phone = "+49 (40) 5 14 00 7180"; mobilePhone = "" })

    [<Test; CategoryAttribute("Database")>]
    let ``addEmployer should add the employer record to the database`` () =
        let employerRecord = { company = "Hallo Welt"; street = "Meine Strasse"; postcode = "99999"; city = "aCity"; gender = Gender.Male; degree = "Prof."; firstName = "Hans"; lastName = "Meiser"; email = "hans@meiser.de"; phone = "080 8192"; mobilePhone = "0171 1231" }
        let addedEmployerId =
            Database.addEmployer
                dbConn
                employerRecord
                1
        Database.getEmployer dbConn addedEmployerId |> should equal (Some employerRecord)
        

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








