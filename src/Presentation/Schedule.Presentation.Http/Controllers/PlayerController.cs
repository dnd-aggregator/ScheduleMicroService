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
    [ProducesResponseType(typeof(AddPlayerResponse), 200)]
    public async Task<IActionResult> AddPlayer(AddPlayerRequest addPayerRequest, CancellationToken cancellationToken)
    {
        AddPlayerResponse result = await _playerService.AddPlayer(addPayerRequest, cancellationToken);
        return new OkObjectResult(result);
    }

    [HttpGet]
    public IAsyncEnumerable<PlayerModel> GetPlayersByScheduleId(long scheduleId, CancellationToken cancellationToken)
    {
        return _playerService.GetPlayersByScheduleId(scheduleId, cancellationToken);
    }

    [HttpPatch]
    public async Task PatchCharacter(PatchCharacterRequest request, CancellationToken cancellationToken)
    {
        await _playerService.PatchCharacter(request, cancellationToken);
    }

    [HttpDelete]
    public async Task DeletePlayerFromSchedule(long scheduleId, long userId, CancellationToken cancellationToken)
    {
        await _playerService.DeletePlayerFromSchedule(scheduleId, userId, cancellationToken);
    }
}