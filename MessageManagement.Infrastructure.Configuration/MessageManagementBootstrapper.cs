using _02_Query.Contracts.Message;
using _02_Query.Query;
using MessageManagement.Application;
using MessageManagement.Application.Contract.Message;
using MessageManagement.Domain.MessageAgg;
using MessageManagement.Infrastructure.EFCore;
using MessageManagement.Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MessageManagement.Infrastructure.Configuration
{
    public class MessageManagementBootstrapper
    {
        public static void Configuration(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IMessageApplication, MessageApplication>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IMessageQuery, MessageQuery>();

            services.AddDbContext<MessageContext>(x => x.UseSqlServer(connectionString));
        }
    }
}