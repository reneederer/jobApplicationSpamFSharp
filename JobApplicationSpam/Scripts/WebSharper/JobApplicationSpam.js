(function()
{
 "use strict";
 var Global,JobApplicationSpam,Types,Gender,Login,UserValues,Employer,JobApplicationPageAction,HtmlPage,FilePage,DocumentPage,DocumentEmail,Document,HtmlPageTemplate,PageDB,Language,Client,Language$1,Str,AddEmployerAction,SC$1,IntelliFactory,Runtime,WebSharper,Operators,UI,Next,Doc,AttrProxy,Concurrency,Collections,Map,Arrays,List,Var,Utils,Unchecked,Remoting,AjaxRemotingProvider,Enumerator,AttrModule,Strings;
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
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Collections=WebSharper&&WebSharper.Collections;
 Map=Collections&&Collections.Map;
 Arrays=WebSharper&&WebSharper.Arrays;
 List=WebSharper&&WebSharper.List;
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
   return this.$==1?"f":"m";
  }
 },null,Gender);
 Gender.Female=new Gender({
  $:1
 });
 Gender.Male=new Gender({
  $:0
 });
 Gender.fromString=function(v)
 {
  return v==="m"?Gender.Male:v==="f"?Gender.Female:Operators.FailWith("Failed to convert string to gender: "+v);
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
  var varDocument,varUserValues,varUserEmail,varEmployer,varCurrentPageIndex,varDisplayedDocument,varAddPage,showHideMutualElements,b;
  function createInput(t,d)
  {
   return Doc.Element("div",[],[Doc.TextNode(t),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("data-"+"bind",d)],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[])]);
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
     var map;
     map=Map.OfArray(Arrays.ofSeq(List.ofArray([["userDegree",[function()
     {
      return varUserValues.c.degree;
     },function(v)
     {
      var i;
      Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,v,i.firstName,i.lastName,i.street,i.postcode,i.city,i.phone,i.mobilePhone)));
     }]],["userFirstName",[function()
     {
      return varUserValues.c.firstName;
     },function(v)
     {
      var i;
      Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,v,i.lastName,i.street,i.postcode,i.city,i.phone,i.mobilePhone)));
     }]],["userLastName",[function()
     {
      return varUserValues.c.lastName;
     },function(v)
     {
      var i;
      Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,v,i.street,i.postcode,i.city,i.phone,i.mobilePhone)));
     }]],["userStreet",[function()
     {
      return varUserValues.c.street;
     },function(v)
     {
      var i;
      Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,v,i.postcode,i.city,i.phone,i.mobilePhone)));
     }]],["userPostcode",[function()
     {
      return varUserValues.c.postcode;
     },function(v)
     {
      var i;
      Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,i.street,v,i.city,i.phone,i.mobilePhone)));
     }]],["userCity",[function()
     {
      return varUserValues.c.city;
     },function(v)
     {
      var i;
      Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,i.street,i.postcode,v,i.phone,i.mobilePhone)));
     }]],["userEmail",[function()
     {
      return varUserEmail.c;
     },function(v)
     {
      Var.Set(varUserEmail,v);
     }]],["userPhone",[function()
     {
      return varUserValues.c.phone;
     },function(v)
     {
      var i;
      Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,i.street,i.postcode,i.city,v,i.mobilePhone)));
     }]],["userMobilePhone",[function()
     {
      return varUserValues.c.mobilePhone;
     },function(v)
     {
      var i;
      Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,i.street,i.postcode,i.city,i.phone,v)));
     }]],["company",[function()
     {
      return varEmployer.c.company;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(v,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
     }]],["companyStreet",[function()
     {
      return varEmployer.c.street;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,v,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
     }]],["companyPostcode",[function()
     {
      return varEmployer.c.postcode;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,v,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
     }]],["companyCity",[function()
     {
      return varEmployer.c.city;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,v,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
     }]],["bossGender",[function()
     {
      return Global.String(varEmployer.c.gender);
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,Gender.fromString(v),i.degree,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
     }]],["bossDegree",[function()
     {
      return varEmployer.c.degree;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,v,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
     }]],["bossFirstName",[function()
     {
      return varEmployer.c.firstName;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,v,i.lastName,i.email,i.phone,i.mobilePhone)));
     }]],["bossLastName",[function()
     {
      return varEmployer.c.lastName;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,v,i.email,i.phone,i.mobilePhone)));
     }]],["bossEmail",[function()
     {
      return varEmployer.c.email;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,v,i.phone,i.mobilePhone)));
     }]],["bossPhone",[function()
     {
      return varEmployer.c.phone;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,v,i.mobilePhone)));
     }]],["bossMobilePhone",[function()
     {
      return varEmployer.c.mobilePhone;
     },function(v)
     {
      var i;
      Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,v)));
     }]]])));
     return Concurrency.Combine(Concurrency.For(map,function(a)
     {
      Global.jQuery((function($1)
      {
       return function($2)
       {
        return $1("[data-bind='"+Utils.toSafe($2)+"']");
       };
      }(Global.id))(a.K)).val(a.V[0]());
      return Concurrency.Zero();
     }),Concurrency.Delay(function()
     {
      Global.jQuery(Global.document.querySelectorAll("[data-bind]")).each(function($1,el)
      {
       function eventAction()
       {
        var bindValue;
        bindValue=Global.String(Global.jQuery(el).data("bind"));
        Global.jQuery((function($2)
        {
         return function($3)
         {
          return $2("[data-bind='"+Utils.toSafe($3)+"']");
         };
        }(Global.id))(bindValue)).each(function($2,updateElement)
        {
         var elValue;
         elValue=Global.String(Global.jQuery(el).val());
         (map.get_Item(bindValue))[1](elValue);
         !Unchecked.Equals(updateElement,el)?Global.jQuery(updateElement).val(elValue):void 0;
        });
       }
       el.removeEventListener("input",eventAction,true);
       el.addEventListener("input",eventAction,true);
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
    Global.alert("hallo");
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
  function hideAll()
  {
   var e;
   e=Enumerator.Get(showHideMutualElements);
   try
   {
    while(e.MoveNext())
     Global.document.getElementById(e.Current()).style.display="none";
   }
   finally
   {
    if("Dispose"in e)
     e.Dispose();
   }
  }
  function show(elId)
  {
   var e,currentElId;
   Global.document.getElementById(elId).style.display="block";
   e=Enumerator.Get(showHideMutualElements);
   try
   {
    while(e.MoveNext())
     {
      currentElId=e.Current();
      currentElId!==elId?Global.document.getElementById(currentElId).style.display="none":void 0;
     }
   }
   finally
   {
    if("Dispose"in e)
     e.Dispose();
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
     return a==null?Concurrency.Zero():(Var.Set(varDocument,a.$0),Concurrency.Zero());
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
      return $1("<li><button id=\"pageButton"+Global.String($2)+"\">"+Utils.toSafe($3)+"</button</li>");
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
           show("displayedDocumentDiv");
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
  varUserValues=Var.Create$1(UserValues.New(Gender.Male,"dr l","ren","ederer","","","","",""));
  varUserEmail=Var.CreateWaiting();
  varEmployer=Var.Create$1(Employer.New("","","","",Gender.Male,"","empfirstl","","","",""));
  varCurrentPageIndex=Var.Create$1(1);
  varDisplayedDocument=Var.Create$1(Doc.Element("div",[],[]));
  varAddPage=Var.Create$1(Doc.Element("div",[],[]));
  showHideMutualElements=List.ofArray(["createFilePageDiv","createHtmlPageDiv","choosePageTypeDiv","emailDiv","newDocumentDiv","editUserValuesDiv","editEmployerDiv","displayedDocumentDiv"]);
  Concurrency.Start((b=null,Concurrency.Delay(function()
  {
   return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getDocumentNames:1994133801",[]),function(a)
   {
    var selectDocumentNameEl;
    selectDocumentNameEl=Global.document.getElementById("selectDocumentName");
    return Concurrency.Combine(Concurrency.For(a,function(a$1)
    {
     addSelectOption(selectDocumentNameEl,a$1);
     return Concurrency.Zero();
    }),Concurrency.Delay(function()
    {
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getLastEditedDocumentId:-646436276",[]),function(a$1)
     {
      return Concurrency.Combine(a$1!=null&&a$1.$==1?(selectDocumentNameEl.selectedIndex=a$1.$0-1,Concurrency.Zero()):(selectDocumentNameEl.selectedIndex=0,Concurrency.Zero()),Concurrency.Delay(function()
      {
       return Concurrency.Bind(setDocument(),function()
       {
        return Concurrency.Bind(setPageButtons(),function()
        {
         return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getHtmlPageTemplates:1297380307",[]),function(a$2)
         {
          var selectHtmlPageTemplateEl;
          selectHtmlPageTemplateEl=Global.document.getElementById("selectHtmlPageTemplate");
          return Concurrency.Combine(Concurrency.For(a$2,function(a$3)
          {
           addSelectOption(selectHtmlPageTemplateEl,a$3.name);
           return Concurrency.Zero();
          }),Concurrency.Delay(function()
          {
           return Concurrency.Bind(fillDocumentValues(),function()
           {
            hideAll();
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
  })),null);
  return Doc.Element("div",[],[Doc.Element("h3",[],[Doc.TextNode("Your application documents: ")]),Doc.Element("select",[AttrProxy.Create("id","selectDocumentName"),AttrModule.Handler("change",function()
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
       return Concurrency.Return(null);
      });
     });
    })),null);
   };
  })],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("style","margin-left: 20px"),AttrProxy.Create("value","+"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return show("newDocumentDiv");
   };
  })],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("style","margin-left: 20px"),AttrProxy.Create("value","-"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return null;
   };
  })],[]),Doc.Element("div",[],[Doc.Element("h3",[],[Doc.TextNode("Your pages:")]),Doc.Element("ul",[AttrProxy.Create("id","pageButtonsUl")],[Doc.Element("li",[],[Doc.Element("button",[AttrProxy.Create("id","addPageButton"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return null;
   };
  })],[Doc.TextNode("+")])])])]),Doc.Element("hr",[],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("value","Email data"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return show("emailDiv");
   };
  })],[]),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("value","Your values"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return show("editUserValuesDiv");
   };
  })],[]),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("value","Edit employer values"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return show("editEmployerDiv");
   };
  })],[]),Doc.Element("hr",[],[]),Doc.Element("div",[AttrProxy.Create("id","newDocumentDiv")],[Doc.TextNode("Document name: "),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("id","txtNewTemplateName")],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("value","Add document"),AttrModule.Handler("click",function()
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
  })],[]),Doc.EmbedView(varDisplayedDocument.v)]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("div",[AttrProxy.Create("id","emailDiv")],[Doc.Element("h3",[],[Doc.TextNode("Email")]),Doc.TextNode("Email subject:"),Doc.Element("br",[],[]),Doc.Element("input",[AttrModule.Handler("input",function(el)
  {
   return function()
   {
    var i,i$1;
    return Var.Set(varDocument,(i=varDocument.c,Document.New(i.id,i.name,i.pages,(i$1=varDocument.c.email,DocumentEmail.New(el.value,i$1.body)))));
   };
  })],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.TextNode("Email body:"),Doc.Element("br",[],[]),Doc.Element("textarea",[AttrModule.Handler("input",function(el)
  {
   return function()
   {
    var i;
    return Var.Set(varDocument,(i=varDocument.c,Document.New(i.id,i.name,i.pages,DocumentEmail.New(varDocument.c.email.subject,el.value))));
   };
  }),AttrProxy.Create("style","min-height:300px")],[])]),Doc.Element("div",[AttrProxy.Create("id","choosePageTypeDiv")],[Doc.Element("input",[AttrProxy.Create("type","radio"),AttrProxy.Create("name","rbgrpPageType"),AttrProxy.Create("id","rbHtmlPage"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return null;
   };
  })],[]),Doc.Element("label",[AttrProxy.Create("for","rbHtmlPage")],[Doc.TextNode("Create online")]),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","radio"),AttrProxy.Create("id","rbFilePage"),AttrProxy.Create("name","rbgrpPageType"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return null;
   };
  })],[]),Doc.Element("label",[AttrProxy.Create("for","rbFilePage")],[Doc.TextNode("Upload")]),Doc.Element("br",[],[]),Doc.Element("br",[],[])]),Doc.Element("div",[AttrProxy.Create("id","createHtmlPageDiv")],[Doc.Element("input",[AttrProxy.Create("id","")],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("button",[AttrProxy.Create("type","submit")],[Doc.TextNode("Add html page")]),Doc.Element("button",[AttrProxy.Create("style","margin-left: 20px"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return Var.Set(varAddPage,Doc.Element("div",[],[]));
   };
  })],[Doc.TextNode("Abort")])]),Doc.Element("div",[AttrProxy.Create("id","createFilePageDiv")],[Doc.Element("form",[AttrProxy.Create("enctype","multipart/form-data"),AttrProxy.Create("method","POST"),AttrProxy.Create("action","")],[Doc.TextNode("Please choose a file: "),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","file"),AttrProxy.Create("name","file")],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("button",[AttrProxy.Create("type","submit")],[Doc.TextNode("Upload")]),Doc.Element("button",[AttrProxy.Create("style","margin-left: 20px"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return Var.Set(varAddPage,Doc.Element("div",[],[]));
   };
  })],[Doc.TextNode("Abort")])])]),Doc.Element("div",[AttrProxy.Create("id","editUserValuesDiv")],[Doc.Element("h3",[],[Doc.TextNode("Your values")]),createInput("Degree","userDegree"),createInput("First name","userFirstName"),createInput("Last name","userLastName"),createInput("Street","userStreet"),createInput("Postcode","userPostcode"),createInput("City","userCity"),createInput("Phone","userPhone"),createInput("Mobile phone","userMobilePhone")]),Doc.Element("div",[AttrProxy.Create("id","editEmployerDiv")],[Doc.Element("h3",[],[Doc.TextNode("Employer")]),createInput("Company name","company"),createInput("Street","companyStreet"),createInput("Postcode","companyPostcode"),createInput("City","companyCity"),createInput("Degree","bossDegree"),createInput("First name","bossFirstName"),createInput("Last name","bossLastName"),createInput("Phone","bossPhone"),createInput("Mobile phone","bossMobilePhone")]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("value","Save as new document"),AttrModule.Handler("click",function()
  {
   return function()
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.saveNewDocument:-229128764",[varDocument.c]),function()
     {
      return Concurrency.Zero();
     });
    })),null);
   };
  })],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("value","Save as new document"),AttrModule.Handler("click",function()
  {
   return function()
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.saveNewDocument:-229128764",[varDocument.c]),function()
     {
      return Concurrency.Zero();
     });
    })),null);
   };
  })],[]),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("value","Overwrite document"),AttrModule.Handler("click",function()
  {
   return function()
   {
    var b$1;
    return Concurrency.Start((b$1=null,Concurrency.Delay(function()
    {
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.overwriteDocument:-229128764",[varDocument.c]),function()
     {
      return Concurrency.Zero();
     });
    })),null);
   };
  })],[]),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("value","Apply now"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return Concurrency.Start((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.applyNowWithHtmlTemplate:2103313263",[varEmployer.c,varDocument.c,varUserValues.c]),null);
   };
  })],[])]);
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
      return a.$==1?(Global.alert(Strings.concat(", ",a.$0)),Concurrency.Zero()):(Global.location.href="/templates",Concurrency.Zero());
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
