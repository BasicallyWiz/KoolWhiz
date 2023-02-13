using Discord;
using Discord.Commands.Builders;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WizBotLibrary.Commands.Structs;

namespace WizBotLibrary.Modules
{
  public class GogRandom : ISlashCommand {
    public SlashCommandBuilder Builder {
      get {
        SlashCommandBuilder command = new SlashCommandBuilder();
        command.Name = nameof(GogRandom).ToLower();
        command.Description = "Posts a random gog";

        return command;
      }
    }

    public async Task Execute(SocketSlashCommand inputCommand, WizBot Bot)
    {
      if (!File.Exists($"{Directory.GetCurrentDirectory()}\\gogs\\gogs.txt")) { await inputCommand.RespondAsync("Looks like we have no gogs cached. A developer needs to do /archivegogs to cache the gogs."); return; }
      IEnumerable<string> Strings = File.ReadAllLines($"{Directory.GetCurrentDirectory()}\\gogs\\gogs.txt");
      
      Random random = new Random();
      int gog = random.Next(Strings.Count());
      EmbedBuilder embedBuilder = new EmbedBuilder();
      embedBuilder.Title = "Here's a random gog!";
      embedBuilder.ImageUrl = Strings.ElementAt(gog);
      embedBuilder.Color = Color.DarkPurple;
      embedBuilder.Footer = new EmbedFooterBuilder();
      embedBuilder.Footer.Text = $"Loading gogs may take some time...";

      WebClient client = new WebClient();

      await Bot.logger.Info($"gograndom came out: {gog}");
      await inputCommand.RespondAsync(embed: embedBuilder.Build());
    }
  }
}
