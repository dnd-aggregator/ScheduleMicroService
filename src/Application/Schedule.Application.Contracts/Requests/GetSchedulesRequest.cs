namespace Schedule.Application.Contracts.Requests;

public record GetSchedulesRequest(
    long[]? ScheduleIds = null,
    string? Location = null,
    DateOnly? Date = null,
    long Cursor = 0,
    int PageSize = int.MaxValue);