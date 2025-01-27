using Schedule.Application.Abstractions.Persistence;
using Schedule.Application.Abstractions.Persistence.Dbo;
using Schedule.Application.Abstractions.Persistence.Queries;
using Schedule.Application.Contracts;
using Schedule.Application.Contracts.Requests;
using Schedule.Application.Models;

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
            request.MasterId,
            request.Location,
            request.Date,
            ScheduleStatus.Draft);

        return await _context.Schedules.AddAsync(scheduleDbo, cancellationToken);
    }

    public async Task<ScheduleModel?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var query = ScheduleQuery.Build(builder => builder
            .WithScheduleIds([id])
            .WithPageSize(1));

        return await _context.Schedules
            .QueryAsync(query, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public IAsyncEnumerable<ScheduleModel> GetSchedulesAsync(
        GetSchedulesRequest request,
        CancellationToken cancellationToken)
    {
        var query = ScheduleQuery.Build(builder => builder
            .WithScheduleIds(request.ScheduleIds ?? [])
            .WithLocation(request.Location)
            .WithDate(request.Date)
            .WithPageSize(request.PageSize)
            .WithCursor(request.Cursor));

        return _context.Schedules.QueryAsync(query, cancellationToken);
    }

    public async Task PatchStatusAsync(long id, ScheduleStatus status, CancellationToken cancellationToken)
    {
        await _context.Schedules.PatchStatusAsync(id, status, cancellationToken);
    }
}