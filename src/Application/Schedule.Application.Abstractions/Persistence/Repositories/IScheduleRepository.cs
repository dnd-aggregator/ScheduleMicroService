using Schedule.Application.Abstractions.Persistence.Dbo;
using Schedule.Application.Abstractions.Persistence.Queries;
using Schedule.Application.Models;

namespace Schedule.Application.Abstractions.Persistence.Repositories;

public interface IScheduleRepository
{
    IAsyncEnumerable<ScheduleModel> QueryAsync(ScheduleQuery query, CancellationToken cancellationToken);

    Task<long> AddAsync(ScheduleDbo schedule, CancellationToken cancellationToken);
}