(function()
{
 "use strict";
 var Global,JobApplicationSpam,Client,Language,Str,AddEmployerAction,SC$1,JobApplicationSpam_GeneratedPrintf,WebSharper,Strings,IntelliFactory,Runtime,Utils,UI,Next,Var,Doc,AttrProxy,AttrModule,Date,Concurrency,Remoting,AjaxRemotingProvider,Numeric,Seq;
 Global=window;
 JobApplicationSpam=Global.JobApplicationSpam=Global.JobApplicationSpam||{};
 Client=JobApplicationSpam.Client=JobApplicationSpam.Client||{};
 Language=Client.Language=Client.Language||{};
 Str=Client.Str=Client.Str||{};
 AddEmployerAction=Client.AddEmployerAction=Client.AddEmployerAction||{};
 SC$1=Global.StartupCode$JobApplicationSpam$Client=Global.StartupCode$JobApplicationSpam$Client||{};
 JobApplicationSpam_GeneratedPrintf=Global.JobApplicationSpam_GeneratedPrintf=Global.JobApplicationSpam_GeneratedPrintf||{};
 WebSharper=Global.WebSharper;
 Strings=WebSharper&&WebSharper.Strings;
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 Utils=WebSharper&&WebSharper.Utils;
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 Var=Next&&Next.Var;
 Doc=Next&&Next.Doc;
 AttrProxy=Next&&Next.AttrProxy;
 AttrModule=Next&&Next.AttrModule;
 Date=Global.Date;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Remoting=WebSharper&&WebSharper.Remoting;
 AjaxRemotingProvider=Remoting&&Remoting.AjaxRemotingProvider;
 Numeric=WebSharper&&WebSharper.Numeric;
 Seq=WebSharper&&WebSharper.Seq;
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
 Client.createTemplate=function()
 {
  var varUserTitle,varUserFirstName,varUserLastName,varUserStreet,varUserPostcode,varUserCity,varBossTitulation,varBossTitle,varBossFirstName,varBossLastName,varCompanyStreet,varCompanyPostcode,varCompanyCity,varSubject,varTextArea,c;
  function resize(el,font,fontSize,fontWeight,defaultWidth)
  {
   var str,span;
   str=Strings.Replace(Global.String(el.val())," ","&nbsp;");
   str===""?el.width(defaultWidth):(span=Global.jQuery("<span />").attr("style",((((Runtime.Curried(function($1,$2,$3,$4)
   {
    return $1("font-family:"+Utils.toSafe($2)+"; font-size: "+Utils.toSafe($3)+"; font-weight: "+Utils.toSafe($4)+"; visibility: hidden");
   },4))(Global.id))(font))(fontSize))(fontWeight)).html(str),span.appendTo("body"),el.width(span.width()),Global.jQuery("body span").last().remove());
   return null;
  }
  function getWidth(s,font,fontSize,fontWeight)
  {
   var str,span,spanWidth;
   str=Strings.Replace(Global.String(s)," ","&nbsp;");
   span=Global.jQuery("<span />").attr("style",function($1)
   {
    return $1("font-family: Arial; font-size: 12pt; font-weight: normal; letter-spacing:0pt; visibility: hidden;");
   }(Global.id)).html(str);
   span.appendTo("body");
   spanWidth=span.width();
   Global.jQuery("body span:last").remove();
   return spanWidth;
  }
  function findLineBreak(str,containerWidth,font,fontSize,fontWeight)
  {
   var myString;
   myString=function(beginIndex,endIndex,n)
   {
    var currentIndex,currentString,width;
    while(true)
     if(n<0)
      return"impossible";
     else
      {
       currentIndex=beginIndex+((endIndex-beginIndex+1)/2>>0);
       currentString=Strings.Substring(str,0,currentIndex);
       width=getWidth(currentString,font,fontSize,fontWeight);
       if(width>containerWidth)
       {
        if(endIndex===currentIndex)
         return Strings.Substring(str,0,currentIndex-1);
        else
         {
          endIndex=currentIndex;
          n=n-1;
         }
       }
       else
        {
         beginIndex=currentIndex;
         n=n-1;
        }
      }
   }(0,str.length,16);
   Global.alert(myString+"\n\n\n\nafter: "+str.substring(myString.length));
   return Var.Set(varUserLastName,myString);
  }
  varUserTitle=Var.Create$1("");
  varUserFirstName=Var.Create$1("");
  varUserLastName=Var.Create$1("");
  varUserStreet=Var.Create$1("");
  varUserPostcode=Var.Create$1("");
  varUserCity=Var.Create$1("Fürth");
  varBossTitulation=Var.Create$1("");
  varBossTitle=Var.Create$1("");
  varBossFirstName=Var.Create$1("");
  varBossLastName=Var.Create$1("");
  varCompanyStreet=Var.Create$1("");
  varCompanyPostcode=Var.Create$1("");
  varCompanyCity=Var.Create$1("Hamburg");
  varSubject=Var.Create$1("Bewerbung als Fachinformatiker für Anwendungsentwicklung");
  varTextArea=Var.Create$1("abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrstuvwxyz1234567890");
  return Doc.Element("div",[],[Doc.Element("h1",[],[Doc.TextNode("Create a template")]),Doc.Element("div",[AttrProxy.Create("class","page")],[Doc.Element("div",[AttrProxy.Create("style","height: 225pt; width: 100%; background-color: lightblue")],[Doc.Input([AttrProxy.Create("class","grow-input"),AttrProxy.Create("autofocus","autofocus"),AttrModule.Handler("input",function(el)
  {
   return function()
   {
    resize(Global.jQuery(el),"Arial","12pt","normal",150);
    return findLineBreak(varTextArea.c,Global.jQuery("#mainText").width(),"Arial","12pt","normal");
   };
  }),AttrProxy.Create("placeholder","Dein Titel")],varUserTitle),Doc.Input([AttrProxy.Create("class","grow-input"),AttrModule.Handler("input",function(el)
  {
   return function()
   {
    return resize(Global.jQuery(el),"Arial","12pt","normal",150);
   };
  }),AttrProxy.Create("placeholder","Dein Vorname")],varUserFirstName),Doc.Input([AttrProxy.Create("class","grow-input"),AttrModule.Handler("input",function(el)
  {
   return function()
   {
    return resize(Global.jQuery(el),"Arial","12pt","normal",150);
   };
  }),AttrProxy.Create("placeholder","Dein Nachname")],varUserLastName),Doc.Element("br",[],[]),Doc.Input([AttrProxy.Create("class","grow-input"),AttrProxy.Create("style","width:150px"),AttrModule.Handler("input",function(el)
  {
   return function()
   {
    return resize(Global.jQuery(el),"Arial","12pt","normal",150);
   };
  }),AttrProxy.Create("placeholder","Deine Straße")],varUserStreet),Doc.Element("br",[],[]),Doc.Input([AttrProxy.Create("class","grow-input"),AttrModule.Handler("input",function(el)
  {
   return function()
   {
    return resize(Global.jQuery(el),"Arial","12pt","normal",150);
   };
  }),AttrProxy.Create("placeholder","Deine Postleitzahl")],varUserPostcode),Doc.Input([AttrProxy.Create("class","grow-input"),AttrModule.Handler("input",function(el)
  {
   return function()
   {
    return resize(Global.jQuery(el),"Arial","12pt","normal",150);
   };
  }),AttrProxy.Create("placeholder","Deine Stadt")],varUserCity),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Input([AttrProxy.Create("class","grow-input"),AttrProxy.Create("style","width:150px"),AttrModule.Handler("input",function(el)
  {
   return function()
   {
    return resize(Global.jQuery(el),"Arial","12pt","normal",150);
   };
  }),AttrProxy.Create("placeholder","Chef-Anrede")],varBossTitulation),Doc.Element("br",[],[]),Doc.Input([AttrProxy.Create("class","grow-input"),AttrModule.Handler("input",function(el)
  {
   return function()
   {
    return resize(Global.jQuery(el),"Arial","12pt","normal",150);
   };
  }),AttrProxy.Create("placeholder","Chef-Titel")],varBossTitle),Doc.Input([AttrProxy.Create("class","grow-input"),AttrModule.Handler("input",function(el)
  {
   return function()
   {
    return resize(Global.jQuery(el),"Arial","12pt","normal",150);
   };
  }),AttrProxy.Create("placeholder","Chef-Vorname")],varBossFirstName),Doc.Input([AttrProxy.Create("class","grow-input"),AttrModule.Handler("input",function(el)
  {
   return function()
   {
    return resize(Global.jQuery(el),"Arial","12pt","normal",150);
   };
  }),AttrProxy.Create("placeholder","Chef-Nachname")],varBossLastName),Doc.Element("br",[],[]),Doc.Input([AttrProxy.Create("class","grow-input"),AttrModule.Handler("input",function(el)
  {
   return function()
   {
    return resize(Global.jQuery(el),"Arial","12pt","normal",150);
   };
  }),AttrProxy.Create("placeholder","Firma-Strasse")],varCompanyStreet),Doc.Element("br",[],[]),Doc.Input([AttrProxy.Create("class","grow-input"),AttrModule.Handler("input",function(el)
  {
   return function()
   {
    return resize(Global.jQuery(el),"Arial","12pt","normal",150);
   };
  }),AttrProxy.Create("placeholder","Firma-Postleitzahl")],varCompanyPostcode),Doc.Input([AttrProxy.Create("class","grow-input"),AttrModule.Handler("input",function(el)
  {
   return function()
   {
    return resize(Global.jQuery(el),"Arial","12pt","normal",150);
   };
  }),AttrProxy.Create("placeholder","Firma-Stadt")],varCompanyCity),Doc.Element("br",[],[]),Doc.Element("span",[AttrProxy.Create("style","float:right")],[Doc.TextView(varUserCity.v),Doc.TextNode(", "+(c=Date.now(),(new Date(c)).toLocaleDateString()))]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Input([AttrProxy.Create("class","grow-input"),AttrProxy.Create("style","font-weight: bold;"),AttrModule.Handler("input",function(el)
  {
   return function()
   {
    return resize(Global.jQuery(el),"Arial","12pt","bold",150);
   };
  }),AttrProxy.Create("placeholder","Betreff")],varSubject),Doc.Element("br",[],[]),Doc.Element("br",[],[])]),Doc.Element("input",[AttrProxy.Create("style","width: 16.55cm; border: none; letter-spacing:0pt; margin: 0px; padding: 0px; min-width:100%; font-family: Arial; font-size: 12pt; font-weight: normal; display: block")],[]),Doc.Element("div",[AttrProxy.Create("style","width:100%; min-height: 322.4645709pt; background-color:red;")],[Doc.InputArea([AttrProxy.Create("id","mainText"),AttrProxy.Create("style","wrap: hard; border: none; outline: none; letter-spacing:0pt; margin: 0px; padding: 0px; background-color: lighblue; overflow: hidden; min-height: 322.4645709pt; min-width:100%; font-family: Arial; font-size: 12pt; font-weight: normal; display: block")],varTextArea)]),Doc.Element("div",[AttrProxy.Create("style","height:96pt; width: 100%;")],[Doc.Element("br",[],[]),Doc.TextNode("Mit freundlichen Grüßen"),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.TextNode("$meinTitel $meinVorname $meinNachname")])])]);
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
    return Concurrency.StartImmediate((b=null,Concurrency.Delay(function()
    {
     (new AjaxRemotingProvider.New()).Sync("JobApplicationSpam:JobApplicationSpam.Server.register:-516061576",[varTxtRegisterEmail.c,varTxtRegisterPassword1.c,varTxtRegisterPassword2.c]);
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
    var m;
    m=(new AjaxRemotingProvider.New()).Sync("JobApplicationSpam:JobApplicationSpam.Server.login:1280212622",[varTxtLoginEmail.c,varTxtLoginPassword.c]);
    return m.$==1?Global.alert(Strings.concat(", ",m.$0)):null;
   };
  })],[Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","txtLoginEmail")],[Doc.TextNode("Email")]),Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("id","txtLoginEmail"),AttrProxy.Create("placeholder","Email")],varTxtLoginEmail)]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","txtLoginPassword")],[Doc.TextNode("Password")]),Doc.PasswordBox([AttrProxy.Create("class","form-control"),AttrProxy.Create("id","txtLoginPassword"),AttrProxy.Create("placeholder","Password")],varTxtLoginPassword)]),Doc.Element("input",[AttrProxy.Create("type","submit"),AttrProxy.Create("value","Login")],[])]);
 };
 Client.uploadTemplate=function()
 {
  var varAddFileDivsDiv;
  function createFileDiv(n)
  {
   return Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","file"+Global.String(n)),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrAddFile))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Element("input",[AttrProxy.Create("class","form-control"),AttrProxy.Create("type","file"),AttrProxy.Create("id","file"+Global.String(n)),AttrProxy.Create("name","file[]"),AttrModule.Handler("change",function()
   {
    return function()
    {
     return addFileDiv(varAddFileDivsDiv,null);
    };
   }),AttrProxy.Create("accept",".odt .pdf .txt")],[])])]);
  }
  function addFileDiv()
  {
   return varAddFileDivsDiv.c.Append(createFileDiv(Numeric.ParseInt32(varAddFileDivsDiv.c.elt.lastChild.lastChild.lastChild.attributes.getNamedItem("id").value.substring(4))+1));
  }
  varAddFileDivsDiv=Var.Create$1(Doc.Element("div",[],[createFileDiv(0)]));
  return Doc.Element("div",[],[Doc.Element("h1",[],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrUploadTemplate))]),Doc.Element("form",[AttrProxy.Create("class","form-horizontal"),AttrProxy.Create("enctype","multipart/form-data"),AttrProxy.Create("method","POST"),AttrProxy.Create("action","")],[Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","templateName"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrTemplateName))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Element("input",[AttrProxy.Create("class","form-control"),AttrProxy.Create("type","text"),AttrProxy.Create("id","templateName"),AttrProxy.Create("name","templateName"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrTemplateName))],[])])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","emailSubject"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrUserAppliesAs))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Element("input",[AttrProxy.Create("class","form-control"),AttrProxy.Create("type","text"),AttrProxy.Create("id","userAppliesAs"),AttrProxy.Create("name","userAppliesAs"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrUserAppliesAs))],[])])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","emailSubject"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrEmailSubject))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Element("input",[AttrProxy.Create("class","form-control"),AttrProxy.Create("type","text"),AttrProxy.Create("id","emailSubject"),AttrProxy.Create("name","emailSubject"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrEmailSubject))],[])])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","emailBody"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrEmailBody))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Element("textarea",[AttrProxy.Create("class","form-control"),AttrProxy.Create("style","min-height:260px"),AttrProxy.Create("type","text"),AttrProxy.Create("id","emailBody"),AttrProxy.Create("name","emailBody"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrEmailBody))],[])])]),varAddFileDivsDiv.c,Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("class","control-label")],[Doc.TextNode("")]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Element("input",[AttrProxy.Create("type","submit"),AttrProxy.Create("value",Client.translate(Client.currentLanguage(),Str.StrUploadTemplate))],[])])])]),Doc.TextNode((function($1)
  {
   return function($2)
   {
    return $1("logged in as: "+JobApplicationSpam_GeneratedPrintf.p($2));
   };
  }(Global.id))((new AjaxRemotingProvider.New()).Sync("JobApplicationSpam:JobApplicationSpam.Server.getCurrentUserId:182929778",[])))]);
 };
 Client.applyNow=function()
 {
  var varEmployerId,varTemplateId,message;
  function submitIt()
  {
   var employerId,templateId,b;
   try
   {
    employerId=Numeric.ParseInt32(varEmployerId.c);
    templateId=Numeric.ParseInt32(varTemplateId.c);
    Concurrency.StartImmediate((b=null,Concurrency.Delay(function()
    {
     return Concurrency.TryWith(Concurrency.Delay(function()
     {
      return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.applyNow:2084076389",[employerId,templateId]),function(a)
      {
       return Concurrency.Combine(a.$==1?(Var.Set(message,"Bewerbung konnte nicht bearbeitet werden."),Concurrency.Zero()):(Var.Set(message,"Bewerbung wurde versandt."),Concurrency.Zero()),Concurrency.Delay(function()
       {
        return Concurrency.Return(null);
       }));
      });
     }),function(a)
     {
      Var.Set(message,"Bewerbung konnte nicht bearbeitet werden."+a.message);
      return Concurrency.Zero();
     });
    })),null);
   }
   catch(e)
   {
    Var.Set(message,"Es trat ein Fehler auf: "+e.message);
   }
  }
  varEmployerId=Var.Create$1("");
  varTemplateId=Var.Create$1("");
  message=Var.Create$1("abc");
  return Doc.Element("div",[],[Doc.Element("form",[AttrProxy.Create("action",""),AttrProxy.Create("method","post"),AttrModule.Handler("submit",function()
  {
   return function(ev)
   {
    submitIt();
    ev.preventDefault();
    return ev.stopImmediatePropagation();
   };
  })],[Doc.Element("label",[],[Doc.TextNode("employerId")]),Doc.Input([AttrProxy.Create("type","text"),AttrProxy.Create("value",varEmployerId.c)],varEmployerId),Doc.Element("br",[],[]),Doc.Element("label",[],[Doc.TextNode("templateId")]),Doc.Input([AttrProxy.Create("type","text"),AttrProxy.Create("value",varTemplateId.c)],varTemplateId),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","submit")],[])]),Doc.TextView(message.v)]);
 };
 Client.addEmployer=function()
 {
  var varMessage,varCanSubmit,varCompany,varStreet,varPostcode,varCity,varGender,varDegree,varFirstName,varLastName,varEmail,varPhone,varMobilePhone,templateList,varTemplate,o,varAddEmployerAction;
  function createEmployer(company,street,postcode,city,gender,degree,firstName,lastName,email,phone,mobilePhone)
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
  }
  function subm(employer,templateName)
  {
   var b;
   function addEmployer123()
   {
    var b$1;
    b$1=null;
    return Concurrency.Delay(function()
    {
     Var.Set(varMessage,"Adding employer...");
     return Concurrency.Bind(Concurrency.Sleep(2000),function()
     {
      return(new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.addEmployer:-762170454",[employer]);
     });
    });
   }
   function applyNow123(employerId,templateName$1)
   {
    var b$1;
    b$1=null;
    return Concurrency.Delay(function()
    {
     Var.Set(varMessage,"Sending job application...");
     return Concurrency.Bind(Concurrency.Sleep(2000),function()
     {
      return(new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.applyNowByTemplateName:1909292144",[employerId,templateName$1]);
     });
    });
   }
   Concurrency.StartImmediate((b=null,Concurrency.Delay(function()
   {
    return Concurrency.Bind(addEmployer123(),function(a)
    {
     return a.$==1?(Var.Set(varMessage,"Unfortunately, adding employer failed."),Concurrency.Return(null)):Concurrency.Bind(applyNow123(a.$0,templateName),function(a$1)
     {
      return a$1.$==1?(Var.Set(varMessage,"Unfortunately, adding employer failed."),Concurrency.Return(null)):(Var.Set(varMessage,"Job application has been sent"),Concurrency.Return(null));
     });
    });
   })),null);
   return Var.Set(varCanSubmit,true);
  }
  varMessage=Var.Create$1("");
  varCanSubmit=Var.Create$1(true);
  varCompany=Var.Create$1("");
  varStreet=Var.Create$1("");
  varPostcode=Var.Create$1("");
  varCity=Var.Create$1("");
  varGender=Var.Create$1({
   $:0
  });
  varDegree=Var.Create$1("");
  varFirstName=Var.Create$1("");
  varLastName=Var.Create$1("");
  varEmail=Var.Create$1("");
  varPhone=Var.Create$1("");
  varMobilePhone=Var.Create$1("");
  templateList=(new AjaxRemotingProvider.New()).Sync("JobApplicationSpam:JobApplicationSpam.Server.getTemplateNames:-1471467441",[]);
  varTemplate=Var.Create$1((o=Seq.tryItem(0,templateList),o==null?"":o.$0));
  varAddEmployerAction=Var.Create$1(AddEmployerAction.JustAddEmployer);
  return Doc.Element("form",[AttrProxy.Create("class","form-horizontal")],[Doc.Element("h1",[],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrAddEmployer))]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","company"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrCompanyName))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrCompanyName)),AttrProxy.Create("id","company"),AttrProxy.Create("value",varStreet.c)],varCompany)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","company"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrCompanyStreet))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrCompanyStreet)),AttrProxy.Create("id","street"),AttrProxy.Create("value",varStreet.c)],varStreet)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","company"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrCompanyPostcode))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrCompanyPostcode)),AttrProxy.Create("id","postcode"),AttrProxy.Create("value",varPostcode.c)],varPostcode)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","city"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrCompanyCity))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrCompanyCity)),AttrProxy.Create("id","city"),AttrProxy.Create("value",varCity.c)],varCity)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrBossGender))]),Doc.Element("br",[],[]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Radio([AttrProxy.Create("id","male"),AttrProxy.Create("radiogroup","gender")],{
   $:0
  },varGender),Doc.Element("label",[AttrProxy.Create("for","male")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrMale))]),Doc.Element("br",[],[]),Doc.Radio([AttrProxy.Create("id","female"),AttrProxy.Create("radiogroup","gender")],{
   $:1
  },varGender),Doc.Element("label",[AttrProxy.Create("for","female")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrFemale))])])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","degree"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrBossDegree))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossDegree)),AttrProxy.Create("id","degree"),AttrProxy.Create("value",varDegree.c)],varDegree)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","firstName"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrBossFirstName))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossFirstName)),AttrProxy.Create("id","firstName"),AttrProxy.Create("value",varFirstName.c)],varFirstName)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","lastName"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrBossLastName))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossLastName)),AttrProxy.Create("value",varLastName.c)],varLastName)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","email"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrBossEmail))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossEmail)),AttrProxy.Create("id","email"),AttrProxy.Create("value",varEmail.c)],varEmail)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","phone"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrBossPhone))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossPhone)),AttrProxy.Create("id","phone"),AttrProxy.Create("value",varPhone.c)],varPhone)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","mobilePhone"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrBossMobilePhone))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossMobilePhone)),AttrProxy.Create("id","mobilePhone"),AttrProxy.Create("value",varMobilePhone.c)],varMobilePhone)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrBossMobilePhone))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Radio([AttrProxy.Create("class","form-control"),AttrProxy.Create("radiogroup","addEmployerAction"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossMobilePhone)),AttrProxy.Create("id","mobilePhone"),AttrProxy.Create("value","hallo")],AddEmployerAction.ApplyImmediately,varAddEmployerAction),Doc.Radio([AttrProxy.Create("class","form-control"),AttrProxy.Create("radiogroup","addEmployerAction"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossMobilePhone)),AttrProxy.Create("id","mobilePhone"),AttrProxy.Create("value",varMobilePhone.c)],AddEmployerAction.SendJobApplicationToUserOnly,varAddEmployerAction),Doc.Radio([AttrProxy.Create("class","form-control"),AttrProxy.Create("radiogroup","addEmployerAction"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossMobilePhone)),AttrProxy.Create("id","mobilePhone"),AttrProxy.Create("value",varMobilePhone.c)],AddEmployerAction.JustAddEmployer,varAddEmployerAction)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrBossMobilePhone))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Select([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossMobilePhone)),AttrProxy.Create("id","mobilePhone"),AttrProxy.Create("value",varMobilePhone.c)],Global.id,templateList,varTemplate)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Button(Client.translate(Client.currentLanguage(),Str.StrAddEmployer),[AttrProxy.Create("type","submit"),AttrProxy.Create("class","form-control")],function()
  {
   if(varCanSubmit.c)
    {
     Var.Set(varCanSubmit,false);
     subm(createEmployer(varCompany.c,varStreet.c,varPostcode.c,varCity.c,varGender.c,varDegree.c,varFirstName.c,varLastName.c,varEmail.c,varPhone.c,varMobilePhone.c),varTemplate.c);
    }
  })]),Doc.TextView(varMessage.v)]);
 };
 Client.editUserValues=function()
 {
  var varMessage,varGender,varDegree,varFirstName,varLastName,varStreet,varPostcode,varCity,varPhone,varMobilePhone,m,userValues;
  function createUserValues(gender,degree,firstName,lastName,street,postcode,city,phone,mobilePhone)
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
  }
  function subm(userValues$1)
  {
   var b;
   Concurrency.StartImmediate((b=null,Concurrency.Delay(function()
   {
    Var.Set(varMessage,"Setting user values...");
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.setUserValues:1230958299",[userValues$1]),function(a)
    {
     var m$1;
     m$1=a.$==1?(function($1)
     {
      return function($2)
      {
       return $1(Utils.printList(Utils.prettyPrint,$2));
      };
     }(Global.id))(a.$0):a.$0;
     return Concurrency.Bind(Concurrency.Sleep(1000),function()
     {
      Var.Set(varMessage,m$1);
      return Concurrency.Return(null);
     });
    });
   })),null);
  }
  varMessage=Var.Create$1("nothing");
  varGender=Var.Create$1({
   $:0
  });
  varDegree=Var.Create$1("");
  varFirstName=Var.Create$1("");
  varLastName=Var.Create$1("");
  varStreet=Var.Create$1("");
  varPostcode=Var.Create$1("");
  varCity=Var.Create$1("");
  varPhone=Var.Create$1("");
  varMobilePhone=Var.Create$1("");
  m=(new AjaxRemotingProvider.New()).Sync("JobApplicationSpam:JobApplicationSpam.Server.getCurrentUserValues:682112376",[]);
  m==null?void 0:(userValues=m.$0,Var.Set(varGender,userValues.gender),Var.Set(varDegree,userValues.degree),Var.Set(varFirstName,userValues.firstName),Var.Set(varLastName,userValues.lastName),Var.Set(varStreet,userValues.street),Var.Set(varPostcode,userValues.postcode),Var.Set(varCity,userValues.city),Var.Set(varPhone,userValues.phone),Var.Set(varMobilePhone,userValues.mobilePhone));
  return Doc.Element("div",[],[Doc.Element("h1",[],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrEditUserValues))]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrUserValuesGender))]),Doc.Element("br",[],[]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Radio([AttrProxy.Create("id","male"),AttrProxy.Create("radiogroup","gender")],{
   $:0
  },varGender),Doc.Element("label",[AttrProxy.Create("for","male")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrMale))]),Doc.Element("br",[],[]),Doc.Radio([AttrProxy.Create("id","female"),AttrProxy.Create("radiogroup","gender")],{
   $:1
  },varGender),Doc.Element("label",[AttrProxy.Create("for","female")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrFemale))])])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","degree"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrUserValuesDegree))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrUserValuesDegree)),AttrProxy.Create("id","degree"),AttrProxy.Create("value",varDegree.c)],varDegree)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","firstName"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrUserValuesFirstName))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrUserValuesFirstName)),AttrProxy.Create("id","firstName"),AttrProxy.Create("value",varFirstName.c)],varFirstName)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","lastName"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrUserValuesLastName))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrUserValuesLastName)),AttrProxy.Create("id","lastName"),AttrProxy.Create("value",varLastName.c)],varLastName)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","street"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrUserValuesStreet))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrUserValuesStreet)),AttrProxy.Create("id","street"),AttrProxy.Create("value",varStreet.c)],varStreet)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","postcode"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrUserValuesPostcode))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrUserValuesPostcode)),AttrProxy.Create("id","postcode"),AttrProxy.Create("value",varPostcode.c)],varPostcode)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","city"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrUserValuesCity))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrUserValuesCity)),AttrProxy.Create("id","city"),AttrProxy.Create("value",varCity.c)],varCity)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","phone"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrUserValuesPhone))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrUserValuesPhone)),AttrProxy.Create("id","phone"),AttrProxy.Create("value",varPhone.c)],varPhone)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","mobilePhone"),AttrProxy.Create("class","control-label")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrUserValuesMobilePhone))]),Doc.Element("div",[AttrProxy.Create("class","")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrUserValuesMobilePhone)),AttrProxy.Create("id","mobilePhone"),AttrProxy.Create("value",varMobilePhone.c)],varMobilePhone)])]),Doc.Button(Client.translate(Client.currentLanguage(),Str.StrSubmitEditUserValues),[AttrProxy.Create("type","submit")],function()
  {
   subm(createUserValues(varGender.c,varDegree.c,varFirstName.c,varLastName.c,varStreet.c,varPostcode.c,varCity.c,varPhone.c,varMobilePhone.c));
  }),Doc.TextView(varMessage.v)]);
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
 JobApplicationSpam_GeneratedPrintf.p=function($1)
 {
  return $1==null?"null":"Some "+Utils.prettyPrint($1.$0);
 };
}());
