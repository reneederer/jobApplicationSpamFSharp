namespace JobApplicationSpam
module Translation =
    open WebSharper

    [<JavaScript>]
    type Language =
    | English
    | German
    with
        static member fromString(s:string) =
            match s.ToLower() with
            | "english" -> English
            | "deutsch" -> German
            | _ -> English
        override this.ToString() =
            match this with
            | English -> "english"
            | German -> "deutsch"


    [<JavaScript>]
    let t l w =
        match l with
        | English -> English.t w
        | German -> German.t w

