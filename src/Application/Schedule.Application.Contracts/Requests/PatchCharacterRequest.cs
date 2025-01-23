namespace Schedule.Application.Contracts.Requests;

public record PatchCharacterRequest(
    long ScheduleId,
    long UserId,
    long CharacterId);