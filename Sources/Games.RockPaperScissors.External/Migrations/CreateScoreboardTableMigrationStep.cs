using Microsoft.Data.Sqlite;

namespace Games.RockPaperScissors.External.Migrations
{
    public class CreateScoreboardTableMigrationStep
    {
        private readonly string connectionString;

        public CreateScoreboardTableMigrationStep(string connectionString = null)
        {
            this.connectionString = connectionString;
        }

        public void Apply()
        {
            using (SqliteConnection connection = this.connectionString is null
                ? new SqliteConnection()
                : new SqliteConnection(this.connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS Scoreboard(
                    [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [GameId] NVARCHAR(128) NOT NULL,
                    [PlayerFigureId] INTEGER NOT NULL,
                    [PlayerFigureName] NVARCHAR(128) NOT NULL,
                    [ComputerFigureId] INTEGER NOT NULL,                
                    [ComputerFigureName] NVARCHAR(128) NOT NULL,
                    [GameResult] NVARCHAR(32) NOT NULL)";

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}