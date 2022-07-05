using System;
using System.Collections.Generic;
using System.Linq;
using Games.RockPaperScissors.Domain.Games;
using LanguageExt.Common;

namespace Games.RockPaperScissors.Domain.Rules
{
    public class RuleSet
    {
        private readonly Dictionary<string, Dictionary<string, bool>> rulesDictionary;

        /// <summary>
        /// Initialize new instance.
        /// </summary>
        /// <param name="rulesDictionary">
        /// key - figure id, value - dictionary, where key - another figure id, and value is result of comparison.
        /// If true - first figure is stronger, if false - first figure is weaker.</param>
        public RuleSet(Dictionary<string, Dictionary<string, bool>> rulesDictionary)
        {
            this.rulesDictionary = rulesDictionary;
        }

        public Result<GameCondition> GetResultByFigures(Figure firstFigure, Figure secondFigure)
        {
            if (this.rulesDictionary.ContainsKey(firstFigure.Id)
                && this.rulesDictionary[firstFigure.Id].ContainsKey(secondFigure.Id))
            {
                return this.rulesDictionary[firstFigure.Id][secondFigure.Id]
                    ? GameCondition.Win
                    : GameCondition.Loose;
            }
            else if (firstFigure.Id == secondFigure.Id)
            {
                return GameCondition.Tie;
            }
            else
            {
                return new Result<GameCondition>(new RuleNotFoundException(firstFigure, secondFigure));
            }
        }

        public Result<string> GetBestFigureId()
        {
            string bestFigure = null;
            int prevWinConditionsCout = 0;
            foreach (KeyValuePair<string, Dictionary<string, bool>> rule in this.rulesDictionary)
            {
                int currentWinConditionsCount = rule.Value.Count(x => x.Value);
                if (prevWinConditionsCout < currentWinConditionsCount)
                {
                    prevWinConditionsCout = currentWinConditionsCount;
                    bestFigure = rule.Key;
                }
            }

            return bestFigure ?? new Result<string>(new Exception("Not found any best figure."));
        }

        public Result<string> GetWorstFigureId()
        {
            string bestFigure = null;
            int prevWinConditionsCout = 0;
            foreach (KeyValuePair<string, Dictionary<string, bool>> rule in this.rulesDictionary)
            {
                int currentWinConditionsCount = rule.Value.Count(x => x.Value);
                if (prevWinConditionsCout < currentWinConditionsCount)
                {
                    prevWinConditionsCout = currentWinConditionsCount;
                    bestFigure = rule.Key;
                }
            }

            return bestFigure ?? new Result<string>(new Exception("Not found any worst figure."));
        }

        public Result<string> CheatAgainstFigure(Figure figure)
        {
            if (this.rulesDictionary.ContainsKey(figure.Id))
            {
                foreach (KeyValuePair<string, bool> kvp in this.rulesDictionary[figure.Id])
                {
                    if (!kvp.Value)
                    {
                        return kvp.Key;
                    }
                }

                return figure.Id;
            }
            else
            {
                return new Result<string>(new Exception("Player figure not found in rule set"));
            }
        }
    }
}