using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyManagement.Domain.SemesterAgg;

namespace StudyManagement.Infrastructure.EFCore.Mapping
{
    public class SemesterMapping: IEntityTypeConfiguration<Semester>
    {
        public void Configure(EntityTypeBuilder<Semester> builder)
        {
            builder.ToTable("Semesters");
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Courses).WithOne(x => x.Semester).HasForeignKey(x => x.SemesterId);

            builder.HasOne(x => x.University).WithMany(x => x.Semesters).HasForeignKey(x => x.UniversityId);
        }
    }
}
