namespace JobApplicationSpam.Client
module JavaScriptElements =
    open WebSharper.UI.Next.Client
    open WebSharper.UI.Next
    open WebSharper
    open WebSharper.JavaScript

    [<JavaScript>]
    type Els =
        { employerModal : Dom.Element
          exampleModalLabel : Dom.Element
          dateFrom : Dom.Element
          dateTo : Dom.Element
          divJobApplicationContent : Dom.Element
          slctDocumentName : Dom.Element
          btnShowDivNewDocument : Dom.Element
          btnDeleteDocument : Dom.Element
          divVariables : Dom.Element
          txtUserDefinedVariables : Dom.Element
          divAttachments : Dom.Element
          divAttachmentButtons : Dom.Element
          btnAddPage : Dom.Element
          divAddDocument : Dom.Element
          txtNewDocumentName : Dom.Element
          btnAddDocument : Dom.Element
          divDisplayedDocument : Dom.Element
          slctHtmlPageTemplate : Dom.Element
          divUploadedFileDownload : Dom.Element
          chkReplaceVariables : Dom.Element
          btnDownloadFilePage : Dom.Element
          divEmail : Dom.Element
          divChoosePageType : Dom.Element
          rbHtmlPage : Dom.Element
          rbFilePage : Dom.Element
          divCreateHtmlPage : Dom.Element
          txtCreateHtmlPage : Dom.Element
          btnCreateHtmlPage : Dom.Element
          divCreateFilePage : Dom.Element
          fileToUpload : Dom.Element
          hiddenDocumentId : Dom.Element
          hiddenNextPageIndex : Dom.Element
          divSentApplications : Dom.Element
          divEditUserValues : Dom.Element
          divAddEmployer : Dom.Element
          btnReadFromWebsite : Dom.Element
          faReadFromWebsite : Dom.Element
          txtReadFromWebsite : Dom.Element
          btnApplyNowTop : Dom.Element
          faBtnApplyNowTop : Dom.Element
          btnApplyNowBottom : Dom.Element
          faBtnApplyNowBottom : Dom.Element
          btnSetEmployerEmailToUserEmail : Dom.Element
          txtEmployerEmail : Dom.Element
        }
    
    [<JavaScript>]
    let getEls () : Els =
        { employerModal = JS.Document.GetElementById("employerModal")
          exampleModalLabel = JS.Document.GetElementById("exampleModalLabel")
          dateFrom = JS.Document.GetElementById("dateFrom")
          dateTo = JS.Document.GetElementById("dateTo")
          divJobApplicationContent = JS.Document.GetElementById("divJobApplicationContent")
          slctDocumentName = JS.Document.GetElementById("slctDocumentName")
          btnShowDivNewDocument = JS.Document.GetElementById("btnShowDivNewDocument")
          btnDeleteDocument = JS.Document.GetElementById("btnDeleteDocument")
          divVariables = JS.Document.GetElementById("divVariables")
          txtUserDefinedVariables = JS.Document.GetElementById("txtUserDefinedVariables")
          divAttachments = JS.Document.GetElementById("divAttachments")
          divAttachmentButtons = JS.Document.GetElementById("divAttachmentButtons")
          btnAddPage = JS.Document.GetElementById("btnAddPage")
          divAddDocument = JS.Document.GetElementById("divAddDocument")
          txtNewDocumentName = JS.Document.GetElementById("txtNewDocumentName")
          btnAddDocument = JS.Document.GetElementById("btnAddDocument")
          divDisplayedDocument = JS.Document.GetElementById("divDisplayedDocument")
          slctHtmlPageTemplate = JS.Document.GetElementById("slctHtmlPageTemplate")
          divUploadedFileDownload = JS.Document.GetElementById("divUploadedFileDownload")
          chkReplaceVariables = JS.Document.GetElementById("chkReplaceVariables")
          btnDownloadFilePage = JS.Document.GetElementById("btnDownloadFilePage")
          divEmail = JS.Document.GetElementById("divEmail")
          divChoosePageType = JS.Document.GetElementById("divChoosePageType")
          rbHtmlPage = JS.Document.GetElementById("rbHtmlPage")
          rbFilePage = JS.Document.GetElementById("rbFilePage")
          divCreateHtmlPage = JS.Document.GetElementById("divCreateHtmlPage")
          txtCreateHtmlPage = JS.Document.GetElementById("txtCreateHtmlPage")
          btnCreateHtmlPage = JS.Document.GetElementById("btnCreateHtmlPage")
          divCreateFilePage = JS.Document.GetElementById("divCreateFilePage")
          fileToUpload = JS.Document.GetElementById("fileToUpload")
          hiddenDocumentId = JS.Document.GetElementById("hiddenDocumentId")
          hiddenNextPageIndex = JS.Document.GetElementById("hiddenNextPageIndex")
          divSentApplications = JS.Document.GetElementById("divSentApplications")
          divEditUserValues = JS.Document.GetElementById("divEditUserValues")
          divAddEmployer = JS.Document.GetElementById("divAddEmployer")
          btnReadFromWebsite = JS.Document.GetElementById("btnReadFromWebsite")
          faReadFromWebsite = JS.Document.GetElementById("faReadFromWebsite")
          txtReadFromWebsite = JS.Document.GetElementById("txtReadFromWebsite")
          btnApplyNowTop = JS.Document.GetElementById("btnApplyNowTop")
          faBtnApplyNowTop = JS.Document.GetElementById("faBtnApplyNowTop")
          btnApplyNowBottom = JS.Document.GetElementById("btnApplyNowBottom")
          faBtnApplyNowBottom = JS.Document.GetElementById("faBtnApplyNowBottom")
          btnSetEmployerEmailToUserEmail = JS.Document.GetElementById("btnSetEmployerEmailToUserEmail")
          txtEmployerEmail = JS.Document.GetElementById("txtEmployerEmail")
      }
