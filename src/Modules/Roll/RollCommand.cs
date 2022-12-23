using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Net;
using Discord.WebSocket;
using Discord;
//using Discord.Commands;
using NinjaBotCore.Database;
using Discord.Interactions;
using NinjaBotCore.Models.Giphy;
using Microsoft.Extensions.Configuration;
using NinjaBotCore.Services;
using Microsoft.Extensions.Logging;

namespace NinjaBotCore.Modules.Roll
{
    public class RollCommand : InteractionModuleBase<SocketInteractionContext>
    {

        //private static ChannelCheck _cc = null;
        //private readonly IConfigurationRoot _config;
        //private string _prefix;
        private readonly ILogger _logger;
        public InteractionService Commands { get; set; }
        private CommandHandler _handler;
       // private SocketSlashCommand _slash;
        public enum dice { D2 = 2, D3 = 3, D4 = 4, D6 = 6, D8 = 8, D10 = 10, D12 = 12, D20 = 20, D100 = 100 }

        public RollCommand(ILogger<RollCommand> logger, CommandHandler handler) //ChannelCheck cc, IConfigurationRoot config, 
        {
            _handler = handler;
            //_config = config;
            //_cc = cc;
            //_prefix = _config["prefix"];
            _logger = logger;
            _logger.LogDebug("Roll command loaded");
            //_slash = handler;
    }
        [SlashCommand("roll","Roll for your life!")]
        public async Task roll() //[Summary("parameter_name"), Autocomplete(typeof(rollAutocompleteHandler))] string parameterWithAutocompletion
        {
            _logger.LogDebug("Roll command called, can we roll?");
            var simple = "simpletest";
            //StringBuilder sb = new StringBuilder();
            //var random = new Random();
            //var user = Context.User;

            //var embed = new EmbedBuilder();
            //embed.Title = $"{user.Username} has Rolled!";
            //embed.Description = sb.ToString();
            //embed.WithColor(new Color(0, 255, 0));
            //sb.AppendLine($"got: {random.NextInt64(1, 100)}");
            //embed.Description = sb.ToString();
            //_logger.LogDebug(sb.ToString());
            //await _cc.Reply(Context, embed);
            //await _slash.RespondAsync(simple);
            //await _slash.FollowupAsync(simple);
            await RespondAsync(simple);
        }
        //public class rollAutocompleteHandler : AutocompleteHandler
        //{
        //    public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        //    {
        //        // Create a collection with suggestions for autocomplete
        //        IEnumerable<AutocompleteResult> results = new[]
        //        {
        //    new AutocompleteResult("roll", "roll value description"),
        //    new AutocompleteResult("Name2", "value2")
        //};

        //        // max - 25 suggestions at a time (API limit)
        //        return AutocompletionResult.FromSuccess(results.Take(25));
        //    }
        //}

        //[Command("roll", RunMode = RunMode.Async)]
        //[Alias("random")]
        //[Summary("Generates a random roll of the dice")]
        //public async Task roll(long maximum)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    var random = new Random();
        //    var user = Context.User;
        //    var embed = new EmbedBuilder();
        //    embed.Title = $"{user.Username} has Rolled!";
        //    embed.Description = sb.ToString();
        //    embed.WithColor(new Color(0, 255, 0));
        //    sb.AppendLine($"got: {random.NextInt64(1, maximum)}");
        //    embed.Description = sb.ToString();
        //    await _cc.Reply(Context, embed);
        //}
        //[Command("roll-dice", RunMode = RunMode.Async)]
        //[Alias("dice")]
        //[Summary("Roll the dice, number of dice, and side count 2,4,6,8,10,12,20,100; E.G. !dice 1 D4")]
        //public async Task roll(int countOfDice,  dice die)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    var random = new Random();
        //    var user = Context.User;
        //
        //    ISocketMessageChannel messageChannel = null;
        //
        //    var embed = new EmbedBuilder();
        //    embed.Title = $"[{user.Username}] has Rolled the dice!";
        //    embed.Description = sb.ToString();
        //    embed.WithColor(new Color(0, 255, 0));
        //    int total = 0;
        //    for (int i = 0; i < countOfDice; i++)
        //    {
        //        int currentDie = random.Next(1, (int)die);
        //        sb.AppendLine($"**{die}** shows: {currentDie}");
        //        total += currentDie;
        //    }
        //    sb.AppendLine($"Total count from die rolls **{total}**");
        //    embed.Description = sb.ToString();
        //    await _cc.Reply(Context, embed);
        //}
        //[Command("roll-dice", RunMode = RunMode.Async)]
        //[Alias("dice")]
        //[Summary("Roll the dice, number of dice, and side count 2,4,6,8,10,12,20,100; E.G. !dice 1D4")]
        //public async Task roll(string die)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    var random = new Random();
        //    var user = Context.User;
        //
        //    dice dieType = new dice();
        //    ISocketMessageChannel messageChannel = null;
        //    if (Int32.TryParse(die.FirstFromSplit('d'), out int countOfDice))
        //    {
        //        var thisDice = die.Split(countOfDice.ToString())[1];
        //        if (Enum.IsDefined(typeof(dice),thisDice.ToUpper())) 
        //            dieType = Enum.Parse<dice>(thisDice.ToUpper());
        //    }
        //    //var countOfDice = integer.par ;
        //    var embed = new EmbedBuilder();
        //    embed.Title = $"[{user.Username}] has Rolled the dice!";
        //    embed.Description = sb.ToString();
        //    embed.WithColor(new Color(0, 255, 0));
        //    int total = 0;
        //    for (int i = 0; i < countOfDice; i++)
        //    {
        //        int currentDie = random.Next(1, (int)dieType);
        //        sb.AppendLine($"**{die.Split(countOfDice.ToString())[1].ToUpper()}** shows: **{currentDie}**");
        //        total += currentDie;
        //    }
        //    sb.AppendLine($"Total count from die rolls: **{total}**");
        //    embed.Description = sb.ToString();
        //    await _cc.Reply(Context, embed);
        //}
        //
        //[Command("roll", RunMode = RunMode.Async)]
        //[Alias("random")]
        //[Summary("Generates a random roll of the dice")]
        //public async Task roll(long minimum, long maximum)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    var random = new Random();
        //    var user = Context.User;
        //
        //    var embed = new EmbedBuilder();
        //    embed.Title = $"{user.Username} has Rolled!";
        //    embed.Description = sb.ToString();
        //    embed.WithColor(new Color(0, 255, 0));
        //    sb.AppendLine($"got: {random.NextInt64(minimum, maximum)}");
        //    embed.Description = sb.ToString();
        //    await _cc.Reply(Context, embed);
        //}
        //
    }
}
