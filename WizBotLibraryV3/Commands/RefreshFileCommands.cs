using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using WizBotLibraryV3;
using WizBotLibraryV3.Modules;
using WizBotLibraryV3.SlashCommands;

public class RefreshFileCommands : SlashCommand
{
  public RefreshFileCommands(WizBot Bot) : base(Bot) { }

  public override SlashCommandProperties Command()
  {
    SlashCommandBuilder builder = new()
    {
      Name = "refreshcommands",
      Description = "Hi! I was compiled from a loose file.",
      IsDefaultPermission = false,
      DefaultMemberPermissions = GuildPermission.UseApplicationCommands
    };
    return builder.Build();
  }

  public override async Task Setup()
  {
    await base.Setup();
  }

  public override async Task Execute(SocketSlashCommand cmd)
  {
    if (cmd.User.Id != Bot.StartupData.OwnerId)
    {
      await cmd.RespondAsync("You are not the owner of this bot.", ephemeral: true);
      return;
    }

    await cmd.DeferAsync(ephemeral: true);

    Bot.ExtCmd!.LoadCommands();
    Bot.ExtCmd!.WriteCommands();

    await cmd.ModifyOriginalResponseAsync((msg) =>
    {
      msg.Content = "Finished job.";
    });
  }
}