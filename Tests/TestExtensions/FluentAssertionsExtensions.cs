using System;
using FluentAssertions;
using FluentAssertions.Numeric;
using LanguageExt.Common;

namespace TestExtensions
{
    public static class FluentAssertionsExtensions
    {
        public static void BeSameResultAs<T>(this ComparableTypeAssertions<Result<T>> actual, Result<T> expected)
        {
            Result<T> actualResult = actual.Subject.As<Result<T>>();

            expected.IsSuccess.Should().Be(actualResult.IsSuccess);
            expected.IsFaulted.Should().Be(actualResult.IsFaulted);

            expected.IfSucc(exp =>
            {
                actualResult.IfSucc(act => exp.Should().BeEquivalentTo(act));
            });
            expected.IfFail(exp =>
            {
                actualResult.IfFail(act =>
                {
                    Type expType = exp.GetType();
                    Type actType = act.GetType();
                    actType.Should().Be(expType);
                });
            });
        }

    }
}