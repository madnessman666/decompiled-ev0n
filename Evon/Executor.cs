// Decompiled with JetBrains decompiler
// Type: Evon.Executor
// Assembly: Evon, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DB5636C-8858-4769-AD95-C6115039B8B8
// Assembly location: C:\Users\miham\Desktop\sw\Evon\Evon.exe

using ColorPicker;
using Evon.Editor;
using Evon.UserControls;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using tools;
using tools.ProcessTools;

namespace Evon
{
  public class Executor : Window, IComponentConnector, IStyleConnector
  {
    private bool showNotiff = false;
    private Dictionary<TabItem, string> Texts = new Dictionary<TabItem, string>();
    private string searchingsystem = "";
    private int pagenum = 1;
    private bool showScripts;
    private int pipeDelay;
    private bool doingDownload;
    private Brush DefaultBrush = (Brush) new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte) 153, (byte) 0, byte.MaxValue));
    private WebView editor = new WebView();
    private Watcher w = new Watcher();
    public List<GameRectangle> scriptItems = new List<GameRectangle>();
    internal Executor window;
    internal System.Windows.Media.Animation.BeginStoryboard IconNotInjected_BeginStoryboard;
    internal Grid grid;
    internal Border ShadowVibe;
    internal Border MainBorder;
    internal Border MenuBar;
    internal Image image;
    internal Button ExitB;
    internal Button MinimizeB;
    internal Border ShowAreaBorder;
    internal Button ExecutorPB;
    internal Button ScriptsPB;
    internal Button SettingsPB;
    internal Grid ExecutorGrid;
    internal Border ExecutorBar;
    internal Grid InjectB;
    internal Label label;
    internal Button injBtn;
    internal Grid ExecuteB;
    internal Grid OpenB;
    internal Grid SaveB;
    internal Grid ClearB;
    internal Grid OpenListB;
    internal Label ScriptListLabel;
    internal TabControl tabs;
    internal Grid ScriptListGrid;
    internal TextBox ListBoxSearch;
    internal ListBox ScriptList;
    internal Grid GameScriptsGrid;
    internal Border GameScriptsBar;
    internal Button ChangePGLeft;
    internal Button ChangePGRight;
    internal Grid GameScriptsInjectB;
    internal TextBox search;
    internal WrapPanel GameSys;
    internal Grid SettingsGrid;
    internal Border SettingsBar;
    internal TabControl settingTabs;
    internal TabItem UISettingsTab;
    internal ScrollViewer sview;
    internal WrapPanel settingsViewWindow;
    internal DropShadowEffect DSB1;
    internal Button fixbtn;
    internal DropShadowEffect DSB2;
    internal DropShadowEffect DSB3;
    internal Button revertbtn;
    internal DropShadowEffect DSB4;
    internal CheckBox TopMostCheck;
    internal DropShadowEffect DSB5;
    internal CheckBox autoinjectcheck;
    internal DropShadowEffect DSB6;
    internal CheckBox unlockfpscheck;
    internal DropShadowEffect DSB7;
    internal Button colorbtn;
    internal PortableColorPicker colr;
    internal DropShadowEffect DSB8;
    internal CheckBox antiskidcheck;
    internal DropShadowEffect DSB9;
    internal CheckBox minimapCheck;
    internal TabItem ApiTab;
    internal ScrollViewer APiView;
    internal WrapPanel ApiPanerl;
    internal DropShadowEffect _1e;
    internal CheckBox api_evon;
    internal DropShadowEffect _2e;
    internal CheckBox api_oxygen;
    internal DropShadowEffect _3e;
    internal CheckBox api_fluxus;
    internal DropShadowEffect _4e;
    internal CheckBox api_krnl;
    internal Border MenuBar_Copy;
    internal DropShadowEffect ShadowNotifColor;
    internal TextBlock Notiftitle;
    internal TextBlock NotifSub;
    internal Border notifBar;
    private bool _contentLoaded;

    public async void showNotif(string title, string text)
    {
      if (this.showNotiff)
        return;
      if (!this.showNotiff)
        this.showNotiff = true;
      this.Notiftitle.Text = title;
      this.NotifSub.Text = text;
      this.Focus();
      await ((Storyboard) this.Resources[(object) "ShowNotif"]).Start();
      await ((Storyboard) this.Resources[(object) "HideNotif"]).Start();
      this.showNotiff = false;
    }

    public static bool WebViewIsInstalled()
    {
      try
      {
        if (Environment.Is64BitOperatingSystem)
        {
          using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\WOW6432Node\\Microsoft\\EdgeUpdate\\Clients\\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}"))
            return registryKey != null && registryKey.GetValue("pv") != null;
        }
        else
        {
          using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\EdgeUpdate\\Clients\\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}"))
            return registryKey != null && registryKey.GetValue("pv") != null;
        }
      }
      catch
      {
        return false;
      }
    }

    public static bool isInstalled()
    {
      try
      {
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\DevDiv\\VC\\Servicing\\14.0\\RuntimeMinimum", false);
        if (registryKey == null)
          return false;
        object obj = registryKey.GetValue("Version");
        return (string) obj != null && ((string) obj).StartsWith("14");
      }
      catch
      {
        return false;
      }
    }

    public static async void doRedistDownload()
    {
      int num1 = (int) MessageBox.Show("Press OK to install vc++ runtime, this may take long, don't be alarmed if nothing shows up!");
      int time = 0;
      try
      {
        using (WebClient wc = new WebClient())
        {
          wc.DownloadFile("https://aka.ms/vs/16/release/vc_redist.x86.exe", "vc_redist.x86.exe");
          Process.Start(Directory.GetCurrentDirectory() + "\\vc_redist.x86.exe", "/q /norestart");
          while (!Executor.isInstalled())
          {
            await Task.Delay(500);
            if (time >= 60000)
            {
              Clipboard.SetText("https://aka.ms/vs/16/release/vc_redist.x86.exe");
              int num2 = (int) MessageBox.Show("This install is taking longer than usual, if it doesn't install, open the download in your browser, we've took the chance to copy it to your clipboard.");
              time = 30000;
            }
            time += 500;
          }
          int num3 = (int) MessageBox.Show("Webview installed.");
        }
      }
      catch
      {
        int num4 = (int) MessageBox.Show("Something happened while attempting to download, copying the link to your clipboard.");
        Clipboard.SetText("https://aka.ms/vs/16/release/vc_redist.x86.exe");
      }
    }

    public static async void doDownload()
    {
      int num1 = (int) MessageBox.Show("Press OK to install webview, this may take long, don't be alarmed if nothing shows up!");
      int time = 0;
      try
      {
        using (WebClient wc = new WebClient())
        {
          wc.DownloadFile("https://msedge.sf.dl.delivery.mp.microsoft.com/filestreamingservice/files/bd675101-bf97-478c-8f02-350c87a54a83/MicrosoftEdgeWebView2RuntimeInstallerX86.exe", "webviewruntime.exe");
          Process.Start(Directory.GetCurrentDirectory() + "\\webviewruntime.exe", "/install");
          while (!Executor.WebViewIsInstalled())
          {
            await Task.Delay(500);
            if (time >= 60000)
            {
              Clipboard.SetText("https://msedge.sf.dl.delivery.mp.microsoft.com/filestreamingservice/files/bd675101-bf97-478c-8f02-350c87a54a83/MicrosoftEdgeWebView2RuntimeInstallerX86.exe");
              int num2 = (int) MessageBox.Show("This install is taking longer than usual, if it doesn't install, open the download in your browser, we've took the chance to copy it to your clipboard.");
              time = 30000;
            }
            time += 500;
          }
          int num3 = (int) MessageBox.Show("Webview installed.");
        }
      }
      catch
      {
        int num4 = (int) MessageBox.Show("Something happened while attempting to download, copying the link to your clipboard.");
        Clipboard.SetText("https://msedge.sf.dl.delivery.mp.microsoft.com/filestreamingservice/files/bd675101-bf97-478c-8f02-350c87a54a83/MicrosoftEdgeWebView2RuntimeInstallerX86.exe");
      }
    }

    public static bool IsAdministrator => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

    public static bool IsValidURI(string uri)
    {
      Uri result;
      return Uri.IsWellFormedUriString(uri, UriKind.Absolute) && Uri.TryCreate(uri, UriKind.Absolute, out result) && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }

    public void reloadScripts(object sender = null, FileSystemEventArgs e = null) => this.Dispatcher.Invoke((Action) (() =>
    {
      IEnumerable<string> source = Directory.EnumerateFiles("scripts", "*.*", SearchOption.AllDirectories);
      this.ScriptList.Items.Clear();
      if (this.ListBoxSearch.Text != null)
      {
        foreach (string path in source.Where<string>((Func<string, bool>) (script => script.ToLower().Contains(this.ListBoxSearch.Text.ToLower()))))
          this.ScriptList.Items.Add((object) Path.GetFileName(path));
      }
      else
      {
        foreach (string path in source)
          this.ScriptList.Items.Add((object) Path.GetFileName(path));
      }
    }));

    private void reloadrScripts() => new Thread((ThreadStart) (() =>
    {
      this.Dispatcher.Invoke((Action) (() =>
      {
        this.ScriptList.Items.Clear();
        this.GameSys.Children.Clear();
      }));
      try
      {
        if (this.searchingsystem == "")
          this.searchingsystem = "a";
        using (WebClient webClient = new WebClient())
        {
          JsonConvert.DeserializeObject(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
          object obj25 = JsonConvert.DeserializeObject(webClient.DownloadString("https://scriptblox.com/api/script/search?q=" + this.searchingsystem + "&mode=free&max=100&page=" + this.pagenum.ToString()));
          List<int> intList = new List<int>();
          BrushConverter BrushConversion = new BrushConverter();
          // ISSUE: reference to a compiler-generated field
          if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__29 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Executor.\u003C\u003Eo__21.\u003C\u003Ep__29 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (Executor)));

        public void Connect(int connectionId, object target)
        {
            throw new NotImplementedException();
        }

        public void InitializeComponent()
        {
            throw new NotImplementedException();
        }
    }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, IEnumerable> target24 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__29.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, IEnumerable>> p29 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__29;
          // ISSUE: reference to a compiler-generated field
          if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Executor.\u003C\u003Eo__21.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "scripts", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object> target25 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__1.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object>> p1 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__1;
          // ISSUE: reference to a compiler-generated field
          if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Executor.\u003C\u003Eo__21.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "result", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj26 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__0.Target((CallSite) Executor.\u003C\u003Eo__21.\u003C\u003Ep__0, obj25);
          object obj27 = target25((CallSite) p1, obj26);
          foreach (object obj28 in target24((CallSite) p29, obj27))
          {
            object script = obj28;
            this.Dispatcher.Invoke((Action) (() =>
            {
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__5 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (Executor)));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, object, string> target26 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__5.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, object, string>> p5 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__5;
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__4 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, object, object> target27 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__4.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, object, object>> p4 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__4;
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__3 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "imageUrl", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, object, object> target28 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__3.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, object, object>> p3 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__3;
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__2 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "game", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj29 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__2.Target((CallSite) Executor.\u003C\u003Eo__21.\u003C\u003Ep__2, script);
              object obj30 = target28((CallSite) p3, obj29);
              object obj31 = target27((CallSite) p4, obj30);
              string str3 = target26((CallSite) p5, obj31);
              if (!str3.Contains("rbxcdn.com"))
              {
                // ISSUE: reference to a compiler-generated field
                if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__10 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  Executor.\u003C\u003Eo__21.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (Executor)));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, string> target29 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__10.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, string>> p10 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__10;
                // ISSUE: reference to a compiler-generated field
                if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__9 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  Executor.\u003C\u003Eo__21.\u003C\u003Ep__9 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, string, object, object> target30 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__9.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, string, object, object>> p9 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__9;
                // ISSUE: reference to a compiler-generated field
                if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__8 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  Executor.\u003C\u003Eo__21.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, object> target31 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__8.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, object>> p8 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__8;
                // ISSUE: reference to a compiler-generated field
                if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__7 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  Executor.\u003C\u003Eo__21.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "imageUrl", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, object> target32 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__7.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, object>> p7 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__7;
                // ISSUE: reference to a compiler-generated field
                if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__6 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  Executor.\u003C\u003Eo__21.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "game", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                object obj32 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__6.Target((CallSite) Executor.\u003C\u003Eo__21.\u003C\u003Ep__6, script);
                object obj33 = target32((CallSite) p7, obj32);
                object obj34 = target31((CallSite) p8, obj33);
                object obj35 = target30((CallSite) p9, "https://scriptblox.com", obj34);
                str3 = target29((CallSite) p10, obj35);
              }
              GameRectangle gameRectangle = new GameRectangle();
              gameRectangle.ScriptImage.ImageSource = Executor.IsValidURI(str3) ? (ImageSource) new BitmapImage(new Uri(str3)) : (ImageSource) null;
              TextBlock scriptTitle = gameRectangle.ScriptTitle;
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__13 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (Executor)));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, object, string> target33 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__13.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, object, string>> p13 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__13;
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__12 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, object, object> target34 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__12.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, object, object>> p12 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__12;
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__11 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "title", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj36 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__11.Target((CallSite) Executor.\u003C\u003Eo__21.\u003C\u003Ep__11, script);
              object obj37 = target34((CallSite) p12, obj36);
              string str4 = target33((CallSite) p13, obj37);
              scriptTitle.Text = str4;
              GameRectangle element = gameRectangle;
              element.items = this.GameSys;
              element.ExecuteB.Click += (RoutedEventHandler) ((ssss, eeee) =>
              {
                // ISSUE: reference to a compiler-generated field
                if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__16 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  Executor.\u003C\u003Eo__21.\u003C\u003Ep__16 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Execute", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                Action<CallSite, Type, object> target12 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__16.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Action<CallSite, Type, object>> p16 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__16;
                Type type = typeof (Evon.Classes.Apis.Apis);
                // ISSUE: reference to a compiler-generated field
                if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__15 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  Executor.\u003C\u003Eo__21.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, object> target13 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__15.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, object>> p15 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__15;
                // ISSUE: reference to a compiler-generated field
                if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__14 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  Executor.\u003C\u003Eo__21.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "script", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                object obj12 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__14.Target((CallSite) Executor.\u003C\u003Eo__21.\u003C\u003Ep__14, script);
                object obj13 = target13((CallSite) p15, obj12);
                target12((CallSite) p16, type, obj13);
              });
              object obj38 = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("./bin/theme.evon"));
              Button executeB = element.ExecuteB;
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__22 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, Brush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (Brush), typeof (Executor)));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, object, Brush> target35 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__22.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, object, Brush>> p22 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__22;
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__21 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__21 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, BrushConverter, object, object> target36 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__21.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, BrushConverter, object, object>> p21 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__21;
              BrushConverter brushConverter3 = BrushConversion;
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__20 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, object, object> target37 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__20.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, object, object>> p20 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__20;
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__19 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, object, object> target38 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__19.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, object, object>> p19 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__19;
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__18 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, object, int, object> target39 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__18.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, object, int, object>> p18 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__18;
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__17 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj39 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__17.Target((CallSite) Executor.\u003C\u003Eo__21.\u003C\u003Ep__17, obj38);
              object obj40 = target39((CallSite) p18, obj39, 0);
              object obj41 = target38((CallSite) p19, obj40);
              object obj42 = target37((CallSite) p20, obj41);
              object obj43 = target36((CallSite) p21, brushConverter3, obj42);
              Brush brush = target35((CallSite) p22, obj43);
              executeB.Background = brush;
              DropShadowEffect buttonGlow = element.ButtonGlow;
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__28 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__28 = CallSite<Func<CallSite, object, SolidColorBrush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (SolidColorBrush), typeof (Executor)));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, object, SolidColorBrush> target40 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__28.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, object, SolidColorBrush>> p28 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__28;
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__27 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__27 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, BrushConverter, object, object> target41 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__27.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, BrushConverter, object, object>> p27 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__27;
              BrushConverter brushConverter4 = BrushConversion;
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__26 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, object, object> target42 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__26.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, object, object>> p26 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__26;
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__25 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, object, object> target43 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__25.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, object, object>> p25 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__25;
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__24 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, object, int, object> target44 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__24.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, object, int, object>> p24 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__24;
              // ISSUE: reference to a compiler-generated field
              if (Executor.\u003C\u003Eo__21.\u003C\u003Ep__23 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Executor.\u003C\u003Eo__21.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj44 = Executor.\u003C\u003Eo__21.\u003C\u003Ep__23.Target((CallSite) Executor.\u003C\u003Eo__21.\u003C\u003Ep__23, obj38);
              object obj45 = target44((CallSite) p24, obj44, 0);
              object obj46 = target43((CallSite) p25, obj45);
              object obj47 = target42((CallSite) p26, obj46);
              object obj48 = target41((CallSite) p27, brushConverter4, obj47);
              System.Windows.Media.Color color = target40((CallSite) p28, obj48).Color;
              buttonGlow.Color = color;
              this.scriptItems.Add(element);
              this.GameSys.Children.Add((UIElement) element);
            }));
          }
          GC.Collect(2, GCCollectionMode.Forced);
        }
      }
      catch (Exception ex)
      {
        if (MessageBox.Show("Something happened while attempting to load scripts from https://www.rbxscripts.xyz . Please make sure your connection is valid. Would you like to retry fetching scripts?", "EVON", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
          return;
        this.reloadrScripts();
      }
    })).Start();

    public Executor()
    {
      if (!Directory.Exists("bin"))
        Directory.CreateDirectory("bin");
      Directory.CreateDirectory("bin\\tabs");
      if (!Directory.Exists("scripts"))
        Directory.CreateDirectory("scripts");
      if (!System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"))
        System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon", "{}");
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\Windows NT\\CurrentVersion");
      if (registryKey.GetValue("ProductName").ToString().Contains("Windows 7"))
      {
        int num = (int) MessageBox.Show("Windows 7 is not supported with EVON. We're terribly sorry for this inconvenience");
        this.Close();
      }
      registryKey.Dispose();
      this.InitializeComponent();
      this.editor.OnTextChanged += (WebView.TextChanged) (text =>
      {
        if (this.tabs.SelectedItem == null)
          return;
        TabItem selectedItem = (TabItem) this.tabs.SelectedItem;
        if (this.Texts.TryGetValue(selectedItem, out string _))
          this.Texts[selectedItem] = text;
        else
          this.Texts.Add(selectedItem, text);
      });
      this.Hide();
      this.w.OnProcessMade += (Watcher.ProcessDelegate) (() => this.Dispatcher.Invoke<Task>((Func<Task>) (async () =>
      {
        await Task.Delay(5000);
        this.doInject((object) null, (RoutedEventArgs) null);
      })));
      this.w.Initialize("RobloxPlayerBeta");
      this.setcolor();
      string str1 = (string) null;
      if (!Executor.WebViewIsInstalled())
        Executor.doDownload();
      if (!Executor.isInstalled())
        Executor.doRedistDownload();
      if (!System.IO.File.Exists("C:\\Windows\\System32\\msvcp140.dll"))
      {
        if (!Executor.IsAdministrator)
        {
          int num = (int) MessageBox.Show("Please restart evon as an admin so we can download missing files.");
          Process.GetCurrentProcess().Kill();
        }
        try
        {
          using (WebClient webClient = new WebClient())
            webClient.DownloadFile("https://github.com/ahhh-ahhh/EVON-downloads/raw/main/msvcp" + (Environment.Is64BitOperatingSystem ? "64" : "32") + ".dll", "C:\\Windows\\System32\\msvcp140.dll");
        }
        catch
        {
          Clipboard.SetText("https://github.com/ahhh-ahhh/EVON-downloads/raw/main/msvcp" + (Environment.Is64BitOperatingSystem ? "64" : "32") + ".dll");
          int num = (int) MessageBox.Show("Something happened while attempting to download msvcp140. We've copied the link to your clipboard.");
          this.Close();
        }
      }
      try
      {
        using (WebClient webClient = new WebClient())
        {
          object obj1 = JsonConvert.DeserializeObject(webClient.DownloadString("https://clientsettingscdn.roblox.com/v2/client-version/WindowsPlayer"));
          // ISSUE: reference to a compiler-generated field
          if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Executor.\u003C\u003Eo__22.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, bool> target1 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__2.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, bool>> p2 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__2;
          // ISSUE: reference to a compiler-generated field
          if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Executor.\u003C\u003Eo__22.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object, object> target2 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__1.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object, object>> p1 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__1;
          // ISSUE: reference to a compiler-generated field
          if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Executor.\u003C\u003Eo__22.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "clientVersionUpload", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__0.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__0, obj1);
          object obj3 = target2((CallSite) p1, obj2, (object) null);
          if (target1((CallSite) p2, obj3))
          {
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__5 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__22.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (Executor)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, string> target3 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__5.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, string>> p5 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__5;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__4 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__22.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object> target4 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__4.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object>> p4 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__4;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__3 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__22.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "clientVersionUpload", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj4 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__3.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__3, obj1);
            object obj5 = target4((CallSite) p4, obj4);
            str1 = target3((CallSite) p5, obj5);
          }
        }
      }
      catch
      {
        int num = (int) MessageBox.Show("Failed to get the ROBLOX Version! Please make sure you have a valid internet connection and your connection to roblox.com is correct.");
        this.Close();
      }
      if (!System.IO.File.Exists("version.data"))
      {
        if (MessageBox.Show("Failed to find a vital file! this could be because your antivirus deleted it, or that you didnt download the right verion of EVON.\nPlease make sure you download from https://sakpot.com/evon-executor .\nWould you like us to open it in your default browser?", "EVON", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
          try
          {
            Process.Start("https://sakpot.com/evon-executor");
          }
          catch
          {
            int num = (int) MessageBox.Show("Couldn't open the link in your default browser! Please make sure you have a default browser set in your settings. The URL has been copied to your clipboard for ease of use.", "EVON");
            Clipboard.SetText("https://sakpot.com/evon-executor");
          }
        }
        Process.GetCurrentProcess().Kill();
      }
      if (System.IO.File.ReadAllText("version.data") != str1)
      {
        if (MessageBox.Show("The current version you're using is out of date, this could be because the exploit is currently patched, or you have downloaded an older verison of EVON. Please make sure you downloaded from https://sakpot.com/evon-executor .\nWould you like us to open it in your default browser?", "EVON", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
          try
          {
            Process.Start("https://sakpot.com/evon-executor");
          }
          catch
          {
            int num = (int) MessageBox.Show("Couldn't open the link in your default browser! Please make sure you have a default browser set in your settings. The URL has been copied to your clipboard for ease of use.", "EVON");
            Clipboard.SetText("https://sakpot.com/evon-executor");
          }
        }
        Process.GetCurrentProcess().Kill();
      }
      if (!System.IO.File.Exists("Evon.dll"))
      {
        if (MessageBox.Show("Failed to find Evon.DLL! this could be because your antivirus deleted it, or that you didnt download the right verion of EVON.\nPlease make sure you download from https://sakpot.com/evon-executor .\nWould you like us to open it in your default browser?", "EVON", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
          try
          {
            Process.Start("https://sakpot.com/evon-executor");
          }
          catch
          {
            int num = (int) MessageBox.Show("Couldn't open the link in your default browser! Please make sure you have a default browser set in your settings. The URL has been copied to your clipboard for ease of use.", "EVON");
            Clipboard.SetText("https://sakpot.com/evon-executor");
          }
        }
        Process.GetCurrentProcess().Kill();
      }
      if (!System.IO.File.Exists("Oxygen API.dll"))
      {
        if (MessageBox.Show("Failed to find Oxygen API.DLL! this could be because your antivirus deleted it, or that you didnt download the right verion of EVON.\nPlease make sure you download from https://sakpot.com/evon-executor .\nWould you like us to open it in your default browser?", "EVON", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
          try
          {
            Process.Start("https://sakpot.com/evon-executor");
          }
          catch
          {
            int num = (int) MessageBox.Show("Couldn't open the link in your default browser! Please make sure you have a default browser set in your settings. The URL has been copied to your clipboard for ease of use.", "EVON");
            Clipboard.SetText("https://sakpot.com/evon-executor");
          }
        }
        Process.GetCurrentProcess().Kill();
      }
      if (!System.IO.File.Exists("KrnlAPI.dll"))
      {
        if (MessageBox.Show("Failed to find KrnlAPI.DLL! this could be because your antivirus deleted it, or that you didnt download the right verion of EVON.\nPlease make sure you download from https://sakpot.com/evon-executor .\nWould you like us to open it in your default browser?", "EVON", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
          try
          {
            Process.Start("https://sakpot.com/evon-executor");
          }
          catch
          {
            int num = (int) MessageBox.Show("Couldn't open the link in your default browser! Please make sure you have a default browser set in your settings. The URL has been copied to your clipboard for ease of use.", "EVON");
            Clipboard.SetText("https://sakpot.com/evon-executor");
          }
        }
        Process.GetCurrentProcess().Kill();
      }
      if (!System.IO.File.Exists("FluxAPI.dll"))
      {
        if (MessageBox.Show("Failed to find FluxAPI.DLL! this could be because your antivirus deleted it, or that you didnt download the right verion of EVON.\nPlease make sure you download from https://sakpot.com/evon-executor .\nWould you like us to open it in your default browser?", "EVON", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
          try
          {
            Process.Start("https://sakpot.com/evon-executor");
          }
          catch
          {
            int num = (int) MessageBox.Show("Couldn't open the link in your default browser! Please make sure you have a default browser set in your settings. The URL has been copied to your clipboard for ease of use.", "EVON");
            Clipboard.SetText("https://sakpot.com/evon-executor");
          }
        }
        Process.GetCurrentProcess().Kill();
      }
      FileSystemWatcher fileSystemWatcher = new FileSystemWatcher("scripts")
      {
        EnableRaisingEvents = true,
        IncludeSubdirectories = true,
        NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite | NotifyFilters.CreationTime
      };
      fileSystemWatcher.Changed += (FileSystemEventHandler) ((poop, pee) => this.reloadScripts());
      fileSystemWatcher.Created += (FileSystemEventHandler) ((poop, pee) => this.reloadScripts());
      fileSystemWatcher.Deleted += (FileSystemEventHandler) ((poop, pee) => this.reloadScripts());
      fileSystemWatcher.Renamed += (RenamedEventHandler) ((poop, pee) => this.reloadScripts());
      object obj6 = JsonConvert.DeserializeObject(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target5 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p8 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__8;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target6 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p7 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__7;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "topmost", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__6.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__6, obj6);
      object obj8 = target6((CallSite) p7, obj7, (object) null);
      if (target5((CallSite) p8, obj8))
      {
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__22.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "topmost", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj9 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__9.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__9, obj6, false);
      }
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target7 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__12.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p12 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__12;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target8 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p11 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__11;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "legacy", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj10 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__10.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__10, obj6);
      object obj11 = target8((CallSite) p11, obj10, (object) null);
      if (target7((CallSite) p12, obj11))
      {
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__13 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__22.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "legacy", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj12 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__13.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__13, obj6, false);
      }
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target9 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__16.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p16 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__16;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__15 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target10 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__15.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p15 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__15;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "antiskid", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj13 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__14.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__14, obj6);
      object obj14 = target10((CallSite) p15, obj13, (object) null);
      if (target9((CallSite) p16, obj14))
      {
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__17 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__22.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "antiskid", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj15 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__17.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__17, obj6, false);
      }
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__20 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target11 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__20.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p20 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__20;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__19 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target12 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__19.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p19 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__19;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__18 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "minimap", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj16 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__18.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__18, obj6);
      object obj17 = target12((CallSite) p19, obj16, (object) null);
      if (target11((CallSite) p20, obj17))
      {
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__21 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__22.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "minimap", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj18 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__21.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__21, obj6, false);
      }
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__24 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target13 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__24.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p24 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__24;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__23 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target14 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__23.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p23 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__23;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__22 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "autoinject", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj19 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__22.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__22, obj6);
      object obj20 = target14((CallSite) p23, obj19, (object) null);
      if (target13((CallSite) p24, obj20))
      {
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__25 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__22.\u003C\u003Ep__25 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "autoinject", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj21 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__25.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__25, obj6, false);
      }
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__28 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__28 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target15 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__28.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p28 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__28;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__27 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__27 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target16 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__27.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p27 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__27;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__26 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "unlockfps", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj22 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__26.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__26, obj6);
      object obj23 = target16((CallSite) p27, obj22, (object) null);
      if (target15((CallSite) p28, obj23))
      {
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__29 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__22.\u003C\u003Ep__29 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "unlockfps", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj24 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__29.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__29, obj6, false);
      }
      if (!System.IO.File.Exists("bin\\info.evon"))
      {
        System.IO.File.WriteAllText("bin\\info.evon", "true");
        if (MessageBox.Show("Would you like to join our discord server for constant updates about EVON? It may seem annoying that we make this request but do not worry, it's only a one-time request", "Evon - Invite", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
          try
          {
            Process.Start("https://discord.gg/YpXFb3xUqz");
          }
          catch
          {
            Clipboard.SetText("https://discord.gg/YpXFb3xUqz");
            int num = (int) MessageBox.Show("Couldn't open your default browser, please make sure that you have one set. For now, we've copied our cute invite to your clipboard!");
          }
        }
      }
      if (!Directory.Exists("bin\\tabs"))
        Directory.CreateDirectory("bin\\tabs");
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__31 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__31 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (bool), typeof (Executor)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target17 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__31.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p31 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__31;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__30 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "topmost", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj25 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__30.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__30, obj6);
      this.Topmost = target17((CallSite) p31, obj25);
      CheckBox topMostCheck = this.TopMostCheck;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__33 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__33 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (bool), typeof (Executor)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target18 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__33.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p33 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__33;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__32 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "topmost", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj26 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__32.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__32, obj6);
      bool? nullable1 = new bool?(target18((CallSite) p33, obj26));
      topMostCheck.IsChecked = nullable1;
      CheckBox antiskidcheck = this.antiskidcheck;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__35 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__35 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (bool), typeof (Executor)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target19 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__35.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p35 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__35;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__34 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "antiskid", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj27 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__34.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__34, obj6);
      bool? nullable2 = new bool?(target19((CallSite) p35, obj27));
      antiskidcheck.IsChecked = nullable2;
      CheckBox minimapCheck = this.minimapCheck;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__37 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__37 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (bool), typeof (Executor)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target20 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__37.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p37 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__37;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__36 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__36 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "minimap", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj28 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__36.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__36, obj6);
      bool? nullable3 = new bool?(target20((CallSite) p37, obj28));
      minimapCheck.IsChecked = nullable3;
      CheckBox autoinjectcheck = this.autoinjectcheck;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__39 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__39 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (bool), typeof (Executor)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target21 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__39.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p39 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__39;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__38 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "autoinject", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj29 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__38.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__38, obj6);
      bool? nullable4 = new bool?(target21((CallSite) p39, obj29));
      autoinjectcheck.IsChecked = nullable4;
      CheckBox unlockfpscheck = this.unlockfpscheck;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__41 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__41 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (bool), typeof (Executor)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target22 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__41.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p41 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__41;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__40 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__40 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "unlockfps", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj30 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__40.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__40, obj6);
      bool? nullable5 = new bool?(target22((CallSite) p41, obj30));
      unlockfpscheck.IsChecked = nullable5;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__44 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__44 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target23 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__44.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p44 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__44;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__43 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__43 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target24 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__43.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p43 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__43;
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__42 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "defaultexec", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj31 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__42.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__42, obj6);
      object obj32 = target24((CallSite) p43, obj31, (object) null);
      if (target23((CallSite) p44, obj32))
      {
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__46 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__22.\u003C\u003Ep__46 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target25 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__46.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p46 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__46;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__45 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__22.\u003C\u003Ep__45 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "defaultexec", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj33 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__45.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__45, obj6);
        switch (target25((CallSite) p46, obj33))
        {
          case 0:
            this.api_oxygen.IsChecked = new bool?(true);
            break;
          case 1:
            this.api_evon.IsChecked = new bool?(true);
            break;
          case 2:
            this.api_fluxus.IsChecked = new bool?(true);
            break;
          case 3:
            this.api_krnl.IsChecked = new bool?(true);
            break;
          default:
            this.api_evon.IsChecked = new bool?(true);
            break;
        }
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__48 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__22.\u003C\u003Ep__48 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target26 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__48.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p48 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__48;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__47 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__22.\u003C\u003Ep__47 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "defaultexec", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj34 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__47.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__47, obj6);
        Evon.Classes.Apis.Apis.selected = target26((CallSite) p48, obj34);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__49 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__22.\u003C\u003Ep__49 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "defaultexec", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj35 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__49.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__49, obj6, 1);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__51 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__22.\u003C\u003Ep__51 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target27 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__51.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p51 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__51;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__50 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__22.\u003C\u003Ep__50 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "defaultexec", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj36 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__50.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__50, obj6);
        Evon.Classes.Apis.Apis.selected = target27((CallSite) p51, obj36);
        this.api_evon.IsChecked = new bool?(true);
      }
      if (this.autoinjectcheck.IsChecked.Value)
        this.w.Start();
      else
        this.w.Stop();
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__53 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__53 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, Type, string, object> target28 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__53.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, Type, string, object>> p53 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__53;
      Type type = typeof (System.IO.File);
      string str2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
      // ISSUE: reference to a compiler-generated field
      if (Executor.\u003C\u003Eo__22.\u003C\u003Ep__52 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Executor.\u003C\u003Eo__22.\u003C\u003Ep__52 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj37 = Executor.\u003C\u003Eo__22.\u003C\u003Ep__52.Target((CallSite) Executor.\u003C\u003Eo__22.\u003C\u003Ep__52, typeof (JsonConvert), obj6);
      target28((CallSite) p53, type, str2, obj37);
      this.Show();
      this.reloadrScripts();
      this.reloadScripts();
      if (!Directory.Exists("runtimes"))
      {
        using (WebClient webClient = new WebClient())
        {
          try
          {
            webClient.DownloadFile("https://github.com/ahhh-ahhh/EVON-downloads/raw/main/runtimes.zip", Path.GetTempPath() + "\\runtimes.zip");
            ZipFile.ExtractToDirectory(Path.GetTempPath() + "\\runtimes.zip", "./");
            System.IO.File.Delete(Path.GetTempPath() + "\\runtimes.zip");
          }
          catch
          {
            int num = (int) MessageBox.Show("Something happened while downloading files required for EVON. Please make sure your Firewall isnt blocking https://www.github.com .", "EVON", MessageBoxButton.OK, MessageBoxImage.Hand);
            Process.GetCurrentProcess().Kill();
          }
        }
      }
      if (!System.IO.File.Exists("bin\\Monaco.html"))
      {
        using (WebClient webClient = new WebClient())
        {
          try
          {
            webClient.DownloadFile("https://raw.githubusercontent.com/ahhh-ahhh/EVON-downloads/main/Monaco.html", "bin\\Monaco.html");
          }
          catch
          {
            int num = (int) MessageBox.Show("Something happened while downloading files required for EVON. Please make sure your Firewall isnt blocking https://www.github.com .", "EVON", MessageBoxButton.OK, MessageBoxImage.Hand);
            Process.GetCurrentProcess().Kill();
          }
        }
      }
      if (Directory.Exists("bin\\vs"))
        return;
      using (WebClient webClient = new WebClient())
      {
        try
        {
          webClient.DownloadFile("https://github.com/ahhh-ahhh/EVON-downloads/raw/main/vs.zip", Path.GetTempPath() + "\\vs.zip");
          ZipFile.ExtractToDirectory(Path.GetTempPath() + "\\vs.zip", "bin");
          System.IO.File.Delete(Path.GetTempPath() + "\\vz.zip");
        }
        catch
        {
          int num = (int) MessageBox.Show("Something happened while downloading files required for EVON. Please make sure your Firewall isnt blocking https://www.github.com .", "EVON", MessageBoxButton.OK, MessageBoxImage.Hand);
          Process.GetCurrentProcess().Kill();
        }
      }
    }

    private void onDrop(object sender, DragEventArgs e)
    {
      try
      {
        foreach (string path in (string[]) e.Data.GetData(DataFormats.FileDrop))
        {
          if (System.IO.File.Exists(path) && (!(Path.GetExtension(path) != ".lua") || !(Path.GetExtension(path) != ".txt")))
            this.createTab(Path.GetFileName(path), System.IO.File.ReadAllText(path));
        }
      }
      catch
      {
        int num = (int) MessageBox.Show("Something happened while attempting to handle your drag-drop.");
      }
    }

    private void createTab(string name = "New Tab", string script = "")
    {
      try
      {
        name = name == "New Tab" ? string.Format("New Tab {0}", (object) (this.tabs.Items.Count + 1)) : name;
        TabItem tabItem = new TabItem();
        tabItem.Style = (Style) this.TryFindResource((object) "Tab");
        tabItem.BorderBrush = this.DefaultBrush;
        TextBox textBox = new TextBox();
        textBox.Text = name;
        textBox.IsHitTestVisible = false;
        textBox.IsEnabled = false;
        textBox.TextWrapping = TextWrapping.NoWrap;
        textBox.Style = (Style) this.TryFindResource((object) "InvisibleTextBox");
        textBox.MaxLength = 26;
        tabItem.Header = (object) textBox;
        TabItem tab = tabItem;
        tab.Content = (object) this.editor;
        tab.MouseLeftButtonDown += (MouseButtonEventHandler) ((a, e) =>
        {
          ((WebView) tab.Content).antiskid();
          ((WebView) tab.Content).minimap();
        });
        tab.MouseRightButtonDown += (MouseButtonEventHandler) ((_, __) =>
        {
          TextBox header = (TextBox) tab.Header;
          header.IsEnabled = true;
          header.Focus();
          header.SelectAll();
        });
        tab.Loaded += (RoutedEventHandler) ((loaded, yay) =>
        {
          TextBox tb = (TextBox) tab.Header;
          ((ButtonBase) tab.Template.FindName("CloseButton", (FrameworkElement) tab)).Click += (RoutedEventHandler) ((_, __) => this.tabs.Items.Remove((object) tab));
          tb.FocusableChanged += (DependencyPropertyChangedEventHandler) ((_, e) =>
          {
            if (tb.IsFocused)
              return;
            tb.IsEnabled = false;
          });
          tb.KeyDown += (KeyEventHandler) ((_, e) =>
          {
            if (e.Key != Key.Return)
              return;
            tb.IsEnabled = false;
          });
        });
        this.tabs.Items.Add((object) tab);
        this.tabs.SelectedItem = (object) tab;
        this.SetText(script);
      }
      catch
      {
      }
    }

    private void addTab(object sender, RoutedEventArgs e) => this.createTab();

    private void doExecute(object sender, RoutedEventArgs e)
    {
      if (this.Texts[(TabItem) this.tabs.SelectedItem] == null)
        return;
      Evon.Classes.Apis.Apis.Execute(this.GetText());
    }

    private void SetText(string text)
    {
      this.editor.SetText(text);
      TabItem selectedItem = (TabItem) this.tabs.SelectedItem;
      if (this.Texts.TryGetValue(selectedItem, out string _))
        this.Texts[selectedItem] = text;
      else
        this.Texts.Add(selectedItem, text);
    }

    private string GetText()
    {
      string str;
      return this.Texts.TryGetValue((TabItem) this.tabs.SelectedItem, out str) ? str : "";
    }

    private void doOpen(object sender, RoutedEventArgs e)
    {
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.Title = "EVON | Open Script";
      openFileDialog1.Filter = "Lua (*.lua) |*.lua|Text (*.txt) |*.txt";
      openFileDialog1.Multiselect = true;
      OpenFileDialog openFileDialog2 = openFileDialog1;
      bool? nullable = openFileDialog2.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      if (openFileDialog2.FileNames.Length > 1)
      {
        foreach (string fileName in openFileDialog2.FileNames)
          this.createTab(Path.GetFileName(fileName), System.IO.File.ReadAllText(fileName));
      }
      else if (this.tabs.SelectedItem != null && this.Texts[(TabItem) this.tabs.SelectedItem] != null)
        this.SetText(System.IO.File.ReadAllText(openFileDialog2.FileName));
      else
        this.createTab(Path.GetFileName(openFileDialog2.FileName), System.IO.File.ReadAllText(openFileDialog2.FileName));
    }

    private void doSave(object sender, RoutedEventArgs e)
    {
      SaveFileDialog saveFileDialog1 = new SaveFileDialog();
      saveFileDialog1.Title = "EVON | Save File";
      saveFileDialog1.Filter = "Lua (*.lua) |*.lua | Text Files (*.txt) |*.txt";
      SaveFileDialog saveFileDialog2 = saveFileDialog1;
      bool? nullable = saveFileDialog2.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      using (StreamWriter streamWriter = new StreamWriter((Stream) System.IO.File.OpenWrite(saveFileDialog2.FileName)))
      {
        if (this.tabs.SelectedItem == null)
          return;
        streamWriter.Write(this.Texts[(TabItem) this.tabs.SelectedItem]);
      }
    }

    private void doClear(object sender, RoutedEventArgs e) => this.SetText("");

    private async void doInject(object sender, RoutedEventArgs e)
    {
      this.pipeDelay = 0;
      this.injBtn.IsEnabled = false;
      Process[] processArray = Process.GetProcessesByName("injector-evon.exe");
      for (int index = 0; index < processArray.Length; ++index)
      {
        Process proc = processArray[index];
        proc.Kill();
        proc = (Process) null;
      }
      processArray = (Process[]) null;
      if (await Evon.Classes.Apis.Apis.Inject())
      {
        if (Evon.Classes.Apis.Apis.selected == 1)
        {
          while (!Evon.Classes.Apis.Apis.p.Exists())
          {
            if (this.pipeDelay >= 30000)
            {
              int num = (int) MessageBox.Show("Couldn't find the pipe for EVON within the selected time! Maybe try setting your no rename mode to true?");
              this.pipeDelay = 0;
              this.injBtn.IsEnabled = true;
              return;
            }
            await Task.Delay(200);
            this.pipeDelay += 200;
          }
        }
        this.showNotif("Wohoo!", "Evon Injected.");
        Evon.Classes.Apis.Apis.p.Write("setfpscap(" + (this.unlockfpscheck.IsChecked.Value ? "999" : "60") + ")");
        ((Storyboard) this.TryFindResource((object) "IconInjected")).Begin();
        while (Process.GetProcessesByName("RobloxPlayerBeta").Length != 0)
          await Task.Delay(1000);
        if (Process.GetProcessesByName("RobloxPlayerBeta").Length == 0)
          ((Storyboard) this.TryFindResource((object) "IconNotInjected")).Begin();
      }
      this.injBtn.IsEnabled = true;
    }

    private void handleShortcuts(object sender, KeyEventArgs e)
    {
      if (Keyboard.Modifiers != ModifierKeys.Control)
        return;
      switch (e.Key)
      {
        case Key.I:
          this.doInject(sender, (RoutedEventArgs) e);
          break;
        case Key.N:
          this.addTab(sender, (RoutedEventArgs) e);
          break;
        case Key.O:
          this.doOpen(sender, (RoutedEventArgs) e);
          break;
        case Key.Q:
          if (MessageBox.Show("Are you sure you want to close EVON?", "EVON", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
          {
            this.Close();
            break;
          }
          break;
        case Key.S:
          this.doSave(sender, (RoutedEventArgs) e);
          break;
      }
    }

    private void selectExecutor(object sender, RoutedEventArgs e) => ((Storyboard) this.TryFindResource((object) "selectExecution")).Begin();

    private void selectScripts(object sender, RoutedEventArgs e) => ((Storyboard) this.TryFindResource((object) nameof (selectScripts))).Begin();

    private void showSettings(object sender, RoutedEventArgs e) => ((Storyboard) this.TryFindResource((object) "selectSettings")).Begin();

    private void doDrag(object sender, MouseButtonEventArgs e) => this.DragMove();

    private void minimizeUI(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;

    private void closeUI(object sender, RoutedEventArgs e) => this.Close();

    private void doScripts(object sender, RoutedEventArgs e)
    {
      this.showScripts = !this.showScripts;
      ((Storyboard) this.TryFindResource(this.showScripts ? (object) "showScripts" : (object) "hideScripts")).Begin();
    }

    private void doSearch(object sender, TextChangedEventArgs e)
    {
      this.ScriptList.Items.Clear();
      foreach (string path in Directory.EnumerateFiles("scripts", "*.*", SearchOption.AllDirectories).Where<string>((Func<string, bool>) (script => script.ToLower().Contains(this.ListBoxSearch.Text.ToLower()))))
        this.ScriptList.Items.Add((object) Path.GetFileName(path));
    }

    private void doScriptSelection(object sender, SelectionChangedEventArgs e)
    {
      string selectedItem = (string) this.ScriptList.SelectedItem;
      if (!System.IO.File.Exists("scripts\\" + selectedItem))
        return;
      this.createTab(selectedItem, System.IO.File.ReadAllText("scripts\\" + selectedItem));
    }

    private void doSearchScripts(object sender, TextChangedEventArgs e)
    {
      this.pagenum = 1;
      this.searchingsystem = this.search.Text;
      this.reloadrScripts();
    }

    private void GameRefreshB_Click(object sender, RoutedEventArgs e)
    {
    }

    private void selectEvonAPI(object sender, RoutedEventArgs e)
    {
      Evon.Classes.Apis.Apis.selected = 1;
      foreach (ToggleButton toggleButton in Extensions.FindVisualChildren<CheckBox>((DependencyObject) this).Where<CheckBox>((Func<CheckBox, bool>) (b => b != (CheckBox) sender && b.Name.Contains("api_"))))
        toggleButton.IsChecked = new bool?(false);
      try
      {
        object obj1 = JsonConvert.DeserializeObject(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__45.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__45.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "defaultexec", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = Executor.\u003C\u003Eo__45.\u003C\u003Ep__0.Target((CallSite) Executor.\u003C\u003Eo__45.\u003C\u003Ep__0, obj1, Evon.Classes.Apis.Apis.selected);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__45.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__45.\u003C\u003Ep__2 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, Type, string, object> target = Executor.\u003C\u003Eo__45.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, Type, string, object>> p2 = Executor.\u003C\u003Eo__45.\u003C\u003Ep__2;
        Type type = typeof (System.IO.File);
        string str = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__45.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__45.\u003C\u003Ep__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = Executor.\u003C\u003Eo__45.\u003C\u003Ep__1.Target((CallSite) Executor.\u003C\u003Eo__45.\u003C\u003Ep__1, typeof (JsonConvert), obj1);
        target((CallSite) p2, type, str, obj3);
      }
      catch
      {
        int num = (int) MessageBox.Show("there was an error while trying to set your settings, please make sure nothing is using the settings file.");
      }
    }

    private void selectOxygenU(object sender, RoutedEventArgs e)
    {
      Evon.Classes.Apis.Apis.selected = 0;
      foreach (ToggleButton toggleButton in Extensions.FindVisualChildren<CheckBox>((DependencyObject) this).Where<CheckBox>((Func<CheckBox, bool>) (b => b != (CheckBox) sender && b.Name.Contains("api_"))))
        toggleButton.IsChecked = new bool?(false);
      try
      {
        object obj1 = JsonConvert.DeserializeObject(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__46.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__46.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "defaultexec", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = Executor.\u003C\u003Eo__46.\u003C\u003Ep__0.Target((CallSite) Executor.\u003C\u003Eo__46.\u003C\u003Ep__0, obj1, Evon.Classes.Apis.Apis.selected);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__46.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__46.\u003C\u003Ep__2 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, Type, string, object> target = Executor.\u003C\u003Eo__46.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, Type, string, object>> p2 = Executor.\u003C\u003Eo__46.\u003C\u003Ep__2;
        Type type = typeof (System.IO.File);
        string str = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__46.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__46.\u003C\u003Ep__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = Executor.\u003C\u003Eo__46.\u003C\u003Ep__1.Target((CallSite) Executor.\u003C\u003Eo__46.\u003C\u003Ep__1, typeof (JsonConvert), obj1);
        target((CallSite) p2, type, str, obj3);
      }
      catch
      {
        int num = (int) MessageBox.Show("there was an error while trying to set your settings, please make sure nothing is using the settings file.");
      }
    }

    private void selectFluxus(object sender, RoutedEventArgs e)
    {
      Evon.Classes.Apis.Apis.selected = 2;
      foreach (ToggleButton toggleButton in Extensions.FindVisualChildren<CheckBox>((DependencyObject) this).Where<CheckBox>((Func<CheckBox, bool>) (b => b != (CheckBox) sender && b.Name.Contains("api_"))))
        toggleButton.IsChecked = new bool?(false);
      try
      {
        object obj1 = JsonConvert.DeserializeObject(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__47.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__47.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "defaultexec", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = Executor.\u003C\u003Eo__47.\u003C\u003Ep__0.Target((CallSite) Executor.\u003C\u003Eo__47.\u003C\u003Ep__0, obj1, Evon.Classes.Apis.Apis.selected);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__47.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__47.\u003C\u003Ep__2 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, Type, string, object> target = Executor.\u003C\u003Eo__47.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, Type, string, object>> p2 = Executor.\u003C\u003Eo__47.\u003C\u003Ep__2;
        Type type = typeof (System.IO.File);
        string str = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__47.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__47.\u003C\u003Ep__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = Executor.\u003C\u003Eo__47.\u003C\u003Ep__1.Target((CallSite) Executor.\u003C\u003Eo__47.\u003C\u003Ep__1, typeof (JsonConvert), obj1);
        target((CallSite) p2, type, str, obj3);
      }
      catch
      {
        int num = (int) MessageBox.Show("there was an error while trying to set your settings, please make sure nothing is using the settings file.");
      }
    }

    private void selectKRNL(object sender, RoutedEventArgs e)
    {
      Evon.Classes.Apis.Apis.selected = 3;
      foreach (ToggleButton toggleButton in Extensions.FindVisualChildren<CheckBox>((DependencyObject) this).Where<CheckBox>((Func<CheckBox, bool>) (b => b != (CheckBox) sender && b.Name.Contains("api_"))))
        toggleButton.IsChecked = new bool?(false);
      try
      {
        object obj1 = JsonConvert.DeserializeObject(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__48.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__48.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "defaultexec", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = Executor.\u003C\u003Eo__48.\u003C\u003Ep__0.Target((CallSite) Executor.\u003C\u003Eo__48.\u003C\u003Ep__0, obj1, Evon.Classes.Apis.Apis.selected);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__48.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__48.\u003C\u003Ep__2 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, Type, string, object> target = Executor.\u003C\u003Eo__48.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, Type, string, object>> p2 = Executor.\u003C\u003Eo__48.\u003C\u003Ep__2;
        Type type = typeof (System.IO.File);
        string str = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__48.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__48.\u003C\u003Ep__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = Executor.\u003C\u003Eo__48.\u003C\u003Ep__1.Target((CallSite) Executor.\u003C\u003Eo__48.\u003C\u003Ep__1, typeof (JsonConvert), obj1);
        target((CallSite) p2, type, str, obj3);
      }
      catch
      {
        int num = (int) MessageBox.Show("there was an error while trying to set your settings, please make sure nothing is using the settings file.");
      }
    }

    private async Task<string> getVersion()
    {
      try
      {
        using (WebClient wc = new WebClient())
        {
          string str = await wc.DownloadStringTaskAsync("https://clientsettingscdn.roblox.com/v2/client-version/WindowsPlayer");
          object res = JsonConvert.DeserializeObject(str);
          str = (string) null;
          // ISSUE: reference to a compiler-generated field
          if (Executor.\u003C\u003Eo__49.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Executor.\u003C\u003Eo__49.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, bool> target1 = Executor.\u003C\u003Eo__49.\u003C\u003Ep__2.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, bool>> p2 = Executor.\u003C\u003Eo__49.\u003C\u003Ep__2;
          // ISSUE: reference to a compiler-generated field
          if (Executor.\u003C\u003Eo__49.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Executor.\u003C\u003Eo__49.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object, object> target2 = Executor.\u003C\u003Eo__49.\u003C\u003Ep__1.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object, object>> p1 = Executor.\u003C\u003Eo__49.\u003C\u003Ep__1;
          // ISSUE: reference to a compiler-generated field
          if (Executor.\u003C\u003Eo__49.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Executor.\u003C\u003Eo__49.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "clientVersionUpload", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj1 = Executor.\u003C\u003Eo__49.\u003C\u003Ep__0.Target((CallSite) Executor.\u003C\u003Eo__49.\u003C\u003Ep__0, res);
          object obj2 = target2((CallSite) p1, obj1, (object) null);
          if (target1((CallSite) p2, obj2))
          {
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__49.\u003C\u003Ep__5 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__49.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (Executor)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, string> target3 = Executor.\u003C\u003Eo__49.\u003C\u003Ep__5.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, string>> p5 = Executor.\u003C\u003Eo__49.\u003C\u003Ep__5;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__49.\u003C\u003Ep__4 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__49.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object> target4 = Executor.\u003C\u003Eo__49.\u003C\u003Ep__4.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object>> p4 = Executor.\u003C\u003Eo__49.\u003C\u003Ep__4;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__49.\u003C\u003Ep__3 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__49.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "clientVersionUpload", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj3 = Executor.\u003C\u003Eo__49.\u003C\u003Ep__3.Target((CallSite) Executor.\u003C\u003Eo__49.\u003C\u003Ep__3, res);
            object obj4 = target4((CallSite) p4, obj3);
            return target3((CallSite) p5, obj4);
          }
          res = (object) null;
        }
      }
      catch
      {
        int num = (int) MessageBox.Show("Failed to get the ROBLOX Version! Please make sure you have a valid internet connection and your connection to roblox.com is correct.");
        return (string) null;
      }
      return (string) null;
    }

    private async void revert268(object sender, RoutedEventArgs e)
    {
      this.revertbtn.IsEnabled = false;
      this.showNotif("Wohoo!", "268 Fixes Reverted.");
      this.revertbtn.IsEnabled = true;
    }

    private async void fix268(object sender, RoutedEventArgs e)
    {
      this.fixbtn.IsEnabled = false;
      Process[] processArray = Process.GetProcessesByName("RobloxPlayerBeta");
      for (int index = 0; index < processArray.Length; ++index)
      {
        Process process = processArray[index];
        process.Kill();
        process = (Process) null;
      }
      processArray = (Process[]) null;
      this.showNotif("Wohoo!", "268 Fixes Applied.");
      this.fixbtn.IsEnabled = true;
    }

    private void killRoblox(object sender, RoutedEventArgs e)
    {
      foreach (Process process in Process.GetProcessesByName("RobloxPlayerBeta"))
        process.Kill();
      this.showNotif("Wohoo!", "Closed all open Roblox processes.");
    }

    private void doUnlockFPS(object sender, RoutedEventArgs e)
    {
      try
      {
        object obj1 = JsonConvert.DeserializeObject(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__53.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__53.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, bool?, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "unlockfps", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = Executor.\u003C\u003Eo__53.\u003C\u003Ep__0.Target((CallSite) Executor.\u003C\u003Eo__53.\u003C\u003Ep__0, obj1, this.unlockfpscheck.IsChecked);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__53.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__53.\u003C\u003Ep__2 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, Type, string, object> target = Executor.\u003C\u003Eo__53.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, Type, string, object>> p2 = Executor.\u003C\u003Eo__53.\u003C\u003Ep__2;
        Type type = typeof (System.IO.File);
        string str = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__53.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__53.\u003C\u003Ep__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = Executor.\u003C\u003Eo__53.\u003C\u003Ep__1.Target((CallSite) Executor.\u003C\u003Eo__53.\u003C\u003Ep__1, typeof (JsonConvert), obj1);
        target((CallSite) p2, type, str, obj3);
      }
      catch
      {
        int num = (int) MessageBox.Show("there was an error while trying to set your settings, please make sure nothing is using the settings file.");
      }
      if (!Evon.Classes.Apis.Apis.p.Exists())
        return;
      Evon.Classes.Apis.Apis.p.Write("setfpscap(" + (this.unlockfpscheck.IsChecked.Value ? "999" : "60") + ")");
    }

    private void doAutoInject(object sender, RoutedEventArgs e)
    {
      try
      {
        object obj1 = JsonConvert.DeserializeObject(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__54.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__54.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, bool?, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "autoinject", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = Executor.\u003C\u003Eo__54.\u003C\u003Ep__0.Target((CallSite) Executor.\u003C\u003Eo__54.\u003C\u003Ep__0, obj1, this.autoinjectcheck.IsChecked);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__54.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__54.\u003C\u003Ep__2 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, Type, string, object> target = Executor.\u003C\u003Eo__54.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, Type, string, object>> p2 = Executor.\u003C\u003Eo__54.\u003C\u003Ep__2;
        Type type = typeof (System.IO.File);
        string str = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__54.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__54.\u003C\u003Ep__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = Executor.\u003C\u003Eo__54.\u003C\u003Ep__1.Target((CallSite) Executor.\u003C\u003Eo__54.\u003C\u003Ep__1, typeof (JsonConvert), obj1);
        target((CallSite) p2, type, str, obj3);
      }
      catch
      {
        int num = (int) MessageBox.Show("there was an error while trying to set your settings, please make sure nothing is using the settings file.");
      }
      if (this.autoinjectcheck.IsChecked.Value)
        this.w.Start();
      else
        this.w.Stop();
    }

    private void doMinimap(object sender, RoutedEventArgs e)
    {
      try
      {
        object obj1 = JsonConvert.DeserializeObject(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__55.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__55.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, bool?, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "minimap", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = Executor.\u003C\u003Eo__55.\u003C\u003Ep__0.Target((CallSite) Executor.\u003C\u003Eo__55.\u003C\u003Ep__0, obj1, this.minimapCheck.IsChecked);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__55.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__55.\u003C\u003Ep__2 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, Type, string, object> target = Executor.\u003C\u003Eo__55.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, Type, string, object>> p2 = Executor.\u003C\u003Eo__55.\u003C\u003Ep__2;
        Type type = typeof (System.IO.File);
        string str = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__55.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__55.\u003C\u003Ep__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = Executor.\u003C\u003Eo__55.\u003C\u003Ep__1.Target((CallSite) Executor.\u003C\u003Eo__55.\u003C\u003Ep__1, typeof (JsonConvert), obj1);
        target((CallSite) p2, type, str, obj3);
        foreach (ContentControl contentControl in (IEnumerable) this.tabs.Items)
          ((WebView) contentControl.Content).minimap();
        this.editor?.minimap();
      }
      catch
      {
        int num = (int) MessageBox.Show("there was an error while trying to set your settings, please make sure nothing is using the settings file.");
      }
    }

    private void doAntiSkid(object sender, RoutedEventArgs e)
    {
      try
      {
        object obj1 = JsonConvert.DeserializeObject(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__56.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__56.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, bool?, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "antiskid", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = Executor.\u003C\u003Eo__56.\u003C\u003Ep__0.Target((CallSite) Executor.\u003C\u003Eo__56.\u003C\u003Ep__0, obj1, this.antiskidcheck.IsChecked);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__56.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__56.\u003C\u003Ep__2 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, Type, string, object> target = Executor.\u003C\u003Eo__56.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, Type, string, object>> p2 = Executor.\u003C\u003Eo__56.\u003C\u003Ep__2;
        Type type = typeof (System.IO.File);
        string str = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__56.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__56.\u003C\u003Ep__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = Executor.\u003C\u003Eo__56.\u003C\u003Ep__1.Target((CallSite) Executor.\u003C\u003Eo__56.\u003C\u003Ep__1, typeof (JsonConvert), obj1);
        target((CallSite) p2, type, str, obj3);
        foreach (ContentControl contentControl in (IEnumerable) this.tabs.Items)
          ((WebView) contentControl.Content).antiskid();
        this.editor?.antiskid();
      }
      catch
      {
        int num = (int) MessageBox.Show("there was an error while trying to set your settings, please make sure nothing is using the settings file.");
      }
    }

    private void doTopMost(object sender, RoutedEventArgs e)
    {
      try
      {
        object obj1 = JsonConvert.DeserializeObject(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon"));
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__57.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__57.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, bool?, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "topmost", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = Executor.\u003C\u003Eo__57.\u003C\u003Ep__0.Target((CallSite) Executor.\u003C\u003Eo__57.\u003C\u003Ep__0, obj1, this.TopMostCheck.IsChecked);
        this.Topmost = this.TopMostCheck.IsChecked.Value;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__57.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__57.\u003C\u003Ep__2 = CallSite<Action<CallSite, Type, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteAllText", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, Type, string, object> target = Executor.\u003C\u003Eo__57.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, Type, string, object>> p2 = Executor.\u003C\u003Eo__57.\u003C\u003Ep__2;
        Type type = typeof (System.IO.File);
        string str = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\settings.evon";
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__57.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__57.\u003C\u003Ep__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = Executor.\u003C\u003Eo__57.\u003C\u003Ep__1.Target((CallSite) Executor.\u003C\u003Eo__57.\u003C\u003Ep__1, typeof (JsonConvert), obj1);
        target((CallSite) p2, type, str, obj3);
      }
      catch
      {
        int num = (int) MessageBox.Show("there was an error while trying to set your settings, please make sure nothing is using the settings file.");
      }
    }

    private async void onLoaded(object sender, RoutedEventArgs e)
    {
      try
      {
        await ((Storyboard) this.TryFindResource((object) "loaded")).Start();
        string[] strArray = Directory.GetFiles("bin\\tabs");
        for (int index = 0; index < strArray.Length; ++index)
        {
          string file = strArray[index];
          try
          {
            this.createTab(Path.GetFileName(file), System.IO.File.ReadAllText(file));
          }
          catch
          {
          }
          file = (string) null;
        }
        strArray = (string[]) null;
        if (this.tabs.Items.Count < 1)
          this.createTab();
      }
      catch
      {
      }
      this.reloadScripts();
    }

    private void onClosing(object sender, CancelEventArgs e)
    {
      TabItem selectedItem = (TabItem) this.tabs.SelectedItem;
      if (selectedItem != null)
      {
        if (this.Texts.TryGetValue(selectedItem, out string _))
          this.Texts[selectedItem] = this.GetText();
        else
          this.Texts.Add(selectedItem, this.GetText());
      }
      foreach (KeyValuePair<TabItem, string> text in this.Texts)
        System.IO.File.WriteAllText("./bin/tabs/" + ((TextBox) text.Key.Header).Text, text.Value);
      Process.GetCurrentProcess().Kill();
    }

    private void sViewMouseMove(object sender, MouseEventArgs e)
    {
    }

    private void onSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.RemovedItems.Count > 0)
      {
        foreach (TabItem removedItem in (IEnumerable) e.RemovedItems)
        {
          if (this.Texts.TryGetValue(removedItem, out string _))
            this.Texts[removedItem] = this.editor.Text;
          else
            this.Texts.Add(removedItem, this.editor.Text);
        }
      }
      e.Handled = true;
      if (this.tabs.SelectedItem == null)
        return;
      string text;
      if (this.Texts.TryGetValue((TabItem) this.tabs.SelectedItem, out text))
        this.editor.SetText(text);
      else
        this.editor.SetText("");
    }

    private void docpl(object sender, RoutedEventArgs e)
    {
      if (this.pagenum < 1)
      {
        int num = (int) MessageBox.Show("You cannot go back any further.");
      }
      else
      {
        --this.pagenum;
        this.reloadrScripts();
      }
    }

    private void docpr(object sender, RoutedEventArgs e)
    {
      ++this.pagenum;
      this.reloadrScripts();
    }

    public static void SetColor(PortableColorPicker s, SolidColorBrush r)
    {
      ((PickerControlBase) s).Color.RGB_R = (double) r.Color.R;
      ((PickerControlBase) s).Color.RGB_G = (double) r.Color.G;
      ((PickerControlBase) s).Color.RGB_B = (double) r.Color.B;
      ((PickerControlBase) s).Color.A = (double) byte.MaxValue;
    }

    private void setcolor()
    {
      try
      {
        if (!System.IO.File.Exists("./bin/theme.evon"))
        {
          Executor.ThemeStrings themeStrings = new Executor.ThemeStrings()
          {
            Theme = new List<Executor.ThemeStrings.ThemeSystem>()
            {
              new Executor.ThemeStrings.ThemeSystem()
              {
                ThemeManufacturer = "Evon",
                Color1 = "#FF9700FF",
                TextboxImage = ""
              }
            }
          };
          try
          {
            System.IO.File.WriteAllText("./bin/theme.evon", JsonConvert.SerializeObject((object) themeStrings, (Formatting) 1));
          }
          catch
          {
          }
        }
        object obj1 = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("./bin/theme.evon"));
        BrushConverter brushConverter1 = new BrushConverter();
        Border shadowVibe = this.ShadowVibe;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, Brush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (Brush), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, Brush> target1 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, Brush>> p5 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__5;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__4 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, BrushConverter, object, object> target2 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__4.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, BrushConverter, object, object>> p4 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__4;
        BrushConverter brushConverter2 = brushConverter1;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target3 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p3 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target4 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p2 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__2;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target5 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p1 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__0.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__0, obj1);
        object obj3 = target5((CallSite) p1, obj2, 0);
        object obj4 = target4((CallSite) p2, obj3);
        object obj5 = target3((CallSite) p3, obj4);
        object obj6 = target2((CallSite) p4, brushConverter2, obj5);
        Brush brush1 = target1((CallSite) p5, obj6);
        shadowVibe.Background = brush1;
        foreach (DependencyObject visualChild1 in Extensions.FindVisualChildren<Border>((DependencyObject) this.settingsViewWindow))
        {
          foreach (CheckBox visualChild2 in Extensions.FindVisualChildren<CheckBox>(visualChild1))
          {
            CheckBox checkBox1 = visualChild2;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__11 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, Brush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (Brush), typeof (Executor)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, Brush> target6 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__11.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, Brush>> p11 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__11;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__10 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__10 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, BrushConverter, object, object> target7 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__10.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, BrushConverter, object, object>> p10 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__10;
            BrushConverter brushConverter3 = brushConverter1;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__9 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object> target8 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__9.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object>> p9 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__9;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__8 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object> target9 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__8.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object>> p8 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__8;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__7 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, int, object> target10 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__7.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, int, object>> p7 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__7;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__6 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj7 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__6.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__6, obj1);
            object obj8 = target10((CallSite) p7, obj7, 0);
            object obj9 = target9((CallSite) p8, obj8);
            object obj10 = target8((CallSite) p9, obj9);
            object obj11 = target7((CallSite) p10, brushConverter3, obj10);
            Brush brush2 = target6((CallSite) p11, obj11);
            checkBox1.Background = brush2;
            CheckBox checkBox2 = visualChild2;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__17 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, Brush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (Brush), typeof (Executor)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, Brush> target11 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__17.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, Brush>> p17 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__17;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__16 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__16 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, BrushConverter, object, object> target12 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__16.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, BrushConverter, object, object>> p16 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__16;
            BrushConverter brushConverter4 = brushConverter1;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__15 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object> target13 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__15.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object>> p15 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__15;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__14 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object> target14 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__14.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object>> p14 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__14;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__13 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, int, object> target15 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__13.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, int, object>> p13 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__13;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__12 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj12 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__12.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__12, obj1);
            object obj13 = target15((CallSite) p13, obj12, 0);
            object obj14 = target14((CallSite) p14, obj13);
            object obj15 = target13((CallSite) p15, obj14);
            object obj16 = target12((CallSite) p16, brushConverter4, obj15);
            Brush brush3 = target11((CallSite) p17, obj16);
            checkBox2.BorderBrush = brush3;
          }
        }
        PortableColorPicker colr = this.colr;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__23 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, System.Windows.Media.Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (System.Windows.Media.Color), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, System.Windows.Media.Color> target16 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__23.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, System.Windows.Media.Color>> p23 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__23;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__22 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__22 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target17 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__22.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p22 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__22;
        Type type1 = typeof (ColorConverter);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__21 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target18 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__21.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p21 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__21;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__20 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target19 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__20.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p20 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__20;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__19 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target20 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__19.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p19 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__19;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__18 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj17 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__18.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__18, obj1);
        object obj18 = target20((CallSite) p19, obj17, 0);
        object obj19 = target19((CallSite) p20, obj18);
        object obj20 = target18((CallSite) p21, obj19);
        object obj21 = target17((CallSite) p22, type1, obj20);
        SolidColorBrush r = new SolidColorBrush(target16((CallSite) p23, obj21));
        Executor.SetColor(colr, r);
        foreach (DependencyObject visualChild3 in Extensions.FindVisualChildren<Border>((DependencyObject) this.ApiPanerl))
        {
          foreach (CheckBox visualChild4 in Extensions.FindVisualChildren<CheckBox>(visualChild3))
          {
            CheckBox checkBox3 = visualChild4;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__29 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__29 = CallSite<Func<CallSite, object, Brush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (Brush), typeof (Executor)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, Brush> target21 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__29.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, Brush>> p29 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__29;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__28 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__28 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, BrushConverter, object, object> target22 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__28.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, BrushConverter, object, object>> p28 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__28;
            BrushConverter brushConverter5 = brushConverter1;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__27 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object> target23 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__27.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object>> p27 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__27;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__26 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object> target24 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__26.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object>> p26 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__26;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__25 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__25 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, int, object> target25 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__25.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, int, object>> p25 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__25;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__24 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj22 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__24.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__24, obj1);
            object obj23 = target25((CallSite) p25, obj22, 0);
            object obj24 = target24((CallSite) p26, obj23);
            object obj25 = target23((CallSite) p27, obj24);
            object obj26 = target22((CallSite) p28, brushConverter5, obj25);
            Brush brush4 = target21((CallSite) p29, obj26);
            checkBox3.Background = brush4;
            CheckBox checkBox4 = visualChild4;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__35 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__35 = CallSite<Func<CallSite, object, Brush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (Brush), typeof (Executor)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, Brush> target26 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__35.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, Brush>> p35 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__35;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__34 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__34 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, BrushConverter, object, object> target27 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__34.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, BrushConverter, object, object>> p34 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__34;
            BrushConverter brushConverter6 = brushConverter1;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__33 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__33 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object> target28 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__33.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object>> p33 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__33;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__32 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object> target29 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__32.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object>> p32 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__32;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__31 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__31 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, int, object> target30 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__31.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, int, object>> p31 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__31;
            // ISSUE: reference to a compiler-generated field
            if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__30 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Executor.\u003C\u003Eo__65.\u003C\u003Ep__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj27 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__30.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__30, obj1);
            object obj28 = target30((CallSite) p31, obj27, 0);
            object obj29 = target29((CallSite) p32, obj28);
            object obj30 = target28((CallSite) p33, obj29);
            object obj31 = target27((CallSite) p34, brushConverter6, obj30);
            Brush brush5 = target26((CallSite) p35, obj31);
            checkBox4.BorderBrush = brush5;
          }
        }
        DropShadowEffect dropShadowEffect1 = this._1e;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__41 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__41 = CallSite<Func<CallSite, object, System.Windows.Media.Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (System.Windows.Media.Color), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, System.Windows.Media.Color> target31 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__41.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, System.Windows.Media.Color>> p41 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__41;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__40 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__40 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target32 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__40.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p40 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__40;
        Type type2 = typeof (ColorConverter);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__39 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__39 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target33 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__39.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p39 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__39;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__38 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target34 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__38.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p38 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__38;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__37 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__37 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target35 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__37.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p37 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__37;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__36 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__36 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj32 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__36.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__36, obj1);
        object obj33 = target35((CallSite) p37, obj32, 0);
        object obj34 = target34((CallSite) p38, obj33);
        object obj35 = target33((CallSite) p39, obj34);
        object obj36 = target32((CallSite) p40, type2, obj35);
        System.Windows.Media.Color color1 = target31((CallSite) p41, obj36);
        dropShadowEffect1.Color = color1;
        DropShadowEffect dropShadowEffect2 = this._2e;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__47 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__47 = CallSite<Func<CallSite, object, System.Windows.Media.Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (System.Windows.Media.Color), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, System.Windows.Media.Color> target36 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__47.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, System.Windows.Media.Color>> p47 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__47;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__46 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__46 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target37 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__46.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p46 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__46;
        Type type3 = typeof (ColorConverter);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__45 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__45 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target38 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__45.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p45 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__45;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__44 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__44 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target39 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__44.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p44 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__44;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__43 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__43 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target40 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__43.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p43 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__43;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__42 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj37 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__42.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__42, obj1);
        object obj38 = target40((CallSite) p43, obj37, 0);
        object obj39 = target39((CallSite) p44, obj38);
        object obj40 = target38((CallSite) p45, obj39);
        object obj41 = target37((CallSite) p46, type3, obj40);
        System.Windows.Media.Color color2 = target36((CallSite) p47, obj41);
        dropShadowEffect2.Color = color2;
        DropShadowEffect dropShadowEffect3 = this._3e;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__53 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__53 = CallSite<Func<CallSite, object, System.Windows.Media.Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (System.Windows.Media.Color), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, System.Windows.Media.Color> target41 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__53.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, System.Windows.Media.Color>> p53 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__53;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__52 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__52 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target42 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__52.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p52 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__52;
        Type type4 = typeof (ColorConverter);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__51 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__51 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target43 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__51.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p51 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__51;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__50 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__50 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target44 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__50.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p50 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__50;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__49 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__49 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target45 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__49.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p49 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__49;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__48 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__48 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj42 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__48.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__48, obj1);
        object obj43 = target45((CallSite) p49, obj42, 0);
        object obj44 = target44((CallSite) p50, obj43);
        object obj45 = target43((CallSite) p51, obj44);
        object obj46 = target42((CallSite) p52, type4, obj45);
        System.Windows.Media.Color color3 = target41((CallSite) p53, obj46);
        dropShadowEffect3.Color = color3;
        DropShadowEffect dropShadowEffect4 = this._4e;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__59 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__59 = CallSite<Func<CallSite, object, System.Windows.Media.Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (System.Windows.Media.Color), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, System.Windows.Media.Color> target46 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__59.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, System.Windows.Media.Color>> p59 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__59;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__58 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__58 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target47 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__58.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p58 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__58;
        Type type5 = typeof (ColorConverter);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__57 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__57 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target48 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__57.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p57 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__57;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__56 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__56 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target49 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__56.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p56 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__56;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__55 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__55 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target50 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__55.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p55 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__55;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__54 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__54 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj47 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__54.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__54, obj1);
        object obj48 = target50((CallSite) p55, obj47, 0);
        object obj49 = target49((CallSite) p56, obj48);
        object obj50 = target48((CallSite) p57, obj49);
        object obj51 = target47((CallSite) p58, type5, obj50);
        System.Windows.Media.Color color4 = target46((CallSite) p59, obj51);
        dropShadowEffect4.Color = color4;
        DropShadowEffect shadowNotifColor = this.ShadowNotifColor;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__65 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__65 = CallSite<Func<CallSite, object, System.Windows.Media.Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (System.Windows.Media.Color), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, System.Windows.Media.Color> target51 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__65.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, System.Windows.Media.Color>> p65 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__65;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__64 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__64 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target52 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__64.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p64 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__64;
        Type type6 = typeof (ColorConverter);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__63 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__63 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target53 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__63.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p63 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__63;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__62 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__62 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target54 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__62.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p62 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__62;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__61 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__61 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target55 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__61.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p61 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__61;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__60 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__60 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj52 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__60.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__60, obj1);
        object obj53 = target55((CallSite) p61, obj52, 0);
        object obj54 = target54((CallSite) p62, obj53);
        object obj55 = target53((CallSite) p63, obj54);
        object obj56 = target52((CallSite) p64, type6, obj55);
        System.Windows.Media.Color color5 = target51((CallSite) p65, obj56);
        shadowNotifColor.Color = color5;
        Border notifBar = this.notifBar;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__71 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__71 = CallSite<Func<CallSite, object, System.Windows.Media.Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (System.Windows.Media.Color), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, System.Windows.Media.Color> target56 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__71.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, System.Windows.Media.Color>> p71 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__71;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__70 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__70 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target57 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__70.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p70 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__70;
        Type type7 = typeof (ColorConverter);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__69 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__69 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target58 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__69.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p69 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__69;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__68 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__68 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target59 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__68.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p68 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__68;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__67 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__67 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target60 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__67.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p67 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__67;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__66 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__66 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj57 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__66.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__66, obj1);
        object obj58 = target60((CallSite) p67, obj57, 0);
        object obj59 = target59((CallSite) p68, obj58);
        object obj60 = target58((CallSite) p69, obj59);
        object obj61 = target57((CallSite) p70, type7, obj60);
        SolidColorBrush solidColorBrush = new SolidColorBrush(target56((CallSite) p71, obj61));
        notifBar.Background = (Brush) solidColorBrush;
        DropShadowEffect dsB1 = this.DSB1;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__77 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__77 = CallSite<Func<CallSite, object, System.Windows.Media.Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (System.Windows.Media.Color), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, System.Windows.Media.Color> target61 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__77.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, System.Windows.Media.Color>> p77 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__77;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__76 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__76 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target62 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__76.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p76 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__76;
        Type type8 = typeof (ColorConverter);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__75 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__75 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target63 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__75.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p75 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__75;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__74 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__74 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target64 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__74.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p74 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__74;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__73 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__73 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target65 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__73.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p73 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__73;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__72 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__72 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj62 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__72.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__72, obj1);
        object obj63 = target65((CallSite) p73, obj62, 0);
        object obj64 = target64((CallSite) p74, obj63);
        object obj65 = target63((CallSite) p75, obj64);
        object obj66 = target62((CallSite) p76, type8, obj65);
        System.Windows.Media.Color color6 = target61((CallSite) p77, obj66);
        dsB1.Color = color6;
        DropShadowEffect dsB2 = this.DSB2;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__83 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__83 = CallSite<Func<CallSite, object, System.Windows.Media.Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (System.Windows.Media.Color), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, System.Windows.Media.Color> target66 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__83.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, System.Windows.Media.Color>> p83 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__83;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__82 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__82 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target67 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__82.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p82 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__82;
        Type type9 = typeof (ColorConverter);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__81 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__81 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target68 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__81.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p81 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__81;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__80 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__80 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target69 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__80.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p80 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__80;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__79 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__79 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target70 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__79.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p79 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__79;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__78 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__78 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj67 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__78.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__78, obj1);
        object obj68 = target70((CallSite) p79, obj67, 0);
        object obj69 = target69((CallSite) p80, obj68);
        object obj70 = target68((CallSite) p81, obj69);
        object obj71 = target67((CallSite) p82, type9, obj70);
        System.Windows.Media.Color color7 = target66((CallSite) p83, obj71);
        dsB2.Color = color7;
        DropShadowEffect dsB3 = this.DSB3;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__89 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__89 = CallSite<Func<CallSite, object, System.Windows.Media.Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (System.Windows.Media.Color), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, System.Windows.Media.Color> target71 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__89.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, System.Windows.Media.Color>> p89 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__89;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__88 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__88 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target72 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__88.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p88 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__88;
        Type type10 = typeof (ColorConverter);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__87 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__87 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target73 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__87.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p87 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__87;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__86 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__86 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target74 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__86.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p86 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__86;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__85 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__85 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target75 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__85.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p85 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__85;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__84 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__84 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj72 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__84.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__84, obj1);
        object obj73 = target75((CallSite) p85, obj72, 0);
        object obj74 = target74((CallSite) p86, obj73);
        object obj75 = target73((CallSite) p87, obj74);
        object obj76 = target72((CallSite) p88, type10, obj75);
        System.Windows.Media.Color color8 = target71((CallSite) p89, obj76);
        dsB3.Color = color8;
        DropShadowEffect dsB4 = this.DSB4;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__95 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__95 = CallSite<Func<CallSite, object, System.Windows.Media.Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (System.Windows.Media.Color), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, System.Windows.Media.Color> target76 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__95.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, System.Windows.Media.Color>> p95 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__95;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__94 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__94 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target77 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__94.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p94 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__94;
        Type type11 = typeof (ColorConverter);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__93 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__93 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target78 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__93.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p93 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__93;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__92 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__92 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target79 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__92.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p92 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__92;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__91 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__91 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target80 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__91.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p91 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__91;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__90 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__90 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj77 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__90.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__90, obj1);
        object obj78 = target80((CallSite) p91, obj77, 0);
        object obj79 = target79((CallSite) p92, obj78);
        object obj80 = target78((CallSite) p93, obj79);
        object obj81 = target77((CallSite) p94, type11, obj80);
        System.Windows.Media.Color color9 = target76((CallSite) p95, obj81);
        dsB4.Color = color9;
        DropShadowEffect dsB5 = this.DSB5;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__101 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__101 = CallSite<Func<CallSite, object, System.Windows.Media.Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (System.Windows.Media.Color), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, System.Windows.Media.Color> target81 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__101.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, System.Windows.Media.Color>> p101 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__101;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__100 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__100 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target82 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__100.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p100 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__100;
        Type type12 = typeof (ColorConverter);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__99 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__99 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target83 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__99.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p99 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__99;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__98 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__98 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target84 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__98.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p98 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__98;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__97 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__97 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target85 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__97.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p97 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__97;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__96 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__96 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj82 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__96.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__96, obj1);
        object obj83 = target85((CallSite) p97, obj82, 0);
        object obj84 = target84((CallSite) p98, obj83);
        object obj85 = target83((CallSite) p99, obj84);
        object obj86 = target82((CallSite) p100, type12, obj85);
        System.Windows.Media.Color color10 = target81((CallSite) p101, obj86);
        dsB5.Color = color10;
        DropShadowEffect dsB6 = this.DSB6;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__107 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__107 = CallSite<Func<CallSite, object, System.Windows.Media.Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (System.Windows.Media.Color), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, System.Windows.Media.Color> target86 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__107.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, System.Windows.Media.Color>> p107 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__107;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__106 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__106 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target87 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__106.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p106 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__106;
        Type type13 = typeof (ColorConverter);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__105 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__105 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target88 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__105.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p105 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__105;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__104 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__104 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target89 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__104.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p104 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__104;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__103 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__103 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target90 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__103.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p103 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__103;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__102 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__102 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj87 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__102.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__102, obj1);
        object obj88 = target90((CallSite) p103, obj87, 0);
        object obj89 = target89((CallSite) p104, obj88);
        object obj90 = target88((CallSite) p105, obj89);
        object obj91 = target87((CallSite) p106, type13, obj90);
        System.Windows.Media.Color color11 = target86((CallSite) p107, obj91);
        dsB6.Color = color11;
        DropShadowEffect dsB7 = this.DSB7;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__113 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__113 = CallSite<Func<CallSite, object, System.Windows.Media.Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (System.Windows.Media.Color), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, System.Windows.Media.Color> target91 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__113.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, System.Windows.Media.Color>> p113 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__113;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__112 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__112 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target92 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__112.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p112 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__112;
        Type type14 = typeof (ColorConverter);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__111 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__111 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target93 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__111.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p111 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__111;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__110 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__110 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target94 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__110.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p110 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__110;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__109 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__109 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target95 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__109.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p109 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__109;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__108 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__108 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj92 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__108.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__108, obj1);
        object obj93 = target95((CallSite) p109, obj92, 0);
        object obj94 = target94((CallSite) p110, obj93);
        object obj95 = target93((CallSite) p111, obj94);
        object obj96 = target92((CallSite) p112, type14, obj95);
        System.Windows.Media.Color color12 = target91((CallSite) p113, obj96);
        dsB7.Color = color12;
        DropShadowEffect dsB8 = this.DSB8;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__119 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__119 = CallSite<Func<CallSite, object, System.Windows.Media.Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (System.Windows.Media.Color), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, System.Windows.Media.Color> target96 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__119.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, System.Windows.Media.Color>> p119 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__119;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__118 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__118 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target97 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__118.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p118 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__118;
        Type type15 = typeof (ColorConverter);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__117 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__117 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target98 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__117.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p117 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__117;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__116 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__116 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target99 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__116.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p116 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__116;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__115 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__115 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target100 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__115.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p115 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__115;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__114 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__114 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj97 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__114.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__114, obj1);
        object obj98 = target100((CallSite) p115, obj97, 0);
        object obj99 = target99((CallSite) p116, obj98);
        object obj100 = target98((CallSite) p117, obj99);
        object obj101 = target97((CallSite) p118, type15, obj100);
        System.Windows.Media.Color color13 = target96((CallSite) p119, obj101);
        dsB8.Color = color13;
        DropShadowEffect dsB9 = this.DSB9;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__125 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__125 = CallSite<Func<CallSite, object, System.Windows.Media.Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (System.Windows.Media.Color), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, System.Windows.Media.Color> target101 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__125.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, System.Windows.Media.Color>> p125 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__125;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__124 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__124 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target102 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__124.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p124 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__124;
        Type type16 = typeof (ColorConverter);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__123 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__123 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target103 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__123.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p123 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__123;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__122 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__122 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target104 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__122.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p122 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__122;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__121 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__121 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target105 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__121.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p121 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__121;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__120 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__120 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj102 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__120.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__120, obj1);
        object obj103 = target105((CallSite) p121, obj102, 0);
        object obj104 = target104((CallSite) p122, obj103);
        object obj105 = target103((CallSite) p123, obj104);
        object obj106 = target102((CallSite) p124, type16, obj105);
        System.Windows.Media.Color color14 = target101((CallSite) p125, obj106);
        dsB9.Color = color14;
        TextBox listBoxSearch = this.ListBoxSearch;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__131 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__131 = CallSite<Func<CallSite, object, Brush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (Brush), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, Brush> target106 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__131.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, Brush>> p131 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__131;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__130 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__130 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, BrushConverter, object, object> target107 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__130.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, BrushConverter, object, object>> p130 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__130;
        BrushConverter brushConverter7 = brushConverter1;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__129 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__129 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target108 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__129.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p129 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__129;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__128 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__128 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target109 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__128.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p128 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__128;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__127 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__127 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target110 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__127.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p127 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__127;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__126 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__126 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj107 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__126.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__126, obj1);
        object obj108 = target110((CallSite) p127, obj107, 0);
        object obj109 = target109((CallSite) p128, obj108);
        object obj110 = target108((CallSite) p129, obj109);
        object obj111 = target107((CallSite) p130, brushConverter7, obj110);
        Brush brush6 = target106((CallSite) p131, obj111);
        listBoxSearch.CaretBrush = brush6;
        this.search.CaretBrush = this.ListBoxSearch.CaretBrush;
        this.search.BorderBrush = this.ListBoxSearch.CaretBrush;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__137 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__137 = CallSite<Func<CallSite, object, System.Windows.Media.Color>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (System.Windows.Media.Color), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, System.Windows.Media.Color> target111 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__137.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, System.Windows.Media.Color>> p137 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__137;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__136 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__136 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target112 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__136.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p136 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__136;
        Type type17 = typeof (ColorConverter);
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__135 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__135 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target113 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__135.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p135 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__135;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__134 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__134 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target114 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__134.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p134 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__134;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__133 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__133 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target115 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__133.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p133 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__133;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__132 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__132 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj112 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__132.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__132, obj1);
        object obj113 = target115((CallSite) p133, obj112, 0);
        object obj114 = target114((CallSite) p134, obj113);
        object obj115 = target113((CallSite) p135, obj114);
        object obj116 = target112((CallSite) p136, type17, obj115);
        this.DefaultBrush = (Brush) new SolidColorBrush(target111((CallSite) p137, obj116));
        foreach (Control control in (IEnumerable) this.tabs.Items)
          control.BorderBrush = this.DefaultBrush;
        this.reloadrScripts();
        TabItem uiSettingsTab = this.UISettingsTab;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__143 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__143 = CallSite<Func<CallSite, object, Brush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (Brush), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, Brush> target116 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__143.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, Brush>> p143 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__143;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__142 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__142 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, BrushConverter, object, object> target117 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__142.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, BrushConverter, object, object>> p142 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__142;
        BrushConverter brushConverter8 = brushConverter1;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__141 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__141 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target118 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__141.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p141 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__141;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__140 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__140 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target119 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__140.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p140 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__140;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__139 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__139 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target120 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__139.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p139 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__139;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__138 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__138 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj117 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__138.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__138, obj1);
        object obj118 = target120((CallSite) p139, obj117, 0);
        object obj119 = target119((CallSite) p140, obj118);
        object obj120 = target118((CallSite) p141, obj119);
        object obj121 = target117((CallSite) p142, brushConverter8, obj120);
        Brush brush7 = target116((CallSite) p143, obj121);
        uiSettingsTab.BorderBrush = brush7;
        TabItem apiTab = this.ApiTab;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__149 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__149 = CallSite<Func<CallSite, object, Brush>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (Brush), typeof (Executor)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, Brush> target121 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__149.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, Brush>> p149 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__149;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__148 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__148 = CallSite<Func<CallSite, BrushConverter, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConvertFromString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, BrushConverter, object, object> target122 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__148.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, BrushConverter, object, object>> p148 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__148;
        BrushConverter brushConverter9 = brushConverter1;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__147 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__147 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target123 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__147.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p147 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__147;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__146 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__146 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Color1", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target124 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__146.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p146 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__146;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__145 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__145 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target125 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__145.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p145 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__145;
        // ISSUE: reference to a compiler-generated field
        if (Executor.\u003C\u003Eo__65.\u003C\u003Ep__144 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Executor.\u003C\u003Eo__65.\u003C\u003Ep__144 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Theme", typeof (Executor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj122 = Executor.\u003C\u003Eo__65.\u003C\u003Ep__144.Target((CallSite) Executor.\u003C\u003Eo__65.\u003C\u003Ep__144, obj1);
        object obj123 = target125((CallSite) p145, obj122, 0);
        object obj124 = target124((CallSite) p146, obj123);
        object obj125 = target123((CallSite) p147, obj124);
        object obj126 = target122((CallSite) p148, brushConverter9, obj125);
        Brush brush8 = target121((CallSite) p149, obj126);
        apiTab.BorderBrush = brush8;
      }
      catch
      {
        System.IO.File.Delete("./bin/theme.evon");
        this.setcolor();
      }
    }

    private void colorbtnc(object sender, RoutedEventArgs e) => this.colrp(sender, (ContextMenuEventArgs) null);

    private void colrp(object sender, ContextMenuEventArgs e)
    {
      Executor.ThemeStrings themeStrings = new Executor.ThemeStrings()
      {
        Theme = new List<Executor.ThemeStrings.ThemeSystem>()
        {
          new Executor.ThemeStrings.ThemeSystem()
          {
            ThemeManufacturer = "Evon",
            Color1 = new SolidColorBrush(System.Windows.Media.Color.FromArgb(byte.MaxValue, (byte) ((PickerControlBase) this.colr).Color.RGB_R, (byte) ((PickerControlBase) this.colr).Color.RGB_G, (byte) ((PickerControlBase) this.colr).Color.RGB_B)).ToString(),
            TextboxImage = ""
          }
        }
      };
      try
      {
        System.IO.File.WriteAllText("./bin/theme.evon", JsonConvert.SerializeObject((object) themeStrings, (Formatting) 1));
      }
      catch
      {
      }
      object obj = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("./bin/theme.evon"));
      obj = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("./bin/theme.evon"));
      this.setcolor();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Evon;component/executor.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.window = (Executor) target;
          this.window.KeyDown += new KeyEventHandler(this.handleShortcuts);
          this.window.Loaded += new RoutedEventHandler(this.onLoaded);
          this.window.Drop += new DragEventHandler(this.onDrop);
          this.window.Closing += new CancelEventHandler(this.onClosing);
          break;
        case 3:
          this.IconNotInjected_BeginStoryboard = (System.Windows.Media.Animation.BeginStoryboard) target;
          break;
        case 4:
          this.grid = (Grid) target;
          break;
        case 5:
          this.ShadowVibe = (Border) target;
          break;
        case 6:
          this.MainBorder = (Border) target;
          break;
        case 7:
          this.MenuBar = (Border) target;
          this.MenuBar.MouseLeftButtonDown += new MouseButtonEventHandler(this.doDrag);
          break;
        case 8:
          this.image = (Image) target;
          break;
        case 9:
          this.ExitB = (Button) target;
          this.ExitB.Click += new RoutedEventHandler(this.closeUI);
          break;
        case 10:
          this.MinimizeB = (Button) target;
          this.MinimizeB.Click += new RoutedEventHandler(this.minimizeUI);
          break;
        case 11:
          this.ShowAreaBorder = (Border) target;
          break;
        case 12:
          this.ExecutorPB = (Button) target;
          this.ExecutorPB.Click += new RoutedEventHandler(this.selectExecutor);
          break;
        case 13:
          this.ScriptsPB = (Button) target;
          this.ScriptsPB.Click += new RoutedEventHandler(this.selectScripts);
          break;
        case 14:
          this.SettingsPB = (Button) target;
          this.SettingsPB.Click += new RoutedEventHandler(this.showSettings);
          break;
        case 15:
          this.ExecutorGrid = (Grid) target;
          break;
        case 16:
          this.ExecutorBar = (Border) target;
          break;
        case 17:
          this.InjectB = (Grid) target;
          break;
        case 18:
          this.label = (Label) target;
          break;
        case 19:
          this.injBtn = (Button) target;
          this.injBtn.Click += new RoutedEventHandler(this.doInject);
          break;
        case 20:
          this.ExecuteB = (Grid) target;
          break;
        case 21:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.doExecute);
          break;
        case 22:
          this.OpenB = (Grid) target;
          break;
        case 23:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.doOpen);
          break;
        case 24:
          this.SaveB = (Grid) target;
          break;
        case 25:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.doSave);
          break;
        case 26:
          this.ClearB = (Grid) target;
          break;
        case 27:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.doClear);
          break;
        case 28:
          this.OpenListB = (Grid) target;
          break;
        case 29:
          this.ScriptListLabel = (Label) target;
          break;
        case 30:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.doScripts);
          break;
        case 31:
          this.tabs = (TabControl) target;
          this.tabs.SelectionChanged += new SelectionChangedEventHandler(this.onSelectionChanged);
          break;
        case 32:
          this.ScriptListGrid = (Grid) target;
          break;
        case 33:
          this.ListBoxSearch = (TextBox) target;
          this.ListBoxSearch.TextChanged += new TextChangedEventHandler(this.doSearch);
          break;
        case 34:
          this.ScriptList = (ListBox) target;
          this.ScriptList.SelectionChanged += new SelectionChangedEventHandler(this.doScriptSelection);
          break;
        case 35:
          this.GameScriptsGrid = (Grid) target;
          break;
        case 36:
          this.GameScriptsBar = (Border) target;
          break;
        case 37:
          this.ChangePGLeft = (Button) target;
          this.ChangePGLeft.Click += new RoutedEventHandler(this.docpl);
          break;
        case 38:
          this.ChangePGRight = (Button) target;
          this.ChangePGRight.Click += new RoutedEventHandler(this.docpr);
          break;
        case 39:
          this.GameScriptsInjectB = (Grid) target;
          break;
        case 40:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.doInject);
          break;
        case 41:
          this.search = (TextBox) target;
          this.search.TextChanged += new TextChangedEventHandler(this.doSearchScripts);
          break;
        case 42:
          this.GameSys = (WrapPanel) target;
          break;
        case 43:
          this.SettingsGrid = (Grid) target;
          break;
        case 44:
          this.SettingsBar = (Border) target;
          break;
        case 45:
          this.settingTabs = (TabControl) target;
          break;
        case 46:
          this.UISettingsTab = (TabItem) target;
          break;
        case 47:
          this.sview = (ScrollViewer) target;
          this.sview.MouseMove += new MouseEventHandler(this.sViewMouseMove);
          break;
        case 48:
          this.settingsViewWindow = (WrapPanel) target;
          break;
        case 49:
          this.DSB1 = (DropShadowEffect) target;
          break;
        case 50:
          this.fixbtn = (Button) target;
          this.fixbtn.Click += new RoutedEventHandler(this.fix268);
          break;
        case 51:
          this.DSB2 = (DropShadowEffect) target;
          break;
        case 52:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.killRoblox);
          break;
        case 53:
          this.DSB3 = (DropShadowEffect) target;
          break;
        case 54:
          this.revertbtn = (Button) target;
          this.revertbtn.Click += new RoutedEventHandler(this.revert268);
          break;
        case 55:
          this.DSB4 = (DropShadowEffect) target;
          break;
        case 56:
          this.TopMostCheck = (CheckBox) target;
          this.TopMostCheck.Click += new RoutedEventHandler(this.doTopMost);
          break;
        case 57:
          this.DSB5 = (DropShadowEffect) target;
          break;
        case 58:
          this.autoinjectcheck = (CheckBox) target;
          this.autoinjectcheck.Click += new RoutedEventHandler(this.doAutoInject);
          break;
        case 59:
          this.DSB6 = (DropShadowEffect) target;
          break;
        case 60:
          this.unlockfpscheck = (CheckBox) target;
          this.unlockfpscheck.Click += new RoutedEventHandler(this.doUnlockFPS);
          break;
        case 61:
          this.DSB7 = (DropShadowEffect) target;
          break;
        case 62:
          this.colorbtn = (Button) target;
          this.colorbtn.Click += new RoutedEventHandler(this.colorbtnc);
          break;
        case 63:
          this.colr = (PortableColorPicker) target;
          break;
        case 64:
          this.DSB8 = (DropShadowEffect) target;
          break;
        case 65:
          this.antiskidcheck = (CheckBox) target;
          this.antiskidcheck.Click += new RoutedEventHandler(this.doAntiSkid);
          break;
        case 66:
          this.DSB9 = (DropShadowEffect) target;
          break;
        case 67:
          this.minimapCheck = (CheckBox) target;
          this.minimapCheck.Click += new RoutedEventHandler(this.doMinimap);
          break;
        case 68:
          this.ApiTab = (TabItem) target;
          break;
        case 69:
          this.APiView = (ScrollViewer) target;
          break;
        case 70:
          this.ApiPanerl = (WrapPanel) target;
          break;
        case 71:
          this._1e = (DropShadowEffect) target;
          break;
        case 72:
          this.api_evon = (CheckBox) target;
          this.api_evon.Click += new RoutedEventHandler(this.selectEvonAPI);
          break;
        case 73:
          this._2e = (DropShadowEffect) target;
          break;
        case 74:
          this.api_oxygen = (CheckBox) target;
          this.api_oxygen.Click += new RoutedEventHandler(this.selectOxygenU);
          break;
        case 75:
          this._3e = (DropShadowEffect) target;
          break;
        case 76:
          this.api_fluxus = (CheckBox) target;
          this.api_fluxus.Click += new RoutedEventHandler(this.selectFluxus);
          break;
        case 77:
          this._4e = (DropShadowEffect) target;
          break;
        case 78:
          this.api_krnl = (CheckBox) target;
          this.api_krnl.Click += new RoutedEventHandler(this.selectKRNL);
          break;
        case 79:
          this.MenuBar_Copy = (Border) target;
          this.MenuBar_Copy.MouseLeftButtonDown += new MouseButtonEventHandler(this.doDrag);
          break;
        case 80:
          this.ShadowNotifColor = (DropShadowEffect) target;
          break;
        case 81:
          this.Notiftitle = (TextBlock) target;
          break;
        case 82:
          this.NotifSub = (TextBlock) target;
          break;
        case 83:
          this.notifBar = (Border) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IStyleConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 2)
        return;
      ((ButtonBase) target).Click += new RoutedEventHandler(this.addTab);
    }

    public class ThemeStrings
    {
      public List<Executor.ThemeStrings.ThemeSystem> Theme;

      public class ThemeSystem
      {
        public string ThemeManufacturer { get; set; }

        public string Color1 { get; set; }

        public string TextboxImage { get; set; }
      }
    }
  }
}
