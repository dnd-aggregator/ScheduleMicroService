using Schedule.Application.Contracts.Requests;
using Schedule.Application.Models;

namespace Schedule.Application.Contracts;

public interface IScheduleService
{
    Task<long> CreateAsync(CreateScheduleRequest request, CancellationToken cancellationToken);

    Task<ScheduleModel?> GetByIdAsync(long id, CancellationToken cancellationToken);

    IAsyncEnumerable<ScheduleModel> GetSchedulesAsync(
        GetSchedulesRequest request,
        CancellationToken cancellationToken);

    Task<PatchScheduleStatusResponse> PatchStatusAsync(long id, ScheduleStatus status, CancellationToken cancellationToken);
}