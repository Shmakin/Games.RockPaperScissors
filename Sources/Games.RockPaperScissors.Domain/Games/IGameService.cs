using Games.RockPaperScissors.Domain.Scoreboards;
using LanguageExt.Common;

namespace Games.RockPaperScissors.Domain.Games
{
    public interface IGameService
    {
        Result<GameOutcome> Play(Figure playerFigure);
        Result<Figure[]> GetAllowedFigures();
        Result<Figure> GetRandomFigure();
        Result<Scoreboard> GetScoreboard(int resultsCount);
    }
}