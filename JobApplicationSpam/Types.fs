namespace JobApplicationSpam
module Types =
    open WebSharper.UI.Next
    open WebSharper.Sitelets
    open log4net
    open log4net.Core
    open WebSharper.Core.ContentTypes.Text

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
    
    type TemplateForJobApplication =
        { emailSubject : string
          emailBody : string
          filePaths : list<string>
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
    type Page =
        { name : string
          templateId : int
          pageIndex : int
          map : Map<string, string>
        }
    
    type File =
        { name : string
          path : string
          pageIndex : int
        }

    [<WebSharper.JavaScript>] 
    type DocumentItem =
    | DocumentPage of Page
    | DocumentFile of File
    with
        member this.Name() =
            match this with
            | DocumentPage page -> page.name
            | DocumentFile file -> file.name
        member this.PageIndex() =
            match this with
            | DocumentPage page -> page.pageIndex
            | DocumentFile file -> file.pageIndex

    //[<WebSharper.JavaScript>]
    type Document =
        { name : string
          items : list<DocumentItem>
        }

    type PageTemplate =
        { html : string
          name : string
          id : int
        }

    type PageDB =
        { name : string
        ; pageTemplateId : int
        }
