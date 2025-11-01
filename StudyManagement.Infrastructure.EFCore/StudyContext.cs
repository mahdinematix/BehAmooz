using Microsoft.EntityFrameworkCore;
using StudyManagement.Domain.ClassAgg;
using StudyManagement.Domain.CourseAgg;
using StudyManagement.Domain.OrderAgg;
using StudyManagement.Domain.SessionAgg;
using StudyManagement.Domain.SessionPictureAgg;
using StudyManagement.Infrastructure.EFCore.Mapping;

namespace StudyManagement.Infrastructure.EFCore
{
    public class StudyContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionPicture> SessionPictures { get; set; }
        public DbSet<Order> Orders { get; set; }

        public StudyContext(DbContextOptions<StudyContext> options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(CourseMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
