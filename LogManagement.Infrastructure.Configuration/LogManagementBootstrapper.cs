using LogManagement.Application;
using LogManagement.Application.Contracts.Log;
using LogManagement.Domain.LogAgg;
using LogManagement.Infrastructure.EFCore;
using LogManagement.Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LogManagement.Infrastructure.Configuration
{
    public class LogManagementBootstrapper
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddTransient<ILogApplication, LogApplication>();
            services.AddTransient<ILogRepository, LogRepository>();

            services.AddDbContext<LogContext>(x => x.UseSqlServer(connectionString));
        }
    }
}
