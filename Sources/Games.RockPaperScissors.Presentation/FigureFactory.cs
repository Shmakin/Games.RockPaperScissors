using System;
using System.Collections.Generic;
using Games.RockPaperScissors.Domain;
using Games.RockPaperScissors.Presentation.Requests;
using LanguageExt.Common;

namespace Games.RockPaperScissors.Presentation
{
    public class FigureFactory
    {
        private readonly Dictionary<byte, string> figures;

        public FigureFactory(
            Dictionary<byte, string> figures)
        {
            this.figures = figures;
        }

        public Result<Figure> Create(PlayRequest playRequest)
        {
            if (!this.figures.ContainsKey((byte)playRequest.PlayerFigureId))
            {
                return new Result<Figure>(new Exception($"Unknown figure id: {playRequest.PlayerFigureId}"));
            }

            string figureName = this.figures[(byte)playRequest.PlayerFigureId];
            return new Result<Figure>(new Figure(playRequest.PlayerFigureId.ToString(), figureName));
        }
    }
}