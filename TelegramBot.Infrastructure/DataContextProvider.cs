using Microsoft.EntityFrameworkCore;
using TelegramBot.Application;

namespace TelegramBot.Infrastructure
{
    public class DataContextProvider : IDataContextProvider
    {
        private readonly DataContext _dataContext;

        public DataContextProvider(IDbContextFactory<DataContext> dataContextFactory)
        {
            _dataContext = dataContextFactory.CreateDbContext();
        }

        public DbSet<T> Get<T>() where T : class
            => _dataContext.Set<T>();

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
            => _dataContext.SaveChangesAsync(cancellationToken);
    }
}
