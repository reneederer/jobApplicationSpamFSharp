(function()
{
 "use strict";
 var Global,JobApplicationSpam,Types,JobApplicationPageAction,DocumentItem,Client,Language,Str,AddEmployerAction,SC$1,IntelliFactory,Runtime,WebSharper,Concurrency,Remoting,AjaxRemotingProvider,UI,Next,Var,Doc,AttrProxy,AttrModule,List,Seq,Unchecked,Collections,Map,Arrays,Date,Utils,Operators,Strings;
 Global=window;
 JobApplicationSpam=Global.JobApplicationSpam=Global.JobApplicationSpam||{};
 Types=JobApplicationSpam.Types=JobApplicationSpam.Types||{};
 JobApplicationPageAction=Types.JobApplicationPageAction=Types.JobApplicationPageAction||{};
 DocumentItem=Types.DocumentItem=Types.DocumentItem||{};
 Client=JobApplicationSpam.Client=JobApplicationSpam.Client||{};
 Language=Client.Language=Client.Language||{};
 Str=Client.Str=Client.Str||{};
 AddEmployerAction=Client.AddEmployerAction=Client.AddEmployerAction||{};
 SC$1=Global.StartupCode$JobApplicationSpam$Client=Global.StartupCode$JobApplicationSpam$Client||{};
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 WebSharper=Global.WebSharper;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Remoting=WebSharper&&WebSharper.Remoting;
 AjaxRemotingProvider=Remoting&&Remoting.AjaxRemotingProvider;
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 Var=Next&&Next.Var;
 Doc=Next&&Next.Doc;
 AttrProxy=Next&&Next.AttrProxy;
 AttrModule=Next&&Next.AttrModule;
 List=WebSharper&&WebSharper.List;
 Seq=WebSharper&&WebSharper.Seq;
 Unchecked=WebSharper&&WebSharper.Unchecked;
 Collections=WebSharper&&WebSharper.Collections;
 Map=Collections&&Collections.Map;
 Arrays=WebSharper&&WebSharper.Arrays;
 Date=Global.Date;
 Utils=WebSharper&&WebSharper.Utils;
 Operators=WebSharper&&WebSharper.Operators;
 Strings=WebSharper&&WebSharper.Strings;
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
 DocumentItem=Types.DocumentItem=Runtime.Class({
  PageIndex:function()
  {
   return this.$==1?this.$0.pageIndex:this.$0.pageIndex;
  },
  Name:function()
  {
   return this.$==1?this.$0.name:this.$0.name;
  }
 },null,DocumentItem);
 Language.German={
  $:1
 };
 Language.English={
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
  var varDocument,varSelectDocumentName,varSelectPageTemplate,varPageButtons,varCurrentPageIndex,varDisplayedDocument,b;
  function setSelectDocumentName()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getDocumentNames:1994133801",[]),function(a)
    {
     Var.Set(varSelectDocumentName,Doc.Element("select",[AttrProxy.Create("id","selectDocumentName"),AttrModule.Handler("change",function()
     {
      return function()
      {
       return indexChanged_selectDocumentName();
      };
     })],List.ofSeq(Seq.delay(function()
     {
      return Seq.map(function(documentName)
      {
       return Doc.Element("option",[],[Doc.TextNode(documentName)]);
      },a);
     }))));
     return Concurrency.Zero();
    });
   });
  }
  function setSelectPageTemplate()
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getPageTemplates:-1239889210",[]),function(a)
    {
     Var.Set(varSelectPageTemplate,Doc.Element("select",[AttrProxy.Create("id","selectPageTemplate"),AttrModule.Handler("change",function()
     {
      return function()
      {
       return indexChanged_selectPageTemplate();
      };
     })],List.ofSeq(Seq.delay(function()
     {
      return Seq.map(function(pageTemplate)
      {
       return Doc.Element("option",[],[Doc.TextNode(pageTemplate.name)]);
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
    Var.Set(varPageButtons,Doc.Element("ul",[],List.ofSeq(Seq.delay(function()
    {
     return Seq.collect(function(documentItem)
     {
      var file,page;
      return documentItem.$==1?(file=documentItem.$0,[Doc.Element("li",[],[Doc.Element("button",[AttrModule.Handler("click",function()
      {
       return function()
       {
        Var.Set(varCurrentPageIndex,file.pageIndex);
        return loadFileUploadTemplate();
       };
      })],[Doc.TextNode(documentItem.Name())])])]):(page=documentItem.$0,[Doc.Element("li",[],[Doc.Element("button",[AttrModule.Handler("click",function()
      {
       return function()
       {
        var b$2;
        return Concurrency.Start((b$2=null,Concurrency.Delay(function()
        {
         Global.document.getElementById("selectPageTemplate").selectedIndex=page.templateId-1;
         Var.Set(varCurrentPageIndex,page.pageIndex);
         return Concurrency.Bind(loadPageTemplate(),function()
         {
          return Concurrency.Bind(fillDocumentValues(),function()
          {
           return Concurrency.Return(null);
          });
         });
        })),null);
       };
      })],[Doc.TextNode(documentItem.Name())])])]);
     },List.sortBy(function(x)
     {
      return x.PageIndex();
     },varDocument.c.items));
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
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getDocumentOffset:-1572854490",[documentIndex]),function(a)
    {
     Var.Set(varDocument,a);
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
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getCurrentUserValues:-337599557",[]),function(a)
    {
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getCurrentUserEmail:-834772631",[]),function(a$1)
     {
      var c,c$1,c$2;
      return Concurrency.Combine(Concurrency.For(Map.OfArray(Arrays.ofSeq(List.ofArray([["userDegree",a.degree],["userFirstName",a.firstName],["userLastName",a.lastName],["userStreet",a.street],["userPostcode",a.postcode],["userCity",a.city],["userEmail",a$1],["userPhone",a.phone],["userMobilePhone",a.mobilePhone],["today",((((Runtime.Curried(function($1,$2,$3,$4)
      {
       return $1(Global.String($2)+"-"+Global.String($3)+"-"+Global.String($4));
      },4))(Global.id))((c=Date.now(),(new Date(c)).getFullYear())))((c$1=Date.now(),(new Date(c$1)).getMonth()+1)))((c$2=Date.now(),(new Date(c$2)).getDate()))]]))),function(a$2)
      {
       Global.jQuery((function($1)
       {
        return function($2)
        {
         return $1("[data-variable-value='"+Utils.toSafe($2)+"']");
        };
       }(Global.id))(a$2.K)).val(a$2.V);
       return Concurrency.Zero();
      }),Concurrency.Delay(function()
      {
       var documentIndex;
       documentIndex=Global.document.getElementById("selectDocumentName").selectedIndex;
       return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getDocumentMapOffset:742047247",[varCurrentPageIndex.c,documentIndex]),function(a$2)
       {
        return Concurrency.Combine(Concurrency.For(Operators.range(0,1000),function()
        {
         return Unchecked.Equals(Global.document.getElementById("insertDiv"),null)?Concurrency.Bind(Concurrency.Sleep(10),function()
         {
          return Concurrency.Return(null);
         }):Concurrency.Zero();
        }),Concurrency.Delay(function()
        {
         return Concurrency.Combine(a$2.ContainsKey("mainText")?(Global.jQuery("#mainText").val(Strings.Replace(a$2.get_Item("mainText"),"\\n","\n")),Concurrency.Zero()):Concurrency.Zero(),Concurrency.Delay(function()
         {
          Global.jQuery(".field-updating").each(function($1,el)
          {
           el.addEventListener("input",function()
           {
            var updateField;
            updateField=Global.String(Global.jQuery(el).data("update-field"));
            updateField==="userDegree"?Global.alert("userDegree"):updateField==="userFirstName"?Global.alert("FirstName!"):updateField==="userLastName"?Global.alert("LastName!"):void 0;
            Global.jQuery((function($2)
            {
             return function($3)
             {
              return $2("[data-update-field='"+Utils.toSafe($3)+"']");
             };
            }(Global.id))(Global.String(Global.jQuery(el).data("update-field")))).each(function($2,updateElement)
            {
             if(!Unchecked.Equals(updateElement,el))
              Global.jQuery(updateElement).val(Global.String(Global.jQuery(el).val()));
            });
            return null;
           },true);
          });
          return Concurrency.Zero();
         }));
        }));
       });
      }));
     });
    });
   });
  }
  function indexChanged_selectDocumentName()
  {
   var b$1;
   Concurrency.Start((b$1=null,Concurrency.Delay(function()
   {
    return Concurrency.Bind(setDocument(),function()
    {
     return Concurrency.Bind(setPageButtons(),function()
     {
      return Concurrency.Return(null);
     });
    });
   })),null);
  }
  function indexChanged_selectPageTemplate()
  {
   var b$1;
   Concurrency.Start((b$1=null,Concurrency.Delay(function()
   {
    return Concurrency.Bind(loadPageTemplate(),function()
    {
     return Concurrency.Bind(fillDocumentValues(),function()
     {
      return Concurrency.Return(null);
     });
    });
   })),null);
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
     pageTemplateIndex=Global.document.getElementById("selectPageTemplate").selectedIndex;
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getPageTemplate:-1212795141",[pageTemplateIndex+1]),function(a)
     {
      Var.Set(varDisplayedDocument,Doc.Verbatim(a));
      return Concurrency.While(function()
      {
       return Unchecked.Equals(Global.document.getElementById("insertDiv"),null);
      },Concurrency.Delay(function()
      {
       return Concurrency.Bind(Concurrency.Sleep(10),function()
       {
        return Concurrency.Return(null);
       });
      }));
     });
    }));
   });
  }
  function loadFileUploadTemplate()
  {
   var c,c$1;
   Var.Set(varDisplayedDocument,Doc.Element("form",[AttrProxy.Create("enctype","multipart/form-data"),AttrProxy.Create("method","POST"),AttrProxy.Create("action","")],[Doc.Element("input",[AttrProxy.Create("type","file"),AttrProxy.Create("name","file")],[]),Doc.Element("input",[AttrProxy.Create("type","hidden"),AttrProxy.Create("name","documentId"),AttrProxy.Create("value",(c=Global.document.getElementById("selectDocumentName").selectedIndex+1,Global.String(c)))],[]),Doc.Element("input",[AttrProxy.Create("type","hidden"),AttrProxy.Create("name","pageIndex"),AttrProxy.Create("value",(c$1=varCurrentPageIndex.c,Global.String(c$1)))],[]),Doc.Element("input",[AttrProxy.Create("type","submit")],[])]));
  }
  varDocument=Var.CreateWaiting();
  varSelectDocumentName=Var.Create$1(Doc.Element("div",[],[]));
  varSelectPageTemplate=Var.Create$1(Doc.Element("div",[],[]));
  varPageButtons=Var.Create$1(Doc.Element("div",[],[]));
  varCurrentPageIndex=Var.Create$1(1);
  varDisplayedDocument=Var.Create$1(Doc.Element("div",[],[]));
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
         return Concurrency.Bind(setSelectPageTemplate(),function()
         {
          return Concurrency.Combine(Concurrency.For(Operators.range(0,1000),function()
          {
           return Unchecked.Equals(Global.document.getElementById("selectPageTemplate"),null)?Concurrency.Bind(Concurrency.Sleep(10),function()
           {
            return Concurrency.Return(null);
           }):Concurrency.Zero();
          }),Concurrency.Delay(function()
          {
           return Concurrency.Bind(fillDocumentValues(),function()
           {
            return Concurrency.Return(null);
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
  return Doc.Element("div",[],[Doc.TextNode("Your application documents: "),Doc.EmbedView(varSelectDocumentName.v),Doc.EmbedView(varPageButtons.v),Doc.EmbedView(varSelectPageTemplate.v),Doc.EmbedView(varDisplayedDocument.v)]);
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
  SC$1.currentLanguage=Language.German;
 };
}());
