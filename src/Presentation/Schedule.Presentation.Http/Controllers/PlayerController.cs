using Microsoft.AspNetCore.Mvc;
using Schedule.Application.Contracts;
using Schedule.Application.Contracts.Requests;
using Schedule.Application.Models;

namespace Schedule.Presentation.Http.Controllers;

[ApiController]
[Route("api/v1/players")]
public class PlayerController
{
    private readonly IPlayerService _playerService;

    public PlayerController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpPost]
    public async Task AddPlayer(AddPlayerRequest addPayerRequest, CancellationToken cancellationToken)
    {
        await _playerService.AddPlayer(addPayerRequest, cancellationToken);
    }

    [HttpGet]
    public IAsyncEnumerable<PlayerModel> GetPlayersByScheduleId(long scheduleId, CancellationToken cancellationToken)
    {
        return _playerService.GetPlayersByScheduleId(scheduleId, cancellationToken);
    }

    [HttpDelete]
    public async Task DeletePlayerFromSchedule(long scheduleId, long userId, CancellationToken cancellationToken)
    {
        await _playerService.DeletePlayerFromSchedule(scheduleId, userId, cancellationToken);
    }
}