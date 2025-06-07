using _01_Framework.Infrastructure;
using _02_Query.Contracts.Message;
using _02_Query.Query;
using MessageManagement.Application;
using MessageManagement.Application.Contract.Message;
using MessageManagement.Domain.MessageAgg;
using MessageManagement.Infrastructure.Configuration.Permission;
using MessageManagement.Infrastructure.EFCore;
using MessageManagement.Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MessageManagement.Infrastructure.Configuration
{
    public class MessageManagementBootstrapper
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IMessageApplication, MessageApplication>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IMessageQuery, MessageQuery>();

            services.AddTransient<IPermissionExposer, MessagePermissionExposer>();

            services.AddDbContext<MessageContext>(x => x.UseSqlServer(connectionString));
        }
    }
}