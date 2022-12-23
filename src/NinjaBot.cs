using Discord.Net;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Interactions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;
using NinjaBotCore.Database;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using NinjaBotCore.Modules.Wow;
using NinjaBotCore.Modules.Admin;
using NinjaBotCore.Modules.Steam;
using NinjaBotCore.Modules.RocketLeague;
using NinjaBotCore.Modules.Fun;
using NinjaBotCore.Modules.Away;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NinjaBotCore.Services;
using NinjaBotCore.Modules.Giphy;
using NinjaBotCore.Modules.Weather;
using NinjaBotCore.Modules.YouTube;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.File;
using Serilog.Sinks.SystemConsole;
using Microsoft;
using NinjaBotCore.Common;
using System.Net.Http;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using NinjaBotCore.Modules.Roll;

namespace NinjaBotCore
{
    public class NinjaBot
    {       
        //private CommandHandler _handler;                
        private IConfigurationRoot _config;
        private DiscordSocketClient _discord;
        private InteractionService _commands;
        public async Task StartAsync()
        {    
            //Create the configuration
            var _builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(path: "config.json");            
            _config = _builder.Build();

            

            //Configure services
            var services = new ServiceCollection()
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    //LogLevel = LogSeverity.Debug,
                    GatewayIntents = 
                        GatewayIntents.GuildMembers | 
                        GatewayIntents.GuildMessages | 
                        GatewayIntents.GuildIntegrations | 
                        GatewayIntents.Guilds |
                        GatewayIntents.GuildBans |
                        GatewayIntents.GuildVoiceStates |
                        GatewayIntents.GuildEmojis | 
                        GatewayIntents.GuildInvites | 
                        GatewayIntents.GuildMessageReactions |
                        GatewayIntents.GuildMessageTyping |
                        GatewayIntents.GuildWebhooks |
                        GatewayIntents.DirectMessageReactions |
                        GatewayIntents.DirectMessages | 
                        GatewayIntents.AllUnprivileged |
                        GatewayIntents.DirectMessageTyping,
                    AlwaysDownloadUsers= true,
                    LogLevel = LogSeverity.Debug,                     
                    MessageCacheSize = 1000,                    
                }))
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton(_config)
                //.AddSingleton(new InteractionService(new InteractionServiceConfig 
                //{ 
                //    DefaultRunMode = Discord.Commands.RunMode.Async,
                //    LogLevel = LogSeverity.Verbose,
                //    CaseSensitiveCommands = false, 
                //    ThrowOnError = false 
                //}))  
                .AddHttpClient()
                //.AddSingleton(new InteractionService(_discord, new InteractionServiceConfig
                //{
                //    //DefaultRunMode = Discord.Interactions.RunMode.Async,
                //    LogLevel = LogSeverity.Debug,
                //    //EnableAutocompleteHandlers = true,
                //    ThrowOnError = false
                //}))
                .AddSingleton<WowApi>()                                                
                .AddSingleton<WowUtilities>()
                .AddSingleton<WarcraftLogs>()
                .AddSingleton<ChannelCheck>()   
                .AddSingleton<OxfordApi>()
                .AddSingleton<AwayCommands>()
                //.AddSingleton<UserInteraction>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<StartupService>()
                .AddSingleton<SteamApi>()        
                .AddSingleton<GiphyApi>()    
                .AddSingleton<WeatherApi>()
                .AddSingleton<RaiderIOApi>()
                .AddSingleton<YouTubeApi>()                
                .AddSingleton<AudioService>()
                .AddScoped<Randos>()
                //.AddSingleton<SocketSlashCommand>()
                .AddSingleton<LoggingService>();

                
                        
            //Add logging      
            ConfigureServices(services);    

            //Build services
            var serviceProvider = services.BuildServiceProvider();                                     

            //Instantiate logger/tie-in logging
            serviceProvider.GetRequiredService<LoggingService>();

            //Start the bot
            await serviceProvider.GetRequiredService<StartupService>().StartAsync();


            var client = serviceProvider.GetRequiredService<DiscordSocketClient>();
            var commands = serviceProvider.GetRequiredService<InteractionService>();
            var _client = client;
            _commands = commands;
            //Load up services
            serviceProvider.GetRequiredService<CommandHandler>();//.InitializeAsync();
            //serviceProvider.GetRequiredService<UserInteraction>();
            serviceProvider.GetRequiredService<InteractionService>();
            //serviceProvider.GetRequiredService<SocketSlashCommand>();

            //var commands = serviceProvider.GetRequiredService<InteractionService>();
            //var _client = serviceProvider.GetRequiredService<DiscordSocketClient>();
            // Subscribe to client log events
            var _logger = serviceProvider.GetRequiredService<LoggingService>();
            // Subscribe to slash command log events
            //commands.Log += _ => serviceProvider.GetRequiredService<LoggingService>().Log(_);

            _client.Ready += async () =>
            {
                // If running the bot with DEBUG flag, register all commands to guild specified in config
                if (IsDebug())
                    // Id of the test guild can be provided from the Configuration object
                    await commands.RegisterCommandsToGuildAsync(UInt64.Parse(_config["testGuild"]), true);
                else
                    // If not debug, register commands globally
                    await commands.RegisterCommandsGloballyAsync(true);
            };

            //Block this program until it is closed.
            await Task.Delay(-1);
        }
        static bool IsDebug()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            //Add SeriLog
            services.AddLogging(configure => configure.AddSerilog()); 
            //Remove default HttpClient logging as it is extremely verbose
            //services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
            //Configure logging level              
            var logLevel = "Debug"; // Environment.GetEnvironmentVariable("NJA_LOG_LEVEL");
            var level = Serilog.Events.LogEventLevel.Error;
            if (!string.IsNullOrEmpty(logLevel))
            {
                switch (logLevel.ToLower())
                {
                    case "error":
                    {
                        level = Serilog.Events.LogEventLevel.Error;
                        break;
                    }
                    case "info":
                    {
                        level = Serilog.Events.LogEventLevel.Information;
                        break;
                    }
                    case "debug":
                    {
                        level = Serilog.Events.LogEventLevel.Debug;
                        break;
                    }
                    case "crit":
                    {
                        level = Serilog.Events.LogEventLevel.Fatal;
                        break;
                    }
                    case "warn":
                    {
                        level = Serilog.Events.LogEventLevel.Warning;
                        break;
                    }
                    case "trace":
                    {
                        level = Serilog.Events.LogEventLevel.Debug;
                        break;
                    }
                }
            }                                 
            Log.Logger = new LoggerConfiguration()
                    .WriteTo.File("logs/njabot.log", rollingInterval: RollingInterval.Day)
                    .WriteTo.Console()             
                    .MinimumLevel.Is(level)                                                                          
                    .CreateLogger();  
        }
    }
}