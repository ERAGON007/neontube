namespace NeonTube
{
    public class PendingVideo
    {
        public PendingVideo(string url, long forChatId, int messageId)
        {
            Url = url;
            ForChatId = forChatId;
            MessageId = messageId;
        }
        public string Url { get; private set; }
        public long ForChatId { get; private set; }
        public int MessageId { get; private set; }
    }
}