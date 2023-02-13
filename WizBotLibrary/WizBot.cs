using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Discord;
using Discord.WebSocket;

using WizBotLibrary.Modules;
namespace WizBotLibrary
{
  public class WizBot
  {
    //  DNet Classes
    public DiscordSocketClient client;

    //  My classes
    public Logger logger; //  Implementation for text log output
    public Input input; //  Implementation for input
    public CommandSystem commandSystem;

    //  My Parameters
    public readonly bool IsDebugMode = false;

    public WizBot()
    {
#if DEBUG
      IsDebugMode = true;
#endif

      logger = new Logger();
      client = new DiscordSocketClient();
      commandSystem = new CommandSystem(this);

      client.SlashCommandExecuted += commandSystem.SlashSystem.ConsumeCommand;
      client.Log += logger.Debug;
    }

    public async Task MainAsync()
    {
      try
      {
        await logger.Debug("Getting shit going...");

        //  Register required client events
        client.Ready += DiscordClientReady;

        await client.LoginAsync(TokenType.Bot, File.ReadAllText("token.txt"));
        
        await client.StartAsync();

      } catch(Exception ex) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("An exception has landed on the root. Fix this immediately.\n");
        Console.WriteLine(ex);
        Console.ForegroundColor = ConsoleColor.White;
      }

      await Task.Delay(-1);
    }

    public async Task DiscordClientReady()
    {
      //  This sucks, fix it later
      commandSystem.SlashSystem.RegisterCommands();
      await commandSystem.RecursiveSystem.RegisterCommands();
    }
  }
}
