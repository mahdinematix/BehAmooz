using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyManagement.Domain.ClassAgg;

namespace StudyManagement.Infrastructure.EFCore.Mapping
{
    public class ClassTemplateMapping: IEntityTypeConfiguration<ClassTemplate>
    {
        public void Configure(EntityTypeBuilder<ClassTemplate> builder)
        {
            builder.ToTable("ClassTemplates");
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Classes).WithOne(x => x.Template).HasForeignKey(x => x.ClassTemplateId);

            builder.HasOne(x => x.Course)
                .WithMany(x=>x.ClassTemplates)
                .HasForeignKey(x => x.CourseId);

            builder.HasMany(x => x.Sessions)
                .WithOne(x=>x.ClassTemplate)
                .HasForeignKey(x => x.ClassTemplateId);

            builder.HasIndex(x => new { x.CourseId, x.ProfessorId })
                .IsUnique();
        }
    }
}
