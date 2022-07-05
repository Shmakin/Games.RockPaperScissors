using System;
using LanguageExt.Common;

namespace Games.RockPaperScissors.Domain
{
    public static class ResultExt
    {
        public static Result<TB> Bind<TA, TB>(this Result<TA> ma, Func<TA, Result<TB>> f)
        {
            try
            {
                return ma.IsBottom
                    ? Result<TB>.Bottom
                    : ma.Match(
                        Succ: v => f(v),
                        Fail: e => new Result<TB>(e));
            }
            catch (Exception e)
            {
                return new Result<TB>(e);
            }
        }
    }
}