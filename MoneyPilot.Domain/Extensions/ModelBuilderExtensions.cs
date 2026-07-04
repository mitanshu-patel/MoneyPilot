using Microsoft.EntityFrameworkCore;
using MoneyPilot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Domain.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seeding(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExpenseCategory>().HasData(
                new ExpenseCategory { Id = 1, Category = "Food" },
                new ExpenseCategory { Id = 2, Category = "Transportation" },
                new ExpenseCategory { Id = 3, Category = "Entertainment" },
                new ExpenseCategory { Id = 4, Category = "Rent" },
                new ExpenseCategory { Id = 5, Category = "EMI" },
                new ExpenseCategory { Id = 6, Category = "Bills(Internet/Electricity/Gas/Other)" }
            );

            modelBuilder.Entity<InvestmentCategory>().HasData(
                new InvestmentCategory { Id = 1, Category = "Stocks" },
                new InvestmentCategory { Id = 2, Category = "Mutual Funds/SIPs" },
                new InvestmentCategory { Id = 3, Category = "Real Estate" },
                new InvestmentCategory { Id = 4, Category = "Gold" }
            );
        }
    }
}
