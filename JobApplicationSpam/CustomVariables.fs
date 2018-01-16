namespace JobApplicationSpam
module CustomVariables =
    open FParsec

    type Token =
    | Variable of string
    | EqualsSign
    | Constant of string
    | Match

    type Expression =
    | MatchExpression of Map<string, string>

    let tokenize str =
        ()


    let parse str =
        let tokens = tokenize str
        ()



