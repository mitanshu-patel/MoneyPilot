using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MoneyPilot.Shared.Helpers;
using System.IO;

namespace MoneyPilot.Infrastructure
{
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<MoneyPilotContext>
    {
        public MoneyPilotContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MoneyPilotContext>();
            var connectionString = ConnectionStringsHelper.GetDbConnectionString();

            // Explicitly set the migrations assembly
            optionsBuilder.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly("MoneyPilot.Infrastructure")
            );

            return new MoneyPilotContext(optionsBuilder.Options);
        }
    }
}
