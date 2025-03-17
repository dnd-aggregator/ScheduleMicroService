using Npgsql;
using Schedule.Application.Abstractions.Persistence.Dbo;
using Schedule.Application.Abstractions.Persistence.Queries;
using Schedule.Application.Abstractions.Persistence.Repositories;
using Schedule.Application.Models;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Schedule.Infrastructure.Persistence.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public ScheduleRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<ScheduleModel> GetById(long id, CancellationToken cancellationToken)
    {
        const string sql = """
                            SELECT *
                            FROM schedules
                            WHERE id = @id;
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add(new NpgsqlParameter("@id", id));

        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        if (await reader.ReadAsync(cancellationToken))
        {
            return new ScheduleModel(
                Id: reader.GetInt64(0),
                MasterId: reader.GetInt64(1),
                Location: reader.GetString(2),
                Date: reader.GetFieldValue<DateOnly>(3),
                Status: reader.GetFieldValue<ScheduleStatus>(4));
        }

        throw new ApplicationException("Schedule not found");
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

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add(new NpgsqlParameter("ids", query.ScheduleIds));
        command.Parameters.Add(new NpgsqlParameter("location", query.Location ?? null));
        command.Parameters.Add(new NpgsqlParameter("date", query.Date ?? null));
        command.Parameters.Add(new NpgsqlParameter("cursor", query.Cursor));
        command.Parameters.Add(new NpgsqlParameter("page_size", query.PageSize));

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

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add(new NpgsqlParameter("@master_id", schedule.MasterId));
        command.Parameters.Add(new NpgsqlParameter("@location", schedule.Location));
        command.Parameters.Add(new NpgsqlParameter("@date", schedule.Date));
        command.Parameters.Add(new NpgsqlParameter("@status", schedule.Status));

        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken)) return reader.GetInt64(0);
        throw new InvalidOperationException();
    }

    public async Task PatchStatusAsync(long id, ScheduleStatus status, CancellationToken cancellationToken)
    {
        const string sql = """
                            update schedules 
                            set status = @status
                            where id = @id;
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add(new NpgsqlParameter("@id", id));
        command.Parameters.Add(new NpgsqlParameter("@status", status));

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}