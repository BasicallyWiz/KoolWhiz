using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizBotLibraryV3.MsgCommands
{
  public class MessageCommand(WizBot Bot)
  {
    public virtual string Name { get; set; } = "Unnamed Command";
    public virtual string Description { get; set; } = "No Description";
    public virtual WizBot Bot { get; set; } = Bot;

    public virtual Task Setup() {
      return Task.CompletedTask;
    }

    public virtual Task Execute(SocketMessage Msg) {
      return Task.CompletedTask;
    }
  }
}
