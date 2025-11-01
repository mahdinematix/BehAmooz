using AccountManagement.Domain.WalletAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountManagement.Infrastructure.EFCore.Mapping
{
    public class WalletTransactionMapping : IEntityTypeConfiguration<WalletTransaction>
    {
        public void Configure(EntityTypeBuilder<WalletTransaction> builder)
        {
            builder.ToTable("WalletTransactions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Description).HasMaxLength(500).IsRequired();
            builder.Property(x => x.CreditCardNo).HasMaxLength(30).IsRequired();

            builder.HasOne(x => x.Wallet).WithMany(x => x.Transactions).HasForeignKey(x => x.WalletId);
        }
    }
}
