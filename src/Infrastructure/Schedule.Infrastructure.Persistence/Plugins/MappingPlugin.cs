using Itmo.Dev.Platform.Persistence.Postgres.Plugins;
using Npgsql;
using Schedule.Application.Models;

namespace Schedule.Infrastructure.Persistence.Plugins;

/// <summary>
///     Plugin for configuring NpgsqlDataSource's mappings
///     ie: enums, composite types
/// </summary>
public class MappingPlugin : IPostgresDataSourcePlugin
{
    public void Configure(NpgsqlDataSourceBuilder dataSource)
    {
        dataSource.MapEnum<ScheduleStatus>("schedule_status");
    }
}