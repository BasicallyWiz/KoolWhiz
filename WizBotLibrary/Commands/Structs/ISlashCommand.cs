using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace WizBotLibrary.Commands.Structs
{
  public interface ISlashCommand : ICommand
  {
    Task Execute(SocketSlashCommand inputCommand);
  }
}
