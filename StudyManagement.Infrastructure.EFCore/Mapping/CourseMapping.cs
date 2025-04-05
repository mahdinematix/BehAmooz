using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyManagement.Domain.CourseAgg;

namespace StudyManagement.Infrastructure.EFCore.Mapping
{
    public class CourseMapping : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(300).IsRequired();
            builder.Property(x => x.Code).HasMaxLength(10).IsRequired();
            builder.Property(x => x.CourseKind).HasMaxLength(10).IsRequired();
            builder.Property(x => x.NumberOfUnit).IsRequired();

            builder.HasMany(x => x.Classes).WithOne(x => x.Course).HasForeignKey(x => x.CourseId);


        }
    }
}
