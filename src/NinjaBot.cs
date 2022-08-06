using Discord.Net;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
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

namespace NinjaBotCore
{
    public class NinjaBot
    {       
        private CommandHandler _handler;                
        private IConfigurationRoot _config;

        public async Task StartAsync()
        {    
            //Create the configuration
            var _builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(path: "config.json");            
            _config = _builder.Build();
            
            //Configure services
            var services = new ServiceCollection()
                .AddSingleton(new DiscordShardedClient(new DiscordSocketConfig
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
                        GatewayIntents.DirectMessageTyping,                
                    LogLevel = LogSeverity.Error,                     
                    MessageCacheSize = 1000,                    
                }))
                .AddSingleton(_config)
                .AddSingleton(new CommandService(new CommandServiceConfig 
                { 
                    DefaultRunMode = RunMode.Async,
                    LogLevel = LogSeverity.Verbose,
                    CaseSensitiveCommands = false, 
                    ThrowOnError = false 
                }))  
                .AddHttpClient()
                .AddSingleton<WowApi>()                                                
                .AddSingleton<WowUtilities>()
                .AddSingleton<WarcraftLogs>()
                .AddSingleton<ChannelCheck>()   
                .AddSingleton<OxfordApi>()
                .AddSingleton<AwayCommands>()
                .AddSingleton<UserInteraction>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<StartupService>()
                .AddSingleton<SteamApi>()        
                .AddSingleton<GiphyApi>()    
                .AddSingleton<WeatherApi>()
                .AddSingleton<RaiderIOApi>()
                .AddSingleton<YouTubeApi>()                
                .AddSingleton<AudioService>()
                .AddScoped<Randos>()
                .AddSingleton<LoggingService>();
                
                        
            //Add logging      
            ConfigureServices(services);    

            //Build services
            var serviceProvider = services.BuildServiceProvider();                                     

            //Instantiate logger/tie-in logging
            serviceProvider.GetRequiredService<LoggingService>();

            //Start the bot
            await serviceProvider.GetRequiredService<StartupService>().StartAsync();

            //Load up services
            serviceProvider.GetRequiredService<CommandHandler>();
            serviceProvider.GetRequiredService<UserInteraction>();            
                                                      
            //Block this program until it is closed.
            await Task.Delay(-1);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            //Add SeriLog
            services.AddLogging(configure => configure.AddSerilog()); 
            //Remove default HttpClient logging as it is extremely verbose
            services.RemoveAll<IHttpMessageHandlerBuilderFilter>();       
            //Configure logging level              
            var logLevel = Environment.GetEnvironmentVariable("NJA_LOG_LEVEL");
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