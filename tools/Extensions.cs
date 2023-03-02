// Decompiled with JetBrains decompiler
// Type: tools.Extensions
// Assembly: Evon, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DB5636C-8858-4769-AD95-C6115039B8B8
// Assembly location: C:\Users\miham\Desktop\sw\Evon\Evon.exe

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace tools
{
  public static class Extensions
  {
    public static bool IsUserVisible(this UIElement element)
    {
      if (!element.IsVisible)
        return false;
      if (!(VisualTreeHelper.GetParent((DependencyObject) element) is FrameworkElement parent))
        throw new ArgumentNullException("container");
      Rect rect = element.TransformToAncestor((Visual) parent).TransformBounds(new Rect(0.0, 0.0, element.RenderSize.Width, element.RenderSize.Height));
      return new Rect(0.0, 0.0, parent.ActualWidth, parent.ActualHeight).IntersectsWith(rect);
    }

    public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
    {
      if (depObj != null)
      {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); ++i)
        {
          DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
          T t;
          int num;
          if (child != null)
          {
            t = child as T;
            num = (object) t != null ? 1 : 0;
          }
          else
            num = 0;
          if (num != 0)
            yield return t;
          foreach (T visualChild in Extensions.FindVisualChildren<T>(child))
          {
            T childOfChild = visualChild;
            yield return childOfChild;
            childOfChild = default (T);
          }
          child = (DependencyObject) null;
          t = default (T);
        }
      }
    }

    public static Task Start(this Storyboard sb)
    {
      TaskCompletionSource<bool> status = new TaskCompletionSource<bool>();
      EventHandler sbHandler = (EventHandler) null;
      sbHandler = (EventHandler) ((sender, eeeeeeeeeeee) =>
      {
        sb.Completed -= sbHandler;
        status.SetResult(true);
      });
      sb.Completed += sbHandler;
      sb.Begin();
      return (Task) status.Task;
    }
  }
}
