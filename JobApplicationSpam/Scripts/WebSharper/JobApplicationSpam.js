(function()
{
 "use strict";
 var Global,JobApplicationSpam,Client,Language,Str,AddEmployerAction,SC$1,JobApplicationSpam_GeneratedPrintf,WebSharper,UI,Next,Doc,Var,AttrModule,Concurrency,Remoting,AjaxRemotingProvider,AttrProxy,Numeric,Utils;
 Global=window;
 JobApplicationSpam=Global.JobApplicationSpam=Global.JobApplicationSpam||{};
 Client=JobApplicationSpam.Client=JobApplicationSpam.Client||{};
 Language=Client.Language=Client.Language||{};
 Str=Client.Str=Client.Str||{};
 AddEmployerAction=Client.AddEmployerAction=Client.AddEmployerAction||{};
 SC$1=Global.StartupCode$JobApplicationSpam$Client=Global.StartupCode$JobApplicationSpam$Client||{};
 JobApplicationSpam_GeneratedPrintf=Global.JobApplicationSpam_GeneratedPrintf=Global.JobApplicationSpam_GeneratedPrintf||{};
 WebSharper=Global.WebSharper;
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 Doc=Next&&Next.Doc;
 Var=Next&&Next.Var;
 AttrModule=Next&&Next.AttrModule;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Remoting=WebSharper&&WebSharper.Remoting;
 AjaxRemotingProvider=Remoting&&Remoting.AjaxRemotingProvider;
 AttrProxy=Next&&Next.AttrProxy;
 Numeric=WebSharper&&WebSharper.Numeric;
 Utils=WebSharper&&WebSharper.Utils;
 Language.German={
  $:1
 };
 Language.English={
  $:0
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
    var b;
    return Concurrency.StartImmediate((b=null,Concurrency.Delay(function()
    {
     (new AjaxRemotingProvider.New()).Sync("JobApplicationSpam:JobApplicationSpam.Server.login:1280212622",[varTxtLoginEmail.c,varTxtLoginPassword.c]);
     return Concurrency.Return(null);
    })),null);
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
  var varMessage,varCanSubmit,varCompany,varStreet,varPostcode,varCity,varGender,varDegree,varFirstName,varLastName,varEmail,varPhone,varMobilePhone,varTemplate,templateList,varAddEmployerAction;
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
  function subm(employer)
  {
   var b;
   Concurrency.StartImmediate((b=null,Concurrency.Delay(function()
   {
    Var.Set(varMessage,"Adding employer...");
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.addEmployer:-759981594",[employer]),function(a)
    {
     var m;
     m=a.$==1?(function($1)
     {
      return function($2)
      {
       return $1(Utils.printList(Utils.prettyPrint,$2));
      };
     }(Global.id))(a.$0):a.$0;
     return Concurrency.Bind(Concurrency.Sleep(1000),function()
     {
      Var.Set(varMessage,m);
      Var.Set(varCanSubmit,true);
      return Concurrency.Return(null);
     });
    });
   })),null);
  }
  varMessage=Var.Create$1("nothing");
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
  varTemplate=Var.Create$1("");
  templateList=(new AjaxRemotingProvider.New()).Sync("JobApplicationSpam:JobApplicationSpam.Server.getTemplateNames:-1471467441",[]);
  varAddEmployerAction=Var.Create$1(AddEmployerAction.JustAddEmployer);
  return Doc.Element("form",[AttrProxy.Create("class","form-horizontal")],[Doc.Element("h1",[],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrAddEmployer))]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","company"),AttrProxy.Create("class","control-label col-sm-2 col-2")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrCompanyName))]),Doc.Element("div",[AttrProxy.Create("class","col-sm-7 col-7")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrCompanyName)),AttrProxy.Create("id","company"),AttrProxy.Create("value",varStreet.c)],varCompany)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","company"),AttrProxy.Create("class","control-label col-sm-2 col-2")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrCompanyStreet))]),Doc.Element("div",[AttrProxy.Create("class","col-sm-7 col-7")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrCompanyStreet)),AttrProxy.Create("id","street"),AttrProxy.Create("value",varStreet.c)],varStreet)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","company"),AttrProxy.Create("class","control-label col-sm-2 col-2")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrCompanyPostcode))]),Doc.Element("div",[AttrProxy.Create("class","col-sm-7 col-7")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrCompanyPostcode)),AttrProxy.Create("id","postcode"),AttrProxy.Create("value",varPostcode.c)],varPostcode)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","city"),AttrProxy.Create("class","control-label col-sm-2 col-2")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrCompanyCity))]),Doc.Element("div",[AttrProxy.Create("class","col-sm-7 col-7")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrCompanyCity)),AttrProxy.Create("id","city"),AttrProxy.Create("value",varCity.c)],varCity)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("class","control-label col-sm-2 col-2")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrBossGender))]),Doc.Element("br",[],[]),Doc.Element("div",[AttrProxy.Create("class","col-sm-7 col-7")],[Doc.Radio([AttrProxy.Create("id","male"),AttrProxy.Create("radiogroup","gender")],{
   $:0
  },varGender),Doc.Element("label",[AttrProxy.Create("for","male")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrMale))]),Doc.Element("br",[],[]),Doc.Radio([AttrProxy.Create("id","female"),AttrProxy.Create("radiogroup","gender")],{
   $:1
  },varGender),Doc.Element("label",[AttrProxy.Create("for","female")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrFemale))])])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","degree"),AttrProxy.Create("class","control-label col-sm-2 col-2")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrBossDegree))]),Doc.Element("div",[AttrProxy.Create("class","col-sm-7 col-7")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossDegree)),AttrProxy.Create("id","degree"),AttrProxy.Create("value",varDegree.c)],varDegree)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","firstName"),AttrProxy.Create("class","control-label col-sm-2 col-2")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrBossFirstName))]),Doc.Element("div",[AttrProxy.Create("class","col-sm-7 col-7")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossFirstName)),AttrProxy.Create("id","firstName"),AttrProxy.Create("value",varFirstName.c)],varFirstName)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","lastName"),AttrProxy.Create("class","control-label col-sm-2 col-2")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrBossLastName))]),Doc.Element("div",[AttrProxy.Create("class","col-sm-7 col-7")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossLastName)),AttrModule.Handler("change",function()
  {
   return function()
   {
    return Global.alert("");
   };
  }),AttrProxy.Create("value",varLastName.c)],varLastName)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","email"),AttrProxy.Create("class","control-label col-sm-2 col-2")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrBossEmail))]),Doc.Element("div",[AttrProxy.Create("class","col-sm-7 col-7")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossEmail)),AttrProxy.Create("id","email"),AttrProxy.Create("value",varEmail.c)],varEmail)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","phone"),AttrProxy.Create("class","control-label col-sm-2 col-2")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrBossPhone))]),Doc.Element("div",[AttrProxy.Create("class","col-sm-7 col-7")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossPhone)),AttrProxy.Create("id","phone"),AttrProxy.Create("value",varPhone.c)],varPhone)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("for","mobilePhone"),AttrProxy.Create("class","control-label col-sm-2 col-2")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrBossMobilePhone))]),Doc.Element("div",[AttrProxy.Create("class","col-sm-7 col-7")],[Doc.Input([AttrProxy.Create("class","form-control"),AttrProxy.Create("type","input"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossMobilePhone)),AttrProxy.Create("id","mobilePhone"),AttrProxy.Create("value",varMobilePhone.c)],varMobilePhone)])]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Element("label",[AttrProxy.Create("class","control-label col-sm-2 col-2")],[Doc.TextNode(Client.translate(Client.currentLanguage(),Str.StrBossMobilePhone))]),Doc.Element("div",[AttrProxy.Create("class","col-sm-7 col-7")],[Doc.Radio([AttrProxy.Create("class","form-control"),AttrProxy.Create("radiogroup","addEmployerAction"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossMobilePhone)),AttrProxy.Create("id","mobilePhone"),AttrProxy.Create("value","hallo")],AddEmployerAction.ApplyImmediately,varAddEmployerAction),Doc.Radio([AttrProxy.Create("class","form-control"),AttrProxy.Create("radiogroup","addEmployerAction"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossMobilePhone)),AttrProxy.Create("id","mobilePhone"),AttrProxy.Create("value",varMobilePhone.c)],AddEmployerAction.SendJobApplicationToUserOnly,varAddEmployerAction),Doc.Radio([AttrProxy.Create("class","form-control"),AttrProxy.Create("radiogroup","addEmployerAction"),AttrProxy.Create("placeholder",Client.translate(Client.currentLanguage(),Str.StrBossMobilePhone)),AttrProxy.Create("id","mobilePhone"),AttrProxy.Create("value",varMobilePhone.c)],AddEmployerAction.JustAddEmployer,varAddEmployerAction)])]),Doc.Element("div",[],[]),Doc.Element("div",[AttrProxy.Create("class","form-group")],[Doc.Button(Client.translate(Client.currentLanguage(),Str.StrAddEmployer),[AttrProxy.Create("type","submit"),AttrProxy.Create("class","form-control")],function()
  {
   if(varCanSubmit.c)
    {
     Var.Set(varCanSubmit,false);
     subm(createEmployer(varCompany.c,varStreet.c,varPostcode.c,varCity.c,varGender.c,varDegree.c,varFirstName.c,varLastName.c,varEmail.c,varPhone.c,varMobilePhone.c));
    }
  })]),Doc.TextView(varMessage.v)]);
 };
 Client.uploadTemplate1=function()
 {
  var varMessage;
  function subme()
  {
   Var.Set(varMessage,"Uploading");
  }
  varMessage=Var.Create$1("nothing");
  return Doc.Element("div",[],[Doc.Element("h1",[],[Doc.TextNode("Hello")]),Doc.Element("form",[AttrProxy.Create("enctype","multipart/form-data"),AttrProxy.Create("method","POST"),AttrModule.Handler("submit",function()
  {
   return function()
   {
    return subme();
   };
  })],[Doc.Element("input",[AttrProxy.Create("type","file"),AttrProxy.Create("name","myFile")],[]),Doc.Element("input",[AttrProxy.Create("type","submit")],[])]),Doc.TextView(varMessage.v)]);
 };
 Client.editUserValues=function()
 {
  var varMessage,varGender,varDegree,varFirstName,varLastName,varStreet,varPostcode,varCity,varPhone,varMobilePhone;
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
  function subm(userValues)
  {
   var b;
   Concurrency.StartImmediate((b=null,Concurrency.Delay(function()
   {
    Var.Set(varMessage,"Setting user values...");
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.setUserValues:1230958299",[userValues]),function(a)
    {
     var m;
     m=a.$==1?(function($1)
     {
      return function($2)
      {
       return $1(Utils.printList(Utils.prettyPrint,$2));
      };
     }(Global.id))(a.$0):a.$0;
     return Concurrency.Bind(Concurrency.Sleep(1000),function()
     {
      Var.Set(varMessage,m);
      return Concurrency.Return(null);
     });
    });
   })),null);
  }
  varMessage=Var.Create$1("nothing");
  varGender=Var.Create$1({
   $:0
  });
  varDegree=Var.Create$1("1");
  varFirstName=Var.Create$1("");
  varLastName=Var.Create$1("");
  varStreet=Var.Create$1("");
  varPostcode=Var.Create$1("");
  varCity=Var.Create$1("");
  varPhone=Var.Create$1("");
  varMobilePhone=Var.Create$1("");
  return Doc.Element("div",[],[Doc.Element("h1",[],[Doc.TextNode("Hello")]),Doc.Radio([AttrProxy.Create("id","male")],{
   $:0
  },varGender),Doc.Element("label",[AttrProxy.Create("for","male"),AttrProxy.Create("radiogroup","gender")],[Doc.TextNode("männlich")]),Doc.Element("br",[],[]),Doc.Radio([AttrProxy.Create("id","female"),AttrProxy.Create("radiogroup","gender")],{
   $:1
  },varGender),Doc.Element("label",[AttrProxy.Create("for","female")],[Doc.TextNode("weiblich")]),Doc.Element("br",[],[]),Doc.Element("label",[AttrProxy.Create("for","degree")],[Doc.TextNode("Titel")]),Doc.Input([AttrProxy.Create("type","input"),AttrProxy.Create("id","degree"),AttrProxy.Create("value",varDegree.c)],varDegree),Doc.Element("br",[],[]),Doc.Element("label",[AttrProxy.Create("for","firstName")],[Doc.TextNode("Vorname")]),Doc.Input([AttrProxy.Create("type","input"),AttrProxy.Create("id","firstName"),AttrProxy.Create("value",varFirstName.c)],varFirstName),Doc.Element("br",[],[]),Doc.Element("label",[AttrProxy.Create("for","lastName")],[Doc.TextNode("Nachname")]),Doc.Input([AttrProxy.Create("type","input"),AttrProxy.Create("name","lastName"),AttrProxy.Create("value",varLastName.c)],varLastName),Doc.Element("br",[],[]),Doc.Element("label",[AttrProxy.Create("for","street")],[Doc.TextNode("Straße")]),Doc.Input([AttrProxy.Create("type","input"),AttrProxy.Create("id","street"),AttrProxy.Create("value",varStreet.c)],varStreet),Doc.Element("br",[],[]),Doc.Element("label",[AttrProxy.Create("for","postcode")],[Doc.TextNode("Postleitzahl")]),Doc.Input([AttrProxy.Create("type","input"),AttrProxy.Create("id","postcode"),AttrProxy.Create("value",varPostcode.c)],varPostcode),Doc.Element("br",[],[]),Doc.Element("label",[AttrProxy.Create("for","city")],[Doc.TextNode("Stadt")]),Doc.Input([AttrProxy.Create("type","input"),AttrProxy.Create("id","city"),AttrProxy.Create("value",varCity.c)],varCity),Doc.Element("br",[],[]),Doc.Element("label",[AttrProxy.Create("for","phone")],[Doc.TextNode("Telefon")]),Doc.Input([AttrProxy.Create("type","input"),AttrProxy.Create("id","phone"),AttrProxy.Create("value",varPhone.c)],varPhone),Doc.Element("br",[],[]),Doc.Element("label",[AttrProxy.Create("for","mobilePhone")],[Doc.TextNode("Mobil")]),Doc.Input([AttrProxy.Create("type","input"),AttrProxy.Create("id","mobilePhone"),AttrProxy.Create("value",varMobilePhone.c)],varMobilePhone),Doc.Element("br",[],[]),Doc.Button("myBut",[AttrProxy.Create("type","submit")],function()
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
  return language.$==1?str.$==1?"weiblich":str.$==2?"Arbeitgeber eingeben":str.$==3?"Firmenname":str.$==4?"Straße":str.$==5?"Postleitzahl":str.$==6?"Stadt":str.$==7?"Geschlecht":str.$==8?"Titel":str.$==9?"Vorname":str.$==10?"Nachname":str.$==11?"Email":str.$==12?"Telefon":str.$==13?"Mobiltelefon":str.$==14?"Vorlage hochladen":str.$==16?"Email-Betreff":str.$==17?"Email-Text":str.$==15?"Name der Vorlage":str.$==18?"Beruf":str.$==19?"Datei hinzufügen":"männlich":str.$==1?"female":str.$==2?"Add Employer":str.$==3?"Company name":str.$==9?"First name":str.$==10?"Last name":str.$==4?"Street":str.$==5?"Postcode":str.$==6?"City":str.$==7?"Gender":str.$==8?"Degree":str.$==11?"Email":str.$==12?"Phone":str.$==13?"Mobile phone":str.$==14?"Add template":str.$==16?"Email subject":str.$==17?"Email body":str.$==15?"Template name":str.$==18?"Job":str.$==19?"Add file":"male";
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
