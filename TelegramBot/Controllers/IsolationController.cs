using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application;
using TelegramBot.Application.Handlers;
using TelegramBot.Application.Handlers.Abstractions; 

namespace TelegramBot.Api.Controllers
{
    [Route("api/bot")]
    public class IsolationController : Controller
    {
        private readonly Dictionary<string, ICommandHandler> _commandHandlers;

        public IsolationController(
            ITelegramBotClient telegramBotClient,
            IDataContextProvider dataContextProvider)
        {
            _commandHandlers = new Dictionary<string, ICommandHandler>
            {
                { "start", new StartCommandHandler(telegramBotClient) },
                { "isolation", new GetIsolationDataCommandHandler(telegramBotClient, dataContextProvider) }
            };
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
        {
            if (update == null) return Ok();

            var message = update.Message;

            if (message.Text.Contains("/start"))
            {
                await _commandHandlers["start"].HandleAsync(message!, cancellationToken);
            }
            else
            {
                await _commandHandlers["isolation"].HandleAsync(message!, cancellationToken);
            }

            return Ok();
        }
    }
}
