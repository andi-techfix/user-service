using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public class ApplicationDbContextFactory 
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") 
                  ?? "Production";
        
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{env}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
        
        var dbConfig = config
            .GetSection(DbConfiguration.SectionName)
            .Get<DbConfiguration>();
        DbConfiguration.ThrowIfInvalid(dbConfig);
        
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        optionsBuilder.UseNpgsql(
            DbConfiguration.GetConnectionString(dbConfig!),
            pg => pg.EnableRetryOnFailure(
                dbConfig!.MaxRetryCount,
                TimeSpan.FromSeconds(dbConfig.MaxRetryTime),
                null
            )
        );
        
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}