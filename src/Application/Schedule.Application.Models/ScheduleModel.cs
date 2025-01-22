namespace Schedule.Application.Models;

public record ScheduleModel(
    long Id,
    DateOnly Date,
    TimeOnly Time);