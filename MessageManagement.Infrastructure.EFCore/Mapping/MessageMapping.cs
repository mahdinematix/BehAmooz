using MessageManagement.Domain.MessageAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessageManagement.Infrastructure.EFCore.Mapping
{
    public class MessageMapping : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Body).HasMaxLength(5000).IsRequired();
            builder.Property(x => x.MessageFor).HasMaxLength(20).IsRequired();
        }
    }
}
