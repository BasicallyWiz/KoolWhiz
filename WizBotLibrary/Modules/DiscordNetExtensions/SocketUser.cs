using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizBotLibrary.Modules.DiscordNetExtensions
{
  public static class SocketUserExtention {
  /// <summary>
  /// Tests if a user is a member in the app's associated Discord team
  /// </summary>
  /// <param name="user">The user to test</param>
  /// <param name="bot">The bot to get team information from.</param>
  /// <returns>true if the tested user is included in <see cref="Discord.ITeam.TeamMembers"/> collected from <see cref="DiscordSocketClient.GetApplicationInfoAsync"/></returns>
    public static bool IsDeveloper(this SocketUser user, WizBot bot) {
      var teamMembers = bot.client.GetApplicationInfoAsync().Result.Team.TeamMembers;

      foreach (var member in teamMembers)
      {
        if (member.User.Id == user.Id)
        {
          return true;
        }
      }

      return false;
    }
  }
}
