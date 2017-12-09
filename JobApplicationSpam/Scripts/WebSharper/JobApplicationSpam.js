(function()
{
 "use strict";
 var Global,JobApplicationSpam,Client,WebSharper,UI,Next,Var,Concurrency,Remoting,AjaxRemotingProvider,Utils,Doc,AttrProxy;
 Global=window;
 JobApplicationSpam=Global.JobApplicationSpam=Global.JobApplicationSpam||{};
 Client=JobApplicationSpam.Client=JobApplicationSpam.Client||{};
 WebSharper=Global.WebSharper;
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 Var=Next&&Next.Var;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Remoting=WebSharper&&WebSharper.Remoting;
 AjaxRemotingProvider=Remoting&&Remoting.AjaxRemotingProvider;
 Utils=WebSharper&&WebSharper.Utils;
 Doc=Next&&Next.Doc;
 AttrProxy=Next&&Next.AttrProxy;
 Client.editUserValues=function()
 {
  var varMessage,varGender,varDegree,varFirstName,varLastName,varStreet,varPostcode,varCity,varPhone,varMobilePhone;
  function subm(userValues)
  {
   var b;
   Var.Set(varMessage,"Doing something");
   Concurrency.StartImmediate((b=null,Concurrency.Delay(function()
   {
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.setUserValues:1239847983",[userValues,1]),function(a)
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
      Global.alert("Hallo");
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
   subm(Client.createUserValues(varGender.c,varDegree.c,varFirstName.c,varLastName.c,varStreet.c,varPostcode.c,varCity.c,varPhone.c,varMobilePhone.c));
  }),Doc.TextView(varMessage.v)]);
 };
 Client.createUserValues=function(gender,degree,firstName,lastName,street,postcode,city,phone,mobilePhone)
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
}());
