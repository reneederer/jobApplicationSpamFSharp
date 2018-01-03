(function()
{
 "use strict";
 var Global,IntelliFactory,Formlets,Base,Tree,Tree$1,Edit,WebSharper,Obj,D,LayoutUtils,Result,Form,Formlet,FormletProvider,FormletBuilder,Validator,Runtime,Seq,Enumerator,List,Util,Unchecked;
 Global=window;
 IntelliFactory=Global.IntelliFactory=Global.IntelliFactory||{};
 Formlets=IntelliFactory.Formlets=IntelliFactory.Formlets||{};
 Base=Formlets.Base=Formlets.Base||{};
 Tree=Base.Tree=Base.Tree||{};
 Tree$1=Tree.Tree=Tree.Tree||{};
 Edit=Tree.Edit=Tree.Edit||{};
 WebSharper=Global.WebSharper;
 Obj=WebSharper&&WebSharper.Obj;
 D=Base.D=Base.D||{};
 LayoutUtils=Base.LayoutUtils=Base.LayoutUtils||{};
 Result=Base.Result=Base.Result||{};
 Form=Base.Form=Base.Form||{};
 Formlet=Base.Formlet=Base.Formlet||{};
 FormletProvider=Base.FormletProvider=Base.FormletProvider||{};
 FormletBuilder=Base.FormletBuilder=Base.FormletBuilder||{};
 Validator=Base.Validator=Base.Validator||{};
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 Seq=WebSharper&&WebSharper.Seq;
 Enumerator=WebSharper&&WebSharper.Enumerator;
 List=WebSharper&&WebSharper.List;
 Util=WebSharper&&WebSharper.Util;
 Unchecked=WebSharper&&WebSharper.Unchecked;
 Tree$1=Tree.Tree=Runtime.Class({
  Map:function(f)
  {
   return this.$==1?new Tree$1({
    $:1,
    $0:f(this.$0)
   }):this.$==2?new Tree$1({
    $:2,
    $0:this.$0.Map(f),
    $1:this.$1.Map(f)
   }):Tree$1.Empty;
  },
  get_Sequence:function()
  {
   return this.$==1?[this.$0]:this.$==2?Seq.append(this.$0.get_Sequence(),this.$1.get_Sequence()):[];
  },
  GetEnumerator:function()
  {
   return Enumerator.Get(this.get_Sequence());
  },
  GetEnumerator0:function()
  {
   return Enumerator.Get(this.get_Sequence());
  }
 },null,Tree$1);
 Tree$1.Empty=new Tree$1({
  $:0
 });
 Edit=Tree.Edit=Runtime.Class({
  get_Sequence:function()
  {
   return this.$==1?this.$0.get_Sequence():this.$==2?this.$0.get_Sequence():this.$0.get_Sequence();
  },
  GetEnumerator:function()
  {
   return Enumerator.Get(this.get_Sequence());
  },
  GetEnumerator0:function()
  {
   return Enumerator.Get(this.get_Sequence());
  }
 },null,Edit);
 Tree.DeepFlipEdit=function(edit)
 {
  return edit.$==1?new Edit({
   $:2,
   $0:Tree.DeepFlipEdit(edit.$0)
  }):edit.$==2?new Edit({
   $:1,
   $0:Tree.DeepFlipEdit(edit.$0)
  }):new Edit({
   $:0,
   $0:edit.$0
  });
 };
 Tree.FlipEdit=function(edit)
 {
  return edit.$==1?new Edit({
   $:2,
   $0:edit.$0
  }):edit.$==2?new Edit({
   $:1,
   $0:edit.$0
  }):new Edit({
   $:0,
   $0:edit.$0
  });
 };
 Tree.Delete=function()
 {
  return new Edit({
   $:0,
   $0:Tree$1.Empty
  });
 };
 Tree.Transform=function(f,edit)
 {
  return edit.$==1?new Edit({
   $:1,
   $0:Tree.Transform(f,edit.$0)
  }):edit.$==2?new Edit({
   $:2,
   $0:Tree.Transform(f,edit.$0)
  }):new Edit({
   $:0,
   $0:f(edit.$0)
  });
 };
 Tree.Set=function(value)
 {
  return new Edit({
   $:0,
   $0:new Tree$1({
    $:1,
    $0:value
   })
  });
 };
 Tree.Apply=function(edit,input)
 {
  function apply(edit$1,input$1)
  {
   return edit$1.$==1?input$1.$==2?new Tree$1({
    $:2,
    $0:apply(edit$1.$0,input$1.$0),
    $1:input$1.$1
   }):apply(new Edit({
    $:1,
    $0:edit$1.$0
   }),new Tree$1({
    $:2,
    $0:Tree$1.Empty,
    $1:input$1
   })):edit$1.$==2?input$1.$==2?new Tree$1({
    $:2,
    $0:input$1.$0,
    $1:apply(edit$1.$0,input$1.$1)
   }):apply(new Edit({
    $:2,
    $0:edit$1.$0
   }),new Tree$1({
    $:2,
    $0:input$1,
    $1:Tree$1.Empty
   })):edit$1.$0;
  }
  return apply(edit,input);
 };
 Tree.ReplacedTree=function(edit,input)
 {
  var edit$1,l,tree,edit$2,r,tree$1;
  while(true)
   if(edit.$==1)
    {
     edit$1=edit.$0;
     if(input.$==2)
      {
       l=input.$0;
       edit=edit$1;
       input=l;
      }
     else
      {
       tree=input;
       edit=new Edit({
        $:1,
        $0:edit$1
       });
       input=new Tree$1({
        $:2,
        $0:Tree$1.Empty,
        $1:tree
       });
      }
    }
   else
    if(edit.$==2)
     {
      edit$2=edit.$0;
      if(input.$==2)
       {
        r=input.$1;
        edit=edit$2;
        input=r;
       }
      else
       {
        tree$1=input;
        edit=new Edit({
         $:2,
         $0:edit$2
        });
        input=new Tree$1({
         $:2,
         $0:tree$1,
         $1:Tree$1.Empty
        });
       }
     }
    else
     return input;
 };
 Tree.FromSequence=function(vs)
 {
  function f(state,v)
  {
   return new Tree$1({
    $:2,
    $0:state,
    $1:new Tree$1({
     $:1,
     $0:v
    })
   });
  }
  return(((Runtime.Curried3(Seq.fold))(f))(Tree$1.Empty))(vs);
 };
 Tree.Range=function(edit,input)
 {
  var edit$1,input$1,offset,edit$2,l,edit$3,r,l$1,tree;
  edit$1=edit;
  input$1=input;
  offset=0;
  while(true)
   if(edit$1.$==1)
    {
     edit$2=edit$1.$0;
     if(input$1.$==2)
      {
       l=input$1.$0;
       edit$1=edit$2;
       input$1=l;
      }
     else
      {
       edit$1=edit$2;
       input$1=Tree$1.Empty;
      }
    }
   else
    if(edit$1.$==2)
     {
      edit$3=edit$1.$0;
      if(input$1.$==2)
       {
        r=input$1.$1;
        l$1=input$1.$0;
        edit$1=edit$3;
        input$1=r;
        offset=offset+Tree.Count(l$1);
       }
      else
       {
        tree=input$1;
        edit$1=edit$3;
        input$1=Tree$1.Empty;
        offset=offset+Tree.Count(tree);
       }
     }
    else
     return[offset,Tree.Count(input$1)];
 };
 Tree.Count=function(t)
 {
  var n,t$1,a,b,a$1,tree,k,ts,t$2;
  n=0;
  t$1=List.T.Empty;
  a=t;
  while(true)
   if(a.$==2)
    {
     b=a.$1;
     a$1=a.$0;
     t$1=new List.T({
      $:1,
      $0:b,
      $1:t$1
     });
     a=a$1;
    }
   else
    {
     tree=a;
     k=tree.$==0?0:1;
     if(t$1.$==1)
      {
       ts=t$1.$1;
       t$2=t$1.$0;
       n=n+k;
       t$1=ts;
       a=t$2;
      }
     else
      return n+k;
    }
 };
 Tree.ShowEdit=function(edit)
 {
  function showE(edit$1)
  {
   return edit$1.$==1?"Left > "+showE(edit$1.$0):edit$1.$==2?"Right > "+showE(edit$1.$0):"Replace";
  }
  return showE(edit);
 };
 D=Base.D=Runtime.Class({
  Dispose:Global.ignore
 },Obj,D);
 D.New=Runtime.Ctor(function()
 {
 },D);
 LayoutUtils=Base.LayoutUtils=Runtime.Class({
  New:function(container)
  {
   return{
    Apply:function(event)
    {
     var panel,tree;
     panel=container();
     tree=[Tree$1.Empty];
     return{
      $:1,
      $0:[panel.Body,event.Subscribe(Util.observer(function(edit)
      {
       var deletedTree,off;
       deletedTree=Tree.ReplacedTree(edit,tree[0]);
       tree[0]=Tree.Apply(edit,tree[0]);
       off=(Tree.Range(edit,tree[0]))[0];
       panel.Remove(deletedTree.get_Sequence());
       Seq.iteri(function(i,e)
       {
        return(panel.Insert(off+i))(e);
       },edit);
      }))]
     };
    }
   };
  },
  Delay:function(f)
  {
   return{
    Apply:function(x)
    {
     return f().Apply(x);
    }
   };
  },
  Default:function()
  {
   return{
    Apply:function()
    {
     return null;
    }
   };
  }
 },Obj,LayoutUtils);
 LayoutUtils.New=Runtime.Ctor(function(R)
 {
 },LayoutUtils);
 Result.Sequence=function(rs)
 {
  return Seq.fold(function(rs$1,r)
  {
   return rs$1.$==1?r.$==1?{
    $:1,
    $0:List.append(rs$1.$0,r.$0)
   }:{
    $:1,
    $0:rs$1.$0
   }:r.$==1?{
    $:1,
    $0:r.$0
   }:{
    $:0,
    $0:List.append(rs$1.$0,List.ofArray([r.$0]))
   };
  },{
   $:0,
   $0:List.T.Empty
  },rs);
 };
 Result.Map=function(f,res)
 {
  return res.$==1?{
   $:1,
   $0:res.$0
  }:{
   $:0,
   $0:f(res.$0)
  };
 };
 Result.OfOption=function(o)
 {
  return o==null?{
   $:1,
   $0:List.T.Empty
  }:{
   $:0,
   $0:o.$0
  };
 };
 Result.Apply=function(f,r)
 {
  return f.$==1?r.$==1?{
   $:1,
   $0:List.append(f.$0,r.$0)
  }:{
   $:1,
   $0:f.$0
  }:r.$==1?{
   $:1,
   $0:r.$0
  }:{
   $:0,
   $0:f.$0(r.$0)
  };
 };
 Result.Join=function(res)
 {
  return res.$==1?{
   $:1,
   $0:res.$0
  }:res.$0;
 };
 Form=Base.Form=Runtime.Class({
  Dispose:function()
  {
   this.Dispose$1();
  }
 },null,Form);
 Form.New=function(Body,Dispose,Notify,State)
 {
  return new Form({
   Body:Body,
   Dispose:Dispose,
   Notify:Notify,
   State:State
  });
 };
 Formlet=Base.Formlet=Runtime.Class({
  MapResultI:function(f)
  {
   var $this;
   $this=this;
   return Formlet.New(this.Layout,function()
   {
    var form;
    form=$this.Build();
    $this.Utils.Reactive.Select(form.State,f);
    return Form.New(form.Body,form.Dispose$1,form.Notify,form.State);
   },this.Utils);
  },
  BuildI:function()
  {
   return this.Build();
  },
  LayoutI:function()
  {
   return this.Layout;
  }
 },null,Formlet);
 Formlet.New=function(Layout,Build,Utils)
 {
  return new Formlet({
   Layout:Layout,
   Build:Build,
   Utils:Utils
  });
 };
 FormletProvider=Base.FormletProvider=Runtime.Class({
  BindWith:function(hF,formlet,f)
  {
   var $this;
   $this=this;
   return $this.New(function()
   {
    var form,$1,$2,$3;
    form=$this.Bind(formlet,f).BuildI();
    return Form.New(($1=$this.U.DefaultLayout.Apply($this.U.Reactive.Where(form.Body,function(edit)
    {
     return edit.$==1&&true;
    })),($2=$this.U.DefaultLayout.Apply($this.U.Reactive.Where(form.Body,function(edit)
    {
     return edit.$==2&&true;
    })),$1!=null&&$1.$==1&&($2!=null&&$2.$==1&&($3=[$1.$0[0],$2.$0[0]],true))?$this.U.Reactive.Return(Tree.Set(hF($3[0],$3[1]))):$this.U.Reactive.Never())),form.Dispose$1,form.Notify,form.State);
   });
  },
  WithNotification:function(notify,formlet)
  {
   var $this,x;
   $this=this;
   x=$this.New(function()
   {
    var form;
    form=$this.BuildForm(formlet);
    return Form.New(form.Body,form.Dispose$1,function(obj)
    {
     form.Notify(obj);
     notify(obj);
    },form.State);
   });
   return $this.WithLayout(formlet.LayoutI(),x);
  },
  LiftResult:function(formlet)
  {
   return this.MapResult(function(a)
   {
    return{
     $:0,
     $0:a
    };
   },formlet);
  },
  Sequence:function(fs)
  {
   var fs$1,fComp,a;
   fs$1=List.ofSeq(fs);
   return fs$1.$==1?(fComp=this.Return(function(v)
   {
    return function(vs)
    {
     return new List.T({
      $:1,
      $0:v,
      $1:vs
     });
    };
   }),(a=this.Sequence(fs$1.$1),this.Apply(this.Apply(fComp,fs$1.$0),a))):this.Return(List.T.Empty);
  },
  Delay:function(f)
  {
   var $this;
   $this=this;
   return Formlet.New(this.L.Delay(function()
   {
    return f().LayoutI();
   }),function()
   {
    return $this.BuildForm(f());
   },this.U);
  },
  Bind:function(formlet,f)
  {
   var $this;
   $this=this;
   return $this.Join($this.Map(f,formlet));
  },
  WithCancelation:function(formlet,cancelFormlet)
  {
   var f1,f2,f3;
   function compose(r1,r2)
   {
    return r2.$==0?{
     $:0,
     $0:null
    }:r1.$==1?{
     $:1,
     $0:r1.$0
    }:{
     $:0,
     $0:{
      $:1,
      $0:r1.$0
     }
    };
   }
   f1=this.Return(function($1)
   {
    return function($2)
    {
     return compose($1,$2);
    };
   });
   f2=this.LiftResult(formlet);
   f3=this.LiftResult(cancelFormlet);
   return this.MapResult(Result.Join,this.Apply(this.Apply(f1,f2),f3));
  },
  Deletable:function(formlet)
  {
   var $this;
   $this=this;
   return this.Replace(formlet,function(value)
   {
    return value!=null&&value.$==1?$this.Return({
     $:1,
     $0:value.$0
    }):$this.ReturnEmpty(null);
   });
  },
  Replace:function(formlet,f)
  {
   var $this;
   $this=this;
   return $this.Switch($this.Map(f,formlet));
  },
  WithNotificationChannel:function(formlet)
  {
   var $this,x;
   $this=this;
   x=$this.New(function()
   {
    var form;
    function a(v)
    {
     return[v,form.Notify];
    }
    form=formlet.BuildI();
    return Form.New(form.Body,form.Dispose$1,form.Notify,$this.U.Reactive.Select(form.State,function(a$1)
    {
     return Result.Map(a,a$1);
    }));
   });
   return $this.WithLayout(formlet.LayoutI(),x);
  },
  SelectMany:function(formlet)
  {
   var $this;
   $this=this;
   return $this.New(function()
   {
    var form1,formStream,tag;
    function incrTag()
    {
     var g;
     function f(a)
     {
      return new Edit({
       $:2,
       $0:a
      });
     }
     tag[0]=(g=tag[0],function(x)
     {
      return f(g(x));
     });
    }
    form1=$this.BuildForm(formlet);
    formStream=$this.U.Reactive.Heat($this.U.Reactive.Choose(form1.State,function(res)
    {
     return res.$==1?null:{
      $:1,
      $0:$this.BuildForm(res.$0)
     };
    }));
    return Form.New($this.U.Reactive.Merge($this.U.Reactive.Select(form1.Body,function(a)
    {
     return new Edit({
      $:1,
      $0:a
     });
    }),(tag=[function(a)
    {
     return new Edit({
      $:1,
      $0:a
     });
    }],$this.U.Reactive.SelectMany($this.U.Reactive.Select(formStream,function(f)
    {
     incrTag();
     return $this.U.Reactive.Select(f.Body,tag[0]);
    })))),function()
    {
     form1.Dispose$1();
    },function(o)
    {
     form1.Notify(o);
    },$this.U.Reactive.Select($this.U.Reactive.CollectLatest($this.U.Reactive.Select(formStream,function(f)
    {
     return f.State;
    })),Result.Sequence));
   });
  },
  FlipBody:function(formlet)
  {
   var $this,x;
   $this=this;
   x=$this.New(function()
   {
    var form;
    form=formlet.BuildI();
    return Form.New($this.U.Reactive.Select(form.Body,Tree.FlipEdit),form.Dispose$1,form.Notify,form.State);
   });
   return $this.WithLayout(formlet.LayoutI(),x);
  },
  Switch:function(formlet)
  {
   var $this;
   $this=this;
   return $this.New(function()
   {
    var form1,formStream;
    form1=$this.BuildForm($this.ApplyLayout($this.WithLayoutOrDefault(formlet)));
    formStream=$this.U.Reactive.Heat($this.U.Reactive.Choose(form1.State,function(res)
    {
     return res.$==1?null:{
      $:1,
      $0:$this.BuildForm(res.$0)
     };
    }));
    return Form.New($this.U.Reactive.Concat(form1.Body,$this.U.Reactive.Switch($this.U.Reactive.Select(formStream,function(f)
    {
     return f.Body;
    }))),function()
    {
     form1.Dispose$1();
    },function(o)
    {
     form1.Notify(o);
    },$this.U.Reactive.Switch($this.U.Reactive.Select(formStream,function(f)
    {
     return f.State;
    })));
   });
  },
  Join:function(formlet)
  {
   var $this;
   $this=this;
   return $this.New(function()
   {
    var form1,formStream,a;
    form1=$this.BuildForm(formlet);
    formStream=$this.U.Reactive.Heat($this.U.Reactive.Select(form1.State,function(res)
    {
     return res.$==1?$this.Fail(res.$0):$this.BuildForm(res.$0);
    }));
    return Form.New((a=$this.U.Reactive.Select($this.U.Reactive.Switch($this.U.Reactive.Select(formStream,function(f)
    {
     return $this.U.Reactive.Concat($this.U.Reactive.Return(Tree.Delete()),f.Body);
    })),function(a$1)
    {
     return new Edit({
      $:2,
      $0:a$1
     });
    }),$this.U.Reactive.Merge($this.U.Reactive.Select(form1.Body,function(a$1)
    {
     return new Edit({
      $:1,
      $0:a$1
     });
    }),a)),function()
    {
     form1.Dispose$1();
    },function(o)
    {
     form1.Notify(o);
    },$this.U.Reactive.Switch($this.U.Reactive.Select(formStream,function(f)
    {
     return f.State;
    })));
   });
  },
  EmptyForm:function()
  {
   return Form.New(this.U.Reactive.Never(),Global.ignore,Global.ignore,this.U.Reactive.Never());
  },
  Empty:function()
  {
   var $this;
   $this=this;
   return $this.New(function()
   {
    return Form.New($this.U.Reactive.Return(Tree.Delete()),Global.ignore,Global.ignore,$this.U.Reactive.Never());
   });
  },
  Never:function()
  {
   var $this;
   $this=this;
   return $this.New(function()
   {
    return Form.New($this.U.Reactive.Never(),Global.ignore,Global.ignore,$this.U.Reactive.Never());
   });
  },
  ReturnEmpty:function(x)
  {
   var $this;
   $this=this;
   return $this.New(function()
   {
    return Form.New($this.U.Reactive.Return(Tree.Delete()),Global.ignore,Global.ignore,$this.U.Reactive.Return({
     $:0,
     $0:x
    }));
   });
  },
  FailWith:function(fs)
  {
   var $this;
   $this=this;
   return $this.New(function()
   {
    return $this.Fail(fs);
   });
  },
  Fail:function(fs)
  {
   return Form.New(this.U.Reactive.Never(),Global.ignore,Global.ignore,this.U.Reactive.Return({
    $:1,
    $0:fs
   }));
  },
  Return:function(x)
  {
   var $this;
   $this=this;
   return $this.New(function()
   {
    return Form.New($this.U.Reactive.Never(),Global.ignore,Global.ignore,$this.U.Reactive.Return({
     $:0,
     $0:x
    }));
   });
  },
  Apply:function(f,x)
  {
   var $this;
   $this=this;
   return $this.New(function()
   {
    var f$1,x$1;
    function a(r,f$2)
    {
     return Result.Apply(f$2,r);
    }
    f$1=$this.BuildForm(f);
    x$1=$this.BuildForm(x);
    return Form.New($this.U.Reactive.Merge($this.U.Reactive.Select(f$1.Body,function(a$1)
    {
     return new Edit({
      $:1,
      $0:a$1
     });
    }),$this.U.Reactive.Select(x$1.Body,function(a$1)
    {
     return new Edit({
      $:2,
      $0:a$1
     });
    })),function()
    {
     x$1.Dispose$1();
     f$1.Dispose$1();
    },function(o)
    {
     x$1.Notify(o);
     f$1.Notify(o);
    },$this.U.Reactive.CombineLatest(x$1.State,f$1.State,function($1)
    {
     return function($2)
     {
      return a($1,$2);
     };
    }));
   });
  },
  Map:function(f,formlet)
  {
   return this.MapResult(function(a)
   {
    return Result.Map(f,a);
   },formlet);
  },
  MapResult:function(f,formlet)
  {
   var $this;
   $this=this;
   return Formlet.New(formlet.LayoutI(),function()
   {
    var form;
    form=formlet.BuildI();
    return Form.New(form.Body,form.Dispose$1,form.Notify,$this.U.Reactive.Select(form.State,f));
   },this.U);
  },
  WithLayoutOrDefault:function(formlet)
  {
   return this.MapBody(Global.id,formlet);
  },
  MapBody:function(f,formlet)
  {
   var $this;
   $this=this;
   return this.WithLayout({
    Apply:function(o)
    {
     var m,m$1;
     m=formlet.LayoutI().Apply(o);
     return m==null?(m$1=$this.U.DefaultLayout.Apply(o),m$1==null?null:{
      $:1,
      $0:[f(m$1.$0[0]),m$1.$0[1]]
     }):{
      $:1,
      $0:[f(m.$0[0]),m.$0[1]]
     };
    }
   },formlet);
  },
  AppendLayout:function(layout,formlet)
  {
   return this.WithLayout(layout,this.ApplyLayout(formlet));
  },
  ApplyLayout:function(formlet)
  {
   var $this;
   $this=this;
   return $this.New(function()
   {
    var form,m;
    form=formlet.BuildI();
    return Form.New((m=formlet.LayoutI().Apply(form.Body),m==null?form.Body:$this.U.Reactive.Return(Tree.Set(m.$0[0]))),form.Dispose$1,form.Notify,form.State);
   });
  },
  InitWithFailure:function(formlet)
  {
   var $this,x;
   $this=this;
   x=$this.New(function()
   {
    var form;
    form=formlet.BuildI();
    return Form.New(form.Body,form.Dispose$1,form.Notify,$this.U.Reactive.Concat($this.U.Reactive.Return({
     $:1,
     $0:List.T.Empty
    }),form.State));
   });
   return $this.WithLayout(formlet.LayoutI(),x);
  },
  ReplaceFirstWithFailure:function(formlet)
  {
   var $this,x;
   $this=this;
   x=$this.New(function()
   {
    var form,a;
    form=formlet.BuildI();
    return Form.New(form.Body,form.Dispose$1,form.Notify,(a=$this.U.Reactive.Drop(form.State,1),$this.U.Reactive.Concat($this.U.Reactive.Return({
     $:1,
     $0:List.T.Empty
    }),a)));
   });
   return $this.WithLayout(formlet.LayoutI(),x);
  },
  InitWith:function(value,formlet)
  {
   var $this,x;
   $this=this;
   x=$this.New(function()
   {
    var form;
    form=formlet.BuildI();
    return Form.New(form.Body,form.Dispose$1,form.Notify,$this.U.Reactive.Concat($this.U.Reactive.Return({
     $:0,
     $0:value
    }),form.State));
   });
   return $this.WithLayout(formlet.LayoutI(),x);
  },
  WithLayout:function(layout,formlet)
  {
   return Formlet.New(layout,function()
   {
    return formlet.BuildI();
   },this.U);
  },
  FromState:function(state)
  {
   var $this;
   $this=this;
   return $this.New(function()
   {
    return Form.New($this.U.Reactive.Never(),Global.ignore,Global.ignore,state);
   });
  },
  New:function(build)
  {
   return Formlet.New(this.L.Default(),build,this.U);
  },
  BuildForm:function(formlet)
  {
   var form,m,d;
   form=formlet.BuildI();
   m=formlet.LayoutI().Apply(form.Body);
   return m!=null&&m.$==1?(d=m.$0[1],Form.New(this.U.Reactive.Return(Tree.Set(m.$0[0])),function()
   {
    form.Dispose$1();
    d.Dispose();
   },form.Notify,form.State)):form;
  }
 },Obj,FormletProvider);
 FormletProvider.New=Runtime.Ctor(function(U)
 {
  this.U=U;
  this.L=new LayoutUtils.New({
   Reactive:this.U.Reactive
  });
 },FormletProvider);
 FormletBuilder=Base.FormletBuilder=Runtime.Class({
  ReturnFrom:Global.id,
  Delay:function(f)
  {
   return this.F.Delay(f);
  },
  Bind:function(x,f)
  {
   return this.F.Bind(x,f);
  },
  Return:function(x)
  {
   return this.F.Return(x);
  }
 },Obj,FormletBuilder);
 FormletBuilder.New=Runtime.Ctor(function(F)
 {
  this.F=F;
 },FormletBuilder);
 Validator=Base.Validator=Runtime.Class({
  IsNotEqual:function(value,msg,flet)
  {
   return this.Validate(function(i)
   {
    return!Unchecked.Equals(i,value);
   },msg,flet);
  },
  IsEqual:function(value,msg,flet)
  {
   return this.Validate(function(i)
   {
    return Unchecked.Equals(i,value);
   },msg,flet);
  },
  IsLessThan:function(max,msg,flet)
  {
   return this.Validate(function(i)
   {
    return Unchecked.Compare(i,max)===-1;
   },msg,flet);
  },
  IsGreaterThan:function(min,msg,flet)
  {
   return this.Validate(function(i)
   {
    return Unchecked.Compare(i,min)===1;
   },msg,flet);
  },
  IsTrue:function(msg,flet)
  {
   return this.Validate(Global.id,msg,flet);
  },
  IsFloat:function(msg)
  {
   var $this;
   $this=this;
   return function(a)
   {
    return $this.IsRegexMatch("^\\s*(\\+|-)?((\\d+(\\.\\d+)?)|(\\.\\d+))\\s*$",msg,a);
   };
  },
  IsInt:function(msg)
  {
   var $this;
   $this=this;
   return function(a)
   {
    return $this.IsRegexMatch("^-?\\d+$",msg,a);
   };
  },
  IsEmail:function(msg)
  {
   var $this;
   $this=this;
   return function(a)
   {
    return $this.IsRegexMatch("^[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?$",msg,a);
   };
  },
  IsRegexMatch:function(regex,msg,flet)
  {
   var $this;
   $this=this;
   return this.Validate(function(x)
   {
    return $this.VP.Matches(regex,x);
   },msg,flet);
  },
  IsNotEmpty:function(msg,flet)
  {
   return this.Validate(function(s)
   {
    return s!=="";
   },msg,flet);
  },
  Is:function(f,m,flet)
  {
   return this.Validate(f,m,flet);
  },
  Validate:function(f,msg,flet)
  {
   return flet.MapResultI(function(res)
   {
    var v;
    return res.$==1?{
     $:1,
     $0:res.$0
    }:(v=res.$0,f(v)?{
     $:0,
     $0:v
    }:{
     $:1,
     $0:List.ofArray([msg])
    });
   });
  }
 },Obj,Validator);
 Validator.New=Runtime.Ctor(function(VP)
 {
  this.VP=VP;
 },Validator);
}());
