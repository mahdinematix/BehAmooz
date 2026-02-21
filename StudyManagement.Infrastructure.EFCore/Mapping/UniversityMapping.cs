using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyManagement.Domain.UniversityAgg;

namespace StudyManagement.Infrastructure.EFCore.Mapping
{
    public class UniversityMapping : IEntityTypeConfiguration<University>
    {
        public void Configure(EntityTypeBuilder<University> builder)
        {
            builder.ToTable("Universities");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(150).IsRequired();


            builder.HasMany(x => x.Semesters).WithOne(x => x.University).HasForeignKey(x=>x.UniversityId);

            builder.HasMany(x => x.Courses).WithOne(x => x.University).HasForeignKey(x => x.UniversityId);
        }
    }
}
