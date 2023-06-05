using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Application;

namespace TelegramBot.Infrastructure
{
    public static class InfrastructureServicesExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IDataContextProvider, DataContextProvider>();

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddPooledDbContextFactory<DataContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Database"),
                            o => o.MigrationsAssembly("TelegramBot.Infrastructure"));
            });
            return services;
        }
    }
}
