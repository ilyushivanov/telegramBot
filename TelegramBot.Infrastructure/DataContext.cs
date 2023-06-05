using Microsoft.EntityFrameworkCore;
using TelegramBot.Infrastructure.Configurations;

namespace TelegramBot.Infrastructure
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new IsolationConfiguration());
        }
    }
}
