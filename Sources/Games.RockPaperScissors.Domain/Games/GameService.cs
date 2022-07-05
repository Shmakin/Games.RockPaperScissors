using Games.RockPaperScissors.Domain.Rules;
using Games.RockPaperScissors.Domain.Scoreboards;
using LanguageExt.Common;

namespace Games.RockPaperScissors.Domain.Games
{
    public class GameService : IGameService
    {
        private readonly string gameId;
        private readonly RuleSet ruleSet;
        private readonly Figure[] allowedFigures;
        private readonly IComputerPlayerService computerPlayerService;
        private readonly IScoreboardRepository scoreboardRepository;

        public GameService(
            string gameId,
            RuleSet ruleSet,
            Figure[] allowedFigures,
            IComputerPlayerService computerPlayerService,
            IScoreboardRepository scoreboardRepository)
        {
            this.gameId = gameId;
            this.ruleSet = ruleSet;
            this.allowedFigures = allowedFigures;
            this.computerPlayerService = computerPlayerService;
            this.scoreboardRepository = scoreboardRepository;
        }

        public Result<GameOutcome> Play(Figure playerFigure)
        {
            return
                this.computerPlayerService.PickFigure(playerFigure).Bind(randomFigure =>
                this.ruleSet.GetResultByFigures(playerFigure, randomFigure).Bind(gameCondition => new Result<GameOutcome>(
                new GameOutcome(playerFigure, randomFigure, gameCondition))).Bind(gameOutcome =>
                this.scoreboardRepository.Store(gameOutcome, this.gameId).Bind(_ => new Result<GameOutcome>(gameOutcome))));
        }

        public Result<Figure[]> GetAllowedFigures()
        {
            return this.allowedFigures;
        }

        public Result<Figure> GetRandomFigure()
        {
            return this.computerPlayerService.PickRandomFigure();
        }

        public Result<Scoreboard> GetScoreboard(int resultsCount)
        {
            return this.scoreboardRepository.Get(resultsCount, this.gameId);
        }
    }
}