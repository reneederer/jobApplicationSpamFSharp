namespace JobApplicationSpam.Test

open NUnit.Framework
open FsUnit
open JobApplicationSpam
open Database
open Npgsql
open Chessie.ErrorHandling
open System
open Types
open log4net
open System.IO

module MyTests =
    log4net.Config.XmlConfigurator.Configure(new FileInfo(@"C:\Users\rene\Documents\Visual Studio 2017\Projects\jobApplicationSpamFSharp\JobApplicationSpam\log4net.config")) |> ignore
    
    let mutable dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
    dbConn.Open()
    let mutable transaction = dbConn.BeginTransaction()
    
    [<SetUp>]
    let setup () =
        dbConn <- new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspamtest")
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
    let ``addUserValues should add the UserValues`` () =
        let userValues = { gender = Gender.Female; degree = "Prof."; firstName = "Katja"; lastName = "Großjohann"; street = "Fürther Str. 22"; postcode = "90429"; city = "Nürnberg"; phone = "0911 91821"; mobilePhone = "0151 147111" }
        let userId = 1
        let addedUserValuesId =
            Database.addUserValues
                dbConn
                userValues
                userId
        Database.getUserValues dbConn userId |> should equal (Some userValues)







