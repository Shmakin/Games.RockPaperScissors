using LanguageExt;
using LanguageExt.Common;

namespace Games.RockPaperScissors.Presentation.Validators
{
    public interface IRequestValidator
    {
        Result<Unit> Validate<T>(T request);
    }
}