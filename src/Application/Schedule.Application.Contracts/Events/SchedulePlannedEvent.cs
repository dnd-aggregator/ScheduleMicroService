using Itmo.Dev.Platform.Events;

namespace Schedule.Application.Contracts.Events;

public record SchedulePlannedEvent(
    long ScheduleId) : IEvent;