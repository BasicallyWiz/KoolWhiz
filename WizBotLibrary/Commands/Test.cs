using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using WizBotLibrary.Commands.Structs;

namespace WizBotLibrary.Commands
{
  public class Test : ISlashCommand
  {
    public string Name 
    { 
      get { return "test"; } 
    }

    public string Description 
    { 
      get { return "Test Command, for use to see if this even works."; } 
    }

    public async Task Execute(SocketSlashCommand inputCommand)
    {
      await inputCommand.RespondAsync("Test worked, commands are functional.");
    }
  }
}
