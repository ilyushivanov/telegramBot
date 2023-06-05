using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Handlers.Abstractions;

namespace TelegramBot.Application.Handlers
{
    public class StartCommandHandler : ICommandHandler
    {
        private readonly ITelegramBotClient _telegramBotClient;

        public StartCommandHandler(
            ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public async Task HandleAsync(Message message, CancellationToken cancellationToken)
        {
            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "Приветствую, чтобы получить данные введите параметры через пробел в таком формате: 11 22 33", cancellationToken: cancellationToken);
        }
    }
}
