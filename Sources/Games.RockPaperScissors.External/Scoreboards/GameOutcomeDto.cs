namespace Games.RockPaperScissors.External.Scoreboards
{
    public class GameOutcomeDto
    {
        public int Id { get; set; }

        public string GameId { get; set; }
        public int PlayerFigureId { get; set; }
        public string PlayerFigureName { get; set; }
        public int ComputerFigureId { get; set; }
        public string ComputerFigureName { get; set; }
        public string GameResult { get; set; }
    }
}