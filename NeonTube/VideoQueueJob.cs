using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using NeonTube.Services;
using Quartz;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using VideoLibrary;

namespace NeonTube
{
    [DisallowConcurrentExecution]
    public class VideoQueueJob : IJob
    {
        private static IBotService _botService;
        private static IVideoQueueService _videoQueueService;
        
        public VideoQueueJob(IBotService botService, IVideoQueueService videoQueueService)
        {
            _botService = botService;
            _videoQueueService = videoQueueService;
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            PendingVideo first;
            if (_videoQueueService.Count() > 0)
            {
                try
                {
                    first = _videoQueueService.First();

                    if (first == null)
                        return;

                    var firstQueue = first;
                    _videoQueueService.Remove(first);

                    var youTube = YouTube.Default;
                    var video = await youTube.GetVideoAsync(firstQueue.Url).ConfigureAwait(true);


                    await using (Stream fileStream = await video.StreamAsync())
                    {
                        TimeSpan duration = new TimeSpan();

                        if (video.Info.LengthSeconds != null)
                        {
                            duration = TimeSpan.FromSeconds((float)video.Info.LengthSeconds);
                        }
                        
                        string caption = $"Title: {video.Title}<br><br>" +
                                         $"Length: {duration.Hours}:{duration.Minutes}:{duration.Seconds}<br>";
                        
                        await _botService.Client.SendVideoAsync(firstQueue.ForChatId,
                            new InputOnlineFile(fileStream), replyToMessageId: firstQueue.MessageId,
                            supportsStreaming: true, thumb: new InputMedia(firstQueue.GetThumbnailUrl()),
                            caption: caption);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred in VideoQueueJob : {ex.Message}");
                    Console.Write(ex.StackTrace);
                }
            }
        }
    }
}