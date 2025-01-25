using Microsoft.Extensions.DependencyInjection;
using Schedule.Application.Contracts;
using Schedule.Presentation.Grpc.Clients;

namespace Schedule.Presentation.Grpc.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationGrpc(this IServiceCollection collection)
    {
        collection.AddScoped<IUsersClient, UserGrpcClient>();
        collection.AddGrpc();
        collection.AddGrpcReflection();

        return collection;
    }
}