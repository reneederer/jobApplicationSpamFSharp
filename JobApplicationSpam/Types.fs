namespace JobApplicationSpam
module Types =
    open WebSharper.UI.Next
    open WebSharper
    open System

    type EmptyTextTagAction =
    | Replace
    | Ignore

    [<JavaScript>]
    let newLine = string (char 13)

    [<JavaScript>]
    type Gender =
    | Male
    | Female
    | Unknown
    with
        override this.ToString() =
            match this with
            | Gender.Male -> "m"
            | Gender.Female -> "f"
            | Gender.Unknown -> "u"
        static member fromString(v) =
            match v with
            | "m" -> Gender.Male
            | "f" -> Gender.Female
            | "u" -> Gender.Unknown
            | x -> failwith ("Failed to convert string to gender: " + x)

    [<JavaScript>]
    type Login =
        { email : string
          password : string
        }


    [<JavaScript>]
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
    

    [<JavaScript>]
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
    

    [<JavaScript>]
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
     

    [<JavaScript>]
    type HtmlPage =
        { name : string
          oTemplateId : option<int>
          pageIndex : int
          map : list<string * string>
        }
    
    [<JavaScript>]
    type FilePage =
        { name : string
          path : string
          pageIndex : int
        }

    [<JavaScript>] 
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
        member this.PageIndex(newIndex) =
            match this with
            | HtmlPage htmlPage -> HtmlPage { htmlPage with pageIndex = newIndex }
            | FilePage filePage -> FilePage { filePage with pageIndex = newIndex }

    [<JavaScript>]
    type DocumentEmail =
        { subject : string
          body : string
        }

    [<JavaScript>]
    type Document =
        { id : int
          name : string
          pages : list<DocumentPage>
          email : DocumentEmail
          jobName : string
          customVariables : string
        }

    [<JavaScript>]
    type HtmlPageTemplate =
        { html : string
          name : string
          id : int
        }

    [<JavaScript>]
    type PageDB =
        { name : string
        ; oTemplateId : option<int>
        }
    


    type SentApplication =
        { jobName : string
          sentDate : string
          email : DocumentEmail
          userValues : UserValues
          userEmail : string
          employer : Employer
          filePages : list<string * int>
          customVariables : string
        }

    [<JavaScript>]
    let emptyUserValues =
        { gender = Gender.Unknown
          degree = ""
          firstName = ""
          lastName = ""
          street = ""
          postcode = ""
          city = ""
          phone = ""
          mobilePhone = ""
        }

    [<JavaScript>]
    let emptyEmployer =
        { company = ""
          gender = Gender.Unknown
          degree = ""
          firstName = ""
          lastName = ""
          street = ""
          postcode = ""
          city = ""
          email = ""
          phone = ""
          mobilePhone = ""
        }


    [<JavaScript>]
    let emptyDocument =
        { id=0
          name=""
          pages=[]
          email=
            { subject = "Bewerbung als $beruf"
              body = String.Format("$anredeZeile{0}{0}anbei sende ich Ihnen meine Bewerbungsunterlagen.{0}Über eine Einladung zu einem Bewerbungsgespräch würde ich mich sehr freuen.{0}{0}Mit freundlichen Grüßen{0}{0}$meinTitel $meinVorname $meinNachname{0}$meineStrasse{0}$meinePlz $meineStadt{0}Telefon: $meineTelefonnr_{0}Mobil: $meineMobilnr", newLine)
            }
          jobName=""
          customVariables = "$datumHeute = $tag + \".\" + $monat + \".\" + $jahr\n\n$anredeZeile =\n\tmatch $chefGeschlecht with\n\t| \"m\" -> \"Sehr geehrter Herr $chefTitel $chefNachname,\"\n\t| \"f\" -> \"Sehr geehrte Frau $chefTitel $chefNachname,\"\n\t| \"u\" -> \"Sehr geehrte Damen und Herren,\"\n\n$chefAnrede =\n\tmatch $chefGeschlecht with\n\t| \"m\" -> \"Herr\"\n\t| \"f\" -> \"Frau\"\n\n$chefAnredeBriefkopf =\n\tmatch $chefGeschlecht with\n\t| \"m\" -> \"Herrn\"\n\t| \"f\" -> \"Frau\"\n\n"

        }   
    


    type DataBinding =
        | TextBinding of IRef<string>
        | GenderBinding of IRef<Gender>
    
    [<JavaScript>]
    let supportedUnoconvFileTypes =
        ["bib"; "doc"; "doc6"; "doc95"; "docbook"; "docx"; "docx7"; "fodt"; "html"; "latex"; "mediawiki"; "odt"; "ooxml"; "ott"; "pdb"; "pdf"; "psw"; "rtf"; "sdw"; "sdw4"; "sdw3"; "stw"; "sxw"; "text"; "txt"; "uot"; "vor"; "vor4"; "vor3"; "wps"; "xhtml"; "emf"; "eps"; "fodg"; "gif"; "html"; "jpg"; "met"; "odd"; "otg"; "pbm"; "pct"; "pdf"; "pgm"; "png"; "ppm"; "ras"; "std"; "svg"; "svm"; "swf"; "sxd"; "sxd3"; "sxd5"; "sxw"; "tiff"; "vor"; "vor3"; "wmf"; "xhtml"; "xpm"; "emf"; "eps"; "fodp"; "gif"; "html"; "jpg"; "met"; "odg"; "odp"; "otp"; "pbm"; "pct"; "pdf"; "pgm"; "png"; "potm"; "pot"; "ppm"; "pptx"; "pps"; "ppt"; "pwp"; "ras"; "sda"; "sdd"; "sdd3"; "sdd4"; "sxd"; "sti"; "svg"; "svm"; "swf"; "sxi"; "tiff"; "uop"; "vor"; "vor3"; "vor4"; "vor5"; "wmf"; "xhtml"; "xpm"; "csv"; "dbf"; "dif"; "fods"; "html"; "ods"; "ooxml"; "ots"; "pdf"; "pxl"; "sdc"; "sdc4"; "sdc3"; "slk"; "stc"; "sxc"; "uos"; "vor3"; "vor4"; "vor"; "xhtml"; "xls"; "xls5"; "xls95"; "xlt"; "xlt5"; "xlt95"; "xlsx"]

    [<JavaScript>]
    let maxUploadSize =
        5000000 // 5 MB

    let convertibleToOdtFormats =
        [ "doc"; "docx"; "fodt"; "ott"; "rtf"; "stw"; "sxw"; "uot" ]





