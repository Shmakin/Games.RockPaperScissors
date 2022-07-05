using System;
using System.Collections.Generic;
using System.Linq;
using Games.RockPaperScissors.Application.Configurations;
using Games.RockPaperScissors.Domain;
using Games.RockPaperScissors.Domain.Games;
using Games.RockPaperScissors.Domain.Rules;

namespace Games.RockPaperScissors.Application
{
    public static class GameRulesFactory
    {
        public static (RuleSet ruleSet, Figure[] figures) CreateGameRulesAndFigures(GameSettings.GameConfiguration gameConfiguration)
        {
            return (BuildRuleSet(gameConfiguration), BuildFigureList(gameConfiguration));
        }

        public static GameStrategy CreateGameStrategyByString(string str)
        {
            switch (str)
            {
                case "BestFigure": return GameStrategy.BestFigure;
                case "WorstFigure": return GameStrategy.WorstFigure;
                case "RandomFigure": return GameStrategy.RandomFigure;
                case "Cheating": return GameStrategy.Cheating;
                default: throw new Exception($"Unknown game strategy: {str}");
            }
        }

        private static RuleSet BuildRuleSet(GameSettings.GameConfiguration gameConfiguration)
        {
            var rulesDictionary = new Dictionary<string, Dictionary<string, bool>>();
            foreach (GameSettings.GameRule gameRule in gameConfiguration.Rules)
            {
                string winnerId = gameConfiguration.Figures[gameRule.Winner].ToString();
                string looserId = gameConfiguration.Figures[gameRule.Looser].ToString();

                if (!rulesDictionary.ContainsKey(winnerId))
                {
                    rulesDictionary.Add(winnerId, new Dictionary<string, bool>());
                }

                if (!rulesDictionary.ContainsKey(looserId))
                {
                    rulesDictionary.Add(looserId, new Dictionary<string, bool>());
                }

                rulesDictionary[winnerId].Add(looserId, true);
                rulesDictionary[looserId].Add(winnerId, false);
            }

            return new RuleSet(rulesDictionary);
        }

        private static Figure[] BuildFigureList(GameSettings.GameConfiguration gameConfiguration)
        {
            return gameConfiguration.Figures
                .Select(x => new Figure(x.Value.ToString(), x.Key))
                .ToArray();
        }
    }
}