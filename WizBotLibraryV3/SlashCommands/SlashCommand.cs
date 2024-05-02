using Discord;
using Discord.WebSocket;

namespace WizBotLibraryV3.SlashCommands
{
  public class SlashCommand(WizBot Bot)
  {
    public virtual WizBot Bot { get; set; } = Bot;

    public virtual SlashCommandProperties Command() {
      SlashCommandBuilder command = new();

      command.WithName("newcommand");
      command.WithDescription("A new slash command");

      return command.Build();
    }

    public virtual Task Setup() { return Task.CompletedTask; }
    public virtual async Task Execute(SocketSlashCommand cmd) {
      await cmd.RespondAsync($"You executed {cmd.Data.Name}");
    }
  }
}
