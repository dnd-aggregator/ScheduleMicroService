using Itmo.Dev.Platform.Events;
using Schedule.Application.Abstractions.Persistence;
using Schedule.Application.Abstractions.Persistence.Dbo;
using Schedule.Application.Abstractions.Persistence.Queries;
using Schedule.Application.Contracts;
using Schedule.Application.Contracts.Events;
using Schedule.Application.Contracts.Requests;
using Schedule.Application.Models;

namespace Schedule.Application;

public class ScheduleService : IScheduleService
{
    private const int PlayerCount = 1;
    private readonly IPersistenceContext _context;
    private readonly IEventPublisher _eventPublisher;
    private readonly IPlayerService _playerService;

    public ScheduleService(IPersistenceContext context, IEventPublisher eventPublisher, IPlayerService playerService)
    {
        _context = context;
        _eventPublisher = eventPublisher;
        _playerService = playerService;
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

    public async Task<PatchScheduleStatusResponse> PatchStatusAsync(long id, ScheduleStatus status, CancellationToken cancellationToken)
    {
        ScheduleModel? schedule = await GetByIdAsync(id, cancellationToken);

        if (schedule == null) return new PatchScheduleStatusResponse.ScheduleNotFoundResponse();

        List<PlayerModel> players =
            await _playerService.GetPlayersByScheduleId(schedule.Id, cancellationToken).ToListAsync(cancellationToken);

        if (players.Count != PlayerCount) return new PatchScheduleStatusResponse.NotEnoughPlayersResponse();

        var evt = new ScheduleGameEvent(schedule.Id, players.Select(player => player.CharacterId).ToArray());

        await _eventPublisher.PublishAsync(evt, cancellationToken);

        await _context.Schedules.PatchStatusAsync(id, status, cancellationToken);

        return new PatchScheduleStatusResponse.SuccessResponse();
    }
}