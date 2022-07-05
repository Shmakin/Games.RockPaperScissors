using System.Collections;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using Games.RockPaperScissors.Domain.Games;
using Games.RockPaperScissors.Domain.Rules;
using LanguageExt.Common;
using NUnit.Framework;
using TestExtensions;

namespace Games.RockPaperScissors.Domain.Tests
{
    [TestFixture]
    public class ComputerPlayerServiceFixture
    {
        private IRandomGateway randomGateway;
        private Figure[] allowedFigures;
        private RuleSet ruleSet;

        [SetUp]
        public void SetUp()
        {
            this.randomGateway = A.Fake<IRandomGateway>();
            this.allowedFigures = new[]
            {
                new Figure("1", "Rock"),
                new Figure("2", "Paper"),
                new Figure("3", "Scissors")
            };
            Dictionary<string, Dictionary<string, bool>> rules = new Dictionary<string, Dictionary<string, bool>>
            {
                { "1", new Dictionary<string, bool> { { "2", true }, { "3", false } } },
                { "2", new Dictionary<string, bool> { { "1", false }, { "3", true } } },
                { "3", new Dictionary<string, bool> { { "1", true }, { "2", false } } }
            };
            this.ruleSet = new RuleSet(rules);
        }

        public class TestCasesForRandomStrategyData
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new NUnit.Framework.TestCaseData(
                            (byte)33,
                            new Result<Figure>(new Figure("1", "Rock")));
                    yield return new NUnit.Framework.TestCaseData(
                            (byte)66,
                            new Result<Figure>(new Figure("2", "Paper")));
                    yield return new NUnit.Framework.TestCaseData(
                            (byte)99,
                            new Result<Figure>(new Figure("3", "Scissors")));
                }
            }
        }

        [TestCaseSource(typeof(TestCasesForRandomStrategyData), nameof(TestCasesForRandomStrategyData.TestCases))]
        public void GivenRandomGameStrategyAndRandomChoiceWhenGetNextIsOkThenResultShouldBeEqualToExpected(
            byte randomValue,
            Result<Figure> expected)
        {
            ComputerPlayerService sut = new ComputerPlayerService(
                this.randomGateway,
                GameStrategy.RandomFigure,
                this.ruleSet,
                this.allowedFigures);
            A.CallTo(() => this.randomGateway.GetNext(A<int>._, A<int>._))
                .Returns(new Result<byte>(randomValue));
            Result<Figure> actual = sut.PickFigure(new Figure("1", "Rock"));
            actual.Should().BeSameResultAs(expected);
        }
    }
}