using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using WizBotLibraryV3;
using WizBotLibraryV3.Modules;
using WizBotLibraryV3.SlashCommands;

public class BotInfo : SlashCommand
{
  public BotInfo(WizBot Bot) : base(Bot) { }

  public override SlashCommandProperties Command()
  {
    SlashCommandBuilder builder = new()
    {
      Name = "botinfo",
      Description = "Displays some info about this bot.",
      IsDefaultPermission = true
    };

    return builder.Build();
  }

  public override Task Execute(SocketSlashCommand cmd)
  {
    var embed = new EmbedBuilder()
    {
      Title = "Bot Info",
      Fields = {
      new EmbedFieldBuilder() {
        Name = "Session Info",
        Value = $"App Launched: <t:{((DateTimeOffset)Bot.Stats.DateTimeLaunched).ToUnixTimeSeconds()}:R>, <t:{((DateTimeOffset)Bot.Stats.DateTimeLaunched).ToUnixTimeSeconds()}:f>\n" +
        $"Gogs Counted: {Bot.Stats.GogsCounted}"
      },
      new EmbedFieldBuilder() {
        Name = "App Info",
        Value = $"Servers: {Bot.Stats.ServersIn}\n" +
        "GitHub: https://github.com/BasicallyWiz/KoolWhiz"
      }
    },
      ImageUrl = $"https://opengraph.githubassets.com/{new Random().Next()}/BasicallyWiz/KoolWhiz",
    };

    cmd.RespondAsync(embed: embed.Build());

    return Task.CompletedTask;
  }
}
