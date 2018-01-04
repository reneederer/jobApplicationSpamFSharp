namespace JobApplicationSpam

module Website =
    open Types
    open HtmlAgilityPack
    open System.Text.RegularExpressions
    open System.Linq

    let readJobboerse (doc : HtmlDocument) = 
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
                | _ -> failwith value
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




    let websiteMap : Map<string, (HtmlDocument -> Employer)> =
        ["^.*jobboerse.*$", readJobboerse]
        |> Map.ofList

    let read url =
        let matchingWebsiteFuns : list<HtmlDocument -> Employer> =
            websiteMap
            |> Map.filter (fun pattern value -> Regex.IsMatch(url, pattern))
            |> Map.toList
            |> List.map (fun (k : string, (v : HtmlDocument -> Employer)) -> v)
        
        try
            let doc = HtmlWeb().Load(url)
            [ for currentWebsiteFun in matchingWebsiteFuns do
                yield
                    //HtmlWeb().Load(url)
                    //|> currentWebsiteFun
                    doc |> currentWebsiteFun
            ]
            |> List.head
        with
        | e ->

        { company = ""
          street = ""
          postcode = ""
          city = ""
          gender = Gender.Unknown
          degree = ""
          firstName = ""
          lastName = ""
          email = ""
          phone = ""
          mobilePhone = ""
        }









