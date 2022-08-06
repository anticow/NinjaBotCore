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
using Microsoft.Extensions.Logging;

namespace NinjaBotCore.Modules.Roll
{
    public class RollCommand : ModuleBase
    {

        private static ChannelCheck _cc = null;
        private readonly IConfigurationRoot _config;
        private string _prefix;
        private readonly ILogger _logger;
        public enum dice { D2 = 2, D3 = 3, D4 = 4, D6 = 6, D8 = 8, D10 = 10, D12 = 12, D20 = 20, D100 = 100 }

        public RollCommand(ChannelCheck cc, IConfigurationRoot config, ILogger<RollCommand> logger)
        {
            _config = config;
            _cc = cc;
            _prefix = _config["prefix"];
            _logger = logger;
    }

        [Command("roll", RunMode = RunMode.Async)]
        [Alias("random")]
        [Summary("Generates a random roll of the dice")]
        public async Task roll()
        {
            StringBuilder sb = new StringBuilder();
            var random = new Random();
            var user = Context.User;

            var embed = new EmbedBuilder();
            embed.Title = $"{user.Username} has Rolled!";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(0, 255, 0));
            sb.AppendLine($"got: {random.NextInt64(1, 100)}");
            embed.Description = sb.ToString();
            await _cc.Reply(Context, embed);
        }
        [Command("roll", RunMode = RunMode.Async)]
        [Alias("random")]
        [Summary("Generates a random roll of the dice")]
        public async Task roll(long maximum)
        {
            StringBuilder sb = new StringBuilder();
            var random = new Random();
            var user = Context.User;
            var embed = new EmbedBuilder();
            embed.Title = $"{user.Username} has Rolled!";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(0, 255, 0));
            sb.AppendLine($"got: {random.NextInt64(1, maximum)}");
            embed.Description = sb.ToString();
            await _cc.Reply(Context, embed);
        }
        [Command("roll-dice", RunMode = RunMode.Async)]
        [Alias("dice")]
        [Summary("Roll the dice, number of dice, and side count 2,4,6,8,10,12,20,100; E.G. !dice 1 D4")]
        public async Task roll(int countOfDice,  dice die)
        {
            StringBuilder sb = new StringBuilder();
            var random = new Random();
            var user = Context.User;

            ISocketMessageChannel messageChannel = null;

            var embed = new EmbedBuilder();
            embed.Title = $"[{user.Username}] has Rolled the dice!";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(0, 255, 0));
            for (int i = 0; i < countOfDice; i++)
            {
                sb.AppendLine($"{die} shows: {random.Next(1, ((int)die))}");
            }
            embed.Description = sb.ToString();
            await _cc.Reply(Context, embed);
        }
        [Command("roll", RunMode = RunMode.Async)]
        [Alias("random")]
        [Summary("Generates a random roll of the dice")]
        public async Task roll(long minimum, long maximum)
        {
            StringBuilder sb = new StringBuilder();
            var random = new Random();
            var user = Context.User;

            var embed = new EmbedBuilder();
            embed.Title = $"{user.Username} has Rolled!";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(0, 255, 0));
            sb.AppendLine($"got: {random.NextInt64(minimum, maximum)}");
            embed.Description = sb.ToString();
            await _cc.Reply(Context, embed);
        }

    }
}
