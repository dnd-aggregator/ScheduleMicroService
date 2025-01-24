using Schedule.Application.Models;

namespace Schedule.Application.Abstractions.Persistence.Dbo;

public record ScheduleDbo(
    long MasterId,
    string Location,
    DateOnly Date,
    ScheduleStatus Status);