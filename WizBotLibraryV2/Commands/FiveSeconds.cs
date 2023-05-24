using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizBotLibrary.Commands.Interfaces;

namespace WizBotLibrary.Commands
{
  public class FiveSeconds : IRecursiveCommand
  {
    public int RecursiveType => throw new NotImplementedException();

    public string InGroup { get { return ""; } }
    public string Name
    {
      get { return "five_seconds"; }
    }

    public string Description => throw new NotImplementedException();

    
    public async Task Setup(WizBot Bot)
    {
      return;
    }
    public async Task Execute()
    {
      Console.WriteLine("Big Booty bitches");
    }
  }
}
