using Schedule.Application.Abstractions.Persistence;
using Schedule.Application.Abstractions.Persistence.Repositories;

namespace Schedule.Infrastructure.Persistence;

public class PersistenceContext : IPersistenceContext
{
    public PersistenceContext(IScheduleRepository schedules, IPlayerRepository players)
    {
        Schedules = schedules;
        Players = players;
    }

    public IScheduleRepository Schedules { get; }

    public IPlayerRepository Players { get; }
}