// Decompiled with JetBrains decompiler
// Type: Evon.Editor.WebView
// Assembly: Evon, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DB5636C-8858-4769-AD95-C6115039B8B8
// Assembly location: C:\Users\miham\Desktop\sw\Evon\Evon.exe

using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace Evon.Editor
{
  public class WebView : Microsoft.Web.WebView2.Wpf.WebView2
  {
    public bool IsLoaded;
    public string Text = "";
    public string scr;

    public event WebView.TextChanged OnTextChanged;

    public WebView(string script = "")
    {
      WebView webView = this;
      this.scr = script;
      ((FrameworkElement) this).BeginInit();
      this.Source = new Uri(Directory.GetCurrentDirectory() + "\\bin\\Monaco.html");
      this.DefaultBackgroundColor = Color.FromArgb(20, 20, 20);
      this.EnsureCoreWebView2Async((CoreWebView2Environment) null);
      this.CoreWebView2InitializationCompleted += (EventHandler<CoreWebView2InitializationCompletedEventArgs>) ((s, e) =>
      {
        webView.CoreWebView2.WebMessageReceived += (EventHandler<CoreWebView2WebMessageReceivedEventArgs>) ((__, args) =>
        {
          webView.Text = args.TryGetWebMessageAsString();
          WebView.TextChanged onTextChanged = webView.OnTextChanged;
          if (onTextChanged == null)
            return;
          onTextChanged(webView.Text);
        });
        webView.CoreWebView2.DOMContentLoaded += (EventHandler<CoreWebView2DOMContentLoadedEventArgs>) (async (sender, args) =>
        {
          await Task.Delay(500);
          webView.IsLoaded = true;
          if (script != "")
            webView.SetText(script);
          webView.antiskid();
          webView.minimap();
        });
        webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
        webView.CoreWebView2.Settings.AreDevToolsEnabled = true;
      });
      ((FrameworkElement) this).EndInit();
    }

    public void minimap()
    {
      object obj1 = JsonConvert.DeserializeObject(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
      // ISSUE: reference to a compiler-generated field
      if (WebView.\u003C\u003Eo__8.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebView.\u003C\u003Eo__8.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (WebView), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = WebView.\u003C\u003Eo__8.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = WebView.\u003C\u003Eo__8.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (WebView.\u003C\u003Eo__8.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebView.\u003C\u003Eo__8.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (WebView), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target2 = WebView.\u003C\u003Eo__8.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p1 = WebView.\u003C\u003Eo__8.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (WebView.\u003C\u003Eo__8.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebView.\u003C\u003Eo__8.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, nameof (minimap), typeof (WebView), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = WebView.\u003C\u003Eo__8.\u003C\u003Ep__0.Target((CallSite) WebView.\u003C\u003Eo__8.\u003C\u003Ep__0, obj1);
      object obj3 = target2((CallSite) p1, obj2, (object) null);
      if (!target1((CallSite) p2, obj3))
        return;
      // ISSUE: reference to a compiler-generated field
      if (WebView.\u003C\u003Eo__8.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebView.\u003C\u003Eo__8.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (WebView), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target3 = WebView.\u003C\u003Eo__8.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p5 = WebView.\u003C\u003Eo__8.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (WebView.\u003C\u003Eo__8.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebView.\u003C\u003Eo__8.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (WebView), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool, object> target4 = WebView.\u003C\u003Eo__8.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool, object>> p4 = WebView.\u003C\u003Eo__8.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (WebView.\u003C\u003Eo__8.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebView.\u003C\u003Eo__8.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, nameof (minimap), typeof (WebView), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = WebView.\u003C\u003Eo__8.\u003C\u003Ep__3.Target((CallSite) WebView.\u003C\u003Eo__8.\u003C\u003Ep__3, obj1);
      object obj5 = target4((CallSite) p4, obj4, true);
      if (target3((CallSite) p5, obj5))
        this.CoreWebView2.ExecuteScriptAsync("ShowMinimap()");
      else
        this.CoreWebView2.ExecuteScriptAsync("HideMinimap()");
    }

    public void antiskid()
    {
      object obj1 = JsonConvert.DeserializeObject(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
      // ISSUE: reference to a compiler-generated field
      if (WebView.\u003C\u003Eo__9.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebView.\u003C\u003Eo__9.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (WebView), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = WebView.\u003C\u003Eo__9.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = WebView.\u003C\u003Eo__9.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (WebView.\u003C\u003Eo__9.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebView.\u003C\u003Eo__9.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (WebView), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target2 = WebView.\u003C\u003Eo__9.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p1 = WebView.\u003C\u003Eo__9.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (WebView.\u003C\u003Eo__9.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebView.\u003C\u003Eo__9.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, nameof (antiskid), typeof (WebView), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = WebView.\u003C\u003Eo__9.\u003C\u003Ep__0.Target((CallSite) WebView.\u003C\u003Eo__9.\u003C\u003Ep__0, obj1);
      object obj3 = target2((CallSite) p1, obj2, (object) null);
      if (!target1((CallSite) p2, obj3))
        return;
      // ISSUE: reference to a compiler-generated field
      if (WebView.\u003C\u003Eo__9.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebView.\u003C\u003Eo__9.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (WebView), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target3 = WebView.\u003C\u003Eo__9.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p5 = WebView.\u003C\u003Eo__9.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (WebView.\u003C\u003Eo__9.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebView.\u003C\u003Eo__9.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (WebView), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool, object> target4 = WebView.\u003C\u003Eo__9.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool, object>> p4 = WebView.\u003C\u003Eo__9.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (WebView.\u003C\u003Eo__9.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebView.\u003C\u003Eo__9.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, nameof (antiskid), typeof (WebView), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = WebView.\u003C\u003Eo__9.\u003C\u003Ep__3.Target((CallSite) WebView.\u003C\u003Eo__9.\u003C\u003Ep__3, obj1);
      object obj5 = target4((CallSite) p4, obj4, true);
      if (target3((CallSite) p5, obj5))
        this.CoreWebView2.ExecuteScriptAsync("enableAntiSkid()");
      else
        this.CoreWebView2.ExecuteScriptAsync("disableAntiSkid()");
    }

    public void Undo()
    {
      if (!this.IsLoaded)
        return;
      this.CoreWebView2.ExecuteScriptAsync("Undo()");
    }

    public void SetText(string text)
    {
      if (!this.IsLoaded)
        return;
      this.CoreWebView2.ExecuteScriptAsync("SetText(\"" + HttpUtility.JavaScriptStringEncode(text) + "\")");
    }

    public delegate void TextChanged(string Text);
  }
}
