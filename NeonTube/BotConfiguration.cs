using System;

namespace NeonTube
{
    public class BotConfiguration
    {
        public string BotToken { get; set; } = Environment.GetEnvironmentVariable("BotToken") ?? throw new Exception("BotToken Environment Variable Not Found!");
    }
}
