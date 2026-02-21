using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyManagement.Domain.SessionVideoViewAgg;

namespace StudyManagement.Infrastructure.EFCore.Mapping
{
    public class SessionVideoViewMapping : IEntityTypeConfiguration<SessionVideoView>
    {
        public void Configure(EntityTypeBuilder<SessionVideoView> builder)
        {
            builder.ToTable("SessionVideoViews");
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.AccountId, x.SessionId }).IsUnique();
        }
    }
}
