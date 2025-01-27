namespace Schedule.Application.Models;

public abstract record PatchCharacterResponse
{
    public sealed record PatchCharacterSuccessResponse() : PatchCharacterResponse();

    public sealed record PatchCharacterScheduleNotFoundResponse() : PatchCharacterResponse();

    public sealed record PatchCharacterUserNotFoundResponse() : PatchCharacterResponse();

    public sealed record PatchCharacterCharacterNotFoundResponse() : PatchCharacterResponse();
}