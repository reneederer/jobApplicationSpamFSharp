
namespace JobApplicationSpam

open WebSharper
open Npgsql
open Chessie.ErrorHandling
open System
open JobApplicationSpam.Types
module DB = JobApplicationSpam.Database


module Server =

//    let rene () : Async<Result<string, string>> =
    [<Remote>]
    let setUserValues (userValues : UserValues) (userId : int) : Async<Result<string, string>> =
        async {
            use dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
            dbConn.Open()
            use command = new NpgsqlCommand("select email from users where id = " + userValues.degree, dbConn)
            try
                match command.ExecuteScalar() with
                | null -> return fail "No record found"
                | v -> return ok (v.ToString())
            with
            //| :? PostgresException
            | _ ->
                return fail "An error occured while trying to add user."
        }

    [<Remote>]
    let setUserValues1 (userValues : UserValues) (userId : int) : Async<Result<string, string>>=
        async {
            use dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
            dbConn.Open()
            use command = new NpgsqlCommand("select email from users where id = 1", dbConn)
            try
                return ok (command.ExecuteScalar () |> string)
            with
            | :? PostgresException
            | _ ->
                return fail "An error occured while trying to add user."
        }

    let userEmailExists (dbConn : NpgsqlConnection) (email : string) =
        use command = new NpgsqlCommand("select count(*) from users where email = :email", dbConn)
        command.Parameters.Add(new NpgsqlParameter("email", email)) |> ignore
        try
            ok (Int32.Parse(command.ExecuteScalar().ToString()) = 1)
        with
        | :? PostgresException
        | _ ->
            fail "An error occured while checking if email exists"

    [<Remote>]
    let emailExists email = 
        async {
            use dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
            dbConn.Open()
            let emailExists = userEmailExists dbConn email
            let resultStr =
                match emailExists with
                | Ok (true, _) -> sprintf "Email %s does exist." email
                | Ok (false, _) -> sprintf "Email %s does not exist" email
                | Bad _ -> "an error occurred"
            return resultStr
        }

    
    [<Remote>]
    let addUser (email : string) (password : string) (salt : string) (guid : string) =
        ()
        (*
        let addUserDB (dbConn : NpgsqlConnection) (email : string) (password : string) (salt : string) (guid : string) =
            use command = new NpgsqlCommand("insert into users (email, password, salt, guid) values(:email, :password, :salt, :guid)", dbConn)
            command.Parameters.Add(new NpgsqlParameter("email", email)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("password", password)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("salt", salt)) |> ignore
            command.Parameters.Add(new NpgsqlParameter("guid", guid)) |> ignore
            try
                command.ExecuteNonQuery () |> ignore
                ok "User has been added."
            with
            | :? PostgresException
            | _ ->
                fail "An error occured while trying to add user."
        use dbConn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=jobapplicationspam")
        dbConn.Open()
        addUserDB dbConn email password salt guid
        *)




