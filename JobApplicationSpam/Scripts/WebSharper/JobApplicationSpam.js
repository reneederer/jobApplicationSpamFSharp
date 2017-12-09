(function()
{
 "use strict";
 var Global,JobApplicationSpam,Client,WebSharper,Concurrency,Remoting,AjaxRemotingProvider,Utils,UI,Next,Var,Doc,AttrProxy,Strings;
 Global=window;
 JobApplicationSpam=Global.JobApplicationSpam=Global.JobApplicationSpam||{};
 Client=JobApplicationSpam.Client=JobApplicationSpam.Client||{};
 WebSharper=Global.WebSharper;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Remoting=WebSharper&&WebSharper.Remoting;
 AjaxRemotingProvider=Remoting&&Remoting.AjaxRemotingProvider;
 Utils=WebSharper&&WebSharper.Utils;
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 Var=Next&&Next.Var;
 Doc=Next&&Next.Doc;
 AttrProxy=Next&&Next.AttrProxy;
 Strings=WebSharper&&WebSharper.Strings;
 Client.editUserValues=function()
 {
  var degreeVar,userValues,b;
  function subm(userValues$1)
  {
   var b$1;
   Concurrency.Start((b$1=null,Concurrency.Delay(function()
   {
    return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.setUserValues:1239847983",[userValues$1,1]),function(a)
    {
     a.$==1?Global.alert((function($1)
     {
      return function($2)
      {
       return $1(Utils.printList(Utils.prettyPrint,$2));
      };
     }(Global.id))(a.$0)):Global.alert(a.$0);
     return Concurrency.Return(null);
    });
   })),null);
  }
  Client.createUserValues1({
   $:0
  },"dr","rene","ederer","Raabstr. 24A","90429","Nuernberg","noPhone","noMobilePhone");
  Client.createUserValues1({
   $:0
  },"dr","rene","ederer","Raabstr. 24A","90429","Nuernberg","noPhone","noMobilePhone");
  degreeVar=Var.Create$1("2");
  return Doc.Element("div",[],[Doc.Element("h1",[],[Doc.TextNode("Hello")]),Doc.Input([AttrProxy.Create("type","input"),AttrProxy.Create("id","degree"),AttrProxy.Create("value",degreeVar.c)],degreeVar),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Element("br",[],[]),Doc.Button("myBut",[AttrProxy.Create("type","submit")],function()
  {
   subm(Client.createUserValues(degreeVar.c));
  }),(userValues=Client.createUserValues(degreeVar.c),Doc.Async((b=null,Concurrency.Delay(function()
  {
   return Concurrency.Bind((new AjaxRemotingProvider.New()).Async("JobApplicationSpam:JobApplicationSpam.Server.setUserValues:1239847983",[userValues,1]),function(a)
   {
    return a.$==1?Concurrency.Return(Doc.Element("div",[],[Doc.TextNode(Strings.concat(", ",a.$0))])):Concurrency.Return(Doc.Element("div",[],[Doc.TextNode(a.$0)]));
   });
  }))))]);
 };
 Client.createUserValues=function(degree)
 {
  return{
   degree:degree
  };
 };
 Client.createUserValues1=function(gender,degree,firstName,lastName,street,postcode,city,phone,mobilePhone)
 {
  return{
   degree:Var.Create$1(degree),
   firstName:Var.Create$1(firstName),
   lastName:Var.Create$1(lastName),
   street:Var.Create$1(street),
   postcode:Var.Create$1(postcode),
   city:Var.Create$1(city),
   phone:Var.Create$1(phone),
   mobilePhone:Var.Create$1(mobilePhone)
  };
 };
}());
