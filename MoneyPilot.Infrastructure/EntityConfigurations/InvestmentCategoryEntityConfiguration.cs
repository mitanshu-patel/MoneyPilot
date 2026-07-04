using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Infrastructure.EntityConfigurations
{
    public class InvestmentCategoryEntityConfiguration : IEntityTypeConfiguration<InvestmentCategory>
    {
        public void Configure(EntityTypeBuilder<InvestmentCategory> builder)
        {
            builder.HasKey(ic => ic.Id);
            builder.Property(ic => ic.Category)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
