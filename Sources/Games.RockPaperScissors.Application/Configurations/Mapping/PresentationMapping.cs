using System;
using System.Linq;
using AutoMapper;
using Games.RockPaperScissors.Domain;
using Games.RockPaperScissors.Domain.Games;
using Games.RockPaperScissors.Domain.Scoreboards;
using Games.RockPaperScissors.Presentation.Responses;

namespace Games.RockPaperScissors.Application.Configurations.Mapping
{
    public class PresentationMapping : Profile
    {
        public PresentationMapping()
        {
            this.CreateMap<GameOutcome, GameResultResponse>()
                .ConvertUsing(gameOutcome => this.MapGameOutcomeToResponse(gameOutcome));
            this.CreateMap<Figure, GetRandomFigureResponse>()
                .ConvertUsing(figure => new GetRandomFigureResponse
                {
                    FigureId = int.Parse(figure.Id),
                    FigureName = figure.FigureName
                });
            this.CreateMap<Figure[], AllowedFigureResponse[]>()
                .ConvertUsing(figures => figures
                    .Select(figure => new AllowedFigureResponse
                    {
                        Id = figure.Id,
                        Name = figure.FigureName
                    })
                    .OrderBy(x => x.Id)
                    .ToArray());
            this.CreateMap<Scoreboard, ScoreboardResponse[]>()
                .ConvertUsing(scoreboard => this.MapScoreboardToResponse(scoreboard));
        }

        private GameResultResponse MapGameOutcomeToResponse(GameOutcome gameOutcome)
        {
            return new GameResultResponse
            {
                Result = this.MapGameConditionToString(gameOutcome.GameCondition),
                ComputerFigure = byte.Parse(gameOutcome.ComputerFigure.Id),
                PlayerFigure = byte.Parse(gameOutcome.PlayerFigure.Id)
            };
        }

        private string MapGameConditionToString(GameCondition gameCondition)
        {
            switch (gameCondition)
            {
                case GameCondition.Tie: return "Tie";
                case GameCondition.Win: return "Win";
                case GameCondition.Loose: return "Loose";
                default: throw new Exception("Unknown game condition");
            }
        }

        private ScoreboardResponse[] MapScoreboardToResponse(Scoreboard scoreboard)
        {
            return scoreboard.GameOutcomes
                .Select(x => new ScoreboardResponse
                {
                    ComputerFigure = x.ComputerFigure.FigureName,
                    GameCondition = this.MapGameConditionToString(x.GameCondition),
                    GameId = scoreboard.GameId,
                    PlayerFigure = x.PlayerFigure.FigureName
                })
                .ToArray();
        }
    }
}