using WizBotExecutable.Modules;
using WizBotLibraryV3;
using System.Xml;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using WizBotLibraryV3.Modules;
using Discord.WebSocket;
using Discord;

namespace WizBotExecutable
{
    public class Exe
  {
    public static void Main(string[] args)
    {
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

      Bot.Logger.OnDebug += Logging.OnDebug;
      Bot.Logger.OnWarning += Logging.OnWarn;
      Bot.Logger.OnInfo += Logging.OnInfo;
      Bot.Logger.OnSetup += Logging.OnSetup;

      Bot.MainAsync().Wait();
    }
  }
}