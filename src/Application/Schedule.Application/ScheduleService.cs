using Schedule.Application.Abstractions.Persistence.Dbo;
using Schedule.Application.Abstractions.Persistence.Queries;
using Schedule.Application.Abstractions.Persistence.Repositories;
using Schedule.Application.Contracts;
using Schedule.Application.Contracts.Requests;
using Schedule.Application.Models;

namespace Schedule.Application;

public class ScheduleService : IScheduleService
{
    private const int PlayerCount = 1;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IPlayerService _playerService;

    public ScheduleService(IScheduleRepository scheduleRepository, IPlayerService playerService)
    {
        _scheduleRepository = scheduleRepository;
        _playerService = playerService;
    }

    public async Task<long> CreateAsync(CreateScheduleRequest request, CancellationToken cancellationToken)
    {
        var scheduleDbo = new ScheduleDbo(
            request.MasterId,
            request.Location,
            request.Date,
            ScheduleStatus.Draft);

        return await _scheduleRepository.AddAsync(scheduleDbo, cancellationToken);
    }

    public async Task<ScheduleModel?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _scheduleRepository.GetById(id, cancellationToken);
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

        return _scheduleRepository.QueryAsync(query, cancellationToken);
    }

    public async Task<PatchScheduleStatusResponse> PatchStatusAsync(
        long id,
        ScheduleStatus status,
        CancellationToken cancellationToken)
    {
        ScheduleModel? schedule = await GetByIdAsync(id, cancellationToken);

        if (schedule == null) return new PatchScheduleStatusResponse.ScheduleNotFoundResponse();

        List<PlayerModel> players =
            await _playerService.GetPlayersByScheduleId(schedule.Id, cancellationToken).ToListAsync(cancellationToken);

        if (players.Count != PlayerCount) return new PatchScheduleStatusResponse.NotEnoughPlayersResponse();

        await _scheduleRepository.PatchStatusAsync(id, status, cancellationToken);

        return new PatchScheduleStatusResponse.SuccessResponse();
    }
}