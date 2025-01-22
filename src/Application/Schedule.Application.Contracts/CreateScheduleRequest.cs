namespace Schedule.Application.Contracts;

public record CreateScheduleRequest(string Location, DateOnly Date);