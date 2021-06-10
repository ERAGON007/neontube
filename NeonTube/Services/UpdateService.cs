using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
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
            switch (update.Type)
            {
                case UpdateType.Message:
                {
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
            

                    if (!Uri.IsWellFormedUriString(message.Text, UriKind.Absolute))
                    {
                        await _botService.Client.SendTextMessageAsync(message.Chat.Id, "Not a valid url!");
                        return;
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

                    break;
                }

                case UpdateType.InlineQuery:
                {
                    var inlineQuery = update.InlineQuery;
                    if (inlineQuery != null)
                    {
                        if (inlineQuery.Query.StartsWith("sh:"))
                        {
                            var response = new InlineQueryResultCachedVideo(Guid.NewGuid().ToString(),
                                inlineQuery.Query.Split("sh:").Last(), "Share this video")
                            {
                                ReplyMarkup = new [] {
                                new InlineKeyboardButton()
                                {
                                    Text = "⚡️ Share",
                                    SwitchInlineQuery = "sh:" + inlineQuery.Query.Split("sh:").Last()
                                }
                                },
                                Caption = $"Downloaded by: @{_botService.Client.GetMeAsync().Result.Username}",
                                Description = "To share this video, click here!",
                            };

                            
                            

                            await _botService.Client.AnswerInlineQueryAsync(inlineQuery.Id,
                                new List<InlineQueryResultBase>
                                {
                                    response
                                });
                        }
                    }
                    break;
                }
            }
            

        }
    }
}
