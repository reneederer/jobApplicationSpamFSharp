namespace JobApplicationSpam

open Types
open WebSharper
(*

[<AutoOpen>]
module SpamExpression =
    open WebSharper.UI.Next.Html.SvgElements
    open WebSharper.UI.Next.Html
    open FSharp.Data.Sql.Common

    type SpamResult<'a> =
    | Ok of 'a
    | NoResult
    | GuestNotAllowed
    | Bug

    let RunSynchronously(x) = x |> Async.RunSynchronously



    [<JavaScript>]
    type Spam() =
        static member Start(x) = x |> Async.Ignore |> Async.Start
        static member Sleep(t) =
            (fun k ->
                async {
                    do! Async.Sleep t
                } |> Async.Start
                k()) (fun () -> async { return Success () })
        [<JavaScript>]
        static member Default(x, y) =
            match x with
            | Success v -> v
            | _ -> y
        member this.Zero() = async { return Error }
        member this.Bind(r : Result<'a>, f : 'a -> Result<'b>) : Result<'b> =
            match r with
            | Success v -> f v
            | _ -> Error
        member this.Bind(r : Result<'a>, f : 'a -> Async<Result<'b>>) : Async<Result<'b>> =
            async {
                    match r with
                    | Success v -> return! f v
                    | _ -> return Error
            }
        member this.Bind(ar : Async<Result<'a>>, f : 'a -> Async<Result<'b>>) : Async<Result<'b>> =
            async {
                let! r = ar
                return! this.Bind(r, f)
            }
        member this.Combine(ar1 : Async<Result<'a>>, ar2 : Async<Result<'b>>) : Async<Result<'b>> =
            async {
                let! r1 = ar1
                let! r2 = ar2
                match r1, r2 with
                | Success v1, Success v2 ->
                    return Success v2
                | _ -> return Error
            }
        member this.For(xs : seq<'a>, f : 'a -> Async<Result<'b>>) =
           xs |> Seq.map f |> Seq.reduce (fun x1 x2 -> this.Combine(x1, x2))
        member this.While((f : unit-> bool), (ar : Async<Result<unit>>)) : Async<Result<unit>> =
            if f ()
            then
                this.Bind(ar, fun () -> this.While(f, ar))
            else
                this.Zero()
        member this.Delay(f : unit -> 'a) = f()
        member this.ReturnFrom(ar : Async<Result<'a>>) = ar
        member this.ReturnFrom(r : Result<'a>) = async { return r }
        member this.Return(v : 'a) = async { return Success v }

    [<JavaScript>]
    let spam = Spam()
    *)
