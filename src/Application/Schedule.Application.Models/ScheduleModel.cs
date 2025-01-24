namespace Schedule.Application.Models;

public record ScheduleModel(
    long Id,
    long MasterId,
    string Location,
    DateOnly Date,
    ScheduleStatus Status);