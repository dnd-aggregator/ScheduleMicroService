using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Schedule.Application.Abstractions.Persistence.Repositories;
using Schedule.Infrastructure.Persistence.Plugins;
using Schedule.Infrastructure.Persistence.Repositories;

namespace Schedule.Infrastructure.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructurePersistence(this IServiceCollection services)
    {
        var connectionString = "Host=localhost;Port=5433;Database=postgres;Username=postgres;Password=postgres;";
    
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

        MappingPlugin.Configure(dataSourceBuilder);
    
        var dataSource = dataSourceBuilder.Build();
        services.AddSingleton(dataSource);
    
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(ServiceCollectionExtensions).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());
    
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();

        return services;
    }
}