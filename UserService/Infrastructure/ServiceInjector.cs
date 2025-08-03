using Domain.Repositories;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public static class ServiceInjector
{
    public static async Task AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConfiguration = configuration
            .GetSection(DbConfiguration.SectionName).Get<DbConfiguration>();
        DbConfiguration.ThrowIfInvalid(dbConfiguration);

        services.AddScoped<IUserRepository, UserRepository>();
        await RunDbMigrations(services);
    }

    private static Task RunDbMigrations(IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return dbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database");
            Environment.Exit(1);
            return Task.CompletedTask;
        }
    }
}