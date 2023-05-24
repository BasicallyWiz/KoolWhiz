using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizBotLibrary.Modules.DiscordNetExtensions
{
  static class IMessageExtensions
  {
  /// <param name="MessageContent"></param>
  /// <returns>The string, without URL parameters, if any</returns>
    public static string RemoveUrlParameters(this string MessageContent)
    {
      if (MessageContent.Contains('?'))
      {
        return MessageContent.Split('?')[0];
      }

      return MessageContent;
    }
  }
}
