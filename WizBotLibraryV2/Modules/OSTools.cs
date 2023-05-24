using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizBotLibrary
{
  public static class OSTools
  {
    #if OS_LINUX
    public static string DirSep = "/";
    #else
    public static string DirSep = "\\";
    #endif
  }
}
