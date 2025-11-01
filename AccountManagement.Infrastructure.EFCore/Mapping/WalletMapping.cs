using AccountManagement.Domain.WalletAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountManagement.Infrastructure.EFCore.Mapping
{
    public class WalletMapping : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallets");
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Transactions).WithOne(x => x.Wallet).HasForeignKey(x => x.WalletId);

        }
    }
}
