// Decompiled with JetBrains decompiler
// Type: Evon.Properties.Settings
// Assembly: Evon, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DB5636C-8858-4769-AD95-C6115039B8B8
// Assembly location: C:\Users\miham\Desktop\sw\Evon\Evon.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Evon.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
      get
      {
        Settings defaultInstance = Settings.defaultInstance;
        return defaultInstance;
      }
    }
  }
}
