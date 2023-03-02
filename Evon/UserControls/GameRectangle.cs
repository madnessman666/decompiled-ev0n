// Decompiled with JetBrains decompiler
// Type: Evon.UserControls.GameRectangle
// Assembly: Evon, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DB5636C-8858-4769-AD95-C6115039B8B8
// Assembly location: C:\Users\miham\Desktop\sw\Evon\Evon.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Evon.UserControls
{
  public class GameRectangle : UserControl, IComponentConnector
  {
    public WrapPanel items;
    public int id;
    internal Border MainBorder;
    internal Border ___No_Name_;
    internal ImageBrush ScriptImage;
    internal TextBlock ScriptTitle;
    internal Button ExecuteB;
    internal DropShadowEffect ButtonGlow;
    private bool _contentLoaded;

    public GameRectangle() => this.InitializeComponent();

    private void doRightClick(object sender, MouseButtonEventArgs e)
    {
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Evon;component/usercontrols/gamerectangle.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((UIElement) target).MouseRightButtonDown += new MouseButtonEventHandler(this.doRightClick);
          break;
        case 2:
          this.MainBorder = (Border) target;
          break;
        case 3:
          this.___No_Name_ = (Border) target;
          break;
        case 4:
          this.ScriptImage = (ImageBrush) target;
          break;
        case 5:
          this.ScriptTitle = (TextBlock) target;
          break;
        case 6:
          this.ExecuteB = (Button) target;
          break;
        case 7:
          this.ButtonGlow = (DropShadowEffect) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
