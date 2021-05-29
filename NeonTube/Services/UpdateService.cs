using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VideoLibrary;

namespace NeonTube.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IBotService _botService;
        private readonly ILogger<UpdateService> _logger;
        private static IVideoQueueService _videoQueueService;
        
        public UpdateService(IBotService botService, ILogger<UpdateService> logger,
            IVideoQueueService videoQueueService)
        {
            _botService = botService;
            _logger = logger;
            _videoQueueService = videoQueueService;
        }

        public async Task HandleUpdate(Update update)
        {
            if (update.Type != UpdateType.Message)
                return;

            var message = update.Message;

            if (message.Text.StartsWith("/"))
            {
                if (message.Text == "/start" || message.Text == "/help")
                {
                    await _botService.Client.SendTextMessageAsync(message.Chat.Id,
                        "Welcome, now send me your youtube video url to get it's video!").ConfigureAwait(true);
                    return;
                }

                return;
            }
            
            Uri uriResult;
            bool result = Uri.TryCreate(message.Text, UriKind.Absolute, out uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!result)
            {
                await _botService.Client.SendTextMessageAsync(message.Chat.Id, "Not a valid url!");
            }

            var split = message.Text.Split(' ');
            if (split.Length > 0)
            {
                await _botService.Client.SendTextMessageAsync(message.Chat.Id,
                    $"Added URL <code>{split[0]}</code> To Queue! your video will be sent soon",
                    ParseMode.Html);
                var newVideo = new PendingVideo(split[0], message.Chat.Id, message.MessageId);
                _videoQueueService.Add(newVideo);
            }
            else
            {
                await _botService.Client.SendTextMessageAsync(message.Chat.Id,
                    "Syntax: <code>/yt https://youtube.com</code>",
                    ParseMode.Html);
            }
        }
    }
}
