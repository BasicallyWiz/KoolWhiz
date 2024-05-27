using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizBotExecutable.Modules
{
  public class Logging
  {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Try not to forget to do .Close() on this object when you're done with it.
    /// </remarks>
    public FileStream LogFile { get; set; }
    public Logging()
    {
      string logDir = $"{Directory.GetCurrentDirectory()}/Logs";
      if (!Directory.Exists(logDir)) { Directory.CreateDirectory(logDir); }
      LogFile = File.OpenWrite($"{logDir}/log-{DateTime.Now.ToFileTime()}.log");
    }
    public void Close()
    {
      LogFile.Dispose();
    }

    public Task OnSetup(string text)
    {
      Console.WriteLine(text);
      LogFile.Write(Encoding.UTF8.GetBytes($"{text}\n"));
      _ = Task.Run(LogFile.FlushAsync);
      return Task.CompletedTask;
    }
    public Task OnInfo(string text)
    {
      Console.WriteLine($"{text}");
      LogFile.Write(Encoding.UTF8.GetBytes($"{text}\n"));
      _ = Task.Run(LogFile.FlushAsync);
      return Task.CompletedTask;
    }
    public Task OnDebug(string text)
    {
      Console.WriteLine(text);
      LogFile.Write(Encoding.UTF8.GetBytes($"{text}\n"));
      _ = Task.Run(LogFile.FlushAsync);
      return Task.CompletedTask;
    }
    public Task OnWarn(string text)
    {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine(text);
      LogFile.Write(Encoding.UTF8.GetBytes($"{text}\n"));
      _ = Task.Run(LogFile.FlushAsync);
      Console.ForegroundColor = ConsoleColor.White;
      return Task.CompletedTask;
    }
    public Task OnError(string text)
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine(text);
      LogFile.Write(Encoding.UTF8.GetBytes($"{text}\n"));
      _ = Task.Run(LogFile.FlushAsync);
      Console.ForegroundColor = ConsoleColor.White;
      return Task.CompletedTask;
    }
  }
}
