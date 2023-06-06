using Microsoft.EntityFrameworkCore;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Entities;
using TelegramBot.Application.Handlers.Abstractions;

namespace TelegramBot.Application.Handlers
{
    public class CalculateIsolationDataCommandHandler : ICalculateIsolationDataCommandHandler
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly DbSet<Isolation> _isolations;
        private readonly DbSet<Session> _session;
        private readonly IDataContextProvider _dataContextProvider;

        public CalculateIsolationDataCommandHandler(
            ITelegramBotClient telegramBotClient,
            IDataContextProvider dataContextProvider)
        {
            _telegramBotClient = telegramBotClient;
            _isolations = dataContextProvider.Get<Isolation>();
            _session = dataContextProvider.Get<Session>();
            _dataContextProvider = dataContextProvider;
        }

        public async Task HandleAsync(Message message, CancellationToken cancellationToken)
        {
            var session = await _session
                .OrderByDescending(e => e.CreatedOn)
                .FirstOrDefaultAsync(e => e.ChatId == message.Chat.Id && e.CreatedOn.AddMinutes(30) > DateTime.UtcNow, cancellationToken);

            if (session == null ||
                message.Text == "/start" ||
                session.Status == SessionStatus.Calculated)
            {
                session = new Session
                {
                    ChatId = message.Chat.Id
                };

                _session.Add(session);

                await HandleSessionAsync(session, SessionStatus.New, _dataContextProvider, _telegramBotClient, "Приветствую, введите высоту подполья вашего здания от 0,4 м до 1,2 м с шагом 0,2 м.", cancellationToken);
                return;
            }

            decimal parsedParameter;

            try
            {
                parsedParameter = decimal.Parse(message.Text);
            }
            catch
            {
                await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "Введенные данные имеют неверный формат", cancellationToken: cancellationToken);
                return;
            }       

            if (session.Status == SessionStatus.New)
            {
                if (parsedParameter == 0.4m ||
                parsedParameter == 0.6m ||
                parsedParameter == 0.8m ||
                parsedParameter == 1m ||
                parsedParameter == 1.2m)
                {
                    session.PileHeight = parsedParameter;
                    await HandleSessionAsync(session, SessionStatus.PileHeightAccepted, _dataContextProvider, _telegramBotClient, "Введите ширину вашего здания от 12 м до 18 м с шагом в 1 м.", cancellationToken);
                    return;
                }

                await _telegramBotClient.SendTextMessageAsync(session.ChatId, "Введенные данные имеют неверный формат", cancellationToken: cancellationToken);
                return;
            }
            else if (session.Status == SessionStatus.PileHeightAccepted)
            {
                if (parsedParameter < 12 || parsedParameter > 18)
                {
                    await _telegramBotClient.SendTextMessageAsync(session.ChatId, "Введенные данные имеют неверный формат", cancellationToken: cancellationToken);
                    return;
                }

                session.Width = (int) parsedParameter;
                await HandleSessionAsync(session, SessionStatus.WidthAccepted, _dataContextProvider, _telegramBotClient, "Введите длину вашего здания от 12 м до 24 м с шагом в 1 м.", cancellationToken);
                return;
            }

            if (session.Status == SessionStatus.WidthAccepted)
            {
                if (parsedParameter < 12 || parsedParameter > 24)
                {
                    await _telegramBotClient.SendTextMessageAsync(session.ChatId, "Введенные данные имеют неверный формат", cancellationToken: cancellationToken);
                    return;
                }

                session.Length = (int) parsedParameter;

                var isolation = await _isolations.FirstOrDefaultAsync(e => e.Width == session.Width && e.Length == session.Length && e.PileHeight == session.PileHeight, cancellationToken);
                if (isolation == null)
                {
                    await _telegramBotClient.SendTextMessageAsync(session.ChatId, "По введенным параметрам данные не найдены", cancellationToken: cancellationToken);
                    return;
                }

                var result = Calculate(isolation.Width, isolation.Length, isolation.PileHeight, isolation.PlinthThickness);

                session.Status = SessionStatus.Calculated;
                await _dataContextProvider.SaveChangesAsync(cancellationToken);
                await _telegramBotClient.SendTextMessageAsync(message.Chat.Id,
                    $"Для вашего здания необходимое утепление наружного ограждения подполья из сэндвич-панелей с толщиной t={isolation.PanelThickness} мм с площадью S={result.Item1} м2\nДля цокольного перекрытия из ПСБ-С М35 с толщиной t={isolation.PlinthThickness} мм в объёме V={result.Item2} м3", cancellationToken: cancellationToken);
                return;
            }

            session = new Session
            {
                ChatId = message.Chat.Id
            };

            _session.Add(session);

            await HandleSessionAsync(session, SessionStatus.New, _dataContextProvider, _telegramBotClient, "Приветствую, введите высоту подполья вашего здания от 0,4 м до 1,2 м с шагом 0,2 м.", cancellationToken);
        }

        private async Task HandleSessionAsync(
            Session session,
            SessionStatus status,
            IDataContextProvider dataContextProvider,
            ITelegramBotClient telegramBotClient,
            string text,
            CancellationToken cancellationToken)
        {
            session.Status = status;
            await dataContextProvider.SaveChangesAsync(cancellationToken);
            await telegramBotClient.SendTextMessageAsync(session.ChatId, text, cancellationToken: cancellationToken);
        }

        private (decimal, decimal) Calculate(int width, int length, decimal pileHeight, decimal plinthThickness)
        {
            var square = (width + length) * 2 * pileHeight;
            var volume = width * length * (plinthThickness / 1000);
            return (square, volume);
        }
    }
}
