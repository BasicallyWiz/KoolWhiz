﻿using Discord.Commands.Builders;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizBotLibrary.Commands.Interfaces
{
  public interface ICommand
  {
    Task Setup(WizBot Bot);
  }
}
