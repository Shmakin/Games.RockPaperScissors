using System;

namespace Games.RockPaperScissors.Domain.Rules
{
    public class RuleNotFoundException : Exception
    {
        public RuleNotFoundException(Figure firstFigure, Figure secondFigure)
            : base($"Rule {firstFigure.FigureName}-{secondFigure.FigureName} was not found.")
        {
        }
    }
}