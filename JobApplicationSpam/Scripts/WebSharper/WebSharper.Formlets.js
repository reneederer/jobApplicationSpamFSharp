(function()
{
 "use strict";
 var Global,WebSharper,Formlets,Utils,CssConstants,SC$1,Body,Obj,ElementStore,Layout,Padding,LabelConfiguration,FormRowConfiguration,LayoutProvider,Html,Client,Pagelet,Data,Formlet,ValidatorProvidor,FormletBuilder,Formlet$1,SC$2,Controls,Enhance,FormButtonConfiguration,ValidationIconConfiguration,ValidationFrameConfiguration,Padding$1,FormContainerConfiguration,ManyConfiguration,List,Tags,IntelliFactory,Runtime,Collections,Dictionary,Enumerator,Seq,Attr,Operators,Util,Formlets$1,Base,Result,Form,Tree,FormletProvider,Reactive,Reactive$1,LayoutUtils,Validator,HotStream,EventsPervasives,Math,Arrays,JSON;
 Global=window;
 WebSharper=Global.WebSharper=Global.WebSharper||{};
 Formlets=WebSharper.Formlets=WebSharper.Formlets||{};
 Utils=Formlets.Utils=Formlets.Utils||{};
 CssConstants=Formlets.CssConstants=Formlets.CssConstants||{};
 SC$1=Global.StartupCode$WebSharper_Formlets$CssConstants=Global.StartupCode$WebSharper_Formlets$CssConstants||{};
 Body=Formlets.Body=Formlets.Body||{};
 Obj=WebSharper&&WebSharper.Obj;
 ElementStore=Formlets.ElementStore=Formlets.ElementStore||{};
 Layout=Formlets.Layout=Formlets.Layout||{};
 Padding=Layout.Padding=Layout.Padding||{};
 LabelConfiguration=Layout.LabelConfiguration=Layout.LabelConfiguration||{};
 FormRowConfiguration=Layout.FormRowConfiguration=Layout.FormRowConfiguration||{};
 LayoutProvider=Formlets.LayoutProvider=Formlets.LayoutProvider||{};
 Html=WebSharper&&WebSharper.Html;
 Client=Html&&Html.Client;
 Pagelet=Client&&Client.Pagelet;
 Data=Formlets.Data=Formlets.Data||{};
 Formlet=Data.Formlet=Data.Formlet||{};
 ValidatorProvidor=Data.ValidatorProvidor=Data.ValidatorProvidor||{};
 FormletBuilder=Formlets.FormletBuilder=Formlets.FormletBuilder||{};
 Formlet$1=Formlets.Formlet=Formlets.Formlet||{};
 SC$2=Global.StartupCode$WebSharper_Formlets$Formlet=Global.StartupCode$WebSharper_Formlets$Formlet||{};
 Controls=Formlets.Controls=Formlets.Controls||{};
 Enhance=Formlets.Enhance=Formlets.Enhance||{};
 FormButtonConfiguration=Enhance.FormButtonConfiguration=Enhance.FormButtonConfiguration||{};
 ValidationIconConfiguration=Enhance.ValidationIconConfiguration=Enhance.ValidationIconConfiguration||{};
 ValidationFrameConfiguration=Enhance.ValidationFrameConfiguration=Enhance.ValidationFrameConfiguration||{};
 Padding$1=Enhance.Padding=Enhance.Padding||{};
 FormContainerConfiguration=Enhance.FormContainerConfiguration=Enhance.FormContainerConfiguration||{};
 ManyConfiguration=Enhance.ManyConfiguration=Enhance.ManyConfiguration||{};
 List=WebSharper&&WebSharper.List;
 Tags=Client&&Client.Tags;
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 Collections=WebSharper&&WebSharper.Collections;
 Dictionary=Collections&&Collections.Dictionary;
 Enumerator=WebSharper&&WebSharper.Enumerator;
 Seq=WebSharper&&WebSharper.Seq;
 Attr=Client&&Client.Attr;
 Operators=Client&&Client.Operators;
 Util=WebSharper&&WebSharper.Util;
 Formlets$1=IntelliFactory&&IntelliFactory.Formlets;
 Base=Formlets$1&&Formlets$1.Base;
 Result=Base&&Base.Result;
 Form=Base&&Base.Form;
 Tree=Base&&Base.Tree;
 FormletProvider=Base&&Base.FormletProvider;
 Reactive=IntelliFactory&&IntelliFactory.Reactive;
 Reactive$1=Reactive&&Reactive.Reactive;
 LayoutUtils=Base&&Base.LayoutUtils;
 Validator=Base&&Base.Validator;
 HotStream=Reactive&&Reactive.HotStream;
 EventsPervasives=Client&&Client.EventsPervasives;
 Math=Global.Math;
 Arrays=WebSharper&&WebSharper.Arrays;
 JSON=Global.JSON;
 Utils.InTable=function(rows)
 {
  var a,a$1;
  a=[(a$1=List.map(function(cols)
  {
   var a$2;
   a$2=List.map(function(c)
   {
    return Tags.Tags().NewTag("td",[c]);
   },cols);
   return Tags.Tags().NewTag("tr",a$2);
  },rows),Tags.Tags().NewTag("tbody",a$1))];
  return Tags.Tags().NewTag("table",a);
 };
 Utils.MapOption=function(f,value)
 {
  return value!=null&&value.$==1?{
   $:1,
   $0:f(value.$0)
  }:null;
 };
 Utils.Maybe=function(d,f,o)
 {
  return o==null?d:f(o.$0);
 };
 CssConstants.InputTextClass=function()
 {
  SC$1.$cctor();
  return SC$1.InputTextClass;
 };
 SC$1.$cctor=function()
 {
  SC$1.$cctor=Global.ignore;
  SC$1.InputTextClass="inputText";
 };
 Body.New$1=function(el,l)
 {
  return Body.New(el,l);
 };
 Body.New=function(Element,Label)
 {
  return{
   Element:Element,
   Label:Label
  };
 };
 ElementStore=Formlets.ElementStore=Runtime.Class({
  Remove:function(key)
  {
   if(this.store.ContainsKey(key))
    {
     (this.store.get_Item(key))();
     this.store.Remove(key);
    }
  },
  RegisterElement:function(key,f)
  {
   if(!this.store.ContainsKey(key))
    this.store.set_Item(key,f);
  },
  Init:function()
  {
   this.store=new Dictionary.New$5();
  }
 },Obj,ElementStore);
 ElementStore.NewElementStore=function()
 {
  var store;
  store=new ElementStore.New();
  store.Init();
  return store;
 };
 ElementStore.New=Runtime.Ctor(function()
 {
 },ElementStore);
 Padding.get_Default=function()
 {
  return Padding.New(null,null,null,null);
 };
 Padding.New=function(Left,Right,Top,Bottom)
 {
  return{
   Left:Left,
   Right:Right,
   Top:Top,
   Bottom:Bottom
  };
 };
 LabelConfiguration.get_Default=function()
 {
  return LabelConfiguration.New({
   $:0
  },{
   $:1
  },{
   $:0
  });
 };
 LabelConfiguration.New=function(Align,VerticalAlign,Placement)
 {
  return{
   Align:Align,
   VerticalAlign:VerticalAlign,
   Placement:Placement
  };
 };
 FormRowConfiguration.get_Default=function()
 {
  return FormRowConfiguration.New(null,null,null,null,null);
 };
 FormRowConfiguration.New=function(Padding$2,Color,Class,Style,LabelConfiguration$1)
 {
  return{
   Padding:Padding$2,
   Color:Color,
   Class:Class,
   Style:Style,
   LabelConfiguration:LabelConfiguration$1
  };
 };
 LayoutProvider=Formlets.LayoutProvider=Runtime.Class({
  get_Horizontal:function()
  {
   return this.ColumnLayout(FormRowConfiguration.get_Default());
  },
  get_Vertical:function()
  {
   return this.RowLayout(FormRowConfiguration.get_Default());
  },
  LabelLayout:function(lc)
  {
   var i;
   return this.RowLayout((i=FormRowConfiguration.get_Default(),FormRowConfiguration.New(i.Padding,i.Color,i.Class,i.Style,{
    $:1,
    $0:lc
   })));
  },
  get_Flowlet:function()
  {
   return this.MakeLayout(function()
   {
    var panel;
    function insert(a,bd)
    {
     var nextScreen;
     nextScreen=Utils.InTable(List.ofArray([List.ofArray([bd.Label!=null?bd.Label.$0():Tags.Tags().NewTag("span",[]),Tags.Tags().NewTag("div",[bd.Element])])]));
     panel.HtmlProvider.Clear(panel.get_Body());
     panel.AppendI(nextScreen);
     return List.ofArray([nextScreen]);
    }
    panel=Tags.Tags().NewTag("div",[]);
    return{
     Insert:function($1)
     {
      return function($2)
      {
       return insert($1,$2);
      };
     },
     Panel:panel
    };
   });
  },
  ColumnLayout:function(rowConfig)
  {
   var $this;
   $this=this;
   return this.LayoutUtils.New(function()
   {
    var row,container,a,store;
    function insert(rowIx,body)
    {
     var elemId,newCol,a$1,a$2,a$3,jqPanel,index,inserted;
     elemId=body.Element.get_Id();
     newCol=(a$1=[(a$2=[(a$3=[$this.MakeRow(rowConfig,rowIx,body)],Tags.Tags().NewTag("tbody",a$3))],Tags.Tags().NewTag("table",a$2))],Tags.Tags().NewTag("td",a$1));
     jqPanel=Global.jQuery(row.get_Body());
     index=[0];
     inserted=[false];
     jqPanel.children().each(function(a$4,el)
     {
      var jqCol;
      jqCol=Global.jQuery(el);
      rowIx===index[0]?(Global.jQuery(newCol.get_Body()).insertBefore(jqCol),newCol.Render(),inserted[0]=true):void 0;
      index[0]++;
     });
     !inserted[0]?row.AppendI(newCol):void 0;
     return store.RegisterElement(elemId,function()
     {
      newCol.HtmlProvider.Remove(newCol.get_Body());
     });
    }
    row=Tags.Tags().NewTag("tr",[]);
    container=(a=[Tags.Tags().NewTag("tbody",[row])],Tags.Tags().NewTag("table",a));
    store=ElementStore.NewElementStore();
    return{
     Body:Body.New(container,null),
     SyncRoot:null,
     Insert:function($1)
     {
      return function($2)
      {
       return insert($1,$2);
      };
     },
     Remove:function(elems)
     {
      var e;
      e=Enumerator.Get(elems);
      try
      {
       while(e.MoveNext())
        store.Remove(e.Current().Element.get_Id());
      }
      finally
      {
       if("Dispose"in e)
        e.Dispose();
      }
     }
    };
   });
  },
  RowLayout:function(rowConfig)
  {
   var $this;
   $this=this;
   return this.LayoutUtils.New(function()
   {
    var panel,container,store;
    function insert(rowIx,body)
    {
     var elemId,row,jqPanel,index,inserted;
     elemId=body.Element.get_Id();
     row=$this.MakeRow(rowConfig,rowIx,body);
     jqPanel=Global.jQuery(panel.get_Body());
     index=[0];
     inserted=[false];
     jqPanel.children().each(function(a,el)
     {
      var jqRow;
      jqRow=Global.jQuery(el);
      rowIx===index[0]?(Global.jQuery(row.get_Body()).insertBefore(jqRow),row.Render(),inserted[0]=true):void 0;
      index[0]++;
     });
     !inserted[0]?panel.AppendI(row):void 0;
     return store.RegisterElement(elemId,function()
     {
      row.HtmlProvider.Remove(row.get_Body());
     });
    }
    panel=Tags.Tags().NewTag("tbody",[]);
    container=Tags.Tags().NewTag("table",[panel]);
    store=ElementStore.NewElementStore();
    return{
     Body:Body.New(container,null),
     SyncRoot:null,
     Insert:function($1)
     {
      return function($2)
      {
       return insert($1,$2);
      };
     },
     Remove:function(elems)
     {
      var e;
      e=Enumerator.Get(elems);
      try
      {
       while(e.MoveNext())
        store.Remove(e.Current().Element.get_Id());
      }
      finally
      {
       if("Dispose"in e)
        e.Dispose();
      }
     }
    };
   });
  },
  MakeLayout:function(lm)
  {
   return this.LayoutUtils.New(function()
   {
    var lm$1,store;
    function insert(ix,bd)
    {
     var elemId,newElems;
     elemId=bd.Element.get_Id();
     newElems=(lm$1.Insert(ix))(bd);
     return store.RegisterElement(elemId,function()
     {
      var e,_this;
      e=Enumerator.Get(newElems);
      try
      {
       while(e.MoveNext())
        {
         _this=e.Current();
         _this.HtmlProvider.Remove(_this.get_Body());
        }
      }
      finally
      {
       if("Dispose"in e)
        e.Dispose();
      }
     });
    }
    lm$1=lm();
    store=ElementStore.NewElementStore();
    return{
     Body:Body.New(lm$1.Panel,null),
     SyncRoot:null,
     Insert:function($1)
     {
      return function($2)
      {
       return insert($1,$2);
      };
     },
     Remove:function(elems)
     {
      var e;
      e=Enumerator.Get(elems);
      try
      {
       while(e.MoveNext())
        store.Remove(e.Current().Element.get_Id());
      }
      finally
      {
       if("Dispose"in e)
        e.Dispose();
      }
     }
    };
   });
  },
  MakeRow:function(rowConfig,rowIndex,body)
  {
   var padding,paddingLeft,paddingTop,paddingRight,paddingBottom,elem,cells,m,labelConf,label,m$1,rowClass,rowStyle,m$2,a,x;
   function makeCell(l,t,r,b,csp,valign,elem$1)
   {
    var paddingStyle,valignStyle,x$1;
    function m$3(k,v)
    {
     return k+Global.String(v)+"px;";
    }
    paddingStyle=Seq.reduce(function(x$2,y)
    {
     return x$2+y;
    },List.map(function($1)
    {
     return m$3($1[0],$1[1]);
    },List.ofArray([["padding-left: ",l],["padding-top: ",t],["padding-right: ",r],["padding-bottom: ",b]])));
    valignStyle=Utils.Maybe("",function(valign$1)
    {
     return"vertical-align: "+(valign$1.$==1?"middle":valign$1.$==2?"bottom":"top")+";";
    },valign);
    x$1=List.append(new List.T({
     $:1,
     $0:Attr.Attr().NewAttr("style",paddingStyle+";"+valignStyle),
     $1:csp?List.ofArray([Attr.Attr().NewAttr("colspan","2")]):List.T.Empty
    }),List.ofArray([elem$1]));
    return Tags.Tags().NewTag("td",x$1);
   }
   padding=Utils.Maybe(Padding.get_Default(),Global.id,rowConfig.Padding);
   paddingLeft=Utils.Maybe(0,Global.id,padding.Left);
   paddingTop=Utils.Maybe(0,Global.id,padding.Top);
   paddingRight=Utils.Maybe(0,Global.id,padding.Right);
   paddingBottom=Utils.Maybe(0,Global.id,padding.Bottom);
   elem=body.Element;
   cells=(m=body.Label,m!=null&&m.$==1?(labelConf=Utils.Maybe(LabelConfiguration.get_Default(),Global.id,rowConfig.LabelConfiguration),(label=this.HorizontalAlignElem(labelConf.Align,m.$0()),(m$1=labelConf.Placement,m$1.$==3?List.ofArray([makeCell(paddingLeft,paddingTop,paddingRight,paddingBottom,true,null,Utils.InTable(List.ofArray([List.ofArray([elem]),List.ofArray([label])])))]):m$1.$==0?List.ofArray([makeCell(paddingLeft,paddingTop,0,paddingBottom,false,{
    $:1,
    $0:labelConf.VerticalAlign
   },label),makeCell(0,paddingTop,paddingRight,paddingBottom,false,null,elem)]):m$1.$==1?List.ofArray([makeCell(paddingLeft,paddingTop,0,paddingBottom,false,{
    $:1,
    $0:labelConf.VerticalAlign
   },elem),makeCell(0,paddingTop,paddingRight,paddingBottom,false,null,label)]):List.ofArray([makeCell(paddingLeft,paddingTop,paddingRight,paddingBottom,true,null,Utils.InTable(List.ofArray([List.ofArray([label]),List.ofArray([elem])])))])))):List.ofArray([makeCell(paddingLeft,paddingTop,paddingRight,paddingBottom,true,null,elem)]));
   rowClass=Utils.Maybe(List.T.Empty,function(classGen)
   {
    var a$1;
    return List.ofArray([(a$1=classGen(rowIndex),Attr.Attr().NewAttr("class",a$1))]);
   },rowConfig.Class);
   rowStyle=(m$2=List.append(Utils.Maybe(List.T.Empty,function(colGen)
   {
    return List.ofArray(["background-color: "+colGen(rowIndex)]);
   },rowConfig.Color),Utils.Maybe(List.T.Empty,function(styleGen)
   {
    return List.ofArray([styleGen(rowIndex)]);
   },rowConfig.Style)),m$2.$==0?List.T.Empty:List.ofArray([(a=Seq.reduce(function($1,$2)
   {
    return $1+";"+$2;
   },m$2),Attr.Attr().NewAttr("style",a))]));
   x=List.append(rowClass,List.append(rowStyle,List.append(rowStyle,cells)));
   return Tags.Tags().NewTag("tr",x);
  },
  VerticalAlignedTD:function(valign,elem)
  {
   var cell;
   cell=Tags.Tags().NewTag("td",[elem]);
   cell.HtmlProvider.SetCss(cell.get_Body(),"vertical-align",valign.$==1?"middle":valign.$==2?"bottom":"top");
   return cell;
  },
  HorizontalAlignElem:function(align,el)
  {
   var a;
   return Operators.add((a=[Attr.Attr().NewAttr("style","float:"+(align.$==0?"left":"right")+";")],Tags.Tags().NewTag("div",a)),[el]);
  }
 },Obj,LayoutProvider);
 LayoutProvider.New=Runtime.Ctor(function(LayoutUtils$1)
 {
  this.LayoutUtils=LayoutUtils$1;
 },LayoutProvider);
 Formlet=Data.Formlet=Runtime.Class({
  Run:function(f)
  {
   var m,formlet,form,el,m$1;
   m=this.get_ElementInternal();
   return m==null?(formlet=this.formletBase.ApplyLayout(this),(form=formlet.BuildI(),(form.State.Subscribe(Util.observer(function(res)
   {
    Result.Map(f,res);
   })),el=(m$1=formlet.LayoutI().Apply(form.Body),m$1==null?Data.DefaultLayout().Apply(form.Body).$0[0].Element:m$1.$0[0].Element),this.set_ElementInternal({
    $:1,
    $0:el
   }),el))):m.$0;
  },
  set_ElementInternal:function(v)
  {
   this.ElementInternal=v;
  },
  get_ElementInternal:function()
  {
   return this.ElementInternal;
  },
  Render:function()
  {
   this.Run(Global.ignore).Render();
  },
  get_Body:function()
  {
   return this.Run(Global.ignore).get_Body();
  },
  MapResultI:function(f)
  {
   var $this;
   $this=this;
   return new Formlet.New(function()
   {
    var form;
    form=$this.buildInternal();
    return Form.New(form.Body,form.Dispose$1,form.Notify,$this.utils.Reactive.Select(form.State,f));
   },this.layoutInternal,this.formletBase,this.utils);
  },
  LayoutI:function()
  {
   return this.layoutInternal;
  },
  BuildI:function()
  {
   return this.buildInternal();
  }
 },Pagelet,Formlet);
 Formlet.New=Runtime.Ctor(function(buildInternal,layoutInternal,formletBase,utils)
 {
  Pagelet.New.call(this);
  this.buildInternal=buildInternal;
  this.layoutInternal=layoutInternal;
  this.formletBase=formletBase;
  this.utils=utils;
  this.ElementInternal=null;
 },Formlet);
 ValidatorProvidor=Data.ValidatorProvidor=Runtime.Class({
  Matches:function(regex,text)
  {
   return text.match(new Global.RegExp(regex));
  }
 },Obj,ValidatorProvidor);
 ValidatorProvidor.New=Runtime.Ctor(function()
 {
 },ValidatorProvidor);
 Data.$=function(f,x)
 {
  return Data.OfIFormlet(Data.BaseFormlet().Apply(f,x));
 };
 Data.Validator=function()
 {
  SC$2.$cctor();
  return SC$2.Validator;
 };
 Data.MkFormlet=function(f)
 {
  return Data.OfIFormlet(Data.BaseFormlet().New(function()
  {
   var p,reset,a;
   p=f();
   reset=p[1];
   return Form.New((a=Tree.Set(Data.NewBody(p[0],null)),Data.RX().Return(a)),Global.ignore,function()
   {
    reset();
   },p[2]);
  }));
 };
 Data.OfIFormlet=function(formlet)
 {
  return Data.PropagateRenderFrom(formlet,new Formlet.New(function()
  {
   return formlet.BuildI();
  },formlet.LayoutI(),Data.BaseFormlet(),Data.UtilsProvider()));
 };
 Data.PropagateRenderFrom=function(f1,f2)
 {
  f1.hasOwnProperty("Render")?f2.Render=f1.Render:void 0;
  return f2;
 };
 Data.BaseFormlet=function()
 {
  return new FormletProvider.New(Data.UtilsProvider());
 };
 Data.UtilsProvider=function()
 {
  return{
   Reactive:Data.RX(),
   DefaultLayout:Data.DefaultLayout()
  };
 };
 Data.DefaultLayout=function()
 {
  SC$2.$cctor();
  return SC$2.DefaultLayout;
 };
 Data.set_DefaultLayout=function($1)
 {
  SC$2.$cctor();
  SC$2.DefaultLayout=$1;
 };
 Data.Layout=function()
 {
  SC$2.$cctor();
  return SC$2.Layout;
 };
 Data.RX=function()
 {
  SC$2.$cctor();
  return SC$2.RX;
 };
 Data.NewBody=function(a,a$1)
 {
  return Body.New$1(a,a$1);
 };
 FormletBuilder=Formlets.FormletBuilder=Runtime.Class({
  ReturnFrom:function(f)
  {
   return Data.OfIFormlet(f);
  },
  Delay:function(f)
  {
   return Data.OfIFormlet(Data.BaseFormlet().Delay(f));
  },
  Bind:function(formlet,f)
  {
   return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().Bind(formlet,f)));
  },
  Return:function(x)
  {
   return Data.OfIFormlet(Data.BaseFormlet().Return(x));
  }
 },Obj,FormletBuilder);
 FormletBuilder.New=Runtime.Ctor(function()
 {
 },FormletBuilder);
 Formlet$1.Choose=function(fs)
 {
  var count,a;
  count=[0];
  return Formlet$1.Map(function(x)
  {
   return x.$0;
  },(a=Formlet$1.Map(function(xs)
  {
   function c(x,a$1)
   {
    return{
     $:1,
     $0:x
    };
   }
   function p(a$1,ix)
   {
    return ix;
   }
   return Seq.tryPick(function($1)
   {
    return c($1[0],$1[1]);
   },List.rev(List.sortBy(function($1)
   {
    return p($1[0],$1[1]);
   },List.choose(function(x)
   {
    return x.$==0?{
     $:1,
     $0:x.$0
    }:null;
   },xs))));
  },Formlet$1.Sequence(Seq.map(function(f)
  {
   return Formlet$1.LiftResult(Formlet$1.InitWithFailure(Formlet$1.Map(function(x)
   {
    count[0]++;
    return[x,count[0]];
   },f)));
  },fs))),Data.Validator().Is(function(x)
  {
   return x!=null;
  },"",a)));
 };
 Formlet$1.Render=function(formlet)
 {
  return Data.PropagateRenderFrom(formlet,formlet.Run(Global.ignore));
 };
 Formlet$1.BindWith=function(compose,formlet,f)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().BindWith(compose,formlet,f)));
 };
 Formlet$1.Run=function(f,formlet)
 {
  return formlet.Run(f);
 };
 Formlet$1.WithLabel=function(label,formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().MapBody(function(body)
  {
   return Body.New(body.Element,label);
  },formlet)));
 };
 Formlet$1.OfElement=function(genElem)
 {
  return Data.MkFormlet(function()
  {
   return[genElem(),Global.ignore,Data.RX().Return({
    $:0,
    $0:null
   })];
  });
 };
 Formlet$1.MapElement=function(f,formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().MapBody(function(b)
  {
   return Body.New(f(b.Element),b.Label);
  },formlet)));
 };
 Formlet$1.ApplyLayout=function(formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().ApplyLayout(formlet)));
 };
 Formlet$1.WithNotificationChannel=function(formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().WithNotificationChannel(formlet)));
 };
 Formlet$1.WithNotification=function(c,formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().WithNotification(c,formlet)));
 };
 Formlet$1.WithLayout=function(l,formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().WithLayout(l,formlet)));
 };
 Formlet$1.Sequence=function(fs)
 {
  return Data.OfIFormlet(Data.BaseFormlet().Sequence(Seq.map(Global.id,fs)));
 };
 Formlet$1.LiftResult=function(formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().LiftResult(formlet)));
 };
 Formlet$1.SelectMany=function(formlet)
 {
  var x;
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,(x=Formlet$1.Map(Global.id,formlet),Data.BaseFormlet().SelectMany(x))));
 };
 Formlet$1.FlipBody=function(formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().FlipBody(formlet)));
 };
 Formlet$1.Switch=function(formlet)
 {
  var x;
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,(x=Formlet$1.Map(Global.id,formlet),Data.BaseFormlet().Switch(x))));
 };
 Formlet$1.Join=function(formlet)
 {
  var x;
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,(x=Formlet$1.Map(Global.id,formlet),Data.BaseFormlet().Join(x))));
 };
 Formlet$1.Replace=function(formlet,f)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().Replace(formlet,f)));
 };
 Formlet$1.Bind=function(fl,f)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(fl,Data.BaseFormlet().Bind(fl,f)));
 };
 Formlet$1.Delay=function(f)
 {
  return Data.OfIFormlet(Data.BaseFormlet().Delay(f));
 };
 Formlet$1.MapResult=function(f,formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().MapResult(f,formlet)));
 };
 Formlet$1.MapBody=function(f,formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().MapBody(f,formlet)));
 };
 Formlet$1.Map=function(f,formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().Map(f,formlet)));
 };
 Formlet$1.Do=function()
 {
  SC$2.$cctor();
  return SC$2.Do;
 };
 Formlet$1.FailWith=function(fs)
 {
  return Data.OfIFormlet(Data.BaseFormlet().FailWith(fs));
 };
 Formlet$1.Deletable=function(formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().Deletable(formlet)));
 };
 Formlet$1.BuildForm=function(f)
 {
  return Data.BaseFormlet().BuildForm(f);
 };
 Formlet$1.ReturnEmpty=function(x)
 {
  return Data.OfIFormlet(Data.BaseFormlet().ReturnEmpty(x));
 };
 Formlet$1.Empty=function()
 {
  return Data.OfIFormlet(Data.BaseFormlet().Empty());
 };
 Formlet$1.Never=function()
 {
  return Data.OfIFormlet(Data.BaseFormlet().Never());
 };
 Formlet$1.ReplaceFirstWithFailure=function(formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().ReplaceFirstWithFailure(formlet)));
 };
 Formlet$1.Flowlet=function(formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().WithLayout(Data.Layout().get_Flowlet(),formlet)));
 };
 Formlet$1.Vertical=function(formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().WithLayout(Data.Layout().get_Vertical(),formlet)));
 };
 Formlet$1.Horizontal=function(formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().WithLayout(Data.Layout().get_Horizontal(),formlet)));
 };
 Formlet$1.InitWithFailure=function(formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().InitWithFailure(formlet)));
 };
 Formlet$1.InitWith=function(value,formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().InitWith(value,formlet)));
 };
 Formlet$1.WithCancelation=function(formlet,c)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().WithCancelation(formlet,c)));
 };
 Formlet$1.Return=function(x)
 {
  return Data.OfIFormlet(Data.BaseFormlet().Return(x));
 };
 Formlet$1.WithLayoutOrDefault=function(formlet)
 {
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Data.BaseFormlet().WithLayoutOrDefault(formlet)));
 };
 Formlet$1.New=function(f)
 {
  return Data.OfIFormlet(Data.BaseFormlet().New(f));
 };
 Formlet$1.BuildFormlet=function(f)
 {
  return Data.MkFormlet(f);
 };
 SC$2.$cctor=function()
 {
  SC$2.$cctor=Global.ignore;
  SC$2.RX=Reactive$1.Default();
  SC$2.Layout=new LayoutProvider.New(new LayoutUtils.New({
   Reactive:Reactive$1.Default()
  }));
  SC$2.DefaultLayout=Data.Layout().get_Vertical();
  SC$2.Validator=new Validator.New(new ValidatorProvidor.New());
  SC$2.Do=new FormletBuilder.New();
 };
 Controls.Button=function(label)
 {
  return Controls.ElementButton(function()
  {
   var a;
   a=[Tags.Tags().text(label)];
   return Tags.Tags().NewTag("button",a);
  });
 };
 Controls.ElementButton=function(genElem)
 {
  return Data.MkFormlet(function()
  {
   var state,count,x;
   function a(a$1,a$2)
   {
    state.Trigger({
     $:0,
     $0:count[0]
    });
    count[0]++;
   }
   state=HotStream.New$1({
    $:1,
    $0:List.T.Empty
   });
   count=[0];
   return[(x=genElem(),(function(a$1)
   {
    EventsPervasives.Events().OnClick(function($1)
    {
     return function($2)
     {
      return a($1,$2);
     };
    },a$1);
   }(x),x)),function()
   {
    count[0]=0;
    state.Trigger({
     $:1,
     $0:List.T.Empty
    });
   },state];
  });
 };
 Controls.ReadOnlyInput=function(value)
 {
  return Controls.InputField(true,"text",CssConstants.InputTextClass(),value);
 };
 Controls.Input=function(value)
 {
  return Controls.InputField(false,"text",CssConstants.InputTextClass(),value);
 };
 Controls.Password=function(value)
 {
  return Controls.InputField(false,"password","inputPassword",value);
 };
 Controls.ReadOnlyRadioButtonGroup=function(def,values)
 {
  return Controls.RadioButtonGroupControl(true,def,values);
 };
 Controls.RadioButtonGroup=function(def,values)
 {
  return Controls.RadioButtonGroupControl(false,def,values);
 };
 Controls.RadioButtonGroupControl=function(readOnly,def,values)
 {
  return Formlet$1.New(function()
  {
   var groupId,state,x,rbLbVls,a;
   function c(ix,value)
   {
    return def==null?null:def.$0===ix?{
     $:1,
     $0:{
      $:0,
      $0:value
     }
    }:null;
   }
   function m(label,value)
   {
    var a$1;
    return[Operators.add((a$1=[Attr.Attr().NewAttr("class","inputRadio"),Attr.Attr().NewAttr("type","radio"),Attr.Attr().NewAttr("name",groupId)],Tags.Tags().NewTag("input",a$1)),readOnly?List.ofArray([Attr.Attr().NewAttr("disabled","disabled")]):List.T.Empty),label,value];
   }
   function resetRB(rb,value,ix)
   {
    return def==null?(rb.HtmlProvider.RemoveAttribute(rb.get_Body(),"checked"),state.Trigger({
     $:1,
     $0:List.T.Empty
    })):def.$0===ix?(rb.HtmlProvider.SetProperty(rb.get_Body(),"checked",true),state.Trigger({
     $:0,
     $0:value
    })):rb.HtmlProvider.SetProperty(rb.get_Body(),"checked",false);
   }
   function reset()
   {
    List.iteri(function(ix,t)
    {
     return resetRB(t[0],t[2],ix);
    },rbLbVls);
   }
   groupId="id"+Math.round(Math.random()*100000000);
   state=(x=def==null?null:Seq.tryPick(function($1)
   {
    return c($1[0],$1[1]);
   },List.mapi(function(ix,t)
   {
    return[ix,t[1]];
   },values)),Utils.Maybe(HotStream.New$1({
    $:1,
    $0:List.T.Empty
   }),HotStream.New$1,x));
   rbLbVls=List.map(function($1)
   {
    return m($1[0],$1[1]);
   },values);
   return Form.New((a=new Tree.Edit({
    $:0,
    $0:Tree.FromSequence(List.mapi(function(ix,t)
    {
     var rb,label,value;
     function a$1(a$2,a$3)
     {
      return!readOnly?state.Trigger({
       $:0,
       $0:value
      }):null;
     }
     rb=t[0];
     label=t[1];
     value=t[2];
     resetRB(rb,value,ix);
     EventsPervasives.Events().OnClick(function($1)
     {
      return function($2)
      {
       return a$1($1,$2);
      };
     },rb);
     return Body.New(rb,{
      $:1,
      $0:function()
      {
       var a$2;
       a$2=[Tags.Tags().text(label)];
       return Tags.Tags().NewTag("label",a$2);
      }
     });
    },rbLbVls))
   }),Data.RX().Return(a)),Global.ignore,function()
   {
    reset();
   },state);
  });
 };
 Controls.CheckboxGroup=function(values)
 {
  return Controls.CheckboxGroupControl(false,values);
 };
 Controls.CheckboxGroupControl=function(readOnly,values)
 {
  function c(b,v)
  {
   return b?{
    $:1,
    $0:v
   }:null;
  }
  function m(l,v,b)
  {
   return Formlet$1.Map(function(b$1)
   {
    return[b$1,v];
   },Formlet$1.WithLabel({
    $:1,
    $0:function()
    {
     var a;
     a=[Tags.Tags().text(l)];
     return Tags.Tags().NewTag("label",a);
    }
   },Controls.CheckboxControl(readOnly,b)));
  }
  return Formlet$1.Map(function(l)
  {
   return List.choose(function($1)
   {
    return c($1[0],$1[1]);
   },l);
  },Formlet$1.Sequence(List.map(function($1)
  {
   return m($1[0],$1[1],$1[2]);
  },values)));
 };
 Controls.ReadOnlyCheckbox=function(def)
 {
  return Controls.CheckboxControl(true,def);
 };
 Controls.Checkbox=function(def)
 {
  return Controls.CheckboxControl(false,def);
 };
 Controls.CheckboxControl=function(readOnly,def)
 {
  return Data.MkFormlet(function()
  {
   var state,body,readOnlyAtts,x,a;
   function a$1(cb,a$2)
   {
    return!readOnly?state.Trigger({
     $:0,
     $0:Global.jQuery(cb.get_Body()).prop("checked")
    }):null;
   }
   function reset()
   {
    def?body.HtmlProvider.SetProperty(body.get_Body(),"checked",true):(body.HtmlProvider.RemoveAttribute(body.get_Body(),"checked"),body.HtmlProvider.SetProperty(body.get_Body(),"checked",false));
    state.Trigger({
     $:0,
     $0:def
    });
   }
   state=HotStream.New$1({
    $:0,
    $0:def
   });
   body=(readOnlyAtts=readOnly?List.ofArray([Attr.Attr().NewAttr("disabled","disabled")]):List.T.Empty,(x=Operators.add((a=[Attr.Attr().NewAttr("type","checkbox"),Attr.Attr().NewAttr("class","inputCheckbox")],Tags.Tags().NewTag("input",a)),readOnlyAtts),(function(a$2)
   {
    EventsPervasives.Events().OnClick(function($1)
    {
     return function($2)
     {
      return a$1($1,$2);
     };
    },a$2);
   }(x),x)));
   def?body.HtmlProvider.SetAttribute(body.get_Body(),"defaultChecked","true"):body.HtmlProvider.RemoveAttribute(body.get_Body(),"checked");
   reset();
   return[body,reset,state];
  });
 };
 Controls.InputField=function(readOnly,typ,cls,value)
 {
  return Controls.InputControl(value,function(state)
  {
   var input,ro,x;
   function f()
   {
    if(!readOnly)
     state.Trigger({
      $:0,
      $0:input.get_Value()
     });
   }
   input=(ro=readOnly?List.ofArray([Attr.Attr().NewAttr("readonly","readonly")]):List.T.Empty,(x=List.append(List.ofArray([Attr.Attr().NewAttr("type",typ),Attr.Attr().NewAttr("class",cls)]),ro),Tags.Tags().NewTag("input",x)));
   (function(c)
   {
    Controls.OnTextChange(f,c);
   }(input));
   return input;
  });
 };
 Controls.ReadOnlyTextArea=function(value)
 {
  return Controls.TextAreaControl(true,value);
 };
 Controls.TextArea=function(value)
 {
  return Controls.TextAreaControl(false,value);
 };
 Controls.TextAreaControl=function(readOnly,value)
 {
  return Controls.InputControl(value,function(state)
  {
   var input,x;
   function f()
   {
    if(!readOnly)
     state.Trigger({
      $:0,
      $0:input.get_Value()
     });
   }
   input=(x=readOnly?List.ofArray([Attr.Attr().NewAttr("readonly","readonly")]):List.T.Empty,Tags.Tags().NewTag("textarea",x));
   (function(c)
   {
    Controls.OnTextChange(f,c);
   }(input));
   return input;
  });
 };
 Controls.OnTextChange=function(f,control)
 {
  var value;
  function up()
  {
   if(control.get_Value()!==value[0])
    {
     value[0]=control.get_Value();
     f();
    }
  }
  function x(a$2)
  {
   up();
  }
  function a(el,a$2)
  {
   return x(el);
  }
  function a$1(a$2,a$3)
  {
   return up();
  }
  value=[control.get_Value()];
  EventsPervasives.Events().OnChange(function($1)
  {
   return function($2)
   {
    return a($1,$2);
   };
  },control);
  EventsPervasives.Events().OnKeyUp(function($1)
  {
   return function($2)
   {
    return a$1($1,$2);
   };
  },control);
  control.Dom.oninput=up;
 };
 Controls.InputControl=function(value,f)
 {
  return Data.MkFormlet(function()
  {
   var state,body;
   state=HotStream.New$1({
    $:0,
    $0:value
   });
   body=f(state);
   body.set_Value(value);
   return[body,function()
   {
    body.set_Value(value);
    state.Trigger({
     $:0,
     $0:value
    });
   },state];
  });
 };
 Controls.ReadOnlySelect=function(def,vls)
 {
  return Controls.SelectControl(true,def,vls);
 };
 Controls.Select=function(def,vls)
 {
  return Controls.SelectControl(false,def,vls);
 };
 Controls.SelectControl=function(readOnly,def,vls)
 {
  return Data.MkFormlet(function()
  {
   var aVls,sIx,body,x,sValue,state;
   function reset()
   {
    body.HtmlProvider.SetProperty(body.get_Body(),"value",Global.String(sIx));
    state.Trigger(sValue);
   }
   function x$1(a$1)
   {
    if(!readOnly)
     state.Trigger({
      $:0,
      $0:Arrays.get(aVls,Global.Number(body.get_Value()))
     });
   }
   function a(el,a$1)
   {
    return x$1(el);
   }
   aVls=Arrays.ofList(List.map(function(t)
   {
    return t[1];
   },vls));
   sIx=def>=0&&def<vls.get_Length()?def:0;
   body=readOnly?Operators.add((x=List.mapi(function(i,t)
   {
    return Tags.Tags().NewTag("option",[Tags.Tags().text(t[0]),Attr.Attr().NewAttr("value",Global.String(i))]);
   },vls),Tags.Tags().NewTag("select",x)),[Attr.Attr().NewAttr("disabled","disabled")]):(x=List.mapi(function(i,t)
   {
    return Tags.Tags().NewTag("option",[Tags.Tags().text(t[0]),Attr.Attr().NewAttr("value",Global.String(i))]);
   },vls),Tags.Tags().NewTag("select",x));
   sValue={
    $:0,
    $0:Arrays.get(aVls,sIx)
   };
   state=HotStream.New$1(sValue);
   reset();
   EventsPervasives.Events().OnChange(function($1)
   {
    return function($2)
    {
     return a($1,$2);
    };
   },body);
   reset();
   return[body,reset,state];
  });
 };
 FormButtonConfiguration.get_Default=function()
 {
  return FormButtonConfiguration.New(null,null,null);
 };
 FormButtonConfiguration.New=function(Label,Style,Class)
 {
  return{
   Label:Label,
   Style:Style,
   Class:Class
  };
 };
 ValidationIconConfiguration.get_Default=function()
 {
  return ValidationIconConfiguration.New("validIcon","errorIcon");
 };
 ValidationIconConfiguration.New=function(ValidIconClass,ErrorIconClass)
 {
  return{
   ValidIconClass:ValidIconClass,
   ErrorIconClass:ErrorIconClass
  };
 };
 ValidationFrameConfiguration.get_Default=function()
 {
  return ValidationFrameConfiguration.New({
   $:1,
   $0:"successFormlet"
  },null,{
   $:1,
   $0:"errorFormlet"
  },null);
 };
 ValidationFrameConfiguration.New=function(ValidClass,ValidStyle,ErrorClass,ErrorStyle)
 {
  return{
   ValidClass:ValidClass,
   ValidStyle:ValidStyle,
   ErrorClass:ErrorClass,
   ErrorStyle:ErrorStyle
  };
 };
 Padding$1.get_Default=function()
 {
  return Padding$1.New(null,null,null,null);
 };
 Padding$1.New=function(Left,Right,Top,Bottom)
 {
  return{
   Left:Left,
   Right:Right,
   Top:Top,
   Bottom:Bottom
  };
 };
 FormContainerConfiguration.get_Default=function()
 {
  return FormContainerConfiguration.New(null,Padding$1.get_Default(),null,null,null,null,null);
 };
 FormContainerConfiguration.New=function(Header,Padding$2,Description,BackgroundColor,BorderColor,CssClass,Style)
 {
  return{
   Header:Header,
   Padding:Padding$2,
   Description:Description,
   BackgroundColor:BackgroundColor,
   BorderColor:BorderColor,
   CssClass:CssClass,
   Style:Style
  };
 };
 ManyConfiguration.get_Default=function()
 {
  return ManyConfiguration.New("addIcon","removeIcon");
 };
 ManyConfiguration.New=function(AddIconClass,RemoveIconClass)
 {
  return{
   AddIconClass:AddIconClass,
   RemoveIconClass:RemoveIconClass
  };
 };
 Enhance.WithJsonPost=function(conf,formlet)
 {
  var postUrl,m,enc,m$1,hiddenField,submitButton,a,x,a$1;
  function f(form)
  {
   var m$2;
   m$2=conf.EncodingType;
   m$2==null?void 0:m$2.$0==="multipart/form-data"?Global.jQuery(form.get_Body()).attr("encoding","multipart/form-data"):void 0;
  }
  postUrl=(m=conf.PostUrl,m==null?List.T.Empty:List.ofArray([Attr.Attr().NewAttr("action",m.$0)]));
  enc=(m$1=conf.EncodingType,m$1==null?List.T.Empty:List.ofArray([Attr.Attr().NewAttr("enctype",m$1.$0)]));
  hiddenField=Tags.Tags().NewTag("input",[Attr.Attr().NewAttr("type","hidden"),Attr.Attr().NewAttr("name",conf.ParameterName)]);
  submitButton=Tags.Tags().NewTag("input",[Attr.Attr().NewAttr("type","submit"),Attr.Attr().NewAttr("value","Submit")]);
  a=[(x=Operators.add((a$1=List.append(new List.T({
   $:1,
   $0:Attr.Attr().NewAttr("method","POST"),
   $1:new List.T({
    $:1,
    $0:Attr.Attr().NewAttr("style","display:none"),
    $1:postUrl
   })
  }),enc),Tags.Tags().NewTag("form",a$1)),[hiddenField,submitButton]),(function(w)
  {
   Operators.OnAfterRender(f,w);
  }(x),x)),Formlet$1.Map(function(value)
  {
   var data;
   data=JSON.stringify(value);
   Global.jQuery(hiddenField.get_Body()).val(data);
   return Global.jQuery(submitButton.get_Body()).click();
  },formlet)];
  return Tags.Tags().NewTag("div",a);
 };
 Enhance.Many=function(formlet)
 {
  return Enhance.CustomMany(ManyConfiguration.get_Default(),formlet);
 };
 Enhance.CustomMany=function(config,formlet)
 {
  var addButton,delF,x,resetS,resetF,b;
  function manyF()
  {
   return Formlet$1.ApplyLayout(Formlet$1.WithLayoutOrDefault(Formlet$1.Map(List.ofSeq,Enhance.Many_(addButton,function()
   {
    return delF;
   }))));
  }
  addButton=Formlet$1.InitWith(1,Controls.ElementButton(function()
  {
   var a;
   return Operators.add((a=[Attr.Attr().NewAttr("class",config.AddIconClass)],Tags.Tags().NewTag("div",a)),[Tags.Tags().NewTag("div",[])]);
  }));
  delF=Enhance.Deletable((x=Formlet$1.WithCancelation(formlet,Formlet$1.Map(Global.ignore,Controls.ElementButton(function()
  {
   var a;
   return Operators.add((a=[Attr.Attr().NewAttr("class",config.RemoveIconClass)],Tags.Tags().NewTag("div",a)),[Tags.Tags().NewTag("div",[])]);
  }))),Formlet$1.WithLayout(Data.Layout().get_Horizontal(),x)));
  resetS=HotStream.New$1({
   $:0,
   $0:null
  });
  resetF=Data.OfIFormlet(Data.BaseFormlet().FromState(resetS));
  return Formlet$1.WithNotification(function()
  {
   resetS.Trigger({
    $:0,
    $0:null
   });
  },(b=Formlet$1.Do(),b.Delay(function()
  {
   return b.Bind(resetF,function()
   {
    return b.ReturnFrom(manyF());
   });
  })));
 };
 Enhance.Many_=function(add,f)
 {
  return Formlet$1.Map(function(s)
  {
   return Seq.choose(Global.id,s);
  },Formlet$1.FlipBody(Formlet$1.SelectMany(Formlet$1.Map(f,add))));
 };
 Enhance.Deletable=function(formlet)
 {
  return Enhance.Replace(formlet,function(value)
  {
   return value!=null&&value.$==1?Formlet$1.Return({
    $:1,
    $0:value.$0
   }):Formlet$1.ReturnEmpty(null);
  });
 };
 Enhance.Replace=function(formlet,f)
 {
  return Formlet$1.Switch(Formlet$1.MapResult(function(res)
  {
   return res.$==1?{
    $:0,
    $0:Formlet$1.FailWith(res.$0)
   }:{
    $:0,
    $0:f(res.$0)
   };
  },formlet));
 };
 Enhance.Cancel=function(formlet,isCancel)
 {
  return Formlet$1.Replace(formlet,function(value)
  {
   return isCancel(value)?Formlet$1.Empty():Formlet$1.Return(value);
  });
 };
 Enhance.WithRowConfiguration=function(rc,formlet)
 {
  var x;
  x=Formlet$1.ApplyLayout(formlet);
  return Formlet$1.WithLayout(Data.Layout().RowLayout(rc),x);
 };
 Enhance.WithLegend=function(label,formlet)
 {
  return Formlet$1.MapBody(function(body)
  {
   var a,a$1,m;
   return Body.New((a=[(a$1=[Tags.Tags().text(label)],Tags.Tags().NewTag("legend",a$1)),(m=body.Label,m==null?body.Element:Utils.InTable(List.ofArray([List.ofArray([m.$0(),body.Element])])))],Tags.Tags().NewTag("fieldset",a)),null);
  },formlet);
 };
 Enhance.WithCssClass=function(css,formlet)
 {
  return Formlet$1.MapElement(function(el)
  {
   el.HtmlProvider.AddClass(el.get_Body(),css);
   return el;
  },formlet);
 };
 Enhance.WithFormContainer=function(formlet)
 {
  return Enhance.WithCustomFormContainer(FormContainerConfiguration.get_Default(),formlet);
 };
 Enhance.WithCustomFormContainer=function(fc,formlet)
 {
  return Formlet$1.MapElement(function(formEl)
  {
   var description,tb,a,cell,a$1,m,m$1,a$2,a$3;
   function a$4(name,value)
   {
    if(value==null)
     ;
    else
     cell.HtmlProvider.SetCss(cell.get_Body(),name,value.$0);
   }
   description=Utils.Maybe(List.T.Empty,function(descr)
   {
    var a$5;
    return descr.$==1?List.ofArray([descr.$0()]):List.ofArray([(a$5=[Tags.Tags().text(descr.$0)],Tags.Tags().NewTag("p",a$5))]);
   },fc.Description);
   tb=Utils.Maybe(Utils.InTable(List.ofArray([List.ofArray([Operators.add((a=[Attr.Attr().NewAttr("class","headerPanel")],Tags.Tags().NewTag("div",a)),description)]),List.ofArray([formEl])])),function(formElem)
   {
    var hdr,a$5,a$6;
    return Utils.InTable(List.ofArray([List.ofArray([(hdr=formElem.$==1?formElem.$0():(a$5=[Tags.Tags().text(formElem.$0)],Tags.Tags().NewTag("h1",a$5)),Operators.add((a$6=[Attr.Attr().NewAttr("class","headerPanel")],Tags.Tags().NewTag("div",a$6)),new List.T({
     $:1,
     $0:hdr,
     $1:description
    })))]),List.ofArray([formEl])]));
   },fc.Header);
   cell=Operators.add((a$1=[Attr.Attr().NewAttr("class","formlet")],Tags.Tags().NewTag("td",a$1)),[tb]);
   Utils.Maybe(null,function(color)
   {
    cell.HtmlProvider.SetStyle(cell.get_Body(),"border-color: "+color);
   },fc.BorderColor);
   List.iter(function($1)
   {
    return a$4($1[0],$1[1]);
   },List.ofArray([["background-color",Utils.MapOption(Global.id,fc.BackgroundColor)],["padding-left",Utils.MapOption(function(v)
   {
    return Global.String(v)+"px";
   },fc.Padding.Left)],["padding-right",Utils.MapOption(function(v)
   {
    return Global.String(v)+"px";
   },fc.Padding.Right)],["padding-top",Utils.MapOption(function(v)
   {
    return Global.String(v)+"px";
   },fc.Padding.Top)],["padding-bottom",Utils.MapOption(function(v)
   {
    return Global.String(v)+"px";
   },fc.Padding.Bottom)]]));
   m=fc.Style;
   m==null?void 0:cell.HtmlProvider.SetStyle(cell.get_Body(),m.$0);
   m$1=fc.CssClass;
   m$1==null?void 0:cell.HtmlProvider.AddClass(cell.get_Body(),m$1.$0);
   a$2=[(a$3=[Tags.Tags().NewTag("tr",[cell])],Tags.Tags().NewTag("tbody",a$3))];
   return Tags.Tags().NewTag("table",a$2);
  },Formlet$1.ApplyLayout(formlet));
 };
 Enhance.WithLabelLeft=function(formlet)
 {
  return Formlet$1.MapBody(function(body)
  {
   var label,m,a,a$1,a$2;
   return Body.New((label=(m=body.Label,m==null?Tags.Tags().NewTag("span",[]):m.$0()),(a=[(a$1=[(a$2=[Tags.Tags().NewTag("td",[body.Element]),Tags.Tags().NewTag("td",[label])],Tags.Tags().NewTag("tr",a$2))],Tags.Tags().NewTag("tbody",a$1))],Tags.Tags().NewTag("table",a))),null);
  },formlet);
 };
 Enhance.WithLabelAbove=function(formlet)
 {
  return Formlet$1.MapBody(function(body)
  {
   var a,a$1,a$2,a$3,m,a$4;
   return Body.New((a=[(a$1=[(a$2=[(a$3=[(m=body.Label,m==null?Tags.Tags().NewTag("span",[]):m.$0())],Tags.Tags().NewTag("td",a$3))],Tags.Tags().NewTag("tr",a$2)),(a$4=[Tags.Tags().NewTag("td",[body.Element])],Tags.Tags().NewTag("tr",a$4))],Tags.Tags().NewTag("tbody",a$1))],Tags.Tags().NewTag("table",a)),null);
  },formlet);
 };
 Enhance.WithTextLabel=function(label,formlet)
 {
  return Enhance.WithLabel(function()
  {
   var a;
   a=[Tags.Tags().text(label)];
   return Tags.Tags().NewTag("label",a);
  },formlet);
 };
 Enhance.WithLabelAndInfo=function(label,info,formlet)
 {
  return Enhance.WithLabel(function()
  {
   var a,a$1;
   return Utils.InTable(List.ofArray([List.ofArray([(a=[Tags.Tags().text(label)],Tags.Tags().NewTag("label",a)),(a$1=[Attr.Attr().NewAttr("title",info),Attr.Attr().NewAttr("class","infoIcon")],Tags.Tags().NewTag("span",a$1))])]));
  },formlet);
 };
 Enhance.WithLabelConfiguration=function(lc,formlet)
 {
  var x;
  x=Formlet$1.ApplyLayout(formlet);
  return Formlet$1.WithLayout(Data.Layout().LabelLayout(lc),x);
 };
 Enhance.WithLabel=function(labelGen,formlet)
 {
  return Formlet$1.WithLabel({
   $:1,
   $0:labelGen
  },formlet);
 };
 Enhance.WithErrorFormlet=function(f,formlet)
 {
  var b;
  return Formlet$1.MapResult(Result.Join,(b=Formlet$1.Do(),b.Delay(function()
  {
   return b.Bind(Formlet$1.LiftResult(formlet),function(a)
   {
    var msgs,b$1;
    return b.ReturnFrom(a.$==1?(msgs=a.$0,(b$1=Formlet$1.Do(),b$1.Delay(function()
    {
     return b$1.Bind(f(msgs),function()
     {
      return b$1.Return(a);
     });
    }))):Formlet$1.Return(a));
   });
  })));
 };
 Enhance.WithValidationFrame=function(formlet)
 {
  return Enhance.WithCustomValidationFrame(ValidationFrameConfiguration.get_Default(),formlet);
 };
 Enhance.WithErrorSummary=function(label,formlet)
 {
  var b;
  function errrFormlet(fs)
  {
   return Formlet$1.OfElement(function()
   {
    var a,a$1,x;
    a=[(a$1=[Tags.Tags().text(label)],Tags.Tags().NewTag("legend",a$1)),(x=List.map(function(f)
    {
     var a$2;
     a$2=[Tags.Tags().text(f)];
     return Tags.Tags().NewTag("li",a$2);
    },fs),Tags.Tags().NewTag("ul",x))];
    return Tags.Tags().NewTag("fieldset",a);
   });
  }
  return Formlet$1.MapResult(Result.Join,(b=Formlet$1.Do(),b.Delay(function()
  {
   return b.Bind(Formlet$1.LiftResult(formlet),function(a)
   {
    return b.ReturnFrom(a.$==1?Formlet$1.Map(function()
    {
     return a;
    },errrFormlet(a.$0)):Formlet$1.Return(a));
   });
  })));
 };
 Enhance.WithSubmitButton=function(formlet)
 {
  return Enhance.WithCustomSubmitButton(FormButtonConfiguration.get_Default(),formlet);
 };
 Enhance.WithCustomSubmitButton=function(buttonConf,formlet)
 {
  var buttonConf$1;
  buttonConf$1=buttonConf.Label==null?FormButtonConfiguration.New({
   $:1,
   $0:"Submit"
  },buttonConf.Style,buttonConf.Class):buttonConf;
  return Enhance.WithSubmitFormlet(formlet,function(res)
  {
   return Formlet$1.Map(Global.ignore,Enhance.InputButton(buttonConf$1,res.$==0&&true));
  });
 };
 Enhance.WithResetButton=function(formlet)
 {
  return Enhance.WithCustomResetButton(FormButtonConfiguration.get_Default(),formlet);
 };
 Enhance.WithCustomResetButton=function(buttonConf,formlet)
 {
  return Enhance.WithResetFormlet(formlet,Enhance.InputButton(buttonConf.Label==null?FormButtonConfiguration.New({
   $:1,
   $0:"Reset"
  },buttonConf.Style,buttonConf.Class):buttonConf,true));
 };
 Enhance.WithCustomValidationFrame=function(vc,formlet)
 {
  return Enhance.WrapFormlet(function(state,body)
  {
   var x;
   function f(panel)
   {
    state.Subscribe(Util.observer(function(res)
    {
     var m,m$1,m$2,m$3,m$4,m$5;
     if(res.$==1)
      {
       m=vc.ValidClass;
       m!=null&&m.$==1?panel.HtmlProvider.RemoveClass(panel.get_Body(),m.$0):void 0;
       m$1=vc.ErrorClass;
       m$1!=null&&m$1.$==1?panel.HtmlProvider.AddClass(panel.get_Body(),m$1.$0):void 0;
       m$2=vc.ErrorStyle;
       m$2!=null&&m$2.$==1?panel.HtmlProvider.SetStyle(panel.get_Body(),m$2.$0):panel.HtmlProvider.SetStyle(panel.get_Body(),"");
      }
     else
      {
       m$3=vc.ErrorClass;
       m$3!=null&&m$3.$==1?panel.HtmlProvider.RemoveClass(panel.get_Body(),m$3.$0):void 0;
       m$4=vc.ValidClass;
       m$4!=null&&m$4.$==1?panel.HtmlProvider.AddClass(panel.get_Body(),m$4.$0):void 0;
       m$5=vc.ValidStyle;
       m$5!=null&&m$5.$==1?panel.HtmlProvider.SetStyle(panel.get_Body(),m$5.$0):panel.HtmlProvider.SetStyle(panel.get_Body(),"");
      }
    }));
   }
   x=Tags.Tags().NewTag("div",[body.Element]);
   (function(w)
   {
    Operators.OnAfterRender(f,w);
   }(x));
   return x;
  },formlet);
 };
 Enhance.WrapFormlet=function(wrapper,formlet)
 {
  return Data.MkFormlet(function()
  {
   var formlet$1,form;
   formlet$1=Formlet$1.WithLayoutOrDefault(formlet);
   form=Formlet$1.BuildForm(formlet$1);
   return[wrapper(form.State,formlet$1.LayoutI().Apply(form.Body).$0[0]),function()
   {
    form.Notify(null);
   },form.State];
  });
 };
 Enhance.WithValidationIcon=function(formlet)
 {
  return Enhance.WithCustomValidationIcon(ValidationIconConfiguration.get_Default(),formlet);
 };
 Enhance.WithCustomValidationIcon=function(vic,formlet)
 {
  var formlet$1,x,b;
  function valid(res)
  {
   return Formlet$1.OfElement(function()
   {
    var title,a,a$1;
    return res.$==1?(title=Seq.fold(function($1,$2)
    {
     return $1+" "+$2;
    },"",res.$0),Operators.add((a=[Attr.Attr().NewAttr("class",vic.ErrorIconClass),Attr.Attr().NewAttr("title",title)],Tags.Tags().NewTag("div",a)),[Tags.Tags().NewTag("div",[])])):Operators.add((a$1=[Attr.Attr().NewAttr("class",vic.ValidIconClass),Attr.Attr().NewAttr("title","")],Tags.Tags().NewTag("div",a$1)),[Tags.Tags().NewTag("div",[])]);
   });
  }
  formlet$1=Formlet$1.WithLayoutOrDefault(formlet);
  x=Formlet$1.MapResult(Result.Join,(b=Formlet$1.Do(),b.Delay(function()
  {
   return b.Bind(Formlet$1.LiftResult(formlet$1),function(a)
   {
    return b.Bind(valid(a),function()
    {
     return b.Return(a);
    });
   });
  })));
  return Formlet$1.WithLayout(Data.Layout().get_Horizontal(),x);
 };
 Enhance.WithSubmitAndResetButtons=function(formlet)
 {
  var i,i$1;
  return Enhance.WithCustomSubmitAndResetButtons((i=FormButtonConfiguration.get_Default(),FormButtonConfiguration.New({
   $:1,
   $0:"Submit"
  },i.Style,i.Class)),(i$1=FormButtonConfiguration.get_Default(),FormButtonConfiguration.New({
   $:1,
   $0:"Reset"
  },i$1.Style,i$1.Class)),formlet);
 };
 Enhance.WithCustomSubmitAndResetButtons=function(submitConf,resetConf,formlet)
 {
  return Enhance.WithSubmitAndReset(formlet,function(reset,result)
  {
   var submit,fs,value,reset$1,b,x;
   submit=result.$==1?(fs=result.$0,Formlet$1.MapResult(function()
   {
    return{
     $:1,
     $0:fs
    };
   },Enhance.InputButton(submitConf,false))):(value=result.$0,Formlet$1.Map(function()
   {
    return value;
   },Enhance.InputButton(submitConf,true)));
   reset$1=(b=Formlet$1.Do(),b.Delay(function()
   {
    return b.Bind(Formlet$1.LiftResult(Enhance.InputButton(resetConf,true)),function(a)
    {
     a.$==0?reset():void 0;
     return b.Return();
    });
   }));
   x=Data.$(Data.$(Formlet$1.Return(function(v)
   {
    return function()
    {
     return v;
    };
   }),submit),reset$1);
   return Formlet$1.WithLayout(Data.Layout().get_Horizontal(),x);
  });
 };
 Enhance.InputButton=function(conf,enabled)
 {
  return Data.MkFormlet(function()
  {
   var state,count,submit,label,submit$1,x,a,m,m$1;
   function a$1(a$2,a$3)
   {
    count[0]++;
    return state.Trigger({
     $:0,
     $0:count[0]
    });
   }
   state=HotStream.New$1({
    $:1,
    $0:List.T.Empty
   });
   count=[0];
   submit=(label=Utils.Maybe("Submit",Global.id,conf.Label),(submit$1=(x=Operators.add((a=[Attr.Attr().NewAttr("type","button")],Tags.Tags().NewTag("input",a)),[Attr.Attr().NewAttr("class","submitButton"),Attr.Attr().NewAttr("value",label)]),(function(a$2)
   {
    EventsPervasives.Events().OnClick(function($1)
    {
     return function($2)
     {
      return a$1($1,$2);
     };
    },a$2);
   }(x),x)),(!enabled?submit$1.HtmlProvider.AddClass(submit$1.get_Body(),"disabledButton"):void 0,m=conf.Style,m!=null&&m.$==1?submit$1.HtmlProvider.SetStyle(submit$1.get_Body(),m.$0):void 0,m$1=conf.Class,m$1!=null&&m$1.$==1?submit$1.HtmlProvider.AddClass(submit$1.get_Body(),m$1.$0):void 0,submit$1)));
   state.Trigger({
    $:1,
    $0:List.T.Empty
   });
   return[submit,function()
   {
    count[0]=0;
    state.Trigger({
     $:1,
     $0:List.T.Empty
    });
   },state];
  });
 };
 Enhance.WithSubmitAndReset=function(formlet,submReset)
 {
  var b;
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,(b=Formlet$1.Do(),b.Delay(function()
  {
   return b.Bind(Formlet$1.WithNotificationChannel(Formlet$1.LiftResult(Formlet$1.InitWithFailure(formlet))),function(a)
   {
    var notify;
    notify=a[1];
    return b.ReturnFrom(submReset(function()
    {
     notify(void 0);
    },a[0]));
   });
  }))));
 };
 Enhance.WithSubmitFormlet=function(formlet,submit)
 {
  var b;
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,Formlet$1.MapResult(Result.Join,(b=Formlet$1.Do(),b.Delay(function()
  {
   return b.Bind(Formlet$1.LiftResult(Formlet$1.InitWithFailure(formlet)),function(a)
   {
    return b.Bind(submit(a),function()
    {
     return b.Return(a);
    });
   });
  })))));
 };
 Enhance.WithResetAction=function(f,formlet)
 {
  var x;
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet,(x=Formlet$1.New(function()
  {
   var form;
   form=formlet.BuildI();
   return Form.New(form.Body,form.Dispose$1,function(o)
   {
    if(f())
     form.Notify(o);
   },form.State);
  }),Formlet$1.WithLayout(formlet.LayoutI(),x))));
 };
 Enhance.WithResetFormlet=function(formlet,reset)
 {
  var formlet$1,button,b;
  formlet$1=Formlet$1.WithNotificationChannel(Formlet$1.LiftResult(Formlet$1.InitWithFailure(Formlet$1.ApplyLayout(Formlet$1.WithLayoutOrDefault(formlet)))));
  button=Formlet$1.LiftResult(reset);
  return Data.OfIFormlet(Data.PropagateRenderFrom(formlet$1,Formlet$1.MapResult(Result.Join,(b=Formlet$1.Do(),b.Delay(function()
  {
   return b.Bind(formlet$1,function(a)
   {
    var v,notify;
    v=a[0];
    notify=a[1];
    return b.Bind(button,function(a$1)
    {
     a$1.$==0?notify(void 0):void 0;
     return b.Return(v);
    });
   });
  })))));
 };
}());
