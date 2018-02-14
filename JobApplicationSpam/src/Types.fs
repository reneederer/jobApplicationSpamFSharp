namespace JobApplicationSpam
module Types =
    open WebSharper.UI.Next
    open WebSharper
    open System
    open FSharp.Configuration
    open System.Configuration
    open System.IO

    type UserId = UserId of int
    type DocumentId = DocumentId of int

    type Settings = AppSettings<"app.config">

    type LoggedInData =
        { userId : UserId
          email : string
          confirmEmailGuid : option<string>
          sessiongGuid : option<string>
        }
    
    type LoginData =
        { userEmail : string
          password : string
        }
    
    type ValidateLoginData =
        { userId : UserId
          userEmail : string
          hashedPassword : string
          salt : string
          confirmEmailGuid : option<string>
        }

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
        { oId : option<DocumentId>
          name : string
          pages : list<DocumentPage>
          email : DocumentEmail
          jobName : string
          customVariables : string
        }
        with
            member this.GetId() =  
                match this.oId with 
                | None -> failwith "Document id was None"
                | Some (DocumentId documentId) -> documentId

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
    

    type DisplaySentApplication =
        { jobName : string
          appliedOn : DateTime
          employer : Employer
          url : string
        }

    type SentUser =
        { values : UserValues
          email : string
          id : UserId
        }

    type SentApplication =
        { jobName : string
          sentDate : string
          email : DocumentEmail
          user : SentUser
          employer : Employer
          filePages : list<string * int>
          customVariables : string
          url : string
        }

    [<JavaScript>]
    let emptyUserValues =
        { gender = Gender.Male
          degree = ""
          firstName = "Max"
          lastName = "Mustermann"
          street = "Musterstraße 3a"
          postcode = "90403"
          city = "Nürnberg"
          phone = "0911 9876543"
          mobilePhone = "0151 1234567"
        }

    [<JavaScript>]
    let emptyEmployer =
        { company = "Beispielfirma"
          gender = Gender.Female
          degree = "Dr."
          firstName = "Martina"
          lastName = "Hase"
          street = "Alleestr. 12"
          postcode = "20095"
          city = "Hamburg"
          email = "martina.hase@beispielfirma.de"
          phone = "040 11111111"
          mobilePhone = "0175 5555555"
        }


    [<JavaScript>]
    let emptyDocument =
        { oId = None
          name="Bewerbungsmappe1"
          pages= []//[FilePage { name = "beispiel_anschreiben.odt"; path="files/anschreiben.odt"; pageIndex = 1; }]
          email=
            { subject = "Bewerbung als $beruf"
              body = String.Format("$anredeZeile{0}{0}anbei sende ich Ihnen meine Bewerbungsunterlagen.{0}Über eine Einladung zu einem Bewerbungsgespräch würde ich mich sehr freuen.{0}{0}Mit freundlichen Grüßen{0}{0}$meinTitel $meinVorname $meinNachname{0}$meineStrasse{0}$meinePlz $meineStadt{0}Telefon: $meineTelefonnr{0}Mobil: $meineMobilnr", newLine)
            }
          jobName=""
          customVariables = "$datumHeute = $tagHeute + \".\" + $monatHeute + \".\" + $jahrHeute\n\n$anredeZeile =\n\tmatch $chefGeschlecht with\n\t| \"m\" -> \"Sehr geehrter Herr $chefTitel $chefNachname,\"\n\t| \"f\" -> \"Sehr geehrte Frau $chefTitel $chefNachname,\"\n\t| \"u\" -> \"Sehr geehrte Damen und Herren,\"\n\n$chefAnrede =\n\tmatch $chefGeschlecht with\n\t| \"m\" -> \"Herr\"\n\t| \"f\" -> \"Frau\"\n\n$chefAnredeBriefkopf =\n\tmatch $chefGeschlecht with\n\t| \"m\" -> \"Herrn\"\n\t| \"f\" -> \"Frau\"\n\n"
        }   
    

    type DataBinding =
        | TextBinding of IRef<string>
        | GenderBinding of IRef<Gender>

    type StreamCompare =
    | OdtStream of Stream
    | OdtFile of string
    | Stream of Stream
    | File of string
    
    [<JavaScript>]
    let supportedUnoconvFileTypes =
        [ "doc"; "docx"; "docx7"; "fodt"; "latex"; "odt"; "ooxml"; "ott"; "pdb"; "pdf"; "psw"; "rtf"; "sdw"; "sdw4"; "sdw3"; "stw"; "sxw"; "text"; "txt"; "uot"; "vor"; "vor4"; "vor3"; "wps"; "xhtml"; "emf"; "eps"; "fodg"; "gif"; "html"; "jpg"; "met"; "odd"; "otg"; "pbm"; "pct"; "pdf"; "pgm"; "png"; "ppm"; "ras"; "std"; "svg"; "svm"; "swf"; "sxd"; "sxd3"; "sxd5"; "sxw"; "tiff"; "vor"; "vor3"; "wmf"; "xhtml"; "xpm"; "emf"; "eps"; "fodp"; "gif"; "html"; "jpg"; "met"; "odg"; "odp"; "otp"; "pbm"; "pct"; "pdf"; "pgm"; "png"; "potm"; "pot"; "ppm"; "pptx"; "pps"; "ppt"; "pwp"; "ras"; "sda"; "sdd"; "sdd3"; "sdd4"; "sxd"; "sti"; "svg"; "svm"; "swf"; "sxi"; "tiff"; "uop"; "vor"; "vor3"; "vor4"; "vor5"; "wmf"; "xhtml"; "xpm"; "csv"; "dbf"; "dif"; "fods"; "html"; "ods"; "ooxml"; "ots"; "pdf"; "pxl"; "sdc"; "sdc4"; "sdc3"; "slk"; "stc"; "sxc"; "uos"; "vor3"; "vor4"; "vor"; "xhtml"; "xls"; "xls5"; "xls95"; "xlt"; "xlt5"; "xlt95"; "xlsx"]

    [<JavaScript>]
    let unoconvImageTypes =
        [ "bmp"; "gif"; "jpg"; "pdf"; "png"; "svg"; "tif"; "tiff" ]

    [<JavaScript>]
    let maxUploadSize =
        5000000 // 5 MB

    let convertibleToOdtFormats =
        [ "doc"; "docx"; "fodt"; "ott"; "rtf"; "stw"; "sxw"; "uot" ]


    let toRootedPath path =
        if System.IO.Path.IsPathRooted path
        then path
        else System.IO.Path.Combine(Settings.DataDirectory, path)

    type JobApplicationSpamException =
    | DBException
    | DBValueNotFound of string
    | DBExpectedOneResult
    | FileException


