using SourceKit.Generators.Builder.Annotations;

namespace Schedule.Application.Abstractions.Persistence.Queries;

[GenerateBuilder]
public partial record ScheduleQuery(long[] ScheduleIds, [RequiredValue] int PageSize, long Cursor);