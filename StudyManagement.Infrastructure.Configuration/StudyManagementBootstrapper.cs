using _02_Query.Contracts;
using _02_Query.Contracts.Class;
using _02_Query.Contracts.Course;
using _02_Query.Contracts.Customer;
using _02_Query.Contracts.Message;
using _02_Query.Contracts.Order;
using _02_Query.Contracts.Session;
using _02_Query.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudyManagement.Application;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Course;
using StudyManagement.Application.Contracts.Order;
using StudyManagement.Application.Contracts.Session;
using StudyManagement.Application.Contracts.SessionPicture;
using StudyManagement.Domain.ClassAgg;
using StudyManagement.Domain.CourseAgg;
using StudyManagement.Domain.OrderAgg;
using StudyManagement.Domain.SessionAgg;
using StudyManagement.Domain.SessionPictureAgg;
using StudyManagement.Infrastructure.EFCore;
using StudyManagement.Infrastructure.EFCore.Repository;

namespace StudyManagement.Infrastructure.Configuration
{
    public class StudyManagementBootstrapper
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddTransient<ICourseApplication, CourseApplication>();
            services.AddTransient<ICourseRepository, CourseRepository>();

            services.AddTransient<IClassApplication, ClassApplication>();
            services.AddTransient<IClassRepository, ClassRepository>();

            services.AddTransient<ISessionApplication, SessionApplication>();
            services.AddTransient<ISessionRepository, SessionRepository>();

            services.AddTransient<ISessionPictureApplication, SessionPictureApplication>();
            services.AddTransient<ISessionPictureRepository, SessionPictureRepository>();

            services.AddTransient<IMessageQuery, MessageQuery>();
            services.AddTransient<ICourseQuery, CourseQuery>();
            services.AddTransient<IClassQuery, ClassQuery>();
            services.AddTransient<ISessionQuery, SessionQuery>();
            services.AddTransient<IOrderItemQuery, OrderItemQuery>();
            services.AddTransient<ICustomerQuery, CustomerQuery>();



            services.AddTransient<ICartCalculatorService, CartCalculatorService>();
            services.AddSingleton<ICartService, CartService>();
            services.AddTransient<IOrderApplication, OrderApplication>();
            services.AddTransient<IOrderRepository, OrderRepository>();


            services.AddDbContext<StudyContext>(x => x.UseSqlServer(connectionString));
        }
    }
}
