namespace JobApplicationSpam
module JobApplicationService =
    open WebSharper
    open WebSharper.JQuery
    open System
    open WebSharper.JavaScript
    open Server
    open Types

    [<JavaScript>]
    let loginWithCookieOrAsGuest() =
        let loginAsGuest () =
            JS.Alert("loginasguest!")
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
                    JS.Alert("not logged in!")
                    do! loginAsGuest ()
            else do! loginAsGuest()
        }
    
















