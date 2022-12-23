using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Interactions;

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NinjaBotCore.Services
{
    public class LoggingService
    {
        private readonly ILogger _logger;
        private readonly DiscordSocketClient _discord;
        private readonly InteractionService _commands;
        
        public LoggingService(ILogger<LoggingService> logger, DiscordSocketClient discord, InteractionService commands)
        {            
            _discord  = discord;
            _commands = commands;
            _logger   = logger;

            //_discord. Ready += OnReadyAsync;
            _discord.Log += OnLogAsync;
            _commands.Log += OnLogAsync;
        }
        public Task OnReadyAsync()
        {
            _logger.LogInformation($"Connected as -> [] :)");
            _logger.LogInformation($"We are on [] servers");
            return Task.CompletedTask;
        }
        public Task OnLogAsync(LogMessage msg)
        {            
            string logText = $"{msg.Source}: {msg.Exception?.ToString() ?? msg.Message}";

            switch (msg.Severity.ToString())
            {
                case "Critical":
                {
                    _logger.LogCritical(logText);
                    break;
                }
                case "Warning":
                {
                    _logger.LogWarning(logText);
                    break;
                }
                case "Info":
                {
                    _logger.LogInformation(logText);
                    break;
                }
                case "Verbose":
                {
                    _logger.LogInformation(logText);
                    break;
                }    
                case "Debug":
                {
                    _logger.LogDebug(logText);
                    break;
                }  
                case "Error":
                {
                    _logger.LogError(logText);
                    break;
                }                                                          
            }
            return Task.CompletedTask;  
        }
    }
}