using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizBotLibrary.Commands.Interfaces;

namespace WizBotLibrary.Commands
{
  public class GogSerious : ITextCommand
  {
    public string Name { get { return "gogserious"; } }

    public string Description { get { return ""; } }

    public async Task Execute(SocketMessage message, WizBot Bot)
    {
      if (message.Channel.Id != 831973302972055615 && message.Author.Id != 962874982663331870) return;

      if (message.Content == "Immediately. <:gogSerious:831917700850384896>") {
        Emote responseEmote;
        if (Bot.IsDebugMode)
        {
          Emote.TryParse("<a:peepopogging:1039399485111029821>", out Emote parsedEmote);
          responseEmote = parsedEmote;
        }
        else {
          Emote.TryParse("<:gogHappy:831979949036273715>", out Emote parsedEmote);
          responseEmote = parsedEmote;
        }
        await message.AddReactionAsync(responseEmote);
      }
      else {
        await message.DeleteAsync();
      }
    }
  }
}
