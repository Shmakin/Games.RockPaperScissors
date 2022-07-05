using Games.RockPaperScissors.Domain.Games;

namespace Games.RockPaperScissors.Domain.Scoreboards
{
    public class Scoreboard
    {
        public Scoreboard(
            GameOutcome[] gameOutcomes,
            string gameId)
        {
            this.GameOutcomes = gameOutcomes;
            this.GameId = gameId;
        }

        public string GameId { get; }
        public GameOutcome[] GameOutcomes { get; }
    }
}