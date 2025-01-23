using Schedule.Application.Abstractions.Persistence.Dbo;
using Schedule.Application.Models;

namespace Schedule.Application.Abstractions.Persistence.Repositories;

public interface IPlayerRepository
{
    Task AddPlayer(PlayerDbo playerDbo, CancellationToken cancellationToken);

    IAsyncEnumerable<PlayerModel> GetPlayersByScheduleId(long scheduleId, CancellationToken cancellationToken);

    Task DeletePlayer(long scheduleId, long userId, CancellationToken cancellationToken);
}