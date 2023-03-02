// Decompiled with JetBrains decompiler
// Type: tools.ProcessTools.Watcher
// Assembly: Evon, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DB5636C-8858-4769-AD95-C6115039B8B8
// Assembly location: C:\Users\miham\Desktop\sw\Evon\Evon.exe

using System;
using System.Diagnostics;
using System.Timers;

namespace tools.ProcessTools
{
  public class Watcher : IDisposable
  {
    public bool DisposedValue;
    private Timer _watcherTimer = new Timer();
    private string _processName;
    public bool _inited;

    public event Watcher.ProcessDelegate OnProcessMade;

    protected virtual void Dispose(bool disposing)
    {
      if (this.DisposedValue)
        return;
      if (disposing)
        this._watcherTimer.Dispose();
      this.DisposedValue = true;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void OnTick(object sender, ElapsedEventArgs e)
    {
      Process[] processesByName = Process.GetProcessesByName(this._processName);
      if (processesByName.Length == 0)
        return;
      processesByName[0].Exited += (EventHandler) ((ee, eee) => this._watcherTimer.Start());
      processesByName[0].EnableRaisingEvents = true;
      this._watcherTimer.Stop();
      this.OnProcessMade();
    }

    public void Initialize(string procName)
    {
      if (this._inited)
        return;
      this._watcherTimer.Interval = 2000.0;
      this._watcherTimer.Elapsed += new ElapsedEventHandler(this.OnTick);
      this._processName = procName;
      this._inited = true;
    }

    public void SwitchProcess(string name) => this._processName = name;

    public void Start()
    {
      if (this.OnProcessMade == null)
        throw new Exception("Expected 'onProcessMade' Delegate. None was given");
      if (!this._inited)
        throw new Exception("Expected Processwatcher to be initialized");
      this._watcherTimer.Start();
    }

    public void Stop()
    {
      if (this.OnProcessMade == null)
        throw new Exception("Expected 'onProcessMade' Delegate. None was given");
      if (!this._inited)
        throw new Exception("Expected Processwatcher to be initialized");
      this._watcherTimer.Stop();
    }

    public delegate void ProcessDelegate();
  }
}
