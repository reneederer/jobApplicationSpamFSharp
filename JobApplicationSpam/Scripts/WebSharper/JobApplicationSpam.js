(function()
{
 "use strict";
 var Global,JobApplicationSpam,Phrases,Word,English,German,Translation,Language,Types,Gender,UserValues,Employer,JobApplicationPageAction,HtmlPage,FilePage,DocumentPage,DocumentEmail,Document,HtmlPageTemplate,PageDB,SC$1,Client,ClientTypes,RefEmployer,RefUserValues,RefDocument,JavaScriptElements,Els,ClientHelpers,JobApplicationService,Employer$1,UserValues$1,Email,Variables,Client$1,IntelliFactory,Runtime,WebSharper,Operators,String,List,Arrays,System,Guid,UI,Next,Doc,AttrProxy,AttrModule,Concurrency,Cookies,Date,Remoting,AjaxRemotingProvider,Math,ListModel,Var,DateUtil,Utils,Collections,Map,Seq,Strings,Unchecked,Enumerator;
 Global=window;
 JobApplicationSpam=Global.JobApplicationSpam=Global.JobApplicationSpam||{};
 Phrases=JobApplicationSpam.Phrases=JobApplicationSpam.Phrases||{};
 Word=Phrases.Word=Phrases.Word||{};
 English=JobApplicationSpam.English=JobApplicationSpam.English||{};
 German=JobApplicationSpam.German=JobApplicationSpam.German||{};
 Translation=JobApplicationSpam.Translation=JobApplicationSpam.Translation||{};
 Language=Translation.Language=Translation.Language||{};
 Types=JobApplicationSpam.Types=JobApplicationSpam.Types||{};
 Gender=Types.Gender=Types.Gender||{};
 UserValues=Types.UserValues=Types.UserValues||{};
 Employer=Types.Employer=Types.Employer||{};
 JobApplicationPageAction=Types.JobApplicationPageAction=Types.JobApplicationPageAction||{};
 HtmlPage=Types.HtmlPage=Types.HtmlPage||{};
 FilePage=Types.FilePage=Types.FilePage||{};
 DocumentPage=Types.DocumentPage=Types.DocumentPage||{};
 DocumentEmail=Types.DocumentEmail=Types.DocumentEmail||{};
 Document=Types.Document=Types.Document||{};
 HtmlPageTemplate=Types.HtmlPageTemplate=Types.HtmlPageTemplate||{};
 PageDB=Types.PageDB=Types.PageDB||{};
 SC$1=Global.StartupCode$JobApplicationSpam$Types=Global.StartupCode$JobApplicationSpam$Types||{};
 Client=JobApplicationSpam.Client=JobApplicationSpam.Client||{};
 ClientTypes=Client.ClientTypes=Client.ClientTypes||{};
 RefEmployer=ClientTypes.RefEmployer=ClientTypes.RefEmployer||{};
 RefUserValues=ClientTypes.RefUserValues=ClientTypes.RefUserValues||{};
 RefDocument=ClientTypes.RefDocument=ClientTypes.RefDocument||{};
 JavaScriptElements=Client.JavaScriptElements=Client.JavaScriptElements||{};
 Els=JavaScriptElements.Els=JavaScriptElements.Els||{};
 ClientHelpers=Client.ClientHelpers=Client.ClientHelpers||{};
 JobApplicationService=Client.JobApplicationService=Client.JobApplicationService||{};
 Employer$1=Client.Employer=Client.Employer||{};
 UserValues$1=Client.UserValues=Client.UserValues||{};
 Email=Client.Email=Client.Email||{};
 Variables=Client.Variables=Client.Variables||{};
 Client$1=Client.Client=Client.Client||{};
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 WebSharper=Global.WebSharper;
 Operators=WebSharper&&WebSharper.Operators;
 String=Global.String;
 List=WebSharper&&WebSharper.List;
 Arrays=WebSharper&&WebSharper.Arrays;
 System=Global.System;
 Guid=System&&System.Guid;
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 Doc=Next&&Next.Doc;
 AttrProxy=Next&&Next.AttrProxy;
 AttrModule=Next&&Next.AttrModule;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Cookies=Global.Cookies;
 Date=Global.Date;
 Remoting=WebSharper&&WebSharper.Remoting;
 AjaxRemotingProvider=Remoting&&Remoting.AjaxRemotingProvider;
 Math=Global.Math;
 ListModel=Next&&Next.ListModel;
 Var=Next&&Next.Var;
 DateUtil=WebSharper&&WebSharper.DateUtil;
 Utils=WebSharper&&WebSharper.Utils;
 Collections=WebSharper&&WebSharper.Collections;
 Map=Collections&&Collections.Map;
 Seq=WebSharper&&WebSharper.Seq;
 Strings=WebSharper&&WebSharper.Strings;
 Unchecked=WebSharper&&WebSharper.Unchecked;
 Enumerator=WebSharper&&WebSharper.Enumerator;
 Word.NewPassword={
  $:55
 };
 Word.ChangePassword={
  $:54
 };
 Word.ReplaceVariables={
  $:53
 };
 Word.UploadLimit={
  $:52
 };
 Word.FileIsTooBig={
  $:51
 };
 Word.YourApplicationHasNotBeenSent={
  $:50
 };
 Word.SorryAnErrorOccurred={
  $:49
 };
 Word.FieldIsRequired={
  $:48
 };
 Word.TheEmailOfYourEmployerDoesNotLookValid={
  $:47
 };
 Word.AppliedOnDate={
  $:46
 };
 Word.AppliedAs={
  $:45
 };
 Word.JobName={
  $:44
 };
 Word.SentApplications={
  $:43
 };
 Word.Register={
  $:42
 };
 Word.Login={
  $:41
 };
 Word.PleaseConfirmYourEmailAddressEmailBody={
  $:40
 };
 Word.PleaseConfirmYourEmailAddressEmailSubject={
  $:39
 };
 Word.WeHaveSentYouAnEmail={
  $:38
 };
 Word.ReallyDeletePage={
  $:37
 };
 Word.ReallyDeleteDocument={
  $:36
 };
 Word.Download={
  $:35
 };
 Word.AddHtmlAttachment={
  $:34
 };
 Word.DocumentName={
  $:33
 };
 Word.AddDocument={
  $:32
 };
 Word.UnknownGender={
  $:31
 };
 Word.Female={
  $:30
 };
 Word.Male={
  $:29
 };
 Word.Employer={
  $:28
 };
 Word.VariablesWillBeReplacedWithTheRightValuesEveryTimeYouSendYourApplication={
  $:27
 };
 Word.YouMightWantToReplaceSomeWordsInYourFileWithVariables={
  $:26
 };
 Word.AddAttachment={
  $:25
 };
 Word.PleaseChooseAFile={
  $:24
 };
 Word.UploadFile={
  $:23
 };
 Word.CreateOnline={
  $:22
 };
 Word.YourAttachments={
  $:21
 };
 Word.EmailBody={
  $:20
 };
 Word.EmailSubject={
  $:19
 };
 Word.YourValues={
  $:18
 };
 Word.MobilePhone={
  $:17
 };
 Word.Phone={
  $:16
 };
 Word.Email={
  $:15
 };
 Word.LastName={
  $:14
 };
 Word.FirstName={
  $:13
 };
 Word.Degree={
  $:12
 };
 Word.Gender={
  $:11
 };
 Word.City={
  $:10
 };
 Word.Postcode={
  $:9
 };
 Word.Street={
  $:8
 };
 Word.CompanyName={
  $:7
 };
 Word.ApplyNow={
  $:6
 };
 Word.LoadFromWebsite={
  $:5
 };
 Word.YourApplicationDocuments={
  $:4
 };
 Word.EditAttachments={
  $:3
 };
 Word.EditEmail={
  $:2
 };
 Word.EditYourValues={
  $:1
 };
 Word.AddEmployerAndApply={
  $:0
 };
 English.t=function(x)
 {
  return x.$==1?"Edit your values":x.$==2?"Edit email":x.$==3?"Edit attachments":x.$==4?"Your application documents":x.$==5?"LoadFromWebsite":x.$==6?"Apply now":x.$==7?"Company name":x.$==8?"Street":x.$==9?"Postcode":x.$==10?"City":x.$==11?"Gender":x.$==12?"Degree":x.$==13?"First name":x.$==14?"Last name":x.$==15?"Email":x.$==16?"Phone":x.$==17?"Mobile phone":x.$==18?"Your values":x.$==19?"Subject":x.$==20?"Body":x.$==21?"Your attachments":x.$==22?"Create online":x.$==23?"Upload a file":x.$==24?"Please choose a file":x.$==25?"Add attachment":x.$==26?"You might want to replace some words in your file with variables":x.$==27?"Variables will be replaced with the right values every time you send your application.":x.$==29?"male":x.$==30?"female":x.$==31?"unknown":x.$==32?"Add document":x.$==33?"Document name: ":x.$==34?"Add Html attachment":x.$==28?"Employer":x.$==35?"Download":x.$==37?"Really delete page \"{0}\"?":x.$==36?"Really delete document \"{0}\"?":x.$==38?"We have sent you an email.":x.$==39?"Please confirm your email address":x.$==40?"Dear user,\n\nplease visit this link to confirm your email address.\nhttp://bewerbungsspam.de/confirmemail?email={0}&guid={1}\nPlease excuse the inconvenience.\n\nYour team from www.bewerbungsspam.de":x.$==41?"Login":x.$==42?"Register":x.$==43?"Sent applications":x.$==44?"Apply as":x.$==45?"Applied as":x.$==46?"Applied on":x.$==47?"The email of your employer does not look valid.":x.$==48?"Field \"{0}\" must not be empty.":x.$==49?"Sorry, an error occurred.":x.$==50?"\nYour application has not been sent :-(":x.$==51?"The chosen file is too big.":x.$==52?"The upload limit is {0} MB.":x.$==53?"Replace variables":x.$==54?"Change password":x.$==55?"New Password":"Add employer and apply";
 };
 German.t=function(x)
 {
  return x.$==1?"Deine Daten":x.$==2?"Email":x.$==3?"Anhänge":x.$==4?"Deine Bewerbungsmappen":x.$==5?"Werte holen":x.$==6?"Jetzt bewerben":x.$==7?"Firmenname":x.$==8?"Straße":x.$==9?"Postleitzahl":x.$==10?"Stadt":x.$==11?"Geschlecht":x.$==12?"Titel":x.$==13?"Vorname":x.$==14?"Nachname":x.$==15?"Email":x.$==16?"Telefonnummer":x.$==17?"Mobilnummer":x.$==18?"Deine Daten":x.$==19?"Betreff":x.$==20?"Text":x.$==21?"Anhänge":x.$==22?"Online erstellen":x.$==23?"Datei hochladen":x.$==24?"Bitte eine Datei aussuchen":x.$==25?"Anhang hinzufügen":x.$==26?"Du kannst Worte oder Phrasen in deiner Datei durch Variablen ersetzen.":x.$==27?"Jedesmal, wenn du eine Bewerbung versendest, werden die Variablen automatisch durch die richtigen Werte ersetzt.":x.$==29?"männlich":x.$==30?"weiblich":x.$==31?"unbekannt":x.$==32?"Bewerbungsmappe hinzufügen":x.$==33?"Name der Bewerbungsmappe":x.$==34?"Html Anhang hinzufügen":x.$==28?"Arbeitgeber":x.$==35?"downloaden":x.$==36?"Document \"{0}\" wirklich löschen?":x.$==37?"Seite \"{0}\" wirklich löschen?":x.$==38?"Wir haben dir eine Email geschickt.":x.$==39?"Bitte bestätige deine Email-Adresse":x.$==40?"Lieber Benutzer,\n\ndu hast zur Zeit nur einen einwöchigen Gast-Zugang. Bitte besuche den folgenden Link, um dauerhaft gratis Vollzugang zu erhalten.\nhttps://bewerbungsspam.de/confirmemail?email={0}&guid={1}\n\nDein Team von www.bewerbungsspam.de":x.$==41?"Einloggen":x.$==42?"Registrieren":x.$==43?"Versandte Bewerbungen":x.$==44?"Bewerben als":x.$==45?"Beworben als":x.$==46?"Beworben am":x.$==47?"Die Email-Adresse des Arbeitgeber scheint fehlerhaft zu sein.":x.$==48?"Feld \"{0}\" darf nicht leer sein.":x.$==49?"Entschuldigung, es ist ein Fehler aufgetreten.":x.$==50?"Deine Bewerbung konnte nicht versendet werden :-(":x.$==51?"Die ausgewählte Datei ist zu groß.":x.$==52?"Die maximale Dateigröße beträgt {0} MB.":x.$==53?"Variablen ersetzen":x.$==54?"Passwort ändern":x.$==55?"Neues Passwort":"Bewerben";
 };
 Language=Translation.Language=Runtime.Class({
  toString:function()
  {
   return this.$==1?"deutsch":"english";
  }
 },null,Language);
 Language.German=new Language({
  $:1
 });
 Language.English=new Language({
  $:0
 });
 Language.fromString=function(s)
 {
  var m;
  m=s.toLowerCase();
  return m==="english"?Language.English:m==="deutsch"?Language.German:Language.English;
 };
 Translation.t=function(l,w)
 {
  return l.$==1?German.t(w):English.t(w);
 };
 Gender=Types.Gender=Runtime.Class({
  toString:function()
  {
   return this.$==1?"f":this.$==2?"u":"m";
  }
 },null,Gender);
 Gender.Unknown=new Gender({
  $:2
 });
 Gender.Female=new Gender({
  $:1
 });
 Gender.Male=new Gender({
  $:0
 });
 Gender.fromString=function(v)
 {
  return v==="m"?Gender.Male:v==="f"?Gender.Female:v==="u"?Gender.Unknown:Operators.FailWith("Failed to convert string to gender: "+v);
 };
 UserValues.New=function(gender,degree,firstName,lastName,street,postcode,city,phone,mobilePhone)
 {
  return{
   gender:gender,
   degree:degree,
   firstName:firstName,
   lastName:lastName,
   street:street,
   postcode:postcode,
   city:city,
   phone:phone,
   mobilePhone:mobilePhone
  };
 };
 Employer.New=function(company,street,postcode,city,gender,degree,firstName,lastName,email,phone,mobilePhone)
 {
  return{
   company:company,
   street:street,
   postcode:postcode,
   city:city,
   gender:gender,
   degree:degree,
   firstName:firstName,
   lastName:lastName,
   email:email,
   phone:phone,
   mobilePhone:mobilePhone
  };
 };
 JobApplicationPageAction=Types.JobApplicationPageAction=Runtime.Class({
  toString:function()
  {
   return this.$==1?"Create":this.$==2?"UseCreated":"Upload";
  }
 },null,JobApplicationPageAction);
 JobApplicationPageAction.UseCreated=new JobApplicationPageAction({
  $:2
 });
 JobApplicationPageAction.Create=new JobApplicationPageAction({
  $:1
 });
 JobApplicationPageAction.Upload=new JobApplicationPageAction({
  $:0
 });
 HtmlPage.New=function(name,oTemplateId,pageIndex,map)
 {
  return{
   name:name,
   oTemplateId:oTemplateId,
   pageIndex:pageIndex,
   map:map
  };
 };
 FilePage.New=function(name,path,pageIndex)
 {
  return{
   name:name,
   path:path,
   pageIndex:pageIndex
  };
 };
 DocumentPage=Types.DocumentPage=Runtime.Class({
  PageIndex:function(newIndex)
  {
   var filePage,htmlPage;
   return this.$==1?(filePage=this.$0,new DocumentPage({
    $:1,
    $0:FilePage.New(filePage.name,filePage.path,newIndex)
   })):(htmlPage=this.$0,new DocumentPage({
    $:0,
    $0:HtmlPage.New(htmlPage.name,htmlPage.oTemplateId,newIndex,htmlPage.map)
   }));
  },
  PageIndex$1:function()
  {
   return this.$==1?this.$0.pageIndex:this.$0.pageIndex;
  },
  Name:function()
  {
   return this.$==1?this.$0.name:this.$0.name;
  }
 },null,DocumentPage);
 DocumentEmail.New=function(subject,body)
 {
  return{
   subject:subject,
   body:body
  };
 };
 Document=Types.Document=Runtime.Class({
  GetId:function()
  {
   var m;
   m=this.oId;
   return m!=null&&m.$==1?m.$0.$0:Operators.FailWith("Document id was None");
  }
 },null,Document);
 Document.New=function(oId,name,pages,email,jobName,customVariables)
 {
  return new Document({
   oId:oId,
   name:name,
   pages:pages,
   email:email,
   jobName:jobName,
   customVariables:customVariables
  });
 };
 HtmlPageTemplate.New=function(html,name,id)
 {
  return{
   html:html,
   name:name,
   id:id
  };
 };
 PageDB.New=function(name,oTemplateId)
 {
  return{
   name:name,
   oTemplateId:oTemplateId
  };
 };
 Types.maxUploadSize=function()
 {
  SC$1.$cctor();
  return SC$1.maxUploadSize;
 };
 Types.unoconvImageTypes=function()
 {
  SC$1.$cctor();
  return SC$1.unoconvImageTypes;
 };
 Types.supportedUnoconvFileTypes=function()
 {
  SC$1.$cctor();
  return SC$1.supportedUnoconvFileTypes;
 };
 Types.emptyDocument=function()
 {
  SC$1.$cctor();
  return SC$1.emptyDocument;
 };
 Types.emptyEmployer=function()
 {
  SC$1.$cctor();
  return SC$1.emptyEmployer;
 };
 Types.emptyUserValues=function()
 {
  SC$1.$cctor();
  return SC$1.emptyUserValues;
 };
 Types.newLine=function()
 {
  SC$1.$cctor();
  return SC$1.newLine;
 };
 SC$1.$cctor=function()
 {
  var $1;
  SC$1.$cctor=Global.ignore;
  SC$1.newLine=String.fromCharCode(13);
  SC$1.emptyUserValues=UserValues.New(Gender.Male,"","Max","Mustermann","Musterstraße 3a","90403","Nürnberg","0911 9876543","0151 1234567");
  SC$1.emptyEmployer=Employer.New("Beispielfirma","Alleestr. 12","20095","Hamburg",Gender.Female,"Dr.","Martina","Hase","martina.hase@beispielfirma.de","040 11111111","0175 5555555");
  SC$1.emptyDocument=Document.New(null,"Bewerbungsmappe1",List.T.Empty,DocumentEmail.New("Bewerbung als $beruf",($1=[Types.newLine()],"$anredeZeile"+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0)))+(""+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+("anbei sende ich Ihnen meine Bewerbungsunterlagen."+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+("Über eine Einladung zu einem Bewerbungsgespräch würde ich mich sehr freuen."+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+(""+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+("Mit freundlichen Grüßen"+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+(""+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+("$meinTitel $meinVorname $meinNachname"+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+("$meineStrasse"+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+("$meinePlz $meineStadt"+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+("Telefon: $meineTelefonnr"+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+"Mobil: $meineMobilnr")),"","$datumHeute = $tagHeute + \".\" + $monatHeute + \".\" + $jahrHeute\n\n$anredeZeile =\n\u0009match $chefGeschlecht with\n\u0009| \"m\" -> \"Sehr geehrter Herr $chefTitel $chefNachname,\"\n\u0009| \"f\" -> \"Sehr geehrte Frau $chefTitel $chefNachname,\"\n\u0009| \"u\" -> \"Sehr geehrte Damen und Herren,\"\n\n$chefAnrede =\n\u0009match $chefGeschlecht with\n\u0009| \"m\" -> \"Herr\"\n\u0009| \"f\" -> \"Frau\"\n\n$chefAnredeBriefkopf =\n\u0009match $chefGeschlecht with\n\u0009| \"m\" -> \"Herrn\"\n\u0009| \"f\" -> \"Frau\"\n\n");
  SC$1.supportedUnoconvFileTypes=List.ofArray(["doc","docx","docx7","fodt","latex","odt","ooxml","ott","pdb","pdf","psw","rtf","sdw","sdw4","sdw3","stw","sxw","text","txt","uot","vor","vor4","vor3","wps","xhtml","emf","eps","fodg","gif","html","jpg","met","odd","otg","pbm","pct","pdf","pgm","png","ppm","ras","std","svg","svm","swf","sxd","sxd3","sxd5","sxw","tiff","vor","vor3","wmf","xhtml","xpm","emf","eps","fodp","gif","html","jpg","met","odg","odp","otp","pbm","pct","pdf","pgm","png","potm","pot","ppm","pptx","pps","ppt","pwp","ras","sda","sdd","sdd3","sdd4","sxd","sti","svg","svm","swf","sxi","tiff","uop","vor","vor3","vor4","vor5","wmf","xhtml","xpm","csv","dbf","dif","fods","html","ods","ooxml","ots","pdf","pxl","sdc","sdc4","sdc3","slk","stc","sxc","uos","vor3","vor4","vor","xhtml","xls","xls5","xls95","xlt","xlt5","xlt95","xlsx"]);
  SC$1.unoconvImageTypes=List.ofArray(["bmp","gif","jpg","pdf","png","svg","tif","tiff"]);
  SC$1.maxUploadSize=5000000;
 };
 RefEmployer.New=function(company,gender,degree,firstName,lastName,street,postcode,city,email,phone,mobilePhone)
 {
  return{
   company:company,
   gender:gender,
   degree:degree,
   firstName:firstName,
   lastName:lastName,
   street:street,
   postcode:postcode,
   city:city,
   email:email,
   phone:phone,
   mobilePhone:mobilePhone
  };
 };
 RefUserValues.New=function(gender,degree,firstName,lastName,street,postcode,city,phone,mobilePhone)
 {
  return{
   gender:gender,
   degree:degree,
   firstName:firstName,
   lastName:lastName,
   street:street,
   postcode:postcode,
   city:city,
   phone:phone,
   mobilePhone:mobilePhone
  };
 };
 RefDocument.New=function(emailSubject,emailBody,jobName,customVariables)
 {
  return{
   emailSubject:emailSubject,
   emailBody:emailBody,
   jobName:jobName,
   customVariables:customVariables
  };
 };
 Els.New=function(employerModal,exampleModalLabel,dateFrom,dateTo,divJobApplicationContent,slctDocumentName,btnShowDivNewDocument,btnDeleteDocument,divVariables,txtUserDefinedVariables,divAttachments,divAttachmentButtons,btnAddPage,divAddDocument,txtNewDocumentName,btnAddDocument,divDisplayedDocument,slctHtmlPageTemplate,divUploadedFileDownload,chkReplaceVariables,btnDownloadFilePage,divEmail,divChoosePageType,rbHtmlPage,rbFilePage,divCreateHtmlPage,txtCreateHtmlPage,btnCreateHtmlPage,divCreateFilePage,fileToUpload,hiddenDocumentId,hiddenNextPageIndex,divSentApplications,divEditUserValues,divAddEmployer,btnReadFromWebsite,faReadFromWebsite,txtReadFromWebsite,btnApplyNowTop,faBtnApplyNowTop,btnApplyNowBottom,faBtnApplyNowBottom,btnSetEmployerEmailToUserEmail,txtEmployerEmail)
 {
  return{
   employerModal:employerModal,
   exampleModalLabel:exampleModalLabel,
   dateFrom:dateFrom,
   dateTo:dateTo,
   divJobApplicationContent:divJobApplicationContent,
   slctDocumentName:slctDocumentName,
   btnShowDivNewDocument:btnShowDivNewDocument,
   btnDeleteDocument:btnDeleteDocument,
   divVariables:divVariables,
   txtUserDefinedVariables:txtUserDefinedVariables,
   divAttachments:divAttachments,
   divAttachmentButtons:divAttachmentButtons,
   btnAddPage:btnAddPage,
   divAddDocument:divAddDocument,
   txtNewDocumentName:txtNewDocumentName,
   btnAddDocument:btnAddDocument,
   divDisplayedDocument:divDisplayedDocument,
   slctHtmlPageTemplate:slctHtmlPageTemplate,
   divUploadedFileDownload:divUploadedFileDownload,
   chkReplaceVariables:chkReplaceVariables,
   btnDownloadFilePage:btnDownloadFilePage,
   divEmail:divEmail,
   divChoosePageType:divChoosePageType,
   rbHtmlPage:rbHtmlPage,
   rbFilePage:rbFilePage,
   divCreateHtmlPage:divCreateHtmlPage,
   txtCreateHtmlPage:txtCreateHtmlPage,
   btnCreateHtmlPage:btnCreateHtmlPage,
   divCreateFilePage:divCreateFilePage,
   fileToUpload:fileToUpload,
   hiddenDocumentId:hiddenDocumentId,
   hiddenNextPageIndex:hiddenNextPageIndex,
   divSentApplications:divSentApplications,
   divEditUserValues:divEditUserValues,
   divAddEmployer:divAddEmployer,
   btnReadFromWebsite:btnReadFromWebsite,
   faReadFromWebsite:faReadFromWebsite,
   txtReadFromWebsite:txtReadFromWebsite,
   btnApplyNowTop:btnApplyNowTop,
   faBtnApplyNowTop:faBtnApplyNowTop,
   btnApplyNowBottom:btnApplyNowBottom,
   faBtnApplyNowBottom:faBtnApplyNowBottom,
   btnSetEmployerEmailToUserEmail:btnSetEmployerEmailToUserEmail,
   txtEmployerEmail:txtEmployerEmail
  };
 };
 JavaScriptElements.getEls=function()
 {
  return Els.New(Global.document.getElementById("employerModal"),Global.document.getElementById("exampleModalLabel"),Global.document.getElementById("dateFrom"),Global.document.getElementById("dateTo"),Global.document.getElementById("divJobApplicationContent"),Global.document.getElementById("slctDocumentName"),Global.document.getElementById("btnShowDivNewDocument"),Global.document.getElementById("btnDeleteDocument"),Global.document.getElementById("divVariables"),Global.document.getElementById("txtUserDefinedVariables"),Global.document.getElementById("divAttachments"),Global.document.getElementById("divAttachmentButtons"),Global.document.getElementById("btnAddPage"),Global.document.getElementById("divAddDocument"),Global.document.getElementById("txtNewDocumentName"),Global.document.getElementById("btnAddDocument"),Global.document.getElementById("divDisplayedDocument"),Global.document.getElementById("slctHtmlPageTemplate"),Global.document.getElementById("divUploadedFileDownload"),Global.document.getElementById("chkReplaceVariables"),Global.document.getElementById("btnDownloadFilePage"),Global.document.getElementById("divEmail"),Global.document.getElementById("divChoosePageType"),Global.document.getElementById("rbHtmlPage"),Global.document.getElementById("rbFilePage"),Global.document.getElementById("divCreateHtmlPage"),Global.document.getElementById("txtCreateHtmlPage"),Global.document.getElementById("btnCreateHtmlPage"),Global.document.getElementById("divCreateFilePage"),Global.document.getElementById("fileToUpload"),Global.document.getElementById("hiddenDocumentId"),Global.document.getElementById("hiddenNextPageIndex"),Global.document.getElementById("divSentApplications"),Global.document.getElementById("divEditUserValues"),Global.document.getElementById("divAddEmployer"),Global.document.getElementById("btnReadFromWebsite"),Global.document.getElementById("faReadFromWebsite"),Global.document.getElementById("txtReadFromWebsite"),Global.document.getElementById("btnApplyNowTop"),Global.document.getElementById("faBtnApplyNowTop"),Global.document.getElementById("btnApplyNowBottom"),Global.document.getElementById("faBtnApplyNowBottom"),Global.document.getElementById("btnSetEmployerEmailToUserEmail"),Global.document.getElementById("txtEmployerEmail"));
 };
 ClientHelpers.createTextarea=function(labelText,ref,minHeight)
 {
  var guid,c;
  guid=(c=Guid.NewGuid(),Guid.ToString(c,"N"));
  return Doc.Element("div",[AttrProxy.Create("class","form-group bottom-distanced")],[Doc.Element("label",[AttrProxy.Create("for",guid),AttrProxy.Create("style","font-weight: bold")],[Doc.TextNode(labelText)]),Doc.InputArea([AttrProxy.Create("id",guid),AttrProxy.Create("class","form-control"),AttrProxy.Create("style","wrap: soft; white-space: nowrap; overflow: auto; min-height: "+minHeight)],ref)]);
 };
 ClientHelpers.createRadio=function(labelText,radioValuesList)
 {
  var radioGroup,c;
  radioGroup=(c=Guid.NewGuid(),Guid.ToString(c,"N"));
  return Doc.Element("div",[AttrProxy.Create("class","bottom-distanced")],List.append(List.ofArray([Doc.Element("label",[AttrProxy.Create("style","font-weight: bold")],[Doc.TextNode(labelText)])]),List.mapi(function(i,t)
  {
   var guid,c$1;
   guid=(c$1=Guid.NewGuid(),Guid.ToString(c$1,"N"));
   return Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Radio([AttrProxy.Create("id",guid),AttrProxy.Create("type","radio"),AttrProxy.Create("name",radioGroup),AttrProxy.Create("checked",t[3])],t[1],t[2]),Doc.Element("label",[AttrProxy.Create("for",guid)],[Doc.TextNode(t[0])])]);
  },radioValuesList)));
 };
 ClientHelpers.createInput=function(labelText,ref,validFun)
 {
  var guid,c;
  guid=(c=Guid.NewGuid(),Guid.ToString(c,"N"));
  return Doc.Element("div",[AttrProxy.Create("class","form-group bottom-distanced")],[Doc.Element("label",[AttrProxy.Create("for",guid),AttrProxy.Create("style","font-weight: bold")],[Doc.TextNode(labelText)]),Doc.Input([AttrProxy.Create("id",guid),AttrProxy.Create("class","form-control"),AttrProxy.Create("type","text"),AttrModule.Handler("blur",function(el)
  {
   return function()
   {
    validFun(el.value);
    return null;
   };
  })],ref)]);
 };
 JobApplicationService.loginWithCookieOrAsGuest=function()
 {
  var b;
  function loginAsGuest()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    var sessionGuid,c,r;
    sessionGuid=(c=Guid.NewGuid(),Guid.ToString(c,"N"));
    Cookies.set("user",sessionGuid,(r={},r.expires=new Global.Date(Date.now()+604800),r));
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.loginAsGuest:-1276324058",[sessionGuid]),function()
    {
     return Concurrency.Return(null);
    });
   });
  }
  b=null;
  return Concurrency.Delay(function()
  {
   var userCookie;
   userCookie=Cookies.get("user");
   return userCookie!==void 0?Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.loginUserBySessionGuid:-1221772142",[userCookie]),function(a)
   {
    return!a?Concurrency.Bind(loginAsGuest(),function()
    {
     return Concurrency.Return(null);
    }):Concurrency.Zero();
   }):Concurrency.Bind(loginAsGuest(),function()
   {
    return Concurrency.Return(null);
   });
  });
 };
 Employer$1.getDivAddEmployer=function(refJobName,varUserEmailInput,refEmployer)
 {
  return Doc.Element("div",[AttrProxy.Create("id","divAddEmployer"),AttrProxy.Create("style","display: none")],[ClientHelpers.createInput(Translation.t(Language.German,Word.JobName),refJobName,function()
  {
   return"";
  }),Doc.EmbedView(varUserEmailInput.v),Doc.Element("h3",[],[Doc.TextNode(Translation.t(Language.German,Word.Employer))]),Doc.Element("div",[AttrProxy.Create("class","form-group row")],[Doc.Element("div",[AttrProxy.Create("class","col-lg-3")],[Doc.Element("button",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btn-block"),AttrProxy.Create("id","btnReadFromWebsite")],[Doc.Element("i",[AttrProxy.Create("class","fa fa-spinner fa-spin"),AttrProxy.Create("id","faReadFromWebsite"),AttrProxy.Create("style","color: black; margin-right: 10px; visibility: hidden")],[]),Doc.TextNode(Translation.t(Language.German,Word.LoadFromWebsite))])]),Doc.Element("div",[AttrProxy.Create("class","col-lg-9")],[Doc.Element("input",[AttrProxy.Create("id","txtReadFromWebsite"),AttrProxy.Create("type","text"),AttrProxy.Create("class","form-control"),AttrProxy.Create("placeholder","URL oder Referenznummer")],[])])]),Doc.Element("div",[AttrProxy.Create("class","form-group row col-12")],[Doc.Element("button",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btnLikeLink btn-block"),AttrProxy.Create("style","min-height: 40px; font-size: 20px"),AttrProxy.Create("id","btnApplyNowTop")],[Doc.Element("i",[AttrProxy.Create("class","fa fa-icon"),AttrProxy.Create("id","faBtnApplyNowTop"),AttrProxy.Create("style","color: #08a81b; margin-right: 10px")],[]),Doc.TextNode(Translation.t(Language.German,Word.ApplyNow))])]),ClientHelpers.createInput(Translation.t(Language.German,Word.CompanyName),refEmployer.company,function()
  {
   return"";
  }),ClientHelpers.createInput(Translation.t(Language.German,Word.Street),refEmployer.street,function()
  {
   return"";
  }),ClientHelpers.createInput(Translation.t(Language.German,Word.Postcode),refEmployer.postcode,function()
  {
   return"";
  }),ClientHelpers.createInput(Translation.t(Language.German,Word.City),refEmployer.city,function()
  {
   return"";
  }),ClientHelpers.createRadio(Translation.t(Language.German,Word.Gender),List.ofArray([[Translation.t(Language.German,Word.Male),Gender.Male,refEmployer.gender,""],[Translation.t(Language.German,Word.Female),Gender.Female,refEmployer.gender,""],[Translation.t(Language.German,Word.UnknownGender),Gender.Unknown,refEmployer.gender,"checked"]])),ClientHelpers.createInput(Translation.t(Language.German,Word.Degree),refEmployer.degree,function()
  {
   return"";
  }),ClientHelpers.createInput(Translation.t(Language.German,Word.FirstName),refEmployer.firstName,function()
  {
   return"";
  }),ClientHelpers.createInput(Translation.t(Language.German,Word.LastName),refEmployer.lastName,function()
  {
   return"";
  }),Doc.Element("div",[AttrProxy.Create("class","form-group bottom-distanced")],[Doc.Element("div",[],[Doc.Element("label",[AttrProxy.Create("for","txtEmployerEmail"),AttrProxy.Create("style","font-weight: bold")],[Doc.TextNode("Email")]),Doc.Element("button",[AttrProxy.Create("id","btnSetEmployerEmailToUserEmail"),AttrProxy.Create("class","distanced")],[Doc.TextNode("an dich")])]),Doc.Input([AttrProxy.Create("id","txtEmployerEmail"),AttrProxy.Create("class","form-control"),AttrProxy.Create("type","text")],refEmployer.email)]),ClientHelpers.createInput(Translation.t(Language.German,Word.Phone),refEmployer.phone,function()
  {
   return"";
  }),ClientHelpers.createInput(Translation.t(Language.German,Word.MobilePhone),refEmployer.mobilePhone,function()
  {
   return"";
  }),Doc.Element("button",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btnLikeLink btn-block"),AttrProxy.Create("style","min-height: 40px; font-size: 20px"),AttrProxy.Create("id","btnApplyNowBottom")],[Doc.Element("i",[AttrProxy.Create("class","fa fa-icon"),AttrProxy.Create("id","faBtnApplyNowBottom"),AttrProxy.Create("style","color: #08a81b; margin-right: 10px")],[]),Doc.TextNode(Translation.t(Language.German,Word.ApplyNow))])]);
 };
 UserValues$1.getDivUserValues=function(refUserValues)
 {
  return Doc.Element("div",[AttrProxy.Create("id","divEditUserValues"),AttrProxy.Create("style","display: none")],[Doc.Element("h3",[],[Doc.TextNode(Translation.t(Language.German,Word.YourValues))]),Doc.Element("b",[],[Doc.TextNode("Dies sind keine Pflichtangaben.")]),Doc.Element("br",[],[]),Doc.TextNode("Diese Angaben setzen die Werte von Variablen die mit \"$mein\" beginnen, zum Beispiel \"$meinVorname\". Statt hier deine Daten einzutragen kannst du alle Vorkommen dieser Variablen auch direkt ersetzen."),Doc.Element("br",[],[]),Doc.Element("br",[],[]),ClientHelpers.createInput(Translation.t(Language.German,Word.Degree),refUserValues.degree,function()
  {
   return"";
  }),ClientHelpers.createRadio(Translation.t(Language.German,Word.Gender),List.ofArray([[Translation.t(Language.German,Word.Male),Gender.Male,refUserValues.gender,""],[Translation.t(Language.German,Word.Female),Gender.Female,refUserValues.gender,""]])),ClientHelpers.createInput(Translation.t(Language.German,Word.FirstName),refUserValues.firstName,function()
  {
   return"";
  }),ClientHelpers.createInput(Translation.t(Language.German,Word.LastName),refUserValues.lastName,function()
  {
   return"";
  }),ClientHelpers.createInput(Translation.t(Language.German,Word.Street),refUserValues.street,function()
  {
   return"";
  }),ClientHelpers.createInput(Translation.t(Language.German,Word.Postcode),refUserValues.postcode,function()
  {
   return"";
  }),ClientHelpers.createInput(Translation.t(Language.German,Word.City),refUserValues.city,function()
  {
   return"";
  }),ClientHelpers.createInput(Translation.t(Language.German,Word.Phone),refUserValues.phone,function()
  {
   return"";
  }),ClientHelpers.createInput(Translation.t(Language.German,Word.MobilePhone),refUserValues.mobilePhone,function()
  {
   return"";
  })]);
 };
 Email.getDivEmail=function(refSubject,refBody)
 {
  return Doc.Element("div",[AttrProxy.Create("id","divEmail"),AttrProxy.Create("style","display: none")],[Doc.Element("h3",[],[Doc.TextNode(Translation.t(Language.German,Word.Email))]),ClientHelpers.createInput(Translation.t(Language.German,Word.EmailSubject),refSubject,function()
  {
   return"";
  }),ClientHelpers.createTextarea(Translation.t(Language.German,Word.EmailBody),refBody,"400px")]);
 };
 Variables.getDivVariables=function(refCustomVariables)
 {
  return Doc.Element("div",[AttrProxy.Create("id","divVariables"),AttrProxy.Create("style","display: none")],[Doc.Element("h3",[AttrProxy.Create("class","distanced-bottom")],[Doc.TextNode("Variablen")]),Doc.Element("h5",[AttrProxy.Create("class","distanced-bottom")],[Doc.TextNode("Vordefiniert")]),Doc.Element("b",[],[Doc.TextNode("Arbeitgeber")]),Doc.Element("br",[],[]),Doc.TextNode("$firmaName"),Doc.Element("br",[],[]),Doc.TextNode("$firmaStrasse"),Doc.Element("br",[],[]),Doc.TextNode("$firmaPlz"),Doc.Element("br",[],[]),Doc.TextNode("$firmaStadt"),Doc.Element("br",[],[]),Doc.TextNode("$chefTitel"),Doc.Element("br",[],[]),Doc.TextNode("$chefVorname"),Doc.Element("br",[],[]),Doc.TextNode("$chefNachname"),Doc.Element("br",[],[]),Doc.TextNode("$chefEmail"),Doc.Element("br",[],[]),Doc.TextNode("$chefTelefon"),Doc.Element("br",[],[]),Doc.TextNode("$chefMobil"),Doc.Element("br",[],[]),Doc.Element("hr",[],[]),Doc.Element("b",[],[Doc.TextNode(Translation.t(Language.German,Word.YourValues))]),Doc.Element("br",[],[]),Doc.TextNode("$meinGeschlecht"),Doc.Element("br",[],[]),Doc.TextNode("$meinTitel"),Doc.Element("br",[],[]),Doc.TextNode("$meinVorname"),Doc.Element("br",[],[]),Doc.TextNode("$meinNachname"),Doc.Element("br",[],[]),Doc.TextNode("$meineStrasse"),Doc.Element("br",[],[]),Doc.TextNode("$meinePlz"),Doc.Element("br",[],[]),Doc.TextNode("$meineStadt"),Doc.Element("br",[],[]),Doc.TextNode("$meineEmail"),Doc.Element("br",[],[]),Doc.TextNode("$meineMobilnr"),Doc.Element("br",[],[]),Doc.TextNode("$meineTelefonnr"),Doc.Element("br",[],[]),Doc.Element("hr",[],[]),Doc.Element("b",[],[Doc.TextNode("Datum")]),Doc.Element("br",[],[]),Doc.TextNode("$tagHeute"),Doc.Element("br",[],[]),Doc.TextNode("$monatHeute"),Doc.Element("br",[],[]),Doc.TextNode("$jahrHeute"),Doc.Element("br",[],[]),Doc.Element("hr",[],[]),Doc.Element("b",[],[Doc.TextNode("Sonstige")]),Doc.Element("br",[],[]),Doc.TextNode("$jobName"),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("hr",[],[]),Doc.Element("br",[],[]),Doc.Element("h5",[AttrProxy.Create("class","distanced-bottom")],[Doc.TextNode("Benutzerdefiniert")]),Doc.InputArea([AttrProxy.Create("style","width: 100%; min-height: 500px"),AttrProxy.Create("id","txtUserDefinedVariables")],refCustomVariables)]);
 };
 Client$1.templates=function()
 {
  var varDocument,refDocument,varUserValues,refUserValues,varUserEmail,varUserEmailInput,varEmployer,refEmployer,varDisplayedDocument,varDivSentApplications,els,b;
  function getCurrentPageIndex()
  {
   return Math.max(Global.jQuery(els.divAttachmentButtons).find(".mainButton").index(Global.jQuery(".active"))+1,1);
  }
  function getSentApplications()
  {
   var varColumns,varEmployerModal,employer,url,b$1;
   varColumns=ListModel.FromSeq([["Firma",refEmployer.company,true],["Vorname",refEmployer.firstName,true],["Nachname",refEmployer.lastName,false],["Straße",refEmployer.street,true],["PLZ",refEmployer.postcode,true],["Stadt",refEmployer.city,false]]);
   varEmployerModal=Var.Create$1((employer=Types.emptyEmployer(),(url="",Doc.Element("div",[AttrProxy.Create("class","modal fade in"),AttrProxy.Create("id","employerModal"),AttrProxy.Create("tabindex","-1"),AttrProxy.Create("role","dialog"),AttrProxy.Create("aria-labelledby","exampleModalLabel"),AttrProxy.Create("aria-hidden","true")],[Doc.Element("div",[AttrProxy.Create("class","modal-dialog"),AttrProxy.Create("role","document")],[Doc.Element("div",[AttrProxy.Create("class","modal-content")],[Doc.Element("div",[AttrProxy.Create("class","modal-header")],[Doc.Element("h5",[AttrProxy.Create("class","modal-title"),AttrProxy.Create("id","exampleModalLabel")],[Doc.TextNode(employer.company)]),Doc.Element("button",[AttrProxy.Create("type","button"),AttrProxy.Create("class","close"),AttrProxy.Create("aria-label","Close"),AttrProxy.Create("data-"+"dismiss","modal")],[Doc.Element("span",[AttrProxy.Create("aria-hidden","true")],[Doc.TextNode("x")])])]),Doc.Element("div",[AttrProxy.Create("class","modal-body")],[Doc.TextNode(employer.company),Doc.Element("br",[],[]),Doc.TextNode(employer.street),Doc.Element("br",[],[]),Doc.TextNode(employer.postcode+" "+employer.city),Doc.Element("br",[],[]),Doc.TextNode((employer.degree!==""?employer.degree+" ":"")+employer.firstName+" "+employer.lastName),Doc.Element("br",[],[]),Doc.TextNode(employer.email),Doc.Element("br",[],[]),Doc.TextNode(employer.phone),Doc.Element("br",[],[]),Doc.TextNode(employer.mobilePhone),Doc.Element("br",[],[]),url.indexOf("://")!=-1?Doc.Element("a",[AttrProxy.Create("href",url),AttrProxy.Create("target","blank")],[Doc.TextNode(url)]):Doc.TextNode(url)]),Doc.Element("div",[AttrProxy.Create("class","modal-footer")],[])])])]))));
   b$1=null;
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getSentApplications:862324573",[]),function(a)
    {
     var varSentApplications,dateFrom,c,c$1,c$2,dateTo,c$3,c$4,c$5;
     function setSentApplicationsFromTo()
     {
      var dateFromParsed,dateFrom$1,dateToParsed,dateTo$1,p,dateTo$2,dateFrom$2;
      function p$1(a$2,a$3,d,a$4)
      {
       return d>=dateFrom$2&&d<=dateTo$2;
      }
      dateFromParsed=DateUtil.Parse(els.dateFrom.value);
      dateFrom$1=(new Date((new Date(dateFromParsed)).getFullYear(),(new Date(dateFromParsed)).getMonth()+1-1,(new Date(dateFromParsed)).getDate())).getTime();
      dateToParsed=DateUtil.Parse(els.dateTo.value);
      dateTo$1=(new Date((new Date(dateToParsed)).getFullYear(),(new Date(dateToParsed)).getMonth()+1-1,(new Date(dateToParsed)).getDate())).getTime();
      p=dateFrom$1<=dateTo$1?[dateFrom$1,dateTo$1]:[dateTo$1,dateFrom$1];
      dateTo$2=p[1];
      dateFrom$2=p[0];
      varSentApplications.Set(List.filter(function($1)
      {
       return p$1($1[0],$1[1],$1[2],$1[3]);
      },a));
     }
     function a$1(name,ref,selected)
     {
      return selected?Doc.Element("th",[],[Doc.TextNode(name)]):Doc.TextNode("");
     }
     varSentApplications=ListModel.FromSeq(a);
     Var.Set(varDivSentApplications,Doc.Element("div",[AttrProxy.Create("style","width: 100%; height: 100%; overflow: auto")],[Doc.TextNode("Von"),Doc.Element("input",[AttrProxy.Create("type","date"),AttrProxy.Create("id","dateFrom"),AttrProxy.Create("value",(dateFrom=(c=(c$1=Date.now(),DateUtil.AddMonths(c$1,-1)),c+(-(c$2=Date.now(),(new Date(c$2)).getDate())+1)*86400000),((((Runtime.Curried(function($1,$2,$3,$4)
     {
      return $1(Utils.padNumLeft(String($2),4)+"-"+Utils.padNumLeft(String($3),2)+"-"+Utils.padNumLeft(String($4),2));
     },4))(Global.id))((new Date(dateFrom)).getFullYear()))((new Date(dateFrom)).getMonth()+1))((new Date(dateFrom)).getDate()))),AttrProxy.Create("style","margin-left: 15px; margin-right: 15px;"),AttrModule.Handler("change",function()
     {
      return function()
      {
       return setSentApplicationsFromTo();
      };
     })],[]),Doc.TextNode("bis"),Doc.Element("input",[AttrProxy.Create("type","date"),AttrProxy.Create("id","dateTo"),AttrProxy.Create("value",(dateTo=(c$3=(c$4=Date.now(),DateUtil.AddMonths(c$4,1)),c$3+-(c$5=Date.now(),(new Date(c$5)).getDate())*86400000),((((Runtime.Curried(function($1,$2,$3,$4)
     {
      return $1(Utils.padNumLeft(String($2),4)+"-"+Utils.padNumLeft(String($3),2)+"-"+Utils.padNumLeft(String($4),2));
     },4))(Global.id))((new Date(dateTo)).getFullYear()))((new Date(dateTo)).getMonth()+1))((new Date(dateTo)).getDate()))),AttrProxy.Create("style","margin-left: 15px;"),AttrModule.Handler("change",function()
     {
      return function()
      {
       return setSentApplicationsFromTo();
      };
     })],[]),Doc.Element("button",[AttrProxy.Create("style","float : right;")],[Doc.TextNode("Liste downloaden")]),Doc.EmbedView(varEmployerModal.v),Doc.Element("table",[AttrProxy.Create("style","border-spacing: 10px; border-collapse: separate")],[Doc.Element("thead",[],[Doc.Element("tr",[],[Doc.Convert(function($1)
     {
      return a$1($1[0],$1[1],$1[2]);
     },varColumns.v)])]),Doc.Element("tbody",[],[])])]));
     return Concurrency.Zero();
    });
   });
  }
  function fillDocumentValues()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    var pageMapElements,m,myMap;
    return Concurrency.Combine(varDocument.c.pages.$!==0?(pageMapElements=Global.document.querySelectorAll("[data-page-key]"),(m=varDocument.c.pages.get_Item(getCurrentPageIndex()-1),m.$==1?Concurrency.Zero():(myMap=Map.OfArray(Arrays.ofSeq(m.$0.map)),(Global.jQuery(pageMapElements).each(function($1,el)
    {
     var key;
     key=el.getAttribute("data-page-key");
     myMap.ContainsKey(key)?el.value=myMap.get_Item(key):el.value=el.getAttribute("data-page-value");
    }),Concurrency.Zero())))):Concurrency.Zero(),Concurrency.Delay(function()
    {
     var refMap;
     refMap=Map.OfArray(Arrays.ofSeq(List.ofArray([["userGender",{
      $:1,
      $0:refUserValues.gender
     }],["userDegree",{
      $:0,
      $0:refUserValues.degree
     }],["userFirstName",{
      $:0,
      $0:refUserValues.firstName
     }],["userLastName",{
      $:0,
      $0:refUserValues.lastName
     }],["userStreet",{
      $:0,
      $0:refUserValues.street
     }],["userPostcode",{
      $:0,
      $0:refUserValues.postcode
     }],["userCity",{
      $:0,
      $0:refUserValues.city
     }],["userPhone",{
      $:0,
      $0:refUserValues.phone
     }],["userMobilePhone",{
      $:0,
      $0:refUserValues.mobilePhone
     }],["employerCompany",{
      $:0,
      $0:refEmployer.company
     }],["employerStreet",{
      $:0,
      $0:refEmployer.street
     }],["employerPostcode",{
      $:0,
      $0:refEmployer.postcode
     }],["employerCity",{
      $:0,
      $0:refEmployer.city
     }],["employerDegree",{
      $:0,
      $0:refEmployer.degree
     }],["employerFirstName",{
      $:0,
      $0:refEmployer.firstName
     }],["employerLastName",{
      $:0,
      $0:refEmployer.lastName
     }],["employerEmail",{
      $:0,
      $0:refEmployer.email
     }],["employerPhone",{
      $:0,
      $0:refEmployer.phone
     }],["employerMobilePhone",{
      $:0,
      $0:refEmployer.mobilePhone
     }],["documentEmailSubject",{
      $:0,
      $0:refDocument.emailSubject
     }],["documentEmailBody",{
      $:0,
      $0:refDocument.emailBody
     }],["documentJobName",{
      $:0,
      $0:refDocument.jobName
     }]])));
     Global.jQuery("[data-bind-ref]").each(function(i,el)
     {
      var attributes,m$1;
      attributes=List.ofSeq(Seq.delay(function()
      {
       return Seq.collect(function(i$1)
       {
        return!Strings.StartsWith(el.attributes.item(i$1).name,"data-bind")?[AttrProxy.Create(el.attributes.item(i$1).name,el.attributes.item(i$1).value)]:[];
       },Operators.range(0,el.attributes.length-1));
      }));
      m$1=refMap.get_Item(el.getAttribute("data-bind-ref"));
      return m$1.$==1?Doc.Radio(attributes,Gender.fromString(el.getAttribute("data-bind-value")),m$1.$0).ReplaceInDom(el):Doc.Input(attributes,m$1.$0).ReplaceInDom(el);
     });
     return Concurrency.Zero();
    }));
   });
  }
  function loadPageTemplate()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    Global.jQuery("#divInsert").remove();
    return Concurrency.Combine(Concurrency.While(function()
    {
     return!Unchecked.Equals(Global.document.getElementById("divInsert"),null);
    },Concurrency.Delay(function()
    {
     return Concurrency.Bind(Concurrency.Sleep(10),function()
     {
      return Concurrency.Return(null);
     });
    })),Concurrency.Delay(function()
    {
     var pageTemplateIndex;
     pageTemplateIndex=els.slctHtmlPageTemplate.selectedIndex;
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getHtmlPageTemplate:-1608612974",[pageTemplateIndex+1]),function(a)
     {
      Var.Set(varDisplayedDocument,Doc.Verbatim(a==null?"":a.$0));
      return Concurrency.Combine(Concurrency.While(function()
      {
       return Unchecked.Equals(Global.document.getElementById("divInsert"),null);
      },Concurrency.Delay(function()
      {
       return Concurrency.Bind(Concurrency.Sleep(10),function()
       {
        return Concurrency.Return(null);
       });
      })),Concurrency.Delay(function()
      {
       Global.jQuery(Global.document.querySelectorAll("[data-page-key]")).each(function(i,el)
       {
        var key;
        function eventAction()
        {
         var p,t,currentAndAfter,p$1,htmlPage,htmlPage$1,i$1;
         p=(t=List.splitAt(getCurrentPageIndex()-1,varDocument.c.pages),(currentAndAfter=t[1],(p$1=currentAndAfter.$==0?Operators.FailWith("pageList was empty"):currentAndAfter.$0.$==1?[new DocumentPage({
          $:1,
          $0:currentAndAfter.$0.$0
         }),List.T.Empty]:currentAndAfter.$1.$==0?(htmlPage=currentAndAfter.$0.$0,[new DocumentPage({
          $:0,
          $0:HtmlPage.New(htmlPage.name,htmlPage.oTemplateId,htmlPage.pageIndex,List.ofSeq(Map.ToSeq(Map.OfArray(Arrays.ofSeq(htmlPage.map)).Add(key,String(Global.jQuery(el).val())))))
         }),List.T.Empty]):(htmlPage$1=currentAndAfter.$0.$0,[new DocumentPage({
          $:0,
          $0:HtmlPage.New(htmlPage$1.name,htmlPage$1.oTemplateId,htmlPage$1.pageIndex,List.ofSeq(Map.ToSeq(Map.OfArray(Arrays.ofSeq(htmlPage$1.map)).Add(key,String(Global.jQuery(el).val())))))
         }),currentAndAfter.$1]),[t[0],p$1[0],p$1[1]])));
         Var.Set(varDocument,(i$1=varDocument.c,Document.New(i$1.oId,i$1.name,List.append(p[0],new List.T({
          $:1,
          $0:p[1],
          $1:p[2]
         })),i$1.email,i$1.jobName,i$1.customVariables)));
        }
        key=el.getAttribute("data-page-key");
        el.removeEventListener("input",eventAction,true);
        return el.addEventListener("input",eventAction,true);
       });
       return Concurrency.Bind(fillDocumentValues(),function()
       {
        return Concurrency.Return(null);
       });
      }));
     });
    }));
   });
  }
  function showHideMutualElements()
  {
   return List.ofArray([els.divCreateFilePage,els.divCreateHtmlPage,els.divChoosePageType,els.divEmail,els.divAddDocument,els.divEditUserValues,els.divAddEmployer,els.divDisplayedDocument,els.divAttachments,els.divUploadedFileDownload,els.divSentApplications,els.divVariables]);
  }
  function show(showEls)
  {
   var e,i,e$1;
   e=Enumerator.Get(showEls);
   try
   {
    while(e.MoveNext())
     e.Current().style.display="block";
   }
   finally
   {
    if("Dispose"in e)
     e.Dispose();
   }
   i=showHideMutualElements();
   e$1=Enumerator.Get(i);
   try
   {
    while(e$1.MoveNext())
     (function()
     {
      var hideEl;
      hideEl=e$1.Current();
      return!List.exists(function(x)
      {
       return x.id===hideEl.id;
      },showEls)?void(hideEl.style.display="none"):null;
     }());
   }
   finally
   {
    if("Dispose"in e$1)
     e$1.Dispose();
   }
  }
  function addSelectOption(el,value)
  {
   var optionEl;
   optionEl=Global.document.createElement("option");
   optionEl.textContent=value;
   return el.add(optionEl);
  }
  function setDocument()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    var b$2;
    return Concurrency.Bind(els.slctDocumentName.selectedIndex>=0?(new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getDocumentOffset:-1633111335",[els.slctDocumentName.selectedIndex]):(b$2=null,Concurrency.Delay(function()
    {
     return Concurrency.Return(null);
    })),function(a)
    {
     return Concurrency.Combine(a==null?Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.saveNewDocument:1535261021",[Types.emptyDocument()]),function(a$1)
     {
      Var.Set(varDocument,Document.New({
       $:1,
       $0:a$1
      },Types.emptyDocument().name,Types.emptyDocument().pages,Types.emptyDocument().email,Types.emptyDocument().jobName,Types.emptyDocument().customVariables));
      addSelectOption(els.slctDocumentName,varDocument.c.name);
      return Concurrency.Zero();
     }):(Var.Set(varDocument,a.$0),Concurrency.Zero()),Concurrency.Delay(function()
     {
      els.hiddenDocumentId.value=varDocument.c.GetId();
      show(List.ofArray([els.divAttachments]));
      return Concurrency.Zero();
     }));
    });
   });
  }
  function setPageButtons()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    Global.jQuery(els.divAttachmentButtons).children("div").remove();
    return Concurrency.Combine(Concurrency.For(varDocument.c.pages,function(a)
    {
     var deleteButton,pageUpButton,pageDownButton,mainButton,firstDiv,_this,secondDiv,_this$1,_this$2,_this$3,a$1,a$2,_this$4,_this$5;
     deleteButton=Global.jQuery("<button class=\"distanced\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></button>").on("click",function()
     {
      var i,t,x,before,after,b$2;
      return Global.confirm(Strings.SFormat(Translation.t(Language.German,Word.ReallyDeletePage),[a.Name()]))?(Var.Set(varDocument,(i=varDocument.c,Document.New(i.oId,i.name,(t=(x=varDocument.c.pages,List.splitAt(a.PageIndex$1()-1,x)),(before=t[0],(after=t[1],after.$==0?before:List.append(before,List.map(function(p)
      {
       return p.PageIndex(p.PageIndex$1()-1);
      },after.$1))))),i.email,i.jobName,i.customVariables))),Concurrency.Start((b$2=null,Concurrency.Delay(function()
      {
       return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.overwriteDocument:-220687727",[varDocument.c]),function()
       {
        return Concurrency.Bind(setDocument(),function()
        {
         return Concurrency.Bind(setPageButtons(),function()
         {
          return Concurrency.Return(null);
         });
        });
       });
      })),null)):null;
     });
     pageUpButton=Global.jQuery("<button class=\"distanced\"><i class=\"fa fa-arrow-up\" aria-hidden=\"true\"></i></button>").on("click",function()
     {
      var b$2;
      return Concurrency.Start((b$2=null,Concurrency.Delay(function()
      {
       var i,t,x,before,after,x1,x2;
       Var.Set(varDocument,(i=varDocument.c,Document.New(i.oId,i.name,(t=(x=varDocument.c.pages,List.splitAt(a.PageIndex$1()-2,x)),(before=t[0],(after=t[1],after.$==0?before:after.$1.$==0?List.append(before,List.ofArray([after.$0])):(x1=after.$0,(x2=after.$1.$0,List.append(before,new List.T({
        $:1,
        $0:x2.PageIndex(x1.PageIndex$1()),
        $1:new List.T({
         $:1,
         $0:x1.PageIndex(x2.PageIndex$1()),
         $1:after.$1.$1
        })
       }))))))),i.email,i.jobName,i.customVariables)));
       return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.overwriteDocument:-220687727",[varDocument.c]),function()
       {
        return Concurrency.Bind(setDocument(),function()
        {
         return Concurrency.Bind(setPageButtons(),function()
         {
          return Concurrency.Return(null);
         });
        });
       });
      })),null);
     });
     pageDownButton=Global.jQuery("<button class=\"distanced\"><i class=\"fa fa-arrow-down\" aria-hidden=\"true\"></i></button>").on("click",function()
     {
      var b$2;
      return Concurrency.Start((b$2=null,Concurrency.Delay(function()
      {
       var i,t,x,before,after,x1,x2;
       Var.Set(varDocument,(i=varDocument.c,Document.New(i.oId,i.name,(t=(x=varDocument.c.pages,List.splitAt(a.PageIndex$1()-1,x)),(before=t[0],(after=t[1],after.$==0?before:after.$1.$==0?List.append(before,List.ofArray([after.$0])):(x1=after.$0,(x2=after.$1.$0,List.append(before,new List.T({
        $:1,
        $0:x2.PageIndex(x1.PageIndex$1()),
        $1:new List.T({
         $:1,
         $0:x1.PageIndex(x2.PageIndex$1()),
         $1:after.$1.$1
        })
       }))))))),i.email,i.jobName,i.customVariables)));
       return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.overwriteDocument:-220687727",[varDocument.c]),function()
       {
        return Concurrency.Bind(setDocument(),function()
        {
         return Concurrency.Bind(setPageButtons(),function()
         {
          return Concurrency.Return(null);
         });
        });
       });
      })),null);
     });
     mainButton=Global.jQuery((function($1)
     {
      return function($2)
      {
       return $1("<button class=\"distanced btn-block mainButton\" style=\"width: 100%\">"+Utils.toSafe($2)+"</button>");
      };
     }(Global.id))(a.Name())).on("click",function()
     {
      var $this,htmlPage,b$2;
      $this=this;
      Global.jQuery(this).addClass("active");
      Global.jQuery(this).parent().parent().parent().find(".mainButton").each(function($1,b$3)
      {
       if(!Unchecked.Equals(b$3,$this))
        Global.jQuery(b$3).removeClass("active");
      });
      return a.$==1?show(List.ofArray([els.divAttachments,els.divUploadedFileDownload])):(htmlPage=a.$0,Concurrency.Start((b$2=null,Concurrency.Delay(function()
      {
       Global.jQuery("#divInsert").remove();
       return Concurrency.Combine(Concurrency.While(function()
       {
        return!Unchecked.Equals(Global.document.getElementById("divInsert"),null);
       },Concurrency.Delay(function()
       {
        return Concurrency.Bind(Concurrency.Sleep(10),function()
        {
         return Concurrency.Return(null);
        });
       })),Concurrency.Delay(function()
       {
        var o;
        return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getHtmlPageTemplate:-1608612974",[(o=htmlPage.oTemplateId,o==null?1:o.$0)]),function(a$3)
        {
         Var.Set(varDisplayedDocument,Doc.Verbatim(a$3==null?"":a$3.$0));
         return Concurrency.Combine(Concurrency.While(function()
         {
          return Unchecked.Equals(Global.document.getElementById("divInsert"),null);
         },Concurrency.Delay(function()
         {
          return Concurrency.Bind(Concurrency.Sleep(10),function()
          {
           return Concurrency.Return(null);
          });
         })),Concurrency.Delay(function()
         {
          return Concurrency.Bind(fillDocumentValues(),function()
          {
           show(List.ofArray([els.divDisplayedDocument,els.divAttachments]));
           return Concurrency.Zero();
          });
         }));
        });
       }));
      })),null));
     });
     firstDiv=(_this=Global.jQuery("<div class=\"col-6 col-sm-7 col-md-8 col-lg-8 col-xl-8\" style=\"float: left; display:inline; padding: 0; margin-left:0;\"></div>"),_this.append.apply(_this,[mainButton]));
     secondDiv=(_this$1=(_this$2=(_this$3=Global.jQuery("<div class=\"col-6 col-sm-5 col-md-4 col-lg-4 col-xl-4\" style=\"float: right; display:inline\"></div>"),(a$1=a.PageIndex$1()>=List.length(varDocument.c.pages)?pageDownButton.css("visibility","hidden"):pageDownButton,_this$3.append.apply(_this$3,[a$1]))),(a$2=a.PageIndex$1()<=1?pageUpButton.css("visibility","hidden"):pageUpButton,_this$2.append.apply(_this$2,[a$2]))),_this$1.append.apply(_this$1,[deleteButton]));
     (_this$4=(_this$5=Global.jQuery("<div class=\"row\" style=\"min-width:100%; margin-bottom: 10px;\"></div>"),_this$5.append.apply(_this$5,[firstDiv])),_this$4.append.apply(_this$4,[secondDiv])).insertBefore("#btnAddPage");
     return Concurrency.Zero();
    }),Concurrency.Delay(function()
    {
     els.hiddenNextPageIndex.value=String(Global.jQuery(els.divAttachmentButtons).children("div").length+1);
     return Concurrency.Zero();
    }));
   });
  }
  function btnApplyNowClicked()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.isLoggedInAsGuest:-900658348",[]),function(a)
    {
     var regex;
     regex=new Global.RegExp("^(([^<>()\\[\\]\\\\.,;:\\s@\"]+(\\.[^<>()\\[\\]\\\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$");
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.tryFindSentApplication:234068265",[varEmployer.c]),function(a$1)
     {
      return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.setUserEmail:2079759932",[varUserEmail.c]),function(a$2)
      {
       return(a?!regex.test(varUserEmail.c)?(Global.alert("Deine Email scheint ungültig zu sein."),false):a$2.$==1?(Global.alert(Strings.Join("",Arrays.ofSeq(a$2.$0))),false):true:true)&&(!regex.test(refEmployer.email.RVal())?(Global.alert(Translation.t(Language.German,Word.TheEmailOfYourEmployerDoesNotLookValid)+", "+refEmployer.email.RVal()),false):true)&&(Strings.Trim(refDocument.jobName.RVal())===""?(Global.alert(Strings.SFormat(Translation.t(Language.German,Word.FieldIsRequired),[Translation.t(Language.German,Word.JobName)])),false):true)&&(a$1==null||a$1!=null&&Global.confirm("Du hast dich schon einmal bei dieser Firmen-Email-Adresse beworben.\nBewerbung trotzdem abschicken?"))?Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.overwriteDocument:-220687727",[varDocument.c]),function()
       {
        var fontAwesomeEls;
        fontAwesomeEls=List.ofArray([Global.jQuery(els.faBtnApplyNowBottom),Global.jQuery(els.faBtnApplyNowTop)]);
        List.iter(function(faEl)
        {
         faEl.css("color","black");
         faEl.addClass("fa-spinner fa-spin");
        },fontAwesomeEls);
        Global.jQuery(els.divJobApplicationContent).find("input,textarea,button,select").prop("disabled",true);
        return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.applyNow:-2017393559",[varEmployer.c,varDocument.c,varUserValues.c,els.txtReadFromWebsite.value]),function(a$3)
        {
         List.iter(function(faEl)
         {
          faEl.removeClass("fa-spinner fa-spin");
         },fontAwesomeEls);
         Global.jQuery(els.divJobApplicationContent).find("input,textarea,button,select").prop("disabled",false);
         return a$3.$==0?(List.iter(function(faEl)
         {
          faEl.css("color","#08a81b");
          faEl.addClass("fa-check");
         },fontAwesomeEls),Var.Set(varEmployer,Types.emptyEmployer()),els.txtReadFromWebsite.value="",Concurrency.Bind(Concurrency.Sleep(4500),function()
         {
          List.iter(function(faEl)
          {
           faEl.removeClass("fa-check");
          },fontAwesomeEls);
          return Concurrency.Zero();
         })):Concurrency.Bind(Concurrency.Sleep(700),function()
         {
          Global.alert(Translation.t(Language.German,Word.SorryAnErrorOccurred)+"\n"+Translation.t(Language.German,Word.YourApplicationHasNotBeenSent));
          return Concurrency.Zero();
         });
        });
       }):Concurrency.Zero();
      });
     });
    });
   });
  }
  function readFromWebsite()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    els.btnReadFromWebsite.disabled=true;
    els.faReadFromWebsite.style.visibility="visible";
    return Concurrency.Bind(Concurrency.Sleep(200),function()
    {
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.readWebsite:1243289988",[els.txtReadFromWebsite.value]),function(a)
     {
      els.btnReadFromWebsite.disabled=false;
      els.faReadFromWebsite.style.visibility="hidden";
      return a.$==1?(Global.alert(Seq.fold(function($1,$2)
      {
       return $1+$2+"\n";
      },"",a.$0)),Concurrency.Zero()):(Var.Set(varEmployer,a.$0),Concurrency.Zero());
     });
    });
   });
  }
  function registerEvents()
  {
   els.slctDocumentName.addEventListener("change",function()
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     return Concurrency.Bind(setDocument(),function()
     {
      return Concurrency.Bind(setPageButtons(),function()
      {
       return Concurrency.Bind(fillDocumentValues(),function()
       {
        return Concurrency.Return(null);
       });
      });
     });
    })),null);
   },false);
   els.btnShowDivNewDocument.addEventListener("click",function()
   {
    return show(List.ofArray([els.divAddDocument]));
   },false);
   els.btnDeleteDocument.addEventListener("click",function(ev)
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     var m;
     return Concurrency.Combine(els.slctDocumentName.selectedIndex>=0&&Global.confirm(Strings.SFormat(Translation.t(Language.German,Word.ReallyDeleteDocument),[varDocument.c.name]))?Concurrency.Combine((m=varDocument.c.oId,m==null?Concurrency.Zero():Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.deleteDocument:-2045396727",[m.$0]),function()
     {
      return Concurrency.Return(null);
     })),Concurrency.Delay(function()
     {
      els.slctDocumentName.removeChild(els.slctDocumentName[els.slctDocumentName.selectedIndex]);
      return Concurrency.Combine(els.slctDocumentName.length===0?(ev.target.style.display="none",Var.Set(varDocument,Types.emptyDocument()),Concurrency.Zero()):Concurrency.Zero(),Concurrency.Delay(function()
      {
       show(List.ofArray([els.divAttachments]));
       return Concurrency.Zero();
      }));
     })):(Var.Set(varDocument,Types.emptyDocument()),Concurrency.Zero()),Concurrency.Delay(function()
     {
      return Concurrency.Bind(setDocument(),function()
      {
       return Concurrency.Bind(setPageButtons(),function()
       {
        return Concurrency.Return(null);
       });
      });
     }));
    })),null);
   },false);
   els.txtUserDefinedVariables.addEventListener("keydown",function(ev)
   {
    var v,s,e;
    return ev.keyCode===9?(v=String(ev.target.value),(s=ev.target.selectionStart,(e=ev.target.selectionEnd,(ev.target.value=Strings.Substring(v,0,s)+"\u0009"+v.substring(e),ev.target.selectionStart=s+1,ev.target.selectionEnd=s+1,ev.stopPropagation(),ev.preventDefault())))):null;
   },false);
   els.btnAddPage.addEventListener("click",function()
   {
    return els.slctDocumentName.selectedIndex>=0?(show(List.ofArray([els.divChoosePageType,els.divAttachments,els.divCreateFilePage])),void(els.rbFilePage.checked=true)):(Global.alert("Bitte erst eine neue Bewerbungsmappe anlegen"),show(List.ofArray([els.divAddDocument])));
   },false);
   els.btnAddDocument.addEventListener("click",function()
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     var newDocumentName,i;
     newDocumentName=String(els.txtNewDocumentName.value);
     return Strings.Trim(newDocumentName)===""?(Global.alert(Strings.SFormat(Translation.t(Language.German,Word.FieldIsRequired),[Translation.t(Language.German,Word.DocumentName)])),Concurrency.Zero()):(Var.Set(varDocument,(i=varDocument.c,Document.New(i.oId,newDocumentName,i.pages,i.email,i.jobName,i.customVariables))),Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.saveNewDocument:1535261021",[varDocument.c]),function(a)
     {
      var i$1;
      Var.Set(varDocument,(i$1=varDocument.c,Document.New({
       $:1,
       $0:a
      },i$1.name,i$1.pages,i$1.email,i$1.jobName,i$1.customVariables)));
      addSelectOption(els.slctDocumentName,newDocumentName);
      els.divAddDocument.style.display="none";
      els.btnDeleteDocument.style.display="inline";
      els.slctDocumentName.selectedIndex=els.slctDocumentName.options.length-1;
      return Concurrency.Bind(setDocument(),function()
      {
       return Concurrency.Bind(setPageButtons(),function()
       {
        show(List.ofArray([els.divAttachments]));
        return Concurrency.Bind(fillDocumentValues(),function()
        {
         return Concurrency.Return(null);
        });
       });
      });
     }));
    })),null);
   },false);
   els.slctHtmlPageTemplate.addEventListener("change",function()
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     return Concurrency.Bind(loadPageTemplate(),function()
     {
      return Concurrency.Bind(fillDocumentValues(),function()
      {
       return Concurrency.Return(null);
      });
     });
    })),null);
   },false);
   els.btnDownloadFilePage.addEventListener("click",function()
   {
    var chkReplaceVariables,m,x,filePage,b$1;
    chkReplaceVariables=Global.jQuery(els.chkReplaceVariables);
    m=(x=varDocument.c.pages,Seq.tryItem(getCurrentPageIndex()-1,x));
    return m==null?null:m.$0.$==0?null:(filePage=m.$0.$0,Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     var b$2;
     return Concurrency.Bind(chkReplaceVariables.prop("checked")?(new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.replaceVariables:-699540492",[filePage.path,varUserValues.c,varEmployer.c,varDocument.c]):(b$2=null,Concurrency.Delay(function()
     {
      return Concurrency.Return(filePage.path);
     })),function(a)
     {
      var fileName,extension;
      fileName=(extension=filePage.path.substring(filePage.path.lastIndexOf(".")+1),Strings.EndsWith(filePage.name,"."+extension)?filePage.name:filePage.name+"."+extension);
      return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.createLink:-1244178866",[a,fileName]),function(a$1)
      {
       Global.location.href=(function($1)
       {
        return function($2)
        {
         return $1("download/"+Utils.toSafe($2));
        };
       }(Global.id))(a$1);
       return Concurrency.Zero();
      });
     });
    })),null));
   },false);
   els.rbHtmlPage.addEventListener("click",function()
   {
    return show(List.ofArray([els.divAttachments,els.divChoosePageType,els.divCreateHtmlPage]));
   },false);
   els.rbFilePage.addEventListener("click",function()
   {
    return show(List.ofArray([els.divAttachments,els.divChoosePageType,els.divCreateFilePage]));
   },false);
   els.btnCreateHtmlPage.addEventListener("click",function()
   {
    var pageIndex,i,b$1;
    pageIndex=Global.jQuery(els.divAttachmentButtons).children("div").length;
    Var.Set(varDocument,(i=varDocument.c,Document.New(i.oId,i.name,List.append(varDocument.c.pages,List.ofArray([new DocumentPage({
     $:0,
     $0:HtmlPage.New(els.txtCreateHtmlPage.value,{
      $:1,
      $0:1
     },pageIndex,List.T.Empty)
    })])),i.email,i.jobName,i.customVariables)));
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     return Concurrency.Bind(setPageButtons(),function()
     {
      return Concurrency.Return(null);
     });
    })),null);
   },false);
   els.fileToUpload.addEventListener("change",function(ev)
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     var fileName,fileExtension,$1;
     fileName=String(ev.target.files.item(0).name);
     fileExtension=fileName.substring(fileName.lastIndexOf(".")+1);
     return ev.target.files.item(0).size>Types.maxUploadSize()?(Global.alert(Translation.t(Language.German,Word.FileIsTooBig)+"\n"+Strings.SFormat(Translation.t(Language.German,Word.UploadLimit),[String(Types.maxUploadSize()/1000000>>0)])),Concurrency.Zero()):!List.contains(fileExtension,Types.supportedUnoconvFileTypes())?(Global.alert(($1=[fileExtension],"Entschuldigung.\n*."+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0)))+" Dateien können zur Zeit nicht ins PDF-Format verwandelt werden.\r\n                                                    \nTypische Dateitypen zum Uploaden sind *.odt, *.doc, *.jpg oder *.pdf.")),Concurrency.Zero()):(ev.target.parentElement.submit(),Concurrency.Zero());
    })),null);
   },false);
   els.btnReadFromWebsite.addEventListener("click",function()
   {
    return Concurrency.Start(readFromWebsite(),null);
   },false);
   els.txtReadFromWebsite.addEventListener("paste",function()
   {
    return Concurrency.Start(readFromWebsite(),null);
   },false);
   els.txtReadFromWebsite.addEventListener("focus",function(ev)
   {
    return ev.target.select();
   },false);
   els.btnSetEmployerEmailToUserEmail.addEventListener("click",function()
   {
    return refEmployer.email.set_RVal(varUserEmail.c);
   },false);
   els.btnApplyNowTop.addEventListener("click",function()
   {
    return Concurrency.Start(btnApplyNowClicked(),null);
   },false);
   els.btnApplyNowBottom.addEventListener("click",function()
   {
    return Concurrency.Start(btnApplyNowClicked(),null);
   },false);
  }
  varDocument=Var.Create$1(Types.emptyDocument());
  refDocument=RefDocument.New(Var.Lens(varDocument,function(x)
  {
   return x.email.subject;
  },function(x,v)
  {
   return Document.New(x.oId,x.name,x.pages,DocumentEmail.New(v,x.email.body),x.jobName,x.customVariables);
  }),Var.Lens(varDocument,function(x)
  {
   return x.email.body;
  },function(x,v)
  {
   return Document.New(x.oId,x.name,x.pages,DocumentEmail.New(x.email.subject,v),x.jobName,x.customVariables);
  }),Var.Lens(varDocument,function(x)
  {
   return x.jobName;
  },function(x,v)
  {
   return Document.New(x.oId,x.name,x.pages,x.email,v,x.customVariables);
  }),Var.Lens(varDocument,function(x)
  {
   return x.customVariables;
  },function(x,v)
  {
   return Document.New(x.oId,x.name,x.pages,x.email,x.jobName,v);
  }));
  varUserValues=Var.Create$1(Types.emptyUserValues());
  refUserValues=RefUserValues.New(Var.Lens(varUserValues,function(x)
  {
   return x.gender;
  },function(x,v)
  {
   return UserValues.New(v,x.degree,x.firstName,x.lastName,x.street,x.postcode,x.city,x.phone,x.mobilePhone);
  }),Var.Lens(varUserValues,function(x)
  {
   return x.degree;
  },function(x,v)
  {
   return UserValues.New(x.gender,v,x.firstName,x.lastName,x.street,x.postcode,x.city,x.phone,x.mobilePhone);
  }),Var.Lens(varUserValues,function(x)
  {
   return x.firstName;
  },function(x,v)
  {
   return UserValues.New(x.gender,x.degree,v,x.lastName,x.street,x.postcode,x.city,x.phone,x.mobilePhone);
  }),Var.Lens(varUserValues,function(x)
  {
   return x.lastName;
  },function(x,v)
  {
   return UserValues.New(x.gender,x.degree,x.firstName,v,x.street,x.postcode,x.city,x.phone,x.mobilePhone);
  }),Var.Lens(varUserValues,function(x)
  {
   return x.street;
  },function(x,v)
  {
   return UserValues.New(x.gender,x.degree,x.firstName,x.lastName,v,x.postcode,x.city,x.phone,x.mobilePhone);
  }),Var.Lens(varUserValues,function(x)
  {
   return x.postcode;
  },function(x,v)
  {
   return UserValues.New(x.gender,x.degree,x.firstName,x.lastName,x.street,v,x.city,x.phone,x.mobilePhone);
  }),Var.Lens(varUserValues,function(x)
  {
   return x.city;
  },function(x,v)
  {
   return UserValues.New(x.gender,x.degree,x.firstName,x.lastName,x.street,x.postcode,v,x.phone,x.mobilePhone);
  }),Var.Lens(varUserValues,function(x)
  {
   return x.phone;
  },function(x,v)
  {
   return UserValues.New(x.gender,x.degree,x.firstName,x.lastName,x.street,x.postcode,x.city,v,x.mobilePhone);
  }),Var.Lens(varUserValues,function(x)
  {
   return x.mobilePhone;
  },function(x,v)
  {
   return UserValues.New(x.gender,x.degree,x.firstName,x.lastName,x.street,x.postcode,x.city,x.phone,v);
  }));
  varUserEmail=Var.CreateWaiting();
  varUserEmailInput=Var.CreateWaiting();
  varEmployer=Var.Create$1(Types.emptyEmployer());
  refEmployer=RefEmployer.New(Var.Lens(varEmployer,function(x)
  {
   return x.company;
  },function(x,v)
  {
   return Employer.New(v,x.street,x.postcode,x.city,x.gender,x.degree,x.firstName,x.lastName,x.email,x.phone,x.mobilePhone);
  }),Var.Lens(varEmployer,function(x)
  {
   return x.gender;
  },function(x,v)
  {
   return Employer.New(x.company,x.street,x.postcode,x.city,v,x.degree,x.firstName,x.lastName,x.email,x.phone,x.mobilePhone);
  }),Var.Lens(varEmployer,function(x)
  {
   return x.degree;
  },function(x,v)
  {
   return Employer.New(x.company,x.street,x.postcode,x.city,x.gender,v,x.firstName,x.lastName,x.email,x.phone,x.mobilePhone);
  }),Var.Lens(varEmployer,function(x)
  {
   return x.firstName;
  },function(x,v)
  {
   return Employer.New(x.company,x.street,x.postcode,x.city,x.gender,x.degree,v,x.lastName,x.email,x.phone,x.mobilePhone);
  }),Var.Lens(varEmployer,function(x)
  {
   return x.lastName;
  },function(x,v)
  {
   return Employer.New(x.company,x.street,x.postcode,x.city,x.gender,x.degree,x.firstName,v,x.email,x.phone,x.mobilePhone);
  }),Var.Lens(varEmployer,function(x)
  {
   return x.street;
  },function(x,v)
  {
   return Employer.New(x.company,v,x.postcode,x.city,x.gender,x.degree,x.firstName,x.lastName,x.email,x.phone,x.mobilePhone);
  }),Var.Lens(varEmployer,function(x)
  {
   return x.postcode;
  },function(x,v)
  {
   return Employer.New(x.company,x.street,v,x.city,x.gender,x.degree,x.firstName,x.lastName,x.email,x.phone,x.mobilePhone);
  }),Var.Lens(varEmployer,function(x)
  {
   return x.city;
  },function(x,v)
  {
   return Employer.New(x.company,x.street,x.postcode,v,x.gender,x.degree,x.firstName,x.lastName,x.email,x.phone,x.mobilePhone);
  }),Var.Lens(varEmployer,function(x)
  {
   return x.email;
  },function(x,v)
  {
   return Employer.New(x.company,x.street,x.postcode,x.city,x.gender,x.degree,x.firstName,x.lastName,v,x.phone,x.mobilePhone);
  }),Var.Lens(varEmployer,function(x)
  {
   return x.phone;
  },function(x,v)
  {
   return Employer.New(x.company,x.street,x.postcode,x.city,x.gender,x.degree,x.firstName,x.lastName,x.email,v,x.mobilePhone);
  }),Var.Lens(varEmployer,function(x)
  {
   return x.mobilePhone;
  },function(x,v)
  {
   return Employer.New(x.company,x.street,x.postcode,x.city,x.gender,x.degree,x.firstName,x.lastName,x.email,x.phone,v);
  }));
  varDisplayedDocument=Var.Create$1(Doc.Element("div",[],[]));
  Var.Create$1(Language.English);
  varDivSentApplications=Var.Create$1(Doc.Element("div",[],[]));
  els=JavaScriptElements.getEls();
  Concurrency.Start((b=null,Concurrency.Delay(function()
  {
   return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.isLoggedIn:-900658348",[]),function(a)
   {
    return Concurrency.Combine(!a?Concurrency.Bind(JobApplicationService.loginWithCookieOrAsGuest(),function()
    {
     Global.location.href="/";
     return Concurrency.Zero();
    }):Concurrency.Zero(),Concurrency.Delay(function()
    {
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.isLoggedInAsGuest:-900658348",[]),function(a$1)
     {
      return Concurrency.Combine(a$1?(Var.Set(varUserEmailInput,ClientHelpers.createInput("Deine Email",varUserEmail,function()
      {
       return"";
      })),Concurrency.Zero()):(Var.Set(varUserEmailInput,Doc.TextNode("")),Concurrency.Zero()),Concurrency.Delay(function()
      {
       Global.jQuery(Global.document).ready(function()
       {
        var b$1;
        return Concurrency.Start((b$1=null,Concurrency.Delay(function()
        {
         function addMenuEntry(entry,f)
         {
          var li,_this;
          li=Global.jQuery((function($1)
          {
           return function($2)
           {
            return $1("<li><button class=\"btnLikeLink1\">"+Utils.toSafe($2)+"</button></li>");
           };
          }(Global.id))(entry)).on("click",Runtime.CreateFuncWithThis(f));
          _this=Global.jQuery("#divSidebarMenu");
          return _this.append.apply(_this,[li]);
         }
         els=JavaScriptElements.getEls();
         addMenuEntry(Translation.t(Language.German,Word.SentApplications),function()
         {
          return function()
          {
           var b$2;
           return Concurrency.Start((b$2=null,Concurrency.Delay(function()
           {
            return Concurrency.Bind(getSentApplications(),function()
            {
             show(List.ofArray([els.divSentApplications]));
             return Concurrency.Zero();
            });
           })),null);
          };
         });
         addMenuEntry("Variablen",function()
         {
          return function()
          {
           return show(List.ofArray([els.divVariables]));
          };
         });
         addMenuEntry(Translation.t(Language.German,Word.EditYourValues),function()
         {
          return function()
          {
           return show(List.ofArray([els.divEditUserValues]));
          };
         });
         addMenuEntry(Translation.t(Language.German,Word.EditEmail),function()
         {
          return function()
          {
           return show(List.ofArray([els.divEmail]));
          };
         });
         addMenuEntry(Translation.t(Language.German,Word.EditAttachments),function()
         {
          return function()
          {
           return show(List.ofArray([els.divAttachments]));
          };
         });
         addMenuEntry(Translation.t(Language.German,Word.AddEmployerAndApply),function()
         {
          return function()
          {
           return show(List.ofArray([els.divAddEmployer]));
          };
         });
         return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getUserEmail:-644426752",[]),function(a$2)
         {
          Var.Set(varUserEmail,a$2==null?"":a$2.$0);
          return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getUserValues:-337599557",[]),function()
          {
           Var.Set(varUserValues,Types.emptyUserValues());
           return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getDocumentNames:1994133801",[]),function(a$3)
           {
            return Concurrency.Combine(Concurrency.For(a$3,function(a$4)
            {
             addSelectOption(els.slctDocumentName,a$4);
             return Concurrency.Zero();
            }),Concurrency.Delay(function()
            {
             return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getLastEditedDocumentOffset:-836782155",[]),function(a$4)
             {
              els.slctDocumentName.selectedIndex=a$4;
              return Concurrency.Bind(setDocument(),function()
              {
               return Concurrency.Bind(setPageButtons(),function()
               {
                return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getHtmlPageTemplates:1297380307",[]),function(a$5)
                {
                 return Concurrency.Combine(Concurrency.For(a$5,function(a$6)
                 {
                  addSelectOption(els.slctHtmlPageTemplate,a$6.name);
                  return Concurrency.Zero();
                 }),Concurrency.Delay(function()
                 {
                  registerEvents();
                  return Concurrency.Zero();
                 }));
                });
               });
              });
             });
            }));
           });
          });
         });
        })),null);
       });
       return Concurrency.Zero();
      }));
     });
    }));
   });
  })),null);
  return Doc.Element("div",[AttrProxy.Create("id","divJobApplicationContent")],[Doc.Element("div",[AttrProxy.Create("style","width : 100%")],[Doc.Element("h3",[],[Doc.TextNode(Translation.t(Language.German,Word.YourApplicationDocuments))]),Doc.Element("select",[AttrProxy.Create("id","slctDocumentName")],[]),Doc.Element("button",[AttrProxy.Create("type","button"),AttrProxy.Create("style","margin-left: 20px"),AttrProxy.Create("class",".btnLikeLink"),AttrProxy.Create("id","btnShowDivNewDocument")],[Doc.Element("i",[AttrProxy.Create("class","fa fa-plus-square"),AttrProxy.Create("aria-hidden","true")],[])]),Doc.Element("button",[AttrProxy.Create("type","button"),AttrProxy.Create("id","btnDeleteDocument"),AttrProxy.Create("class",".btnLikeLink"),AttrProxy.Create("style","margin-left: 20px")],[Doc.Element("i",[AttrProxy.Create("class","fa fa-trash"),AttrProxy.Create("aria-hidden","true")],[])])]),Doc.Element("hr",[],[]),Doc.Element("div",[AttrProxy.Create("id","divAttachments"),AttrProxy.Create("style","display: none")],[Doc.Element("h3",[],[Doc.TextNode(Translation.t(Language.German,Word.YourAttachments))]),Doc.Element("div",[AttrProxy.Create("id","divAttachmentButtons")],[Doc.Element("button",[AttrProxy.Create("id","btnAddPage"),AttrProxy.Create("style","margin:0;"),AttrProxy.Create("class","btnLikeLink")],[Doc.Element("i",[AttrProxy.Create("class","fa fa-plus-square"),AttrProxy.Create("aria-hidden","true")],[])])]),Doc.Element("br",[],[]),Doc.Element("hr",[],[]),Doc.Element("br",[],[])]),Doc.Element("div",[AttrProxy.Create("id","divAddDocument"),AttrProxy.Create("style","display: none")],[Doc.Element("br",[],[]),Doc.Element("h3",[],[Doc.TextNode(Translation.t(Language.German,Word.AddDocument))]),Doc.Element("br",[],[]),Doc.Element("label",[AttrProxy.Create("for","txtNewDocumentName")],[Doc.TextNode(Translation.t(Language.German,Word.DocumentName))]),Doc.Element("input",[AttrProxy.Create("id","txtNewDocumentName"),AttrProxy.Create("class","form-control")],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btnLikeLink"),AttrProxy.Create("id","btnAddDocument"),AttrProxy.Create("value",Translation.t(Language.German,Word.AddDocument))],[])]),Doc.Element("div",[AttrProxy.Create("id","divDisplayedDocument"),AttrProxy.Create("style","display: none")],[Doc.Element("select",[AttrProxy.Create("id","slctHtmlPageTemplate")],[]),Doc.EmbedView(varDisplayedDocument.v)]),Doc.Element("div",[AttrProxy.Create("id","divUploadedFileDownload"),AttrProxy.Create("style","display: none")],[Doc.Element("input",[AttrProxy.Create("type","checkbox"),AttrProxy.Create("value","false"),AttrProxy.Create("id","chkReplaceVariables")],[]),Doc.Element("label",[AttrProxy.Create("for","chkReplaceVariables")],[Doc.TextNode(Translation.t(Language.German,Word.ReplaceVariables))]),Doc.Element("br",[],[]),Doc.Element("button",[AttrProxy.Create("type","button"),AttrProxy.Create("id","btnDownloadFilePage")],[Doc.TextNode(Translation.t(Language.German,Word.Download))])]),Doc.Element("div",[AttrProxy.Create("id","divChoosePageType"),AttrProxy.Create("style","display: none")],[Doc.Element("input",[AttrProxy.Create("type","radio"),AttrProxy.Create("disabled","true"),AttrProxy.Create("name","rbgrpPageType"),AttrProxy.Create("id","rbHtmlPage")],[]),Doc.Element("label",[AttrProxy.Create("for","rbHtmlPage")],[Doc.TextNode(Translation.t(Language.German,Word.CreateOnline))]),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","radio"),AttrProxy.Create("id","rbFilePage"),AttrProxy.Create("name","rbgrpPageType")],[]),Doc.Element("label",[AttrProxy.Create("for","rbFilePage")],[Doc.TextNode(Translation.t(Language.German,Word.UploadFile))]),Doc.Element("br",[],[]),Doc.Element("br",[],[])]),Doc.Element("div",[AttrProxy.Create("id","divCreateHtmlPage"),AttrProxy.Create("style","display: none")],[Doc.Element("input",[AttrProxy.Create("id","txtCreateHtmlPage")],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("button",[AttrProxy.Create("type","submit"),AttrProxy.Create("id","btnCreateHtmlPage")],[Doc.TextNode(Translation.t(Language.German,Word.AddHtmlAttachment))])]),Doc.Element("div",[AttrProxy.Create("id","divCreateFilePage"),AttrProxy.Create("style","display: none")],[Doc.Element("form",[AttrProxy.Create("method","POST"),AttrProxy.Create("action","upload"),AttrProxy.Create("enctype","multipart/form-data")],[Doc.TextNode(Translation.t(Language.German,Word.PleaseChooseAFile)),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","file"),AttrProxy.Create("name","fileToUpload"),AttrProxy.Create("id","fileToUpload")],[]),Doc.Element("input",[AttrProxy.Create("type","hidden"),AttrProxy.Create("id","hiddenDocumentId"),AttrProxy.Create("name","documentId"),AttrProxy.Create("value","1")],[]),Doc.Element("input",[AttrProxy.Create("type","hidden"),AttrProxy.Create("id","hiddenNextPageIndex"),AttrProxy.Create("name","pageIndex"),AttrProxy.Create("value","1")],[])]),Doc.Element("br",[],[]),Doc.Element("hr",[],[])]),Doc.Element("div",[AttrProxy.Create("id","divSentApplications"),AttrProxy.Create("style","display: none")],[Doc.EmbedView(varDivSentApplications.v)]),Variables.getDivVariables(refDocument.customVariables),UserValues$1.getDivUserValues(refUserValues),Employer$1.getDivAddEmployer(refDocument.jobName,varUserEmailInput,refEmployer),Email.getDivEmail(refDocument.emailSubject,refDocument.emailBody)]);
 };
 Client$1.login=function()
 {
  return Doc.Element("div",[],[Doc.Element("form",[AttrProxy.Create("action","/ghi"),AttrProxy.Create("method","POST")],[Doc.Element("h3",[],[Doc.TextNode(Translation.t(Language.German,Word.Login))]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","txtLoginEmail")],[Doc.TextNode("Email")]),Doc.Element("input",[AttrProxy.Create("class","form-control"),AttrProxy.Create("name","txtLoginEmail"),AttrProxy.Create("id","txtLoginEmail")],[])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","txtLoginPassword")],[Doc.TextNode("Password")]),Doc.Element("input",[AttrProxy.Create("type","password"),AttrProxy.Create("class","form-control"),AttrProxy.Create("name","txtLoginPassword"),AttrProxy.Create("id","txtLoginPassword")],[])]),Doc.Element("input",[AttrProxy.Create("type","submit"),AttrProxy.Create("value","Login"),AttrProxy.Create("name","btnLogin")],[])])]);
 };
 Client$1.changePassword=function()
 {
  return Doc.Element("div",[],[Doc.Element("form",[AttrModule.Handler("submit",function()
  {
   return function()
   {
    var b;
    return Concurrency.Start((b=null,Concurrency.Delay(function()
    {
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.setPassword:-1276324058",[Global.document.getElementById("txtNewPassword").value]),function()
     {
      return Concurrency.Return(null);
     });
    })),null);
   };
  })],[Doc.Element("h3",[],[Doc.TextNode(Translation.t(Language.German,Word.ChangePassword))]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("b",[],[Doc.Element("label",[AttrProxy.Create("for","txtNewPassword")],[Doc.TextNode("Neues Password")])]),Doc.Element("div",[],[Doc.Element("input",[AttrProxy.Create("type","text"),AttrProxy.Create("class","form-control"),AttrProxy.Create("name","txtNewPassword"),AttrProxy.Create("id","txtNewPassword")],[]),Doc.Element("i",[AttrProxy.Create("class","fa fa-eye fa-2x"),AttrProxy.Create("aria-hidden","true"),AttrProxy.Create("style","float: right; position: relative; margin-top: -36px; margin-right: 15px"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return Client$1.togglePassword(Global.jQuery("#txtNewPassword"));
   };
  })],[])])]),Doc.Element("input",[AttrProxy.Create("type","submit"),AttrProxy.Create("value",Translation.t(Language.German,Word.ChangePassword))],[])])]);
 };
 Client$1.logout=function()
 {
  Cookies.expire("user");
  return Doc.Element("div",[],[]);
 };
 Client$1.loginOrOutButton=function()
 {
  var varIsGuest,b;
  varIsGuest=Var.Create$1(false);
  Concurrency.Start((b=null,Concurrency.Delay(function()
  {
   return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.isLoggedInAsGuest:-900658348",[]),function(a)
   {
    Var.Set(varIsGuest,a);
    return Concurrency.Zero();
   });
  })),null);
  return varIsGuest.c?Doc.Element("form",[AttrProxy.Create("action","/ghi")],[Doc.Element("button",[AttrProxy.Create("type","submit")],[Doc.TextNode("Login")])]):Doc.Element("form",[AttrProxy.Create("action","/logout")],[Doc.Element("button",[AttrProxy.Create("type","submit")],[Doc.TextNode("Logout")])]);
 };
 Client$1.togglePassword=function(el)
 {
  el.attr("type")==="password"?el.attr("type","text"):el.attr("type","password");
  el.next().toggleClass("fa-eye fa-eye-slash");
  el.focus();
 };
}());
