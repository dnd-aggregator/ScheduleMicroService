using Google.Protobuf.Collections;
using Grpc.Core;
using Schedule.Application.Contracts;
using Schedule.Application.Contracts.Requests;
using Schedule.Application.Models;
using Schedules.Contracts;
using System.Diagnostics;

namespace Schedule.Presentation.Grpc.Controllers;

public class PlayersController : PlayersGrpcService.PlayersGrpcServiceBase
{
    private readonly IPlayerService _playerService;

    public PlayersController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    public override async Task<AddPlayerGrpcResponse> AddPlayer(AddPlayerGrpcRequest request, ServerCallContext context)
    {
        var applicationRequest = new AddPlayerRequest(
            request.Player.ScheduleId,
            request.Player.UserId,
            request.Player.CharacterId);

        AddPlayerResponse response =
            await _playerService.AddPlayer(applicationRequest, context.CancellationToken);

        return response switch
        {
            AddPlayerResponse.AddPlayerSuccessResponse => new AddPlayerGrpcResponse() { Success = new SuccessResult() },
            AddPlayerResponse.AddPlayerScheduleNotFoundResponse => new AddPlayerGrpcResponse()
                { ScheduleNotFound = new ScheduleNotFound() },
            AddPlayerResponse.AddPlayerUserNotFoundResponse => new AddPlayerGrpcResponse()
                { UserNotFound = new UserNotFoundResult() },
            AddPlayerResponse.AddPlayerCharacterNotFoundResponse => new AddPlayerGrpcResponse()
                { CharacterNotFound = new CharacterNotFoundResult() },
            _ => throw new UnreachableException(),
        };
    }

    public override async Task<GetPlayersGrpcResponse> GetPlayersByScheduleId(
        GetPlayersGrpcRequest request,
        ServerCallContext context)
    {
        List<PlayerModel> players = await _playerService
            .GetPlayersByScheduleId(request.ScheduleId, context.CancellationToken)
            .ToListAsync();

        return new GetPlayersGrpcResponse()
        {
            Players = { ToGrpcResponse(players) },
        };
    }

    public override async Task<PatchCharacterGrpcResponse> PatchCharacter(
        PatchCharacterGrpcRequest request,
        ServerCallContext context)
    {
        var applicationRequest = new PatchCharacterRequest(
            request.ScheduleId,
            request.UserId,
            request.CharacterId);

        await _playerService.PatchCharacter(applicationRequest, context.CancellationToken);

        return new PatchCharacterGrpcResponse()
        {
            Success = new SuccessResult(),
        };
    }

    public override async Task<DeletePlayerGrpcResponse> DeletePlayerFromSchedule(
        DeletePlayerGrpcRequest request,
        ServerCallContext context)
    {
        await _playerService.DeletePlayerFromSchedule(request.ScheduleId, request.PayerId, context.CancellationToken);

        return new DeletePlayerGrpcResponse()
        {
            Success = new SuccessResult(),
        };
    }

    private static RepeatedField<PlayerGrpc> ToGrpcResponse(List<PlayerModel> players)
    {
        var grpcPlayers = new RepeatedField<PlayerGrpc>();

        foreach (PlayerModel player in players)
        {
            grpcPlayers.Add(new PlayerGrpc()
            {
                CharacterId = player.CharacterId,
                ScheduleId = player.ScheduleId,
                UserId = player.UserId,
            });
        }

        return grpcPlayers;
    }
}