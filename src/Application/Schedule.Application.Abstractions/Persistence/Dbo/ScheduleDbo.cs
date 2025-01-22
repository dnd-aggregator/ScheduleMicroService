namespace Schedule.Application.Abstractions.Persistence.Dbo;

public record ScheduleDbo(
    string Location,
    DateOnly Date);