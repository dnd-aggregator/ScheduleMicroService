using Microsoft.AspNetCore.Mvc;
using Schedule.Application.Contracts;
using Schedule.Application.Models;

namespace Schedule.Presentation.Http.Controllers;

[ApiController]
[Route("api/v1/schedules")]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleService _scheduleService;

    public ScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpPost]
    public async Task<long> CreateSchedule(
        [FromBody] CreateScheduleRequest request,
        CancellationToken cancellationToken)
    {
        return await _scheduleService.CreateAsync(request, cancellationToken);
    }

    [HttpGet("{id:long}")]
    public async Task<ScheduleModel?> GetSchedule([FromRoute] long id, CancellationToken cancellationToken)
    {
        return await _scheduleService.GetByIdAsync(id, cancellationToken);
    }

    [HttpGet]
    public IAsyncEnumerable<ScheduleModel> GetSchedules(
        [FromQuery] GetSchedulesRequest request,
        CancellationToken cancellationToken)
    {
        return _scheduleService.GetSchedulesAsync(request, cancellationToken);
    }
}