using Npgsql;

namespace Infrastructure;

public record DbConfiguration
{
    public const string SectionName = "Database";
    public string Host { get; init; } = null!;
    public string ReadHost { get; init; } = null!;
    public int Port { get; init; }
    public string Name { get; init; } = null!;
    public string User { get; init; } = null!;
    public string Password { get; init; } = null!;
    public int MaxRetryCount { get; init; }
    public int MaxRetryTime { get; init; }

    public static void ThrowIfInvalid(DbConfiguration? configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrEmpty(configuration.Host);
        if (configuration.Port <= 0)
        {
            throw new ArgumentException("Port must be greater than 0");
        }

        ArgumentException.ThrowIfNullOrEmpty(configuration.Name);
        ArgumentException.ThrowIfNullOrEmpty(configuration.User);
    }

    public static string GetConnectionString(DbConfiguration options, bool isReadOnly = false)
    {
        var csBuilder = isReadOnly
            ? new NpgsqlConnectionStringBuilder
            {
                Host = options.ReadHost,
                Database = options.Name,
                Username = options.User,
                Port = options.Port,
                Password = options.Password,
                IncludeErrorDetail = true
            }
            : new NpgsqlConnectionStringBuilder
            {
                Host = options.Host,
                Database = options.Name,
                Username = options.User,
                Port = options.Port,
                Password = options.Password,
                IncludeErrorDetail = true
            };

        return csBuilder.ConnectionString;
    }
}