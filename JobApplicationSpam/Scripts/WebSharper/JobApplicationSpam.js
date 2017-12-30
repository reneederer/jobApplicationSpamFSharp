(function()
{
 "use strict";
 var Global,JobApplicationSpam,Types,JobApplicationPageAction,Client,Language,Str,AddEmployerAction,SC$1,IntelliFactory,Runtime,WebSharper,Strings,Utils,List,Arrays,Concurrency,Collections,Map,Remoting,AjaxRemotingProvider,Unchecked,UI,Next,Var,Doc,Seq,AttrModule,Date,FSharpMap,AttrProxy;
 Global=window;
 JobApplicationSpam=Global.JobApplicationSpam=Global.JobApplicationSpam||{};
 Types=JobApplicationSpam.Types=JobApplicationSpam.Types||{};
 JobApplicationPageAction=Types.JobApplicationPageAction=Types.JobApplicationPageAction||{};
 Client=JobApplicationSpam.Client=JobApplicationSpam.Client||{};
 Language=Client.Language=Client.Language||{};
 Str=Client.Str=Client.Str||{};
 AddEmployerAction=Client.AddEmployerAction=Client.AddEmployerAction||{};
 SC$1=Global.StartupCode$JobApplicationSpam$Client=Global.StartupCode$JobApplicationSpam$Client||{};
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 WebSharper=Global.WebSharper;
 Strings=WebSharper&&WebSharper.Strings;
 Utils=WebSharper&&WebSharper.Utils;
 List=WebSharper&&WebSharper.List;
 Arrays=WebSharper&&WebSharper.Arrays;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Collections=WebSharper&&WebSharper.Collections;
 Map=Collections&&Collections.Map;
 Remoting=WebSharper&&WebSharper.Remoting;
 AjaxRemotingProvider=Remoting&&Remoting.AjaxRemotingProvider;
 Unchecked=WebSharper&&WebSharper.Unchecked;
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 Var=Next&&Next.Var;
 Doc=Next&&Next.Doc;
 Seq=WebSharper&&WebSharper.Seq;
 AttrModule=Next&&Next.AttrModule;
 Date=Global.Date;
 FSharpMap=Collections&&Collections.FSharpMap;
 AttrProxy=Next&&Next.AttrProxy;
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
  var varUserGender,varUserDegree,varUserFirstName,varUserLastName,varUserStreet,varUserPostcode,varUserCity,varUserPhone,varUserMobilePhone,varCompanyName,varCompanyStreet,varCompanyPostcode,varCompanyCity,varBossGender,varBossDegree,varBossFirstName,varBossLastName,varBossEmail,varBossPhone,varBossMobilePhone,varMainText,varHtmlJobApplication,varHtmlJobApplicationNames,varHtmlJobApplicationName,varHtmlJobApplicationPageTemplateNames,varHtmlJobApplicationPageTemplateName,varPageTemplate,varPageTemplateMap,varPages,uploadDiv,createDiv,varPageActionDiv;
  function resize(el,defaultWidth)
  {
   var jEl,str,span;
   jEl=Global.jQuery(el);
   str=Strings.Replace(Global.String(jEl.val())," ","&nbsp;");
   str===""?jEl.width(defaultWidth):(span=Global.jQuery("<span />").attr("style",((((Runtime.Curried(function($1,$2,$3,$4)
   {
    return $1("font-family:"+Utils.toSafe($2)+"; font-size: "+Utils.toSafe($3)+"; font-weight: "+Utils.toSafe($4)+"; visibility: hidden");
   },4))(Global.id))(el.style.fontFamily))(el.style.fontSize))(el.style.fontWeight)).html(str),span.appendTo("body"),jEl.width(span.width()),Global.jQuery("body span").last().remove());
   return null;
  }
  function getWidth(s,font,fontSize,fontWeight)
  {
   var str,span,spanWidth;
   str=Strings.Replace(Global.String(s)," ","&nbsp;");
   span=Global.jQuery("<span />").attr("style",((((Runtime.Curried(function($1,$2,$3,$4)
   {
    return $1("font-family: "+Utils.toSafe($2)+"; font-size: "+Utils.toSafe($3)+"; font-weight: "+Utils.toSafe($4)+"; letter-spacing:0pt; visibility: hidden;");
   },4))(Global.id))(font))(fontSize))(fontWeight)).html(str);
   span.appendTo("body");
   spanWidth=span.width();
   Global.jQuery("body span:last").remove();
   return spanWidth;
  }
  function findLineBreak(str,textAreaWidth,font,fontSize,fontWeight)
  {
   var beginIndex,endIndex,n,currentIndex,currentString,width;
   beginIndex=0;
   endIndex=str.length;
   n=30;
   while(true)
    if(n<0)
     return str.length;
    else
     {
      currentIndex=beginIndex+((endIndex-beginIndex+1)/2>>0);
      currentString=Strings.Substring(str,0,currentIndex);
      width=getWidth(currentString,font,fontSize,fontWeight);
      if(width>textAreaWidth)
      {
       if(endIndex===currentIndex)
        return currentIndex-1;
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
  }
  function getTextAreaLines(str,textAreaWidth,font,fontSize,fontWeight)
  {
   return List.unfold(function(state)
   {
    var x,splitIndex,splitIndexSpace,$1,$2,front,back,$3;
    return state.$==1?(x=state.$0,(splitIndex=findLineBreak(x,textAreaWidth,font,fontSize,fontWeight),(splitIndexSpace=($1=Arrays.tryFindIndexBack(function(c)
    {
     return List.contains(c,List.ofArray([" ",".",",",";","-"]));
    },Arrays.take(splitIndex,Strings.ToCharArray(x))),($2=splitIndex===x.length,$1!=null&&$1.$==1?$2?splitIndex:$1.$0+1:splitIndex)),(front=Strings.Substring(x,0,splitIndexSpace),(back=x.substring(splitIndexSpace),($3=state.$1,back===""?$3.$==1?{
     $:1,
     $0:[front,new List.T({
      $:1,
      $0:$3.$0,
      $1:$3.$1
     })]
    }:{
     $:1,
     $0:[front,List.T.Empty]
    }:$3.$==1?{
     $:1,
     $0:[front,new List.T({
      $:1,
      $0:back,
      $1:new List.T({
       $:1,
       $0:$3.$0,
       $1:$3.$1
      })
     })]
    }:{
     $:1,
     $0:[front,List.ofArray([back])]
    })))))):null;
   },List.ofArray(Arrays.map(function(x)
   {
    return Strings.EndsWith(x," ")?Strings.TrimEnd(x,[" "])+"\n":x;
   },Strings.SplitChars(str,["\n"],0))));
  }
  function saveHtmlJobApplication(htmlJobApplicationName)
  {
   var b;
   b=null;
   return Concurrency.Delay(function()
   {
    var htmlJobApplication;
    htmlJobApplication={
     name:htmlJobApplicationName,
     pages:List.ofArray([{
      name:"Anschreiben",
      jobApplicationPageTemplateId:1,
      map:Map.OfArray(Arrays.ofSeq(List.ofArray([["mainText",Strings.concat("\n",getTextAreaLines(varMainText.c,Global.jQuery("#mainText").width(),"Arial","12pt","normal"))]])))
     }])
    };
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.saveHtmlJobApplication:-1556746703",[htmlJobApplication]),function(a)
    {
     return Concurrency.Return(a);
    });
   });
  }
  function setUserValues()
  {
   var b;
   b=null;
   return Concurrency.Delay(function()
   {
    var userValues;
    userValues={
     gender:varUserGender.c,
     degree:varUserDegree.c,
     firstName:varUserFirstName.c,
     lastName:varUserLastName.c,
     street:varUserStreet.c,
     postcode:varUserPostcode.c,
     city:varUserCity.c,
     phone:varUserPhone.c,
     mobilePhone:varUserMobilePhone.c
    };
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.addUserValues:1230958299",[userValues]),function()
    {
     return Concurrency.Return(null);
    });
   });
  }
  function addEmployer()
  {
   var b;
   b=null;
   return Concurrency.Delay(function()
   {
    var employer;
    employer={
     company:varCompanyName.c,
     street:varCompanyStreet.c,
     postcode:varCompanyPostcode.c,
     city:varCompanyCity.c,
     gender:varBossGender.c,
     degree:varBossDegree.c,
     firstName:varBossFirstName.c,
     lastName:varBossLastName.c,
     email:varBossEmail.c,
     phone:varBossPhone.c,
     mobilePhone:varBossMobilePhone.c
    };
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.addEmployer:-762170454",[employer]),function(a)
    {
     return Concurrency.Return(a.$==1?null:{
      $:1,
      $0:a.$0
     });
    });
   });
  }
  function applyNowWithHtmlTemplate()
  {
   var b;
   Concurrency.Start((b=null,Concurrency.Delay(function()
   {
    return Concurrency.Bind(addEmployer(),function(a)
    {
     return Concurrency.Bind(setUserValues(),function()
     {
      return Concurrency.Bind(saveHtmlJobApplication(varHtmlJobApplicationName.c),function()
      {
       var htmlJobApplication,userValues,employer;
       return a==null?Concurrency.Zero():(htmlJobApplication={
        name:varHtmlJobApplication.c.name,
        pages:List.ofArray([{
         name:"Anschreiben",
         jobApplicationPageTemplateId:1,
         map:Map.OfArray(Arrays.ofSeq(List.ofArray([["mainText",varMainText.c]])))
        }])
       },(userValues={
        gender:varUserGender.c,
        degree:varUserDegree.c,
        firstName:varUserFirstName.c,
        lastName:varUserLastName.c,
        street:varUserStreet.c,
        postcode:varUserPostcode.c,
        city:varUserCity.c,
        phone:varUserPhone.c,
        mobilePhone:varUserMobilePhone.c
       },(employer={
        company:varCompanyName.c,
        street:varCompanyStreet.c,
        postcode:varCompanyPostcode.c,
        city:varCompanyCity.c,
        gender:varBossGender.c,
        degree:varBossDegree.c,
        firstName:varBossFirstName.c,
        lastName:varBossLastName.c,
        email:varBossEmail.c,
        phone:varBossPhone.c,
        mobilePhone:varBossMobilePhone.c
       },((new AjaxRemotingProvider.New()).Send("JobApplicationSpam:JobApplicationSpam.Server.applyNowWithHtmlTemplate:-77811991",[employer,a.$0,htmlJobApplication,userValues]),Concurrency.Zero()))));
      });
     });
    });
   })),null);
  }
  function prepare()
  {
   var b;
   Concurrency.Start((b=null,Concurrency.Delay(function()
   {
    return Concurrency.Combine(Unchecked.Equals(varHtmlJobApplicationNames.c,List.ofArray([""]))?Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getHtmlJobApplicationNames:1994133801",[]),function(a)
    {
     return!(a.$==0)?(Var.Set(varHtmlJobApplicationName,a.get_Item(0)),Var.Set(varHtmlJobApplicationNames,a),Concurrency.Zero()):Concurrency.Zero();
    }):Concurrency.Zero(),Concurrency.Delay(function()
    {
     return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getHtmlJobApplicationPageTemplates:-611341156",[]),function(a)
     {
      Var.Set(varHtmlJobApplicationPageTemplateName,List.head(a).name);
      Var.Set(varHtmlJobApplicationPageTemplateNames,List.map(function(t)
      {
       return t.name;
      },a));
      Var.Set(varPageTemplateMap,Map.OfArray(Arrays.ofSeq(List.map(function(t)
      {
       return[t.name,t.html];
      },a))));
      Var.Set(varPageTemplate,Doc.Verbatim(varPageTemplateMap.c.get_Item(varHtmlJobApplicationPageTemplateName.c)));
      return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getHtmlJobApplicationPages:1412016016",[Global.document.getElementById("selectHtmlJobApplicationName").selectedIndex+1]),function(a$1)
      {
       var el;
       Var.Set(varPages,Doc.Element("ul",[],List.ofSeq(Seq.delay(function()
       {
        return Seq.map(function(htmlJobApplicationPageDB)
        {
         return Doc.Element("li",[],[Doc.Element("button",[AttrModule.Handler("click",function()
         {
          return function()
          {
           return Global.alert(htmlJobApplicationPageDB.name+", "+Global.String(htmlJobApplicationPageDB.htmlJobApplicationPageTemplateId));
          };
         })],[Doc.TextNode(htmlJobApplicationPageDB.name)])]);
        },a$1);
       }))));
       el=Global.document.getElementById("selectHtmlJobApplicationPageTemplate");
       return!Unchecked.Equals(el,null)?(el.selectedIndex=0,Global.jQuery(".resizing").each(function($1,el$1)
       {
        el$1.addEventListener("input",function()
        {
         return resize(el$1,150);
        },true);
       }),Global.jQuery(".field-updating").each(function($1,el$1)
       {
        el$1.addEventListener("input",function()
        {
         var updateField;
         updateField=Global.String(Global.jQuery(el$1).data("update-field"));
         updateField==="userDegree"?Global.alert("userDegree"):updateField==="userFirstName"?Global.alert("FirstName!"):updateField==="userLastName"?Global.alert("LastName!"):void 0;
         Global.jQuery((function($2)
         {
          return function($3)
          {
           return $2("[data-update-field='"+Utils.toSafe($3)+"']");
          };
         }(Global.id))(Global.String(Global.jQuery(el$1).data("update-field")))).each(function($2,updateElement)
         {
          if(!Unchecked.Equals(updateElement,el$1))
           {
            Global.jQuery(updateElement).val(Global.String(Global.jQuery(el$1).val()));
            resize(updateElement,150);
           }
         });
         return null;
        },true);
       }),Global.jQuery("[data-variable-value]").each(function($1,$2)
       {
        var jEl,c,c$1,c$2;
        jEl=Global.jQuery($2);
        Global.String(jEl.data("variable-value"))==="today"?jEl.val(((((Runtime.Curried(function($3,$4,$5,$6)
        {
         return $3(Global.String($4)+"."+Global.String($5)+"."+Global.String($6));
        },4))(Global.id))((c=Date.now(),(new Date(c)).getDate())))((c$1=Date.now(),(new Date(c$1)).getMonth()+1)))((c$2=Date.now(),(new Date(c$2)).getFullYear()))):void 0;
       }),Concurrency.Zero()):Concurrency.Zero();
      });
     });
    }));
   })),null);
  }
  varUserGender=Var.Create$1({
   $:1
  });
  varUserDegree=Var.Create$1("");
  varUserFirstName=Var.Create$1("");
  varUserLastName=Var.Create$1("");
  varUserStreet=Var.Create$1("");
  varUserPostcode=Var.Create$1("");
  varUserCity=Var.Create$1("");
  varUserPhone=Var.Create$1("");
  varUserMobilePhone=Var.Create$1("");
  varCompanyName=Var.Create$1("");
  varCompanyStreet=Var.Create$1("");
  varCompanyPostcode=Var.Create$1("");
  varCompanyCity=Var.Create$1("");
  varBossGender=Var.Create$1({
   $:0
  });
  varBossDegree=Var.Create$1("");
  varBossFirstName=Var.Create$1("");
  varBossLastName=Var.Create$1("");
  varBossEmail=Var.Create$1("");
  varBossPhone=Var.Create$1("");
  varBossMobilePhone=Var.Create$1("");
  Var.Create$1("Bewerbung als ...");
  varMainText=Var.Create$1("");
  varHtmlJobApplication=Var.Create$1({
   name:"",
   pages:List.T.Empty
  });
  varHtmlJobApplicationNames=Var.Create$1(List.ofArray([""]));
  varHtmlJobApplicationName=Var.Create$1("");
  varHtmlJobApplicationPageTemplateNames=Var.Create$1(List.ofArray([""]));
  varHtmlJobApplicationPageTemplateName=Var.Create$1("");
  Var.Create$1("");
  varPageTemplate=Var.Create$1(Doc.Verbatim("<div>nothing here yet</div>"));
  varPageTemplateMap=Var.Create$1(new FSharpMap.New([]));
  varPages=Var.Create$1(Doc.Verbatim("<div>nothin to see here</div>"));
  uploadDiv=Doc.Element("div",[],[Doc.TextNode("Uploading")]);
  createDiv=Doc.Element("div",[],[Doc.SelectDyn([AttrProxy.Create("style","min-width: 300px"),AttrProxy.Create("id","selectHtmlJobApplicationPageTemplate"),AttrModule.Handler("change",function()
  {
   return function()
   {
    return varPageTemplateMap.c.ContainsKey(varHtmlJobApplicationPageTemplateName.c)?Var.Set(varPageTemplate,Doc.Verbatim(varPageTemplateMap.c.get_Item(varHtmlJobApplicationPageTemplateName.c))):null;
   };
  })],Global.id,varHtmlJobApplicationPageTemplateNames.v,varHtmlJobApplicationPageTemplateName),Doc.Element("br",[],[]),Doc.EmbedView(varPageTemplate.v),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("value","Abschicken"),AttrModule.Handler("click",function()
  {
   return function()
   {
    applyNowWithHtmlTemplate();
    return null;
   };
  })],[])]);
  Doc.Element("div",[],[Doc.TextNode("UseCreated!")]);
  varPageActionDiv=Var.Create$1(uploadDiv);
  Global.jQuery().ready(function()
  {
   return prepare();
  });
  return Doc.Element("div",[],[Doc.Element("h1",[],[Doc.TextNode("Create a template")]),Doc.SelectDyn([AttrProxy.Create("style","min-width: 300px"),AttrProxy.Create("id","selectHtmlJobApplicationName"),AttrModule.Handler("change",function()
  {
   return function()
   {
    return prepare();
   };
  })],Global.id,varHtmlJobApplicationNames.v,varHtmlJobApplicationName),Doc.Element("br",[],[]),Doc.TextNode("Pages: "),Doc.Element("br",[],[]),Doc.EmbedView(varPages.v),Doc.Element("br",[],[]),Doc.Element("hr",[],[]),Doc.Element("br",[],[]),Doc.Element("div",[],[Doc.Radio([AttrProxy.Create("id","upload"),AttrProxy.Create("radiogroup","pageAction"),AttrModule.Handler("change",function()
  {
   return function()
   {
    return prepare();
   };
  })],uploadDiv,varPageActionDiv),Doc.Element("label",[AttrProxy.Create("for","upload")],[Doc.TextNode("Hochladen")]),Doc.Element("br",[],[]),Doc.Radio([AttrProxy.Create("id","create"),AttrProxy.Create("radiogroup","pageAction"),AttrModule.Handler("change",function()
  {
   return function()
   {
    return prepare();
   };
  })],createDiv,varPageActionDiv),Doc.Element("label",[AttrProxy.Create("for","create")],[Doc.TextNode("Online erstellen")]),Doc.Element("br",[],[])]),Doc.Element("br",[],[]),Doc.Element("hr",[],[]),Doc.Element("br",[],[]),Doc.EmbedView(varPageActionDiv.v)]);
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
 Client.uploadTemplate=function()
 {
  return Doc.Element("div",[],[]);
 };
 Client.applyNow=function()
 {
  return Doc.Element("div",[],[]);
 };
 Client.addEmployer=function()
 {
  return Doc.Element("div",[],[]);
 };
 Client.editUserValues=function()
 {
  return Doc.Element("div",[],[]);
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
