(function()
{
 "use strict";
 var Global,JobApplicationSpam,Types,Gender,Login,UserValues,Employer,JobApplicationPageAction,HtmlPage,FilePage,DocumentPage,DocumentEmail,Document,HtmlPageTemplate,PageDB,Language,Word,SC$1,Deutsch,SC$2,Client,SC$3,IntelliFactory,Runtime,WebSharper,Operators,String,List,Arrays,Math,Concurrency,Remoting,AjaxRemotingProvider,Date,UI,Next,Var,Doc,AttrProxy,Seq,Utils,AttrModule,System,Guid,Collections,Map,Strings,Unchecked,Enumerator;
 Global=window;
 JobApplicationSpam=Global.JobApplicationSpam=Global.JobApplicationSpam||{};
 Types=JobApplicationSpam.Types=JobApplicationSpam.Types||{};
 Gender=Types.Gender=Types.Gender||{};
 Login=Types.Login=Types.Login||{};
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
 Language=Types.Language=Types.Language||{};
 Word=Types.Word=Types.Word||{};
 SC$1=Global.StartupCode$JobApplicationSpam$Types=Global.StartupCode$JobApplicationSpam$Types||{};
 Deutsch=JobApplicationSpam.Deutsch=JobApplicationSpam.Deutsch||{};
 SC$2=Global.StartupCode$JobApplicationSpam$Deutsch=Global.StartupCode$JobApplicationSpam$Deutsch||{};
 Client=JobApplicationSpam.Client=JobApplicationSpam.Client||{};
 SC$3=Global.StartupCode$JobApplicationSpam$Client=Global.StartupCode$JobApplicationSpam$Client||{};
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 WebSharper=Global.WebSharper;
 Operators=WebSharper&&WebSharper.Operators;
 String=Global.String;
 List=WebSharper&&WebSharper.List;
 Arrays=WebSharper&&WebSharper.Arrays;
 Math=Global.Math;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Remoting=WebSharper&&WebSharper.Remoting;
 AjaxRemotingProvider=Remoting&&Remoting.AjaxRemotingProvider;
 Date=Global.Date;
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 Var=Next&&Next.Var;
 Doc=Next&&Next.Doc;
 AttrProxy=Next&&Next.AttrProxy;
 Seq=WebSharper&&WebSharper.Seq;
 Utils=WebSharper&&WebSharper.Utils;
 AttrModule=Next&&Next.AttrModule;
 System=Global.System;
 Guid=System&&System.Guid;
 Collections=WebSharper&&WebSharper.Collections;
 Map=Collections&&Collections.Map;
 Strings=WebSharper&&WebSharper.Strings;
 Unchecked=WebSharper&&WebSharper.Unchecked;
 Enumerator=WebSharper&&WebSharper.Enumerator;
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
 Login.New=function(email,password)
 {
  return{
   email:email,
   password:password
  };
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
 Document.New=function(id,name,pages,email,jobName)
 {
  return{
   id:id,
   name:name,
   pages:pages,
   email:email,
   jobName:jobName
  };
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
 Language=Types.Language=Runtime.Class({
  toString:function()
  {
   return this.$==1?"deutsch":"english";
  }
 },null,Language);
 Language.Deutsch=new Language({
  $:1
 });
 Language.English=new Language({
  $:0
 });
 Language.fromString=function(s)
 {
  var m;
  m=s.toLowerCase();
  return m==="english"?Language.English:m==="deutsch"?Language.Deutsch:Language.English;
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
  SC$1.emptyUserValues=UserValues.New(Gender.Unknown,"","","","","","","","");
  SC$1.emptyEmployer=Employer.New("","","","",Gender.Unknown,"","","","","","");
  SC$1.emptyDocument=Document.New(0,"",List.T.Empty,DocumentEmail.New("Bewerbung als $beruf",($1=[Types.newLine()],"Sehr $geehrter $chefAnrede $chefTitel $chefNachname,"+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0)))+(""+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+("anbei sende ich Ihnen meine Bewerbungsunterlagen."+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+("Über eine Einladung zu einem Bewerbungsgespräch würde ich mich sehr freuen."+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+(""+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+("Mit freundlichen Grüßen"+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+(""+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+("$meinTitel $meinVorname $meinNachname"+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+("$meineStrasse"+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+("$meinePlz $meineStadt"+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+("Telefon: $meineTelefonnr"+(Arrays.get($1,0)==null?"":String(Arrays.get($1,0))))+"Mobil: $meineMobilnr")),"");
 };
 Deutsch.dict=function()
 {
  SC$2.$cctor();
  return SC$2.dict;
 };
 SC$2.$cctor=function()
 {
  SC$2.$cctor=Global.ignore;
  SC$2.dict=List.ofArray([[Word.AddEmployerAndApply,"Bewerben"],[Word.EditYourValues,"Deine Daten"],[Word.EditEmail,"Email"],[Word.EditAttachments,"Anhänge"],[Word.YourApplicationDocuments,"Deine Bewerbungsmappen"],[Word.LoadFromWebsite,"Werte holen"],[Word.ApplyNow,"Jetzt bewerben"],[Word.CompanyName,"Firmenname"],[Word.Street,"Straße"],[Word.Postcode,"Postleitzahl"],[Word.City,"Stadt"],[Word.Gender,"Geschlecht"],[Word.Degree,"Titel"],[Word.FirstName,"Vorname"],[Word.LastName,"Nachname"],[Word.Email,"Email"],[Word.Phone,"Telefonnummer"],[Word.MobilePhone,"Mobilnummer"],[Word.YourValues,"Deine Daten"],[Word.EmailSubject,"Betreff"],[Word.EmailBody,"Text"],[Word.YourAttachments,"Anhänge"],[Word.CreateOnline,"Online erstellen"],[Word.UploadFile,"Datei hochladen"],[Word.PleaseChooseAFile,"Bitte eine Datei aussuchen"],[Word.AddAttachment,"Anhang hinzufügen"],[Word.YouMightWantToReplaceSomeWordsInYourFileWithVariables,"Du kannst Worte oder Phrasen in deiner Datei durch Variablen ersetzen."],[Word.VariablesWillBeReplacedWithTheRightValuesEveryTimeYouSendYourApplication,"Jedesmal, wenn du eine Bewerbung versendest, werden die Variablen automatisch durch die richtigen Werte ersetzt."],[Word.Male,"männlich"],[Word.Female,"weiblich"],[Word.UnknownGender,"unbekannt"],[Word.AddDocument,"Bewerbungsmappe hinzufügen"],[Word.DocumentName,"Name der Bewerbungsmappe"],[Word.AddHtmlAttachment,"Html Anhang hinzufügen"],[Word.Employer,"Arbeitgeber"],[Word.Download,"downloaden"],[Word.ReallyDeleteDocument,"Document \"{0}\" wirklich löschen?"],[Word.ReallyDeletePage,"Seite \"{0}\" wirklich löschen?"],[Word.WeHaveSentYouAnEmail,"Wir haben dir eine Email geschickt."],[Word.PleaseConfirmYourEmailAddressEmailSubject,"Bitte bestätige deine Email-Adresse"],[Word.PleaseConfirmYourEmailAddressEmailBody,"Hallo!\n\nBitte besuche den folgenden Link, um deine Email-Adresse zu bestätigen.\nhttp://bewerbungsspam.de/confirmemail?email={0}&guid={1}\n\nDein Team von www.bewerbungsspam.de"],[Word.Login,"Einloggen"],[Word.Register,"Registrieren"],[Word.SentApplications,"Versandte Bewerbungen"],[Word.JobName,"Bewerben als"],[Word.AppliedAs,"Beworben als"],[Word.AppliedOnDate,"Beworben am"],[Word.TheEmailOfYourEmployerDoesNotLookValid,"Die Email-Adresse des Arbeitgeber scheint fehlerhaft zu sein."],[Word.FieldIsRequired,"Feld \"{0}\" darf nicht leer sein."],[Word.SorryAnErrorOccurred,"Entschuldigung, es ist ein Fehler aufgetreten."],[Word.YourApplicationHasNotBeenSent,"Deine Bewerbung konnte nicht versendet werden :-("],[Word.FileIsTooBig,"Die ausgewählte Datei ist zu groß."],[Word.UploadLimit,"Die maximale Dateigröße beträgt {0} MB."],[Word.ReplaceVariables,"Variablen ersetzen"]]);
 };
 Client.templates=function()
 {
  var varDocument,documentEmailSubject,documentEmailBody,documentJobName,varUserValues,userGender,userDegree,userFirstName,userLastName,userStreet,userPostcode,userCity,userPhone,userMobilePhone,varUserEmail,varEmployer,employerCompany,employerGender,employerDegree,employerFirstName,employerLastName,employerStreet,employerPostcode,employerCity,employerEmail,employerPhone,employerMobilePhone,varDisplayedDocument,varDivSentApplications,showHideMutualElements,b,labelText,guid,c;
  function getCurrentPageIndex()
  {
   return Math.max(Global.jQuery("#divAttachmentButtons").find(".mainButton").index(Global.jQuery(".active"))+1,1);
  }
  function getSentApplications()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getSentApplications:494885027",[Date.now(),Date.now()]),function(a)
    {
     Var.Set(varDivSentApplications,Doc.Element("div",[AttrProxy.Create("style","width: 100%; height: 100%; overflow: auto")],[Doc.Element("table",[AttrProxy.Create("style","border-spacing: 10px; border-collapse: separate")],[Doc.Element("thead",[],[Doc.Element("tr",[],[Doc.Element("th",[],[Doc.TextNode(Client.t(Word.CompanyName))]),Doc.Element("th",[],[Doc.TextNode(Client.t(Word.AppliedOnDate))]),Doc.Element("th",[],[Doc.TextNode(Client.t(Word.AppliedAs))]),Doc.Element("th",[],[Doc.TextNode("Url")])])]),Doc.Element("tbody",[],List.ofSeq(Seq.delay(function()
     {
      function emailSentApplicationToUserFun(el,ev)
      {
       var b$2;
       return Concurrency.Start((b$2=null,Concurrency.Delay(function()
       {
        return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.emailSentApplicationToUser:2130902632",[el.parentElement.parentElement.rowIndex-1]),function(a$1)
        {
         return a$1.$==1?(Global.alert("Entschuldigung, es trat ein Fehler auf"),Concurrency.Zero()):Concurrency.Zero();
        });
       })),null);
      }
      return Seq.collect(function(m)
      {
       var appliedOn;
       appliedOn=m[2];
       return List.ofArray([Doc.Element("tr",[],[Doc.Element("td",[],[Doc.TextNode(m[0])]),Doc.Element("td",[],[Doc.TextNode(((((Runtime.Curried(function($1,$2,$3,$4)
       {
        return $1(Utils.padNumLeft(String($2),2)+"."+Utils.padNumLeft(String($3),2)+"."+Utils.padNumLeft(String($4),4));
       },4))(Global.id))((new Date(appliedOn)).getDate()))((new Date(appliedOn)).getMonth()+1))((new Date(appliedOn)).getFullYear()))]),Doc.Element("td",[],[Doc.TextNode(m[1])]),Doc.Element("td",[],[Doc.TextNode(m[3])]),Doc.Element("td",[],[Doc.Element("button",[AttrModule.Handler("click",function($1)
       {
        return function($2)
        {
         return emailSentApplicationToUserFun($1,$2);
        };
       })],[Doc.Element("i",[AttrProxy.Create("class","fa fa-envelope"),AttrProxy.Create("aria-hidden","true")],[])])])])]);
      },a);
     })))])]));
     return Concurrency.Zero();
    });
   });
  }
  function createInputWithColumnSizes1(column1Size,column2Size,labelText$1,ref,validFun)
  {
   var guid$1,c$1;
   guid$1=(c$1=Guid.NewGuid(),Guid.ToString(c$1,"N"));
   return Doc.Element("div",[AttrProxy.Create("class","form-group row")],[Doc.Element("label",[AttrProxy.Create("class",column1Size+" col-form-label"),AttrProxy.Create("for",guid$1)],[Doc.TextNode(labelText$1)]),Doc.Element("div",[AttrProxy.Create("class",column2Size)],[Doc.Input([AttrProxy.Create("id",guid$1),AttrProxy.Create("class","form-control"),AttrProxy.Create("type","text"),AttrModule.Handler("blur",function(el)
   {
    return function()
    {
     validFun(el.value);
     return null;
    };
   })],ref)]),Doc.Element("div",[AttrProxy.Create("class",column1Size),AttrProxy.Create("style","display: none")],[Doc.Element("small",[AttrProxy.Create("class","text-danger")],[Doc.TextNode("Must be 8-20 characters")])])]);
  }
  function createRadioWithColumnSizes1(column1Size,column2Size,labelText$1,radioValuesList)
  {
   var radioGroup,c$1;
   radioGroup=(c$1=Guid.NewGuid(),Guid.ToString(c$1,"N"));
   return Doc.Element("div",[],List.mapi(function(i,t)
   {
    var guid$1,c$2;
    guid$1=(c$2=Guid.NewGuid(),Guid.ToString(c$2,"N"));
    return Doc.Element("div",[AttrProxy.Create("class","form-group row")],[Doc.Element("label",[AttrProxy.Create("class",column1Size+" col-form-label")],[Doc.TextNode(i===0?labelText$1:"")]),Doc.Element("div",[AttrProxy.Create("class",column2Size)],[Doc.Radio([AttrProxy.Create("id",guid$1),AttrProxy.Create("type","radio"),AttrProxy.Create("name",radioGroup),AttrProxy.Create("checked",t[3])],t[1],t[2]),Doc.Element("label",[AttrProxy.Create("for",guid$1)],[Doc.TextNode(t[0])])])]);
   },radioValuesList));
  }
  function createInput($1,$2,$3)
  {
   return createInputWithColumnSizes1("col-lg-3","col-lg-9",$1,$2,$3);
  }
  function createRadio($1,$2)
  {
   return createRadioWithColumnSizes1("col-lg-3","col-lg-9",$1,$2);
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
      $0:userGender
     }],["userDegree",{
      $:0,
      $0:userDegree
     }],["userFirstName",{
      $:0,
      $0:userFirstName
     }],["userLastName",{
      $:0,
      $0:userLastName
     }],["userStreet",{
      $:0,
      $0:userStreet
     }],["userPostcode",{
      $:0,
      $0:userPostcode
     }],["userCity",{
      $:0,
      $0:userCity
     }],["userPhone",{
      $:0,
      $0:userPhone
     }],["userMobilePhone",{
      $:0,
      $0:userMobilePhone
     }],["employerCompany",{
      $:0,
      $0:employerCompany
     }],["employerStreet",{
      $:0,
      $0:employerStreet
     }],["employerPostcode",{
      $:0,
      $0:employerPostcode
     }],["employerCity",{
      $:0,
      $0:employerCity
     }],["employerDegree",{
      $:0,
      $0:employerDegree
     }],["employerFirstName",{
      $:0,
      $0:employerFirstName
     }],["employerLastName",{
      $:0,
      $0:employerLastName
     }],["employerEmail",{
      $:0,
      $0:employerEmail
     }],["employerPhone",{
      $:0,
      $0:employerPhone
     }],["employerMobilePhone",{
      $:0,
      $0:employerMobilePhone
     }],["documentEmailSubject",{
      $:0,
      $0:documentEmailSubject
     }],["documentEmailBody",{
      $:0,
      $0:documentEmailBody
     }],["documentJobName",{
      $:0,
      $0:documentJobName
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
     pageTemplateIndex=Global.document.getElementById("slctHtmlPageTemplate").selectedIndex;
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getHtmlPageTemplate:-1212795141",[pageTemplateIndex+1]),function(a)
     {
      Var.Set(varDisplayedDocument,Doc.Verbatim(a));
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
         Var.Set(varDocument,(i$1=varDocument.c,Document.New(i$1.id,i$1.name,List.append(p[0],new List.T({
          $:1,
          $0:p[1],
          $1:p[2]
         })),i$1.email,i$1.jobName)));
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
  function show(elIds)
  {
   var e,e$1,hideElId;
   e=Enumerator.Get(elIds);
   try
   {
    while(e.MoveNext())
     Global.document.getElementById(e.Current()).style.display="block";
   }
   finally
   {
    if("Dispose"in e)
     e.Dispose();
   }
   e$1=Enumerator.Get(showHideMutualElements);
   try
   {
    while(e$1.MoveNext())
     {
      hideElId=e$1.Current();
      !List.contains(hideElId,elIds)?Global.document.getElementById(hideElId).style.display="none":void 0;
     }
   }
   finally
   {
    if("Dispose"in e$1)
     e$1.Dispose();
   }
  }
  function setDocument()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    var slctDocumentNameEl;
    slctDocumentNameEl=Global.document.getElementById("slctDocumentName");
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getDocumentOffset:-1633111335",[slctDocumentNameEl.selectedIndex]),function(a)
    {
     return a==null?(Var.Set(varDocument,Types.emptyDocument()),Global.document.getElementById("btnAddPage").style.visibility="hidden",Concurrency.Zero()):(Var.Set(varDocument,a.$0),Global.document.getElementById("hiddenDocumentId").value=String(varDocument.c.id),Global.document.getElementById("btnAddPage").style.visibility="visible",Concurrency.Zero());
    });
   });
  }
  function setPageButtons()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    Global.jQuery("#divAttachmentButtons").children("div").remove();
    return Concurrency.Combine(Concurrency.For(varDocument.c.pages,function(a)
    {
     var deleteButton,pageUpButton,pageDownButton,mainButton,firstDiv,_this,secondDiv,_this$1,_this$2,_this$3,a$1,a$2,_this$4,_this$5;
     deleteButton=Global.jQuery("<button class=\"distanced\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></button>").on("click",function()
     {
      var i,t,x,before,after,b$2;
      return Global.confirm(Strings.SFormat(Client.t(Word.ReallyDeletePage),[a.Name()]))?(Var.Set(varDocument,(i=varDocument.c,Document.New(i.id,i.name,(t=(x=varDocument.c.pages,List.splitAt(a.PageIndex$1()-1,x)),(before=t[0],(after=t[1],after.$==0?before:List.append(before,List.map(function(p)
      {
       return p.PageIndex(p.PageIndex$1()-1);
      },after.$1))))),i.email,i.jobName))),Concurrency.Start((b$2=null,Concurrency.Delay(function()
      {
       return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.overwriteDocument:-229128764",[varDocument.c]),function()
       {
        return Concurrency.Bind(setDocument(),function()
        {
         return Concurrency.Bind(setPageButtons(),function()
         {
          show(List.ofArray(["divAttachments"]));
          return Concurrency.Zero();
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
       Var.Set(varDocument,(i=varDocument.c,Document.New(i.id,i.name,(t=(x=varDocument.c.pages,List.splitAt(a.PageIndex$1()-2,x)),(before=t[0],(after=t[1],after.$==0?before:after.$1.$==0?List.append(before,List.ofArray([after.$0])):(x1=after.$0,(x2=after.$1.$0,List.append(before,new List.T({
        $:1,
        $0:x2.PageIndex(x1.PageIndex$1()),
        $1:new List.T({
         $:1,
         $0:x1.PageIndex(x2.PageIndex$1()),
         $1:after.$1.$1
        })
       }))))))),i.email,i.jobName)));
       return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.overwriteDocument:-229128764",[varDocument.c]),function()
       {
        return Concurrency.Bind(setDocument(),function()
        {
         return Concurrency.Bind(setPageButtons(),function()
         {
          show(List.ofArray(["divAttachments"]));
          return Concurrency.Zero();
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
       Var.Set(varDocument,(i=varDocument.c,Document.New(i.id,i.name,(t=(x=varDocument.c.pages,List.splitAt(a.PageIndex$1()-1,x)),(before=t[0],(after=t[1],after.$==0?before:after.$1.$==0?List.append(before,List.ofArray([after.$0])):(x1=after.$0,(x2=after.$1.$0,List.append(before,new List.T({
        $:1,
        $0:x2.PageIndex(x1.PageIndex$1()),
        $1:new List.T({
         $:1,
         $0:x1.PageIndex(x2.PageIndex$1()),
         $1:after.$1.$1
        })
       }))))))),i.email,i.jobName)));
       return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.overwriteDocument:-229128764",[varDocument.c]),function()
       {
        return Concurrency.Bind(setDocument(),function()
        {
         return Concurrency.Bind(setPageButtons(),function()
         {
          show(List.ofArray(["divAttachments"]));
          return Concurrency.Zero();
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
      return a.$==1?show(List.ofArray(["divAttachments","divUploadedFileDownload"])):(htmlPage=a.$0,Concurrency.Start((b$2=null,Concurrency.Delay(function()
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
        return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getHtmlPageTemplate:-1212795141",[(o=htmlPage.oTemplateId,o==null?1:o.$0)]),function(a$3)
        {
         Var.Set(varDisplayedDocument,Doc.Verbatim(a$3));
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
           show(List.ofArray(["divDisplayedDocument","divAttachments"]));
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
     Global.document.getElementById("hiddenNextPageIndex").value=String(Global.jQuery("#divAttachmentButtons").children("div").length+1);
     return Concurrency.Zero();
    }));
   });
  }
  function addSelectOption(el,value)
  {
   var optionEl;
   optionEl=Global.document.createElement("option");
   optionEl.textContent=value;
   return el.add(optionEl);
  }
  function btnApplyNowClicked()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    return!(new Global.RegExp("^(([^<>()\\[\\]\\\\.,;:\\s@\"]+(\\.[^<>()\\[\\]\\\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$")).test(employerEmail.RVal())?(Global.alert(Client.t(Word.TheEmailOfYourEmployerDoesNotLookValid)),Concurrency.Zero()):Strings.Trim(documentJobName.RVal())===""?(Global.alert(Strings.SFormat(Client.t(Word.FieldIsRequired),[Client.t(Word.JobName)])),Concurrency.Zero()):Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.tryFindSentApplication:234068265",[varEmployer.c]),function(a)
    {
     var btnLoadFromWebsite,fontAwesomeEls;
     return a==null||a!=null&&Global.confirm("Du hast dich schon einmal bei einer Firma mit diesem Firmennamen beworben.\nBewerbung trotzdem abschicken?")?(btnLoadFromWebsite=Global.jQuery("#btnLoadFromWebsite"),(fontAwesomeEls=List.ofArray([Global.jQuery("#faBtnApplyNowBottom"),Global.jQuery("#faBtnApplyNowTop")]),(List.iter(function(faEl)
     {
      faEl.css("color","black");
      faEl.addClass("fa-spinner fa-spin");
     },fontAwesomeEls),btnLoadFromWebsite.prop("disabled",true),Global.jQuery("#divJobApplicationContent").find("input,textarea,button,select").prop("disabled",true),Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.applyNow:-2017393559",[varEmployer.c,varDocument.c,varUserValues.c,Global.document.getElementById("txtReadEmployerFromWebsite").value]),function(a$1)
     {
      List.iter(function(faEl)
      {
       faEl.removeClass("fa-spinner fa-spin");
      },fontAwesomeEls);
      btnLoadFromWebsite.prop("disabled",false);
      Global.jQuery("#divJobApplicationContent").find("input,textarea,button,select").prop("disabled",false);
      return a$1.$==0?(List.iter(function(faEl)
      {
       faEl.css("color","#08a81b");
       faEl.addClass("fa-check");
      },fontAwesomeEls),Var.Set(varEmployer,Types.emptyEmployer()),Global.document.getElementById("txtReadEmployerFromWebsite").value="",Concurrency.Bind(Concurrency.Sleep(4500),function()
      {
       List.iter(function(faEl)
       {
        faEl.removeClass("fa-check");
       },fontAwesomeEls);
       return Concurrency.Zero();
      })):Concurrency.Bind(Concurrency.Sleep(700),function()
      {
       Global.alert(Client.t(Word.SorryAnErrorOccurred)+"\n"+Client.t(Word.YourApplicationHasNotBeenSent));
       return Concurrency.Zero();
      });
     })))):Concurrency.Zero();
    });
   });
  }
  function readFromWebsite()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    Global.document.getElementById("btnReadFromWebsite").disabled=true;
    Global.document.getElementById("faReadFromWebsite").style.visibility="visible";
    return Concurrency.Bind(Concurrency.Sleep(200),function()
    {
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.readWebsite:1243289988",[Global.document.getElementById("txtReadEmployerFromWebsite").value]),function(a)
     {
      Global.document.getElementById("btnReadFromWebsite").disabled=false;
      Global.document.getElementById("faReadFromWebsite").style.visibility="hidden";
      return a.$==1?(Global.alert(Seq.fold(function($1,$2)
      {
       return $1+$2+"\n";
      },"",a.$0)),Concurrency.Zero()):(Var.Set(varEmployer,a.$0),Concurrency.Zero());
     });
    });
   });
  }
  varDocument=Var.Create$1(Types.emptyDocument());
  documentEmailSubject=Var.Lens(varDocument,function(x)
  {
   return x.email.subject;
  },function(x,v)
  {
   return Document.New(x.id,x.name,x.pages,DocumentEmail.New(v,x.email.body),x.jobName);
  });
  documentEmailBody=Var.Lens(varDocument,function(x)
  {
   return x.email.body;
  },function(x,v)
  {
   return Document.New(x.id,x.name,x.pages,DocumentEmail.New(x.email.subject,v),x.jobName);
  });
  documentJobName=Var.Lens(varDocument,function(x)
  {
   return x.jobName;
  },function(x,v)
  {
   return Document.New(x.id,x.name,x.pages,x.email,v);
  });
  varUserValues=Var.Create$1(Types.emptyUserValues());
  userGender=Var.Lens(varUserValues,function(x)
  {
   return x.gender;
  },function(x,v)
  {
   return UserValues.New(v,x.degree,x.firstName,x.lastName,x.street,x.postcode,x.city,x.phone,x.mobilePhone);
  });
  userDegree=Var.Lens(varUserValues,function(x)
  {
   return x.degree;
  },function(x,v)
  {
   return UserValues.New(x.gender,v,x.firstName,x.lastName,x.street,x.postcode,x.city,x.phone,x.mobilePhone);
  });
  userFirstName=Var.Lens(varUserValues,function(x)
  {
   return x.firstName;
  },function(x,v)
  {
   return UserValues.New(x.gender,x.degree,v,x.lastName,x.street,x.postcode,x.city,x.phone,x.mobilePhone);
  });
  userLastName=Var.Lens(varUserValues,function(x)
  {
   return x.lastName;
  },function(x,v)
  {
   return UserValues.New(x.gender,x.degree,x.firstName,v,x.street,x.postcode,x.city,x.phone,x.mobilePhone);
  });
  userStreet=Var.Lens(varUserValues,function(x)
  {
   return x.street;
  },function(x,v)
  {
   return UserValues.New(x.gender,x.degree,x.firstName,x.lastName,v,x.postcode,x.city,x.phone,x.mobilePhone);
  });
  userPostcode=Var.Lens(varUserValues,function(x)
  {
   return x.postcode;
  },function(x,v)
  {
   return UserValues.New(x.gender,x.degree,x.firstName,x.lastName,x.street,v,x.city,x.phone,x.mobilePhone);
  });
  userCity=Var.Lens(varUserValues,function(x)
  {
   return x.city;
  },function(x,v)
  {
   return UserValues.New(x.gender,x.degree,x.firstName,x.lastName,x.street,x.postcode,v,x.phone,x.mobilePhone);
  });
  userPhone=Var.Lens(varUserValues,function(x)
  {
   return x.phone;
  },function(x,v)
  {
   return UserValues.New(x.gender,x.degree,x.firstName,x.lastName,x.street,x.postcode,x.city,v,x.mobilePhone);
  });
  userMobilePhone=Var.Lens(varUserValues,function(x)
  {
   return x.mobilePhone;
  },function(x,v)
  {
   return UserValues.New(x.gender,x.degree,x.firstName,x.lastName,x.street,x.postcode,x.city,x.phone,v);
  });
  varUserEmail=Var.CreateWaiting();
  varEmployer=Var.Create$1(Types.emptyEmployer());
  employerCompany=Var.Lens(varEmployer,function(x)
  {
   return x.company;
  },function(x,v)
  {
   return Employer.New(v,x.street,x.postcode,x.city,x.gender,x.degree,x.firstName,x.lastName,x.email,x.phone,x.mobilePhone);
  });
  employerGender=Var.Lens(varEmployer,function(x)
  {
   return x.gender;
  },function(x,v)
  {
   return Employer.New(x.company,x.street,x.postcode,x.city,v,x.degree,x.firstName,x.lastName,x.email,x.phone,x.mobilePhone);
  });
  employerDegree=Var.Lens(varEmployer,function(x)
  {
   return x.degree;
  },function(x,v)
  {
   return Employer.New(x.company,x.street,x.postcode,x.city,x.gender,v,x.firstName,x.lastName,x.email,x.phone,x.mobilePhone);
  });
  employerFirstName=Var.Lens(varEmployer,function(x)
  {
   return x.firstName;
  },function(x,v)
  {
   return Employer.New(x.company,x.street,x.postcode,x.city,x.gender,x.degree,v,x.lastName,x.email,x.phone,x.mobilePhone);
  });
  employerLastName=Var.Lens(varEmployer,function(x)
  {
   return x.lastName;
  },function(x,v)
  {
   return Employer.New(x.company,x.street,x.postcode,x.city,x.gender,x.degree,x.firstName,v,x.email,x.phone,x.mobilePhone);
  });
  employerStreet=Var.Lens(varEmployer,function(x)
  {
   return x.street;
  },function(x,v)
  {
   return Employer.New(x.company,v,x.postcode,x.city,x.gender,x.degree,x.firstName,x.lastName,x.email,x.phone,x.mobilePhone);
  });
  employerPostcode=Var.Lens(varEmployer,function(x)
  {
   return x.postcode;
  },function(x,v)
  {
   return Employer.New(x.company,x.street,v,x.city,x.gender,x.degree,x.firstName,x.lastName,x.email,x.phone,x.mobilePhone);
  });
  employerCity=Var.Lens(varEmployer,function(x)
  {
   return x.city;
  },function(x,v)
  {
   return Employer.New(x.company,x.street,x.postcode,v,x.gender,x.degree,x.firstName,x.lastName,x.email,x.phone,x.mobilePhone);
  });
  employerEmail=Var.Lens(varEmployer,function(x)
  {
   return x.email;
  },function(x,v)
  {
   return Employer.New(x.company,x.street,x.postcode,x.city,x.gender,x.degree,x.firstName,x.lastName,v,x.phone,x.mobilePhone);
  });
  employerPhone=Var.Lens(varEmployer,function(x)
  {
   return x.phone;
  },function(x,v)
  {
   return Employer.New(x.company,x.street,x.postcode,x.city,x.gender,x.degree,x.firstName,x.lastName,x.email,v,x.mobilePhone);
  });
  employerMobilePhone=Var.Lens(varEmployer,function(x)
  {
   return x.mobilePhone;
  },function(x,v)
  {
   return Employer.New(x.company,x.street,x.postcode,x.city,x.gender,x.degree,x.firstName,x.lastName,x.email,x.phone,v);
  });
  varDisplayedDocument=Var.Create$1(Doc.Element("div",[],[]));
  Var.Create$1(Language.English);
  varDivSentApplications=Var.Create$1(Doc.Element("div",[],[]));
  showHideMutualElements=List.ofArray(["divCreateFilePage","divCreateHtmlPage","divChoosePageType","divEmail","divNewDocument","divEditUserValues","divAddEmployer","divDisplayedDocument","divAttachments","divUploadedFileDownload","divSentApplications"]);
  Concurrency.Start((b=null,Concurrency.Delay(function()
  {
   var divMenu;
   Var.Set(Client.varLanguageDict(),Map.OfArray(Arrays.ofSeq(Deutsch.dict())));
   divMenu=Global.document.getElementById("divSidebarMenu");
   return Concurrency.Combine(Concurrency.While(function()
   {
    return Unchecked.Equals(divMenu,null);
   },Concurrency.Delay(function()
   {
    return Concurrency.Bind(Concurrency.Sleep(10),function()
    {
     return Concurrency.Return(null);
    });
   })),Concurrency.Delay(function()
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
     _this=Global.jQuery(divMenu);
     return _this.append.apply(_this,[li]);
    }
    addMenuEntry(Client.t(Word.SentApplications),function()
    {
     return function()
     {
      var b$1;
      return Concurrency.Start((b$1=null,Concurrency.Delay(function()
      {
       return Concurrency.Bind(getSentApplications(),function()
       {
        show(List.ofArray(["divSentApplications"]));
        return Concurrency.Zero();
       });
      })),null);
     };
    });
    addMenuEntry(Client.t(Word.EditYourValues),function()
    {
     return function()
     {
      return show(List.ofArray(["divEditUserValues"]));
     };
    });
    addMenuEntry(Client.t(Word.EditEmail),function()
    {
     return function()
     {
      return show(List.ofArray(["divEmail"]));
     };
    });
    addMenuEntry(Client.t(Word.EditAttachments),function()
    {
     return function()
     {
      return show(List.ofArray(["divAttachments"]));
     };
    });
    addMenuEntry(Client.t(Word.AddEmployerAndApply),function()
    {
     return function()
     {
      return show(List.ofArray(["divAddEmployer"]));
     };
    });
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getCurrentUserEmail:-834772631",[]),function(a)
    {
     Var.Set(varUserEmail,a);
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getCurrentUserValues:-337599557",[]),function(a$1)
     {
      Var.Set(varUserValues,a$1);
      return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getDocumentNames:1994133801",[]),function(a$2)
      {
       var slctDocumentNameEl;
       slctDocumentNameEl=Global.document.getElementById("slctDocumentName");
       return Concurrency.Combine(Concurrency.While(function()
       {
        return Unchecked.Equals(Global.document.getElementById("slctDocumentName"),null);
       },Concurrency.Delay(function()
       {
        return Concurrency.Bind(Concurrency.Sleep(10),function()
        {
         return Concurrency.Return(null);
        });
       })),Concurrency.Delay(function()
       {
        return Concurrency.Combine(Concurrency.For(a$2,function(a$3)
        {
         addSelectOption(slctDocumentNameEl,a$3);
         return Concurrency.Zero();
        }),Concurrency.Delay(function()
        {
         return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getLastEditedDocumentOffset:-836782155",[]),function(a$3)
         {
          slctDocumentNameEl.selectedIndex=a$3;
          return Concurrency.Bind(setDocument(),function()
          {
           return Concurrency.Bind(setPageButtons(),function()
           {
            return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getHtmlPageTemplates:1297380307",[]),function(a$4)
            {
             var slctHtmlPageTemplateEl;
             slctHtmlPageTemplateEl=Global.document.getElementById("slctHtmlPageTemplate");
             return Concurrency.Combine(Concurrency.While(function()
             {
              return Unchecked.Equals(slctHtmlPageTemplateEl,null);
             },Concurrency.Delay(function()
             {
              return Concurrency.Bind(Concurrency.Sleep(10),function()
              {
               return Concurrency.Return(null);
              });
             })),Concurrency.Delay(function()
             {
              return Concurrency.Combine(Concurrency.For(a$4,function(a$5)
              {
               addSelectOption(slctHtmlPageTemplateEl,a$5.name);
               return Concurrency.Zero();
              }),Concurrency.Delay(function()
              {
               show(List.ofArray(["divAttachments"]));
               return Concurrency.Zero();
              }));
             }));
            });
           });
          });
         });
        }));
       }));
      });
     });
    });
   }));
  })),null);
  return Doc.Element("div",[AttrProxy.Create("id","divJobApplicationContent")],[Doc.Element("div",[AttrProxy.Create("style","width : 100%")],[Doc.Element("h4",[],[Doc.TextNode(Client.t(Word.YourApplicationDocuments))]),Doc.Element("select",[AttrProxy.Create("id","slctDocumentName"),AttrModule.Handler("change",function()
  {
   return function()
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     show(List.ofArray(["divAttachments"]));
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
   };
  })],[]),Doc.Element("button",[AttrProxy.Create("type","button"),AttrProxy.Create("style","margin-left: 20px"),AttrProxy.Create("class",".btnLikeLink"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return show(List.ofArray(["divNewDocument"]));
   };
  })],[Doc.Element("i",[AttrProxy.Create("class","fa fa-plus-square"),AttrProxy.Create("aria-hidden","true")],[])]),Doc.Element("button",[AttrProxy.Create("type","button"),AttrProxy.Create("id","btnDeleteDocument"),AttrProxy.Create("class",".btnLikeLink"),AttrProxy.Create("style","margin-left: 20px"),AttrModule.Handler("click",function(el)
  {
   return function()
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     return Global.document.getElementById("slctDocumentName").selectedIndex>=0&&Global.confirm(Strings.SFormat(Client.t(Word.ReallyDeleteDocument),[varDocument.c.name]))?Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.deleteDocument:-1223116942",[varDocument.c.id]),function()
     {
      var slctEl;
      slctEl=Global.document.getElementById("slctDocumentName");
      slctEl.removeChild(slctEl[slctEl.selectedIndex]);
      return Concurrency.Combine(slctEl.length===0?(el.style.display="none",Var.Set(varDocument,Types.emptyDocument()),Concurrency.Zero()):Concurrency.Zero(),Concurrency.Delay(function()
      {
       return Concurrency.Bind(setDocument(),function()
       {
        return Concurrency.Bind(setPageButtons(),function()
        {
         show(List.ofArray(["divAttachments"]));
         return Concurrency.Zero();
        });
       });
      }));
     }):Concurrency.Zero();
    })),null);
   };
  })],[Doc.Element("i",[AttrProxy.Create("class","fa fa-trash"),AttrProxy.Create("aria-hidden","true")],[])])]),Doc.Element("hr",[],[]),Doc.Element("div",[AttrProxy.Create("id","divAttachments"),AttrProxy.Create("style","display: none")],[Doc.Element("h4",[],[Doc.TextNode(Client.t(Word.YourAttachments))]),Doc.Element("div",[AttrProxy.Create("id","divAttachmentButtons")],[Doc.Element("button",[AttrProxy.Create("id","btnAddPage"),AttrProxy.Create("style","margin:0; visibility : hidden"),AttrProxy.Create("class","btnLikeLink"),AttrModule.Handler("click",function()
  {
   return function()
   {
    show(List.ofArray(["divChoosePageType","divAttachments","divCreateFilePage"]));
    Global.document.getElementById("rbFilePage").checked=true;
   };
  })],[Doc.Element("i",[AttrProxy.Create("class","fa fa-plus-square"),AttrProxy.Create("aria-hidden","true")],[])])]),Doc.Element("br",[],[]),Doc.Element("hr",[],[]),Doc.Element("br",[],[])]),Doc.Element("div",[AttrProxy.Create("id","divNewDocument"),AttrProxy.Create("style","display: none")],[Doc.Element("br",[],[]),Doc.Element("h4",[],[Doc.TextNode(Client.t(Word.AddDocument))]),Doc.Element("br",[],[]),Doc.Element("label",[AttrProxy.Create("for","txtNewDocumentName")],[Doc.TextNode(Client.t(Word.DocumentName))]),Doc.Element("input",[AttrProxy.Create("id","txtNewDocumentName"),AttrProxy.Create("class","form-control")],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btnLikeLink"),AttrProxy.Create("value",Client.t(Word.AddDocument)),AttrModule.Handler("click",function()
  {
   return function()
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     var newDocumentName,i;
     newDocumentName=String(Global.document.getElementById("txtNewDocumentName").value);
     return Strings.Trim(newDocumentName)===""?(Global.alert(Strings.SFormat(Client.t(Word.FieldIsRequired),[Client.t(Word.DocumentName)])),Concurrency.Zero()):(Var.Set(varDocument,(i=varDocument.c,Document.New(i.id,newDocumentName,i.pages,i.email,i.jobName))),Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.saveNewDocument:-229128764",[varDocument.c]),function(a)
     {
      var i$1,slctEl;
      Var.Set(varDocument,(i$1=varDocument.c,Document.New(a,i$1.name,i$1.pages,i$1.email,i$1.jobName)));
      slctEl=Global.document.getElementById("slctDocumentName");
      addSelectOption(slctEl,newDocumentName);
      Global.document.getElementById("divNewDocument").style.display="none";
      Global.document.getElementById("btnDeleteDocument").style.display="inline";
      slctEl.selectedIndex=slctEl.options.length-1;
      return Concurrency.Bind(setDocument(),function()
      {
       return Concurrency.Bind(setPageButtons(),function()
       {
        show(List.ofArray(["divAttachments"]));
        return Concurrency.Bind(fillDocumentValues(),function()
        {
         return Concurrency.Return(null);
        });
       });
      });
     }));
    })),null);
   };
  })],[])]),Doc.Element("div",[AttrProxy.Create("id","divDisplayedDocument"),AttrProxy.Create("style","display: none")],[Doc.Element("select",[AttrProxy.Create("id","slctHtmlPageTemplate"),AttrModule.Handler("change",function()
  {
   return function()
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
   };
  })],[]),Doc.EmbedView(varDisplayedDocument.v)]),Doc.Element("div",[AttrProxy.Create("id","divUploadedFileDownload"),AttrProxy.Create("style","display: none")],[Doc.Element("input",[AttrProxy.Create("type","checkbox"),AttrProxy.Create("value","false"),AttrProxy.Create("id","chkReplaceVariables")],[]),Doc.Element("label",[AttrProxy.Create("for","chkReplaceVariables")],[Doc.TextNode(Client.t(Word.ReplaceVariables))]),Doc.Element("br",[],[]),Doc.Element("button",[AttrProxy.Create("type","button"),AttrModule.Handler("click",function()
  {
   return function()
   {
    var chkReplaceVariables,m,x,filePage,b$1;
    chkReplaceVariables=Global.jQuery("#chkReplaceVariables");
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
   };
  })],[Doc.TextNode(Client.t(Word.Download))])]),Doc.Element("div",[AttrProxy.Create("id","divEmail"),AttrProxy.Create("style","display: none")],[Doc.Element("h4",[],[Doc.TextNode(Client.t(Word.Email))]),createInput(Client.t(Word.EmailSubject),documentEmailSubject,function()
  {
   return"";
  }),(labelText=Client.t(Word.EmailBody),(guid=(c=Guid.NewGuid(),Guid.ToString(c,"N")),Doc.Element("div",[AttrProxy.Create("class","form-group row")],[Doc.Element("label",[AttrProxy.Create("class","col-lg-3"+" col-form-label"),AttrProxy.Create("for",guid)],[Doc.TextNode(labelText)]),Doc.Element("div",[AttrProxy.Create("class","col-lg-9")],[Doc.InputArea([AttrProxy.Create("id",guid),AttrProxy.Create("class","form-control"),AttrProxy.Create("style","wrap: soft; white-space: nowrap; overflow: auto; min-height: "+"400px")],documentEmailBody)])])))]),Doc.Element("div",[AttrProxy.Create("id","divChoosePageType"),AttrProxy.Create("style","display: none")],[Doc.Element("input",[AttrProxy.Create("type","radio"),AttrProxy.Create("disabled","true"),AttrProxy.Create("name","rbgrpPageType"),AttrProxy.Create("id","rbHtmlPage"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return show(List.ofArray(["divAttachments","divChoosePageType","divCreateHtmlPage"]));
   };
  })],[]),Doc.Element("label",[AttrProxy.Create("for","rbHtmlPage")],[Doc.TextNode(Client.t(Word.CreateOnline))]),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","radio"),AttrProxy.Create("id","rbFilePage"),AttrProxy.Create("name","rbgrpPageType"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return show(List.ofArray(["divAttachments","divChoosePageType","divCreateFilePage"]));
   };
  })],[]),Doc.Element("label",[AttrProxy.Create("for","rbFilePage")],[Doc.TextNode(Client.t(Word.UploadFile))]),Doc.Element("br",[],[]),Doc.Element("br",[],[])]),Doc.Element("div",[AttrProxy.Create("id","divCreateHtmlPage"),AttrProxy.Create("style","display: none")],[Doc.Element("input",[AttrProxy.Create("id","txtCreateHtmlPage")],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("button",[AttrProxy.Create("type","submit"),AttrModule.Handler("click",function()
  {
   return function()
   {
    var pageIndex,i,b$1;
    pageIndex=Global.jQuery("#divAttachmentButtons").children("div").length;
    Var.Set(varDocument,(i=varDocument.c,Document.New(i.id,i.name,List.append(varDocument.c.pages,List.ofArray([new DocumentPage({
     $:0,
     $0:HtmlPage.New(Global.document.getElementById("txtCreateHtmlPage").value,{
      $:1,
      $0:1
     },pageIndex,List.T.Empty)
    })])),i.email,i.jobName)));
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     return Concurrency.Bind(setPageButtons(),function()
     {
      return Concurrency.Return(null);
     });
    })),null);
   };
  })],[Doc.TextNode(Client.t(Word.AddHtmlAttachment))])]),Doc.Element("div",[AttrProxy.Create("id","divCreateFilePage"),AttrProxy.Create("style","display: none")],[Doc.Element("form",[AttrProxy.Create("method","POST"),AttrProxy.Create("action","upload"),AttrProxy.Create("enctype","multipart/form-data")],[Doc.TextNode(Client.t(Word.PleaseChooseAFile)),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","file"),AttrProxy.Create("name","myFile"),AttrProxy.Create("id","myFile"),AttrModule.Handler("change",function(el)
  {
   return function()
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getMaxUploadSize:-836782155",[]),function(a)
     {
      return el.files.item(0).size>a?(Global.alert(Client.t(Word.FileIsTooBig)+"\n"+Strings.SFormat(Client.t(Word.UploadLimit),[String(a/1000000>>0)])),Concurrency.Zero()):(el.parentElement.submit(),Concurrency.Zero());
     });
    })),null);
   };
  })],[]),Doc.Element("input",[AttrProxy.Create("type","hidden"),AttrProxy.Create("id","hiddenDocumentId"),AttrProxy.Create("name","documentId"),AttrProxy.Create("value","1")],[]),Doc.Element("input",[AttrProxy.Create("type","hidden"),AttrProxy.Create("id","hiddenNextPageIndex"),AttrProxy.Create("name","pageIndex"),AttrProxy.Create("value","1")],[])]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("h4",[],[Doc.TextNode(Client.t(Word.YouMightWantToReplaceSomeWordsInYourFileWithVariables))]),Doc.TextNode(Client.t(Word.VariablesWillBeReplacedWithTheRightValuesEveryTimeYouSendYourApplication)),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.TextNode("$firmaName"),Doc.Element("br",[],[]),Doc.TextNode("$firmaStrasse"),Doc.Element("br",[],[]),Doc.TextNode("$firmaPlz"),Doc.Element("br",[],[]),Doc.TextNode("$firmaStadt"),Doc.Element("br",[],[]),Doc.TextNode("$chefAnredeBriefkopf"),Doc.Element("br",[],[]),Doc.TextNode("$chefAnrede"),Doc.Element("br",[],[]),Doc.TextNode("$geehrter"),Doc.Element("br",[],[]),Doc.TextNode("$chefTitel"),Doc.Element("br",[],[]),Doc.TextNode("$chefVorname"),Doc.Element("br",[],[]),Doc.TextNode("$chefNachname"),Doc.Element("br",[],[]),Doc.TextNode("$chefEmail"),Doc.Element("br",[],[]),Doc.TextNode("$chefTelefon"),Doc.Element("br",[],[]),Doc.TextNode("$chefMobil"),Doc.Element("br",[],[]),Doc.TextNode("$meinGeschlecht"),Doc.Element("br",[],[]),Doc.TextNode("$meinTitel"),Doc.Element("br",[],[]),Doc.TextNode("$meinVorname"),Doc.Element("br",[],[]),Doc.TextNode("$meinNachname"),Doc.Element("br",[],[]),Doc.TextNode("$meineStrasse"),Doc.Element("br",[],[]),Doc.TextNode("$meinePlz"),Doc.Element("br",[],[]),Doc.TextNode("$meineStadt"),Doc.Element("br",[],[]),Doc.TextNode("$meineEmail"),Doc.Element("br",[],[]),Doc.TextNode("$meinMobilTelefon"),Doc.Element("br",[],[]),Doc.TextNode("$meineTelefonnr"),Doc.Element("br",[],[]),Doc.TextNode("$datumHeute"),Doc.Element("br",[],[]),Doc.TextNode("$jobName")]),Doc.Element("div",[AttrProxy.Create("id","divSentApplications"),AttrProxy.Create("style","display: none")],[Doc.EmbedView(varDivSentApplications.v)]),Doc.Element("div",[AttrProxy.Create("id","divEditUserValues"),AttrProxy.Create("style","display: none")],[Doc.Element("h4",[],[Doc.TextNode(Client.t(Word.YourValues))]),createInput(Client.t(Word.Degree),userDegree,function()
  {
   return"";
  }),createRadio(Client.t(Word.Gender),List.ofArray([[Client.t(Word.Male),Gender.Male,userGender,""],[Client.t(Word.Female),Gender.Female,userGender,""]])),createInput(Client.t(Word.FirstName),userFirstName,function()
  {
   return"";
  }),createInput(Client.t(Word.LastName),userLastName,function()
  {
   return"";
  }),createInput(Client.t(Word.Street),userStreet,function()
  {
   return"";
  }),createInput(Client.t(Word.Postcode),userPostcode,function()
  {
   return"";
  }),createInput(Client.t(Word.City),userCity,function()
  {
   return"";
  }),createInput(Client.t(Word.Phone),userPhone,function()
  {
   return"";
  }),createInput(Client.t(Word.MobilePhone),userMobilePhone,function()
  {
   return"";
  })]),Doc.Element("div",[AttrProxy.Create("id","divAddEmployer"),AttrProxy.Create("style","display: none")],[createInput(Client.t(Word.JobName),documentJobName,function()
  {
   return"";
  }),Doc.Element("h4",[],[Doc.TextNode(Client.t(Word.Employer))]),Doc.Element("div",[AttrProxy.Create("class","form-group row")],[Doc.Element("div",[AttrProxy.Create("class","col-lg-3")],[Doc.Element("button",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btn-block"),AttrProxy.Create("id","btnReadFromWebsite"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return Concurrency.Start(readFromWebsite(),null);
   };
  })],[Doc.Element("i",[AttrProxy.Create("class","fa fa-spinner fa-spin"),AttrProxy.Create("id","faReadFromWebsite"),AttrProxy.Create("style","color: black; margin-right: 10px; visibility: hidden")],[]),Doc.TextNode(Client.t(Word.LoadFromWebsite))])]),Doc.Element("div",[AttrProxy.Create("class","col-lg-9")],[Doc.Element("input",[AttrProxy.Create("id","txtReadEmployerFromWebsite"),AttrProxy.Create("type","text"),AttrModule.Handler("paste",function()
  {
   return function()
   {
    return Concurrency.Start(readFromWebsite(),null);
   };
  }),AttrProxy.Create("class","form-control"),AttrProxy.Create("placeholder","URL oder Referenznummer"),AttrModule.Handler("focus",function(el)
  {
   return function()
   {
    return el.select();
   };
  })],[])])]),Doc.Element("div",[AttrProxy.Create("class","form-group row")],[Doc.Element("div",[AttrProxy.Create("class","col-12")],[Doc.Element("button",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btnLikeLink btn-block"),AttrProxy.Create("style","min-height: 40px; font-size: 20px"),AttrProxy.Create("id","btnApplyNowTop"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return Concurrency.Start(btnApplyNowClicked(),null);
   };
  })],[Doc.Element("i",[AttrProxy.Create("class","fa fa-icon"),AttrProxy.Create("id","faBtnApplyNowTop"),AttrProxy.Create("style","color: #08a81b; margin-right: 10px")],[]),Doc.TextNode(Client.t(Word.ApplyNow))])])]),createInput(Client.t(Word.CompanyName),employerCompany,function()
  {
   return"";
  }),createInput(Client.t(Word.Street),employerStreet,function()
  {
   return"";
  }),createInput(Client.t(Word.Postcode),employerPostcode,function()
  {
   return"";
  }),createInput(Client.t(Word.City),employerCity,function()
  {
   return"";
  }),createRadio(Client.t(Word.Gender),List.ofArray([[Client.t(Word.Male),Gender.Male,employerGender,""],[Client.t(Word.Female),Gender.Female,employerGender,""],[Client.t(Word.UnknownGender),Gender.Unknown,employerGender,"checked"]])),createInput(Client.t(Word.Degree),employerDegree,function()
  {
   return"";
  }),createInput(Client.t(Word.FirstName),employerFirstName,function()
  {
   return"";
  }),createInput(Client.t(Word.LastName),employerLastName,function()
  {
   return"";
  }),createInput(Client.t(Word.Email),employerEmail,function()
  {
   return"";
  }),createInput(Client.t(Word.Phone),employerPhone,function()
  {
   return"";
  }),createInput(Client.t(Word.MobilePhone),employerMobilePhone,function()
  {
   return"";
  }),Doc.Element("button",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btnLikeLink btn-block"),AttrProxy.Create("style","min-height: 40px; font-size: 20px"),AttrProxy.Create("id","btnApplyNowBottom"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return Concurrency.Start(btnApplyNowClicked(),null);
   };
  })],[Doc.Element("i",[AttrProxy.Create("class","fa fa-icon"),AttrProxy.Create("id","faBtnApplyNowBottom"),AttrProxy.Create("style","color: #08a81b; margin-right: 10px")],[]),Doc.TextNode(Client.t(Word.ApplyNow))])])]);
 };
 Client.login=function()
 {
  return Doc.Element("div",[],[Doc.Element("form",[AttrProxy.Create("action","/login"),AttrProxy.Create("method","POST")],[Doc.Element("h4",[],[Doc.TextNode(Client.t(Word.Login))]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","txtLoginEmail")],[Doc.TextNode("Email")]),Doc.Element("input",[AttrProxy.Create("class","form-control"),AttrProxy.Create("name","txtLoginEmail"),AttrProxy.Create("id","txtLoginEmail")],[])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","txtLoginPassword")],[Doc.TextNode("Password")]),Doc.Element("input",[AttrProxy.Create("type","password"),AttrProxy.Create("class","form-control"),AttrProxy.Create("name","txtLoginPassword"),AttrProxy.Create("id","txtLoginPassword")],[])]),Doc.Element("input",[AttrProxy.Create("type","submit"),AttrProxy.Create("value","Login"),AttrProxy.Create("name","btnLogin")],[]),Doc.Element("input",[AttrProxy.Create("type","submit"),AttrProxy.Create("style","margin-left: 30px;"),AttrProxy.Create("name","btnRegister"),AttrProxy.Create("value","Register")],[])])]);
 };
 Client.t=function(w)
 {
  return Client.varLanguageDict().c.get_Item(w);
 };
 Client.varLanguageDict=function()
 {
  SC$3.$cctor();
  return SC$3.varLanguageDict;
 };
 SC$3.$cctor=function()
 {
  SC$3.$cctor=Global.ignore;
  SC$3.varLanguageDict=Var.Create$1(Map.OfArray(Arrays.ofSeq(Deutsch.dict())));
 };
}());
