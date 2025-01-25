using Schedule.Application.Models;

namespace Schedule.Application.Contracts;

public interface IUsersClient
{
    Task<CharacterValidationResponse> ValidateUsers(PlayerModel player);
}