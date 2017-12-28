(function()
{
 "use strict";
 var Global,JobApplicationSpam,Types,JobApplicationContent,Client,Language,Str,AddEmployerAction,SC$1,JobApplicationSpam_GeneratedPrintf,IntelliFactory,Runtime,WebSharper,Concurrency,Remoting,AjaxRemotingProvider,UI,Next,Var,Strings,Seq,Utils,List,Arrays,Collections,Map,Doc,AttrProxy,AttrModule,Date,Numeric;
 Global=window;
 JobApplicationSpam=Global.JobApplicationSpam=Global.JobApplicationSpam||{};
 Types=JobApplicationSpam.Types=JobApplicationSpam.Types||{};
 JobApplicationContent=Types.JobApplicationContent=Types.JobApplicationContent||{};
 Client=JobApplicationSpam.Client=JobApplicationSpam.Client||{};
 Language=Client.Language=Client.Language||{};
 Str=Client.Str=Client.Str||{};
 AddEmployerAction=Client.AddEmployerAction=Client.AddEmployerAction||{};
 SC$1=Global.StartupCode$JobApplicationSpam$Client=Global.StartupCode$JobApplicationSpam$Client||{};
 JobApplicationSpam_GeneratedPrintf=Global.JobApplicationSpam_GeneratedPrintf=Global.JobApplicationSpam_GeneratedPrintf||{};
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 WebSharper=Global.WebSharper;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Remoting=WebSharper&&WebSharper.Remoting;
 AjaxRemotingProvider=Remoting&&Remoting.AjaxRemotingProvider;
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 Var=Next&&Next.Var;
 Strings=WebSharper&&WebSharper.Strings;
 Seq=WebSharper&&WebSharper.Seq;
 Utils=WebSharper&&WebSharper.Utils;
 List=WebSharper&&WebSharper.List;
 Arrays=WebSharper&&WebSharper.Arrays;
 Collections=WebSharper&&WebSharper.Collections;
 Map=Collections&&Collections.Map;
 Doc=Next&&Next.Doc;
 AttrProxy=Next&&Next.AttrProxy;
 AttrModule=Next&&Next.AttrModule;
 Date=Global.Date;
 Numeric=WebSharper&&WebSharper.Numeric;
 JobApplicationContent=Types.JobApplicationContent=Runtime.Class({
  toString:function()
  {
   return this.$==1?"Create":this.$==2?"Ignore":"Upload";
  }
 },null,JobApplicationContent);
 JobApplicationContent.Ignore=new JobApplicationContent({
  $:2
 });
 JobApplicationContent.Create=new JobApplicationContent({
  $:1
 });
 JobApplicationContent.Upload=new JobApplicationContent({
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
  var varUserValuesFromDb,b,varUserGender,varUserDegree,varUserFirstName,varUserLastName,varUserStreet,varUserPostcode,varUserCity,varUserPhone,varUserMobilePhone,varCompanyName,varCompanyStreet,varCompanyPostcode,varCompanyCity,varBossGender,varBossDegree,varBossFirstName,varBossLastName,varBossEmail,varBossPhone,varBossMobilePhone,varSubject,varMainText,varCover,varHtmlJobApplication,varHtmlJobApplicationNames,varHtmlJobApplicationName,varHtmlJobApplicationPageTemplateNames,varHtmlJobApplicationPageTemplateName,varTxtSaveAs,varHtmlJobApplicationId,varOAddEmployerId,b$1,varMessage,c;
  function fillHtmlJobApplication(htmlJobApplicationOffset)
  {
   var b$2;
   Concurrency.Start((b$2=null,Concurrency.Delay(function()
   {
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getHtmlJobApplicationOffset:-164096779",[htmlJobApplicationOffset]),function(a)
    {
     Var.Set(varHtmlJobApplication,a);
     Global.jQuery("#mainText").val(Strings.Replace(Seq.nth(0,a.pages).map.get_Item("mainText"),"\\n","\n"));
     return Concurrency.Zero();
    });
   })),null);
  }
  function fillHtmlJobApplicationNames()
  {
   var b$2;
   Concurrency.Start((b$2=null,Concurrency.Delay(function()
   {
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getHtmlJobApplicationNames:1994133801",[]),function(a)
    {
     return Concurrency.Combine(!(a.$==0)?(Var.Set(varHtmlJobApplicationName,a.get_Item(0)),Var.Set(varHtmlJobApplicationNames,a),Concurrency.Zero()):Concurrency.Zero(),Concurrency.Delay(function()
     {
      return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getHtmlJobApplicationPageTemplateNames:1994133801",[]),function(a$1)
      {
       return Concurrency.Combine(!(a$1.$==0)?(Var.Set(varHtmlJobApplicationPageTemplateName,a$1.get_Item(0)),Var.Set(varHtmlJobApplicationPageTemplateNames,a$1),Concurrency.Zero()):Concurrency.Zero(),Concurrency.Delay(function()
       {
        fillHtmlJobApplication(Seq.length(a)-1);
        return Concurrency.Zero();
       }));
      });
     }));
    });
   })),null);
  }
  function updateMainText()
  {
   if(varBossGender.c.$==0)
    Var.Set(varMainText,"Sehr geehrter Herr "+varBossDegree.c+" "+varBossLastName.c+"\n"+varMainText.c);
   else
    Var.Set(varMainText,"Sehr geehrte Frau "+varBossDegree.c+" "+varBossLastName.c+"\n"+varMainText.c);
  }
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
        {
         Global.alert(Global.String(currentIndex-1));
         return currentIndex-1;
        }
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
    return state.$==1?(x=state.$0,(splitIndex=findLineBreak(x,textAreaWidth,font,fontSize,fontWeight),(splitIndexSpace=($1=Arrays.tryFindIndexBack(function(c$1)
    {
     return List.contains(c$1,List.ofArray([" ",".",",",";","-"]));
    },Arrays.take(splitIndex,Strings.ToCharArray(x))),($2=splitIndex===x.length,$1!=null&&$1.$==1?$2?splitIndex:$1.$0+1:splitIndex)),(Global.alert(Global.String(splitIndexSpace)),front=Strings.Substring(x,0,splitIndexSpace),back=x.substring(splitIndexSpace),$3=state.$1,back===""?$3.$==1?{
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
    })))):null;
   },List.ofArray(Arrays.map(function(x)
   {
    return Strings.EndsWith(x," ")?Strings.TrimEnd(x,[" "])+"\n":x;
   },Strings.SplitChars(str,["\n"],0))));
  }
  function saveHtmlJobApplication(htmlJobApplicationName)
  {
   var b$2;
   Concurrency.Start((b$2=null,Concurrency.Delay(function()
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
     Var.Set(varHtmlJobApplicationId,a);
     return Concurrency.Zero();
    });
   })),null);
  }
  function setUserValues()
  {
   var b$2;
   Concurrency.Start((b$2=null,Concurrency.Delay(function()
   {
    var userValues;
    Var.Set(varMessage,"Setting user values...");
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
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.addUserValues:1230958299",[userValues]),function(a)
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
  function addEmployer()
  {
   var employer,b$2;
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
   Concurrency.Start((b$2=null,Concurrency.Delay(function()
   {
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.addEmployer:-762170454",[employer]),function(a)
    {
     Var.Set(varOAddEmployerId,a.$==1?null:{
      $:1,
      $0:a.$0
     });
     return Concurrency.Zero();
    });
   })),null);
  }
  function applyNowWithHtmlTemplate()
  {
   var b$2;
   Concurrency.Start((b$2=null,Concurrency.Delay(function()
   {
    var htmlJobApplication,userValues,employer;
    return varOAddEmployerId.c==null?Concurrency.Zero():(htmlJobApplication={
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
    },((new AjaxRemotingProvider.New()).Send("JobApplicationSpam:JobApplicationSpam.Server.applyNowWithHtmlTemplate:2021728765",[employer,htmlJobApplication,userValues]),Concurrency.Zero()))));
   })),null);
  }
  varUserValuesFromDb=Var.Create$1({
   gender:{
    $:0
   },
   degree:"",
   firstName:"",
   lastName:"",
   street:"",
   postcode:"",
   city:"",
   phone:"",
   mobilePhone:""
  });
  Concurrency.Start((b=null,Concurrency.Delay(function()
  {
   return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getCurrentUserValues:-337599557",[]),function(a)
   {
    Var.Set(varUserValuesFromDb,a);
    return Concurrency.Zero();
   });
  })),null);
  varUserGender=Var.Create$1(varUserValuesFromDb.c.gender);
  varUserDegree=Var.Create$1(varUserValuesFromDb.c.degree);
  varUserFirstName=Var.Create$1(varUserValuesFromDb.c.firstName);
  varUserLastName=Var.Create$1(varUserValuesFromDb.c.lastName);
  varUserStreet=Var.Create$1(varUserValuesFromDb.c.street);
  varUserPostcode=Var.Create$1(varUserValuesFromDb.c.postcode);
  varUserCity=Var.Create$1(varUserValuesFromDb.c.city);
  varUserPhone=Var.Create$1(varUserValuesFromDb.c.phone);
  varUserMobilePhone=Var.Create$1(varUserValuesFromDb.c.mobilePhone);
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
  varSubject=Var.Create$1("Bewerbung als ...");
  varMainText=Var.Create$1("");
  varCover=Var.Create$1(JobApplicationContent.Upload);
  varHtmlJobApplication=Var.Create$1({
   name:"",
   pages:List.T.Empty
  });
  varHtmlJobApplicationNames=Var.Create$1(List.ofArray([""]));
  varHtmlJobApplicationName=Var.Create$1("");
  varHtmlJobApplicationPageTemplateNames=Var.Create$1(List.ofArray([""]));
  varHtmlJobApplicationPageTemplateName=Var.Create$1("");
  varTxtSaveAs=Var.Create$1("");
  varHtmlJobApplicationId=Var.Create$1(0);
  varOAddEmployerId=Var.Create$1(null);
  fillHtmlJobApplicationNames();
  Concurrency.Start((b$1=null,Concurrency.Delay(function()
  {
   return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getCurrentUserValues:-337599557",[]),function(a)
   {
    Var.Set(varUserGender,a.gender);
    Var.Set(varUserDegree,a.degree);
    Var.Set(varUserFirstName,a.firstName);
    Var.Set(varUserLastName,a.lastName);
    Var.Set(varUserStreet,a.street);
    Var.Set(varUserPostcode,a.postcode);
    Var.Set(varUserCity,a.city);
    Var.Set(varUserPhone,a.phone);
    Var.Set(varUserMobilePhone,a.mobilePhone);
    return Concurrency.Zero();
   });
  })),null);
  varMessage=Var.Create$1("nothing");
  return Doc.Element("div",[],[Doc.Element("h1",[],[Doc.TextNode("Create a template")]),Doc.SelectDyn([AttrProxy.Create("style","min-width: 300px"),AttrProxy.Create("id","selectHtmlJobApplicationName"),AttrModule.Handler("change",function(el)
  {
   return function()
   {
    return fillHtmlJobApplication(el.selectedIndex);
   };
  })],Global.id,varHtmlJobApplicationNames.v,varHtmlJobApplicationName),Doc.SelectDyn([AttrProxy.Create("style","min-width: 300px"),AttrProxy.Create("id","selectHtmlJobApplicationPageTemplate")],Global.id,varHtmlJobApplicationPageTemplateNames.v,varHtmlJobApplicationPageTemplateName),Doc.Element("div",[],[Doc.Radio([AttrProxy.Create("id","uploadCover"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return Global.alert(Global.String(varCover.c));
   };
  }),AttrProxy.Create("radiogroup","cover")],JobApplicationContent.Upload,varCover),Doc.Element("label",[AttrProxy.Create("for","uploadCover")],[Doc.TextNode("Hochladen")]),Doc.Element("br",[],[]),Doc.Radio([AttrProxy.Create("id","createCover"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return Global.alert(Global.String(varCover.c));
   };
  }),AttrProxy.Create("radiogroup","cover")],JobApplicationContent.Create,varCover),Doc.Element("label",[AttrProxy.Create("for","createCover")],[Doc.TextNode("Online erstellen")]),Doc.Element("br",[],[]),Doc.Radio([AttrProxy.Create("id","ignoreCover"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return Global.alert(Global.String(varCover.c));
   };
  }),AttrProxy.Create("radiogroup","cover")],JobApplicationContent.Ignore,varCover),Doc.Element("label",[AttrProxy.Create("for","ignoreCover")],[Doc.TextNode("Nicht verwenden")]),Doc.Element("br",[],[])]),Doc.Element("div",[AttrProxy.Create("class","page")],[Doc.Element("div",[AttrProxy.Create("style","height: 225pt; width: 100%; background-color: lightblue")],[Doc.Input([AttrProxy.Create("class","grow-input"),AttrProxy.Create("autofocus","autofocus"),AttrModule.Handler("input",function(el)
  {
   return function()
   {
    resize(Global.jQuery(el),"Arial","12pt","normal",150);
    return fillHtmlJobApplicationNames();
   };
  }),AttrProxy.Create("placeholder","Dein Titel")],varUserDegree),Doc.Input([AttrProxy.Create("class","grow-input"),AttrModule.Handler("input",function(el)
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
  }),AttrProxy.Create("placeholder","Deine Stadt")],varUserCity),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Select([AttrModule.Handler("change",function()
  {
   return function()
   {
    return updateMainText();
   };
  })],function(x)
  {
   return x.$==1?"Frau":"Herrn";
  },List.ofArray([{
   $:0
  },{
   $:1
  }]),varBossGender),Doc.Element("br",[],[]),Doc.Input([AttrProxy.Create("class","grow-input"),AttrModule.Handler("input",function(el)
  {
   return function()
   {
    return resize(Global.jQuery(el),"Arial","12pt","normal",150);
   };
  }),AttrProxy.Create("placeholder","Chef-Titel")],varBossDegree),Doc.Input([AttrProxy.Create("class","grow-input"),AttrModule.Handler("input",function(el)
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
  }),AttrProxy.Create("placeholder","Betreff")],varSubject),Doc.Element("br",[],[]),Doc.Element("br",[],[])]),Doc.Element("div",[AttrProxy.Create("style","width:100%; min-height: 322.4645709pt; background-color:red;")],[Doc.InputArea([AttrProxy.Create("id","mainText"),AttrProxy.Create("style","wrap: soft; border: none; outline: none; letter-spacing:0pt; margin: 0px; padding: 0px; background-color: lighblue; overflow: hidden; min-height: 322.4645709pt; min-width:100%; font-family: Arial; font-size: 12pt; font-weight: normal; display: block")],varMainText)]),Doc.Element("div",[AttrProxy.Create("style","height:96pt; width: 100%;")],[Doc.Element("br",[],[]),Doc.TextNode("Mit freundlichen Grüßen"),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.TextView(varUserDegree.v),Doc.TextNode(" "),Doc.TextView(varUserFirstName.v),Doc.TextNode(" "),Doc.TextView(varUserLastName.v)])]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("value","Speichern als"),AttrModule.Handler("click",function()
  {
   return function()
   {
    return saveHtmlJobApplication(varTxtSaveAs.c);
   };
  })],[Doc.TextNode("Speichern als")]),Doc.Input([],varTxtSaveAs),Doc.Element("br",[],[]),Doc.Element("input",[AttrProxy.Create("type","button"),AttrProxy.Create("value","Abschicken"),AttrModule.Handler("click",function()
  {
   return function()
   {
    setUserValues();
    addEmployer();
    saveHtmlJobApplication(varHtmlJobApplicationName.c);
    return applyNowWithHtmlTemplate();
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
  }(Global.id))((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getCurrentUserId:-646436276",[])))]);
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
    Concurrency.Start((b=null,Concurrency.Delay(function()
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
  return Doc.Element("div",[],[]);
 };
 Client.editUserValues=function()
 {
  var varMessage,varGender,varDegree,varFirstName,varLastName,varStreet,varPostcode,varCity,varPhone,varMobilePhone,b;
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
   var b$1;
   Concurrency.Start((b$1=null,Concurrency.Delay(function()
   {
    Var.Set(varMessage,"Setting user values...");
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.addUserValues:1230958299",[userValues]),function(a)
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
  varDegree=Var.Create$1("");
  varFirstName=Var.Create$1("");
  varLastName=Var.Create$1("");
  varStreet=Var.Create$1("");
  varPostcode=Var.Create$1("");
  varCity=Var.Create$1("");
  varPhone=Var.Create$1("");
  varMobilePhone=Var.Create$1("");
  Concurrency.Start((b=null,Concurrency.Delay(function()
  {
   return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.getCurrentUserValues:-337599557",[]),function(a)
   {
    Var.Set(varGender,a.gender);
    Var.Set(varDegree,a.degree);
    Var.Set(varFirstName,a.firstName);
    Var.Set(varLastName,a.lastName);
    Var.Set(varStreet,a.street);
    Var.Set(varPostcode,a.postcode);
    Var.Set(varCity,a.city);
    Var.Set(varPhone,a.phone);
    Var.Set(varMobilePhone,a.mobilePhone);
    return Concurrency.Zero();
   });
  })),null);
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
  return"P <fun>";
 };
}());
