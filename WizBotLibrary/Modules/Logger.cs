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

    //  Methods
    public async Task Debug(string LogText)
    {
      OnDebug?.Invoke(LogText);
    }
    public async Task Debug(LogMessage msg)
    {
      OnDebug?.Invoke(msg.ToString());
    }
    public async Task Warn(string LogText) {
      OnWarning?.Invoke(LogText);
    }
  }
}
