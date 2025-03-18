using Character.Validation;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Schedule.Application.Extensions;
using Schedule.Infrastructure.Persistence.Background;
using Schedule.Infrastructure.Persistence.Extensions;
using Schedule.Presentation.Grpc.Extensions;
using Schedule.Presentation.Http.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddOptions<JsonSerializerSettings>();
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JsonSerializerSettings>>().Value);

builder.Services.AddApplication();
builder.Services.AddInfrastructurePersistence();
builder.Services.AddPresentationGrpc();
builder.Services.AddHostedService<MigrationBackgroundService>();

builder.Services.AddGrpcClient<UserGrpcService.UserGrpcServiceClient>((_, o) =>
{
    o.Address = new Uri("http://localhost:5000");
});

builder.Services
    .AddControllers()
    .AddNewtonsoftJson()
    .AddPresentationHttp();

builder.Services.AddSwaggerGen().AddEndpointsApiExplorer();

WebApplication app = builder.Build();

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();

app.UsePresentationGrpc();
app.MapControllers();

await app.RunAsync();