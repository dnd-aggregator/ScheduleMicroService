namespace Schedule.Application.Abstractions.Persistence.Dbo;

public record PlayerDbo(
    long ScheduleId,
    long UserId,
    long CharacterId);