#pragma warning disable CA1506

using Character.Validation;
using Itmo.Dev.Platform.Common.Extensions;
using Itmo.Dev.Platform.Observability;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Schedule.Application.Extensions;
using Schedule.Infrastructure.Persistence.Extensions;
using Schedule.Presentation.Grpc.Extensions;
using Schedule.Presentation.Http.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddOptions<JsonSerializerSettings>();
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JsonSerializerSettings>>().Value);

builder.Services.AddPlatform();
builder.AddPlatformObservability();

builder.Services.AddApplication();
builder.Services.AddInfrastructurePersistence();
builder.Services.AddPresentationGrpc();

builder.Services.AddGrpcClient<UserGrpcService.UserGrpcServiceClient>((_, o) =>
{
    o.Address = new Uri("https://localhost:8070");
});

// builder.Services.AddPresentationKafka(builder.Configuration);
builder.Services
    .AddControllers()
    .AddNewtonsoftJson()
    .AddPresentationHttp();

builder.Services.AddSwaggerGen().AddEndpointsApiExplorer();

// builder.Services.AddPlatformEvents(b => b.AddPresentationKafkaHandlers());
builder.Services.AddUtcDateTimeProvider();

WebApplication app = builder.Build();

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();

app.UsePlatformObservability();

app.UsePresentationGrpc();
app.MapControllers();

await app.RunAsync();