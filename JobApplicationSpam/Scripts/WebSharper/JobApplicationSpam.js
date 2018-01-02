(function()
{
 "use strict";
 var Global,JobApplicationSpam,Types,Gender,Login,UserValues,Employer,JobApplicationPageAction,HtmlPage,FilePage,DocumentPage,DocumentEmail,Document,HtmlPageTemplate,PageDB,Language,Client,Language$1,Str,AddEmployerAction,SC$1,IntelliFactory,Runtime,WebSharper,Operators,UI,Next,Doc,AttrProxy,AttrModule,Concurrency,Var,View,Remoting,AjaxRemotingProvider,List,Seq,Numeric,Unchecked,Collections,Map,Arrays,Utils,Strings;
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
 AttrModule=Next&&Next.AttrModule;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Var=Next&&Next.Var;
 View=Next&&Next.View;
 Remoting=WebSharper&&WebSharper.Remoting;
 AjaxRemotingProvider=Remoting&&Remoting.AjaxRemotingProvider;
 List=WebSharper&&WebSharper.List;
 Seq=WebSharper&&WebSharper.Seq;
 Numeric=WebSharper&&WebSharper.Numeric;
 Unchecked=WebSharper&&WebSharper.Unchecked;
 Collections=WebSharper&&WebSharper.Collections;
 Map=Collections&&Collections.Map;
 Arrays=WebSharper&&WebSharper.Arrays;
 Utils=WebSharper&&WebSharper.Utils;
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
  var varEditUserValuesDiv,varAddEmployerDiv,varEmailDiv,b,varDocument,varUserValues,varUserEmail,varEmployer,varSelectDocumentName,varSelectHtmlPageTemplate,varNewDocument,varPageButtonsDiv,varPageButtons,varCurrentPageIndex,varPageCount,varDisplayedDocument,varAddPage;
  function createInput(t,v,updateF)
  {
   return Doc.Element("div",[],[Doc.TextNode(t),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("value",v),AttrModule.Handler("input",function(el)
   {
    return function()
    {
     updateF(el.value);
     return Concurrency.Start(fillDocumentValues(),null);
    };
   })],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[])]);
  }
  function updateEditUserValuesDiv()
  {
   Var.Set(varEditUserValuesDiv,Doc.Element("div",[],[Doc.TextView(View.Map(function(x)
   {
    return x.firstName;
   },varUserValues.v)),createInput("Degree",varUserValues.c.degree,function(v)
   {
    var i;
    Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,v,i.firstName,i.lastName,i.street,i.postcode,i.city,i.phone,i.mobilePhone)));
   }),createInput("First name",varUserValues.c.firstName,function(v)
   {
    var i;
    Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,v,i.lastName,i.street,i.postcode,i.city,i.phone,i.mobilePhone)));
   }),createInput("Last name",varUserValues.c.lastName,function(v)
   {
    var i;
    Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,v,i.street,i.postcode,i.city,i.phone,i.mobilePhone)));
   }),createInput("Street",varUserValues.c.street,function(v)
   {
    var i;
    Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,v,i.postcode,i.city,i.phone,i.mobilePhone)));
   }),createInput("Postcode",varUserValues.c.postcode,function(v)
   {
    var i;
    Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,i.street,v,i.city,i.phone,i.mobilePhone)));
   }),createInput("City",varUserValues.c.city,function(v)
   {
    var i;
    Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,i.street,i.postcode,v,i.phone,i.mobilePhone)));
   }),createInput("Phone",varUserValues.c.phone,function(v)
   {
    var i;
    Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,i.street,i.postcode,i.city,v,i.mobilePhone)));
   }),createInput("Mobile phone",varUserValues.c.mobilePhone,function(v)
   {
    var i;
    Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,i.street,i.postcode,i.city,i.phone,v)));
   })]));
  }
  function updateAddEmployerDiv()
  {
   Var.Set(varAddEmployerDiv,Doc.Element("div",[],[createInput("Company",varEmployer.c.company,function(v)
   {
    var i;
    Var.Set(varEmployer,(i=varEmployer.c,Employer.New(v,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
   }),createInput("Degree",varEmployer.c.degree,function(v)
   {
    var i;
    Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,v,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
   }),createInput("First name",varEmployer.c.firstName,function(v)
   {
    var i;
    Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,v,i.lastName,i.email,i.phone,i.mobilePhone)));
   }),createInput("Last name",varEmployer.c.lastName,function(v)
   {
    var i;
    Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,v,i.email,i.phone,i.mobilePhone)));
   }),createInput("Street",varEmployer.c.street,function(v)
   {
    var i;
    Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,v,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
   }),createInput("Postcode",varEmployer.c.postcode,function(v)
   {
    var i;
    Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,v,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
   }),createInput("City",varEmployer.c.city,function(v)
   {
    var i;
    Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,v,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
   }),createInput("Email",varEmployer.c.email,function(v)
   {
    var i;
    Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,v,i.phone,i.mobilePhone)));
   }),createInput("Phone",varEmployer.c.phone,function(v)
   {
    var i;
    Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,v,i.mobilePhone)));
   }),createInput("Mobile phone",varEmployer.c.mobilePhone,function(v)
   {
    var i;
    Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,v)));
   })]));
  }
  function updateEmailDiv()
  {
   Var.Set(varEmailDiv,Doc.Element("div",[],[createInput("Email-Subject",varDocument.c.email.subject,function(v)
   {
    var i;
    Var.Set(varDocument,(i=varDocument.c,Document.New(i.id,i.name,i.pages,DocumentEmail.New(v,varDocument.c.email.body))));
   }),createInput("Email-Body",varDocument.c.email.body,function(v)
   {
    var i;
    Var.Set(varDocument,(i=varDocument.c,Document.New(i.id,i.name,i.pages,DocumentEmail.New(varDocument.c.email.subject,v))));
   })]));
  }
  function setSelectDocumentName()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getDocumentNames:1994133801",[]),function(a)
    {
     Var.Set(varSelectDocumentName,Doc.Element("div",[],[Doc.TextNode("Your application documents: "),Doc.Element("select",[AttrProxy.Create("id","selectDocumentName"),AttrModule.Handler("change",function()
     {
      return function()
      {
       return Concurrency.Start(indexChanged_selectDocumentName(),null);
      };
     })],List.ofSeq(Seq.delay(function()
     {
      return Seq.map(function(documentName)
      {
       return Doc.Element("option",[],[Doc.TextNode(documentName)]);
      },a);
     }))),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("style","margin-left: 20px"),AttrProxy.Create("value","+"),AttrModule.Handler("click",function()
     {
      return function()
      {
       return setNewDocument();
      };
     })],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("style","margin-left: 20px"),AttrProxy.Create("value","-"),AttrModule.Handler("click",function()
     {
      return function()
      {
       return Concurrency.Start(setNewDocumentEmpty(),null);
      };
     })],[])]));
     return Concurrency.Zero();
    });
   });
  }
  function saveNewDocument()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.saveNewDocument:-229128764",[varDocument.c]),function()
    {
     return Concurrency.Zero();
    });
   });
  }
  function overwriteDocument()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.overwriteDocument:-229128764",[varDocument.c]),function()
    {
     return Concurrency.Zero();
    });
   });
  }
  function addSelectDocumentName(value)
  {
   var documentNames;
   documentNames=List.T.Empty;
   Global.jQuery("#selectDocumentName option").each(function(i,el)
   {
    documentNames=new List.T({
     $:1,
     $0:Global.String(el.text),
     $1:documentNames
    });
   });
   documentNames=List.append(documentNames,List.ofArray([value]));
   Var.Set(varSelectDocumentName,Doc.Element("div",[],[Doc.TextNode("Your application documents: "),Doc.Element("select",[AttrProxy.Create("id","selectDocumentName"),AttrModule.Handler("change",function()
   {
    return function()
    {
     return Concurrency.Start(indexChanged_selectDocumentName(),null);
    };
   })],List.ofSeq(Seq.delay(function()
   {
    return Seq.map(function(documentName)
    {
     return Doc.Element("option",[],[Doc.TextNode(documentName)]);
    },documentNames);
   }))),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("style","margin-left: 20px"),AttrProxy.Create("value","+"),AttrModule.Handler("click",function()
   {
    return function()
    {
     return setNewDocument();
    };
   })],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("style","margin-left: 20px"),AttrProxy.Create("value","-"),AttrModule.Handler("click",function()
   {
    return function()
    {
     return Concurrency.Start(setNewDocumentEmpty(),null);
    };
   })],[])]));
  }
  function setSelectHtmlPageTemplate()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getHtmlPageTemplates:1297380307",[]),function(a)
    {
     Var.Set(varSelectHtmlPageTemplate,Doc.Element("select",[AttrProxy.Create("id","selectHtmlPageTemplate"),AttrModule.Handler("change",function()
     {
      return function()
      {
       return Concurrency.Start(indexChanged_selectHtmlPageTemplate(),null);
      };
     })],List.ofSeq(Seq.delay(function()
     {
      return Seq.map(function(htmlPageTemplate)
      {
       return Doc.Element("option",[],[Doc.TextNode(htmlPageTemplate.name)]);
      },a);
     }))));
     return Concurrency.Zero();
    });
   });
  }
  function setPageButtons()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    function setPageNameDiv()
    {
     var varPageDiv;
     function addHtmlPage(pageName)
     {
      var b$2;
      b$2=null;
      return Concurrency.Delay(function()
      {
       return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getDocumentIdOffset:-1214783449",[Numeric.ParseInt32(Global.String(Global.document.getElementById("selectDocumentName").selectedIndex))]),function(a)
       {
        return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.addHtmlPage:309601797",[a,null,varPageCount.c+1,pageName]),function()
        {
         Var.Set(varPageButtons,Doc.Element("div",[],List.append(List.ofArray([varPageButtons.c]),List.ofArray([Doc.Element("button",[AttrModule.Handler("click",function()
         {
          return function()
          {
           var b$3;
           return Concurrency.Start((b$3=null,Concurrency.Delay(function()
           {
            var o;
            Global.document.getElementById("selectHtmlPageTemplate").selectedIndex=(o=null,o==null?1:o.$0)-1;
            Var.Set(varCurrentPageIndex,varPageCount.c);
            return Concurrency.Bind(loadPageTemplate(),function()
            {
             return Concurrency.Bind(fillDocumentValues(),function()
             {
              return Concurrency.Return(null);
             });
            });
           })),null);
          };
         })],[Doc.TextNode(pageName)])]))));
         return Concurrency.Zero();
        });
       });
      });
     }
     function htmlPageDiv()
     {
      return Doc.Element("div",[],[Doc.TextNode("Page name: "),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("id","txtAddPageName")],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("button",[AttrModule.Handler("click",function()
      {
       return function()
       {
        Concurrency.Start(addHtmlPage(Global.String(Global.document.getElementById("txtAddPageName").value)),null);
        return Var.Set(varAddPage,Doc.Element("div",[],[]));
       };
      })],[Doc.TextNode("Add page")]),Doc.Element("button",[AttrProxy.Create("style","margin-left: 20px"),AttrModule.Handler("click",function()
      {
       return function()
       {
        return Var.Set(varAddPage,Doc.Element("div",[],[]));
       };
      })],[Doc.TextNode("Abort")])]);
     }
     function filePageDiv()
     {
      var c;
      return Doc.Element("div",[],[Doc.Element("form",[AttrProxy.Create("enctype","multipart/form-data"),AttrProxy.Create("method","POST"),AttrProxy.Create("action","")],[Doc.TextNode("Please choose a file: "),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","file"),AttrProxy.Create("name","file")],[]),Doc.Element("input",[AttrProxy.Create("type","hidden"),AttrProxy.Create("name","documentId"),AttrProxy.Create("value",(c=Global.document.getElementById("selectDocumentName").selectedIndex+1,Global.String(c)))],[]),Doc.Element("input",[AttrProxy.Create("type","hidden"),AttrProxy.Create("name","pageIndex"),AttrProxy.Create("value",Global.String(varPageCount.c+1))],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("button",[AttrProxy.Create("type","submit")],[Doc.TextNode("Add page")]),Doc.Element("button",[AttrProxy.Create("style","margin-left: 20px"),AttrModule.Handler("click",function()
      {
       return function()
       {
        return Var.Set(varAddPage,Doc.Element("div",[],[]));
       };
      })],[Doc.TextNode("Abort")])])]);
     }
     varPageDiv=Var.Create$1(Doc.Element("div",[],[]));
     return Doc.Element("div",[],[Doc.Element("input",[AttrProxy.Create("type","radio"),AttrProxy.Create("name","rbgrpPageType"),AttrProxy.Create("id","rbHtmlPage"),AttrModule.Handler("click",function()
     {
      return function()
      {
       return Var.Set(varPageDiv,htmlPageDiv());
      };
     })],[]),Doc.Element("label",[AttrProxy.Create("for","rbHtmlPage")],[Doc.TextNode("Create online")]),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","radio"),AttrProxy.Create("id","rbFilePage"),AttrProxy.Create("name","rbgrpPageType"),AttrModule.Handler("click",function()
     {
      return function()
      {
       return Var.Set(varPageDiv,filePageDiv());
      };
     })],[]),Doc.Element("label",[AttrProxy.Create("for","rbFilePage")],[Doc.TextNode("Upload")]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.EmbedView(varPageDiv.v)]);
    }
    Var.Set(varAddPage,Doc.Element("div",[],[]));
    Var.Set(varPageCount,varDocument.c.pages.get_Length());
    Var.Set(varPageButtonsDiv,Doc.Element("div",[AttrProxy.Create("id","ulPageButtons")],List.append(List.ofArray([Doc.EmbedView(varPageButtons.v)]),List.ofArray([Doc.Element("div",[],[Doc.Element("button",[AttrModule.Handler("click",function()
    {
     return function()
     {
      return Var.Set(varAddPage,setPageNameDiv());
     };
    })],[Doc.TextNode("+")])])]))));
    Var.Set(varPageButtons,Doc.Element("div",[],List.ofSeq(Seq.delay(function()
    {
     return Seq.collect(function(page)
     {
      var filePage,htmlPage;
      return page.$==1?(filePage=page.$0,[Doc.Element("div",[],[Doc.Element("button",[AttrModule.Handler("click",function()
      {
       return function()
       {
        Var.Set(varCurrentPageIndex,filePage.pageIndex);
        return loadFileUploadTemplate();
       };
      })],[Doc.TextNode(page.Name())])])]):(htmlPage=page.$0,[Doc.Element("div",[],[Doc.Element("button",[AttrModule.Handler("click",function()
      {
       return function()
       {
        var b$2;
        return Concurrency.Start((b$2=null,Concurrency.Delay(function()
        {
         var o;
         Global.document.getElementById("selectHtmlPageTemplate").selectedIndex=(o=htmlPage.oTemplateId,o==null?1:o.$0)-1;
         Var.Set(varCurrentPageIndex,htmlPage.pageIndex);
         return Concurrency.Bind(loadPageTemplate(),function()
         {
          return Concurrency.Bind(fillDocumentValues(),function()
          {
           return Concurrency.Return(null);
          });
         });
        })),null);
       };
      })],[Doc.TextNode(page.Name())])])]);
     },List.sortBy(function(x)
     {
      return x.PageIndex();
     },varDocument.c.pages));
    }))));
    return Concurrency.Zero();
   });
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
     return a==null?Concurrency.Zero():(Var.Set(varDocument,a.$0),updateEmailDiv(),Concurrency.Zero());
    });
   });
  }
  function setUserValues()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getCurrentUserValues:-337599557",[]),function(a)
    {
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getCurrentUserEmail:-834772631",[]),function(a$1)
     {
      Var.Set(varUserValues,a);
      Var.Set(varUserEmail,a$1);
      updateEditUserValuesDiv();
      return Concurrency.Zero();
     });
    });
   });
  }
  function fillDocumentValues()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    var pageMapElements,m,htmlPage;
    pageMapElements=Global.document.querySelectorAll("[data-html-page-key]");
    return Concurrency.Combine((m=varDocument.c.pages.get_Item(varCurrentPageIndex.c-1),m.$==1?Concurrency.Zero():(htmlPage=m.$0,(Global.jQuery(pageMapElements).each(function($1,el)
    {
     var jEl,key;
     jEl=Global.jQuery(el);
     key=el.getAttribute("data-html-page-key");
     htmlPage.map.ContainsKey(key)?jEl.val(htmlPage.map.get_Item(key)):jEl.val(Global.String(el.getAttribute("data-html-page-value")));
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
        return $1("[data-update-field='"+Utils.toSafe($2)+"']");
       };
      }(Global.id))(a.K)).val(a.V[0]());
      return Concurrency.Zero();
     }),Concurrency.Delay(function()
     {
      Global.jQuery(".field-updating").each(function($1,el)
      {
       function eventAction()
       {
        var updateFieldValue;
        updateFieldValue=Global.String(Global.jQuery(el).data("update-field"));
        Global.jQuery((function($2)
        {
         return function($3)
         {
          return $2("[data-update-field='"+Utils.toSafe($3)+"']");
         };
        }(Global.id))(updateFieldValue)).each(function($2,updateElement)
        {
         var elValue;
         elValue=Global.String(Global.jQuery(el).val());
         (map.get_Item(updateFieldValue))[1](elValue);
         !Unchecked.Equals(updateElement,el)||true?Global.jQuery(updateElement).val(elValue):void 0;
        });
        updateEditUserValuesDiv();
        updateAddEmployerDiv();
        updateEmailDiv();
       }
       el.removeEventListener("input",eventAction,true);
       el.addEventListener("input",eventAction,true);
      });
      return Concurrency.Zero();
     }));
    }));
   });
  }
  function setNewDocument()
  {
   Var.Set(varNewDocument,Doc.Element("div",[],[Doc.TextNode("Name: "),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("id","txtNewTemplateName")],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.TextNode("Email-Subject: "),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("id","txtNewTemplateEmailSubject")],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.TextNode("Email-Body: "),Doc.Element("br",[],[]),Doc.Element("textarea",[AttrProxy.Create("id","txtNewTemplateEmailBody"),AttrProxy.Create("style","min-height: 300px; min-width: 100%")],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("value","Add"),AttrModule.Handler("click",function()
   {
    return function()
    {
     var b$1;
     return Concurrency.Start((b$1=null,Concurrency.Delay(function()
     {
      var newDocumentName;
      newDocumentName=Global.String(Global.document.getElementById("txtNewTemplateName").value);
      Global.String(Global.document.getElementById("txtNewTemplateEmailSubject").value);
      Global.String(Global.document.getElementById("txtNewTemplateEmailBody").value);
      return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.addNewDocument:-1276324058",[newDocumentName]),function()
      {
       addSelectDocumentName(newDocumentName);
       return Concurrency.Bind(setNewDocumentEmpty(),function()
       {
        return Concurrency.Return(null);
       });
      });
     })),null);
    };
   })],[])]));
   Var.Set(varPageButtonsDiv,Doc.Element("div",[],[]));
   Var.Set(varSelectHtmlPageTemplate,Doc.Element("div",[],[]));
   Var.Set(varDisplayedDocument,Doc.Element("div",[],[]));
  }
  function setNewDocumentEmpty()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    Var.Set(varNewDocument,Doc.Element("div",[],[]));
    return Concurrency.Bind(setPageButtons(),function()
    {
     return Concurrency.Bind(setSelectHtmlPageTemplate(),function()
     {
      return Concurrency.Return(null);
     });
    });
   });
  }
  function indexChanged_selectDocumentName()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind(setDocument(),function()
    {
     return Concurrency.Bind(setPageButtons(),function()
     {
      return Concurrency.Return(null);
     });
    });
   });
  }
  function indexChanged_selectHtmlPageTemplate()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind(loadPageTemplate(),function()
    {
     return Concurrency.Return(null);
    });
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
       Global.jQuery(Global.document.querySelectorAll("[data-html-page-key]")).each(function(i,el)
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
          $0:HtmlPage.New(htmlPage.name,htmlPage.oTemplateId,htmlPage.pageIndex,htmlPage.map.Add(key,Global.String(Global.jQuery(el).val())))
         }),List.T.Empty]):(htmlPage$1=currentAndAfter.$0.$0,[new DocumentPage({
          $:0,
          $0:HtmlPage.New(htmlPage$1.name,htmlPage$1.oTemplateId,htmlPage$1.pageIndex,htmlPage$1.map.Add(key,Global.String(Global.jQuery(el).val())))
         }),currentAndAfter.$1]),[t[0],p$1[0],p$1[1]])));
         Var.Set(varDocument,(i$1=varDocument.c,Document.New(i$1.id,i$1.name,List.append(p[0],new List.T({
          $:1,
          $0:p[1],
          $1:p[2]
         })),i$1.email)));
        }
        key=el.getAttribute("data-html-page-key");
        el.removeEventListener("input",eventAction,true);
        return el.addEventListener("input",eventAction,true);
       });
       return Concurrency.Zero();
      }));
     });
    }));
   });
  }
  function applyNow()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.applyNowWithHtmlTemplate:2103313263",[varEmployer.c,varDocument.c,varUserValues.c]),function()
    {
     return Concurrency.Zero();
    });
   });
  }
  function loadFileUploadTemplate()
  {
   var c;
   Var.Set(varDisplayedDocument,Doc.Element("form",[AttrProxy.Create("enctype","multipart/form-data"),AttrProxy.Create("method","POST"),AttrProxy.Create("action","")],[Doc.Element("input",[AttrProxy.Create("type","file"),AttrProxy.Create("name","file")],[]),Doc.Element("input",[AttrProxy.Create("type","hidden"),AttrProxy.Create("name","documentId"),AttrProxy.Create("value",(c=Global.document.getElementById("selectDocumentName").selectedIndex+1,Global.String(c)))],[]),Doc.Element("input",[AttrProxy.Create("type","hidden"),AttrProxy.Create("name","pageIndex"),AttrProxy.Create("value",Global.String(varCurrentPageIndex))],[]),Doc.Element("input",[AttrProxy.Create("type","submit")],[])]));
  }
  varDocument=Var.CreateWaiting();
  varUserValues=Var.Create$1(UserValues.New(Gender.Male,"","","","","","","",""));
  varUserEmail=Var.CreateWaiting();
  varEmployer=Var.Create$1(Employer.New("","","","",Gender.Male,"","","","","",""));
  varSelectDocumentName=Var.Create$1(Doc.Element("div",[],[]));
  varSelectHtmlPageTemplate=Var.Create$1(Doc.Element("div",[],[]));
  varNewDocument=Var.Create$1(Doc.Element("div",[],[]));
  varPageButtonsDiv=Var.Create$1(Doc.Element("div",[],[]));
  varPageButtons=Var.Create$1(Doc.Element("div",[],[]));
  varCurrentPageIndex=Var.Create$1(1);
  varPageCount=Var.Create$1(1);
  varDisplayedDocument=Var.Create$1(Doc.Element("div",[],[]));
  varAddPage=Var.Create$1(Doc.Element("div",[],[]));
  varEditUserValuesDiv=Var.Create$1(Doc.Element("div",[],[Doc.TextView(View.Map(function(x)
  {
   return x.firstName;
  },varUserValues.v)),createInput("Degree",varUserValues.c.degree,function(v)
  {
   var i;
   Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,v,i.firstName,i.lastName,i.street,i.postcode,i.city,i.phone,i.mobilePhone)));
  }),createInput("First name",varUserValues.c.firstName,function(v)
  {
   var i;
   Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,v,i.lastName,i.street,i.postcode,i.city,i.phone,i.mobilePhone)));
  }),createInput("Last name",varUserValues.c.lastName,function(v)
  {
   var i;
   Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,v,i.street,i.postcode,i.city,i.phone,i.mobilePhone)));
  }),createInput("Street",varUserValues.c.street,function(v)
  {
   var i;
   Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,v,i.postcode,i.city,i.phone,i.mobilePhone)));
  }),createInput("Postcode",varUserValues.c.postcode,function(v)
  {
   var i;
   Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,i.street,v,i.city,i.phone,i.mobilePhone)));
  }),createInput("City",varUserValues.c.city,function(v)
  {
   var i;
   Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,i.street,i.postcode,v,i.phone,i.mobilePhone)));
  }),createInput("Phone",varUserValues.c.phone,function(v)
  {
   var i;
   Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,i.street,i.postcode,i.city,v,i.mobilePhone)));
  }),createInput("Mobile phone",varUserValues.c.mobilePhone,function(v)
  {
   var i;
   Var.Set(varUserValues,(i=varUserValues.c,UserValues.New(i.gender,i.degree,i.firstName,i.lastName,i.street,i.postcode,i.city,i.phone,v)));
  })]));
  varAddEmployerDiv=Var.Create$1(Doc.Element("div",[],[createInput("Company",varEmployer.c.company,function(v)
  {
   var i;
   Var.Set(varEmployer,(i=varEmployer.c,Employer.New(v,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
  }),createInput("Degree",varEmployer.c.degree,function(v)
  {
   var i;
   Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,v,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
  }),createInput("First name",varEmployer.c.firstName,function(v)
  {
   var i;
   Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,v,i.lastName,i.email,i.phone,i.mobilePhone)));
  }),createInput("Last name",varEmployer.c.lastName,function(v)
  {
   var i;
   Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,v,i.email,i.phone,i.mobilePhone)));
  }),createInput("Street",varEmployer.c.street,function(v)
  {
   var i;
   Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,v,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
  }),createInput("Postcode",varEmployer.c.postcode,function(v)
  {
   var i;
   Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,v,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
  }),createInput("City",varEmployer.c.city,function(v)
  {
   var i;
   Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,v,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,i.mobilePhone)));
  }),createInput("Email",varEmployer.c.email,function(v)
  {
   var i;
   Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,v,i.phone,i.mobilePhone)));
  }),createInput("Phone",varEmployer.c.phone,function(v)
  {
   var i;
   Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,v,i.mobilePhone)));
  }),createInput("Mobile phone",varEmployer.c.mobilePhone,function(v)
  {
   var i;
   Var.Set(varEmployer,(i=varEmployer.c,Employer.New(i.company,i.street,i.postcode,i.city,i.gender,i.degree,i.firstName,i.lastName,i.email,i.phone,v)));
  })]));
  varEmailDiv=Var.Create$1(Doc.Element("div",[],[]));
  Concurrency.Start((b=null,Concurrency.Delay(function()
  {
   return Concurrency.Bind(setSelectDocumentName(),function()
   {
    return Concurrency.Combine(Concurrency.While(function()
    {
     return Unchecked.Equals(Global.document.getElementById("selectDocumentName"),null);
    },Concurrency.Delay(function()
    {
     return Concurrency.Bind(Concurrency.Sleep(10),function()
     {
      return Concurrency.Return(null);
     });
    })),Concurrency.Delay(function()
    {
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getLastEditedDocumentId:-646436276",[]),function(a)
     {
      return Concurrency.Combine(a!=null&&a.$==1?(Global.document.getElementById("selectDocumentName").selectedIndex=a.$0-1,Concurrency.Zero()):(Global.document.getElementById("selectDocumentName").selectedIndex=0,Concurrency.Zero()),Concurrency.Delay(function()
      {
       return Concurrency.Bind(setDocument(),function()
       {
        return Concurrency.Bind(setPageButtons(),function()
        {
         return Concurrency.Bind(setSelectHtmlPageTemplate(),function()
         {
          return Concurrency.Combine(Concurrency.While(function()
          {
           return Unchecked.Equals(Global.document.getElementById("selectHtmlPageTemplate"),null);
          },Concurrency.Delay(function()
          {
           return Concurrency.Bind(Concurrency.Sleep(10),function()
           {
            return Concurrency.Return(null);
           });
          })),Concurrency.Delay(function()
          {
           return Concurrency.Bind(setUserValues(),function()
           {
            return Concurrency.Bind(fillDocumentValues(),function()
            {
             return Concurrency.Return(null);
            });
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
  return Doc.Element("div",[],[Doc.EmbedView(varSelectDocumentName.v),Doc.EmbedView(varNewDocument.v),Doc.EmbedView(varPageButtonsDiv.v),Doc.EmbedView(varAddPage.v),Doc.EmbedView(varSelectHtmlPageTemplate.v),Doc.EmbedView(varDisplayedDocument.v),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.EmbedView(varEmailDiv.v),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.EmbedView(varEditUserValuesDiv.v),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.EmbedView(varAddEmployerDiv.v),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("value","Save as new document"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return Concurrency.Start(saveNewDocument(),null);
   };
  })],[]),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("value","Overwrite document"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return Concurrency.Start(overwriteDocument(),null);
   };
  })],[]),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("value","Apply now"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return Concurrency.Start(applyNow(),null);
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
   return function()
   {
    var loginResult;
    loginResult=(new AjaxRemotingProvider.New()).Sync("JobApplicationSpam:JobApplicationSpam.Server.login:1280212622",[varTxtLoginEmail.c,varTxtLoginPassword.c]);
    return loginResult.$==1?Global.alert(Strings.concat(", ",loginResult.$0)):null;
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
