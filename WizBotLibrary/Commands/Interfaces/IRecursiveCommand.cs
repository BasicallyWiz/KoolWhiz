using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizBotLibrary.Commands.Interfaces
{
  public interface IRecursiveCommand : ICommand
  {
    /// <summary>
    /// 0 = On every X-day, at 8-am<br/>
    /// 1 = Every X time<br/>
    /// </summary>
    int RecursiveType { get; }
  }
}
