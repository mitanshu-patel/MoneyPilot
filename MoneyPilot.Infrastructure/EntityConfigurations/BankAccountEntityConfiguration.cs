using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Infrastructure.EntityConfigurations
{
    public class BankAccountEntityConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            builder.HasKey(v => v.Id);
            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(v => v.HolderName).IsRequired();
            builder.Property(v => v.HolderName).HasMaxLength(100);
            builder.Property(v => v.Balance).IsRequired().HasColumnType("decimal(18,2)");
        }
    }
}
