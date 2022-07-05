using Games.RockPaperScissors.Domain.Games;
using LanguageExt;
using LanguageExt.Common;

namespace Games.RockPaperScissors.Domain.Scoreboards
{
    public interface IScoreboardRepository
    {
        Result<Unit> Store(GameOutcome gameOutcome, string gameId);

        /// <summary>
        /// Get last N game outcomes.
        /// </summary>
        /// <param name="count">Count of last results to take.</param>
        /// <returns>Game scoreboards with last N game outcomes.</returns>
        Result<Scoreboard> Get(int count, string gameId);
    }
}