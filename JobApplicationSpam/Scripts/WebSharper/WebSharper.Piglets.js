(function()
{
 "use strict";
 var Global,WebSharper,Piglets,Id,ErrorMessage,Result,Reader,Stream,Disposable,ConstReader,ConcreteWriter,ConcreteReader,Submitter,Piglet,Validation,Pervasives,_Writer1,Many,Operations,Stream$1,UnitStream,Choose,Stream$2,Builder,SC$1,Controls,HtmlContainer,SC$2,IntelliFactory,Runtime,List,Util,Reactive,HotStream,Concurrency,Arrays,Seq,Collections,Dictionary,Enumerator,Operators,Unchecked,Html,Client,Operators$1,Attr,Tags,EventsPervasives;
 Global=window;
 WebSharper=Global.WebSharper=Global.WebSharper||{};
 Piglets=WebSharper.Piglets=WebSharper.Piglets||{};
 Id=Piglets.Id=Piglets.Id||{};
 ErrorMessage=Piglets.ErrorMessage=Piglets.ErrorMessage||{};
 Result=Piglets.Result=Piglets.Result||{};
 Reader=Piglets.Reader=Piglets.Reader||{};
 Stream=Piglets.Stream=Piglets.Stream||{};
 Disposable=Piglets.Disposable=Piglets.Disposable||{};
 ConstReader=Piglets.ConstReader=Piglets.ConstReader||{};
 ConcreteWriter=Piglets.ConcreteWriter=Piglets.ConcreteWriter||{};
 ConcreteReader=Piglets.ConcreteReader=Piglets.ConcreteReader||{};
 Submitter=Piglets.Submitter=Piglets.Submitter||{};
 Piglet=Piglets.Piglet=Piglets.Piglet||{};
 Validation=Piglets.Validation=Piglets.Validation||{};
 Pervasives=Piglets.Pervasives=Piglets.Pervasives||{};
 _Writer1=Pervasives["Writer`1"]=Pervasives["Writer`1"]||{};
 Many=Piglets.Many=Piglets.Many||{};
 Operations=Many.Operations=Many.Operations||{};
 Stream$1=Many.Stream=Many.Stream||{};
 UnitStream=Many.UnitStream=Many.UnitStream||{};
 Choose=Piglets.Choose=Piglets.Choose||{};
 Stream$2=Choose.Stream=Choose.Stream||{};
 Builder=Piglet.Builder=Piglet.Builder||{};
 SC$1=Global.StartupCode$WebSharper_Piglets$Piglets=Global.StartupCode$WebSharper_Piglets$Piglets||{};
 Controls=Piglets.Controls=Piglets.Controls||{};
 HtmlContainer=Controls.HtmlContainer=Controls.HtmlContainer||{};
 SC$2=Global.StartupCode$WebSharper_Piglets$Controls=Global.StartupCode$WebSharper_Piglets$Controls||{};
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 List=WebSharper&&WebSharper.List;
 Util=WebSharper&&WebSharper.Util;
 Reactive=IntelliFactory&&IntelliFactory.Reactive;
 HotStream=Reactive&&Reactive.HotStream;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Arrays=WebSharper&&WebSharper.Arrays;
 Seq=WebSharper&&WebSharper.Seq;
 Collections=WebSharper&&WebSharper.Collections;
 Dictionary=Collections&&Collections.Dictionary;
 Enumerator=WebSharper&&WebSharper.Enumerator;
 Operators=WebSharper&&WebSharper.Operators;
 Unchecked=WebSharper&&WebSharper.Unchecked;
 Html=WebSharper&&WebSharper.Html;
 Client=Html&&Html.Client;
 Operators$1=Client&&Client.Operators;
 Attr=Client&&Client.Attr;
 Tags=Client&&Client.Tags;
 EventsPervasives=Client&&Client.EventsPervasives;
 Id.next=function()
 {
  SC$1.$cctor();
  return SC$1.next;
 };
 ErrorMessage=Piglets.ErrorMessage=Runtime.Class({
  get_Source:function()
  {
   return this.source;
  },
  get_Message:function()
  {
   return this.message;
  }
 },WebSharper.Obj,ErrorMessage);
 ErrorMessage.Create=function(msg,reader)
 {
  return new ErrorMessage.New(msg,reader.get_Id());
 };
 ErrorMessage.New=Runtime.Ctor(function(message,source)
 {
  this.message=message;
  this.source=source;
 },ErrorMessage);
 Result=Piglets.Result=Runtime.Class({
  get_isSuccess:function()
  {
   return this.$==1?false:true;
  }
 },null,Result);
 Result.Bind=function(f)
 {
  return function(a)
  {
   return a.$==1?new Result({
    $:1,
    $0:a.$0
   }):f(a.$0);
  };
 };
 Result.Iter=function(f)
 {
  return function(a)
  {
   if(a.$==1)
    ;
   else
    f(a.$0);
  };
 };
 Result.Map2=function(f,ra,rb)
 {
  var $1;
  switch(ra.$==1?rb.$==1?($1=[ra.$0,rb.$0],1):($1=ra.$0,2):rb.$==1?($1=rb.$0,2):($1=[ra.$0,rb.$0],0))
  {
   case 0:
    return new Result({
     $:0,
     $0:f($1[0],$1[1])
    });
    break;
   case 1:
    return new Result({
     $:1,
     $0:List.append($1[0],$1[1])
    });
    break;
   case 2:
    return new Result({
     $:1,
     $0:$1
    });
    break;
  }
 };
 Result.Map=function(f,ra)
 {
  return ra.$==1?new Result({
   $:1,
   $0:ra.$0
  }):new Result({
   $:0,
   $0:f(ra.$0)
  });
 };
 Result.Join=function(r)
 {
  var $1;
  return(r.$==0?r.$0.$==0?($1=r.$0.$0,false):($1=r.$0.$0,true):($1=r.$0,true))?new Result({
   $:1,
   $0:$1
  }):new Result({
   $:0,
   $0:$1
  });
 };
 Result.Ap=function(r1,r2)
 {
  var $1;
  switch(r1.$==1?r2.$==1?($1=[r1.$0,r2.$0],2):($1=r1.$0,1):r2.$==1?($1=r2.$0,1):($1=[r1.$0,r2.$0],0))
  {
   case 0:
    return new Result({
     $:0,
     $0:$1[0]($1[1])
    });
    break;
   case 1:
    return new Result({
     $:1,
     $0:$1
    });
    break;
   case 2:
    return new Result({
     $:1,
     $0:List.append($1[0],$1[1])
    });
    break;
  }
 };
 Result.Failwith=function(msg)
 {
  return new Result({
   $:1,
   $0:List.ofArray([new ErrorMessage.New(msg,0)])
  });
 };
 Reader=Piglets.Reader=Runtime.Class({
  Through:function(r)
  {
   var $this,out;
   $this=this;
   out=new Stream.New(this.get_Latest(),null);
   r.Subscribe(function(a)
   {
    var $1,$2;
    if(a.$==1)
     {
      $1=$this.get_Latest();
      $2=List.filter(function(m)
      {
       return m.get_Source()===$this.get_Id();
      },a.$0);
      $2.$==0?out.Trigger($this.get_Latest()):$1.$==1?out.Trigger(new Result({
       $:1,
       $0:List.append($1.$0,$2)
      })):out.Trigger(new Result({
       $:1,
       $0:$2
      }));
     }
    else
     out.Trigger($this.get_Latest());
   });
   return out;
  },
  get_Id:function()
  {
   return this.id;
  },
  SubscribeImmediate:function(f)
  {
   return this.Subscribe(f);
  }
 },WebSharper.Obj,Reader);
 Reader.ConstResult=function(x)
 {
  return new ConstReader.New(x);
 };
 Reader.Const=function(x)
 {
  return new ConstReader.New(new Result({
   $:0,
   $0:x
  }));
 };
 Reader.MapToResult=function(f,r)
 {
  return Reader.MapResult(Result.Bind(f),r);
 };
 Reader.Map2=function(f,rb,rc)
 {
  return Reader.MapResult2(function(b,c)
  {
   return Result.Map2(f,b,c);
  },rb,rc);
 };
 Reader.Map=function(f,r)
 {
  return Reader.MapResult(function(a)
  {
   return Result.Map(f,a);
  },r);
 };
 Reader.MapResult2=function(f,rb,rc)
 {
  var out;
  out=new Stream.New(f(rb.get_Latest(),rc.get_Latest()),null);
  rb.Subscribe(function(b)
  {
   out.Trigger(f(b,rc.get_Latest()));
  });
  rc.Subscribe(function(c)
  {
   out.Trigger(f(rb.get_Latest(),c));
  });
  return out;
 };
 Reader.MapResult=function(f,r)
 {
  var out;
  function f$1(a)
  {
   out.Trigger(a);
  }
  out=new Stream.New(f(r.get_Latest()),null);
  r.Subscribe(function(x)
  {
   return f$1(f(x));
  });
  return out;
 };
 Reader.New=Runtime.Ctor(function(id)
 {
  this.id=id;
 },Reader);
 Stream=Piglets.Stream=Runtime.Class({
  Write:function(x)
  {
   var $this;
   $this=this;
   return new ConcreteWriter.New$1(function(a)
   {
    if(a.$==0)
     $this.Trigger(new Result({
      $:0,
      $0:x
     }));
    else
     $this.Trigger(new Result({
      $:1,
      $0:a.$0
     }));
   });
  },
  Trigger:function(x)
  {
   this.s.Trigger(x);
  },
  Subscribe:function(f)
  {
   return this.s.Subscribe(Util.observer(f));
  },
  get_Latest:function()
  {
   return this.s.Latest[0].$0;
  },
  WebSharper_Piglets_Writer_1$Trigger:function(x)
  {
   this.Trigger(x);
  }
 },Reader,Stream);
 Stream.New=Runtime.Ctor(function(init,id)
 {
  Stream.New$1.call(this,HotStream.New$1(init),id);
 },Stream);
 Stream.New$1=Runtime.Ctor(function(s,id)
 {
  Reader.New.call(this,id==null?(Id.next())():id.$0);
  this.s=s;
 },Stream);
 Disposable=Piglets.Disposable=Runtime.Class({
  Dispose:function()
  {
   this.dispose();
  }
 },WebSharper.Obj,Disposable);
 Disposable.New=Runtime.Ctor(function(dispose)
 {
  this.dispose=dispose;
 },Disposable);
 ConstReader=Piglets.ConstReader=Runtime.Class({
  Subscribe:function(f)
  {
   return new Disposable.New(Global.ignore);
  },
  get_Latest:function()
  {
   return this.x;
  }
 },Reader,ConstReader);
 ConstReader.New=Runtime.Ctor(function(x)
 {
  Reader.New.call(this,(Id.next())());
  this.x=x;
 },ConstReader);
 ConcreteWriter=Piglets.ConcreteWriter=Runtime.Class({
  WebSharper_Piglets_Writer_1$Trigger:function(x)
  {
   this.trigger(x);
  }
 },WebSharper.Obj,ConcreteWriter);
 ConcreteWriter.New=function(trigger)
 {
  return new ConcreteWriter.New$1(function(a)
  {
   if(a.$==1)
    ;
   else
    trigger(a.$0);
  });
 };
 ConcreteWriter.New$1=Runtime.Ctor(function(trigger)
 {
  this.trigger=trigger;
 },ConcreteWriter);
 ConcreteReader=Piglets.ConcreteReader=Runtime.Class({
  Subscribe:function(f)
  {
   return this.subscribe(f);
  },
  get_Latest:function()
  {
   return this.latest();
  }
 },Reader,ConcreteReader);
 ConcreteReader.New=Runtime.Ctor(function(latest,subscribe)
 {
  Reader.New.call(this,(Id.next())());
  this.latest=latest;
  this.subscribe=subscribe;
 },ConcreteReader);
 Submitter=Piglets.Submitter=Runtime.Class({
  Trigger:function()
  {
   this.writer.WebSharper_Piglets_Writer_1$Trigger(new Result({
    $:0,
    $0:null
   }));
  },
  get_Output:function()
  {
   return this.output;
  },
  get_Input:function()
  {
   return this.input;
  },
  Subscribe:function(f)
  {
   return this.output.Subscribe(f);
  },
  get_Latest:function()
  {
   return this.output.get_Latest();
  },
  WebSharper_Piglets_Writer_1$Trigger:function(x)
  {
   this.writer.WebSharper_Piglets_Writer_1$Trigger(x);
  }
 },Reader,Submitter);
 Submitter.New=Runtime.Ctor(function(input,clearError)
 {
  var $this;
  $this=this;
  Reader.New.call(this,(Id.next())());
  this.input=input;
  this.output=new Stream.New(new Result({
   $:1,
   $0:List.T.Empty
  }),null);
  this.writer=new ConcreteWriter.New$1(function(unitIn)
  {
   var $1,$2;
   $2=$this.input.get_Latest();
   switch(unitIn.$==0?$2.$==0?($1=$2.$0,2):($1=$2.$0,1):$2.$==0?($1=unitIn.$0,1):($1=[unitIn.$0,$2.$0],0))
   {
    case 0:
     $this.output.Trigger(new Result({
      $:1,
      $0:List.append($1[0],$1[1])
     }));
     break;
    case 1:
     $this.output.Trigger(new Result({
      $:1,
      $0:$1
     }));
     break;
    case 2:
     $this.output.Trigger(new Result({
      $:0,
      $0:$1
     }));
     break;
   }
  });
  clearError?this.input.Subscribe(function()
  {
   var m,$1;
   m=$this.output.get_Latest();
   m.$==1&&(m.$0.$==0&&true)?void 0:$this.output.Trigger(new Result({
    $:1,
    $0:List.T.Empty
   }));
  }):void 0;
 },Submitter);
 Stream.Map=function(a2b,b2a,s)
 {
  var _s,pa,pb;
  _s=new Stream.New(Result.Map(a2b,s.get_Latest()),{
   $:1,
   $0:s.get_Id()
  });
  pa=[s.get_Latest()];
  pb=[_s.get_Latest()];
  s.Subscribe(function(a)
  {
   if(pa[0]!==a)
    {
     pb[0]=Result.Map(a2b,a);
     _s.Trigger(pb[0]);
    }
  });
  _s.Subscribe(function(b)
  {
   if(pb[0]!==b)
    {
     pa[0]=Result.Map(b2a,b);
     s.Trigger(pa[0]);
    }
  });
  return _s;
 };
 Stream.ApJoin=function(sf,sx)
 {
  var out;
  out=new Stream.New(Result.Ap(sf.get_Latest(),Result.Join(sx.get_Latest())),null);
  sf.Subscribe(function(f)
  {
   out.Trigger(Result.Ap(f,Result.Join(sx.get_Latest())));
  });
  sx.Subscribe(function(x)
  {
   out.Trigger(Result.Ap(sf.get_Latest(),Result.Join(x)));
  });
  return out;
 };
 Stream.Ap=function(sf,sx)
 {
  var out;
  out=new Stream.New(Result.Ap(sf.get_Latest(),sx.get_Latest()),null);
  sf.Subscribe(function(f)
  {
   out.Trigger(Result.Ap(f,sx.get_Latest()));
  });
  sx.Subscribe(function(x)
  {
   out.Trigger(Result.Ap(sf.get_Latest(),x));
  });
  return out;
 };
 Piglet=Piglets.Piglet=Runtime.Class({
  get_Stream:function()
  {
   return this.stream;
  }
 },null,Piglet);
 Piglet.New=function(stream,view)
 {
  return new Piglet({
   stream:stream,
   view:view
  });
 };
 Validation.IsMatch=function(re,msg,p)
 {
  return Validation.Is(Validation.Match(re),msg,p);
 };
 Validation.IsNotEmpty=function(msg,p)
 {
  return Validation.Is(Validation.NotEmpty,msg,p);
 };
 Validation.Match=function(re)
 {
  var o;
  o=new Global.RegExp(re);
  return function(a)
  {
   return o.test(a);
  };
 };
 Validation.NotEmpty=function(x)
 {
  return x!=="";
 };
 Validation.Is=function(pred,msg,p)
 {
  var _s;
  _s=new Stream.New(p.stream.get_Latest(),{
   $:1,
   $0:p.stream.get_Id()
  });
  p.stream.Subscribe(function(a)
  {
   if(a.$==0)
   {
    if(pred(a.$0))
     _s.Trigger(new Result({
      $:0,
      $0:a.$0
     }));
    else
     _s.Trigger(new Result({
      $:1,
      $0:List.ofArray([new ErrorMessage.New(msg,_s.get_Id())])
     }));
   }
   else
    _s.Trigger(new Result({
     $:1,
     $0:a.$0
    }));
  });
  return Piglet.New(_s,p.view);
 };
 Validation["Is'"]=function(pred,msg,p)
 {
  var _s;
  _s=new Stream.New(p.stream.get_Latest(),{
   $:1,
   $0:p.stream.get_Id()
  });
  p.stream.Subscribe(function(a)
  {
   if(a.$==0)
   {
    if(pred(a.$0))
     _s.Trigger(new Result({
      $:0,
      $0:a.$0
     }));
    else
     _s.Trigger(new Result({
      $:1,
      $0:List.ofArray([msg])
     }));
   }
   else
    _s.Trigger(new Result({
     $:1,
     $0:a.$0
    }));
  });
  return Piglet.New(_s,p.view);
 };
 Pervasives.op_LessMultiplyQmarkGreater=function(f,x)
 {
  var f$1,g;
  return Piglet.New(Stream.ApJoin(f.stream,x.stream),(f$1=f.view,(g=x.view,function(x$1)
  {
   return g(f$1(x$1));
  })));
 };
 Pervasives.op_LessMultiplyGreater=function(f,x)
 {
  var f$1,g;
  return Piglet.New(Stream.Ap(f.stream,x.stream),(f$1=f.view,(g=x.view,function(x$1)
  {
   return g(f$1(x$1));
  })));
 };
 _Writer1.WrapAsync=function(f,r)
 {
  return _Writer1.WrapToAsyncResult(function(b)
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    return Concurrency.Bind(f(b),function(a)
    {
     return Concurrency.Return(new Result({
      $:0,
      $0:a
     }));
    });
   });
  },r);
 };
 _Writer1.WrapToAsyncResult=function(f,r)
 {
  return _Writer1.WrapAsyncResult(function(b)
  {
   var b$1;
   b$1=null;
   return Concurrency.Delay(function()
   {
    return b.$==1?Concurrency.Return(new Result({
     $:1,
     $0:b.$0
    })):f(b.$0);
   });
  },r);
 };
 _Writer1.WrapAsyncResult=function(f,r)
 {
  return new ConcreteWriter.New$1(function(ra)
  {
   var b;
   Concurrency.Start((b=null,Concurrency.Delay(function()
   {
    return Concurrency.Bind(f(ra),function(a)
    {
     r.WebSharper_Piglets_Writer_1$Trigger(a);
     return Concurrency.Zero();
    });
   })),null);
  });
 };
 _Writer1.WrapResult=function(f,r)
 {
  return new ConcreteWriter.New$1(function(a)
  {
   r.WebSharper_Piglets_Writer_1$Trigger(f(a));
  });
 };
 _Writer1.WrapToResult=function(f,r)
 {
  return new ConcreteWriter.New$1(function(a)
  {
   r.WebSharper_Piglets_Writer_1$Trigger((Result.Bind(f))(a));
  });
 };
 _Writer1.Wrap=function(f,r)
 {
  return new ConcreteWriter.New$1(function(a)
  {
   r.WebSharper_Piglets_Writer_1$Trigger(Result.Map(f,a));
  });
 };
 Operations=Many.Operations=Runtime.Class({
  get_MoveDown:function()
  {
   return this.moveDown;
  },
  get_MoveUp:function()
  {
   return this.moveUp;
  },
  get_Delete:function()
  {
   return ConcreteWriter.New(this["delete"]);
  }
 },WebSharper.Obj,Operations);
 Operations.New=Runtime.Ctor(function(_delete,moveUp,moveDown)
 {
  this["delete"]=_delete;
  this.moveUp=moveUp;
  this.moveDown=moveDown;
 },Operations);
 Stream$1=Many.Stream=Runtime.Class({
  update:function()
  {
   this.out.Trigger(Result.Map(function(x)
   {
    return Arrays.ofList(List.rev(x));
   },Seq.fold(function($1,$2)
   {
    var $3,$4;
    $4=$2.get_Latest();
    switch($1.$==1?$4.$==1?($3=[$1.$0,$4.$0],2):($3=$1.$0,1):$4.$==1?($3=$4.$0,1):($3=[$1.$0,$4.$0],0))
    {
     case 0:
      return new Result({
       $:0,
       $0:new List.T({
        $:1,
        $0:$3[1],
        $1:$3[0]
       })
      });
      break;
     case 1:
      return new Result({
       $:1,
       $0:$3
      });
      break;
     case 2:
      return new Result({
       $:1,
       $0:List.append($3[1],$3[0])
      });
      break;
    }
   },new Result({
    $:0,
    $0:List.T.Empty
   }),this.streams)));
  },
  AddRender:function(f)
  {
   return this.adder.view(f);
  },
  get_Add:function()
  {
   return this.adder.stream;
  },
  Render:function(c,f)
  {
   var $this,m;
   function add(x)
   {
    var piglet,inMoveUp,inMoveDown,outSubscription,subMoveUp,subMoveDown,subUpSubscription,subDownSubscription;
    function getThisIndex()
    {
     return Seq.findIndex(function(x$1)
     {
      return x$1.get_Id()===piglet.stream.get_Id();
     },$this.streams);
    }
    function moveUp(i)
    {
     var s;
     if(i>0&&i<Arrays.length($this.streams))
      {
       s=Arrays.get($this.streams,i);
       Arrays.set($this.streams,i,Arrays.get($this.streams,i-1));
       Arrays.set($this.streams,i-1,s);
       c.WebSharper_Piglets_Container_2$MoveUp(i);
       $this.update();
      }
    }
    function canMoveUp()
    {
     return getThisIndex()>0?new Result({
      $:0,
      $0:null
     }):new Result({
      $:1,
      $0:List.T.Empty
     });
    }
    function canMoveDown()
    {
     return getThisIndex()<Arrays.length($this.streams)-1?new Result({
      $:0,
      $0:null
     }):new Result({
      $:1,
      $0:List.T.Empty
     });
    }
    piglet=$this.p(x);
    $this.streams.push(piglet.stream);
    piglet.stream.Subscribe(function()
    {
     $this.update();
    });
    inMoveUp=new Stream.New(canMoveUp(),null);
    inMoveDown=new Stream.New(canMoveDown(),null);
    outSubscription=$this.out.Subscribe(function()
    {
     inMoveUp.Trigger(canMoveUp());
     inMoveDown.Trigger(canMoveDown());
    });
    subMoveUp=new Submitter.New(inMoveUp,false);
    subMoveDown=new Submitter.New(inMoveDown,false);
    subUpSubscription=subMoveUp.Subscribe(Result.Iter(function()
    {
     moveUp(getThisIndex());
    }));
    subDownSubscription=subMoveDown.Subscribe(Result.Iter(function()
    {
     moveUp(getThisIndex()+1);
    }));
    c.WebSharper_Piglets_Container_2$Add(piglet.view(f(new Operations.New(function()
    {
     var i,_this;
     i=getThisIndex();
     _this=$this.streams;
     _this.splice.apply(_this,[i,1]);
     c.WebSharper_Piglets_Container_2$Remove(i);
     outSubscription.Dispose();
     subUpSubscription.Dispose();
     subDownSubscription.Dispose();
     $this.update();
    },subMoveUp,subMoveDown))));
   }
   $this=this;
   m=this.out.get_Latest();
   m.$==0?Arrays.iter(add,m.$0):void 0;
   this.adder.stream.Subscribe(function(a)
   {
    if(a.$==0)
     add(a.$0);
   });
   return c.WebSharper_Piglets_Container_2$get_Container();
  },
  get_Latest:function()
  {
   return this.out.get_Latest();
  },
  Subscribe:function(f)
  {
   return this.out.Subscribe(f);
  }
 },Reader,Stream$1);
 Stream$1.New=Runtime.Ctor(function(p,out,adder)
 {
  Reader.New.call(this,out.get_Id());
  this.p=p;
  this.out=out;
  this.adder=adder;
  this.streams=[];
 },Stream$1);
 UnitStream=Many.UnitStream=Runtime.Class({
  get_Add$1:function()
  {
   return this.submitStream;
  }
 },Stream$1,UnitStream);
 UnitStream.New=Runtime.Ctor(function(p,out,init,_default)
 {
  var submitter,trigger,o;
  Stream$1.New.call(this,p,out,init);
  this.submitStream=(submitter=new Stream.New(new Result({
   $:1,
   $0:List.T.Empty
  }),null),(trigger=(o=init.get_Stream(),function(a)
  {
   o.Trigger(a);
  }),(submitter.Subscribe(function(a)
  {
   if(a.$==0)
    trigger(new Result({
     $:0,
     $0:_default
    }));
   else
    trigger(new Result({
     $:1,
     $0:a.$0
    }));
  }),submitter)));
 },UnitStream);
 Stream$2=Choose.Stream=Runtime.Class({
  Choice:function(c,f)
  {
   var $this,renders,hasChild;
   $this=this;
   renders=new Dictionary.New$5();
   hasChild=[false];
   this.subscriptions[0]=new List.T({
    $:1,
    $0:this.pStream.Subscribe(function(res)
    {
     var p,i,render;
     if(res.$==0)
      {
       p=res.$0[1];
       i=res.$0[0];
       render=renders.ContainsKey(i)?renders.get_Item(i):p.view(f);
       $this.out.Trigger(p.stream.get_Latest());
       hasChild[0]?c.WebSharper_Piglets_Container_2$Remove(0):void 0;
       hasChild[0]=true;
       c.WebSharper_Piglets_Container_2$Add(render);
      }
    }),
    $1:this.subscriptions[0]
   });
   return c.WebSharper_Piglets_Container_2$get_Container();
  },
  get_ChooserStream:function()
  {
   return this.chooser.stream;
  },
  Chooser:function(f)
  {
   return this.chooser.view(f);
  },
  Subscribe:function(f)
  {
   return this.out.Subscribe(f);
  },
  get_Latest:function()
  {
   return this.out.get_Latest();
  },
  Dispose:function()
  {
   var i,e;
   i=this.subscriptions[0];
   e=Enumerator.Get(i);
   try
   {
    while(e.MoveNext())
     e.Current().Dispose();
   }
   finally
   {
    if("Dispose"in e)
     e.Dispose();
   }
   Seq.iter(function(a)
   {
    (Operators.KeyValue(a))[1][1].Dispose();
   },this.choiceSubscriptions);
  }
 },Reader,Stream$2);
 Stream$2.New=Runtime.Ctor(function(chooser,choice,out)
 {
  var $this;
  $this=this;
  Reader.New.call(this,out.get_Id());
  this.chooser=chooser;
  this.out=out;
  this.pStream=new Stream.New(new Result({
   $:1,
   $0:List.T.Empty
  }),null);
  this.choiceSubscriptions=new Dictionary.New$5();
  this.subscriptions=[List.ofArray([this.chooser.stream.Subscribe(function(res)
  {
   $this.pStream.Trigger(Result.Map(function(i)
   {
    var p,o;
    return[i,$this.choiceSubscriptions.ContainsKey(i)?($this.choiceSubscriptions.get_Item(i))[0]:(p=choice(i),($this.choiceSubscriptions.set_Item(i,[p,p.stream.Subscribe((o=$this.out,function(a)
    {
     o.Trigger(a);
    }))]),p))];
   },res));
  })])];
 },Stream$2);
 Builder=Piglet.Builder=Runtime.Class({
  Zero:function()
  {
   return Piglet.ReturnFailure();
  },
  YieldFrom:Global.id,
  Yield:function(x)
  {
   return Piglet.Yield(x);
  },
  ReturnFrom:Global.id,
  Return:function(x)
  {
   return Piglet.Return(x);
  },
  Bind:function(p,f)
  {
   return Piglet.Choose(p,f);
  }
 },null,Builder);
 Builder.Do=new Builder({
  $:0
 });
 Piglet.Confirm=function(init,validate,nomatch)
 {
  var second,x;
  function a(a$1,b)
  {
   return[a$1,b];
  }
  second=Piglet.Yield(init);
  return Piglet.MapViewArgs(function($1)
  {
   return function($2)
   {
    return a($1,$2);
   };
  },Piglet.Map(function(t)
  {
   return t[0];
  },(x=Pervasives.op_LessMultiplyGreater(Pervasives.op_LessMultiplyGreater(Piglet.Return(function(a$1)
  {
   return function(b)
   {
    return[a$1,b];
   };
  }),validate(Piglet.Yield(init))),second),Validation["Is'"](function($1)
  {
   return Unchecked.Equals($1[0],$1[1]);
  },ErrorMessage.Create(nomatch,second.get_Stream()),x))));
 };
 Piglet.YieldOption=function(x,none)
 {
  function a(a$2)
  {
   return a$2!=null&&a$2.$==1?a$2.$0:none;
  }
  function a$1(x$1)
  {
   return Unchecked.Equals(x$1,none)?null:{
    $:1,
    $0:x$1
   };
  }
  return Piglet.MapViewArgs(function(a$2)
  {
   return Stream.Map(a,a$1,a$2);
  },Piglet.Yield(x));
 };
 Piglet.MapViewArgs=function(view,p)
 {
  var a;
  return Piglet.New(p.stream,(a=p.view,function(a$1)
  {
   return a$1(a(view));
  }));
 };
 Piglet.Render=function(view,p)
 {
  return p.view(view);
 };
 Piglet.Run=function(action,p)
 {
  return Piglet.RunResult(Result.Iter(action),p);
 };
 Piglet.RunResult=function(action,p)
 {
  p.stream.Subscribe(action);
  return p;
 };
 Piglet.FlushErrors=function(p)
 {
  return Piglet.MapResult(function(a)
  {
   return a.$==1?new Result({
    $:1,
    $0:List.T.Empty
   }):a;
  },p);
 };
 Piglet.MapWithWriter=function(f,p)
 {
  function _f(out,r)
  {
   return r.$==0?f(out,r.$0):out.WebSharper_Piglets_Writer_1$Trigger(new Result({
    $:1,
    $0:r.$0
   }));
  }
  return Piglet.MapResultWithWriter(function($1)
  {
   return function($2)
   {
    return _f($1,$2);
   };
  },p);
 };
 Piglet.MapResultWithWriter=function(f,p)
 {
  var stream;
  stream=new Stream.New(new Result({
   $:1,
   $0:List.T.Empty
  }),null);
  p.stream.Subscribe(f(stream));
  return Piglet.New(stream,p.view);
 };
 Piglet.MapAsync=function(m,p)
 {
  return Piglet.MapAsyncResult(function(a)
  {
   var x,b;
   return a.$==0?(x=a.$0,(b=null,Concurrency.Delay(function()
   {
    return Concurrency.Bind(m(x),function(a$1)
    {
     return Concurrency.Return(new Result({
      $:0,
      $0:a$1
     }));
    });
   }))):Concurrency.Return(new Result({
    $:1,
    $0:a.$0
   }));
  },p);
 };
 Piglet.MapToAsyncResult=function(m,p)
 {
  return Piglet.MapAsyncResult(function(a)
  {
   return a.$==0?m(a.$0):Concurrency.Return(new Result({
    $:1,
    $0:a.$0
   }));
  },p);
 };
 Piglet.MapAsyncResult=function(m,p)
 {
  var out,b;
  out=new Stream.New(new Result({
   $:1,
   $0:List.T.Empty
  }),null);
  p.stream.Subscribe(function(v)
  {
   var b$1;
   Concurrency.Start((b$1=null,Concurrency.Delay(function()
   {
    return Concurrency.Bind(m(v),function(a)
    {
     out.Trigger(a);
     return Concurrency.Return(null);
    });
   })),null);
  });
  Concurrency.Start((b=null,Concurrency.Delay(function()
  {
   return Concurrency.Bind(m(p.stream.get_Latest()),function(a)
   {
    out.Trigger(a);
    return Concurrency.Return(null);
   });
  })),null);
  return Piglet.New(out,p.view);
 };
 Piglet.Map=function(m,p)
 {
  return Piglet.MapResult(function(a)
  {
   return a.$==0?new Result({
    $:0,
    $0:m(a.$0)
   }):new Result({
    $:1,
    $0:a.$0
   });
  },p);
 };
 Piglet.MapToResult=function(m,p)
 {
  return Piglet.MapResult(function(a)
  {
   return a.$==0?m(a.$0):new Result({
    $:1,
    $0:a.$0
   });
  },p);
 };
 Piglet.MapResult=function(m,p)
 {
  var out;
  function f(a)
  {
   out.Trigger(a);
  }
  out=new Stream.New(m(p.stream.get_Latest()),null);
  p.stream.Subscribe(function(x)
  {
   return f(m(x));
  });
  return Piglet.New(out,p.view);
 };
 Piglet.TransmitWriter=function(p)
 {
  var a;
  function v($1,$2)
  {
   return(p.view($1))($2);
  }
  return Piglet.New(p.stream,(a=p.stream,function(x)
  {
   return v(x,a);
  }));
 };
 Piglet.TransmitReader=function(p)
 {
  var a;
  function v($1,$2)
  {
   return(p.view($1))($2);
  }
  return Piglet.New(p.stream,(a=p.stream,function(x)
  {
   return v(x,a);
  }));
 };
 Piglet.TransmitReaderMap=function(f,p)
 {
  var a;
  function v($1,$2)
  {
   return(p.view($1))($2);
  }
  return Piglet.New(p.stream,(a=Reader.Map(f,p.stream),function(x)
  {
   return v(x,a);
  }));
 };
 Piglet.TransmitReaderMapToResult=function(f,p)
 {
  var a;
  function v($1,$2)
  {
   return(p.view($1))($2);
  }
  return Piglet.New(p.stream,(a=Reader.MapToResult(f,p.stream),function(x)
  {
   return v(x,a);
  }));
 };
 Piglet.TransmitReaderMapResult=function(f,p)
 {
  var a;
  function v($1,$2)
  {
   return(p.view($1))($2);
  }
  return Piglet.New(p.stream,(a=Reader.MapResult(f,p.stream),function(x)
  {
   return v(x,a);
  }));
 };
 Piglet.TransmitStream=function(p)
 {
  var a;
  function v($1,$2)
  {
   return(p.view($1))($2);
  }
  return Piglet.New(p.stream,(a=p.stream,function(x)
  {
   return v(x,a);
  }));
 };
 Piglet.Many=function(init,p)
 {
  return Piglet.ManyInit([init],init,p);
 };
 Piglet.ManyInit=function(inits,init,p)
 {
  var s,m;
  s=new Stream.New(new Result({
   $:0,
   $0:inits
  }),null);
  m=new UnitStream.New(p,s,p(init),init);
  return Piglet.New(s,function(f)
  {
   return f(m);
  });
 };
 Piglet.ManyPiglet=function(inits,create,p)
 {
  var s,m;
  s=new Stream.New(new Result({
   $:0,
   $0:inits
  }),null);
  m=new Stream$1.New(p,s,create);
  return Piglet.New(s,function(f)
  {
   return f(m);
  });
 };
 Piglet.Choose=function(chooser,choices)
 {
  var s,c;
  s=new Stream.New(new Result({
   $:1,
   $0:List.T.Empty
  }),null);
  c=new Stream$2.New(chooser,choices,s);
  return Piglet.New(s,function(f)
  {
   return f(c);
  });
 };
 Piglet.WithSubmitClearError=function(pin)
 {
  var submitter;
  function v($1,$2)
  {
   return(pin.view($1))($2);
  }
  submitter=new Submitter.New(pin.stream,true);
  return Piglet.New(submitter.get_Output(),function(x)
  {
   return v(x,submitter);
  });
 };
 Piglet.WithSubmit=function(pin)
 {
  var submitter;
  function v($1,$2)
  {
   return(pin.view($1))($2);
  }
  submitter=new Submitter.New(pin.stream,false);
  return Piglet.New(submitter.get_Output(),function(x)
  {
   return v(x,submitter);
  });
 };
 Piglet.ReturnFailure=function()
 {
  return Piglet.New(new Stream.New(new Result({
   $:1,
   $0:List.T.Empty
  }),null),Global.id);
 };
 Piglet.Return=function(x)
 {
  return Piglet.New(new Stream.New(new Result({
   $:0,
   $0:x
  }),null),Global.id);
 };
 Piglet.YieldFailure=function()
 {
  var s;
  s=new Stream.New(new Result({
   $:1,
   $0:List.T.Empty
  }),null);
  return Piglet.New(s,function(f)
  {
   return f(s);
  });
 };
 Piglet.Yield=function(x)
 {
  var s;
  s=new Stream.New(new Result({
   $:0,
   $0:x
  }),null);
  return Piglet.New(s,function(f)
  {
   return f(s);
  });
 };
 SC$1.$cctor=function()
 {
  var current;
  SC$1.$cctor=Global.ignore;
  SC$1.next=(current=[0],function()
  {
   current[0]++;
   return current[0];
  });
 };
 HtmlContainer=Controls.HtmlContainer=Runtime.Class({
  WebSharper_Piglets_Container_2$get_Container:function()
  {
   return this.container;
  },
  WebSharper_Piglets_Container_2$MoveUp:function(i)
  {
   var elt_i,elt_i_1;
   elt_i=this.container.get_Body().childNodes[i];
   elt_i_1=this.container.get_Body().childNodes[i-1];
   this.container.get_Body().removeChild(elt_i);
   this.container.get_Body().insertBefore(elt_i,elt_i_1);
  },
  WebSharper_Piglets_Container_2$Remove:function(i)
  {
   this.container.get_Body().removeChild(this.container.get_Body().childNodes[i]);
  },
  WebSharper_Piglets_Container_2$Add:function(elt)
  {
   this.container.AppendI(elt);
  }
 },WebSharper.Obj,HtmlContainer);
 HtmlContainer.New=Runtime.Ctor(function(container)
 {
  this.container=container;
 },HtmlContainer);
 Controls.CssResult=function(reader,attrName,render,element)
 {
  function f(element$1)
  {
   function set(x)
   {
    var a;
    a=render(x);
    element$1.HtmlProvider.SetCss(element$1.get_Body(),attrName,a);
   }
   set(reader.get_Latest());
   reader.Subscribe(set);
  }
  (function(w)
  {
   Operators$1.OnAfterRender(f,w);
  }(element));
  return element;
 };
 Controls.Css=function(reader,attrName,render,element)
 {
  function f(element$1)
  {
   function set(x)
   {
    var a;
    if(x.$==0)
     {
      a=render(x.$0);
      element$1.HtmlProvider.SetCss(element$1.get_Body(),attrName,a);
     }
   }
   set(reader.get_Latest());
   reader.Subscribe(set);
  }
  (function(w)
  {
   Operators$1.OnAfterRender(f,w);
  }(element));
  return element;
 };
 Controls.AttrResult=function(reader,attrName,render,element)
 {
  function f(element$1)
  {
   function set(x)
   {
    var a;
    a=render(x);
    element$1.HtmlProvider.SetAttribute(element$1.get_Body(),attrName,a);
   }
   set(reader.get_Latest());
   reader.Subscribe(set);
  }
  (function(w)
  {
   Operators$1.OnAfterRender(f,w);
  }(element));
  return element;
 };
 Controls.Attr=function(reader,attrName,render,element)
 {
  function f(element$1)
  {
   function set(x)
   {
    var a;
    if(x.$==0)
     {
      a=render(x.$0);
      element$1.HtmlProvider.SetAttribute(element$1.get_Body(),attrName,a);
     }
   }
   set(reader.get_Latest());
   reader.Subscribe(set);
  }
  (function(w)
  {
   Operators$1.OnAfterRender(f,w);
  }(element));
  return element;
 };
 Controls.Link=function(submit)
 {
  var x,a;
  function f(e)
  {
   Global.jQuery(e.get_Body()).on("click",function(ev)
   {
    submit.WebSharper_Piglets_Writer_1$Trigger(new Result({
     $:0,
     $0:null
    }));
    return ev.preventDefault();
   });
  }
  x=(a=[Attr.Attr().NewAttr("href","#")],Tags.Tags().NewTag("a",a));
  (function(w)
  {
   Operators$1.OnAfterRender(f,w);
  }(x));
  return x;
 };
 Controls.ButtonValidate=function(submit)
 {
  var x;
  x=Controls.Button(submit);
  return Controls.EnableOnSuccess(submit.get_Input(),x);
 };
 Controls.Button=function(submit)
 {
  var x;
  function a(a$1,a$2)
  {
   return submit.WebSharper_Piglets_Writer_1$Trigger(new Result({
    $:0,
    $0:null
   }));
  }
  x=Tags.Tags().NewTag("button",[]);
  (function(a$1)
  {
   EventsPervasives.Events().OnClick(function($1)
   {
    return function($2)
    {
     return a($1,$2);
    };
   },a$1);
  }(x));
  return x;
 };
 Controls.SubmitValidate=function(submit)
 {
  var x;
  x=Controls.Submit(submit);
  return Controls.EnableOnSuccess(submit.get_Input(),x);
 };
 Controls.Submit=function(submit)
 {
  var x;
  function a(a$1,a$2)
  {
   return submit.WebSharper_Piglets_Writer_1$Trigger(new Result({
    $:0,
    $0:null
   }));
  }
  x=Tags.Tags().NewTag("input",[Attr.Attr().NewAttr("type","submit")]);
  (function(a$1)
  {
   EventsPervasives.Events().OnClick(function($1)
   {
    return function($2)
    {
     return a($1,$2);
    };
   },a$1);
  }(x));
  return x;
 };
 Controls.EnableOnSuccess=function(reader,element)
 {
  function f(el)
  {
   el.get_Body().disabled=!reader.get_Latest().get_isSuccess();
   reader.Subscribe(function(x)
   {
    el.get_Body().disabled=!x.get_isSuccess();
   });
  }
  (function(w)
  {
   Operators$1.OnAfterRender(f,w);
  }(element));
  return element;
 };
 Controls.ShowErrors=function(reader,render,container)
 {
  return Controls.ShowResult(reader,function(a)
  {
   return a.$==1?render(List.map(function(m)
   {
    return m.get_Message();
   },a.$0)):[];
  },container);
 };
 Controls.ShowString=function(reader,render,container)
 {
  return Controls.Show(reader,function(x)
  {
   var x$1;
   return List.ofArray([(x$1=render(x),Tags.Tags().text(x$1))]);
  },container);
 };
 Controls.Show=function(reader,render,container)
 {
  return Controls.ShowResult(reader,function(a)
  {
   return a.$==1?[]:render(a.$0);
  },container);
 };
 Controls.ShowResult=function(reader,render,container)
 {
  var i,e;
  i=render(reader.get_Latest());
  e=Enumerator.Get(i);
  try
  {
   while(e.MoveNext())
    container.AppendI(e.Current());
  }
  finally
  {
   if("Dispose"in e)
    e.Dispose();
  }
  reader.Subscribe(function(x)
  {
   var i$1,e$1;
   container.HtmlProvider.Clear(container.get_Body());
   i$1=render(x);
   e$1=Enumerator.Get(i$1);
   try
   {
    while(e$1.MoveNext())
     container.AppendI(e$1.Current());
   }
   finally
   {
    if("Dispose"in e$1)
     e$1.Dispose();
   }
  });
  return container;
 };
 Controls.Container=function(c)
 {
  return new HtmlContainer.New(c);
 };
 Controls.RenderChoice=function(choice,renderIt,container)
 {
  return choice.Choice(new HtmlContainer.New(container),renderIt);
 };
 Controls.RenderMany=function(many,renderOne,container)
 {
  return many.Render(new HtmlContainer.New(container),renderOne);
 };
 Controls.Select=function(stream,values)
 {
  var values$1,elts,x,x$1;
  function m(x$3,label)
  {
   var id;
   id=(Controls.nextId())();
   return Operators$1.add(Tags.Tags().NewTag("option",[Attr.Attr().NewAttr("value",id)]),[Tags.Tags().text(label)]);
  }
  function x$2(e)
  {
   if(e.get_Body().selectedIndex>=0)
    stream.Trigger(new Result({
     $:0,
     $0:(Arrays.get(values$1,e.get_Body().selectedIndex))[0]
    }));
  }
  function a(el,a$1)
  {
   return x$2(el);
  }
  function f(div)
  {
   stream.Subscribe(function(a$1)
   {
    var v,m$1,_this;
    if(a$1.$==1)
     ;
    else
     {
      v=a$1.$0;
      m$1=Arrays.tryFindIndex(function(t)
      {
       return Unchecked.Equals(v,t[0]);
      },values$1);
      m$1==null?void 0:(_this=Arrays.get(elts,m$1.$0),_this.HtmlProvider.SetAttribute(_this.get_Body(),"selected",""));
     }
   });
  }
  (Controls.nextId())();
  values$1=Arrays.ofSeq(values);
  elts=Arrays.map(function($1)
  {
   return m($1[0],$1[1]);
  },values$1);
  x=(x$1=Tags.Tags().NewTag("select",elts),(function(a$1)
  {
   EventsPervasives.Events().OnChange(function($1)
   {
    return function($2)
    {
     return a($1,$2);
    };
   },a$1);
  }(x$1),x$1));
  (function(w)
  {
   Operators$1.OnAfterRender(f,w);
  }(x));
  return x;
 };
 Controls.RadioLabelled=function(stream,values)
 {
  var x,y;
  function m(a,label)
  {
   return function(input)
   {
    var id,a$1,a$2;
    id=(Controls.nextId())();
    a$1=[Operators$1.add(input,[Attr.Attr().NewAttr("id",id)]),(a$2=[Attr.Attr().NewAttr("for",id),Tags.Tags().text(label)],Tags.Tags().NewTag("label",a$2))];
    return Tags.Tags().NewTag("span",a$1);
   };
  }
  x=(y=Controls.Radio(stream,Seq.map(function(t)
  {
   return t[0];
  },values)),(((Runtime.Curried3(Seq.map2))(function($1,$2)
  {
   return(function($3)
   {
    return m($3[0],$3[1]);
   }($1))($2);
  }))(values))(y));
  return Tags.Tags().NewTag("div",x);
 };
 Controls.Radio=function(stream,values)
 {
  var name,values$1,elts;
  function set(a)
  {
   var v;
   function a$1(x,input)
   {
    input.get_Body().checked=Unchecked.Equals(x,v);
   }
   if(a.$==1)
    ;
   else
    {
     v=a.$0;
     (((Runtime.Curried3(List.iter2))(a$1))(values$1))(elts);
    }
  }
  name=(Controls.nextId())();
  values$1=List.ofSeq(values);
  elts=List.map(function(x)
  {
   var x$1;
   function x$2(div)
   {
    if(div.get_Body().checked)
     stream.Trigger(new Result({
      $:0,
      $0:x
     }));
   }
   function a(el,a$1)
   {
    return x$2(el);
   }
   x$1=Tags.Tags().NewTag("input",[Attr.Attr().NewAttr("type","radio"),Attr.Attr().NewAttr("name",name)]);
   (function(a$1)
   {
    EventsPervasives.Events().OnChange(function($1)
    {
     return function($2)
     {
      return a($1,$2);
     };
    },a$1);
   }(x$1));
   return x$1;
  },values$1);
  set(stream.get_Latest());
  stream.Subscribe(set);
  return elts;
 };
 Controls.CheckBox=function(stream)
 {
  var id,i,m;
  id=(Controls.nextId())();
  i=Tags.Tags().NewTag("input",[Attr.Attr().NewAttr("type","checkbox"),Attr.Attr().NewAttr("id",id)]);
  m=stream.get_Latest();
  m.$==0?i.get_Body().checked=m.$0:void 0;
  stream.Subscribe(function(a)
  {
   var x;
   if(a.$==1)
    ;
   else
    {
     x=a.$0;
     !Unchecked.Equals(i.get_Body().checked,x)?i.get_Body().checked=x:void 0;
    }
  });
  i.get_Body().addEventListener("change",function()
  {
   stream.Trigger(new Result({
    $:0,
    $0:i.get_Body().checked
   }));
  },true);
  return i;
 };
 Controls.TextArea=function(stream)
 {
  var i,m;
  function ev(a)
  {
   stream.Trigger(new Result({
    $:0,
    $0:i.get_Value()
   }));
  }
  i=Tags.Tags().NewTag("textarea",[]);
  m=stream.get_Latest();
  m.$==0?i.set_Value(m.$0):void 0;
  stream.Subscribe(function(a)
  {
   var x;
   if(a.$==1)
    ;
   else
    {
     x=a.$0;
     i.get_Value()!==x?i.set_Value(x):void 0;
    }
  });
  i.get_Body().addEventListener("keyup",ev,true);
  i.get_Body().addEventListener("change",ev,true);
  return i;
 };
 Controls.IntInput=function(stream)
 {
  return Controls.input("number",Global.Number,Global.String,stream);
 };
 Controls.WithLabelAfter=function(label,element)
 {
  var id,a,a$1;
  id=(Controls.nextId())();
  a=[Operators$1.add(element,[Attr.Attr().NewAttr("id",id)]),(a$1=[Attr.Attr().NewAttr("for",id),Tags.Tags().text(label)],Tags.Tags().NewTag("label",a$1))];
  return Tags.Tags().NewTag("span",a);
 };
 Controls.WithLabel=function(label,element)
 {
  var id,a,a$1;
  id=(Controls.nextId())();
  a=[(a$1=[Attr.Attr().NewAttr("for",id),Tags.Tags().text(label)],Tags.Tags().NewTag("label",a$1)),Operators$1.add(element,[Attr.Attr().NewAttr("id",id)])];
  return Tags.Tags().NewTag("span",a);
 };
 Controls.input=function(type,ofString,toString,stream)
 {
  var i,m;
  function ev(a)
  {
   var v;
   v=new Result({
    $:0,
    $0:ofString(i.get_Value())
   });
   !Unchecked.Equals(v,stream.get_Latest())?stream.Trigger(v):void 0;
  }
  i=Tags.Tags().NewTag("input",[Attr.Attr().NewAttr("type",type)]);
  m=stream.get_Latest();
  m.$==0?i.set_Value(toString(m.$0)):void 0;
  stream.Subscribe(function(a)
  {
   var s;
   if(a.$==1)
    ;
   else
    {
     s=toString(a.$0);
     i.get_Value()!==s?i.set_Value(s):void 0;
    }
  });
  i.get_Body().addEventListener("keyup",ev,true);
  i.get_Body().addEventListener("change",ev,true);
  return i;
 };
 Controls.nextId=function()
 {
  SC$2.$cctor();
  return SC$2.nextId;
 };
 SC$2.$cctor=function()
 {
  var current;
  SC$2.$cctor=Global.ignore;
  SC$2.nextId=(current=[0],function()
  {
   current[0]++;
   return"pl__"+Global.String(current[0]);
  });
 };
}());
