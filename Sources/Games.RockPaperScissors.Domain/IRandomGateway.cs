using LanguageExt.Common;

namespace Games.RockPaperScissors.Domain
{
    public interface IRandomGateway
    {
        /// <summary>
        /// Get next random value.
        /// </summary>
        /// <returns>Random value in range.</returns>
        public Result<byte> GetNext(int minValue, int maxValue);
    }
}