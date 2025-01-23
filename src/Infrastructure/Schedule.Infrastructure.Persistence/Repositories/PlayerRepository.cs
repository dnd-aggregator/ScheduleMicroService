using Itmo.Dev.Platform.Persistence.Abstractions.Commands;
using Itmo.Dev.Platform.Persistence.Abstractions.Connections;
using Schedule.Application.Abstractions.Persistence.Dbo;
using Schedule.Application.Abstractions.Persistence.Repositories;
using Schedule.Application.Models;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Schedule.Infrastructure.Persistence.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly IPersistenceConnectionProvider _connectionProvider;

    public PlayerRepository(IPersistenceConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task AddPlayer(PlayerDbo playerDbo, CancellationToken cancellationToken)
    {
        const string sql = """
                           insert into players (schedule_id, user_id, character_id)
                           VALUES (@scheduleId, @userId, @characterId)
                           """;

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("@scheduleId", playerDbo.ScheduleId)
            .AddParameter("@userId", playerDbo.UserId)
            .AddParameter("@characterId", playerDbo.CharacterId);

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

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("scheduleId", scheduleId);

        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new PlayerModel(
                ScheduleId: reader.GetInt64(0),
                UserId: reader.GetInt64(1),
                CharacterId: reader.GetInt64(2));
        }
    }

    public async Task DeletePlayer(long scheduleId, long userId, CancellationToken cancellationToken)
    {
        const string sql = """
                           DELETE
                           FROM players
                           WHERE schedule_id = @scheduleId AND user_id = @userId
                           """;

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("@scheduleId", scheduleId)
            .AddParameter("@userId", userId);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}