using Npgsql;
using Schedule.Application.Abstractions.Persistence.Dbo;
using Schedule.Application.Abstractions.Persistence.Repositories;
using Schedule.Application.Models;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Schedule.Infrastructure.Persistence.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public PlayerRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task AddPlayer(PlayerDbo playerDbo, CancellationToken cancellationToken)
    {
        const string sql = """
                           insert into players (schedule_id, user_id, character_id)
                           VALUES (@scheduleId, @userId, @characterId)
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add(new NpgsqlParameter("@scheduleId", playerDbo.ScheduleId));
        command.Parameters.Add(new NpgsqlParameter("@userId", playerDbo.UserId));
        command.Parameters.Add(new NpgsqlParameter("@characterId", playerDbo.CharacterId));

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async IAsyncEnumerable<PlayerModel> GetPlayersByScheduleId(
        long scheduleId,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
                           select *
                           from players
                           where schedule_id = @scheduleId;
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add(new NpgsqlParameter("@scheduleId", scheduleId));

        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new PlayerModel(
                ScheduleId: reader.GetInt64(0),
                UserId: reader.GetInt64(1),
                CharacterId: reader.GetInt64(2));
        }
    }

    public async Task PatchPlayer(PatchPlayerDbo playerDbo, CancellationToken cancellationToken)
    {
        const string sql = """
                           update players 
                           set character_id = @characterId
                           where schedule_id = @scheduleId and user_id = @userId
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add(new NpgsqlParameter("@scheduleId", playerDbo.ScheduleId));
        command.Parameters.Add(new NpgsqlParameter("@userId", playerDbo.UserId));
        command.Parameters.Add(new NpgsqlParameter("@characterId", playerDbo.CharacterId));

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task DeletePlayer(long scheduleId, long userId, CancellationToken cancellationToken)
    {
        const string sql = """
                           DELETE
                           FROM players
                           WHERE schedule_id = @scheduleId AND user_id = @userId
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add(new NpgsqlParameter("@scheduleId", scheduleId));
        command.Parameters.Add(new NpgsqlParameter("@userId", userId));

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}