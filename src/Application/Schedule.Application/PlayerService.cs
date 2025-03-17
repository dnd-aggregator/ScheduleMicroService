using Schedule.Application.Abstractions.Persistence.Dbo;
using Schedule.Application.Abstractions.Persistence.Repositories;
using Schedule.Application.Contracts;
using Schedule.Application.Contracts.Requests;
using Schedule.Application.Models;

namespace Schedule.Application;

public class PlayerService : IPlayerService
{
    private readonly IUsersClient _usersClient;
    private readonly IPlayerRepository _playerRepository;

    public PlayerService(IUsersClient usersClient, IPlayerRepository playerRepository)
    {
        _usersClient = usersClient;
        _playerRepository = playerRepository;
    }

    public async Task<AddPlayerResponse> AddPlayer(
        AddPlayerRequest addPlayerRequest,
        CancellationToken cancellationToken)
    {
        var playerModel = new PlayerModel(
            addPlayerRequest.ScheduleId,
            addPlayerRequest.UserId,
            addPlayerRequest.CharacterId);

        CharacterValidationResponse validationResult = await _usersClient.ValidateUsers(playerModel);

        if (validationResult is CharacterValidationResponse.UserNotFoundValidationResult)
            return new AddPlayerResponse.AddPlayerUserNotFoundResponse();
        if (validationResult is CharacterValidationResponse.CharacterNotFoundValidationResult)
            return new AddPlayerResponse.AddPlayerCharacterNotFoundResponse();

        var dbo = new PlayerDbo(
            addPlayerRequest.ScheduleId,
            addPlayerRequest.UserId,
            addPlayerRequest.CharacterId);

        await _playerRepository.AddPlayer(dbo, cancellationToken);

        return new AddPlayerResponse.AddPlayerSuccessResponse();
    }

    public IAsyncEnumerable<PlayerModel> GetPlayersByScheduleId(long scheduleId, CancellationToken cancellationToken)
    {
        return _playerRepository.GetPlayersByScheduleId(scheduleId, cancellationToken);
    }

    public async Task<PatchCharacterResponse> PatchCharacter(
        PatchCharacterRequest patchCharacterRequest,
        CancellationToken cancellationToken)
    {
        var playerModel = new PlayerModel(
            patchCharacterRequest.ScheduleId,
            patchCharacterRequest.UserId,
            patchCharacterRequest.CharacterId);

        CharacterValidationResponse validationResult = await _usersClient.ValidateUsers(playerModel);

        if (validationResult is CharacterValidationResponse.UserNotFoundValidationResult)
            return new PatchCharacterResponse.PatchCharacterUserNotFoundResponse();
        if (validationResult is CharacterValidationResponse.CharacterNotFoundValidationResult)
            return new PatchCharacterResponse.PatchCharacterCharacterNotFoundResponse();
        var dboPatch = new PatchPlayerDbo(
            patchCharacterRequest.ScheduleId,
            patchCharacterRequest.UserId,
            patchCharacterRequest.CharacterId);

        await _playerRepository.PatchPlayer(dboPatch, cancellationToken);

        return new PatchCharacterResponse.PatchCharacterSuccessResponse();
    }

    public async Task DeletePlayerFromSchedule(long scheduleId, long userId, CancellationToken cancellationToken)
    {
        await _playerRepository.DeletePlayer(scheduleId, userId, cancellationToken);
    }
}