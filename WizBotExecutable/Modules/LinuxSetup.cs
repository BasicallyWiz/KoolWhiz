using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizBotExecutable.Modules
{
  public static class LinuxSetup
  {
    public static void Setup() {
      CreateShellScripts();
    }

    static void CreateShellScripts() {
      string here = Directory.GetCurrentDirectory();

      if (!Directory.Exists(here + "/scripts/")) { Directory.CreateDirectory(here + "/scripts/"); }
      if (!File.Exists($"{here}/exe.sh")) { File.WriteAllText(here + "/exe.sh", "#!/bin/bash\n\n./WizBotExecutable"); }
      if (!File.Exists($"{here}/scripts/wizbot.service")) { File.WriteAllText($"{here}/scripts/wizbot.service", $"[Unit]\nDescription=WizBot for Gogcord\nAfter=network.target\n\n[Service]\nUser={Environment.UserName}\nWorkingDirectory={Directory.GetCurrentDirectory()}\nExecStart={Directory.GetCurrentDirectory()}/exe.sh\nType=simple\n\n[Install]\nWantedBy=multi-user.target"); }
      if (!File.Exists($"{here}/scripts/startservice.sh")) { File.WriteAllText($"{here}/scripts/startservice.sh", $"systemctl start wizbot.service\nsystemctl status wizbot.service"); }
      if (!File.Exists($"{here}/scripts/createservice.sh")) { File.WriteAllText($"{here}/scripts/createservice.sh", "cp ./wizbot.service /etc/systemd/system/\nsystemctl daemon-reload\n./startservice.sh"); }
      if (!File.Exists($"{here}/scripts/stopservice.sh")) { File.WriteAllText($"{here}/scripts/stopservice.sh", "systemctl stop wizbot.service\nsystemctl status wizbot.service"); }
    }
  }
}