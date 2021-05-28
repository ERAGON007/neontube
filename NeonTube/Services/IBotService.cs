using Telegram.Bot;

namespace NeonTube.Services
{
    public interface IBotService
    {
        TelegramBotClient Client { get; }
    }
}