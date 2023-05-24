using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizBotLibrary.Modules
{
  public class Logger
  {
    //  Parameters
    public readonly Color color;

    //  Delegates
    public delegate Task Log(string LogText);

    //  Events
    public event Log OnDebug;
    public event Log OnWarning;
    public event Log OnInfo;
    public event Log OnSetup;

    //  Methods
    public async Task Info(string LogText)
    {
      OnInfo?.Invoke($"[WizBot - INFO] {DateTime.Now.ToShortTimeString()} - " + LogText);
    }
    public async Task Info(LogMessage msg)
    {
      OnInfo?.Invoke($"[WizBot - INFO] {DateTime.Now.ToShortTimeString()} - " + msg.ToString());
    }
    public async Task Debug(string LogText)
    {
      OnDebug?.Invoke($"[WizBot - DEBUG] {DateTime.Now.ToShortTimeString()} - " + LogText);
    }
    public async Task Debug(LogMessage msg)
    {
      OnDebug?.Invoke($"[WizBot - DEBUG] {DateTime.Now.ToShortTimeString()} - " + msg.ToString());
    }
    public async Task Warn(string LogText) {
      OnWarning?.Invoke($"[WizBot - WARN] {DateTime.Now.ToShortTimeString()} - " + LogText);
    }
    public async Task Setup(string LogText)
    {
      OnSetup?.Invoke($"[WizBot - SETUP] {DateTime.Now.ToShortTimeString()} - " + LogText);
    }
    public async Task Setup(LogMessage msg)
    {
      OnSetup?.Invoke($"[WizBot - SETUP] {DateTime.Now.ToShortTimeString()} - " + msg);
    }
  }
}
