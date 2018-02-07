namespace JobApplicationSpam.Client
module ClientTypes =
    open WebSharper
    open WebSharper.UI.Next
    open JobApplicationSpam.Types

    [<JavaScript>]
    type RefEmployer =
        { company : IRef<string>
          gender : IRef<Gender>
          degree : IRef<string>
          firstName : IRef<string>
          lastName : IRef<string>
          street : IRef<string>
          postcode : IRef<string>
          city : IRef<string>
          email : IRef<string>
          phone : IRef<string>
          mobilePhone : IRef<string>
        }

    [<JavaScript>]
    type RefUserValues =
        { gender : IRef<Gender>
          degree : IRef<string>
          firstName : IRef<string>
          lastName : IRef<string>
          street : IRef<string>
          postcode : IRef<string>
          city : IRef<string>
          phone : IRef<string>
          mobilePhone : IRef<string>
        }
    
    [<JavaScript>]
    type RefDocument =
        { emailSubject : IRef<string>
          emailBody : IRef<string>
          jobName : IRef<string>
          customVariables : IRef<string>
        }
