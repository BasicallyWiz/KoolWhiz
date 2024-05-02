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
  public class GogCount : MessageCommand
  {
    int? lastCount { get; set; }

    public GogCount(WizBot Bot) : base(Bot) { }

    public override async Task Execute(SocketMessage Msg)
    {
      if (Msg.Channel.Id == Bot.StartupData.GogcordData.GogCountChannel)
      {
        try
        {
          Console.WriteLine("cocka");
          var strings = Msg.Content.Split(' ');
          var oldstrings = await Msg.Channel.GetMessagesAsync(2).Flatten().ElementAtAsync(1);
          if (strings.Length <= 1)
          {
            await Msg.DeleteAsync();
            return;
          }

          if (strings[0] is not "gog" || int.Parse(strings[1]) != int.Parse(oldstrings.Content.Split(' ')[1]) + 1)
          {
            await Msg.DeleteAsync();
          }
          else {
            await Msg.AddReactionAsync(Emote.Parse($"<{Bot.StartupData.GogcordData.SuccessReaction}>"));
            Bot.Stats.GogsCounted++;
          }
        }
        catch (Exception) { await Msg.DeleteAsync(); }
      } 
      
      await base.Execute(Msg);
    }
  }
}
