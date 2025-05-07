using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyManagement.Domain.SessionPictureAgg;

namespace StudyManagement.Infrastructure.EFCore.Mapping
{
    public class SessionPictureMapping: IEntityTypeConfiguration<SessionPicture>
    {
        public void Configure(EntityTypeBuilder<SessionPicture> builder)
        {
            builder.ToTable("SessionPictures");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Picture).HasMaxLength(1000).IsRequired();

            builder.HasOne(x => x.Session).WithMany(x => x.SessionPictures).HasForeignKey(x => x.SessionId);
        }
    }
}
