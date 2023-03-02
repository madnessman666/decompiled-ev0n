// Decompiled with JetBrains decompiler
// Type: tools.Pipes.Pipe
// Assembly: Evon, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DB5636C-8858-4769-AD95-C6115039B8B8
// Assembly location: C:\Users\miham\Desktop\sw\Evon\Evon.exe

using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;

namespace tools.Pipes
{
  public class Pipe
  {
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool WaitNamedPipe(string pipe, int timeout = 10);

    public string Name { get; set; }

    public Pipe(string n) => this.Name = n;

    public bool Exists() => Pipe.WaitNamedPipe("\\\\.\\pipe\\" + this.Name);

    public string Read()
    {
      if (this.Name == null)
        throw new Exception("Pipe Name was not set.");
      if (!this.Exists())
        return "";
      using (NamedPipeClientStream pipeClientStream = new NamedPipeClientStream(".", this.Name, PipeDirection.InOut))
      {
        pipeClientStream.Connect();
        using (StreamReader streamReader = new StreamReader((Stream) pipeClientStream))
          return streamReader.ReadToEnd();
      }
    }

    public bool Write(string content)
    {
      if (this.Name == null)
        throw new Exception("Pipe Name was not set.");
      if (string.IsNullOrWhiteSpace(content) || string.IsNullOrEmpty(content) || !this.Exists())
        return false;
      using (NamedPipeClientStream pipeClientStream = new NamedPipeClientStream(".", this.Name, PipeDirection.InOut))
      {
        pipeClientStream.Connect();
        using (StreamWriter streamWriter = new StreamWriter((Stream) pipeClientStream))
          streamWriter.Write(content);
        return true;
      }
    }
  }
}
