using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands.Builders;
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
  public class SlashCommands
  {
    WizBot CurrentBot;
    IEnumerable<ISlashCommand> Commands;

    public SlashCommands(WizBot Bot)
    {
      CurrentBot = Bot;
    }

    //  Register commands
    public void RegisterCommands()
    {
      CurrentBot.client.BulkOverwriteGlobalApplicationCommandsAsync(null);
      CurrentBot.client.Rest.GetGuildAsync(603162720199639061).Result.BulkOverwriteApplicationCommandsAsync(null);

      Commands = RegisterCommandsFromFiles();
      RegisterCommandsToDiscord();
    }

    /// <summary>
    /// Method using reflection to collect all commands, in this case: all classes that inherit <see cref="ISlashCommand"/>
    /// </summary>
    /// <returns>an <see cref="IEnumerable<ISlashCommand>"/> containing all commands collected.</returns>
    IEnumerable<ISlashCommand> RegisterCommandsFromFiles()
    {
      IEnumerable<ISlashCommand> Commands = from t in Assembly.GetExecutingAssembly().GetTypes()
                                            where t.GetInterfaces().Contains(typeof(ISlashCommand))
                                            && t.GetConstructor(Type.EmptyTypes) != null
                                            select Activator.CreateInstance(t) as ISlashCommand;

      return Commands;
    }
    void RegisterCommandsToDiscord()
    {
      if (CurrentBot.IsDebugMode)
      {
        foreach (ISlashCommand command in Commands)
        {
          //TODO: Add support for command groups and subcommands
          var guild = CurrentBot.client.Rest.GetGuildAsync(603162720199639061).Result;
          command.Builder.Description += " (DEBUG)";
          guild.CreateApplicationCommandAsync(command.Builder.Build());
        }
      }
      else
      {
        foreach (ISlashCommand command in Commands)
        {
          //TODO: Add support for command groups and subcommands
          var guild = CurrentBot.client.Rest.GetGuildAsync(603162720199639061).Result;

          var Slash = new SlashCommandBuilder();


          CurrentBot.client.CreateGlobalApplicationCommandAsync(command.Builder.Build());
        }
      }
    }

    /// <summary>
    /// Event fired to consume a slash command.
    /// </summary>
    /// <param name="inputCommand">The slash command from a user that fired this command</param>
    /// <returns>Nothing; it's async.</returns>
    public async Task ConsumeCommand(SocketSlashCommand inputCommand)
    {
      foreach (ISlashCommand commandToExcecute in Commands.Where(command => command.Builder.Name == inputCommand.Data.Name))
      {
        await CurrentBot.logger.Debug($"Command excecuted: {commandToExcecute.Builder.Name}");
        await commandToExcecute.Execute(inputCommand, CurrentBot);
      }
    }
  }

  //  Not executed by a user, but by a specific point in time.
  public class RecursiveCommands
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
    IEnumerable<ITextCommand> Commands;

    public TextCommands(WizBot Bot)
    {
      this.Bot = Bot;
    }

    public void RegisterCommands()
    {
      Commands = RegisterCommandsFromFiles();
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
