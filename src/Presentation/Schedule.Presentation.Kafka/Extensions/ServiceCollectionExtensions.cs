using Dnd;
using Itmo.Dev.Platform.Kafka.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Schedule.Presentation.Kafka.ConsumerHandlers;

namespace Schedule.Presentation.Kafka.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationKafka(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        const string consumerKey = "Presentation:Kafka:Consumers";

        const string producerKey = "Presentation:Kafka:Producers";

        collection.AddPlatformKafka(builder => builder
            .ConfigureOptions(configuration.GetSection("Presentation:Kafka"))
            .AddConsumer(b => b
                .WithKey<GameStatusKey>()
                .WithValue<GameStatusValue>()
                .WithConfiguration(configuration.GetSection($"{consumerKey}:GameStatus"))
                .DeserializeKeyWithProto()
                .DeserializeValueWithProto()
                .HandleWith<ScheduleStatusConsumerHandler>())
            .AddProducer(b => b
                .WithKey<GameScheduleKey>()
                .WithValue<GameScheduleValue>()
                .WithConfiguration(configuration.GetSection($"{producerKey}:ScheduleGame"))
                .SerializeKeyWithProto()
                .SerializeValueWithProto()));

        return collection;
    }
}