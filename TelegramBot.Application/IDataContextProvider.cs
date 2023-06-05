using Microsoft.EntityFrameworkCore;

namespace TelegramBot.Application
{
    public interface IDataContextProvider
    {
        DbSet<T> Get<T>() where T : class;

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
