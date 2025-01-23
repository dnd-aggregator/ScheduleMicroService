using Schedule.Application.Abstractions.Persistence;
using Schedule.Application.Abstractions.Persistence.Dbo;
using Schedule.Application.Contracts;
using Schedule.Application.Contracts.Requests;
using Schedule.Application.Models;

namespace Schedule.Application;

public class PlayerService : IPlayerService
{
    private readonly IPersistenceContext _context;

    public PlayerService(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task AddPlayer(AddPlayerRequest addPlayerRequest, CancellationToken cancellationToken)
    {
        var dbo = new PlayerDbo(
            addPlayerRequest.ScheduleId,
            addPlayerRequest.UserId,
            addPlayerRequest.CharacterId);

        await _context.Players.AddPlayer(dbo, cancellationToken);
    }

    public IAsyncEnumerable<PlayerModel> GetPlayersByScheduleId(long id, CancellationToken cancellationToken)
    {
        return _context.Players.GetPlayersByScheduleId(id, cancellationToken);
    }
}