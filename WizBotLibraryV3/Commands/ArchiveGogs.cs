using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.IO;
using WizBotLibraryV3;
using WizBotLibraryV3.Modules;
using WizBotLibraryV3.SlashCommands;

public class ArchiveGogs : SlashCommand
{
  public ArchiveGogs(WizBot Bot) : base(Bot) { }

  public override SlashCommandProperties Command()
  {
    SlashCommandBuilder builder = new()
    {
      Name = "archivegogs",
      Description = "Attempts to index and archive the gog series.",
      IsDefaultPermission = false,
      DefaultMemberPermissions = GuildPermission.ManageMessages
    };
    return builder.Build();
  }

  public override Task Setup()
  {
    if (!Directory.Exists($"{Directory.GetCurrentDirectory()}/Gogs/"))
      Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/Gogs/");

    return base.Setup();
  }

  public override async Task Execute(SocketSlashCommand cmd)
  {
    await cmd.DeferAsync();
    GogArchiver archiver = new(Bot);

    var GogSeries = await Bot.Client.GetChannelAsync(Bot.StartupData.GogcordData.GogSeriesChannel) as IMessageChannel;
    var list = GogSeries.GetMessagesAsync(int.MaxValue).Flatten();

    await archiver.Archive(list);

    _ = Bot.Logger.Debug("Finished Archival Job!");

    await cmd.ModifyOriginalResponseAsync((msg) =>
    {
      msg.Content = "Finished job.";
    });
  }
}