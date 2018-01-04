(function()
{
 "use strict";
 var Global,JobApplicationSpam,Types,Gender,Login,UserValues,Employer,JobApplicationPageAction,HtmlPage,FilePage,DocumentPage,DocumentEmail,Document,HtmlPageTemplate,PageDB,Language,Client,Language$1,Str,AddEmployerAction,SC$1,IntelliFactory,Runtime,WebSharper,Operators,UI,Next,Doc,AttrProxy,System,Guid,List,Seq,Concurrency,Collections,Map,Arrays,Var,Utils,Unchecked,Remoting,AjaxRemotingProvider,Enumerator,AttrModule,Strings;
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
 Client=JobApplicationSpam.Client=JobApplicationSpam.Client||{};
 Language$1=Client.Language=Client.Language||{};
 Str=Client.Str=Client.Str||{};
 AddEmployerAction=Client.AddEmployerAction=Client.AddEmployerAction||{};
 SC$1=Global.StartupCode$JobApplicationSpam$Client=Global.StartupCode$JobApplicationSpam$Client||{};
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 WebSharper=Global.WebSharper;
 Operators=WebSharper&&WebSharper.Operators;
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 Doc=Next&&Next.Doc;
 AttrProxy=Next&&Next.AttrProxy;
 System=Global.System;
 Guid=System&&System.Guid;
 List=WebSharper&&WebSharper.List;
 Seq=WebSharper&&WebSharper.Seq;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Collections=WebSharper&&WebSharper.Collections;
 Map=Collections&&Collections.Map;
 Arrays=WebSharper&&WebSharper.Arrays;
 Var=Next&&Next.Var;
 Utils=WebSharper&&WebSharper.Utils;
 Unchecked=WebSharper&&WebSharper.Unchecked;
 Remoting=WebSharper&&WebSharper.Remoting;
 AjaxRemotingProvider=Remoting&&Remoting.AjaxRemotingProvider;
 Enumerator=WebSharper&&WebSharper.Enumerator;
 AttrModule=Next&&Next.AttrModule;
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
  PageIndex:function()
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
 Language.Deutsch={
  $:1
 };
 Language.English={
  $:0
 };
 Language$1.German={
  $:1
 };
 Language$1.English={
  $:0
 };
 Str.StrUserValuesMobilePhone={
  $:30
 };
 Str.StrUserValuesPhone={
  $:29
 };
 Str.StrUserValuesCity={
  $:28
 };
 Str.StrUserValuesPostcode={
  $:27
 };
 Str.StrUserValuesStreet={
  $:26
 };
 Str.StrUserValuesLastName={
  $:25
 };
 Str.StrUserValuesFirstName={
  $:24
 };
 Str.StrUserValuesDegree={
  $:23
 };
 Str.StrUserValuesGender={
  $:22
 };
 Str.StrSubmitEditUserValues={
  $:21
 };
 Str.StrEditUserValues={
  $:20
 };
 Str.StrAddFile={
  $:19
 };
 Str.StrUserAppliesAs={
  $:18
 };
 Str.StrEmailBody={
  $:17
 };
 Str.StrEmailSubject={
  $:16
 };
 Str.StrTemplateName={
  $:15
 };
 Str.StrUploadTemplate={
  $:14
 };
 Str.StrBossMobilePhone={
  $:13
 };
 Str.StrBossPhone={
  $:12
 };
 Str.StrBossEmail={
  $:11
 };
 Str.StrBossLastName={
  $:10
 };
 Str.StrBossFirstName={
  $:9
 };
 Str.StrBossDegree={
  $:8
 };
 Str.StrBossGender={
  $:7
 };
 Str.StrCompanyCity={
  $:6
 };
 Str.StrCompanyPostcode={
  $:5
 };
 Str.StrCompanyStreet={
  $:4
 };
 Str.StrCompanyName={
  $:3
 };
 Str.StrAddEmployer={
  $:2
 };
 Str.StrFemale={
  $:1
 };
 Str.StrMale={
  $:0
 };
 AddEmployerAction.JustAddEmployer={
  $:2
 };
 AddEmployerAction.SendJobApplicationToUserOnly={
  $:1
 };
 AddEmployerAction.ApplyImmediately={
  $:0
 };
 Client.templates=function()
 {
  var varDocument,varUserValues,varUserEmail,varEmployer,varCurrentPageIndex,varDisplayedDocument,showHideMutualElements,b;
  function createInput(t,d)
  {
   return Doc.Element("div",[AttrProxy.Create("class","form-group row")],[Doc.Element("label",[AttrProxy.Create("class","col-sm-3 col-form-label"),AttrProxy.Create("for",d)],[Doc.TextNode(t)]),Doc.Element("div",[AttrProxy.Create("class","col-sm-9")],[Doc.Element("input",[AttrProxy.Create("id",d),AttrProxy.Create("data-"+"bind",d),AttrProxy.Create("class","form-control")],[])])]);
  }
  function createRadio(header,rs)
  {
   var radioGroup,c;
   radioGroup=(c=Guid.NewGuid(),Global.String(c));
   return Doc.Element("div",[],List.ofSeq(Seq.delay(function()
   {
    return Seq.collect(function(i)
    {
     var m,id,c$1;
     m=rs.get_Item(i);
     id=(c$1=Guid.NewGuid(),Global.String(c$1));
     return[Doc.Element("div",[AttrProxy.Create("class","form-group row")],[Doc.Element("label",[AttrProxy.Create("class","col-sm-3 col-form-label")],[Doc.TextNode(i===0?header:"")]),Doc.Element("div",[AttrProxy.Create("class","col-sm-9")],[Doc.Element("input",[AttrProxy.Create("id",id),AttrProxy.Create("type","radio"),AttrProxy.Create("name",radioGroup),AttrProxy.Create("value",m[2]),AttrProxy.Create("data-"+"bind",m[1])],[]),Doc.Element("label",[AttrProxy.Create("for",id)],[Doc.TextNode(m[0])])])])];
    },Operators.range(0,rs.get_Length()-1));
   })));
  }
  function fillDocumentValues()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    var pageMapElements,m,myMap;
    pageMapElements=Global.document.querySelectorAll("[data-page-key]");
    return Concurrency.Combine((m=varDocument.c.pages.get_Item(varCurrentPageIndex.c-1),m.$==1?Concurrency.Zero():(myMap=Map.OfArray(Arrays.ofSeq(m.$0.map)),(Global.jQuery(pageMapElements).each(function($1,el)
    {
     var jEl,key;
     jEl=Global.jQuery(el);
     key=el.getAttribute("data-page-key");
     myMap.ContainsKey(key)?jEl.val(myMap.get_Item(key)):jEl.val(Global.String(el.getAttribute("data-page-value")));
    }),Concurrency.Zero()))),Concurrency.Delay(function()
    {
     return Concurrency.Bind(Concurrency.Sleep(2000),function()
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
       Var.Set(varDocument,(i=varDocument.c,Document.New(i.id,i.name,i.pages,DocumentEmail.New(v,varDocument.c.email.body))));
      }]],["emailBody",["text",function()
      {
       return varDocument.c.email.body;
      },function(v)
      {
       var i;
       Var.Set(varDocument,(i=varDocument.c,Document.New(i.id,i.name,i.pages,DocumentEmail.New(varDocument.c.email.subject,v))));
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
     });
    }));
   });
  }
  function loadPageTemplate()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    Global.jQuery("#insertDiv").remove();
    return Concurrency.Combine(Concurrency.While(function()
    {
     return!Unchecked.Equals(Global.document.getElementById("insertDiv"),null);
    },Concurrency.Delay(function()
    {
     return Concurrency.Bind(Concurrency.Sleep(10),function()
     {
      return Concurrency.Return(null);
     });
    })),Concurrency.Delay(function()
    {
     var pageTemplateIndex;
     pageTemplateIndex=Global.document.getElementById("selectHtmlPageTemplate").selectedIndex;
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getHtmlPageTemplate:-1212795141",[pageTemplateIndex+1]),function(a)
     {
      Var.Set(varDisplayedDocument,Doc.Verbatim(a));
      return Concurrency.Combine(Concurrency.While(function()
      {
       return Unchecked.Equals(Global.document.getElementById("insertDiv"),null);
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
         p=(t=List.splitAt(varCurrentPageIndex.c-1,varDocument.c.pages),(currentAndAfter=t[1],(p$1=currentAndAfter.$==0?Operators.FailWith("pageList was empty"):currentAndAfter.$0.$==1?[new DocumentPage({
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
         })),i$1.email)));
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
    var documentIndex;
    documentIndex=!Unchecked.Equals(Global.document.getElementById("selectDocumentName"),null)?Global.document.getElementById("selectDocumentName").selectedIndex:0;
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getDocumentOffset:-1633111335",[documentIndex]),function(a)
    {
     var c;
     return a==null?Concurrency.Zero():(Var.Set(varDocument,a.$0),Global.document.getElementById("hiddenDocumentId").value=(c=Global.document.getElementById("selectDocumentName").selectedIndex+1,Global.String(c)),Concurrency.Zero());
    });
   });
  }
  function setPageButtons()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    Global.jQuery("#pageButtonsUl li:not(:last-child)").remove();
    return Concurrency.For(varDocument.c.pages,function(a)
    {
     Global.jQuery((((Runtime.Curried3(function($1,$2,$3)
     {
      return $1("<li><button id=\"pageButton"+Global.String($2)+"\" class=\"btnLikeLink\">"+Utils.toSafe($3)+"</button</li>");
     }))(Global.id))(a.PageIndex()))(a.Name())).insertBefore("#addPageButton").on("click",function()
     {
      var htmlPage,b$2;
      return a.$==1?null:(htmlPage=a.$0,Concurrency.Start((b$2=null,Concurrency.Delay(function()
      {
       Global.jQuery("#insertDiv").remove();
       return Concurrency.Combine(Concurrency.While(function()
       {
        return!Unchecked.Equals(Global.document.getElementById("insertDiv"),null);
       },Concurrency.Delay(function()
       {
        return Concurrency.Bind(Concurrency.Sleep(10),function()
        {
         return Concurrency.Return(null);
        });
       })),Concurrency.Delay(function()
       {
        var o;
        return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getHtmlPageTemplate:-1212795141",[(o=htmlPage.oTemplateId,o==null?1:o.$0)]),function(a$1)
        {
         Var.Set(varDisplayedDocument,Doc.Verbatim(a$1));
         return Concurrency.Combine(Concurrency.While(function()
         {
          return Unchecked.Equals(Global.document.getElementById("insertDiv"),null);
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
           show(List.ofArray(["displayedDocumentDiv","attachmentsDiv"]));
           return Concurrency.Zero();
          });
         }));
        });
       }));
      })),null));
     });
     return Concurrency.Zero();
    });
   });
  }
  function addSelectOption(el,value)
  {
   var optionEl;
   optionEl=Global.document.createElement("option");
   optionEl.textContent=value;
   return el.add(optionEl);
  }
  varDocument=Var.CreateWaiting();
  varUserValues=Var.Create$1(UserValues.New(Gender.Female,"","","","","","","",""));
  varUserEmail=Var.CreateWaiting();
  varEmployer=Var.Create$1(Employer.New("","","","",Gender.Unknown,"","","","","",""));
  varCurrentPageIndex=Var.Create$1(1);
  varDisplayedDocument=Var.Create$1(Doc.Element("div",[],[]));
  showHideMutualElements=List.ofArray(["createFilePageDiv","createHtmlPageDiv","choosePageTypeDiv","emailDiv","newDocumentDiv","editUserValuesDiv","addEmployerDiv","displayedDocumentDiv","attachmentsDiv"]);
  Concurrency.Start((b=null,Concurrency.Delay(function()
  {
   var menuDiv;
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
    _this=Global.jQuery(menuDiv);
    return _this.append.apply(_this,[li]);
   }
   menuDiv=Global.document.getElementById("sidebarMenuDiv");
   addMenuEntry("Add employer and apply",function()
   {
    return function()
    {
     return show(List.ofArray(["addEmployerDiv"]));
    };
   });
   addMenuEntry("Edit your values",function()
   {
    return function()
    {
     return show(List.ofArray(["editUserValuesDiv"]));
    };
   });
   addMenuEntry("Edit email",function()
   {
    return function()
    {
     return show(List.ofArray(["emailDiv"]));
    };
   });
   addMenuEntry("Edit attachments",function()
   {
    return function()
    {
     return show(List.ofArray(["attachmentsDiv"]));
    };
   });
   return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getCurrentUserValues:-337599557",[]),function(a)
   {
    Var.Set(varUserValues,a);
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getDocumentNames:1994133801",[]),function(a$1)
    {
     var selectDocumentNameEl;
     selectDocumentNameEl=Global.document.getElementById("selectDocumentName");
     return Concurrency.Combine(Concurrency.For(a$1,function(a$2)
     {
      addSelectOption(selectDocumentNameEl,a$2);
      return Concurrency.Zero();
     }),Concurrency.Delay(function()
     {
      return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getLastEditedDocumentId:-646436276",[]),function(a$2)
      {
       return Concurrency.Combine(a$2!=null&&a$2.$==1?(selectDocumentNameEl.selectedIndex=a$2.$0-1,Concurrency.Zero()):(selectDocumentNameEl.selectedIndex=0,Concurrency.Zero()),Concurrency.Delay(function()
       {
        return Concurrency.Bind(setDocument(),function()
        {
         return Concurrency.Bind(setPageButtons(),function()
         {
          return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getHtmlPageTemplates:1297380307",[]),function(a$3)
          {
           var selectHtmlPageTemplateEl;
           selectHtmlPageTemplateEl=Global.document.getElementById("selectHtmlPageTemplate");
           return Concurrency.Combine(Concurrency.For(a$3,function(a$4)
           {
            addSelectOption(selectHtmlPageTemplateEl,a$4.name);
            return Concurrency.Zero();
           }),Concurrency.Delay(function()
           {
            return Concurrency.Bind(fillDocumentValues(),function()
            {
             show(List.ofArray(["addEmployerDiv"]));
             return Concurrency.Zero();
            });
           }));
          });
         });
        });
       }));
      });
     }));
    });
   });
  })),null);
  return Doc.Element("div",[],[Doc.Element("h4",[],[Doc.TextNode("Your application documents: ")]),Doc.Element("select",[AttrProxy.Create("id","selectDocumentName"),AttrModule.Handler("change",function()
  {
   return function()
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
   };
  })],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("style","margin-left: 20px"),AttrProxy.Create("class",".btnLikeLink"),AttrProxy.Create("value","+"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return show(List.ofArray(["newDocumentDiv"]));
   };
  })],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("class",".btnLikeLink"),AttrProxy.Create("style","margin-left: 20px"),AttrProxy.Create("value","-"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return null;
   };
  })],[]),Doc.Element("hr",[],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btnLikeLink"),AttrProxy.Create("value","Email data"),AttrProxy.Create("style","display: none"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return show(List.ofArray(["emailDiv"]));
   };
  })],[]),Doc.Element("div",[AttrProxy.Create("id","attachmentsDiv")],[Doc.Element("h4",[],[Doc.TextNode("Your attachments:")]),Doc.Element("ul",[AttrProxy.Create("id","pageButtonsUl"),AttrProxy.Create("style","list-style-type: none; padding: 0; margin: 0;")],[Doc.Element("li",[],[Doc.Element("button",[AttrProxy.Create("id","addPageButton"),AttrProxy.Create("class","btnLikeLink"),AttrModule.Handler("click",function()
  {
   return function()
   {
    show(List.ofArray(["choosePageTypeDiv","attachmentsDiv","createFilePageDiv"]));
    Global.document.getElementById("rbFilePage").checked=true;
   };
  })],[Doc.TextNode("+")])])])]),Doc.Element("div",[AttrProxy.Create("id","newDocumentDiv")],[Doc.TextNode("Document name: "),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("id","txtNewTemplateName")],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btnLikeLink"),AttrProxy.Create("value","Add document"),AttrModule.Handler("click",function()
  {
   return function()
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     var newDocumentName;
     newDocumentName=Global.String(Global.document.getElementById("txtNewDocumentName").value);
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.addNewDocument:-1276324058",[newDocumentName]),function()
     {
      addSelectOption("selectDocumentName",newDocumentName);
      return Concurrency.Zero();
     });
    })),null);
   };
  })],[])]),Doc.Element("div",[AttrProxy.Create("id","displayedDocumentDiv")],[Doc.Element("select",[AttrProxy.Create("id","selectHtmlPageTemplate"),AttrModule.Handler("change",function()
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
  })],[]),Doc.EmbedView(varDisplayedDocument.v)]),Doc.Element("div",[AttrProxy.Create("id","emailDiv")],[Doc.Element("h4",[],[Doc.TextNode("Email")]),Doc.TextNode("Email subject:"),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("data-"+"bind","emailSubject"),AttrProxy.Create("style","width:100%")],[]),Doc.Element("br",[],[]),Doc.TextNode("Email body:"),Doc.Element("br",[],[]),Doc.Element("textarea",[AttrProxy.Create("data-"+"bind","emailBody"),AttrProxy.Create("style","min-height:300px; width: 100%")],[])]),Doc.Element("div",[AttrProxy.Create("id","choosePageTypeDiv")],[Doc.Element("input",[AttrProxy.Create("type","radio"),AttrProxy.Create("name","rbgrpPageType"),AttrProxy.Create("id","rbHtmlPage"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return show(List.ofArray(["attachmentsDiv","choosePageTypeDiv","createHtmlPageDiv"]));
   };
  })],[]),Doc.Element("label",[AttrProxy.Create("for","rbHtmlPage")],[Doc.TextNode("Create online")]),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","radio"),AttrProxy.Create("id","rbFilePage"),AttrProxy.Create("name","rbgrpPageType"),AttrModule.Handler("click",function()
  {
   return function()
   {
    var c;
    show(List.ofArray(["attachmentsDiv","choosePageTypeDiv","createFilePageDiv"]));
    Global.document.getElementById("hiddenDocumentId").value=(c=Global.document.getElementById("selectDocumentName").selectedIndex+1,Global.String(c));
   };
  })],[]),Doc.Element("label",[AttrProxy.Create("for","rbFilePage")],[Doc.TextNode("File upload")]),Doc.Element("br",[],[]),Doc.Element("br",[],[])]),Doc.Element("div",[AttrProxy.Create("id","createHtmlPageDiv")],[Doc.Element("input",[AttrProxy.Create("id","")],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("button",[AttrProxy.Create("type","submit")],[Doc.TextNode("Add html attachment")])]),Doc.Element("div",[AttrProxy.Create("id","createFilePageDiv")],[Doc.Element("form",[AttrProxy.Create("enctype","multipart/form-data"),AttrProxy.Create("method","POST"),AttrProxy.Create("action","")],[Doc.TextNode("Please choose a file: "),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","file"),AttrProxy.Create("name","file")],[]),Doc.Element("input",[AttrProxy.Create("type","hidden"),AttrProxy.Create("id","hiddenDocumentId"),AttrProxy.Create("name","documentId")],[]),Doc.Element("input",[AttrProxy.Create("type","hidden"),AttrProxy.Create("name","pageIndex"),AttrProxy.Create("value",Global.String(Global.jQuery("#pageButtonsUl li").length-1))],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("button",[AttrProxy.Create("type","submit")],[Doc.TextNode("Add attachment")])])]),Doc.Element("div",[AttrProxy.Create("id","editUserValuesDiv")],[Doc.Element("h4",[],[Doc.TextNode("Your values")]),createInput("Degree","userDegree"),createRadio("Gender",List.ofArray([["male","userGender","m"],["female","userGender","f"]])),createInput("First name","userFirstName"),createInput("Last name","userLastName"),createInput("Street","userStreet"),createInput("Postcode","userPostcode"),createInput("City","userCity"),createInput("Phone","userPhone"),createInput("Mobile phone","userMobilePhone")]),Doc.Element("div",[AttrProxy.Create("id","addEmployerDiv")],[Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("value","Load from website"),AttrModule.Handler("click",function()
  {
   return function()
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.readWebsite:-2112793874",[Global.document.getElementById("txtReadEmployerFromWebsite").value]),function(a)
     {
      Var.Set(varEmployer,a);
      Global.alert(varEmployer.c.company);
      return Concurrency.Bind(fillDocumentValues(),function()
      {
       Global.alert(varEmployer.c.company);
       return Concurrency.Zero();
      });
     });
    })),null);
   };
  })],[]),Doc.Element("input",[AttrProxy.Create("id","txtReadEmployerFromWebsite"),AttrProxy.Create("type","text"),AttrProxy.Create("value","https://jobboerse.arbeitsagentur.de/vamJB/stellenangeboteFinden.html?execution=e4s1&_eventId_detailView&bencs=ECCL4bGU%2BoeU3dXfDx34zLzb40uikic%2B2KKQU5eGJmbIR%2B7U88EatZPz4c6thxWn&bencs=m4%2BYgQaq%2BX3rqfQIFvibQOfuTdWSRPhHFObxFs%2BMsVl5i8Ha2yIwL1W5WT0iPA4PxFEqmlYn%2F%2BS1r%2FuIRfNrBw%3D%3D&bencs=6PQaRUFDQLZ%2BGNPAPRG8v%2BzbdKHav8zjyetSZpAojmXOPuJQd%2F4O3ojlMh1kXaLryb44mxmmwUNC%2F0m3Nq0xAXci%2FOEbKO0KpeEsoXm%2BGVaRIDnp67LAL434DTMOym9f&bencs=ScHZtBeeBMNt7ILR4tjstoAti5XHVScqFoc6%2FRQffzYt%2FJrTwlVXtA8Y77YD%2Fth0")],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btnLikeLink"),AttrProxy.Create("value","Apply now"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return Concurrency.Start((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.applyNowWithHtmlTemplate:2103313263",[varEmployer.c,varDocument.c,varUserValues.c]),null);
   };
  })],[]),Doc.Element("h4",[],[Doc.TextNode("Employer")]),createInput("Company name","company"),createInput("Street","companyStreet"),createInput("Postcode","companyPostcode"),createInput("City","companyCity"),createRadio("Gender",List.ofArray([["male","bossGender","m"],["female","bossGender","f"]])),createInput("Degree","bossDegree"),createInput("First name","bossFirstName"),createInput("Last name","bossLastName"),createInput("Email","bossEmail"),createInput("Phone","bossPhone"),createInput("Mobile phone","bossMobilePhone"),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("class","btnLikeLink"),AttrProxy.Create("value","Apply now"),AttrModule.Handler("click",function()
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
 Client.currentLanguage=function()
 {
  SC$1.$cctor();
  return SC$1.currentLanguage;
 };
 Client.translate=function(language,str)
 {
  return language.$==1?str.$==1?"weiblich":str.$==2?"Arbeitgeber eingeben":str.$==3?"Firmenname":str.$==4?"Straße":str.$==5?"Postleitzahl":str.$==6?"Stadt":str.$==7?"Geschlecht":str.$==8?"Titel":str.$==9?"Vorname":str.$==10?"Nachname":str.$==11?"Email":str.$==12?"Telefon":str.$==13?"Mobiltelefon":str.$==14?"Vorlage hochladen":str.$==16?"Email-Betreff":str.$==17?"Email-Text":str.$==15?"Name der Vorlage":str.$==18?"Beruf":str.$==19?"Datei hinzufügen":str.$==20?"Angaben zum Bewerber":str.$==21?"Speichern":str.$==22?"Geschlecht":str.$==23?"Titel":str.$==24?"Vorname":str.$==25?"Nachname":str.$==26?"Straße":str.$==27?"PLZ":str.$==28?"Stadt":str.$==29?"Telefon":str.$==30?"Mobiltelefon":"männlich":str.$==1?"female":str.$==2?"Add Employer":str.$==3?"Company name":str.$==9?"First name":str.$==10?"Last name":str.$==4?"Street":str.$==5?"Postcode":str.$==6?"City":str.$==7?"Gender":str.$==8?"Degree":str.$==11?"Email":str.$==12?"Phone":str.$==13?"Mobile phone":str.$==14?"Add template":str.$==16?"Email subject":str.$==17?"Email body":str.$==15?"Template name":str.$==18?"Job":str.$==19?"Add file":str.$==20?"Edit your values":str.$==21?"Save values":str.$==22?"Gender":str.$==23?"Degree":str.$==24?"First name":str.$==25?"Last name":str.$==26?"Street":str.$==27?"Postcode":str.$==28?"City":str.$==29?"Phone":str.$==30?"Mobile phone":"male";
 };
 SC$1.$cctor=function()
 {
  SC$1.$cctor=Global.ignore;
  SC$1.currentLanguage=Language$1.German;
 };
}());
