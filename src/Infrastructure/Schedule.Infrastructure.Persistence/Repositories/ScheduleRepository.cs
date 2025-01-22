using Itmo.Dev.Platform.Persistence.Abstractions.Commands;
using Itmo.Dev.Platform.Persistence.Abstractions.Connections;
using Schedule.Application.Abstractions.Persistence.Dbo;
using Schedule.Application.Abstractions.Persistence.Queries;
using Schedule.Application.Abstractions.Persistence.Repositories;
using Schedule.Application.Models;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Schedule.Infrastructure.Persistence.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly IPersistenceConnectionProvider _connectionProvider;

    public ScheduleRepository(IPersistenceConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async IAsyncEnumerable<ScheduleModel> QueryAsync(
        ScheduleQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
                           select *
                           from schedules
                           where
                            (id > :cursor)
                            and (cardinality(:ids) = 0 or id = any (:ids))
                            and (:location is null or location like :location)
                           limit :page_size;
                           """;

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("ids", query.ScheduleIds)
            .AddParameter("location", query.Location)
            .AddParameter("cursor", query.Cursor)
            .AddParameter("page_size", query.PageSize);

        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new ScheduleModel(
                Id: reader.GetInt64(0),
                Location: reader.GetString(1),
                Date: reader.GetFieldValue<DateOnly>(2));
        }
    }

    public async Task<long> AddAsync(ScheduleDbo schedule, CancellationToken cancellationToken)
    {
        const string sql = """
                           insert into schedules (location, date) 
                           values (@location, @date)
                           returning id;
                           """;

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("@location", schedule.Location)
            .AddParameter("@date", schedule.Date);

        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken)) return reader.GetInt64(0);
        throw new InvalidOperationException();
    }
}