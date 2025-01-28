using Dnd;
using Itmo.Dev.Platform.Kafka.Consumer;
using Schedule.Application.Contracts;
using Schedule.Application.Models;

namespace Schedule.Presentation.Kafka.ConsumerHandlers;

public class ScheduleStatusConsumerHandler : IKafkaConsumerHandler<GameStatusKey, GameStatusValue>
{
    private readonly IScheduleService _scheduleService;

    public ScheduleStatusConsumerHandler(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaConsumerMessage<GameStatusKey, GameStatusValue>> messages,
        CancellationToken cancellationToken)
    {
        foreach (IKafkaConsumerMessage<GameStatusKey, GameStatusValue> message in messages)
        {
            if (message.Value.EventCase == GameStatusValue.EventOneofCase.GameStarted)
                await _scheduleService.PatchStatusAsync(message.Key.GameId, ScheduleStatus.Started, cancellationToken);
            else
                await _scheduleService.PatchStatusAsync(message.Key.GameId, ScheduleStatus.Finished, cancellationToken);
        }
    }
}