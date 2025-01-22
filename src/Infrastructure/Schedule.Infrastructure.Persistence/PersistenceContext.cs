using Schedule.Application.Abstractions.Persistence;
using Schedule.Application.Abstractions.Persistence.Repositories;

namespace Schedule.Infrastructure.Persistence;

public class PersistenceContext : IPersistenceContext
{
    public PersistenceContext(IScheduleRepository schedules)
    {
        Schedules = schedules;
    }

    public IScheduleRepository Schedules { get; }
}