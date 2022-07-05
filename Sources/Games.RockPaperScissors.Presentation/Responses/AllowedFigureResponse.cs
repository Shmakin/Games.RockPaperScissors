using System.Text.Json.Serialization;

namespace Games.RockPaperScissors.Presentation.Responses
{
    public class AllowedFigureResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}