using MessageManagement.Domain.MessageAgg;
using MessageManagement.Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace MessageManagement.Infrastructure.EFCore
{
    public class MessageContext: DbContext
    {
        public DbSet<Message> Messages { get; set; }
        public MessageContext(DbContextOptions<MessageContext> options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(MessageMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
