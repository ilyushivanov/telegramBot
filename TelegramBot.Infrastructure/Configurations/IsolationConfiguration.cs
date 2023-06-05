using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TelegramBot.Application.Entities;

namespace TelegramBot.Infrastructure.Configurations
{
    public class IsolationConfiguration : IEntityTypeConfiguration<Isolation>
    {
        public void Configure(EntityTypeBuilder<Isolation> builder)
        {
        }
    }
}
