using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Entities;
using TelegramBot.Application.Handlers.Abstractions;

namespace TelegramBot.Application.Handlers
{
    public class GetIsolationDataCommandHandler : ICommandHandler
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly DbSet<Isolation> _isolations;

        public GetIsolationDataCommandHandler(
            ITelegramBotClient telegramBotClient,
            IDataContextProvider dataContextProvider)
        {
            _telegramBotClient = telegramBotClient;
            _isolations = dataContextProvider.Get<Isolation>();
        }

        public async Task HandleAsync(Message message, CancellationToken cancellationToken)
        {
            var parameters = message.Text.Split(" ");
            decimal[] parsedParameters = new decimal[parameters.Length];

            try
            {
                for (var i = 0; i < parameters.Length; i++)
                {
                    parsedParameters[i] = decimal.Parse(parameters[i]);
                }
            }
            catch
            {
                await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "Введенные данные имеют неверный формат", cancellationToken: cancellationToken);
                return;
            }       

            if (parsedParameters.Count() != 2)
            {
                await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "Неправильное количество параметров", cancellationToken: cancellationToken);
                return;
            }

            var isolation = await _isolations.FirstOrDefaultAsync(e => e.Width == parsedParameters[0] && e.Length == parsedParameters[1]);
            if (isolation == null)
            {
                await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "По введенным данным ничего не найдено", cancellationToken: cancellationToken);
                return;
            }

            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Высота теплоизоляции: {isolation.PileHeight}", cancellationToken: cancellationToken);
        }
    }
}
