using System;
using System.Collections.Generic;
using System.Linq;
using Games.RockPaperScissors.Domain.Games;
using Games.RockPaperScissors.Domain.Rules;
using LanguageExt.Common;

namespace Games.RockPaperScissors.Domain
{
    public class ComputerPlayerService : IComputerPlayerService
    {
        private readonly IRandomGateway randomGateway;
        private readonly GameStrategy gameStrategy;
        private readonly RuleSet ruleSet;
        private readonly Figure[] allowedFigures;
        private readonly Dictionary<string, Figure> figuresById;

        public ComputerPlayerService(
            IRandomGateway randomGateway,
            GameStrategy gameStrategy,
            RuleSet ruleSet,
            Figure[] allowedFigures)
        {
            this.randomGateway = randomGateway;
            this.gameStrategy = gameStrategy;
            this.ruleSet = ruleSet;
            this.allowedFigures = allowedFigures;
            this.figuresById = allowedFigures.ToDictionary(x => x.Id, y => y);
        }

        public Result<Figure> PickFigure(Figure playerFigure)
        {
            (int min, int max) minAndMaxRandomValues = this.DefineExpectedMinAndMaxRandomValue();
            switch (this.gameStrategy)
            {
                case GameStrategy.RandomFigure:
                    return this.randomGateway.GetNext(
                            minAndMaxRandomValues.min,
                            minAndMaxRandomValues.max)
                        .Map(x =>
                        {
                            int index = this.GetIndexByRandom(x);
                            return this.allowedFigures[index];
                        });
                case GameStrategy.BestFigure:
                    return
                        this.ruleSet.GetBestFigureId().Bind(
                        this.GetFigureById);
                case GameStrategy.WorstFigure:
                    return
                        this.ruleSet.GetWorstFigureId().Bind(
                        this.GetFigureById);
                case GameStrategy.Cheating:
                    return
                        this.ruleSet.CheatAgainstFigure(playerFigure).Bind(
                        this.GetFigureById);
                default:
                    return new Result<Figure>(new Exception($"Unknown game strategy - {this.gameStrategy}"));
            }
        }

        public Result<Figure> PickRandomFigure()
        {
            (int min, int max) minAndMaxRandomValues = this.DefineExpectedMinAndMaxRandomValue();
            return this.randomGateway.GetNext(minAndMaxRandomValues.min, minAndMaxRandomValues.max)
                .Map(x =>
                {
                    int index = this.GetIndexByRandom(x);
                    return this.allowedFigures[index];
                });
        }

        private (int min, int max) DefineExpectedMinAndMaxRandomValue()
        {
            // we can fail if allowedFigures.Count > 100, but we don't expect this case.
            int segmentSize = 100 / this.allowedFigures.Length;
            return (1, segmentSize * this.allowedFigures.Length);
        }

        private int GetIndexByRandom(int rnd)
        {
            int segmentSize = 100 / this.allowedFigures.Length;
            return (rnd - 1) / segmentSize;
        }

        private Result<Figure> GetFigureById(string id)
        {
            return this.figuresById[id];
        }
    }
}