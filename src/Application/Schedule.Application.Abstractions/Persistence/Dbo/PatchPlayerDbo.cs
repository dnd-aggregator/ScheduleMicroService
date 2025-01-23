namespace Schedule.Application.Abstractions.Persistence.Dbo;

public record PatchPlayerDbo(
    long ScheduleId,
    long UserId,
    long CharacterId);