using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizBotLibrary.Commands.Structs;

namespace WizBotLibrary.Commands
{
  public class ArchiveGogs : ISlashCommand
  {
    public string Name { get { return "ArchiveGogs"; } }

    public string Description { get { return "Saves all available gogs to disk, for later use."; } }

    public Task Execute(SocketSlashCommand inputCommand)
    {
      throw new NotImplementedException();
    }
  }
}
