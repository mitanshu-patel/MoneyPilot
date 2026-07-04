using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Infrastructure.EntityConfigurations
{
    public class InvestmentEntityConfiguration : IEntityTypeConfiguration<Investment>
    {
        public void Configure(EntityTypeBuilder<Investment> builder)
        {
            builder.HasKey(v => v.Id);
            builder.Property(v => v.Category).IsRequired();
            builder.HasIndex(v => v.Category).IsUnique();
            builder.HasOne(i => i.Transaction)
                .WithMany(a => a.Investments)
                .HasForeignKey(i => i.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
