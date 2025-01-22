using Itmo.Dev.Platform.Persistence.Abstractions.Extensions;
using Itmo.Dev.Platform.Persistence.Postgres.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Schedule.Application.Abstractions.Persistence;
using Schedule.Application.Abstractions.Persistence.Repositories;
using Schedule.Infrastructure.Persistence.Plugins;
using Schedule.Infrastructure.Persistence.Repositories;

namespace Schedule.Infrastructure.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructurePersistence(this IServiceCollection collection)
    {
        collection.AddPlatformPersistence(persistence => persistence
            .UsePostgres(postgres => postgres
                .WithConnectionOptions(b => b.BindConfiguration("Infrastructure:Persistence:Postgres"))
                .WithMigrationsFrom(typeof(IAssemblyMarker).Assembly)
                .WithDataSourcePlugin<MappingPlugin>()));

        // TODO: add repositories
        collection.AddScoped<IScheduleRepository, ScheduleRepository>();

        collection.AddScoped<IPersistenceContext, PersistenceContext>();

        return collection;
    }
}