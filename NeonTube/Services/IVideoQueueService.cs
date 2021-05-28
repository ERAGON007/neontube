using System.Collections.Generic;

namespace NeonTube.Services
{
    public interface IVideoQueueService
    {
        void Add(PendingVideo item);
        void Remove(PendingVideo item);
        PendingVideo? First();
        int Count();
    }
}