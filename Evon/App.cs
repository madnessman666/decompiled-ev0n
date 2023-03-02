// Decompiled with JetBrains decompiler
// Type: Evon.App
// Assembly: Evon, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DB5636C-8858-4769-AD95-C6115039B8B8
// Assembly location: C:\Users\miham\Desktop\sw\Evon\Evon.exe

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;

namespace Evon
{
  public class App : Application
  {
    private bool _contentLoaded;

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      this.StartupUri = new Uri("Executor.xaml", UriKind.Relative);
      Application.LoadComponent((object) this, new Uri("/Evon;component/app.xaml", UriKind.Relative));
    }

    [STAThread]
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public static void Main()
    {
      App app = new App();
      app.InitializeComponent();
      app.Run();
    }
  }
}
