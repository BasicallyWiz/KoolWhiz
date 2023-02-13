using Discord;
using Discord.Commands.Builders;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
      IEnumerable<string> Strings = File.ReadAllLines($"{Directory.GetCurrentDirectory()}\\gogs\\gogs.txt");
      
      Random random = new Random();
      int gog = random.Next(Strings.Count());
      EmbedBuilder embedBuilder = new EmbedBuilder();
      embedBuilder.Title = "Here's a random gog!";
      embedBuilder.ImageUrl = Strings.ElementAt(gog);
      embedBuilder.Color = Color.DarkPurple;
      embedBuilder.Footer = new EmbedFooterBuilder();
      embedBuilder.Footer.Text = $"Loading gogs may take some time...";

      await inputCommand.RespondAsync(embed: embedBuilder.Build());
    }
  }
}
