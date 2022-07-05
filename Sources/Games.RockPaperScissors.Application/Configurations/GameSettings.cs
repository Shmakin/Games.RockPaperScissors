using System.Collections.Generic;

namespace Games.RockPaperScissors.Application.Configurations
{
    public class GameSettings
    {
        public class GameConfiguration
        {
            public string ComputerStrategy { get; set; }

            public Dictionary<string, int> Figures { get; set; }

            public GameRule[] Rules { get; set; }
        }

        public class GameRule
        {
            public string Winner { get; set; }
            public string Looser { get; set; }
        }

        public Dictionary<string, GameConfiguration> Games { get; set; }
    }
}