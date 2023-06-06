using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Application.Handlers;
using TelegramBot.Application.Handlers.Abstractions;

namespace TelegramBot.Application
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<ICalculateIsolationDataCommandHandler, CalculateIsolationDataCommandHandler>();
            return services;
        }
    }
}
