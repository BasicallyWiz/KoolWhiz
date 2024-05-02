using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using System.Reflection.Metadata;
using WizBotLibraryV3.MsgCommands;
namespace WizBotLibraryV3.Modules
{
  public class MessageCommandHandler
  {
    WizBot Bot { get; }
    List<MessageCommand> MsgCommands { get; set; }

    public MessageCommandHandler(WizBot bot)
    {
      this.Bot = bot;
      Bot.Client.MessageReceived += HandleCommand;
      MsgCommands = [];

      var typesWithMyAttribute = Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => t.GetCustomAttributes<TextCommandAttribute>(true).Any());

      foreach (var type in typesWithMyAttribute)
      {
        // Create an instance of the type
        MessageCommand? Instance = Activator.CreateInstance(type, args: [Bot]) as MessageCommand;
        if (Instance is null) continue;
        MsgCommands.Add(Instance);
        Instance?.Setup();
      }
    }

    private Task HandleCommand(SocketMessage msg)
    {
      _ = Bot.Logger.Debug("Recieved message: " + msg.Content);

      foreach (var command in MsgCommands) {
        command?.Execute(msg);
      }

      return Task.CompletedTask;
    }
  }

  public class TextCommandAttribute : Attribute
  {
    //  Hi
  }
}
