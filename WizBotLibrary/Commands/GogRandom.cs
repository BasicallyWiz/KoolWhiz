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
using WizBotLibrary.Commands.Interfaces;

namespace WizBotLibrary.Modules
{
  public class GogRandom : ISlashCommand {
    public SlashCommandBuilder Builder {
      get {
        SlashCommandBuilder command = new SlashCommandBuilder();
        command.Name = nameof(GogRandom).ToLower();
        command.Description = "Posts a random gog";
        command.IsDMEnabled = true;

        return command;
      }
    }

    public async Task Execute(SocketSlashCommand inputCommand, WizBot Bot)
    {
      if (!File.Exists($"{Directory.GetCurrentDirectory()}{OSTools.DirSep}gogs{OSTools.DirSep}gogs.txt")) { await inputCommand.RespondAsync("Looks like we have no gogs cached. A developer needs to do /archivegogs to cache the gogs."); return; }
      IEnumerable<string> Strings = File.ReadAllLines($"{Directory.GetCurrentDirectory()}{OSTools.DirSep}gogs{OSTools.DirSep}gogs.txt");
      
      Random random = new Random();
      int gog = random.Next(Strings.Count());
      EmbedBuilder embedBuilder = new EmbedBuilder();
      embedBuilder.Title = "Here's a random gog!";
      string videoUrl = "";
      if (Strings.ElementAt(gog).EndsWith(".gif") || Strings.ElementAt(gog).EndsWith(".png")) {
        embedBuilder.ImageUrl = Strings.ElementAt(gog);
      }
      else { 
        videoUrl = Strings.ElementAt(gog);
      }
      embedBuilder.Color = Color.DarkPurple;
      embedBuilder.Footer = new EmbedFooterBuilder();
      if (videoUrl == "") embedBuilder.Footer.Text = "Loading gogs may take some time..."; else embedBuilder.Footer.Text = "This gog url didn't end with \".gif\", so the alternate view will be used.";

      if (Bot.IsDebugMode) {
        EmbedFieldBuilder debugField = new EmbedFieldBuilder();
        debugField.Name = "Debug";
        debugField.Value = $"gog num: {gog}\n" +
        $"gog file: {Strings.ElementAt(gog)}";
        embedBuilder.AddField(debugField);
      }

      await inputCommand.RespondAsync(embed: embedBuilder.Build());
      if (videoUrl != "") {
        await inputCommand.FollowupAsync(videoUrl);
      }
    }
  }
}
