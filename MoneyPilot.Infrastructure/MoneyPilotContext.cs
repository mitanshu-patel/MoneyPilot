using Microsoft.EntityFrameworkCore;
using MoneyPilot.Domain.Entities;
using MoneyPilot.Domain.Extensions;

namespace MoneyPilot.Infrastructure
{
    public class MoneyPilotContext : DbContext
    {
        public MoneyPilotContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        public DbSet<Investment> Investments { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }

        public DbSet<InvestmentCategory> InvestmentCategories { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seeding();
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(MoneyPilotContext).Assembly);
        }
    }
}
