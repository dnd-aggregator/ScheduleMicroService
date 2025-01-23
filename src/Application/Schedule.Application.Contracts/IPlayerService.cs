using Schedule.Application.Contracts.Requests;
using Schedule.Application.Models;

namespace Schedule.Application.Contracts;

public interface IPlayerService
{
    Task AddPlayer(AddPlayerRequest addPlayerRequest, CancellationToken cancellationToken);

    IAsyncEnumerable<PlayerModel> GetPlayersByScheduleId(long id, CancellationToken cancellationToken);
}