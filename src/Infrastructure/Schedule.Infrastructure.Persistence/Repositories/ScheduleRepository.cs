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
                            and (:date is null or date = :date)
                           limit :page_size;
                           """;

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("ids", query.ScheduleIds)
            .AddParameter("location", query.Location)
            .AddParameter("date", query.Date)
            .AddParameter("cursor", query.Cursor)
            .AddParameter("page_size", query.PageSize);

        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new ScheduleModel(
                Id: reader.GetInt64(0),
                MasterId: reader.GetInt64(1),
                Location: reader.GetString(2),
                Date: reader.GetFieldValue<DateOnly>(3),
                Status: reader.GetFieldValue<ScheduleStatus>(4));
        }
    }

    public async Task<long> AddAsync(ScheduleDbo schedule, CancellationToken cancellationToken)
    {
        const string sql = """
                           insert into schedules (master_id, location, date, status) 
                           values (@master_id, @location, @date, @status)
                           returning id;
                           """;

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("@location", schedule.Location)
            .AddParameter("@date", schedule.Date)
            .AddParameter("@master_id", schedule.MasterId)
            .AddParameter("@status", schedule.Status);

        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken)) return reader.GetInt64(0);
        throw new InvalidOperationException();
    }
}