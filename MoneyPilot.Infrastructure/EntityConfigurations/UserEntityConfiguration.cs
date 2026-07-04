using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Infrastructure.EntityConfigurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            builder.HasKey(v => v.Id);
            builder.Property(v => v.Email).IsRequired();
            builder.Property(v => v.Email).HasMaxLength(255);
            builder.HasIndex(v => v.Email).IsUnique();
            builder.Property(v => v.Password).IsRequired();
        }
    }
}
