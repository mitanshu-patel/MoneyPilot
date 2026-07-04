using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Infrastructure.EntityConfigurations
{
    public class ExpenseCategoryEntityConfiguration : IEntityTypeConfiguration<ExpenseCategory>
    {
        public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            builder.HasKey(v => v.Id);
            builder.Property(ic => ic.Category)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(v => v.Category).IsUnique();
        }
    }
}
