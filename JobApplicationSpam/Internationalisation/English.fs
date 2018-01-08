﻿namespace JobApplicationSpam
module English =
    open Types

    let dict =
        [ AddEmployerAndApply, "Add employer and apply"
          EditYourValues, "Edit your values"
          EditEmail, "Edit email"
          EditAttachments, "Edit attachments"
          YourApplicationDocuments, "Your applicationDocuments"
          LoadFromWebsite, "LoadFromWebsite"
          ApplyNow, "Apply now"
          CompanyName, "Company name"
          Street, "Street"
          Postcode, "Postcode"
          City, "City"
          Gender, "Gender"
          Degree, "Degree"
          FirstName, "First name"
          LastName, "Last name"
          Email, "Email"
          Phone, "Phone"
          MobilePhone, "Mobile phone"
          YourValues, "Your values"
          EmailSubject, "Subject"
          EmailBody, "Body"
          YourAttachments, "Your attachments"
          CreateOnline, "Create online"
          UploadFile, "Upload a file"
          PleaseChooseAFile, "Please choose a file"
          AddAttachment, "Add attachment"
          YouMightWantToReplaceSomeWordsInYourFileWithVariables, "You might want to replace some words in your file with variables"
          VariablesWillBeReplacedWithTheRightValuesEveryTimeYouSendYourApplication, "Variables will be replaced with the right values every time you send your application."
          Male, "male"
          Female, "female"
          UnknownGender, "unknown"
          AddDocument, "Add document"
          DocumentName, "Document name: "
          AddHtmlAttachment, "Add Html attachment"
          Employer, "Employer"
          JustDownload, "Just Download"
          DownloadWithReplacedVariables, "Download with replaced variables"
          ReallyDeletePage, """Really delete page "{0}"?"""
          ReallyDeleteDocument, """Really delete document "{0}"?"""
          PleaseConfirmYourEmailAddressEmailSubject, "Please confirm your email address"
          PleaseConfirmYourEmailAddressEmailBody, "Dear user,\n\nplease visit this link to confirm your email address.\nhttp://bewerbungsspam.de/confirmemail?email={0}&guid={1}\nPlease excuse the inconvenience.\n\nYour team from www.bewerbungsspam.de"
          Login, "Login"
          Register, "Register"
          SentApplications, "Sent applications"
          JobName, "Apply as"
          AppliedAs, "Applied as"
          AppliedOnDate, "Applied on"
          TheEmailOfYourEmployerDoesNotLookValid, "The email of your employer does not look valid."
          FieldIsRequired, """Field "{0}" must not be empty."""
          SorryAnErrorOccurred, "Sorry, an error occurred."
          YourApplicationHasNotBeenSent, "\nYour application has not been sent :-("
        ]