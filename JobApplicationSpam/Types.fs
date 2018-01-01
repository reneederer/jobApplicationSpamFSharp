namespace JobApplicationSpam
module Types =
    open WebSharper.UI.Next
    open WebSharper.Sitelets
    open log4net
    open log4net.Core
    open WebSharper.Core.ContentTypes.Text

    [<WebSharper.JavaScript>]
    type Gender =
    | Male
    | Female
    with
        override this.ToString() =
            match this with
            | Gender.Male -> "m"
            | Gender.Female -> "f"
        static member fromString(v) =
            match v with
            | "m" -> Gender.Male
            | "f" -> Gender.Female
            | x -> failwith ("Failed to convert string to gender: " + x)

    type Login =
        { email : string
          password : string
        }


    type UserValues =
        { gender : Gender
          degree : string
          firstName : string
          lastName : string
          street : string
          postcode : string
          city : string
          phone : string
          mobilePhone : string
        }
    

    type Employer =
        { company : string
          street : string
          postcode : string
          city : string
          gender : Gender
          degree : string
          firstName : string
          lastName : string
          email : string
          phone : string
          mobilePhone : string
        }
    

    [<WebSharper.JavaScript>]
    type JobApplicationPageAction =
    | Upload
    | Create
    | UseCreated
    with
        override this.ToString() =
            match this with 
            | Upload -> "Upload"
            | Create -> "Create"
            | UseCreated -> "UseCreated"
     

    //[<WebSharper.JavaScript>]
    type HtmlPage =
        { name : string
          oTemplateId : option<int>
          pageIndex : int
          map : Map<string, string>
        }
    
    type FilePage =
        { name : string
          path : string
          pageIndex : int
        }

    [<WebSharper.JavaScript>] 
    type DocumentPage =
    | HtmlPage of HtmlPage
    | FilePage of FilePage
    with
        member this.Name() =
            match this with
            | HtmlPage htmlPage -> htmlPage.name
            | FilePage filePage -> filePage.name
        member this.PageIndex() =
            match this with
            | HtmlPage htmlPage -> htmlPage.pageIndex
            | FilePage filePage -> filePage.pageIndex

    //[<WebSharper.JavaScript>]
    type Document =
        { name : string
          pages : list<DocumentPage>
        }

    type HtmlPageTemplate =
        { html : string
          name : string
          id : int
        }

    type PageDB =
        { name : string
        ; oTemplateId : option<int>
        }
    
    type Language =
    | English
    | Deutsch

