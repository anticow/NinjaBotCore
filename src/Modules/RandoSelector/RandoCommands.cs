﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Net;
using Discord.WebSocket;
using Discord;
using Discord.Commands;
using NinjaBotCore.Database;
using NinjaBotCore.Models.Giphy;
using Microsoft.Extensions.Configuration;
using NinjaBotCore.Services;
using Microsoft.Extensions.Logging;



namespace NinjaBotCore.Modules.RandoSelector
{
    public class RandoCommands : ModuleBase
    {
        private static ChannelCheck _cc = null;
        private readonly IConfigurationRoot _config;
        private string _prefix;
        private readonly ILogger _logger;


        public RandoCommands(ChannelCheck cc, IConfigurationRoot config, ILogger<RandoCommands> logger)
        {
            _config = config;
            _cc = cc;
            _prefix = _config["prefix"];
            _logger = logger;

        }

        [Command("rando", RunMode = RunMode.Async)]
        [Alias("rando")]
        [Summary("Generates a random selection based on a pre-determined list")]
        public async Task RandoSelect(string args)
        {
            StringBuilder sb = new StringBuilder();

            var user = Context.User;
            var randoData = new Rando();

            try
            {
                var randoResult = randoData.getRandomFromList(args, null);
                sb.AppendLine($"**{randoResult}** is for you!");
                displayMessage(sb, $"Some Rando has decreed:");

            }
            catch (Exception ex)
            {

                _logger.LogError($"Rando Insert command error {ex.Message}");
                displayMessage(sb, $"Something collecting from the database for list {args}:");

            }

        }

        [Command("rando", RunMode = RunMode.Async)]
        [Alias("rando")]
        [Summary("Generates a random selection based on a pre-determined list")]
        public async Task RandoSelect(string itemName, int numberOfPlayers)
        {
            StringBuilder sb = new StringBuilder();

            var user = Context.User;
            var randoData = new Rando();

            try
            {
                var randoResult = randoData.getRandomFromList(itemName, numberOfPlayers);
                sb.AppendLine($"**{randoResult}** is for you!");
                displayMessage(sb, $"Some Rando has decreed:");

            }
            catch (Exception ex)
            {

                _logger.LogError($"Rando Insert command error {ex.Message}");
                displayMessage(sb, $"Something collecting from the database for list {itemName}:");

            }

        }


        [Command("rando-update", RunMode = RunMode.Async)]
        [Alias("rando-update")]
        [Summary("Generates a random selection based on a pre-determined list")]
        public async Task RandoItemUpdate(string listName, string listItem, int weight, int numberOfPlayers)
        {
            var user = Context.User;
            var rando = new Rando();

            rando.setRandomToList(listName, listItem, weight, numberOfPlayers, user.Username);

        }
        [Command("rando-insert", RunMode = RunMode.Async)]
        [Alias("rando-insert")]
        [Summary("Generates a random selection based on a pre-determined list")]
        public async Task RandoInsert(string args)
        {
            StringBuilder sb = new StringBuilder();
            var user = Context.User;
            Rando randoData = new Rando();
            var splitArgs = args.Split(' ');
            var itemWeight = 0;
            if (splitArgs.Length < 4)
            {
                sb.AppendLine("List Name,");
                sb.AppendLine("List Item to add,");
                sb.AppendLine("Weight of item, the higher the value the more often it will appear");
                sb.AppendLine("Number of players");
                displayMessage(sb, $"Adding to the List requires 4 input;");

            }
            else
            {
                bool failure = false;
                int weight;
                bool successWeight = int.TryParse(splitArgs[2], out weight);
                if (!successWeight)
                {
                    failure = true;
                    displayMessage(sb, $"Item Weight invalid {splitArgs[2]}");

                }
                int playerCount;
                bool playersInt = int.TryParse(splitArgs[3], out playerCount);
                if (!playersInt)
                {
                    failure = true;
                    displayMessage(sb, $"Player count invalid {splitArgs[3]}");

                }

                if (!failure)
                {
                    randoData.setRandomToList(splitArgs[0], splitArgs[1], weight, playerCount, splitArgs[4]);
                }
                else
                {
                    sb.AppendLine("Inputs incorrect, please try again");
                    sb.AppendLine("Adding to the List requires 4 input;");
                    sb.AppendLine("List Name,");
                    sb.AppendLine("List Item to add,");
                    sb.AppendLine("Weight of item, the higher the value the more often it will appear");
                    sb.AppendLine("Number of players");

                    displayMessage(sb, $"Adding to list requires the following");

                }

            }


        }
        [Command("rando-insert", RunMode = RunMode.Async)]
        [Alias("rando-insert")]
        [Summary("Adds a new value to the list list")]
        public async Task RandoInsert(string listName, string itemName, int itemWeight, int playerCount)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                var user = Context.User;
                Rando randoData = new Rando();

                bool failure = false;
                int weight;

                randoData.setRandomToList(listName, itemName, itemWeight, playerCount, user.Username);

                sb.AppendLine($"{itemName} has been added!");

                displayMessage(sb, $"Adding to List {listName}");

            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Something went adding to the rando list: {ex.Message}");
                _logger.LogError($"Rando Insert command error {ex.Message}");
                displayMessage(sb, $"Error adding to list");

            }
        }


        [Command("rando-insert", RunMode = RunMode.Async)]
        [Alias("rando-insert")]
        [Summary("Generates a random selection based on a pre-determined list")]
        public async Task RandoInsert()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Inputs incorrect, please try again");
            sb.AppendLine("Adding to the List requires 4 input;");
            sb.AppendLine("List Name,");
            sb.AppendLine("List Item to add,");
            sb.AppendLine("Weight of item, the higher the value the more often it will appear");
            sb.AppendLine("Number of players");
            displayMessage(sb, $"List addition requires:");

        }

        [Command("rando-fulldetail", RunMode = RunMode.Async)]
        [Alias("rando-deets")]
        [Summary("Generates a random selection based on a pre-determined list")]
        public async Task getRandoDetails(string listName, string itemName, int playerCount)
        {
            StringBuilder sb = new StringBuilder();
            var rando = new Rando();
            var details = rando.getFullItemContext(listName, itemName, playerCount);
            sb.AppendLine($"Item Details: ");
            sb.AppendLine($"Item Name: {details.ListItem} ");
            sb.AppendLine($"Item Created by: {details.AddedBy} ");
            sb.AppendLine($"Item Id: {details.ListId} ");
            sb.AppendLine($"Item Weight: {details.ListWeight}");
            sb.AppendLine($"Item PlayerCount: {details.NumberOfPlayers}");
            sb.AppendLine($"Item included in list: {details.ListName}");
            //await _cc.Reply(Context, sb.ToString());
            displayMessage(sb, $"Details of Item in list {listName}");
        }
        [Command("rando-list", RunMode = RunMode.Async)]
        [Alias("rando-list")]
        [Summary("Generates a random selection based on a pre-determined list")]
        public async Task getRandoList(string listName)
        {
            StringBuilder sb = new StringBuilder();
            var rando1 = new Rando();
            var randoReturn = rando1.getItemList(listName);
                          
            foreach (var rando in randoReturn)
            {
                sb.AppendLine($"Item Name: **{rando.ListItem}** \r\n[Player Count: {rando.NumberOfPlayers}], [Item Weight: {rando.ListWeight}]");
            }
            displayMessage(sb, $"List: {listName} contains {randoReturn.Count} items");

        }
        [Command("rando-list", RunMode = RunMode.Async)]
        [Alias("rando-list")]
        [Summary("Generates a random selection based on a pre-determined list")]
        public async Task getRandoList()
        {
            StringBuilder sb = new StringBuilder();
            var rando1 = new Rando();
            var randoReturn = rando1.getItemList();

            foreach (var rando in randoReturn)
            {
                sb.AppendLine($"List Name: **{rando.Key}** [Item Count: {rando.Value}]");
            }
            displayMessage(sb, $"There are {randoReturn.Count} lists");

        }

        [Command("rando-remove", RunMode = RunMode.Async)]
        [Alias("rando-remove")]
        [Summary("Generates a random selection based on a pre-determined list")]
        public async Task removeRandomFromList(string guidValue)
        {
            StringBuilder sb = new StringBuilder();
            var rando = new Rando();
            var itemGuid = new Guid(guidValue);
            rando.removeRandomFromList(itemGuid);
            sb.AppendLine($"item removed");
            displayMessage(sb, $"{rando.getFullItemContext(itemGuid).ListItem} removed");

        }

        public void displayMessage(StringBuilder sb, string title)
        {
            var embed = new EmbedBuilder();
            embed.Title = title;
            embed.Description = sb.ToString();
            embed.WithColor(new Color(0, 255, 0));
            //sb.AppendLine($"got: {random.NextInt64(minimum, maximum)}");
            //embed.Description = sb.ToString();
            _cc.Reply(Context, embed).GetAwaiter().GetResult();

        }
    }
}