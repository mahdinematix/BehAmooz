using LogManagement.Domain.LogAgg;
using LogManagement.Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace LogManagement.Infrastructure.EFCore
{
    public class LogContext : DbContext
    {
        
        public DbSet<Log> Logs { get; set; }

        public LogContext(DbContextOptions<LogContext> options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LogMapping());
            base.OnModelCreating(modelBuilder);
        }
    }
}
