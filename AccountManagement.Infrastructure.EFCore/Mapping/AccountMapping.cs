using AccountManagement.Domain.AccountAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountManagement.Infrastructure.EFCore.Mapping
{
    public class AccountMapping : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName).HasMaxLength(150).IsRequired();
            builder.Property(x => x.LastName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.Code).HasMaxLength(20).IsRequired();
            builder.Property(x => x.NationalCode).HasMaxLength(10).IsRequired();
            builder.Property(x => x.NationalCardPicture).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(300).IsRequired();
            builder.Property(x => x.PhoneNumber).HasMaxLength(14).IsRequired();


            builder.HasOne(x => x.Role).WithMany(x => x.Accounts).HasForeignKey(x => x.RoleId);

        }
    }
}
