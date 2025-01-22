namespace Schedule.Application.Contracts;

public record GetSchedulesRequest(
    long[]? ScheduleIds = null,
    string? Location = null,
    DateOnly? Date = null,
    long Cursor = 0,
    int PageSize = int.MaxValue);