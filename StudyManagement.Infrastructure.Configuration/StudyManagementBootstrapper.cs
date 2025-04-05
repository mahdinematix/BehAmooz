﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudyManagement.Application;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Course;
using StudyManagement.Domain.ClassAgg;
using StudyManagement.Domain.CourseAgg;
using StudyManagement.Infrastructure.EFCore;
using StudyManagement.Infrastructure.EFCore.Repository;

namespace StudyManagement.Infrastructure.Configuration
{
    public class StudyManagementBootstrapper
    {
        public static void Configuration(IServiceCollection services, string connectionString)
        {
            services.AddTransient<ICourseApplication, CourseApplication>();
            services.AddTransient<ICourseRepository, CourseRepository>();

            services.AddTransient<IClassApplication, ClassApplication>();
            services.AddTransient<IClassRepository, ClassRepository>();


            services.AddDbContext<StudyContext>(x => x.UseSqlServer(connectionString));
        }
    }
}
