using LanguageExt.Common;

namespace Games.RockPaperScissors.Domain
{
    public interface IComputerPlayerService
    {
        Result<Figure> PickFigure(Figure playerFigure);
        Result<Figure> PickRandomFigure();
    }
}