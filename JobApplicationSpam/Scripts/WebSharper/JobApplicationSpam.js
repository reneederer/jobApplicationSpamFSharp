(function()
{
 "use strict";
 var Global,JobApplicationSpam,Types,Gender,Login,UserValues,Employer,JobApplicationPageAction,HtmlPage,FilePage,DocumentPage,DocumentEmail,Document,HtmlPageTemplate,PageDB,Language,Word,Deutsch,SC$1,Client,IntelliFactory,Runtime,WebSharper,Operators,List,Math,UI,Next,Var,Doc,AttrProxy,AttrModule,System,Guid,Concurrency,Collections,Map,Arrays,Utils,Unchecked,Remoting,AjaxRemotingProvider,Enumerator,Strings,Seq;
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
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 WebSharper=Global.WebSharper;
 Operators=WebSharper&&WebSharper.Operators;
 List=WebSharper&&WebSharper.List;
 Math=Global.Math;
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 Var=Next&&Next.Var;
 Doc=Next&&Next.Doc;
 AttrProxy=Next&&Next.AttrProxy;
 AttrModule=Next&&Next.AttrModule;
 System=Global.System;
 Guid=System&&System.Guid;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Collections=WebSharper&&WebSharper.Collections;
 Map=Collections&&Collections.Map;
 Arrays=WebSharper&&WebSharper.Arrays;
 Utils=WebSharper&&WebSharper.Utils;
 Unchecked=WebSharper&&WebSharper.Unchecked;
 Remoting=WebSharper&&WebSharper.Remoting;
 AjaxRemotingProvider=Remoting&&Remoting.AjaxRemotingProvider;
 Enumerator=WebSharper&&WebSharper.Enumerator;
 Strings=WebSharper&&WebSharper.Strings;
 Seq=WebSharper&&WebSharper.Seq;
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
 Document.New=function(id,name,pages,email)
 {
  return{
   id:id,
   name:name,
   pages:pages,
   email:email
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
  SC$1.dict=List.ofArray([[Word.AddEmployerAndApply,"Bewerben"],[Word.EditYourValues,"Deine Daten"],[Word.EditEmail,"Email"],[Word.EditAttachments,"Anhänge"],[Word.YourApplicationDocuments,"Deine Bewerbungsunterlagen"],[Word.LoadFromWebsite,"Von Website lesen"],[Word.ApplyNow,"Jetzt bewerben"],[Word.CompanyName,"Firmenname"],[Word.Street,"Straße"],[Word.Postcode,"Postleitzahl"],[Word.City,"Stadt"],[Word.Gender,"Geschlecht"],[Word.Degree,"Titel"],[Word.FirstName,"Vorname"],[Word.LastName,"Nachname"],[Word.Email,"Email"],[Word.Phone,"Telefonnummer"],[Word.MobilePhone,"Mobilnummer"],[Word.YourValues,"Deine Daten"],[Word.EmailSubject,"Betreff"],[Word.EmailBody,"Text"],[Word.YourAttachments,"Deine Anhänge"],[Word.CreateOnline,"Online erstellen"],[Word.UploadFile,"Datei hochladen"],[Word.PleaseChooseAFile,"Bitte eine Datei aussuchen"],[Word.AddAttachment,"Anhang hinzufügen"],[Word.YouMightWantToReplaceSomeWordsInYourFileWithVariables,"Vielleicht möchtest du einige Wörter in deiner Datei durch Variablen ersetzen."],[Word.VariablesWillBeReplacedWithTheRightValuesEveryTimeYouSendYourApplication,"Variablen werden bei jedem Versenden einer Bewerbung durch die richtigen Werte ersetzt."],[Word.Male,"männlich"],[Word.Female,"weiblich"],[Word.UnknownGender,"unbekannt"],[Word.AddDocument,"Dokument hinzufügen"],[Word.DocumentName,"Name des Dokuments"],[Word.AddHtmlAttachment,"Html Anhang hinzufügen"],[Word.Employer,"Arbeitgeber"],[Word.JustDownload,"Nur downloaden"],[Word.DownloadWithReplacedVariables,"Variablen ersetzen und downloaden"],[Word.ReallyDeleteDocument,"Document {0} wirklich löschen?"],[Word.ReallyDeletePage,"Seite {0} wirklich löschen?"]]);
 };
 Client.templates=function()
 {
  var varDocument,varUserValues,varUserEmail,varEmployer,varDisplayedDocument,varLanguageDict,i,o,str,o$1,showHideMutualElements,b,m,x,labelText,dataBind;
  function getCurrentPageIndex()
  {
   return Math.max(Global.jQuery("#ulPageButtons .active").first().index()+1,1);
  }
  function t(w)
  {
   Var.Set(i,i.c+1);
   return varLanguageDict.c.get_Item(w);
  }
  function createInputWithColumnSizes(column1Size,column2Size,labelText$1,dataBind$1,validFun)
  {
   return Doc.Element("div",[AttrProxy.Create("class","form-group row")],[Doc.Element("label",[AttrProxy.Create("class",column1Size+" col-form-label"),AttrProxy.Create("for",dataBind$1)],[Doc.TextNode(labelText$1)]),Doc.Element("div",[AttrProxy.Create("class",column2Size)],[Doc.Element("input",[AttrProxy.Create("id",dataBind$1),AttrProxy.Create("data-"+"bind",dataBind$1),AttrProxy.Create("class","form-control"),AttrModule.Handler("blur",function(el)
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
   return Doc.Element("div",[],List.mapi(function(i$1,t$1)
   {
    var id,c$1;
    id=(c$1=Guid.NewGuid(),Global.String(c$1));
    return Doc.Element("div",[AttrProxy.Create("class","form-group row")],[Doc.Element("label",[AttrProxy.Create("class",column1Size+" col-form-label")],[Doc.TextNode(i$1===0?labelText$1:"")]),Doc.Element("div",[AttrProxy.Create("class",column2Size)],[Doc.Element("input",[AttrProxy.Create("id",id),AttrProxy.Create("type","radio"),AttrProxy.Create("name",radioGroup),AttrProxy.Create("value",t$1[2]),AttrProxy.Create("data-"+"bind",t$1[1]),AttrProxy.Create("checked",t$1[3])],[]),Doc.Element("label",[AttrProxy.Create("for",id)],[Doc.TextNode(t$1[0])])])]);
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
    var pageMapElements,m$1,myMap;
    return varDocument.c.pages.$!==0?(pageMapElements=Global.document.querySelectorAll("[data-page-key]"),Concurrency.Combine((m$1=varDocument.c.pages.get_Item(getCurrentPageIndex()-1),m$1.$==1?Concurrency.Zero():(myMap=Map.OfArray(Arrays.ofSeq(m$1.$0.map)),(Global.jQuery(pageMapElements).each(function($1,el)
    {
     var jEl,key;
     jEl=Global.jQuery(el);
     key=el.getAttribute("data-page-key");
     myMap.ContainsKey(key)?jEl.val(myMap.get_Item(key)):jEl.val(Global.String(el.getAttribute("data-page-value")));
    }),Concurrency.Zero()))),Concurrency.Delay(function()
    {
     var map;
     map=Map.OfArray(Arrays.ofSeq(List.ofArray([["userGender",["radio",function()
     {
      return Global.String(varUserValues.c.gender);
     },function(v)
     {
      var i$1;
      Var.Set(varUserValues,(i$1=varUserValues.c,UserValues.New(Gender.fromString(v),i$1.degree,i$1.firstName,i$1.lastName,i$1.street,i$1.postcode,i$1.city,i$1.phone,i$1.mobilePhone)));
     }]],["userDegree",["text",function()
     {
      return varUserValues.c.degree;
     },function(v)
     {
      var i$1;
      Var.Set(varUserValues,(i$1=varUserValues.c,UserValues.New(i$1.gender,v,i$1.firstName,i$1.lastName,i$1.street,i$1.postcode,i$1.city,i$1.phone,i$1.mobilePhone)));
     }]],["userFirstName",["text",function()
     {
      return varUserValues.c.firstName;
     },function(v)
     {
      var i$1;
      Var.Set(varUserValues,(i$1=varUserValues.c,UserValues.New(i$1.gender,i$1.degree,v,i$1.lastName,i$1.street,i$1.postcode,i$1.city,i$1.phone,i$1.mobilePhone)));
     }]],["userLastName",["text",function()
     {
      return varUserValues.c.lastName;
     },function(v)
     {
      var i$1;
      Var.Set(varUserValues,(i$1=varUserValues.c,UserValues.New(i$1.gender,i$1.degree,i$1.firstName,v,i$1.street,i$1.postcode,i$1.city,i$1.phone,i$1.mobilePhone)));
     }]],["userStreet",["text",function()
     {
      return varUserValues.c.street;
     },function(v)
     {
      var i$1;
      Var.Set(varUserValues,(i$1=varUserValues.c,UserValues.New(i$1.gender,i$1.degree,i$1.firstName,i$1.lastName,v,i$1.postcode,i$1.city,i$1.phone,i$1.mobilePhone)));
     }]],["userPostcode",["text",function()
     {
      return varUserValues.c.postcode;
     },function(v)
     {
      var i$1;
      Var.Set(varUserValues,(i$1=varUserValues.c,UserValues.New(i$1.gender,i$1.degree,i$1.firstName,i$1.lastName,i$1.street,v,i$1.city,i$1.phone,i$1.mobilePhone)));
     }]],["userCity",["text",function()
     {
      return varUserValues.c.city;
     },function(v)
     {
      var i$1;
      Var.Set(varUserValues,(i$1=varUserValues.c,UserValues.New(i$1.gender,i$1.degree,i$1.firstName,i$1.lastName,i$1.street,i$1.postcode,v,i$1.phone,i$1.mobilePhone)));
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
      var i$1;
      Var.Set(varUserValues,(i$1=varUserValues.c,UserValues.New(i$1.gender,i$1.degree,i$1.firstName,i$1.lastName,i$1.street,i$1.postcode,i$1.city,v,i$1.mobilePhone)));
     }]],["userMobilePhone",["text",function()
     {
      return varUserValues.c.mobilePhone;
     },function(v)
     {
      var i$1;
      Var.Set(varUserValues,(i$1=varUserValues.c,UserValues.New(i$1.gender,i$1.degree,i$1.firstName,i$1.lastName,i$1.street,i$1.postcode,i$1.city,i$1.phone,v)));
     }]],["company",["text",function()
     {
      return varEmployer.c.company;
     },function(v)
     {
      var i$1;
      Var.Set(varEmployer,(i$1=varEmployer.c,Employer.New(v,i$1.street,i$1.postcode,i$1.city,i$1.gender,i$1.degree,i$1.firstName,i$1.lastName,i$1.email,i$1.phone,i$1.mobilePhone)));
     }]],["companyStreet",["text",function()
     {
      return varEmployer.c.street;
     },function(v)
     {
      var i$1;
      Var.Set(varEmployer,(i$1=varEmployer.c,Employer.New(i$1.company,v,i$1.postcode,i$1.city,i$1.gender,i$1.degree,i$1.firstName,i$1.lastName,i$1.email,i$1.phone,i$1.mobilePhone)));
     }]],["companyPostcode",["text",function()
     {
      return varEmployer.c.postcode;
     },function(v)
     {
      var i$1;
      Var.Set(varEmployer,(i$1=varEmployer.c,Employer.New(i$1.company,i$1.street,v,i$1.city,i$1.gender,i$1.degree,i$1.firstName,i$1.lastName,i$1.email,i$1.phone,i$1.mobilePhone)));
     }]],["companyCity",["text",function()
     {
      return varEmployer.c.city;
     },function(v)
     {
      var i$1;
      Var.Set(varEmployer,(i$1=varEmployer.c,Employer.New(i$1.company,i$1.street,i$1.postcode,v,i$1.gender,i$1.degree,i$1.firstName,i$1.lastName,i$1.email,i$1.phone,i$1.mobilePhone)));
     }]],["bossGender",["radio",function()
     {
      return Global.String(varEmployer.c.gender);
     },function(v)
     {
      var i$1;
      Var.Set(varEmployer,(i$1=varEmployer.c,Employer.New(i$1.company,i$1.street,i$1.postcode,i$1.city,Gender.fromString(v),i$1.degree,i$1.firstName,i$1.lastName,i$1.email,i$1.phone,i$1.mobilePhone)));
     }]],["bossDegree",["text",function()
     {
      return varEmployer.c.degree;
     },function(v)
     {
      var i$1;
      Var.Set(varEmployer,(i$1=varEmployer.c,Employer.New(i$1.company,i$1.street,i$1.postcode,i$1.city,i$1.gender,v,i$1.firstName,i$1.lastName,i$1.email,i$1.phone,i$1.mobilePhone)));
     }]],["bossFirstName",["text",function()
     {
      return varEmployer.c.firstName;
     },function(v)
     {
      var i$1;
      Var.Set(varEmployer,(i$1=varEmployer.c,Employer.New(i$1.company,i$1.street,i$1.postcode,i$1.city,i$1.gender,i$1.degree,v,i$1.lastName,i$1.email,i$1.phone,i$1.mobilePhone)));
     }]],["bossLastName",["text",function()
     {
      return varEmployer.c.lastName;
     },function(v)
     {
      var i$1;
      Var.Set(varEmployer,(i$1=varEmployer.c,Employer.New(i$1.company,i$1.street,i$1.postcode,i$1.city,i$1.gender,i$1.degree,i$1.firstName,v,i$1.email,i$1.phone,i$1.mobilePhone)));
     }]],["bossEmail",["text",function()
     {
      return varEmployer.c.email;
     },function(v)
     {
      var i$1;
      Var.Set(varEmployer,(i$1=varEmployer.c,Employer.New(i$1.company,i$1.street,i$1.postcode,i$1.city,i$1.gender,i$1.degree,i$1.firstName,i$1.lastName,v,i$1.phone,i$1.mobilePhone)));
     }]],["bossPhone",["text",function()
     {
      return varEmployer.c.phone;
     },function(v)
     {
      var i$1;
      Var.Set(varEmployer,(i$1=varEmployer.c,Employer.New(i$1.company,i$1.street,i$1.postcode,i$1.city,i$1.gender,i$1.degree,i$1.firstName,i$1.lastName,i$1.email,v,i$1.mobilePhone)));
     }]],["bossMobilePhone",["text",function()
     {
      return varEmployer.c.mobilePhone;
     },function(v)
     {
      var i$1;
      Var.Set(varEmployer,(i$1=varEmployer.c,Employer.New(i$1.company,i$1.street,i$1.postcode,i$1.city,i$1.gender,i$1.degree,i$1.firstName,i$1.lastName,i$1.email,i$1.phone,v)));
     }]],["emailSubject",["text",function()
     {
      return varDocument.c.email.subject;
     },function(v)
     {
      var i$1;
      Var.Set(varDocument,(i$1=varDocument.c,Document.New(i$1.id,i$1.name,i$1.pages,DocumentEmail.New(v,varDocument.c.email.body))));
     }]],["emailBody",["text",function()
     {
      return varDocument.c.email.body;
     },function(v)
     {
      var i$1;
      Var.Set(varDocument,(i$1=varDocument.c,Document.New(i$1.id,i$1.name,i$1.pages,DocumentEmail.New(varDocument.c.email.subject,v))));
     }]]])));
     return Concurrency.Combine(Concurrency.For(map,function(a)
     {
      var m$2,get,get$1;
      m$2=a.V;
      return m$2[0]==="radio"?(get=m$2[1],(Global.jQuery((function($1)
      {
       return function($2)
       {
        return $1("[data-bind='"+Utils.toSafe($2)+"']");
       };
      }(Global.id))(a.K)).each(function(i$1,el)
      {
       return get()===el.value?void(el.checked=true):null;
      }),Concurrency.Zero())):m$2[0]==="text"?(get$1=m$2[1],(Global.jQuery((function($1)
      {
       return function($2)
       {
        return $1("[data-bind='"+Utils.toSafe($2)+"']");
       };
      }(Global.id))(a.K)).each(function(i$1,el)
      {
       el.value=get$1();
      }),Concurrency.Zero())):(Operators.FailWith("Unknown input type: "+m$2[0]),Concurrency.Zero());
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
         var m$2;
         if(!Unchecked.Equals(updateElement,el))
          {
           m$2=map.get_Item(bindValue);
           m$2[0]==="radio"?updateElement.checked=elValue===updateElement.value:m$2[0]==="text"?updateElement.value=elValue:Operators.FailWith("Unknown input type: "+m$2[0]);
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
    }))):Concurrency.Zero();
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
       Global.jQuery(Global.document.querySelectorAll("[data-page-key]")).each(function(i$1,el)
       {
        var key;
        function eventAction()
        {
         var p,t$1,currentAndAfter,p$1,htmlPage,htmlPage$1,i$2;
         p=(t$1=List.splitAt(getCurrentPageIndex()-1,varDocument.c.pages),(currentAndAfter=t$1[1],(p$1=currentAndAfter.$==0?Operators.FailWith("pageList was empty"):currentAndAfter.$0.$==1?[new DocumentPage({
          $:1,
          $0:currentAndAfter.$0.$0
         }),List.T.Empty]:currentAndAfter.$1.$==0?(htmlPage=currentAndAfter.$0.$0,[new DocumentPage({
          $:0,
          $0:HtmlPage.New(htmlPage.name,htmlPage.oTemplateId,htmlPage.pageIndex,List.ofSeq(Map.ToSeq(Map.OfArray(Arrays.ofSeq(htmlPage.map)).Add(key,Global.String(Global.jQuery(el).val())))))
         }),List.T.Empty]):(htmlPage$1=currentAndAfter.$0.$0,[new DocumentPage({
          $:0,
          $0:HtmlPage.New(htmlPage$1.name,htmlPage$1.oTemplateId,htmlPage$1.pageIndex,List.ofSeq(Map.ToSeq(Map.OfArray(Arrays.ofSeq(htmlPage$1.map)).Add(key,Global.String(Global.jQuery(el).val())))))
         }),currentAndAfter.$1]),[t$1[0],p$1[0],p$1[1]])));
         Var.Set(varDocument,(i$2=varDocument.c,Document.New(i$2.id,i$2.name,List.append(p[0],new List.T({
          $:1,
          $0:p[1],
          $1:p[2]
         })),i$2.email)));
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
     return a==null?(Var.Set(varDocument,Document.New(0,"",List.T.Empty,DocumentEmail.New("",""))),Concurrency.Zero()):(Var.Set(varDocument,a.$0),Global.document.getElementById("hiddenDocumentId").value=Global.String(varDocument.c.id),Concurrency.Zero());
    });
   });
  }
  function setPageButtons()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    Global.jQuery("#ulPageButtons li:not(:last-child)").remove();
    return Concurrency.Combine(Concurrency.For(varDocument.c.pages,function(a)
    {
     var deleteButton,pageUpButton,pageDownButton,_this,_this$1,_this$2;
     deleteButton=Global.jQuery((function($1)
     {
      return function($2)
      {
       return $1("<button class=\"text-right\" id=\"pageButton"+Global.String($2)+"Delete\">-</button>");
      };
     }(Global.id))(a.PageIndex$1())).on("click",function()
     {
      var i$1,t$1,x$1,before,after,b$2;
      return Global.confirm(Strings.SFormat(t(Word.ReallyDeletePage),[a.Name()]))?(Var.Set(varDocument,(i$1=varDocument.c,Document.New(i$1.id,i$1.name,(t$1=(x$1=varDocument.c.pages,List.splitAt(a.PageIndex$1()-1,x$1)),(before=t$1[0],(after=t$1[1],after.$==0?before:List.append(before,List.map(function(p)
      {
       return p.PageIndex(p.PageIndex$1()-1);
      },after.$1))))),i$1.email))),Concurrency.Start((b$2=null,Concurrency.Delay(function()
      {
       return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.overwriteDocument:-229128764",[varDocument.c]),function()
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
       });
      })),null)):null;
     });
     pageUpButton=Global.jQuery((function($1)
     {
      return function($2)
      {
       return $1("<button class=\"text-right\" id=\"pageButton"+Global.String($2)+"Up\">&uarr;</button>");
      };
     }(Global.id))(a.PageIndex$1())).on("click",function()
     {
      var b$2;
      return Concurrency.Start((b$2=null,Concurrency.Delay(function()
      {
       var i$1,t$1,x$1,before,after,x1,x2;
       Var.Set(varDocument,(i$1=varDocument.c,Document.New(i$1.id,i$1.name,(t$1=(x$1=varDocument.c.pages,List.splitAt(a.PageIndex$1()-2,x$1)),(before=t$1[0],(after=t$1[1],after.$==0?before:after.$1.$==0?List.append(before,List.ofArray([after.$0])):(x1=after.$0,(x2=after.$1.$0,List.append(before,new List.T({
        $:1,
        $0:x2.PageIndex(x1.PageIndex$1()),
        $1:new List.T({
         $:1,
         $0:x1.PageIndex(x2.PageIndex$1()),
         $1:after.$1.$1
        })
       }))))))),i$1.email)));
       return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.overwriteDocument:-229128764",[varDocument.c]),function()
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
       });
      })),null);
     });
     pageDownButton=Global.jQuery((function($1)
     {
      return function($2)
      {
       return $1("<button class=\"text-right\" id=\"pageButton"+Global.String($2)+"Delete\">&darr;</button>");
      };
     }(Global.id))(a.PageIndex$1())).on("click",function()
     {
      var b$2;
      return Concurrency.Start((b$2=null,Concurrency.Delay(function()
      {
       var i$1,t$1,x$1,before,after,x1,x2;
       Var.Set(varDocument,(i$1=varDocument.c,Document.New(i$1.id,i$1.name,(t$1=(x$1=varDocument.c.pages,List.splitAt(a.PageIndex$1()-1,x$1)),(before=t$1[0],(after=t$1[1],after.$==0?before:after.$1.$==0?List.append(before,List.ofArray([after.$0])):(x1=after.$0,(x2=after.$1.$0,List.append(before,new List.T({
        $:1,
        $0:x2.PageIndex(x1.PageIndex$1()),
        $1:new List.T({
         $:1,
         $0:x1.PageIndex(x2.PageIndex$1()),
         $1:after.$1.$1
        })
       }))))))),i$1.email)));
       return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.overwriteDocument:-229128764",[varDocument.c]),function()
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
       });
      })),null);
     });
     (_this=(_this$1=(_this$2=Global.jQuery((((Runtime.Curried3(function($1,$2,$3)
     {
      return $1("<li><button id=\"pageButton"+Global.String($2)+"\" class=\"\" style=\"width:80%\">"+Utils.toSafe($3)+"</button></li>");
     }))(Global.id))(a.PageIndex$1()))(a.Name())).insertBefore("#btnAddPage"),_this$2.append.apply(_this$2,[deleteButton])),_this$1.append.apply(_this$1,[pageUpButton])),_this.append.apply(_this,[pageDownButton])).on("click",function()
     {
      var $this,htmlPage,b$2;
      $this=this;
      Global.jQuery(this).addClass("active");
      Global.jQuery(this).parent().parent().find("button").each(function($1,b$3)
      {
       Global.alert("testa");
       !Unchecked.Equals(b$3,$this)?Global.jQuery(b$3).removeClass("active"):void 0;
      });
      Global.alert("currentIndex: "+Global.String(getCurrentPageIndex()));
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
        var o$2;
        return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getHtmlPageTemplate:-1212795141",[(o$2=htmlPage.oTemplateId,o$2==null?1:o$2.$0)]),function(a$1)
        {
         Var.Set(varDisplayedDocument,Doc.Verbatim(a$1));
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
     return Concurrency.Zero();
    }),Concurrency.Delay(function()
    {
     Global.document.getElementById("hiddenNextPageIndex").value=Global.String(Global.jQuery("#ulPageButtons li").length);
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
  varDocument=Var.Create$1(Document.New(0,"",List.T.Empty,DocumentEmail.New("","")));
  varUserValues=Var.Create$1(UserValues.New(Gender.Female,"","","","","","","",""));
  varUserEmail=Var.CreateWaiting();
  varEmployer=Var.Create$1(Employer.New("","","","",Gender.Unknown,"","","","","",""));
  varDisplayedDocument=Var.Create$1(Doc.Element("div",[],[]));
  Var.Create$1(Language.English);
  varLanguageDict=Var.Create$1(Map.OfArray(Arrays.ofSeq(Deutsch.dict())));
  i=Var.Create$1(0);
  Global.alert((o=(str="upload",(o$1=Arrays.tryFind(function(x$1)
  {
   return Strings.StartsWith(x$1,str+"=");
  },Strings.SplitStrings(Global.document.cookie,["; ",";"],0)),o$1==null?null:{
   $:1,
   $0:o$1.$0.substring(str.length+1)
  })),o==null?"no upload":o.$0));
  showHideMutualElements=List.ofArray(["divCreateFilePage","divCreateHtmlPage","divChoosePageType","divEmail","divNewDocument","divEditUserValues","divAddEmployer","divDisplayedDocument","divAttachments"]);
  Concurrency.Start((b=null,Concurrency.Delay(function()
  {
   var divMenu;
   Var.Set(varLanguageDict,Map.OfArray(Arrays.ofSeq(Deutsch.dict())));
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
    addMenuEntry(t(Word.EditYourValues),function()
    {
     return function()
     {
      return show(List.ofArray(["divEditUserValues"]));
     };
    });
    addMenuEntry(t(Word.EditEmail),function()
    {
     return function()
     {
      return show(List.ofArray(["divEmail"]));
     };
    });
    addMenuEntry(t(Word.EditAttachments),function()
    {
     return function()
     {
      return show(List.ofArray(["divAttachments"]));
     };
    });
    addMenuEntry(t(Word.AddEmployerAndApply),function()
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
  return Doc.Element("div",[],[Doc.Element("div",[AttrProxy.Create("style","width : 100%")],[Doc.Element("h4",[],[Doc.TextNode(t(Word.YourApplicationDocuments))]),Doc.Element("select",[AttrProxy.Create("id","slctDocumentName"),AttrModule.Handler("change",function()
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
  })],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("style","margin-left: 20px"),AttrProxy.Create("class",".btnLikeLink"),AttrProxy.Create("value","+"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return show(List.ofArray(["divNewDocument"]));
   };
  })],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("id","btnDeleteDocument"),AttrProxy.Create("class",".btnLikeLink"),AttrProxy.Create("style","margin-left: 20px"),AttrProxy.Create("value","-"),AttrModule.Handler("click",function(el)
  {
   return function()
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     return Global.document.getElementById("slctDocumentName").selectedIndex>=0&&Global.confirm(Strings.SFormat(t(Word.ReallyDeleteDocument),[varDocument.c.name]))?Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.deleteDocument:-1223116942",[varDocument.c.id]),function()
     {
      var slctEl;
      slctEl=Global.document.getElementById("slctDocumentName");
      slctEl.removeChild(slctEl[slctEl.selectedIndex]);
      return Concurrency.Combine(slctEl.length===0?(el.style.display="none",Var.Set(varDocument,Document.New(0,"",List.T.Empty,DocumentEmail.New("",""))),Concurrency.Zero()):Concurrency.Zero(),Concurrency.Delay(function()
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
  })],[])]),Doc.Element("hr",[],[]),Doc.Element("div",[AttrProxy.Create("id","divAttachments"),AttrProxy.Create("style","display: none")],[Doc.Element("h4",[],[Doc.TextNode(t(Word.YourAttachments))]),Doc.Element("ul",[AttrProxy.Create("id","ulPageButtons"),AttrProxy.Create("style","list-style-type: none; padding: 0; margin: 0;")],[Doc.Element("li",[],[Doc.Element("button",[AttrProxy.Create("id","btnAddPage"),AttrProxy.Create("class","btnLikeLink"),AttrModule.Handler("click",function()
  {
   return function()
   {
    show(List.ofArray(["divChoosePageType","divAttachments","divCreateFilePage"]));
    Global.document.getElementById("rbFilePage").checked=true;
   };
  })],[Doc.TextNode("+")])])])]),Doc.Element("div",[AttrProxy.Create("id","divNewDocument"),AttrProxy.Create("style","display: none")],[Doc.TextNode(t(Word.DocumentName)),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("id","txtNewDocumentName"),AttrProxy.Create("autofocus","autofocus")],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btnLikeLink"),AttrProxy.Create("value",t(Word.AddDocument)),AttrModule.Handler("click",function()
  {
   return function()
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     var newDocumentName,i$1;
     newDocumentName=Global.String(Global.document.getElementById("txtNewDocumentName").value);
     Var.Set(varDocument,(i$1=varDocument.c,Document.New(i$1.id,newDocumentName,i$1.pages,i$1.email)));
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.saveNewDocument:-229128764",[varDocument.c]),function(a)
     {
      var i$2,slctEl;
      Var.Set(varDocument,(i$2=varDocument.c,Document.New(a,i$2.name,i$2.pages,i$2.email)));
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
  })],[]),Doc.EmbedView(varDisplayedDocument.v)]),Doc.Element("div",[AttrProxy.Create("id","divUploadedFileDownload"),AttrProxy.Create("style","display: none")],[Doc.Element("form",[AttrModule.Handler("submit",function(el)
  {
   return function()
   {
    var m$1,x$1;
    return el.setAttribute("action",(m$1=(x$1=varDocument.c.pages,Seq.tryItem(getCurrentPageIndex()-1,x$1)),m$1==null?(Global.alert(Global.String(List.length(varDocument.c.pages))+"hallofa"),""):m$1.$0.$==0?(Global.alert("emy"+Global.String(getCurrentPageIndex())),""):"download/"+m$1.$0.$0.path));
   };
  }),AttrProxy.Create("target","_blank"),AttrProxy.Create("method","get")],[Doc.Element("button",[AttrProxy.Create("type","submit")],[Doc.TextNode(t(Word.JustDownload))])]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("form",[AttrProxy.Create("action",(m=(x=varDocument.c.pages,Seq.tryItem(getCurrentPageIndex()-1,x)),m!=null&&m.$==1?m.$0.$==1?m.$0.$0.path:"":"")),AttrProxy.Create("method","get")],[Doc.Element("button",[AttrProxy.Create("type","submit")],[Doc.TextNode(t(Word.DownloadWithReplacedVariables))])])]),Doc.Element("div",[AttrProxy.Create("id","divEmail"),AttrProxy.Create("style","display: none")],[Doc.Element("h4",[],[Doc.TextNode(t(Word.Email))]),createInput(t(Word.EmailSubject),"emailSubject",function(s)
  {
   return[s!=="","Required"];
  }),(labelText=t(Word.EmailBody),(dataBind="emailBody",Doc.Element("div",[AttrProxy.Create("class","form-group row")],[Doc.Element("label",[AttrProxy.Create("class","col-lg-3"+" col-form-label"),AttrProxy.Create("for",dataBind)],[Doc.TextNode(labelText)]),Doc.Element("div",[AttrProxy.Create("class","col-lg-9")],[Doc.Element("textarea",[AttrProxy.Create("id",dataBind),AttrProxy.Create("data-"+"bind",dataBind),AttrProxy.Create("class","form-control"),AttrProxy.Create("style","wrap: soft; white-space: nowrap; overflow: auto; min-height: "+"400px")],[])])])))]),Doc.Element("div",[AttrProxy.Create("id","divChoosePageType"),AttrProxy.Create("style","display: none")],[Doc.Element("input",[AttrProxy.Create("type","radio"),AttrProxy.Create("name","rbgrpPageType"),AttrProxy.Create("id","rbHtmlPage"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return show(List.ofArray(["divAttachments","divChoosePageType","divCreateHtmlPage"]));
   };
  })],[]),Doc.Element("label",[AttrProxy.Create("for","rbHtmlPage")],[Doc.TextNode(t(Word.CreateOnline))]),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","radio"),AttrProxy.Create("id","rbFilePage"),AttrProxy.Create("name","rbgrpPageType"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return show(List.ofArray(["divAttachments","divChoosePageType","divCreateFilePage"]));
   };
  })],[]),Doc.Element("label",[AttrProxy.Create("for","rbFilePage")],[Doc.TextNode(t(Word.UploadFile))]),Doc.Element("br",[],[]),Doc.Element("br",[],[])]),Doc.Element("div",[AttrProxy.Create("id","divCreateHtmlPage"),AttrProxy.Create("style","display: none")],[Doc.Element("input",[AttrProxy.Create("id","txtCreateHtmlPage")],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("button",[AttrProxy.Create("type","submit"),AttrModule.Handler("click",function()
  {
   return function()
   {
    var pageIndex,i$1,b$1;
    pageIndex=Global.jQuery("#ulPageButtons li").length;
    Var.Set(varDocument,(i$1=varDocument.c,Document.New(i$1.id,i$1.name,List.append(varDocument.c.pages,List.ofArray([new DocumentPage({
     $:0,
     $0:HtmlPage.New(Global.document.getElementById("txtCreateHtmlPage").value,{
      $:1,
      $0:1
     },pageIndex,List.T.Empty)
    })])),i$1.email)));
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     return Concurrency.Bind(setPageButtons(),function()
     {
      return Concurrency.Return(null);
     });
    })),null);
   };
  })],[Doc.TextNode(t(Word.AddHtmlAttachment))])]),Doc.Element("div",[AttrProxy.Create("id","divCreateFilePage"),AttrProxy.Create("style","display: none")],[Doc.Element("form",[AttrProxy.Create("enctype","multipart/form-data"),AttrProxy.Create("method","POST"),AttrProxy.Create("action","")],[Doc.TextNode(t(Word.PleaseChooseAFile)),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","file"),AttrProxy.Create("name","file")],[]),Doc.Element("input",[AttrProxy.Create("type","hidden"),AttrProxy.Create("id","hiddenDocumentId"),AttrProxy.Create("name","documentId")],[]),Doc.Element("input",[AttrProxy.Create("type","hidden"),AttrProxy.Create("id","hiddenNextPageIndex"),AttrProxy.Create("name","pageIndex")],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("button",[AttrProxy.Create("type","submit")],[Doc.TextNode(t(Word.AddAttachment))]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("b",[],[Doc.TextNode(t(Word.YouMightWantToReplaceSomeWordsInYourFileWithVariables)+t(Word.VariablesWillBeReplacedWithTheRightValuesEveryTimeYouSendYourApplication))]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.TextNode("$firmaName"),Doc.Element("br",[],[]),Doc.TextNode("$firmaStrasse"),Doc.Element("br",[],[]),Doc.TextNode("$firmaPlz"),Doc.Element("br",[],[]),Doc.TextNode("$firmaStadt"),Doc.Element("br",[],[]),Doc.TextNode("$chefAnredeBriefkopf"),Doc.Element("br",[],[]),Doc.TextNode("$chefAnrede"),Doc.Element("br",[],[]),Doc.TextNode("$geehrter"),Doc.Element("br",[],[]),Doc.TextNode("$chefTitel"),Doc.Element("br",[],[]),Doc.TextNode("$chefVorname"),Doc.Element("br",[],[]),Doc.TextNode("$chefNachname"),Doc.Element("br",[],[]),Doc.TextNode("$chefEmail"),Doc.Element("br",[],[]),Doc.TextNode("$chefTelefon"),Doc.Element("br",[],[]),Doc.TextNode("$chefMobil"),Doc.Element("br",[],[]),Doc.TextNode("$meinGeschlecht"),Doc.Element("br",[],[]),Doc.TextNode("$meinTitel"),Doc.Element("br",[],[]),Doc.TextNode("$meinVorname"),Doc.Element("br",[],[]),Doc.TextNode("$meinNachname"),Doc.Element("br",[],[]),Doc.TextNode("$meineStrasse"),Doc.Element("br",[],[]),Doc.TextNode("$meinePlz"),Doc.Element("br",[],[]),Doc.TextNode("$meineStadt"),Doc.Element("br",[],[]),Doc.TextNode("$meineEmail"),Doc.Element("br",[],[]),Doc.TextNode("$meinMobilTelefon"),Doc.Element("br",[],[]),Doc.TextNode("$meineTelefonnr"),Doc.Element("br",[],[]),Doc.TextNode("$datumHeute")])]),Doc.Element("div",[AttrProxy.Create("id","divEditUserValues"),AttrProxy.Create("style","display: none")],[Doc.Element("h4",[],[Doc.TextNode(t(Word.YourValues))]),createInput(t(Word.Degree),"userDegree",function()
  {
   return[true,""];
  }),createRadio(t(Word.Gender),List.ofArray([[t(Word.Male),"userGender","m",""],[t(Word.Female),"userGender","f",""]])),createInput(t(Word.FirstName),"userFirstName",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(t(Word.LastName),"userLastName",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(t(Word.Street),"userStreet",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(t(Word.Postcode),"userPostcode",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(t(Word.City),"userCity",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(t(Word.Phone),"userPhone",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(t(Word.MobilePhone),"userMobilePhone",function(s)
  {
   return[s!=="","This field is required"];
  })]),Doc.Element("div",[AttrProxy.Create("id","divAddEmployer")],[Doc.Element("h4",[],[Doc.TextNode(t(Word.Employer))]),Doc.Element("div",[AttrProxy.Create("class","form-group row")],[Doc.Element("div",[AttrProxy.Create("class","col-lg-3")],[Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btn-block"),AttrProxy.Create("value",t(Word.LoadFromWebsite)),AttrModule.Handler("click",function()
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
  })],[])])]),Doc.Element("div",[AttrProxy.Create("class","form-group row")],[Doc.Element("div",[AttrProxy.Create("class","col-12")],[Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btn-block"),AttrProxy.Create("value",t(Word.ApplyNow)),AttrModule.Handler("click",function()
  {
   return function()
   {
    return Concurrency.Start((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.applyNowWithHtmlTemplate:2103313263",[varEmployer.c,varDocument.c,varUserValues.c]),null);
   };
  })],[])])]),createInput(t(Word.CompanyName),"company",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(t(Word.Street),"companyStreet",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(t(Word.Postcode),"companyPostcode",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(t(Word.City),"companyCity",function(s)
  {
   return[s!=="","This field is required"];
  }),createRadio(t(Word.Gender),List.ofArray([[t(Word.Male),"bossGender","m",""],[t(Word.Female),"bossGender","f",""],[t(Word.UnknownGender),"bossGender","u","checked"]])),createInput(t(Word.Degree),"bossDegree",function()
  {
   return[true,""];
  }),createInput(t(Word.FirstName),"bossFirstName",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(t(Word.LastName),"bossLastName",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(t(Word.Email),"bossEmail",function(s)
  {
   return[s!=="","This field is required"];
  }),createInput(t(Word.Phone),"bossPhone",function()
  {
   return[true,""];
  }),createInput(t(Word.MobilePhone),"bossMobilePhone",function()
  {
   return[true,""];
  }),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btnLikeLink"),AttrProxy.Create("value",t(Word.ApplyNow)),AttrModule.Handler("click",function()
  {
   return function()
   {
    return Concurrency.Start((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.applyNowWithHtmlTemplate:2103313263",[varEmployer.c,varDocument.c,varUserValues.c]),null);
   };
  })],[])])]);
 };
 Client.showSentJobApplications=function()
 {
  return Doc.Element("h1",[],[Doc.TextNode("hallo")]);
 };
 Client.register=function()
 {
  var varTxtRegisterEmail,varTxtRegisterPassword1,varTxtRegisterPassword2;
  varTxtRegisterEmail=Var.Create$1("");
  varTxtRegisterPassword1=Var.Create$1("");
  varTxtRegisterPassword2=Var.Create$1("");
  return Doc.Element("form",[AttrModule.Handler("submit",function()
  {
   return function()
   {
    var b;
    return Concurrency.Start((b=null,Concurrency.Delay(function()
    {
     (new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.register:-788765221",[varTxtRegisterEmail.c,varTxtRegisterPassword1.c,varTxtRegisterPassword2.c]);
     return Concurrency.Return(null);
    })),null);
   };
  })],[Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","txtRegisterEmail")],[Doc.TextNode("Email")]),Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("id","txtRegisterEmail"),AttrProxy.Create("placeholder","Email")],varTxtRegisterEmail),Doc.Element("label",[AttrProxy.Create("for","txtRegisterPassword1")],[Doc.TextNode("Password")]),Doc.PasswordBox([AttrProxy.Create("class","form-control"),AttrProxy.Create("id","txtRegisterPassword1"),AttrProxy.Create("placeholder","Password")],varTxtRegisterPassword1),Doc.Element("label",[AttrProxy.Create("for","txtRegisterPassword2")],[Doc.TextNode("Password")]),Doc.PasswordBox([AttrProxy.Create("class","form-control"),AttrProxy.Create("id","txtRegisterPassword2"),AttrProxy.Create("placeholder","Repeat Password")],varTxtRegisterPassword2),Doc.Element("input",[AttrProxy.Create("type","submit"),AttrProxy.Create("value","Register")],[])])]);
 };
 Client.login=function()
 {
  var varTxtLoginEmail,varTxtLoginPassword;
  varTxtLoginEmail=Var.Create$1("");
  varTxtLoginPassword=Var.Create$1("");
  return Doc.Element("form",[AttrModule.Handler("submit",function()
  {
   return function(ev)
   {
    var b;
    Concurrency.Start((b=null,Concurrency.Delay(function()
    {
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.login:2110856612",[varTxtLoginEmail.c,varTxtLoginPassword.c]),function(a)
     {
      return a.$==1?(Global.alert(Strings.concat(", ",a.$0)),Concurrency.Zero()):(Global.location.href="",Concurrency.Zero());
     });
    })),null);
    ev.preventDefault();
    ev.stopImmediatePropagation();
    return ev.stopPropagation();
   };
  })],[Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","txtLoginEmail")],[Doc.TextNode("Email")]),Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("id","txtLoginEmail"),AttrProxy.Create("placeholder","Email")],varTxtLoginEmail)]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","txtLoginPassword")],[Doc.TextNode("Password")]),Doc.PasswordBox([AttrProxy.Create("class","form-control"),AttrProxy.Create("id","txtLoginPassword"),AttrProxy.Create("placeholder","Password")],varTxtLoginPassword)]),Doc.Element("input",[AttrProxy.Create("type","submit"),AttrProxy.Create("value","Login")],[])]);
 };
}());
