namespace JobApplicationSpam
module Types =
    open WebSharper.UI.Next
    open WebSharper.Sitelets
    open log4net
    open log4net.Core

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
    type JobApplicationContent =
    | Upload
    | Create
    | Ignore
    with
        override this.ToString() =
            match this with 
            | Upload -> "Upload"
            | Create -> "Create"
            | Ignore -> "Ignore"
     

    //[<WebSharper.JavaScript>]
    type HtmlJobApplicationPage =
        { name : string
          jobApplicationPageTemplateId : int
          map : Map<string, string>
        }

    //[<WebSharper.JavaScript>]
    type HtmlJobApplication =
        { name : string
          pages : list<HtmlJobApplicationPage>
        }


