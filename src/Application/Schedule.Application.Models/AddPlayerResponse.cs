namespace Schedule.Application.Models;

public abstract record AddPlayerResponse
{
    public sealed record AddPlayerSuccess() : AddPlayerResponse();

    public sealed record AddPlayerFailure() : AddPlayerResponse();
}