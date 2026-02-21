using _01_Framework.Infrastructure;
using LogManagement.Application;
using LogManagement.Application.Contracts.LogContracts;
using LogManagement.Domain.LogAgg;
using LogManagement.Infrastructure.Configuration.Permissions;
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

            services.AddTransient<IPermissionExposer, ActivityLogPermissionExposer>();

            services.AddDbContext<LogContext>(x => x.UseSqlServer(connectionString));
        }
    }
}
