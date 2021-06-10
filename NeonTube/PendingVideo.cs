using System;

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


        public string GetThumbnailUrl()
        {
            string youtubeUrl = this.Url;

            string youTubeThumb = string.Empty;
            if (youtubeUrl == "")
                return "";

            if (youtubeUrl.IndexOf("=", StringComparison.Ordinal) > 0)
            {
                youTubeThumb = youtubeUrl.Split('=')[1];
            }
            else if (youtubeUrl.IndexOf("/v/", StringComparison.Ordinal) > 0)
            {
                string strVideoCode = youtubeUrl.Substring(youtubeUrl.IndexOf("/v/", StringComparison.Ordinal) + 3);
                int ind = strVideoCode.IndexOf("?", StringComparison.Ordinal);
                youTubeThumb = strVideoCode.Substring(0, ind == -1 ? strVideoCode.Length : ind);
            }
            else if (youtubeUrl.IndexOf('/') < 6)
            {
                youTubeThumb = youtubeUrl.Split('/')[3];
            }
            else if (youtubeUrl.IndexOf('/') > 6)
            {
                youTubeThumb = youtubeUrl.Split('/')[1];
            }

            return "http://img.youtube.com/vi/" + youTubeThumb + "/mqdefault.jpg";
        }
    }
}