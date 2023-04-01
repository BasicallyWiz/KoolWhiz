using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizBotLibrary.Commands.Interfaces
{
  public interface ITextCommand : ICommand
  {
    string Name { get; }
    string Description { get; }
    Task Setup(WizBot Bot);
    Task Execute(SocketMessage message, WizBot Bot);
  }
}
