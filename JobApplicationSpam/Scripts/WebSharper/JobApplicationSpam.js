(function()
{
 "use strict";
 var Global,JobApplicationSpam,Types,Gender,Login,UserValues,Employer,JobApplicationPageAction,HtmlPage,FilePage,DocumentPage,DocumentEmail,Document,HtmlPageTemplate,PageDB,Language,Word,Deutsch,SC$1,Client,SC$2,IntelliFactory,Runtime,WebSharper,Operators,List,Math,Concurrency,Remoting,AjaxRemotingProvider,Date,UI,Next,Var,Doc,AttrProxy,Seq,Utils,AttrModule,System,Guid,Collections,Map,Arrays,Unchecked,Enumerator,Strings;
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
 Deutsch=JobApplicationSpam.Deutsch=JobApplicationSpam.Deutsch||{};
 SC$1=Global.StartupCode$JobApplicationSpam$Deutsch=Global.StartupCode$JobApplicationSpam$Deutsch||{};
 Client=JobApplicationSpam.Client=JobApplicationSpam.Client||{};
 SC$2=Global.StartupCode$JobApplicationSpam$Client=Global.StartupCode$JobApplicationSpam$Client||{};
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 WebSharper=Global.WebSharper;
 Operators=WebSharper&&WebSharper.Operators;
 List=WebSharper&&WebSharper.List;
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
 Arrays=WebSharper&&WebSharper.Arrays;
 Unchecked=WebSharper&&WebSharper.Unchecked;
 Enumerator=WebSharper&&WebSharper.Enumerator;
 Strings=WebSharper&&WebSharper.Strings;
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
 Word.ReallyDeletePage={
  $:38
 };
 Word.ReallyDeleteDocument={
  $:37
 };
 Word.DownloadWithReplacedVariables={
  $:36
 };
 Word.JustDownload={
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
 Deutsch.dict=function()
 {
  SC$1.$cctor();
  return SC$1.dict;
 };
 SC$1.$cctor=function()
 {
  SC$1.$cctor=Global.ignore;
  SC$1.dict=List.ofArray([[Word.AddEmployerAndApply,"Bewerben"],[Word.EditYourValues,"Deine Daten"],[Word.EditEmail,"Email"],[Word.EditAttachments,"Anhänge"],[Word.YourApplicationDocuments,"Deine Bewerbungsunterlagen"],[Word.LoadFromWebsite,"Von Website lesen"],[Word.ApplyNow,"Jetzt bewerben"],[Word.CompanyName,"Firmenname"],[Word.Street,"Straße"],[Word.Postcode,"Postleitzahl"],[Word.City,"Stadt"],[Word.Gender,"Geschlecht"],[Word.Degree,"Titel"],[Word.FirstName,"Vorname"],[Word.LastName,"Nachname"],[Word.Email,"Email"],[Word.Phone,"Telefonnummer"],[Word.MobilePhone,"Mobilnummer"],[Word.YourValues,"Deine Daten"],[Word.EmailSubject,"Betreff"],[Word.EmailBody,"Text"],[Word.YourAttachments,"Deine Anhänge"],[Word.CreateOnline,"Online erstellen"],[Word.UploadFile,"Datei hochladen"],[Word.PleaseChooseAFile,"Bitte eine Datei aussuchen"],[Word.AddAttachment,"Anhang hinzufügen"],[Word.YouMightWantToReplaceSomeWordsInYourFileWithVariables,"Vielleicht möchtest du einige Worte in deiner Datei durch Variablen ersetzen."],[Word.VariablesWillBeReplacedWithTheRightValuesEveryTimeYouSendYourApplication,"Jedesmal, wenn du eine Bewerbung versendest, werden die Variablen automatisch durch die richtigen Werte ersetzt."],[Word.Male,"männlich"],[Word.Female,"weiblich"],[Word.UnknownGender,"unbekannt"],[Word.AddDocument,"Dokument hinzufügen"],[Word.DocumentName,"Name des Dokuments"],[Word.AddHtmlAttachment,"Html Anhang hinzufügen"],[Word.Employer,"Arbeitgeber"],[Word.JustDownload,"Nur downloaden"],[Word.DownloadWithReplacedVariables,"Variablen ersetzen und downloaden"],[Word.ReallyDeleteDocument,"Document \"{0}\" wirklich löschen?"],[Word.ReallyDeletePage,"Seite \"{0}\" wirklich löschen?"],[Word.PleaseConfirmYourEmailAddressEmailSubject,"Bitte bestätige deine Email-Adresse"],[Word.PleaseConfirmYourEmailAddressEmailBody,"Hallo!\n\nBitte besuche den folgenden Link, um deine Email-Adresse zu bestätigen.\nhttp://bewerbungsspam.de/confirmemail?email={0}&guid={1}\n\nDein Team von www.bewerbungsspam.de"],[Word.Login,"Einloggen"],[Word.Register,"Registrieren"],[Word.SentApplications,"Versandte Bewerbungen"],[Word.JobName,"Bewerben als"],[Word.AppliedAs,"Beworben als"],[Word.AppliedOnDate,"Beworben am"],[Word.TheEmailOfYourEmployerDoesNotLookValid,"Die Email-Adresse des Arbeitgeber scheint fehlerhaft zu sein."],[Word.FieldIsRequired,"Feld \"{0}\" darf nicht leer sein."],[Word.SorryAnErrorOccurred,"Entschuldigung, es ist ein Fehler aufgetreten."],[Word.YourApplicationHasNotBeenSent,"Deine Bewerbung konnte nicht versendet werden :-("]]);
 };
 Client.templates=function()
 {
  var varDocument,varUserValues,varUserEmail,varEmployer,varDisplayedDocument,varDivSentApplications,showHideMutualElements,b,labelText,dataBind;
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
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getSentApplications:-272535369",[Date.now(),Date.now()]),function(a)
    {
     Var.Set(varDivSentApplications,Doc.Element("table",[AttrProxy.Create("style","border-spacing: 10px; border-collapse: separate")],[Doc.Element("thead",[],[Doc.Element("tr",[],[Doc.Element("th",[],[Doc.TextNode(Client.t(Word.CompanyName))]),Doc.Element("th",[],[Doc.TextNode(Client.t(Word.AppliedOnDate))]),Doc.Element("th",[],[Doc.TextNode(Client.t(Word.AppliedAs))])])]),Doc.Element("tbody",[],List.ofSeq(Seq.delay(function()
     {
      return Seq.collect(function(app)
      {
       return List.ofArray([Doc.Element("tr",[],[Doc.Element("td",[],[Doc.TextNode(app.companyName)]),Doc.Element("td",[],[Doc.TextNode(((((Runtime.Curried(function($1,$2,$3,$4)
       {
        return $1(Utils.padNumLeft(Global.String($2),2)+"."+Utils.padNumLeft(Global.String($3),2)+"."+Utils.padNumLeft(Global.String($4),4));
       },4))(Global.id))((new Date(app.statusChangedOn)).getDate()))((new Date(app.statusChangedOn)).getMonth()+1))((new Date(app.statusChangedOn)).getFullYear()))]),Doc.Element("td",[],[Doc.TextNode(app.appliedAs)])])]);
      },a);
     })))]));
     return Concurrency.Zero();
    });
   });
  }
  function createInputWithColumnSizes(column1Size,column2Size,labelText$1,dataBind$1,validFun)
  {
   return Doc.Element("div",[AttrProxy.Create("class","form-group row")],[Doc.Element("label",[AttrProxy.Create("class",column1Size+" col-form-label"),AttrProxy.Create("for",dataBind$1)],[Doc.TextNode(labelText$1)]),Doc.Element("div",[AttrProxy.Create("class",column2Size)],[Doc.Element("input",[AttrProxy.Create("id",dataBind$1),AttrProxy.Create("data-"+"bind",dataBind$1),AttrProxy.Create("class","form-control"),AttrProxy.Create("type","text"),AttrModule.Handler("blur",function(el)
   {
    return function()
    {
     var p;
     p=validFun(el.value);
     return p[0]?(Global.jQuery(el).removeClass("is-invalid"),Global.jQuery(el).addClass("is-valid"),void Global.jQuery(el).parent().next().hide()):(Global.jQuery(el).removeClass("is-valid"),Global.jQuery(el).addClass("is-invalid"),Global.jQuery(el).parent().next().toggle(true),void Global.jQuery(el).parent().next().first().html(p[1]));
    };
   })],[])]),Doc.Element("div",[AttrProxy.Create("class",column1Size),AttrProxy.Create("style","display: none")],[Doc.Element("small",[AttrProxy.Create("class","text-danger")],[Doc.TextNode("Must be 8-20 characters")])])]);
  }
  function createRadioWithColumnSizes(column1Size,column2Size,labelText$1,radioValuesList)
  {
   var radioGroup,c;
   radioGroup=(c=Guid.NewGuid(),Global.String(c));
   return Doc.Element("div",[],List.mapi(function(i,t)
   {
    var id,c$1;
    id=(c$1=Guid.NewGuid(),Global.String(c$1));
    return Doc.Element("div",[AttrProxy.Create("class","form-group row")],[Doc.Element("label",[AttrProxy.Create("class",column1Size+" col-form-label")],[Doc.TextNode(i===0?labelText$1:"")]),Doc.Element("div",[AttrProxy.Create("class",column2Size)],[Doc.Element("input",[AttrProxy.Create("id",id),AttrProxy.Create("type","radio"),AttrProxy.Create("name",radioGroup),AttrProxy.Create("value",t[2]),AttrProxy.Create("data-"+"bind",t[1]),AttrProxy.Create("checked","checked")],[]),Doc.Element("label",[AttrProxy.Create("for",id)],[Doc.TextNode(t[0])])])]);
   },radioValuesList));
  }
  function createInput($1,$2,$3)
  {
   return createInputWithColumnSizes("col-lg-3","col-lg-9",$1,$2,$3);
  }
  function createRadio($1,$2)
  {
   return createRadioWithColumnSizes("col-lg-3","col-lg-9",$1,$2);
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
     var jEl,key;
     jEl=Global.jQuery(el);
     key=el.getAttribute("data-page-key");
     myMap.ContainsKey(key)?jEl.val(myMap.get_Item(key)):jEl.val(Global.String(el.getAttribute("data-page-value")));
    }),Concurrency.Zero())))):Concurrency.Zero(),Concurrency.Delay(function()
    {
     var map;
     map=Map.OfArray(Arrays.ofSeq(List.ofArray([["userGender",["radio",function()
     {
      return Global.String(varUserValues.c.gender);
     },function(v)
     {
      var i;
      Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(Gender.fromString(v),i.degree,i.firstName,i.lastName,i.street,i.postcode,i.city,i.phone,i.mobilePhone)));
     }]],["userDegree",["text",function()
     {
      return varUserValues.c.degree;
     },function(v)
     {
      var i;
      Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,v,i.firstName,i.lastName,i.street,i.postcode,i.city,i.phone,i.mobilePhone)));
     }]],["userFirstName",["text",function()
     {
      return varUserValues.c.firstName;
     },function(v)
     {
      var i;
      Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,v,i.lastName,i.street,i.postcode,i.city,i.phone,i.mobilePhone)));
     }]],["userLastName",["text",function()
     {
      return varUserValues.c.lastName;
     },function(v)
     {
      var i;
      Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,v,i.street,i.postcode,i.city,i.phone,i.mobilePhone)));
     }]],["userStreet",["text",function()
     {
      return varUserValues.c.street;
     },function(v)
     {
      var i;
      Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,v,i.postcode,i.city,i.phone,i.mobilePhone)));
     }]],["userPostcode",["text",function()
     {
      return varUserValues.c.postcode;
     },function(v)
     {
      var i;
      Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,i.street,v,i.city,i.phone,i.mobilePhone)));
     }]],["userCity",["text",function()
     {
      return varUserValues.c.city;
     },function(v)
     {
      var i;
      Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,i.street,i.postcode,v,i.phone,i.mobilePhone)));
     }]],["userEmail",["text",function()
     {
      return varUserEmail.c;
     },function(v)
     {
      Var.Set(varUserEmail,v);
     }]],["userPhone",["text",function()
     {
      return varUserValues.c.phone;
     },function(v)
     {
      var i;
      Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,i.street,i.postcode,i.city,v,i.mobilePhone)));
     }]],["userMobilePhone",["text",function()
     {
      return varUserValues.c.mobilePhone;
     },function(v)
     {
      var i;
      Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,i.street,i.postcode,i.city,i.phone,v)));
     }]],["company",["text",function()
     {
      return varEmployer.c.company;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(v,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
     }]],["companyStreet",["text",function()
     {
      return varEmployer.c.street;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,v,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
     }]],["companyPostcode",["text",function()
     {
      return varEmployer.c.postcode;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,v,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
     }]],["companyCity",["text",function()
     {
      return varEmployer.c.city;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,v,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
     }]],["bossGender",["radio",function()
     {
      return Global.String(varEmployer.c.gender);
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,Gender.fromString(v),i.degree,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
     }]],["bossDegree",["text",function()
     {
      return varEmployer.c.degree;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,v,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
     }]],["bossFirstName",["text",function()
     {
      return varEmployer.c.firstName;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,v,i.lastName,i.email,i.phone,i.mobilePhone)));
     }]],["bossLastName",["text",function()
     {
      return varEmployer.c.lastName;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,v,i.email,i.phone,i.mobilePhone)));
     }]],["bossEmail",["text",function()
     {
      return varEmployer.c.email;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,v,i.phone,i.mobilePhone)));
     }]],["bossPhone",["text",function()
     {
      return varEmployer.c.phone;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,v,i.mobilePhone)));
     }]],["bossMobilePhone",["text",function()
     {
      return varEmployer.c.mobilePhone;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,v)));
     }]],["emailSubject",["text",function()
     {
      return varDocument.c.email.subject;
     },function(v)
     {
      var i;
      Var.Set(varDocument,(i=varDocument.c,Document.New(i.id,i.name,i.pages,DocumentEmail.New(v,varDocument.c.email.body),i.jobName)));
     }]],["emailBody",["text",function()
     {
      return varDocument.c.email.body;
     },function(v)
     {
      var i;
      Var.Set(varDocument,(i=varDocument.c,Document.New(i.id,i.name,i.pages,DocumentEmail.New(varDocument.c.email.subject,v),i.jobName)));
     }]],["jobName",["text",function()
     {
      return varDocument.c.jobName;
     },function(v)
     {
      var i;
      Var.Set(varDocument,(i=varDocument.c,Document.New(i.id,i.name,i.pages,i.email,v)));
     }]]])));
     return Concurrency.Combine(Concurrency.For(map,function(a)
     {
      var m$1,get,get$1;
      m$1=a.V;
      return m$1[0]==="radio"?(get=m$1[1],(Global.jQuery((function($1)
      {
       return function($2)
       {
        return $1("[data-bind='"+Utils.toSafe($2)+"']");
       };
      }(Global.id))(a.K)).each(function(i,el)
      {
       return get()===el.value?void(el.checked=true):null;
      }),Concurrency.Zero())):m$1[0]==="text"?(get$1=m$1[1],(Global.jQuery((function($1)
      {
       return function($2)
       {
        return $1("[data-bind='"+Utils.toSafe($2)+"']");
       };
      }(Global.id))(a.K)).each(function(i,el)
      {
       el.value=get$1();
      }),Concurrency.Zero())):(Operators.FailWith("Unknown input type: "+m$1[0]),Concurrency.Zero());
     }),Concurrency.Delay(function()
     {
      Global.jQuery(Global.document.querySelectorAll("[data-bind]")).each(function($1,el)
      {
       function eventAction()
       {
        var elValue,bindValue;
        elValue=Global.String(Global.jQuery(el).val());
        bindValue=Global.String(Global.jQuery(el).data("bind"));
        (map.get_Item(bindValue))[2](elValue);
        Global.jQuery((function($2)
        {
         return function($3)
         {
          return $2("[data-bind='"+Utils.toSafe($3)+"']");
         };
        }(Global.id))(bindValue)).each(function($2,updateElement)
        {
         var m$1;
         if(!Unchecked.Equals(updateElement,el))
          {
           m$1=map.get_Item(bindValue);
           m$1[0]==="radio"?updateElement.checked=elValue===updateElement.value:m$1[0]==="text"?updateElement.value=elValue:Operators.FailWith("Unknown input type: "+m$1[0]);
          }
        });
       }
       el.removeEventListener("input",eventAction,true);
       el.addEventListener("input",eventAction,true);
       el.removeEventListener("click",eventAction,true);
       el.addEventListener("click",eventAction,true);
      });
      return Concurrency.Zero();
     }));
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
          $0:HtmlPage.New(htmlPage.name,htmlPage.oTemplateId,htmlPage.pageIndex,List.ofSeq(Map.ToSeq(Map.OfArray(Arrays.ofSeq(htmlPage.map)).Add(key,Global.String(Global.jQuery(el).val())))))
         }),List.T.Empty]):(htmlPage$1=currentAndAfter.$0.$0,[new DocumentPage({
          $:0,
          $0:HtmlPage.New(htmlPage$1.name,htmlPage$1.oTemplateId,htmlPage$1.pageIndex,List.ofSeq(Map.ToSeq(Map.OfArray(Arrays.ofSeq(htmlPage$1.map)).Add(key,Global.String(Global.jQuery(el).val())))))
         }),currentAndAfter.$1]),[t[0],p$1[0],p$1[1]])));
         Var.Set(varDocument,(i$1=varDocument.c,Document.New(i$1.id,i$1.name,List.append(p[0],new List.T({
          $:1,
          $0:p[1],
          $1:p[2]
         })),i$1.email,i$1.jobName)));
         Concurrency.Start(fillDocumentValues(),null);
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
     return a==null?(Var.Set(varDocument,Document.New(0,"",List.T.Empty,DocumentEmail.New("",""),"")),Concurrency.Zero()):(Var.Set(varDocument,a.$0),Global.document.getElementById("hiddenDocumentId").value=Global.String(varDocument.c.id),Concurrency.Zero());
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
          return Concurrency.Bind(fillDocumentValues(),function()
          {
           show(List.ofArray(["divAttachments"]));
           return Concurrency.Zero();
          });
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
          return Concurrency.Bind(fillDocumentValues(),function()
          {
           return Concurrency.Return(null);
          });
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
          return Concurrency.Bind(fillDocumentValues(),function()
          {
           return Concurrency.Return(null);
          });
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
     Global.document.getElementById("hiddenNextPageIndex").value=Global.String(Global.jQuery("#divAttachmentButtons").children("div").length+1);
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
    var bossEmail,jobName,btnLoadFromWebsite,buttons;
    function a(bEl,faEl)
    {
     bEl.prop("disabled",true);
     faEl.css("color","black");
     faEl.addClass("fa-spinner fa-spin");
    }
    bossEmail=Global.String(Global.jQuery("#divAddEmployer input[data-bind='bossEmail']").first().val());
    jobName=Global.String(Global.jQuery("#divAddEmployer input[data-bind='jobName']").first().val());
    return!(new Global.RegExp("^(([^<>()\\[\\]\\\\.,;:\\s@\"]+(\\.[^<>()\\[\\]\\\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$")).test(bossEmail)?(Global.alert(Client.t(Word.TheEmailOfYourEmployerDoesNotLookValid)),Concurrency.Zero()):Strings.Trim(jobName)===""?(Global.alert(Strings.SFormat(Client.t(Word.FieldIsRequired),[Client.t(Word.JobName)])),Concurrency.Zero()):(btnLoadFromWebsite=Global.jQuery("#btnLoadFromWebsite"),(buttons=List.ofArray([[Global.jQuery("#btnApplyNowBottom"),Global.jQuery("#faBtnApplyNowBottom")],[Global.jQuery("#btnApplyNowTop"),Global.jQuery("#faBtnApplyNowTop")]]),(List.iter(function($1)
    {
     return a($1[0],$1[1]);
    },buttons),btnLoadFromWebsite.prop("disabled",true),Global.jQuery("#divJobApplicationContent").find("input,textarea,button,select").prop("disabled",true),Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.applyNow:-1253815691",[varEmployer.c,varDocument.c,varUserValues.c]),function(a$1)
    {
     function a$2(bEl,faEl)
     {
      bEl.prop("disabled",false);
      faEl.removeClass("fa-spinner fa-spin");
     }
     function a$3(bEl,faEl)
     {
      faEl.css("color","#08a81b");
      faEl.addClass("fa-check");
     }
     List.iter(function($1)
     {
      return a$2($1[0],$1[1]);
     },buttons);
     btnLoadFromWebsite.prop("disabled",false);
     Global.jQuery("#divJobApplicationContent").find("input,textarea,button,select").prop("disabled",false);
     return a$1.$==0?(List.iter(function($1)
     {
      return a$3($1[0],$1[1]);
     },buttons),Global.jQuery("#divAddEmployer input[type='text'][data-bind]").val(""),Global.jQuery("#divAddEmployer input[type='radio'][data-bind='bossGender'][value='u']").prop("checked","checked"),Concurrency.Bind(Concurrency.Sleep(3500),function()
     {
      function a$4(bEl,faEl)
      {
       faEl.removeClass("fa-check");
      }
      List.iter(function($1)
      {
       return a$4($1[0],$1[1]);
      },buttons);
      return Concurrency.Zero();
     })):Concurrency.Bind(Concurrency.Sleep(700),function()
     {
      Global.alert(Client.t(Word.SorryAnErrorOccurred)+"\n"+Client.t(Word.YourApplicationHasNotBeenSent));
      return Concurrency.Zero();
     });
    }))));
   });
  }
  varDocument=Var.Create$1(Document.New(0,"",List.T.Empty,DocumentEmail.New("",""),""));
  varUserValues=Var.Create$1(UserValues.New(Gender.Female,"","","","","","","",""));
  varUserEmail=Var.CreateWaiting();
  varEmployer=Var.Create$1(Employer.New("","","","",Gender.Unknown,"","","","","",""));
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
         return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getLastEditedDocumentId:-646436276",[]),function(a$3)
         {
          return Concurrency.Combine(a$3!=null&&a$3.$==1?(slctDocumentNameEl.selectedIndex=a$3.$0-1,Concurrency.Zero()):(slctDocumentNameEl.selectedIndex=0,Concurrency.Zero()),Concurrency.Delay(function()
          {
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
                return Concurrency.Bind(fillDocumentValues(),function()
                {
                 return Concurrency.Return(null);
                });
               }));
              }));
             });
            });
           });
          }));
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
      return Concurrency.Combine(slctEl.length===0?(el.style.display="none",Var.Set(varDocument,Document.New(0,"",List.T.Empty,DocumentEmail.New("",""),"")),Concurrency.Zero()):Concurrency.Zero(),Concurrency.Delay(function()
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
      }));
     }):Concurrency.Zero();
    })),null);
   };
  })],[Doc.Element("i",[AttrProxy.Create("class","fa fa-trash"),AttrProxy.Create("aria-hidden","true")],[])])]),Doc.Element("hr",[],[]),Doc.Element("div",[AttrProxy.Create("id","divAttachments"),AttrProxy.Create("style","display: none")],[Doc.Element("h4",[],[Doc.TextNode(Client.t(Word.YourAttachments))]),Doc.Element("div",[AttrProxy.Create("id","divAttachmentButtons")],[Doc.Element("button",[AttrProxy.Create("id","btnAddPage"),AttrProxy.Create("style","margin:0;"),AttrProxy.Create("class","btnLikeLink"),AttrModule.Handler("click",function()
  {
   return function()
   {
    show(List.ofArray(["divChoosePageType","divAttachments","divCreateFilePage"]));
    Global.document.getElementById("rbFilePage").checked=true;
   };
  })],[Doc.Element("i",[AttrProxy.Create("class","fa fa-plus-square"),AttrProxy.Create("aria-hidden","true")],[])])]),Doc.Element("br",[],[]),Doc.Element("hr",[],[]),Doc.Element("br",[],[])]),Doc.Element("div",[AttrProxy.Create("id","divNewDocument"),AttrProxy.Create("style","display: none")],[Doc.TextNode(Client.t(Word.DocumentName)),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("id","txtNewDocumentName"),AttrProxy.Create("autofocus","autofocus")],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btnLikeLink"),AttrProxy.Create("value",Client.t(Word.AddDocument)),AttrModule.Handler("click",function()
  {
   return function()
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     var newDocumentName,i;
     newDocumentName=Global.String(Global.document.getElementById("txtNewDocumentName").value);
     Var.Set(varDocument,(i=varDocument.c,Document.New(i.id,newDocumentName,i.pages,i.email,i.jobName)));
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.saveNewDocument:-229128764",[varDocument.c]),function(a)
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
        return Concurrency.Bind(fillDocumentValues(),function()
        {
         return Concurrency.Return(null);
        });
       });
      });
     });
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
  })],[]),Doc.EmbedView(varDisplayedDocument.v)]),Doc.Element("div",[AttrProxy.Create("id","divUploadedFileDownload"),AttrProxy.Create("style","display: none")],[Doc.Element("button",[AttrProxy.Create("type","button"),AttrModule.Handler("click",function()
  {
   return function()
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     var m,x,$1;
     m=(x=varDocument.c.pages,Seq.tryItem(getCurrentPageIndex()-1,x));
     return m!=null&&m.$==1&&(m.$0.$==1&&($1=m.$0.$0,true))?Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getFullPath:-1278585997",[$1.path,varDocument.c.id]),function(a)
     {
      return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.createLink:-1287498065",[a]),function(a$1)
      {
       Global.location.href=(function($2)
       {
        return function($3)
        {
         return $2("download/"+Utils.toSafe($3));
        };
       }(Global.id))(a$1);
       return Concurrency.Zero();
      });
     }):Concurrency.Zero();
    })),null);
   };
  })],[Doc.TextNode(Client.t(Word.JustDownload))]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("button",[AttrProxy.Create("type","button"),AttrModule.Handler("click",function()
  {
   return function()
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     var m,x,$1;
     m=(x=varDocument.c.pages,Seq.tryItem(getCurrentPageIndex()-1,x));
     return m!=null&&m.$==1&&(m.$0.$==1&&($1=m.$0.$0,true))?Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.replaceVariables:-699540492",[$1.path,varUserValues.c,varEmployer.c,varDocument.c]),function(a)
     {
      return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.createLink:-1287498065",[a]),function(a$1)
      {
       Global.location.href=(function($2)
       {
        return function($3)
        {
         return $2("download/"+Utils.toSafe($3));
        };
       }(Global.id))(a$1);
       return Concurrency.Zero();
      });
     }):Concurrency.Zero();
    })),null);
   };
  })],[Doc.TextNode(Client.t(Word.DownloadWithReplacedVariables))])]),Doc.Element("div",[AttrProxy.Create("id","divEmail"),AttrProxy.Create("style","display: none")],[Doc.Element("h4",[],[Doc.TextNode(Client.t(Word.Email))]),createInput(Client.t(Word.EmailSubject),"emailSubject",function(s)
  {
   return[s!=="","Required"];
  }),(labelText=Client.t(Word.EmailBody),(dataBind="emailBody",Doc.Element("div",[AttrProxy.Create("class","form-group row")],[Doc.Element("label",[AttrProxy.Create("class","col-lg-3"+" col-form-label"),AttrProxy.Create("for",dataBind)],[Doc.TextNode(labelText)]),Doc.Element("div",[AttrProxy.Create("class","col-lg-9")],[Doc.Element("textarea",[AttrProxy.Create("id",dataBind),AttrProxy.Create("data-"+"bind",dataBind),AttrProxy.Create("class","form-control"),AttrProxy.Create("style","wrap: soft; white-space: nowrap; overflow: auto; min-height: "+"400px")],[])])])))]),Doc.Element("div",[AttrProxy.Create("id","divChoosePageType"),AttrProxy.Create("style","display: none")],[Doc.Element("input",[AttrProxy.Create("type","radio"),AttrProxy.Create("name","rbgrpPageType"),AttrProxy.Create("id","rbHtmlPage"),AttrModule.Handler("click",function()
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
  })],[Doc.TextNode(Client.t(Word.AddHtmlAttachment))])]),Doc.Element("div",[AttrProxy.Create("id","divCreateFilePage"),AttrProxy.Create("style","display: none")],[Doc.Element("form",[AttrProxy.Create("enctype","multipart/form-data"),AttrProxy.Create("method","POST"),AttrProxy.Create("action","")],[Doc.TextNode(Client.t(Word.PleaseChooseAFile)),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","file"),AttrProxy.Create("name","file"),AttrModule.Handler("change",function()
  {
   return function()
   {
    Global.document.getElementById("flUpload").style.visibility="visible";
   };
  })],[]),Doc.Element("input",[AttrProxy.Create("type","hidden"),AttrProxy.Create("id","hiddenDocumentId"),AttrProxy.Create("name","documentId")],[]),Doc.Element("input",[AttrProxy.Create("type","hidden"),AttrProxy.Create("id","hiddenNextPageIndex"),AttrProxy.Create("name","pageIndex")],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("button",[AttrProxy.Create("type","submit"),AttrProxy.Create("id","flUpload"),AttrProxy.Create("style","visibility: hidden")],[Doc.TextNode(Client.t(Word.AddAttachment))]),Doc.Element("br",[],[]),Doc.Element("hr",[],[]),Doc.Element("br",[],[]),Doc.Element("b",[],[Doc.TextNode(Client.t(Word.YouMightWantToReplaceSomeWordsInYourFileWithVariables)+Client.t(Word.VariablesWillBeReplacedWithTheRightValuesEveryTimeYouSendYourApplication))]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.TextNode("$firmaName"),Doc.Element("br",[],[]),Doc.TextNode("$firmaStrasse"),Doc.Element("br",[],[]),Doc.TextNode("$firmaPlz"),Doc.Element("br",[],[]),Doc.TextNode("$firmaStadt"),Doc.Element("br",[],[]),Doc.TextNode("$chefAnredeBriefkopf"),Doc.Element("br",[],[]),Doc.TextNode("$chefAnrede"),Doc.Element("br",[],[]),Doc.TextNode("$geehrter"),Doc.Element("br",[],[]),Doc.TextNode("$chefTitel"),Doc.Element("br",[],[]),Doc.TextNode("$chefVorname"),Doc.Element("br",[],[]),Doc.TextNode("$chefNachname"),Doc.Element("br",[],[]),Doc.TextNode("$chefEmail"),Doc.Element("br",[],[]),Doc.TextNode("$chefTelefon"),Doc.Element("br",[],[]),Doc.TextNode("$chefMobil"),Doc.Element("br",[],[]),Doc.TextNode("$meinGeschlecht"),Doc.Element("br",[],[]),Doc.TextNode("$meinTitel"),Doc.Element("br",[],[]),Doc.TextNode("$meinVorname"),Doc.Element("br",[],[]),Doc.TextNode("$meinNachname"),Doc.Element("br",[],[]),Doc.TextNode("$meineStrasse"),Doc.Element("br",[],[]),Doc.TextNode("$meinePlz"),Doc.Element("br",[],[]),Doc.TextNode("$meineStadt"),Doc.Element("br",[],[]),Doc.TextNode("$meineEmail"),Doc.Element("br",[],[]),Doc.TextNode("$meinMobilTelefon"),Doc.Element("br",[],[]),Doc.TextNode("$meineTelefonnr"),Doc.Element("br",[],[]),Doc.TextNode("$datumHeute"),Doc.Element("br",[],[]),Doc.TextNode("$jobName")])]),Doc.Element("div",[AttrProxy.Create("id","divSentApplications"),AttrProxy.Create("style","display: none")],[Doc.EmbedView(varDivSentApplications.v)]),Doc.Element("div",[AttrProxy.Create("id","divEditUserValues"),AttrProxy.Create("style","display: none")],[Doc.Element("h4",[],[Doc.TextNode(Client.t(Word.YourValues))]),createInput(Client.t(Word.Degree),"userDegree",function()
  {
   return[true,""];
  }),createRadio(Client.t(Word.Gender),List.ofArray([[Client.t(Word.Male),"userGender","m",""],[Client.t(Word.Female),"userGender","f",""]])),createInput(Client.t(Word.FirstName),"userFirstName",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(Client.t(Word.LastName),"userLastName",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(Client.t(Word.Street),"userStreet",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(Client.t(Word.Postcode),"userPostcode",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(Client.t(Word.City),"userCity",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(Client.t(Word.Phone),"userPhone",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(Client.t(Word.MobilePhone),"userMobilePhone",function(s)
  {
   return[s!=="","This field is required"];
  })]),Doc.Element("div",[AttrProxy.Create("id","divAddEmployer"),AttrProxy.Create("style","display: none")],[createInput(Client.t(Word.JobName),"jobName",function(s)
  {
   return[s!=="","This field is required"];
  }),Doc.Element("h4",[],[Doc.TextNode(Client.t(Word.Employer))]),Doc.Element("div",[AttrProxy.Create("class","form-group row")],[Doc.Element("div",[AttrProxy.Create("class","col-lg-3")],[Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btn-block"),AttrProxy.Create("id","btnLoadFromWebsite"),AttrProxy.Create("value",Client.t(Word.LoadFromWebsite)),AttrModule.Handler("click",function()
  {
   return function()
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.readWebsite:-2112793874",[Global.document.getElementById("txtReadEmployerFromWebsite").value]),function(a)
     {
      Var.Set(varEmployer,a);
      return Concurrency.Bind(fillDocumentValues(),function()
      {
       return Concurrency.Return(null);
      });
     });
    })),null);
   };
  })],[])]),Doc.Element("div",[AttrProxy.Create("class","col-lg-9")],[Doc.Element("input",[AttrProxy.Create("id","txtReadEmployerFromWebsite"),AttrProxy.Create("type","text"),AttrProxy.Create("class","form-control"),AttrProxy.Create("value","https://jobboerse.arbeitsagentur.de/vamJB/stellenangeboteFinden.html?execution=e4s1&_eventId_detailView&bencs=ECCL4bGU%2BoeU3dXfDx34zLzb40uikic%2B2KKQU5eGJmbIR%2B7U88EatZPz4c6thxWn&bencs=m4%2BYgQaq%2BX3rqfQIFvibQOfuTdWSRPhHFObxFs%2BMsVl5i8Ha2yIwL1W5WT0iPA4PxFEqmlYn%2F%2BS1r%2FuIRfNrBw%3D%3D&bencs=6PQaRUFDQLZ%2BGNPAPRG8v%2BzbdKHav8zjyetSZpAojmXOPuJQd%2F4O3ojlMh1kXaLryb44mxmmwUNC%2F0m3Nq0xAXci%2FOEbKO0KpeEsoXm%2BGVaRIDnp67LAL434DTMOym9f&bencs=ScHZtBeeBMNt7ILR4tjstoAti5XHVScqFoc6%2FRQffzYt%2FJrTwlVXtA8Y77YD%2Fth0"),AttrModule.Handler("click",function(el)
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
  })],[Doc.Element("i",[AttrProxy.Create("class","fa fa-icon"),AttrProxy.Create("id","faBtnApplyNowTop"),AttrProxy.Create("style","color: #08a81b; margin-right: 10px")],[]),Doc.TextNode(Client.t(Word.ApplyNow))])])]),createInput(Client.t(Word.CompanyName),"company",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(Client.t(Word.Street),"companyStreet",function()
  {
   return[true,""];
  }),createInput(Client.t(Word.Postcode),"companyPostcode",function()
  {
   return[true,""];
  }),createInput(Client.t(Word.City),"companyCity",function()
  {
   return[true,""];
  }),createRadio(Client.t(Word.Gender),List.ofArray([[Client.t(Word.Male),"bossGender","m",""],[Client.t(Word.Female),"bossGender","f",""],[Client.t(Word.UnknownGender),"bossGender","u","checked"]])),createInput(Client.t(Word.Degree),"bossDegree",function()
  {
   return[true,""];
  }),createInput(Client.t(Word.FirstName),"bossFirstName",function()
  {
   return[true,""];
  }),createInput(Client.t(Word.LastName),"bossLastName",function()
  {
   return[true,""];
  }),createInput(Client.t(Word.Email),"bossEmail",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(Client.t(Word.Phone),"bossPhone",function()
  {
   return[true,""];
  }),createInput(Client.t(Word.MobilePhone),"bossMobilePhone",function()
  {
   return[true,""];
  }),Doc.Element("button",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btnLikeLink btn-block"),AttrProxy.Create("style","min-height: 40px; font-size: 20px"),AttrProxy.Create("id","btnApplyNowBottom"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return Concurrency.Start(btnApplyNowClicked(),null);
   };
  })],[Doc.Element("i",[AttrProxy.Create("class","fa fa-icon"),AttrProxy.Create("id","faBtnApplyNowBottom"),AttrProxy.Create("style","color: #08a81b; margin-right: 10px")],[]),Doc.TextNode(Client.t(Word.ApplyNow))])])]);
 };
 Client.showSentJobApplications=function()
 {
  return Doc.Element("h1",[],[Doc.TextNode("hallo")]);
 };
 Client.login=function()
 {
  return Doc.Element("div",[],[Doc.Element("h4",[],[Doc.TextNode(Client.t(Word.Login))]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","txtLoginEmail")],[Doc.TextNode("Email")]),Doc.Element("input",[AttrProxy.Create("class","form-control"),AttrProxy.Create("id","txtLoginEmail")],[])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","txtLoginPassword")],[Doc.TextNode("Password")]),Doc.Element("input",[AttrProxy.Create("type","password"),AttrProxy.Create("class","form-control"),AttrProxy.Create("id","txtLoginPassword")],[])]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("value","Login"),AttrModule.Handler("click",function()
  {
   return function(ev)
   {
    var b;
    Concurrency.Start((b=null,Concurrency.Delay(function()
    {
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.login:2110856612",[Global.String(Global.jQuery("#txtLoginEmail").val()),Global.String(Global.jQuery("#txtLoginPassword").val())]),function(a)
     {
      return a.$==1?(Global.alert(Strings.concat(", ",a.$0)),Concurrency.Zero()):(Global.location.href="",Concurrency.Zero());
     });
    })),null);
    ev.preventDefault();
    return ev.stopPropagation();
   };
  })],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("style","margin-left: 30px;"),AttrProxy.Create("value","Register"),AttrModule.Handler("click",function()
  {
   return function()
   {
    var b;
    return Concurrency.Start((b=null,Concurrency.Delay(function()
    {
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.register:2110856612",[Global.document.getElementById("txtLoginEmail").value,Global.document.getElementById("txtLoginPassword").value]),function(a)
     {
      return a.$==1?(Global.alert(Strings.concat(", ",a.$0)),Concurrency.Zero()):(Global.alert(Client.t(Word.PleaseConfirmYourEmailAddressEmailSubject)),Concurrency.Zero());
     });
    })),null);
   };
  })],[])]);
 };
 Client.t=function(w)
 {
  return Client.varLanguageDict().c.get_Item(w);
 };
 Client.varLanguageDict=function()
 {
  SC$2.$cctor();
  return SC$2.varLanguageDict;
 };
 SC$2.$cctor=function()
 {
  SC$2.$cctor=Global.ignore;
  SC$2.varLanguageDict=Var.Create$1(Map.OfArray(Arrays.ofSeq(Deutsch.dict())));
 };
}());
