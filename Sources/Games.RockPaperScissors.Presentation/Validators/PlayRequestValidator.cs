using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Games.RockPaperScissors.Domain;
using Games.RockPaperScissors.Presentation.Requests;

namespace Games.RockPaperScissors.Presentation.Validators
{
    public class PlayRequestValidator : AbstractValidator<PlayRequest>
    {
        private readonly Dictionary<string, Figure> allowedFigures;

        public PlayRequestValidator(Figure[] allowedFigures)
        {
            this.allowedFigures = allowedFigures.ToDictionary(x => x.Id, y => y);
            this.RuleFor(request => request.PlayerFigureId)
                .Must(figureId => this.allowedFigures.ContainsKey(figureId.ToString()));
        }
    }
}