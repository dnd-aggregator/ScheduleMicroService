using Itmo.Dev.Platform.Events;

namespace Schedule.Application.Contracts.Events;

public record ScheduleCreatedEvent(
    long ScheduleId) : IEvent;