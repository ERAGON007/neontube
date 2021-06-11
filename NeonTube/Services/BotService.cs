using System;
using Microsoft.Extensions.Options;
using MihaZupan;
using Telegram.Bot;

namespace NeonTube.Services
{
    public class BotService : IBotService
    {
        //private readonly BotConfiguration _config;

        public BotService(IOptions<BotConfiguration> config)
        {
            // _config = config.Value;
            Client = new TelegramBotClient(Environment.GetEnvironmentVariable("BotToken"));
        }

        public TelegramBotClient Client { get; }
    }
}
