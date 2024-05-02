using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizBotLibraryV3.Modules;

namespace WizBotLibraryV3.MsgCommands
{
  [TextCommand]
  public class Immediately : MessageCommand
  {
    public Immediately(WizBot Bot) : base(Bot) { }

    public override async Task Execute(SocketMessage Msg)
    {
      if (Msg.Channel.Id == Bot.StartupData.GogcordData.ImmediatelyChannel)
      {
        if (Msg.Content != "Immediately. <:gogSerious:831917700850384896>")
          await Msg.DeleteAsync();
        else
          await Msg.AddReactionAsync(Emote.Parse($"<{Bot.StartupData.GogcordData.SuccessReaction}>"));
      }
      await base.Execute(Msg);
    }
  }
}
