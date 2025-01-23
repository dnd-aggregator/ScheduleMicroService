using Schedule.Application.Contracts.Requests;
using Schedule.Application.Models;

namespace Schedule.Application.Contracts;

public interface IPlayerService
{
    Task AddPlayer(AddPlayerRequest addPlayerRequest, CancellationToken cancellationToken);

    IAsyncEnumerable<PlayerModel> GetPlayersByScheduleId(long scheduleId, CancellationToken cancellationToken);

    Task PatchCharacter(PatchCharacterRequest patchCharacterRequest, CancellationToken cancellationToken);

    Task DeletePlayerFromSchedule(long scheduleId, long userId, CancellationToken cancellationToken);
}