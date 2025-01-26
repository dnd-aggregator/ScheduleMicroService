using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Schedule.Application.Contracts;
using Schedule.Application.Contracts.Requests;
using Schedule.Application.Models;
using Schedules.Contracts;

namespace Schedule.Presentation.Grpc.Controllers;

public class ScheduleController : ScheduleService.ScheduleServiceBase
{
    private readonly IScheduleService _scheduleService;

    public ScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    public override async Task<CreateScheduleGrpcResponse> CreateSchedule(
        CreateScheduleGrpcRequest request,
        ServerCallContext context)
    {
        var applicationRequest = new CreateScheduleRequest(
            request.MasterId,
            request.Location,
            DateOnly.FromDateTime(request.Date.ToDateTime()));

        long scheduleId =
            await _scheduleService.CreateAsync(applicationRequest, cancellationToken: context.CancellationToken);

        return new CreateScheduleGrpcResponse()
        {
            Id = scheduleId,
        };
    }

    public override async Task<GetScheduleGrpcResponse> GetSchedule(
        GetScheduleGrpcRequest request,
        ServerCallContext context)
    {
        ScheduleModel schedule =
            await _scheduleService.GetByIdAsync(request.Id, cancellationToken: context.CancellationToken) ??
            throw new RpcException(new Status(StatusCode.NotFound, "Schedule not found"));

        return new GetScheduleGrpcResponse()
        {
            Response = new ScheduleGrpc()
            {
                Id = schedule.Id,
                MasterId = schedule.MasterId,
                Location = schedule.Location,
                Date = Timestamp.FromDateTime(schedule.Date.ToDateTime(TimeOnly.MinValue).ToUniversalTime()),
                ScheduleStatus = MapStatusToGrpc(schedule.Status),
            },
        };
    }

    public override async Task<GetSchedulesGrpcResponse> GetSchedules(
        GetSchedulesGrpcRequest request,
        ServerCallContext context)
    {
        var applicationRequest = new GetSchedulesRequest(
            ScheduleIds: request.Ids.ToArray(),
            Location: request.Location,
            Date: DateOnly.FromDateTime(request.Date.ToDateTime()),
            Cursor: request.Cursor,
            PageSize: request.PageSize);

        List<ScheduleModel> schedules = await _scheduleService
            .GetSchedulesAsync(applicationRequest, cancellationToken: context.CancellationToken)
            .ToListAsync();

        RepeatedField<ScheduleGrpc> grpcSchedules = ToGrpcResponse(schedules);

        return new GetSchedulesGrpcResponse()
        {
            Response = { grpcSchedules },
        };
    }

    private static RepeatedField<ScheduleGrpc> ToGrpcResponse(List<ScheduleModel> schedules)
    {
        var grpcSchedules = new RepeatedField<ScheduleGrpc>();

        foreach (ScheduleModel schedule in schedules)
        {
            grpcSchedules.Add(new ScheduleGrpc
            {
                Id = schedule.Id,
                MasterId = schedule.MasterId,
                Location = schedule.Location,
                Date = Timestamp.FromDateTime(schedule.Date.ToDateTime(TimeOnly.MinValue).ToUniversalTime()),
                ScheduleStatus = MapStatusToGrpc(schedule.Status),
            });
        }

        return grpcSchedules;
    }

    private static ScheduleStatusGrpc MapStatusToGrpc(ScheduleStatus status)
    {
        return status switch
        {
            ScheduleStatus.Unknown => ScheduleStatusGrpc.ScheduleStatusUnspecified,
            ScheduleStatus.Draft => ScheduleStatusGrpc.ScheduleStatusDraft,
            ScheduleStatus.Planned => ScheduleStatusGrpc.ScheduleStatusPlanned,
            ScheduleStatus.Started => ScheduleStatusGrpc.ScheduleStatusStarted,
            ScheduleStatus.Finished => ScheduleStatusGrpc.ScheduleStatusFinished,
            _ => ScheduleStatusGrpc.ScheduleStatusUnspecified,
        };
    }
}