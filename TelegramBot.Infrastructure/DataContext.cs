using Microsoft.EntityFrameworkCore;
using TelegramBot.Application.Entities;
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
            modelBuilder.ApplyConfiguration(new SessionConfiguration());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            var entities = ChangeTracker
                .Entries()
                .Where(x => x.State is EntityState.Added);

            foreach (var entity in entities)
            {
                var baseEntity = (BaseEntity) entity.Entity;
                baseEntity.CreatedOn = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
