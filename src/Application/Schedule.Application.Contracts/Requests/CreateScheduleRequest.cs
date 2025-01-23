namespace Schedule.Application.Contracts.Requests;

public record CreateScheduleRequest(
    long MasterId,
    string Location,
    DateOnly Date);