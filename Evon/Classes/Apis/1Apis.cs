// Decompiled with JetBrains decompiler
// Type: Evon.Classes.Apis.Apis
// Assembly: Evon, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DB5636C-8858-4769-AD95-C6115039B8B8
// Assembly location: C:\Users\miham\Desktop\sw\Evon\Evon.exe

using KrnlAPI;
using Oxygen;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Evon.Classes.Apis
{
  public static class Apis
  {
    public static tools.Pipes.Pipe p = new tools.Pipes.Pipe("EvonSakpot");
    private static KrnlApi kAPI = new KrnlApi();
    private static FluxAPI.API fAPI = new FluxAPI.API();
    private static int injectionms = 0;
    private static int pipems = 0;
    public static int selected = 1;

    public static bool Execute(string script)
    {
      try
      {
        switch (Evon.Classes.Apis.Apis.selected)
        {
          case 0:
            switch (Execution.Execute(script))
            {
              case Execution.ExecutionResult.Success:
                return true;
              case Execution.ExecutionResult.DLLNotFound:
                int num1 = (int) MessageBox.Show("Couldn't find the Oxygen U dll, please make sure your anti-virus's exclusions has had the EVON folder added.\nFirst time executing? Make sure you're injected.");
                return false;
              case Execution.ExecutionResult.PipeNotFound:
                int num2 = (int) MessageBox.Show("Please inject EVON before trying to execute.");
                return false;
              case Execution.ExecutionResult.Failed:
                int num3 = (int) MessageBox.Show("Something unexpected happened while attempting to execute your script. Sorry!");
                return false;
              default:
                return false;
            }
          case 1:
            if (Process.GetProcessesByName("RobloxPlayerBeta").Length < 1)
            {
              int num4 = (int) MessageBox.Show("Please open ROBLOX before executing.");
              return false;
            }
            if (!Evon.Classes.Apis.Apis.p.Exists())
            {
              int num5 = (int) MessageBox.Show("Please inject ur balls in ur ass.");
              return false;
            }
            Evon.Classes.Apis.Apis.p.Write(script);
            return true;
          case 2:
            if (Process.GetProcessesByName("RobloxPlayerBeta").Length < 1)
            {
              int num6 = (int) MessageBox.Show("Please open ROBLOX before executing.");
              return false;
            }
            Evon.Classes.Apis.Apis.fAPI.Execute(script);
            return true;
          case 3:
            if (!Evon.Classes.Apis.Apis.kAPI.IsInitialized())
            {
              int num7 = (int) MessageBox.Show("Please try injecting first before executing.");
              return false;
            }
            if (Process.GetProcessesByName("RobloxPlayerBeta").Length >= 1)
              return Evon.Classes.Apis.Apis.kAPI.Execute(script);
            int num8 = (int) MessageBox.Show("Please open ROBLOX before executing.");
            return false;
        }
      }
      catch
      {
        return false;
      }
      return false;
    }

    public static Task<bool> Inject()
    {
      TaskCompletionSource<bool> result1 = new TaskCompletionSource<bool>();
      try
      {
        new Thread((ThreadStart) (async () =>
        {
          APIs selected = (APIs) Evon.Classes.Apis.Apis.selected;
          switch (selected)
          {
            case APIs.OxygenU:
              Oxygen.API.injectionResult injectionResult = await Oxygen.API.Inject();
              switch (injectionResult)
              {
                case Oxygen.API.injectionResult.RobloxNotFound:
                  int num1 = (int) MessageBox.Show("Please open ROBLOX before trying to inject.");
                  result1.SetResult(false);
                  return;
                case Oxygen.API.injectionResult.InjectionFailed:
                  int num2 = (int) MessageBox.Show("An error has occured while attempting to inject, if your key isnt valid this could be why.\nOnce your key is validated, you can execute scripts.");
                  result1.SetResult(false);
                  return;
                case Oxygen.API.injectionResult.AlreadyInjected:
                  result1.SetResult(false);
                  return;
                case Oxygen.API.injectionResult.DLLBlocked:
                  int num3 = (int) MessageBox.Show("An error has occured while attempting to download Oxygen U, please make sure your anti-virus's exclusions has had the EVON folder added.");
                  result1.SetResult(false);
                  return;
                case Oxygen.API.injectionResult.InjectorBlocked:
                  int num4 = (int) MessageBox.Show("An error has occured while attempting to download Oxygen U's Injector, please make sure your anti-virus's exclusions has had the EVON folder added.");
                  result1.SetResult(false);
                  return;
                case Oxygen.API.injectionResult.AlreadyInjecting:
                  result1.SetResult(false);
                  return;
                case Oxygen.API.injectionResult.Success:
                  result1.SetResult(true);
                  return;
                default:
                  return;
              }
            case APIs.EVON:
              Evon.Classes.Apis.Apis.injectionms = 0;
              if (Process.GetProcessesByName("RobloxPlayerBeta").Length < 1)
              {
                int num5 = (int) MessageBox.Show("Please open ROBLOX before injecting.");
                result1.SetResult(false);
                break;
              }
              if (Evon.Classes.Apis.Apis.p.Exists())
              {
                result1.SetResult(false);
                break;
              }
              try
              {
                using (WebClient wc = new WebClient())
                {
                  wc.DownloadFile("https://github.com/ahhh-ahhh/EVON-downloads/raw/main/Injector_evon.exe", "injector_evon.exe");
                  wc.DownloadFile("https://github.com/ahhh-ahhh/EVON-downloads/raw/main/Evon.dll", "Evon.dll");
                }
                ProcessStartInfo args = new ProcessStartInfo()
                {
                  FileName = "injector_evon.exe",
                  WorkingDirectory = Directory.GetCurrentDirectory()
                };
                Process proc = new Process()
                {
                  StartInfo = {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    WorkingDirectory = Environment.CurrentDirectory,
                    FileName = Environment.CurrentDirectory + "\\injector_evon.exe"
                  }
                };
                proc.Start();
                while (!proc.HasExited)
                {
                  if (Evon.Classes.Apis.Apis.injectionms >= 20000)
                  {
                    int num6 = (int) MessageBox.Show("the injector is taking longer then usual. Maybe try reinjecting?");
                    result1.SetResult(false);
                    return;
                  }
                  await Task.Delay(200);
                  Evon.Classes.Apis.Apis.injectionms += 200;
                }
                Evon.Classes.Apis.Apis.injectionms = 0;
                while (!Evon.Classes.Apis.Apis.p.Exists())
                  await Task.Delay(500);
                result1.SetResult(true);
                break;
              }
              catch (Exception ex)
              {
                int num7 = (int) MessageBox.Show(ex.ToString());
                result1.SetResult(false);
                break;
              }
            case APIs.Fluxus:
              if (Process.GetProcessesByName("RobloxPlayerBeta").Length < 1)
              {
                int num8 = (int) MessageBox.Show("Please open ROBLOX before injecting.");
                result1.SetResult(false);
                break;
              }
              FluxAPI.API.Result result2 = Evon.Classes.Apis.Apis.fAPI.Inject(false);
              switch (result2)
              {
                case FluxAPI.API.Result.Success:
                  result1.SetResult(true);
                  return;
                case FluxAPI.API.Result.RobloxNotOpen:
                  return;
                case FluxAPI.API.Result.DLLNotFound:
                  int num9 = (int) MessageBox.Show("Couldn't find Fluxus DLL!");
                  result1.SetResult(false);
                  return;
                case FluxAPI.API.Result.OpenProcFail:
                  int num10 = (int) MessageBox.Show("Couldn't open process!");
                  result1.SetResult(false);
                  return;
                case FluxAPI.API.Result.AllocFail:
                  int num11 = (int) MessageBox.Show("Couldn't alloc memory for injection!");
                  result1.SetResult(false);
                  return;
                case FluxAPI.API.Result.LoadLibFail:
                  result1.SetResult(false);
                  int num12 = (int) MessageBox.Show("Load library fail!");
                  return;
                case FluxAPI.API.Result.Unknown:
                  result1.SetResult(false);
                  int num13 = (int) MessageBox.Show("An unknown error occured while attempting to inject.", "EVON", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                  return;
                default:
                  return;
              }
            case APIs.KRNL:
              if (!Evon.Classes.Apis.Apis.kAPI.IsInitialized())
                Evon.Classes.Apis.Apis.kAPI.Initialize();
              Injector.InjectionStatus injectionStatus = Evon.Classes.Apis.Apis.kAPI.Inject();
              switch (injectionStatus)
              {
                case Injector.InjectionStatus.failure:
                  int num14 = (int) MessageBox.Show("Something happened while attempting to inject.");
                  result1.SetResult(false);
                  return;
                case Injector.InjectionStatus.success:
                  result1.SetResult(true);
                  return;
                case Injector.InjectionStatus.loadimage_fail:
                  int num15 = (int) MessageBox.Show("Load Image Fail!");
                  result1.SetResult(false);
                  return;
                case Injector.InjectionStatus.no_rbx_proc:
                  int num16 = (int) MessageBox.Show("Please open ROBLOX before injecting.");
                  result1.SetResult(false);
                  return;
                default:
                  return;
              }
          }
        })).Start();
        return result1.Task;
      }
      catch
      {
        result1.SetResult(false);
        return result1.Task;
      }
    }
  }
}
