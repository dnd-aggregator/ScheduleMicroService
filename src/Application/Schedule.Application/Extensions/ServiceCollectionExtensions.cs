using Microsoft.Extensions.DependencyInjection;
using Schedule.Application.Contracts;

namespace Schedule.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<IScheduleService, ScheduleService>();
        return collection;
    }
}