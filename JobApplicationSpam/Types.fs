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

    type UserValues1 =
        { //gender : Var<Gender>
          degree : Var<string>
          firstName : Var<string>
          lastName : Var<string>
          street : Var<string>
          postcode : Var<string>
          city : Var<string>
          phone : Var<string>
          mobilePhone : Var<string>
        }

    type UserValues =
        { //gender : Var<Gender>
          degree : string
          (*
          firstName : string
          lastName : string
          street : string
          postcode : string
          city : string
          phone : string
          mobilePhone : string
          *)
        }


    type EmployerContact = Person

    type Employer =
        { company : Var<string>
          contact : EmployerContact
        }



