using Microsoft.Extensions.Configuration;

namespace MoneyPilot.Shared.Helpers
{
    public static class ConnectionStringsHelper
    {
        public static string GetDbConnectionString()
        {
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

            // Azure App Service sets connection strings as SQLCONNSTR_<name>
            var azureConnStr = configuration.GetValue<string>("SQLCONNSTR_SystemDbConnectionString");
            if (!string.IsNullOrEmpty(azureConnStr))
                return azureConnStr;

            var connectionString = configuration.GetValue<string>("ConnectionStrings:SystemDbConnectionString");
            if (!string.IsNullOrEmpty(connectionString))
                return connectionString;

            var serverName = configuration.GetValue<string>("DB_SERVER_NAME") ?? "localhost";
            var dbName = configuration.GetValue<string>("DB_NAME") ?? "moneypilot";
            var dbUser = configuration.GetValue<string>("DB_USER") ?? "sa";
            var dbPassword = configuration.GetValue<string>("DB_PASSWORD") ?? "admin123";
            var defaultConnectionString = $"Server={serverName};Database={dbName};User Id={dbUser};Password={dbPassword};Encrypt=False;TrustServerCertificate=True";
            return defaultConnectionString;
        }
    }
}
