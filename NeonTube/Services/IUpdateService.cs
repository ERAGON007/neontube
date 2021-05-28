using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace NeonTube.Services
{
    public interface IUpdateService
    {
        Task HandleUpdate(Update update);
    }
}
