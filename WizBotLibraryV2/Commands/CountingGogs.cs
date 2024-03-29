﻿using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizBotLibrary.Commands.Interfaces;

namespace WizBotLibrary.Commands
{
  public class CountingGogs : ITextCommand
  {
    public string Name { get { return nameof(CountingGogs).ToLower(); } }
    public string Description { get { return "System to moderate <#831623456470466620>"; } }

    public async Task Setup(WizBot Bot)
    {
      return;
    }
    public async Task Execute(SocketMessage message, WizBot Bot)
    {
      IMessageChannel channel;
      if (Bot.IsDebugMode) channel = Bot.client.GetChannel(1035239361383497858) as IMessageChannel;
      else channel = Bot.client.GetChannel(831623456470466620) as IMessageChannel;

      if (message.Channel.Id != channel.Id) return;

      List<IMessage> messages = new List<IMessage>();
      await channel.GetMessagesAsync(2).Flatten().ForEachAwaitAsync((IMessage msg) => {
        messages.Add(msg);
        return Task.CompletedTask;
      });

      string[] messageStuff = message.Content.Split(' ');
      if (messageStuff[0] != "gog") { await message.DeleteAsync(); return; }
      
      ulong oldnum = 0;
      IUser oldUser = null;
      ulong newnum = 0;
      IUser newUser = null;
      int i = 0;
      try
      {
        foreach (IMessage msg in messages)
        {
          switch (i)
          {
            case 0:
              newnum = ulong.Parse(msg.Content.Split(' ')[1]);
              newUser = msg.Author;
              i++;
              break;

            case 1:
              oldnum = ulong.Parse(msg.Content.Split(' ')[1]);
              oldUser= msg.Author;
              break;
          }
        }
        if (newnum != oldnum + 1 || oldUser == newUser) { await message.DeleteAsync(); return; }
      }
      catch(Exception ex) { return; }

      Emote reactEmote;
      if (!Emote.TryParse("<:gogHappy:831979949036273715>", out reactEmote)) return;
      await message.AddReactionAsync(reactEmote);
      Bot.botStats.gogsCounted++;
    }
  }
}
