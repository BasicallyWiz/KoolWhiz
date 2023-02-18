using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizBotLibrary.Commands.Interfaces;
using System.Net;
using Discord.Rest;
using WizBotLibrary.Modules.DiscordNetExtensions;
using System.Diagnostics;

namespace WizBotLibrary.Commands
{
  public class ArchiveGogs : ISlashCommand
  {
    public SlashCommandBuilder Builder
    {
      get
      {
        SlashCommandBuilder builder = new SlashCommandBuilder();
        builder.Name = nameof(ArchiveGogs).ToLower();
        builder.Description = "Collects gogs and archives them.";
        builder.IsDMEnabled = true;

        return builder;
      }
    }

    public async Task Execute(SocketSlashCommand inputCommand, WizBot Bot)
    {
      if (!inputCommand.User.IsDeveloper(Bot)) { await inputCommand.RespondAsync("Since you are not a dev, you cannot use this command."); return; }
      await inputCommand.DeferAsync();

      IMessageChannel SourceChannel;
      if (Bot.IsDebugMode) { // This section executes in debug mode
        SourceChannel = Bot.client.GetChannel(780649979039580170) as IMessageChannel;
      }
      else {  //  This section executes in release node
        SourceChannel = Bot.client.GetChannel(831612912266641408) as IMessageChannel;
      }

      if (SourceChannel == null) { await inputCommand.FollowupAsync("We can't reach the channel that's meant to be archived."); return; }
      List<string> UrlList = new List<string>();
      
      //  Executes regardless of debug or release
      var messagesEnumerable = SourceChannel.GetMessagesAsync(1000, CacheMode.AllowDownload).Flatten();
      await messagesEnumerable.ForEachAwaitAsync((IMessage message) =>
      {
        foreach (IAttachment attachment in message.Attachments) {
          UrlList.Add(attachment.Url);
        }
        return Task.CompletedTask;
      });

      if (!Directory.Exists($"{Directory.GetCurrentDirectory()}/gogs/")) {
        Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/gogs/");
      }

      File.WriteAllLines($"{Directory.GetCurrentDirectory()}/gogs/gogs.txt", UrlList.ToArray());
      await Bot.logger.Info($"Archived gogs to: {Directory.GetCurrentDirectory()}/gogs/");
      await inputCommand.FollowupAsync("Finished caching urls, but we couldn't download any files.");
    }
  }
}
