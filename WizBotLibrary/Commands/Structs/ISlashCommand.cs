using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace WizBotLibrary.Commands.Structs
{
  public interface ISlashCommand : ICommand
  {
    SlashCommandBuilder Builder { get; }
    Task Execute(SocketSlashCommand inputCommand, WizBot Bot);
  }
}
