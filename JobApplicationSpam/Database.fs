﻿namespace JobApplicationSpam
module Database =
    open System
    open Npgsql
    open Types
    open Chessie.ErrorHandling

    let getUserValuesId (dbConn : NpgsqlConnection) (userId : int) =
        use command = new NpgsqlCommand("select userId from userValues where userId = :userId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        try
            match command.ExecuteScalar() |> string |> Int32.TryParse with
            | true, v ->
                ok v
            | false, _ -> fail "Not found"
        with
        | :? PostgresException
        | _ ->
            fail "An error occured while trying to add user."
        
    let getEmployer (dbConn : NpgsqlConnection) (employerId : int) =
        use command = new NpgsqlCommand("select company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone from employer where id = :employerId limit 1", dbConn)
        command.Parameters.Add(new NpgsqlParameter("employerId", employerId)) |> ignore
        use reader = command.ExecuteReader()
        reader.Read() |> ignore
        ok
          { company = reader.GetString(0)
            street = reader.GetString(1)
            postcode = reader.GetString(2)
            city = reader.GetString(3)
            gender = match reader.GetString(4) with "m" -> Gender.Male | "f" -> Gender.Female | _ -> failwith "Gender not valid"
            degree = reader.GetString(5)
            firstName = reader.GetString(6)
            lastName = reader.GetString(7)
            email = reader.GetString(8)
            phone = reader.GetString(9)
            mobilePhone = reader.GetString(10)
          }
    

    let getFirstEmployerOffset12ekjksdfa (dbConn : NpgsqlConnection) (userId : int) (offset : int) =
        use command = new NpgsqlCommand("select company, street, postcode, city, gender, degree, firstName, lastName, email, phone, mobilePhone from employer where userId = :userId limit 1 offset :offset", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("offset", offset)) |> ignore
        try
            let reader = command.ExecuteReader()
            failwith "Not implemenetedddd"
        with
        | :? PostgresException
        | _ ->
            fail "An error occured while trying to add user."

    

    let getTemplateForJobApplication (dbConn : NpgsqlConnection) (templateId : int) =
        use command = new NpgsqlCommand("select emailSubject, emailBody, odtPath from jobApplicationTemplate where id = :templateId", dbConn)
        command.Parameters.Add(new NpgsqlParameter("templateId", templateId)) |> ignore
        use reader = command.ExecuteReader()
        reader.Read() |> ignore
        let emailSubject = reader.GetString(0)
        let emailBody = reader.GetString(1)
        let odtPath = reader.GetString(2)
        reader.Dispose()
        command.Dispose()
        use command1 = new NpgsqlCommand("select pdfPath from jobApplicationPdfAppendix where jobApplicationTemplateId = :templateId", dbConn)
        command1.Parameters.Add(new NpgsqlParameter("templateId", templateId)) |> ignore
        use reader1 = command1.ExecuteReader()
        let mutable pdfPaths = list.Empty
        while reader1.Read() do
            pdfPaths <- reader1.GetString(0) :: pdfPaths
        ok
          { emailSubject = emailSubject
            emailBody = emailBody
            odtPath = odtPath
            pdfPaths = pdfPaths
          }


    let getTemplateIdWithOffset (dbConn : NpgsqlConnection) (userId : int) (offset : int)=
        use command = new NpgsqlCommand("select id from jobApplicationTemplate where userId = :userId limit 1 offset :offset", dbConn)
        command.Parameters.Add(new NpgsqlParameter("userId", userId)) |> ignore
        command.Parameters.Add(new NpgsqlParameter("offset", offset)) |> ignore
        try
            ok (command.ExecuteScalar() |> string |> Int32.Parse)
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
    


