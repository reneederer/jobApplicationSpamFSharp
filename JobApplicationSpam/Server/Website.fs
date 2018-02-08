namespace JobApplicationSpam

module Website =
    open Types
    open HtmlAgilityPack
    open System.Text.RegularExpressions
    open System.Linq
    open System.Net.Http
    open System.Collections.Generic
    open System
    open System.Net
    open Chessie.ErrorHandling

    type WebResource =
    | Identifier of string
    | Document of HtmlDocument

    let getDoc webResource =
        match webResource with
        | Identifier s -> HtmlWeb().Load(s)
        | Document doc -> doc

    let readJobboerseDesktop (webResource : WebResource) = 
        let doc = getDoc webResource
        let company =
            match doc.GetElementbyId("ruckfragenundbewerbungenan_-2147483648") with
            | null -> ""
            | el -> el.InnerHtml

        let gender = 
            try
                let innerHtml =
                    doc.GetElementbyId("ruckfragenundbewerbungenan_-2147483648")
                      .ParentNode
                      .InnerHtml
                      .Replace("\r\n", " ").Replace("\n", " ")
                let value = Regex.Match(innerHtml, @"<br.*?>\s*(.*?)\s*<span").Groups.[1].Value
                match value with
                | "Herr"
                | "Herrn" -> Gender.Male
                | "Frau" -> Gender.Female
                | _ -> Gender.Unknown
            with
            | e -> Gender.Unknown

        let degree = 
            match doc.GetElementbyId("titel_-2147483648") with
            | null -> ""
            | el -> el.InnerHtml
        let firstName =
            match doc.GetElementbyId("vorname_-2147483648") with
            | null -> ""
            | el -> el.InnerHtml
        let lastName =
            match doc.GetElementbyId("nachname_-2147483648") with
            | null -> ""
            | el -> el.InnerHtml
        let street =
            match doc.GetElementbyId("ruckfragenUndBewerbungenAn.wert['adresse']Strasse_-2147483648") with
            | null -> ""
            | el -> el.InnerHtml
        let postcode =
            match doc.GetElementbyId("ruckfragenUndBewerbungenAn.wert['adresse']Plz_-2147483648") with
            | null -> ""
            | el -> el.InnerHtml
        let city =
            match doc.GetElementbyId("ruckfragenUndBewerbungenAn.wert['adresse']Ort_-2147483648") with
            | null -> ""
            | el -> el.InnerHtml
        let phone, mobilePhone, fax =
            let telecommunication =
                try
                    doc.DocumentNode
                      .Descendants("span")
                      .First(fun x -> x.InnerHtml.StartsWith("Telekommunikation"))
                      .ParentNode
                      .Descendants("p")
                      .First()
                      .InnerHtml.Replace("\r\n", " ").Replace("\n", " ")
                with
                | e -> ""
            ( Regex.Match(telecommunication, @"Telefonnummer:\s*(.*?)\s*<br").Groups.[1].Value
            , Regex.Match(telecommunication, @"Mobilnummer:\s*(.*?)\s*<br").Groups.[1].Value
            , Regex.Match(telecommunication, @"Faxnummer:\s*(.*?)\s*<br").Groups.[1].Value
            )

        let email =
            match doc.GetElementbyId("vermittlung.stellenangeboteverwalten.detailszumstellenangebot.email") with
            | null -> ""
            | el -> el.Attributes.["href"].Value.Substring(7)

        ok
          { company = company |> System.Net.WebUtility.HtmlDecode
            street = street |> System.Net.WebUtility.HtmlDecode
            postcode = postcode |> System.Net.WebUtility.HtmlDecode
            city = city |> System.Net.WebUtility.HtmlDecode
            gender = gender
            degree = degree |> System.Net.WebUtility.HtmlDecode
            firstName = firstName |> System.Net.WebUtility.HtmlDecode
            lastName = lastName |> System.Net.WebUtility.HtmlDecode
            email = email |> System.Net.WebUtility.HtmlDecode
            phone = phone |> System.Net.WebUtility.HtmlDecode
            mobilePhone = mobilePhone |> System.Net.WebUtility.HtmlDecode
          }

    
    let readJobboerseByReferenzNummer (webResource : WebResource) =
        match webResource with
        | Identifier refNr ->
            let url = "https://jobboerse.arbeitsagentur.de/vamJB/schnellsuche.html"
            let cookies = new CookieContainer();
            use handler = new HttpClientHandler();
            handler.CookieContainer <- cookies;
            use httpClient = new HttpClient(handler)
            httpClient.BaseAddress <- new Uri(url)
            let newUrl, cookies =
                async {
                    let! response = httpClient.GetAsync(url) |> Async.AwaitTask
                    response.EnsureSuccessStatusCode() |> ignore
                    let responseUri = response.RequestMessage.RequestUri.ToString()
                    let responseCookies = cookies.GetCookies(new Uri(responseUri)).Cast<Cookie>();
                    return responseUri,responseCookies
                } |> Async.RunSynchronously
            let doc = HtmlWeb().Load(newUrl)
            let csrfTokenValue = doc.DocumentNode.Descendants("input").Where(fun x -> x.GetAttributeValue("name", "") = "CSRFToken").First().GetAttributeValue("value", "")
            let data =
                [ KeyValuePair("sieSuchen.wert.wert", "leer")
                  KeyValuePair("suchbegriff.wert", refNr)
                  KeyValuePair("arbeitsort.lokation", "")
                  KeyValuePair("_eventId_suchen", "Suchen")
                  KeyValuePair("CSRFToken", csrfTokenValue)
                ]

            let content = new FormUrlEncodedContent(data)
            handler.Dispose()
            httpClient.Dispose()
            use handler = new HttpClientHandler()

            for cookie in cookies do
                handler.CookieContainer.Add(cookie)
            use httpClient = new HttpClient(handler, BaseAddress = new Uri(newUrl))
            httpClient.DefaultRequestHeaders.ExpectContinue <- Option.toNullable (Some false)
            httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache")
            httpClient.DefaultRequestHeaders.Add("Connection", "Keep-Alive")
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2")
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8")
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br")
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "de-DE,de;q=0.9,en-US;q=0.8,en;q=0.7,es;q=0.6")
            httpClient.DefaultRequestHeaders.Add("Referer", "https://jobboerse.arbeitsagentur.de/vamJB/startseite.html?aa=1&m=1&kgr=as&vorschlagsfunktionaktiv=true")
            let url =
                async {
                    let! response = httpClient.PostAsync(url, content) |> Async.AwaitTask
                    response.EnsureSuccessStatusCode() |> ignore
                    return response.RequestMessage.RequestUri.ToString()
                } |> Async.RunSynchronously
            readJobboerseDesktop (Identifier url)
        | _ -> ok emptyEmployer



    let readMeineStadt (webResource : WebResource) = 
        let doc = getDoc webResource
        try
            let node =
                doc
                    .GetElementbyId("ms-job-detail-bewerbung")

            let text =
                node
                    .Descendants("p")
                    .ElementAt(2)
                    .InnerHtml
            let pattern = @"\s*(.*?)\s*<br\/?>\s*(.*?)\s*<span>(\d{5})</span>\s*<span>\s*(?:&nbsp;)*\s*(.*?)\s*</span>"
            let m = Regex.Match(text, pattern, RegexOptions.Singleline)
            let company = m.Groups.[1].Value
            let street = m.Groups.[2].Value
            let postcode = m.Groups.[3].Value
            let city = m.Groups.[4].Value

            let phone, email =
                let mutable phone1 = ""
                let mutable email1 = ""
                try
                let divs = node.Element("div").Elements("div").ElementAt(1).Elements("div")
                phone1 <- divs.First().InnerHtml.Trim()
                email1 <- divs.ElementAt(1).FirstChild.Attributes.["href"].Value.Replace("<!-- meinestadt -->", "").Replace("[AT]", "@").Replace("[PUNKT]", ".").Substring(8)
                phone1, email1
                with
                | e ->
                    phone1, email1

            ok
              { company = company |> System.Net.WebUtility.HtmlDecode
                street = street |> System.Net.WebUtility.HtmlDecode
                postcode = postcode |> System.Net.WebUtility.HtmlDecode
                city = city |> System.Net.WebUtility.HtmlDecode
                gender = Gender.Unknown
                degree = ""
                firstName = ""
                lastName = ""
                email = email |> System.Net.WebUtility.HtmlDecode
                phone = phone |> System.Net.WebUtility.HtmlDecode
                mobilePhone = ""
              }
        with
        | e ->
            ok emptyEmployer


    let websiteMap : Map<string, (WebResource -> Result<Employer, string>)> =
        [ @"^.*jobboerse\.arbeitsagentur\.de.*$", readJobboerseDesktop
          @"^.*jobboerse\.mobil\.arbeitsagentur\.de.*$", (fun _ -> fail "Bitte die Referenznummer des Jobangebots einfügen.\n\nVon der Mobilseite der Arbeitsagentur kann nicht gelesen werden.")
          @"^\d+[\d\-\w]+$", readJobboerseByReferenzNummer
          @"^.*meinestadt.*$", readMeineStadt
        ]
        |> Map.ofList

    let read (str : string) =
        try
            websiteMap
            |> Map.tryPick (fun k v -> if Regex.IsMatch(str, k) then Some v else None)
            |> Option.defaultValue (fun _ -> ok emptyEmployer)
            <| Identifier str
        with
        | e -> ok emptyEmployer
        
            
        
