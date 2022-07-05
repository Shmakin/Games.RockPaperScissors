
using System;
using AutoMapper;
using Games.RockPaperScissors.Domain;
using Games.RockPaperScissors.Domain.Games;
using Games.RockPaperScissors.External.Scoreboards;

namespace Games.RockPaperScissors.Application.Configurations.Mapping
{
    public class ExternalMapping : Profile
    {
        public ExternalMapping()
        {
            this.CreateMap<GameOutcomeDto, GameOutcome>()
                .ConvertUsing(dto => this.MapDtoToGameOutcome(dto));
            this.CreateMap<GameOutcome, GameOutcomeDto>()
                .ConvertUsing(gameOutcome => this.MapGameOutcomeToDto(gameOutcome));
        }

        public GameOutcome MapDtoToGameOutcome(GameOutcomeDto dto)
        {
            return new GameOutcome(
                new Figure(dto.PlayerFigureId.ToString(), dto.PlayerFigureName),
                new Figure(dto.ComputerFigureId.ToString(), dto.ComputerFigureName),
                this.MapStringToGameCondition(dto.GameResult));
        }

        public GameOutcomeDto MapGameOutcomeToDto(GameOutcome gameOutcome)
        {
            return new GameOutcomeDto
            {
                GameResult = this.MapGameConditionToString(gameOutcome.GameCondition),
                ComputerFigureId = int.Parse(gameOutcome.ComputerFigure.Id),
                ComputerFigureName = gameOutcome.ComputerFigure.FigureName,
                PlayerFigureId = int.Parse(gameOutcome.PlayerFigure.Id),
                PlayerFigureName = gameOutcome.PlayerFigure.FigureName
            };
        }

        public GameCondition MapStringToGameCondition(string str)
        {
            switch (str)
            {
                case "Tie": return GameCondition.Tie;
                case "Win": return GameCondition.Win;
                case "Loose": return GameCondition.Loose;
                default: throw new Exception("Unknown game condition");
            }
        }

        public string MapGameConditionToString(GameCondition gameCondition)
        {
            switch (gameCondition)
            {
                case GameCondition.Win: return "Win";
                case GameCondition.Loose: return "Loose";
                case GameCondition.Tie: return "Tie";
                default: throw new Exception("Unknown game condition");
            }
        }
    }
}