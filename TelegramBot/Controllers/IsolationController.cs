using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramBot.Application.Handlers.Abstractions; 

namespace TelegramBot.Api.Controllers
{
    [Route("api/bot")]
    public class IsolationController : Controller
    {
        private readonly ICalculateIsolationDataCommandHandler _calculateIsolationDataCommandHandler;

        public IsolationController(
            ICalculateIsolationDataCommandHandler calculateIsolationDataCommandHandler)
        {
            _calculateIsolationDataCommandHandler = calculateIsolationDataCommandHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
        {
            if (update == null) return Ok();

            var message = update.Message;

            await _calculateIsolationDataCommandHandler.HandleAsync(message, cancellationToken);

            return Ok();
        }
    }
}
