using Dnd;
using Itmo.Dev.Platform.Events;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;
using Schedule.Application.Contracts.Events;

namespace Schedule.Presentation.Kafka.ProducerHandlers;

public class ScheduleGameHandler : IEventHandler<ScheduleGameEvent>
{
    private readonly IKafkaMessageProducer<GameScheduleKey, GameScheduleValue> _producer;

    public ScheduleGameHandler(IKafkaMessageProducer<GameScheduleKey, GameScheduleValue> producer)
    {
        _producer = producer;
    }

    public async ValueTask HandleAsync(ScheduleGameEvent evt, CancellationToken cancellationToken)
    {
        var key = new GameScheduleKey() { GameId = evt.ScheduleId };

        var value = new GameScheduleValue()
        {
            GameId = evt.ScheduleId,
            CharacterIds = { evt.CharacterIds },
        };

        var message = new KafkaProducerMessage<GameScheduleKey, GameScheduleValue>(key, value);

        await _producer.ProduceAsync(message, cancellationToken);
    }
}