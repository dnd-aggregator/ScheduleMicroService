using Schedule.Application.Abstractions.Persistence;
using Schedule.Application.Abstractions.Persistence.Dbo;
using Schedule.Application.Contracts;
using Schedule.Application.Contracts.Requests;
using Schedule.Application.Models;

namespace Schedule.Application;

public class PlayerService : IPlayerService
{
    private readonly IUsersClient _usersClient;
    private readonly IPersistenceContext _context;

    public PlayerService(IUsersClient usersClient, IPersistenceContext context)
    {
        _usersClient = usersClient;
        _context = context;
    }

    public async Task<AddPlayerResponse> AddPlayer(AddPlayerRequest addPlayerRequest, CancellationToken cancellationToken)
    {
        var playerModel = new PlayerModel(
            addPlayerRequest.ScheduleId,
            addPlayerRequest.UserId,
            addPlayerRequest.CharacterId);

        CharacterValidationResponse validationResult = await _usersClient.ValidateUsers(playerModel);

        if (validationResult is not CharacterValidationResponse.SuccessValidationResult) return new AddPlayerResponse.AddPlayerFailure();

        var dbo = new PlayerDbo(
            addPlayerRequest.ScheduleId,
            addPlayerRequest.UserId,
            addPlayerRequest.CharacterId);

        await _context.Players.AddPlayer(dbo, cancellationToken);

        return new AddPlayerResponse.AddPlayerSuccess();
    }

    public IAsyncEnumerable<PlayerModel> GetPlayersByScheduleId(long scheduleId, CancellationToken cancellationToken)
    {
        return _context.Players.GetPlayersByScheduleId(scheduleId, cancellationToken);
    }

    public async Task PatchCharacter(PatchCharacterRequest patchCharacterRequest, CancellationToken cancellationToken)
    {
        var dboPatch = new PatchPlayerDbo(
            patchCharacterRequest.ScheduleId,
            patchCharacterRequest.UserId,
            patchCharacterRequest.CharacterId);

        await _context.Players.PatchPlayer(dboPatch, cancellationToken);
    }

    public async Task DeletePlayerFromSchedule(long scheduleId, long userId, CancellationToken cancellationToken)
    {
        await _context.Players.DeletePlayer(scheduleId, userId, cancellationToken);
    }
}