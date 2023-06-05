using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TelegramBot.Infrastructure
{
    public static class InfrastructureServicesExtensions
    {

        public static IServiceCollection AddDatabase(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Database"),
                            o => o.MigrationsAssembly("TelegramBot.Infrastructure"));
            });
            return services;
        }
    }
}
