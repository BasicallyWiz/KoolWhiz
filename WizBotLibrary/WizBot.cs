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
    public Statistics botStats;

    //  My Parameters
    public readonly bool IsDebugMode = false;

    public WizBot(string[] args)
    {
#if DEBUG
      IsDebugMode = true;
#endif

      foreach (string arg in args)
      {
        if (arg == "--debug") { IsDebugMode = true; logger.Info("Debug argument found!"); }
      }

      logger = new Logger();
      DiscordSocketConfig socketConfig = new DiscordSocketConfig { GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent };

      client = new DiscordSocketClient(socketConfig);
      commandSystem = new CommandSystem(this);
      botStats = new Statistics();

      client.SlashCommandExecuted += commandSystem.SlashSystem.ConsumeCommand;
      client.MessageReceived += commandSystem.TextSystem.ConsumeCommand;
      client.Log += logger.Info;

      botStats.creationDateTime = DateTime.UtcNow;
    }

    public async Task MainAsync(string[] args)
    {
      

      try
      {
        string DebugAppend = "";
        if (IsDebugMode) { DebugAppend = "in debug mode!"; }
        await logger.Info($"Getting the bot running... {DebugAppend}");

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
      await commandSystem.SlashSystem.Register();
      commandSystem.TextSystem.RegisterCommands();
      await commandSystem.RecursiveSystem.RegisterCommands();
      await logger.Info($"Working directory is: {Directory.GetCurrentDirectory()}");
    }
  }
}
