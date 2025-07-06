using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PojistovnaFullAspPrzeczek.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<ApplicationDbContextFactory>() // + user-secrets
                .AddEnvironmentVariables()                     // + Railway/Production env
                .Build();

            var provider = config["DatabaseProvider"] ?? "PostgreSql";
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            switch (provider)
            {
                case "PostgreSql":
                    optionsBuilder.UseNpgsql(config.GetConnectionString("PostgreSqlConnection")
                        ?? config.GetConnectionString("DefaultConnection"));
                    break;
                case "Sqlite":
                    optionsBuilder.UseSqlite(config.GetConnectionString("SqliteConnection"));
                    break;
                case "SqlServer":
                    optionsBuilder.UseSqlServer(config.GetConnectionString("SqlServerConnection"));
                    break;
                default:
                    throw new Exception("Unknown database provider: " + provider);
            }

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
