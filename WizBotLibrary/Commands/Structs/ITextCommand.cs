using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizBotLibrary.Commands.Structs
{
  public interface ITextCommand : ICommand
  {
    string Name { get; }
    string Description { get; }
    Task Execute(SocketMessage message, WizBot Bot);
  }
}
