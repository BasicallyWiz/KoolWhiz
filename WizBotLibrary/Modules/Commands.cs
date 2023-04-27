using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands.Builders;
using Discord.Rest;
using Discord.WebSocket;

using WizBotLibrary.Commands.Interfaces;
namespace WizBotLibrary.Modules
{
  public class CommandSystem
  {
    public SlashCommands SlashSystem;
    public RecursiveCommands RecursiveSystem;
    public TextCommands TextSystem;

    public CommandSystem(WizBot Bot)
    {
      SlashSystem = new SlashCommands(Bot);
      RecursiveSystem = new RecursiveCommands(Bot);
      TextSystem = new TextCommands(Bot);
    }
  }
  public class SlashCommands {
    WizBot Bot;
    public IEnumerable<ISlashCommand> Commands;

    public SlashCommands(WizBot Bot) {
      this.Bot = Bot;
    }

    public async Task Register() {
      Commands = GetCommandsFromFiles();
      if (!Bot.IsDebugMode) {
        await RegisterCommands(Commands);
      }
      else 
      {
        await RegisterDebugCommands(Commands);
      }
    }
    IEnumerable<ISlashCommand> GetCommandsFromFiles() {
      IEnumerable<ISlashCommand> Commands = from t in Assembly.GetExecutingAssembly().GetTypes()
                                            where t.GetInterfaces().Contains(typeof(ISlashCommand))
                                            && t.GetConstructor(Type.EmptyTypes) != null
                                            select Activator.CreateInstance(t) as ISlashCommand;

      return Commands;
    }
    async Task RegisterCommands(IEnumerable<ISlashCommand> Commands) {
      List<ApplicationCommandProperties> AppCommands = new List<ApplicationCommandProperties>();

      foreach (ISlashCommand Command in Commands) {
        AppCommands.Add(Command.Builder.Build());
      }

      await Bot.client.BulkOverwriteGlobalApplicationCommandsAsync(AppCommands.ToArray());
    }

    async Task RegisterDebugCommands(IEnumerable<ISlashCommand> Commands) {
      List<ApplicationCommandProperties> AppCommands = new List<ApplicationCommandProperties>();

      foreach (ISlashCommand Command in Commands) {
        AppCommands.Add(Command.Builder.Build());
      }

      await Bot.client.GetGuild(603162720199639061).BulkOverwriteApplicationCommandAsync(AppCommands.ToArray());
    }
    public async Task ConsumeCommand(SocketSlashCommand Slash) {
      Bot.botStats.slashCommandsUsed++;

      foreach (ISlashCommand Command in Commands) 
      {
        if (Slash.Data.Name == Command.Builder.Name) 
        {
          await Command.Execute(Slash, Bot);
        }
      }
    }
  }
  public class RecursiveCommands //Not executed by a user, but by a specific point in time.
  {
    /// X Find recurcive commands
    /// Start command ticking, and subscribe to events
    /// When event fires, execute method on recursive command

    WizBot CurrentBot;
    IEnumerable<IRecursiveCommand> Commands;

    public RecursiveCommands(WizBot Bot)
    {
      this.CurrentBot = Bot;
    }


    //  Register commands
    public async Task RegisterCommands()
    {
      Commands = RegisterCommandsFromFiles();

      //foreach (IRecursiveCommand command in Commands) {
      //  await CurrentBot.logger.Debug(command);
      //}
    }
    IEnumerable<IRecursiveCommand> RegisterCommandsFromFiles()
    {
      IEnumerable<IRecursiveCommand> Commands = from t in Assembly.GetExecutingAssembly().GetTypes()
                                                where t.GetInterfaces().Contains(typeof(IRecursiveCommand))
                                                && t.GetConstructor(Type.EmptyTypes) != null
                                                select Activator.CreateInstance(t) as IRecursiveCommand;

      return Commands;
    }
    

  }
  public class TextCommands
  {
    WizBot Bot;
    public IEnumerable<ITextCommand> Commands;

    public TextCommands(WizBot Bot)
    {
      this.Bot = Bot;
    }

    public void RegisterCommands()
    {
      Commands = RegisterCommandsFromFiles();
      foreach (ITextCommand command in Commands) 
      {
        command.Setup(Bot);
      }
    }

    IEnumerable<ITextCommand> RegisterCommandsFromFiles()
    {
      IEnumerable<ITextCommand> Commands = from t in Assembly.GetExecutingAssembly().GetTypes()
                                            where t.GetInterfaces().Contains(typeof(ITextCommand))
                                            && t.GetConstructor(Type.EmptyTypes) != null
                                            select Activator.CreateInstance(t) as ITextCommand;

      return Commands;
    }
    public async Task ConsumeCommand(SocketMessage message) {
      foreach (ITextCommand command in Commands) {
        await command.Execute(message, Bot);
      }
    }
  }
}
