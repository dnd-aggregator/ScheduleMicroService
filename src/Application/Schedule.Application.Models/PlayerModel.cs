namespace Schedule.Application.Models;

public record PlayerModel(
    long ScheduleId,
    long UserId,
    long CharacterId);