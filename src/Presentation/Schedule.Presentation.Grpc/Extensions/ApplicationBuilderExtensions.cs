using Microsoft.AspNetCore.Builder;
using Schedule.Presentation.Grpc.Controllers;

namespace Schedule.Presentation.Grpc.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UsePresentationGrpc(this IApplicationBuilder builder)
    {
        builder.UseEndpoints(routeBuilder =>
        {
            // TODO: add gRPC services implementation
            routeBuilder.MapGrpcService<ScheduleController>();
            routeBuilder.MapGrpcReflectionService();
        });

        return builder;
    }
}