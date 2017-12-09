namespace JobApplicationSpam
module Database =
    open System
    open Npgsql
    open Types
    open Chessie.ErrorHandling

    let private getUserValuesId (dbConn : NpgsqlConnection) (userId : int) =
        use command = new NpgsqlCommand("select userId from userValues where userId = :userId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        try
            stdout.WriteLine("hallo welt")
            match command.ExecuteScalar() |> string |> Int32.TryParse with
            | true, v ->
                stdout.WriteLine(sprintf "v: %i" v)
                ok v
            | false, _ -> fail "Not found"
        with
        | :? PostgresException
        | _ ->
            fail "An error occured while trying to add user."
        


    let setUserValuesDB (dbConn : NpgsqlConnection) (userValues : UserValues) (userId : int) =
        use command =
            match getUserValuesId dbConn userId with
            | Ok (userValuesId, _) ->
                let command = new NpgsqlCommand("""
                    update userValues set
                        (gender, degree, firstName, lastName, street, postcode, city, phone, mobilePhone)
                        = (:gender, :degree, :firstName, :lastName, :street, :postcode, :city, :phone, :mobilePhone)
                        where id = :userValuesId"""
                    , dbConn)
                //command.Parameters.Add(new NpgsqlParameter("gender", match userValues.gender.Value with Gender.Male -> 'm' | Gender.Female -> 'f')) |> ignore
                command.Parameters.Add(new NpgsqlParameter("degree", userValues.degree)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("firstName", userValues.firstName)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("lastName", userValues.lastName)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("street", userValues.street)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("postcode", userValues.postcode)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("city", userValues.city)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("phone", userValues.phone)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("mobilePhone", userValues.mobilePhone)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("userValuesId", userValuesId)) |> ignore
                command
            | Bad _ ->
                let command = new NpgsqlCommand("""
                    insert into userValues
                        (gender, degree, firstName, lastName, street, postcode, city, phone, mobilePhone)
                        values (:gender, :degree, :firstName, :lastName, :street, :postcode, :city, :phone, :mobilePhone)"""
                    , dbConn)
                //command.Parameters.Add(new NpgsqlParameter("gender", match userValues.gender.Value with Gender.Male -> 'm' | Gender.Female -> 'f')) |> ignore
                command.Parameters.Add(new NpgsqlParameter("degree", userValues.degree)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("firstName", userValues.firstName)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("lastName", userValues.lastName)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("street", userValues.street)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("postcode", userValues.postcode)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("city", userValues.city)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("phone", userValues.phone)) |> ignore
                command.Parameters.Add(new NpgsqlParameter("mobilePhone", userValues.mobilePhone)) |> ignore
                command
        try
            async {
                let _ = command.ExecuteNonQuery () 
                return ok "UserValues have been updated."
            }
        with
        | :? PostgresException
        | _ ->
            async {
                return fail "An error occured while trying to update userValues."
            }
    


