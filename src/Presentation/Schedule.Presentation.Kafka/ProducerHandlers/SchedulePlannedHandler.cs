using Itmo.Dev.Platform.Events;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;
using Schedule.Application.Contracts.Events;
using Schedule.Kafka.Contracts;

namespace Schedule.Presentation.Kafka.ProducerHandlers;

public class SchedulePlannedHandler : IEventHandler<SchedulePlannedEvent>
{
    private readonly IKafkaMessageProducer<ScheduleCreationKey, ScheduleCreationValue> _producer;

    public SchedulePlannedHandler(IKafkaMessageProducer<ScheduleCreationKey, ScheduleCreationValue> producer)
    {
        _producer = producer;
    }

    public async ValueTask HandleAsync(SchedulePlannedEvent evt, CancellationToken cancellationToken)
    {
        var key = new ScheduleCreationKey() { ScheduleId = evt.ScheduleId };

        var value = new ScheduleCreationValue()
        {
            SchedulePlanned = new ScheduleCreationValue.Types.SchedulePlanned()
            {
                ScheduleId = evt.ScheduleId,
            },
        };

        var message = new KafkaProducerMessage<ScheduleCreationKey, ScheduleCreationValue>(key, value);

        await _producer.ProduceAsync(message, cancellationToken);
    }
}