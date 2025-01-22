namespace Schedule.Application.Contracts;

public interface IScheduleService
{
    Task<long> CreateAsync(CreateScheduleRequest request, CancellationToken cancellationToken);
}