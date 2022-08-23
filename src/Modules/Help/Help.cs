using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NinjaBotCore.Services;
using Microsoft.Extensions.Logging;
using Discord.WebSocket;
using Discord;
using Discord.Commands;

namespace NinjaBotCore.Modules.Help
{
    public class Help : ModuleBase
    {

        private static ChannelCheck _cc = null;
        private readonly IConfigurationRoot _config;
        private string _prefix;
        private readonly ILogger _logger;
        public Help(ChannelCheck cc, IConfigurationRoot config, ILogger<Help> logger)
        {
            _config = config;
            _cc = cc;
            _prefix = _config["prefix"];
            _logger = logger;
        }

        [Command("help", RunMode = RunMode.Async)]
        [Alias("help")]
        [Summary("Display available comamnds")]
        public async Task roll()
        {
            StringBuilder sb = new StringBuilder();
            var user = Context.User;

            var embed = new EmbedBuilder();
            embed.Title = $"Commands Available";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(0, 255, 0));
            
            sb.AppendLine($"**!Rando** <name of list>; e.g. !Rando multi");
            sb.AppendLine($"**!Rando-insert** <listname> <\"gameName\" <weight, usually 1>, <maximum Player Count> \r\n e.g. !rando-insert multi vrising 1 40");
            sb.AppendLine($"**!rando-update** <listName> <gameName> <weight> <player count>, this will allow you to change a value based on list and title names, normally player count changes");
            sb.AppendLine($"**!rando-list**, returns all known lists, and count of games");
            sb.AppendLine($"**!rando-list** <listName>, returns all titles in the list");
            sb.AppendLine($"**!roll**, multiple options here !roll for a 1-100 result, !roll <low number> <high number>, for range");
            sb.AppendLine($"**!roll-dice** 1d6 to get a die roll first number is any number in an int and the last number must match a real die");
            embed.Description = sb.ToString();
            await _cc.Reply(Context, embed);
        }


    }
}
