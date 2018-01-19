namespace JobApplicationSpam
module CustomVariables =
    open FParsec

    (*
        match $chefGeschlecht with
        | "m" -> "Sehr geehrter Herr $chefTitel $chefNachname,"
        | "f" -> "Sehr geehrte Frau $chefTitel $chefNachname,"
        | "u" -> "Sehr geehrte Damen und Herren,"
    *)

    type VariableName = string
    type VariableValue = string

    type Variable =
    | PureString of string * string

    type Expression =
    | MatchExpression of VariableName * list<Variable>
    | VariableExpression of Variable

    let tryGetValue (expression : Expression) =
        match expression with
        | MatchExpression (variableName, variables) ->
                variables
                |> List.tryPick (fun variable ->
                    match variable with
                    | PureString (k, v) -> if k = variableName then Some v else None
                )
        | VariableExpression variable ->
            match variable with
            | PureString (k, v) ->
                Some v
    


    let expr1 = VariableExpression (PureString ("$meinVorname", "René"))
    let expr =
        MatchExpression ("$chefVorname", [PureString ("$chefName", "Meier")])




    let tokenize str =
        ()


    let parse str =
        let tokens = tokenize str
        ()



    let test p str =
        match run p str with
        | Success(result, _, _)   -> printfn "Success: %A" result
        | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg
   

    
