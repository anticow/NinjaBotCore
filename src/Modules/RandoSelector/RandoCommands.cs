using System;
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


namespace NinjaBotCore.Modules.RandoSelector
{
    public class RandoCommands : ModuleBase
    {
        private static ChannelCheck _cc = null;
        private readonly IConfigurationRoot _config;
        private string _prefix;

        public RandoCommands(ChannelCheck cc, IConfigurationRoot config)
        {
            _config = config;
            _cc = cc;
            _prefix = _config["prefix"];
        }

        [Command("rando", RunMode = RunMode.Async)]
        [Alias("rando")]
        [Summary("Generates a random selection based on a pre-determined list")]
        public async Task RandoSelect(string args)
        {
            var user = Context.User;

        }

        [Command("rando-update", RunMode = RunMode.Async)]
        [Alias("rando-update")]
        [Summary("Generates a random selection based on a pre-determined list")]
        public async Task RandoItemUpdate(string args)
        {
            var user = Context.User;

        }
        [Command("rando-insert", RunMode = RunMode.Async)]
        [Alias("rando-insert")]
        [Summary("Generates a random selection based on a pre-determined list")]
        public async Task RandoInsert(string args)
        {
            StringBuilder sb = new StringBuilder();
            var user = Context.User;
            RandoData randoData = new RandoData();
            var splitArgs = args.Split(' ');
            var itemWeight = 0;
            if (splitArgs.Length < 4)
            {
                sb.AppendLine("Adding to the List requires 4 input;");
                sb.AppendLine("List Name,");
                sb.AppendLine("List Item to add,");
                sb.AppendLine("Weight of item, the higher the value the more often it will appear");
                sb.AppendLine("Number of players");
            }
            else
            {
                bool failure = false;
                int weight;
                bool successWeight = int.TryParse(splitArgs[2], out weight);
                if (!successWeight)
                {
                    sb.AppendLine("item weight invalid");
                    failure = true;
                }
                int playerCount;
                bool playersInt = int.TryParse(splitArgs[2], out playerCount);
                if (!playersInt)
                {
                    sb.AppendLine("player count invalid");
                    failure = true;
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


                }

            }


        }
        [Command("rando-insert", RunMode = RunMode.Async)]
        [Alias("rando-insert")]
        [Summary("Generates a random selection based on a pre-determined list")]
        public async Task RandoInsert(string listName, string itemName, int itemWeight, int playerCount)
        {
            StringBuilder sb = new StringBuilder();
            var user = Context.User;
            RandoData randoData = new RandoData();

            bool failure = false;
            int weight;

            randoData.setRandomToList(listName, itemName, itemWeight, playerCount, user.Username);

            sb.AppendLine("Inputs incorrect, please try again");
            sb.AppendLine("Adding to the List requires 4 input;");
            sb.AppendLine("List Name,");
            sb.AppendLine("List Item to add,");
            sb.AppendLine("Weight of item, the higher the value the more often it will appear");
            sb.AppendLine("Number of players");
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
        }

    }
}
