using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyManagement.Domain.ClassAgg;

namespace StudyManagement.Infrastructure.EFCore.Mapping
{
    public class ClassMapping : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.ToTable("Classes");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code).HasMaxLength(20).IsRequired();
            builder.Property(x => x.StartTime).HasMaxLength(20).IsRequired();
            builder.Property(x => x.EndTime).HasMaxLength(20).IsRequired();

            builder.HasOne(x => x.Course).WithMany(x => x.Classes).HasForeignKey(x => x.CourseId);

            builder.HasMany(x => x.Sessions).WithOne(x => x.Class).HasForeignKey(x => x.ClassId);

        }
    }
}
