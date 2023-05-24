using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizBotLibrary.Modules.DiscordNetExtensions {
  public static class SocketSlashCommandDataExtension {
    public static string ToStringFormatted(this SocketSlashCommandData data) {

      string TheString = "";
      TheString += $"{data.Id}\n";
      TheString += $"{data.Name}\n";
      foreach(SocketSlashCommandDataOption option in data.Options) {
        TheString += $"{option.ToStringFormatted()}\n";
      }

      return TheString;
    }

    public static string ToStringFormatted(this SocketSlashCommandDataOption option) {
      string TheString = "";
      TheString += $"{option.Name}";
      foreach (SocketSlashCommandDataOption Option in option.Options) {
        TheString += $"Option.ToStringFormatted()\n";
      }

      return TheString;
    }
  }
}
