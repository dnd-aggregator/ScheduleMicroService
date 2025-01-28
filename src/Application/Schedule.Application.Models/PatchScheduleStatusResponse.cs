namespace Schedule.Application.Models;

public abstract record PatchScheduleStatusResponse
{
    public sealed record SuccessResponse() : PatchScheduleStatusResponse;

    public sealed record ScheduleNotFoundResponse() : PatchScheduleStatusResponse;

    public sealed record NotEnoughPlayersResponse() : PatchScheduleStatusResponse;
}