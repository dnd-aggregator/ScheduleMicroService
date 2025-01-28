using Itmo.Dev.Platform.Events;

namespace Schedule.Application.Contracts.Events;

public record ScheduleGameEvent(long ScheduleId, long[] CharacterIds) : IEvent;