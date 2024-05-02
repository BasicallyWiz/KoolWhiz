using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizBotLibraryV3.Modules
{
  public class Statistics(WizBot Bot)
  {
    //  Session Stats
    public ulong GogsCounted { get; set; } = 0;
    public DateTime DateTimeLaunched { get; set; } = DateTime.Now;

    //  App stats
    public int ServersIn { get { return Bot.Client.Guilds.Count; } }

    //  Server Stats
    public ulong CurrentGog {  get; set; } = 0;
  }
}
