namespace Games.RockPaperScissors.Domain.Games
{
    public class GameOutcome
    {
        public GameOutcome(
            Figure playerFigure,
            Figure computerFigure,
            GameCondition gameCondition)
        {
            this.PlayerFigure = playerFigure;
            this.ComputerFigure = computerFigure;
            this.GameCondition = gameCondition;
        }


        public Figure PlayerFigure { get; }
        public Figure ComputerFigure { get; }
        public GameCondition GameCondition { get; }
    }
}