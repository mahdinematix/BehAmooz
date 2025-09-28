using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyManagement.Domain.SessionAgg;

namespace StudyManagement.Infrastructure.EFCore.Mapping
{
    public class SessionMapping : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("Sessions");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).HasMaxLength(200);
            builder.Property(x => x.Video).HasMaxLength(1000);
            builder.Property(x => x.Booklet).HasMaxLength(1000);
            builder.Property(x => x.Description).HasMaxLength(5000);

            builder.HasOne(x => x.Class).WithMany(x => x.Sessions).HasForeignKey(x => x.ClassId);

            builder.HasMany(x => x.SessionPictures).WithOne(x => x.Session).HasForeignKey(x => x.SessionId);
        }
    }
}
