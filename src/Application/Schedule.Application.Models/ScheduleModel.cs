namespace Schedule.Application.Models;

public record ScheduleModel(
    long Id,
    string Location,
    DateOnly Date);