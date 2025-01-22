using SourceKit.Generators.Builder.Annotations;

namespace Schedule.Application.Abstractions.Persistence.Queries;

[GenerateBuilder]
public partial record ScheduleQuery(
    long[] ScheduleIds,
    string? Location,
    DateOnly? Date,
    [RequiredValue] int PageSize,
    long Cursor);