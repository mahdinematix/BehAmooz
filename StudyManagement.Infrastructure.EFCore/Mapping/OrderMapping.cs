﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyManagement.Domain.OrderAgg;

namespace StudyManagement.Infrastructure.EFCore.Mapping
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.IssueTrackingNo).HasMaxLength(8).IsRequired();

            builder.OwnsMany(x => x.Items, navigationBuilder =>
            {
                navigationBuilder.ToTable("OrderItems");
                navigationBuilder.HasKey(x => x.Id);
                navigationBuilder.WithOwner(x => x.Order).HasForeignKey(x=>x.OrderId);
            });

        }
    }
}
