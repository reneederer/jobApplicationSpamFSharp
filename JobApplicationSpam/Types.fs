namespace JobApplicationSpam
module Types =
    open WebSharper.UI.Next

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


    type Contact = 
        { gender : Gender
          degree : string
          firstName : string
          lastName : string
          street : string
          postcode : string
          city : string
          email : string
          phone : string
          mobilePhone : string
        }

    type Employer =
        { company : string
          street : string
          postcode : string
          city : string
          boss : Contact
        }



