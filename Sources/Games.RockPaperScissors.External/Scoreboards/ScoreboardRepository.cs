using System;
using System.Collections.Generic;
using AutoMapper;
using Games.RockPaperScissors.Domain.Games;
using Games.RockPaperScissors.Domain.Scoreboards;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Data.Sqlite;

namespace Games.RockPaperScissors.External.Scoreboards
{
    public class ScoreboardRepository : IScoreboardRepository
    {
        private readonly string connectionString;
        private readonly IMapper mapper;

        public ScoreboardRepository(IMapper mapper, string connectionString = null)
        {
            this.connectionString = connectionString;
            this.mapper = mapper;
        }

        public Result<Unit> Store(GameOutcome gameOutcome, string gameId)
        {
            try
            {
                using (SqliteConnection connection = this.connectionString is null
                    ? new SqliteConnection()
                    : new SqliteConnection(this.connectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            @"
                            INSERT INTO Scoreboard (GameId, PlayerFigureId, PlayerFigureName, ComputerFigureId, ComputerFigureName, GameResult)
                            VALUES ($gameIdValue, $playerFigureIdValue, $playerFigureNameValue, $computerFigureIdValue, $computerFigureNameValue, $gameResultValue)
                            ";

                        var dto = this.mapper.Map<GameOutcomeDto>(gameOutcome);

                        SqliteParameter gameIdParameter = command.CreateParameter();
                        gameIdParameter.ParameterName = "$gameIdValue";
                        gameIdParameter.Value = gameId;
                        command.Parameters.Add(gameIdParameter);

                        SqliteParameter playerFigureIdParameter = command.CreateParameter();
                        playerFigureIdParameter.ParameterName = "$playerFigureIdValue";
                        playerFigureIdParameter.Value = dto.PlayerFigureId;
                        command.Parameters.Add(playerFigureIdParameter);

                        SqliteParameter playerFigureNameParameter = command.CreateParameter();
                        playerFigureNameParameter.ParameterName = "$playerFigureNameValue";
                        playerFigureNameParameter.Value = dto.PlayerFigureName;
                        command.Parameters.Add(playerFigureNameParameter);

                        SqliteParameter computerFigureIdParameter = command.CreateParameter();
                        computerFigureIdParameter.ParameterName = "$computerFigureIdValue";
                        computerFigureIdParameter.Value = dto.ComputerFigureId;
                        command.Parameters.Add(computerFigureIdParameter);

                        SqliteParameter computerFigureNameParameter = command.CreateParameter();
                        computerFigureNameParameter.ParameterName = "$computerFigureNameValue";
                        computerFigureNameParameter.Value = dto.ComputerFigureName;
                        command.Parameters.Add(computerFigureNameParameter);

                        SqliteParameter gameResultParameter = command.CreateParameter();
                        gameResultParameter.ParameterName = "$gameResultValue";
                        gameResultParameter.Value = dto.GameResult;
                        command.Parameters.Add(gameResultParameter);

                        command.ExecuteNonQuery();
                    }
                }

                return new Result<Unit>(Unit.Default);
            }
            catch (Exception e)
            {
                return new Result<Unit>(e);
            }
        }

        public Result<Scoreboard> Get(int count, string gameId)
        {
            try
            {
                using (SqliteConnection connection = new SqliteConnection(this.connectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            @"
                            SELECT Id, GameId, PlayerFigureName, ComputerFigureName, GameResult
                            FROM Scoreboard
                            ORDER BY Id ASC LIMIT ($count)
                            ";

                        SqliteParameter countParameter = command.CreateParameter();
                        countParameter.ParameterName = "$count";
                        countParameter.Value = count;
                        command.Parameters.Add(countParameter);

                        using (var reader = command.ExecuteReader())
                        {
                            List<GameOutcome> gameOutcomes = new List<GameOutcome>();
                            while (reader.Read())
                            {
                                GameOutcomeDto dto = new GameOutcomeDto
                                {
                                    Id = reader.GetInt32(0),
                                    GameId = reader.GetString(1),
                                    PlayerFigureName = reader.GetString(2),
                                    ComputerFigureName = reader.GetString(3),
                                    GameResult = reader.GetString(4)
                                };
                                gameOutcomes.Add(this.mapper.Map<GameOutcome>(dto));
                            }

                            return new Result<Scoreboard>(new Scoreboard(gameOutcomes.ToArray(), gameId));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return new Result<Scoreboard>(e);
            }
        }
    }
}