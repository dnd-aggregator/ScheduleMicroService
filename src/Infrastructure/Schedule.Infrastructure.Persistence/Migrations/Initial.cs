using FluentMigrator;
using Itmo.Dev.Platform.Persistence.Postgres.Migrations;

namespace Schedule.Infrastructure.Persistence.Migrations;

[Migration(1731949849, "initial")]
public class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider) =>
        """
        create type schedule_status as enum
        (
            'draft',
            'planned',
            'started',
            'finished'
        );

        CREATE TABLE schedules (
            id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
            master_id BIGINT NOT NULL,
            location VARCHAR(255) NOT NULL,
            date DATE NOT NULL,
            status schedule_status NOT NULL
        );

        CREATE TABLE players (
            schedule_id BIGINT NOT NULL,
            user_id BIGINT NOT NULL,
            character_id BIGINT NOT NULL
        )
        """;

    protected override string GetDownSql(IServiceProvider serviceProvider) =>
        """
        drop table schedules;
        drop table players;
        drop type schedule_status;
        """;
}