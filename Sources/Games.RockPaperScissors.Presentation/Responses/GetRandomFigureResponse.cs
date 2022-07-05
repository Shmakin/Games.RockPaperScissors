using System.Text.Json.Serialization;

namespace Games.RockPaperScissors.Presentation.Responses
{
    public class GetRandomFigureResponse
    {
        [JsonPropertyName("id")]
        public int FigureId { get; set; }

        [JsonPropertyName("name")]
        public string FigureName { get; set; }
    }
}