using Discord;

namespace WizBotLibraryV3.Modules
{
  public class Logger
  {
    //  Parameters
    public readonly Color color;

    //  Delegates
    public delegate Task Log(string LogText);

    //  Events
    public event Log? OnDebug;
    public event Log? OnWarning;
    public event Log? OnInfo;
    public event Log? OnSetup;
    public event Log? OnError;

    //  Methods
    public Task? Setup(string LogText) =>  OnSetup?.Invoke($"[WizBot - SETUP] {DateTime.Now.ToShortTimeString()} - " + LogText);
    public Task? Setup(LogMessage msg) =>  OnSetup?.Invoke($"[DiscordNet - SETUP] " + msg);
    public Task? Info(string LogText) =>   OnInfo?.Invoke($"[WizBot - INFO] {DateTime.Now.ToShortTimeString()} - " + LogText);
    public Task? Info(LogMessage msg) =>   OnInfo?.Invoke($"[DiscordNet - INFO] " + msg.Message);
    public Task? Debug(string LogText) =>  OnDebug?.Invoke($"[WizBot - DEBUG] {DateTime.Now.ToShortTimeString()} - " + LogText);
    public Task? Debug(LogMessage msg) =>  OnDebug?.Invoke($"[DiscordNet - DEBUG] " + msg.ToString());
    public Task? Warn(string LogText) =>   OnWarning?.Invoke($"[WizBot - WARN] {DateTime.Now.ToShortTimeString()} - " + LogText);
    public Task? Warn(LogMessage msg) => OnWarning?.Invoke($"[DiscordNet - WARN] " + msg.ToString());
    public Task? Error (string LogText) => OnError?.Invoke($"[WizBot - ERROR] {DateTime.Now.ToShortTimeString()} - " + LogText);
    public Task? Error(LogMessage msg) => OnError?.Invoke($"[DiscordNet - ERROR] " + msg.ToString());
  }
}