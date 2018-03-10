namespace JobApplicationSpam.Client
module JobApplicationService =
    open WebSharper
    open WebSharper.JQuery
    open System
    open WebSharper.JavaScript
    open JobApplicationSpam

    [<JavaScript>]
    let loginWithCookieOrAsGuest() =
        let loginAsGuest () =
            async {
                let sessionGuid = Guid.NewGuid().ToString("N")
                Cookies.Set("user", sessionGuid, Cookies.Options(Expires = Date(60800)))
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
                else
                    Cookies.Expire("user")
                    Cookies.Set("user", userCookie, Cookies.Options(Expires = Date(2147483647)))
            else do! loginAsGuest()
        }
    
    [<JavaScript>]
    let logout() =
        Cookies.Expire("user")
        loginWithCookieOrAsGuest()
    