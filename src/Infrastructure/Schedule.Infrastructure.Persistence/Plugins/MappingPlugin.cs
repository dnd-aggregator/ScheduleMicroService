using Npgsql;
using Schedule.Application.Models;

namespace Schedule.Infrastructure.Persistence.Plugins;

public static class MappingPlugin
{
    public static void Configure(NpgsqlDataSourceBuilder dataSource)
    {
        dataSource.MapEnum<ScheduleStatus>("schedule_status");
    }
}