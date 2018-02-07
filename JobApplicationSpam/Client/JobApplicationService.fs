namespace JobApplicationSpam.Client
module JobApplicationService =
    open WebSharper
    open WebSharper.JQuery
    open System
    open WebSharper.JavaScript
    module Server = JobApplicationSpam.Server
    open JobApplicationSpam.Types
    open JavaScriptElements

    [<JavaScript>]
    let loginWithCookieOrAsGuest() =
        let loginAsGuest () =
            async {
                let sessionGuid = Guid.NewGuid().ToString("N")
                Cookies.Set("user", sessionGuid, Cookies.Options(Expires = Date(Date.Now() + 604800)))
                do! Server.loginAsGuest sessionGuid
            }

        async {
            let userCookie = Cookies.Get("user").Value
            if userCookie <> JS.Undefined
            then
                let! loggedIn = Server.loginUserBySessionGuid userCookie
                if not loggedIn
                then
                    do! loginAsGuest ()
            else do! loginAsGuest()
        }
    














