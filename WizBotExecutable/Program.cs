using WizBotExecutable.Modules;
using WizBotLibraryV3;
using System.Xml;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using WizBotLibraryV3.Modules;
using Discord.WebSocket;
using Discord;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace WizBotExecutable
{
  public class Exe
  {
    Logging logger = new Logging();

    public static void Main(string[] args)
    {
      new Exe().ExeMain(args);
    }

    public void ExeMain(string[] args)
    {
      AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
      Process.GetCurrentProcess().Exited += OnProcessExit;


      XmlSerializer xml = new(typeof(BotData));
      if (!File.Exists($"{Directory.GetCurrentDirectory()}/BotInfo.xml"))
      {
        var stream = File.OpenWrite($"{Directory.GetCurrentDirectory()}/BotInfo.xml");
        xml.Serialize(stream, new BotData());
        stream.Close();
      }

      WizBot Bot = new WizBot(
        (BotData)xml.Deserialize(File.OpenRead($"{Directory.GetCurrentDirectory()}/BotInfo.xml")),
        new DiscordSocketConfig
        {
          GatewayIntents = GatewayIntents.MessageContent |
          GatewayIntents.GuildMessages |
          GatewayIntents.Guilds |
          GatewayIntents.DirectMessages
        }
      )
      {
#if DEBUG
        IsDebug = true
#endif
      };

      Bot.Logger.OnDebug += logger.OnDebug;
      Bot.Logger.OnWarning += logger.OnWarn;
      Bot.Logger.OnInfo += logger.OnInfo;
      Bot.Logger.OnSetup += logger.OnSetup;

      if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
      {
        LinuxSetup.Setup();
      }

      Bot.MainAsync().Wait();
      logger.Close();
      Environment.Exit(0);
    }

    public void OnProcessExit(object? sender, EventArgs? e)
    {
      Console.WriteLine("Exiting...");
      logger.Close();
      Environment.Exit(0);
    }
  }
}