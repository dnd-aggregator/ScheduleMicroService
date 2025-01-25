using Character.Validation;
using Schedule.Application.Contracts;
using Schedule.Application.Models;
using CharacterValidationResponse = Schedule.Application.Models.CharacterValidationResponse;

namespace Schedule.Presentation.Grpc.Clients;

public class UserGrpcClient : IUsersClient
{
    private readonly UserGrpcService.UserGrpcServiceClient _userGrpcService;

    public UserGrpcClient(UserGrpcService.UserGrpcServiceClient userGrpcService)
    {
        _userGrpcService = userGrpcService;
    }

    public async Task<CharacterValidationResponse> ValidateUsers(PlayerModel player)
    {
        var grpcRequest = new ValidateUserRequest()
        {
            UserId = player.UserId,
            CharacterId = player.CharacterId,
        };

        Character.Validation.CharacterValidationResponse grpcResponse =
            await _userGrpcService.ValidateUserAsync(grpcRequest);

        return grpcResponse.ResultCase switch
        {
            Character.Validation.CharacterValidationResponse.ResultOneofCase.Success =>
                new CharacterValidationResponse.SuccessValidationResult(),
            Character.Validation.CharacterValidationResponse.ResultOneofCase.CharacterNotFound =>
                new CharacterValidationResponse.CharacterNotFoundValidationResult(),
            Character.Validation.CharacterValidationResponse.ResultOneofCase.UserNotFound =>
                new CharacterValidationResponse.UserNotFoundValidationResult(),
            Character.Validation.CharacterValidationResponse.ResultOneofCase.None =>
                new CharacterValidationResponse.CharacterNoKnownValidationResult(),
            _ => new CharacterValidationResponse.CharacterNoKnownValidationResult(),
        };
    }
}