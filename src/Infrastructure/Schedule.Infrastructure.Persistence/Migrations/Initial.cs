using FluentMigrator;
using Itmo.Dev.Platform.Persistence.Postgres.Migrations;

namespace Schedule.Infrastructure.Persistence.Migrations;

[Migration(1731949849, "initial")]
public class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider) =>
        """
        CREATE TABLE schedules (
            id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
            location VARCHAR(255) NOT NULL,
            date DATE NOT NULL
        );
        """;

    protected override string GetDownSql(IServiceProvider serviceProvider) =>
        """
        drop table schedules;
        """;
}