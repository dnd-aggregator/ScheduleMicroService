namespace Schedule.Application.Contracts.Requests;

public record AddPlayerRequest(
    long ScheduleId,
    long UserId,
    long CharacterId);