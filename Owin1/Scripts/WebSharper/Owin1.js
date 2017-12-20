(function()
{
 "use strict";
 var Global,Owin1,Client,WebSharper,UI,Next,Var,Submitter,View,Remoting,AjaxRemotingProvider,Concurrency,Doc,AttrProxy;
 Global=window;
 Owin1=Global.Owin1=Global.Owin1||{};
 Client=Owin1.Client=Owin1.Client||{};
 WebSharper=Global.WebSharper;
 UI=WebSharper&&WebSharper.UI;
 Next=UI&&UI.Next;
 Var=Next&&Next.Var;
 Submitter=Next&&Next.Submitter;
 View=Next&&Next.View;
 Remoting=WebSharper&&WebSharper.Remoting;
 AjaxRemotingProvider=Remoting&&Remoting.AjaxRemotingProvider;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Doc=Next&&Next.Doc;
 AttrProxy=Next&&Next.AttrProxy;
 Client.Main=function()
 {
  var rvInput,submit,vReversed;
  rvInput=Var.Create$1("");
  submit=Submitter.CreateOption(rvInput.v);
  vReversed=View.MapAsync(function(a)
  {
   var b;
   return a!=null&&a.$==1?(new AjaxRemotingProvider.New()).Async("Owin1:Owin1.Server.DoSomething:-1287498065",[a.$0]):(b=null,Concurrency.Delay(function()
   {
    return Concurrency.Return("");
   }));
  },submit.view);
  return Doc.Element("div",[],[Doc.Input([],rvInput),Doc.Button("Send",[],function()
  {
   submit.Trigger();
  }),Doc.Element("hr",[],[]),Doc.Element("h4",[AttrProxy.Create("class","text-muted")],[Doc.TextNode("The server responded:")]),Doc.Element("div",[AttrProxy.Create("class","jumbotron")],[Doc.Element("h1",[],[Doc.TextView(vReversed)])])]);
 };
}());
