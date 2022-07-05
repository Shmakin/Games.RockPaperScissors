using Newtonsoft.Json;

namespace Games.RockPaperScissors.External
{
    public class RandomValueResponse
    {
        [JsonProperty("random_number")]
        public byte RandomValue { get; set; }
    }
}