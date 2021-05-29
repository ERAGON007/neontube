using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NeonTube.Services;
using Quartz;
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
                        var sentVideo = await _botService.Client.SendVideoAsync(firstQueue.ForChatId,
                            new InputOnlineFile(fileStream), replyToMessageId: firstQueue.MessageId);

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