using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

using WizBotLibrary.Commands.Structs;
namespace WizBotLibrary.Modules
{
  public class CommandSystem
  {
    public SlashCommands SlashSystem;
    public RecursiveCommands RecursiveSystem;

    public CommandSystem(WizBot Bot)
    {
      SlashSystem = new SlashCommands(Bot);
      RecursiveSystem = new RecursiveCommands(Bot);
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
      Commands = RegisterCommandsFromFiles();
      RegisterCommandsToDiscord();
    }
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
          var guild = CurrentBot.client.Rest.GetGuildAsync(603162720199639061).Result;

          var Slash = new SlashCommandBuilder();

          if (command.Name.Any(char.IsUpper)) { CurrentBot.logger.Warn($"The command \"{command.Name}\" includes illegal characters.\nThe most likely issue, is that it contains uppercase characters.\nIn attempt to alleviate the issue, ToLower() will be applied.\nFor more details, see: https://discord.com/developers/docs/interactions/application-commands#application-command-object-application-command-naming").Wait(); }
          Slash.Name = command.Name.ToLower();
          Slash.Description = command.Description;

          guild.CreateApplicationCommandAsync(Slash.Build());
        }
      }
      else
      {
        throw new NotImplementedException("Slash Command setup has not been implemented for release mode!");
      }
    }

    //  Handle commands
    public async Task ConsumeCommand(SocketSlashCommand inputCommand)
    {
      foreach (ISlashCommand commandToExcecute in Commands.Where(command => command.Name == inputCommand.Data.Name))
      {
        await commandToExcecute.Execute(inputCommand);
      }
    }
  }

  //  Not executed by a user, but by a specific point in time.
  public class RecursiveCommands {
    /// X Find recurcive commands
    /// Start command ticking, and subscribe to events
    /// When event fires, execute method on recursive command

    WizBot CurrentBot;
    IEnumerable<IRecursiveCommand> Commands;

    public RecursiveCommands(WizBot Bot) {
      this.CurrentBot = Bot;
    }

    
    //  Register commands
    public async Task RegisterCommands()
    {
      Commands = RegisterCommandsFromFiles();

      foreach (IRecursiveCommand command in Commands) {
        await CurrentBot.logger.Debug(command.Name);
      }
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
}
