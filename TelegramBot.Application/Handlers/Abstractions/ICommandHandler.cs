using Telegram.Bot.Types;

namespace TelegramBot.Application.Handlers.Abstractions
{
    public interface ICommandHandler
    {
        Task HandleAsync(Message message, CancellationToken cancellationToken);
    }
}
