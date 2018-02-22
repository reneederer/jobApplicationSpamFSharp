namespace JobApplicationSpam

module UserInputValidation =
    open WebSharper
    open Chessie.ErrorHandling
    open System.Text.RegularExpressions
    open WebSharper.JavaScript

    [<JavaScript>]
    let testRegex pattern (input : string) : bool =
        let regex = RegExp(pattern)
        regex?test(input)

    [<JavaScript>]
    let testEmail input =
        testRegex
            """^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$"""
            input
    
    [<JavaScript>]
    let atMost n (input : string) = input.Length <= n

    [<JavaScript>]
    let atLeast n (input : string) = input.Length >= n

    [<JavaScript>]
    let notEmpty = atLeast 1
