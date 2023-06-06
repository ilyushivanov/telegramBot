using Telegram.Bot.Types;

namespace TelegramBot.Application.Handlers.Abstractions
{
    public interface ICalculateIsolationDataCommandHandler
    {
        Task HandleAsync(Message message, CancellationToken cancellationToken);
    }
}
