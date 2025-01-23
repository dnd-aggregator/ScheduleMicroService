using Schedule.Application.Abstractions.Persistence.Repositories;

namespace Schedule.Application.Abstractions.Persistence;

public interface IPersistenceContext
{
    // TODO: add repository properties
    IScheduleRepository Schedules { get; }

    IPlayerRepository Players { get; }
}