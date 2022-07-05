using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Games.RockPaperScissors.Domain.Games;
using Games.RockPaperScissors.Domain.Rules;
using LanguageExt.Common;
using NUnit.Framework;
using TestExtensions;

namespace Games.RockPaperScissors.Domain.Tests
{
    [TestFixture]
    public class RuleSetFixture
    {
        private RuleSet sut;

        [SetUp]
        public void SetUp()
        {
            Dictionary<string, Dictionary<string, bool>> rules = new Dictionary<string, Dictionary<string, bool>>
            {
                { "1", new Dictionary<string, bool> { { "2", true }, { "3", false } } },
                { "2", new Dictionary<string, bool> { { "1", false }, { "3", true } } },
                { "3", new Dictionary<string, bool> { { "1", true }, { "2", false } } }
            };
            this.sut = new RuleSet(rules);
        }

        public class TestCasesData
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new NUnit.Framework.TestCaseData(
                        new Figure("1", "Rock"),
                        new Figure("2", "Scissors"))
                        .Returns(new Result<GameCondition>(GameCondition.Win));
                    yield return new NUnit.Framework.TestCaseData(
                            new Figure("1", "Rock"),
                            new Figure("3", "Paper"))
                        .Returns(new Result<GameCondition>(GameCondition.Loose));
                    yield return new NUnit.Framework.TestCaseData(
                            new Figure("1", "Rock"),
                            new Figure("1", "Rock"))
                        .Returns(new Result<GameCondition>(GameCondition.Tie));
                }
            }
        }

        [TestCaseSource(typeof(TestCasesData), nameof(TestCasesData.TestCases))]
        public Result<GameCondition> GivenRulesExistWhenGetResultByFiguresIsOkThenResultShouldBeEqualToExpected(
            Figure firstFigure,
            Figure secondFigure)
        {
            return this.sut.GetResultByFigures(firstFigure, secondFigure);
        }

        [Test]
        public void GivenRulesExistWhenGetResultByFiguresIsFailureThenResultShouldBeFailure()
        {
            var firstFigure = new Figure("1", "Rock");
            var secondFigure = new Figure("66", "Alien");

            Result<GameCondition> actual = this.sut.GetResultByFigures(firstFigure, secondFigure);
            Result<GameCondition> expected = new Result<GameCondition>(
                new RuleNotFoundException(firstFigure, secondFigure));
            actual.Should().BeSameResultAs(expected);
        }
    }
}