namespace Schedule.Application.Models;

public abstract record CharacterValidationResponse
{
    public sealed record SuccessValidationResult() : CharacterValidationResponse();

    public sealed record UserNotFoundValidationResult() : CharacterValidationResponse();

    public sealed record CharacterNotFoundValidationResult() : CharacterValidationResponse();

    public sealed record CharacterNoKnownValidationResult() : CharacterValidationResponse();
}