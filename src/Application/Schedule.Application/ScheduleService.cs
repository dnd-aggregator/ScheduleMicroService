using Schedule.Application.Abstractions.Persistence;
using Schedule.Application.Abstractions.Persistence.Dbo;
using Schedule.Application.Contracts;

namespace Schedule.Application;

public class ScheduleService : IScheduleService
{
    private readonly IPersistenceContext _context;

    // private readonly IPersistenceTransactionProvider _transactionProvider;
    public ScheduleService(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<long> CreateAsync(CreateScheduleRequest request, CancellationToken cancellationToken)
    {
        var scheduleDbo = new ScheduleDbo(
            request.Location);

        return await _context.Schedules.AddAsync(scheduleDbo, cancellationToken);
    }
}