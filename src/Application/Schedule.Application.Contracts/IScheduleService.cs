using Schedule.Application.Models;

namespace Schedule.Application.Contracts;

public interface IScheduleService
{
    Task<long> CreateAsync(CreateScheduleRequest request, CancellationToken cancellationToken);

    Task<ScheduleModel> GetByIdAsync(long id, CancellationToken cancellationToken);
}