using System.Collections.Generic;
using System.Linq;

namespace NeonTube.Services
{
    public class VideoQueueService : IVideoQueueService
    {
        private static List<PendingVideo> _pendingVideos = new List<PendingVideo>();
        
        public void Add(PendingVideo item)
        {
            _pendingVideos.Add(item);
        }

        public void Remove(PendingVideo item)
        {
            _pendingVideos.Remove(item);
        }

        public PendingVideo? First()
        {
            return _pendingVideos.FirstOrDefault();
        }

        public int Count()
        {
            return _pendingVideos.Count;
        }
    }
}