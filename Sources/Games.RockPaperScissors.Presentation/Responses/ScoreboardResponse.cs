namespace Games.RockPaperScissors.Presentation.Responses
{
    public class ScoreboardResponse
    {
        public string GameId { get; set; }

        public string PlayerFigure { get; set; }

        public string ComputerFigure { get; set; }

        public string GameCondition { get; set; }
    }
}