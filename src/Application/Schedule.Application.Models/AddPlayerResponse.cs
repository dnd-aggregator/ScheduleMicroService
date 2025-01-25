using System.Text.Json.Serialization;

namespace Schedule.Application.Models;

[JsonSerializable(typeof(AddPlayerResponse))]
[JsonSerializable(typeof(AddPlayerResponse.AddPlayerSuccess))]
[JsonSerializable(typeof(AddPlayerResponse.AddPlayerFailure))]
public abstract record AddPlayerResponse
{
    public sealed record AddPlayerSuccess() : AddPlayerResponse();

    public sealed record AddPlayerFailure() : AddPlayerResponse();
}