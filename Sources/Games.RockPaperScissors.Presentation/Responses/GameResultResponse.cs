using System.Text.Json.Serialization;

namespace Games.RockPaperScissors.Presentation.Responses
{
    public class GameResultResponse
    {
        [JsonPropertyName("results")]
        public string Result { get; set; }

        [JsonPropertyName("player")]
        public byte PlayerFigure { get; set; }

        [JsonPropertyName("computer")]
        public byte ComputerFigure { get; set; }
    }
}