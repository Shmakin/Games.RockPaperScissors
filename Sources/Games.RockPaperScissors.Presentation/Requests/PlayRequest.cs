using System.Text.Json.Serialization;

namespace Games.RockPaperScissors.Presentation.Requests
{
    public class PlayRequest
    {
        [JsonPropertyName("player")]
        public int PlayerFigureId { get; set; }
    }
}