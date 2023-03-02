// Decompiled with JetBrains decompiler
// Type: Evon.TabEdit
// Assembly: Evon, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DB5636C-8858-4769-AD95-C6115039B8B8
// Assembly location: C:\Users\miham\Desktop\sw\Evon\Evon.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Evon
{
  public class TabEdit : Page, IComponentConnector
  {
    private bool _contentLoaded;

    public TabEdit() => this.InitializeComponent();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Evon;component/tabedit.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
