using Itmo.Dev.Platform.Persistence.Abstractions.Commands;
using Itmo.Dev.Platform.Persistence.Abstractions.Connections;
using Schedule.Application.Abstractions.Persistence.Dbo;
using Schedule.Application.Abstractions.Persistence.Queries;
using Schedule.Application.Abstractions.Persistence.Repositories;
using Schedule.Application.Models;

namespace Schedule.Infrastructure.Persistence.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly IPersistenceConnectionProvider _connectionProvider;

    public ScheduleRepository(IPersistenceConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public IAsyncEnumerable<ScheduleModel> QueryAsync(ScheduleQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<long> AddAsync(ScheduleDbo schedule, CancellationToken cancellationToken)
    {
        const string sql = """
                           insert into schedules (location) 
                           values (@location)
                           returning id;
                           """;

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("@location", schedule.Location);

        return await command.ExecuteNonQueryAsync(cancellationToken);
    }
}