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
  internal class Info : ISlashCommand
  {
    public SlashCommandBuilder Builder
    {
      get
      {
        SlashCommandBuilder builder = new SlashCommandBuilder();
        builder.Name = nameof(Info).ToLower();
        builder.Description = "Gets some info about the bot.";
        builder.IsDMEnabled = true;

        return builder;
      }
    }
    public async Task Setup(WizBot Bot)
    {
      return;
    }
    public async Task Execute(SocketSlashCommand inputCommand, WizBot Bot)
    {
      EmbedBuilder embed = new EmbedBuilder();
      embed.Title = "Bot info. Very nice.";
      embed.ImageUrl = $"https://opengraph.githubassets.com/{new Random().Next()}/BasicallyWiz/KoolWhiz";

      EmbedFieldBuilder clientField = new EmbedFieldBuilder();
      clientField.Name = "Discord Client";
      clientField.Value = //$"DMs open: {Bot.client.GetDMChannelsAsync().Result.Count()}\n" +
      $"Servers: {Bot.client.Guilds.Count}\n" +
      $"SlashCommands used this session: {Bot.botStats.slashCommandsUsed}\n" +
      $"Gogs counted this session: {Bot.botStats.gogsCounted}";
      embed.AddField(clientField);


      EmbedFieldBuilder appField = new EmbedFieldBuilder();
      appField.Name = "App info";
      appField.Value = $"Application launched <t:{(int)Bot.botStats.creationDateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds}:R>, <t:{(int)Bot.botStats.creationDateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds}:f>\n" +
      $"Repo: https://github.com/BasicallyWiz/KoolWhiz (YOU can contribute!)";
      embed.AddField(appField);

      await inputCommand.RespondAsync(embed: embed.Build());
    }
  }
}
