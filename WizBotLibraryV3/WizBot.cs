using Discord;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizBotLibraryV3.Modules;

namespace WizBotLibraryV3
{
  public class WizBot
  {
    //  DiscordNet Parameters
    public DiscordSocketClient Client { get; set; }

    //  My Prarameters
    public MessageCommandHandler? TxtCmd { get; set; }
    //public SlashCommandHandler? SlashCmd { get; set; }
    public ExternalCommandLoader? ExtCmd { get; set; }
    public Logger Logger { get; set; }
    public bool IsDebug { get; set; }
    public BotData StartupData { get; set; }
    public Statistics Stats { get; set; }

    public WizBot(BotData StartupData, DiscordSocketConfig SocketConfig) {
      this.StartupData = StartupData;

      Stats = new(this);
      Logger = new();
      _ = Logger.Info($"Current directory is: {Directory.GetCurrentDirectory()}");

      Client = new DiscordSocketClient(SocketConfig);

      Client.Log += Logger.Info;
      Client.Ready += OnReady;
    }

    public async Task MainAsync() {
      Client.LoginAsync(TokenType.Bot, StartupData.ClientToken).Wait();
      await Client.StartAsync();

      await Task.Delay(-1);
    }

    public async Task OnReady() {
      TxtCmd = new(this);
      ExtCmd = new(this);
      ExtCmd.LoadCommands();
      ExtCmd.WriteCommands();
    }
  }
}
