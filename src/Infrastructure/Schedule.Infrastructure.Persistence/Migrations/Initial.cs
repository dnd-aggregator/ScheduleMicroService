using FluentMigrator;

namespace Schedule.Infrastructure.Persistence.Migrations;

[Migration(1731949849, "initial")]
public class Initial : Migration
{
    public override void Up()
    {
        Execute.WithConnection((conn, _) =>
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "CREATE TYPE schedule_status AS ENUM ('draft', 'planned', 'started', 'finished');";
            cmd.ExecuteNonQuery();
        });

        Execute.Sql("""
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
                    );
                    """);
    }

    public override void Down()
    {
        Execute.Sql("""
                    DROP TABLE schedules;
                    DROP TABLE players;
                    """);

        Execute.WithConnection((conn, _) =>
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DROP TYPE schedule_status;";
            cmd.ExecuteNonQuery();
        });
    }
}