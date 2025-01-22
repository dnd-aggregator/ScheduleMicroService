using Microsoft.AspNetCore.Mvc;
using Schedule.Application.Contracts;

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
}