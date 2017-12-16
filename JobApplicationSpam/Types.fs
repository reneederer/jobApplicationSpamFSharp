namespace JobApplicationSpam
module Types =
    open WebSharper.UI.Next
    open WebSharper.Sitelets


    type Gender =
    | Male
    | Female

    type Login =
        { email : Var<string>
          password : Var<string>
        }

    type Address =
        { street : Var<string>
        ; postcode : Var<string>
        ; city : Var<string>
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
          odtPath : string
          pdfPaths : list<string>
        }



