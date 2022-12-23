using System.Threading.Tasks;
using System.Reflection;
using Discord.Net;
//using Discord.Commands;
using Discord.WebSocket;
using Discord.Interactions;

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;
using NinjaBotCore.Database;
using Discord;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace NinjaBotCore.Services
{
    public class CommandHandler
    {
        //private InteractionService _commands;
        //private readonly IConfigurationRoot _config;
        //private InteractionService _interactions;
        private readonly DiscordSocketClient _client;
        private readonly ILogger _logger;
        private readonly IServiceProvider _services;
        private readonly InteractionService _commands;

        public CommandHandler(InteractionService commands, DiscordSocketClient client, IServiceProvider services )
        {
            //_config = services.GetRequiredService<IConfigurationRoot>();
            //_interactions = services.GetRequiredService<InteractionService>();
            _services = services;
            _client = client;// services.GetRequiredService<DiscordSocketClient>();
            _commands = commands;// services.GetRequiredService<InteractionService>();
            _logger = services.GetRequiredService<ILogger<CommandHandler>>();
            _logger.LogInformation("InteractionService Started");
        }

        private async Task LogCommandUsage(SocketInteractionContext context, IResult result)
        {
            await Task.Run(async () =>
            {
                if (context.Channel is IGuildChannel)
                {
                    var logTxt = $"User: [{context.User.Username}]<->[{context.User.Id}] Discord Server: [{context.Guild.Name}] -> [{context.Interaction.Data}]";
                    _logger.LogInformation(logTxt);
                }
                else
                {
                    var logTxt = $"User: [{context.User.Username}]<->[{context.User.Id}] -> [{context.Interaction.Data}]";
                    _logger.LogInformation(logTxt);
                }
            });
        }

        public async Task InitializeAsync()
        {
            // process the InteractionCreated payloads to execute Interactions commands
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            _client.InteractionCreated += HandleInteraction;

            _logger.LogInformation("HandleInteraction loaded");
            // process the command execution results 
            _commands.SlashCommandExecuted += SlashCommandExecuted;
            _commands.ContextCommandExecuted += ContextCommandExecuted;
            _commands.ComponentCommandExecuted += ComponentCommandExecuted;
            _logger.LogInformation("command handlers loaded");
        }
        private Task ComponentCommandExecuted(ComponentCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        _logger.LogDebug(arg3.Error.Value.ToString());
                        break;
                    case InteractionCommandError.UnknownCommand:
                        _logger.LogDebug(arg3.Error.Value.ToString());
                        break;
                    case InteractionCommandError.BadArgs:
                        _logger.LogDebug(arg3.Error.Value.ToString());
                        break;
                    case InteractionCommandError.Exception:
                        _logger.LogDebug(arg3.Error.Value.ToString());
                        break;
                    case InteractionCommandError.Unsuccessful:
                        _logger.LogDebug(arg3.Error.Value.ToString());
                        break;
                    default:
                        break;
                }
            }
            _logger.LogInformation(arg3.ToString());

            return Task.CompletedTask;
        }

        private Task ContextCommandExecuted(ContextCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        _logger.LogDebug(arg3.Error.Value.ToString());
                        break;
                    case InteractionCommandError.UnknownCommand:
                        _logger.LogDebug(arg3.Error.Value.ToString());
                        break;
                    case InteractionCommandError.BadArgs:
                        _logger.LogDebug(arg3.Error.Value.ToString());
                        break;
                    case InteractionCommandError.Exception:
                        _logger.LogDebug(arg3.Error.Value.ToString());
                        break;
                    case InteractionCommandError.Unsuccessful:
                        _logger.LogDebug(arg3.Error.Value.ToString());
                        break;
                    default:
                        break;
                }
            }
            _logger.LogInformation(arg3.ToString());
            return Task.CompletedTask;
        }

        private Task SlashCommandExecuted(SlashCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            _logger.LogInformation("slash command used");
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        _logger.LogDebug(arg3.Error.Value.ToString());
                        break;
                    case InteractionCommandError.UnknownCommand:
                        _logger.LogDebug(arg3.Error.Value.ToString());
                        break;
                    case InteractionCommandError.BadArgs:
                        _logger.LogDebug(arg3.Error.Value.ToString());
                        break;
                    case InteractionCommandError.Exception:
                        _logger.LogDebug(arg3.Error.Value.ToString());
                        break;
                    case InteractionCommandError.Unsuccessful:
                        _logger.LogDebug(arg3.Error.Value.ToString());
                        break;
                    default:
                        break;
                }
            }
            _logger.LogInformation(arg3.ToString());

            _logger.LogInformation("slash command used");
            return Task.CompletedTask;
        }
        private async Task HandleInteraction(SocketInteraction arg)
        {
            _logger.LogDebug(arg.ToString());
            _logger.LogDebug("starting interaction handle");
            try
            {
               // if (arg.Channel is IGuildChannel)
               // {
               //     var logTxt = $"User: [{arg.User.Username}]<->[{arg.User.Id}] Discord Server: [{arg.GuildId}] -> [{arg.Data}]";
               //     _logger.LogInformation(logTxt);
               // }
               // else
               // {
               //     var logTxt = $"User: [{arg.User.Username}]<->[{arg.User.Id}] -> [{arg.Data}]";
               //     _logger.LogInformation(logTxt);
               // }
                _logger.LogDebug("Interaction called");
                
                // create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules
                var ctx = new SocketInteractionContext(_client, arg);
                await _commands.ExecuteCommandAsync(ctx, _services);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _logger.LogError(ex.Message);
                // if a Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
                // response, or at least let the user know that something went wrong during the command execution.
                if (arg.Type == InteractionType.ApplicationCommand)
                {
                    await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
                }
            }

                //public async Task HandleCommand(SocketInteraction socketInteraction)
                //{
                //    // Don't handle the command if it is a system message
                //    var message = socketInteraction;// as SocketMessage;            
                //    if (message == null) return;

                //    // Don't listen to bots
                //    if (message.User.IsBot)
                //    {

                //        return;
                //    }

                //    // Mark where the prefix ends and the command begins
                //    int argPos = 0;

                //    // Create a Command Context
                //    var context = new SocketInteractionContext(_client, message);

                //    char prefix = Char.Parse(_config["prefix"]);

                //    var serverPrefix = GetPrefix((long)context.Guild.Id);

                //    if (serverPrefix != null)
                //    {
                //        prefix = serverPrefix.Prefix;
                //    }

                //    // Determine if the message has a valid prefix, adjust argPos
                //    //if (!(message. HasMentionPrefix(_client.CurrentUser, ref argPos) || message.HasCharPrefix(prefix, ref argPos))) return;

                //    //Check blacklist
                //    List<Blacklist> blacklist = new List<Blacklist>();

                //    using (var db = new NinjaBotEntities())
                //    {
                //        blacklist = db.Blacklist.ToList();
                //    }
                //    if (blacklist != null)
                //    {
                //        var matched = blacklist.Where(b => b.DiscordUserId == (long)context.User.Id).FirstOrDefault();
                //        if (matched != null)
                //        {
                //            return;
                //        }
                //    }

                //    // Execute the Command, store the result            
                //    var result = await _commands.ExecuteCommandAsync(context, _services);// ExecuteAsync(context, argPos, _services);

                //    await LogCommandUsage(context, result);
                //    // If the command failed, notify the user
                //    if (!result.IsSuccess)
                //    {
                //        if (result.ErrorReason != "Unknown command.")
                //        {
                //            await message.Channel.SendMessageAsync($"**Error:** {result.ErrorReason}");
                //        }
                //    }
                //}
                //        private async Task ReadyAsync()
                //        {
                //            _logger.LogInformation("Handler Readying");

                //            ulong _guildId = 564662942780358677;
                //            var commands =
                //#if DEBUG
                //                await _commands.RegisterCommandsToGuildAsync(_guildId); //_configuration.GetValue<ulong>("testguild")
                //#else
                //                await _handler.RegisterCommandsGloballyAsync();
                //#endif

                //            foreach (var command in commands)
                //                _ = _services.GetRequiredService<LoggingService>();//.DebugAsync($"Name:{command.Name} Type.{command.Type} loaded");


                //        }


                //private PrefixList GetPrefix(long serverId)
                //{
                //    PrefixList prefix = null;

                //    using (var db = new NinjaBotEntities())
                //    {
                //        prefix = db.PrefixList.Where(p => p.ServerId == serverId).FirstOrDefault();
                //    }

                //    return prefix;
                //}

                /*
string commandIssued = string.Empty;
if (!result.IsSuccess)
{
    request.Success = false;
    request.FailureReason = result.ErrorReason;
}
          request.ChannelId = (long)context.Channel.Id;
request.ChannelName = context.Channel.Name;
request.UserId = (long)context.User.Id;
request.Command = context.Message.Content;
request.UserName = context.User.Username;
request.Success = true;
request.RequestTime = DateTime.Now;
using (var db = new NinjaBotEntities())
{

    db.Requests.Add(request);
    await db.SaveChangesAsync();
}
 */
            
            //private async Task LogCommandUsage(SocketInteractionContext context, IResult result)
            //{
            //    await Task.Run(async () =>
            //    {
            //        if (context.Channel is IGuildChannel)
            //        {
            //            var logTxt = $"User: [{context.User.Username}]<->[{context.User.Id}] Discord Server: [{context.Guild.Name}] -> [{context.Interaction.Data}]";
            //            _logger.LogInformation(logTxt);
            //        }
            //        else
            //        {
            //            var logTxt = $"User: [{context.User.Username}]<->[{context.User.Id}] -> [{context.Interaction.Data}]";
            //            _logger.LogInformation(logTxt);
            //        }
            //    });
            //}

        }
    }
}