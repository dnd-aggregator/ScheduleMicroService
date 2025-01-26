using System.Text.Json.Serialization;

namespace Schedule.Application.Models;

[JsonSerializable(typeof(AddPlayerResponse))]
[JsonSerializable(typeof(AddPlayerSuccessResponse))]
[JsonSerializable(typeof(AddPlayerScheduleNotFoundResponse))]
[JsonSerializable(typeof(AddPlayerUserNotFoundResponse))]
[JsonSerializable(typeof(AddPlayerCharacterNotFoundResponse))]
public abstract record AddPlayerResponse
{
    public sealed record AddPlayerSuccessResponse() : AddPlayerResponse();

    public sealed record AddPlayerScheduleNotFoundResponse() : AddPlayerResponse();

    public sealed record AddPlayerUserNotFoundResponse() : AddPlayerResponse();

    public sealed record AddPlayerCharacterNotFoundResponse() : AddPlayerResponse();
}