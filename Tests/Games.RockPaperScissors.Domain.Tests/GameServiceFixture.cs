using System;
using System.Collections;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using Games.RockPaperScissors.Domain.Games;
using Games.RockPaperScissors.Domain.Rules;
using Games.RockPaperScissors.Domain.Scoreboards;
using LanguageExt;
using LanguageExt.Common;
using NUnit.Framework;
using TestExtensions;

namespace Games.RockPaperScissors.Domain.Tests
{
    [TestFixture]
    public class GameServiceFixture
    {
        private GameService sut;
        private IComputerPlayerService computerPlayerService;
        private IScoreboardRepository scoreboardRepository;

        public class TestCasesData
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new NUnit.Framework.TestCaseData(
                            new Figure("1", "Rock"),
                            new Figure("1", "Rock"),
                            new Result<GameOutcome>(new GameOutcome(
                                new Figure("1", "Rock"),
                                new Figure("1", "Rock"),
                                GameCondition.Tie)));
                    yield return new NUnit.Framework.TestCaseData(
                        new Figure("2", "Scissors"),
                        new Figure("1", "Rock"),
                        new Result<GameOutcome>(new GameOutcome(
                            new Figure("2", "Scissors"),
                            new Figure("1", "Rock"),
                            GameCondition.Loose)));
                    yield return new NUnit.Framework.TestCaseData(
                        new Figure("3", "Paper"),
                        new Figure("1", "Rock"),
                        new Result<GameOutcome>(new GameOutcome(
                            new Figure("3", "Paper"),
                            new Figure("1", "Rock"),
                            GameCondition.Win)));
                }
            }
        }

        [SetUp]
        public void SetUp()
        {
            RuleSet ruleSet = this.BuildRuleSet();
            Figure[] allowedFigures = this.BuildAllowedFigures();
            this.computerPlayerService = A.Fake<IComputerPlayerService>();
            this.scoreboardRepository = A.Fake<IScoreboardRepository>();
            A.CallTo(() => this.scoreboardRepository.Store(A<GameOutcome>._, A<string>._))
                .Returns(new Result<Unit>(Unit.Default));

            this.sut = new GameService(
                "0",
                ruleSet,
                allowedFigures,
                this.computerPlayerService,
                this.scoreboardRepository);
        }

        [TestCaseSource(typeof(TestCasesData), nameof(TestCasesData.TestCases))]
        public void GivenPlayerPlaysWhenComputerResultIsOkThenResultShouldBeEqualToExpected(
            Figure playerFigure,
            Figure computerFigure,
            Result<GameOutcome> expected)
        {
            A.CallTo(() => this.computerPlayerService.PickFigure(playerFigure))
                .Returns(new Result<Figure>(computerFigure));
            Result<GameOutcome> actual = this.sut.Play(playerFigure);
            actual.Should().BeSameResultAs(expected);
        }

        [Test]
        public void GivenPlayerPlaysWhenComputerResultIsFailureThenResultShouldBeFailure()
        {
            A.CallTo(() => this.computerPlayerService.PickFigure(A<Figure>._))
                .Returns(new Result<Figure>(new Exception("fail")));
            var figure = new Figure("2", "Scissors");
            Result<GameOutcome> actual = this.sut.Play(figure);
            var expected = new Result<GameOutcome>(new Exception("test"));
            actual.Should().BeSameResultAs(expected);
        }

        [Test]
        public void GivenPlayerPlaysWhenRuleDoesNotExistThenResultShouldBeFailure()
        {
            var playerFigure = new Figure("66", "Alien");
            var computerFigure = new Figure("1", "Rock");
            A.CallTo(() => this.computerPlayerService.PickFigure(playerFigure))
                .Returns(new Result<Figure>(computerFigure));
            Result<GameOutcome> actual = this.sut.Play(playerFigure);
            var expected = new Result<GameOutcome>(new RuleNotFoundException(playerFigure, computerFigure));
            actual.Should().BeSameResultAs(expected);
        }

        [Test]
        public void GivenPlayerGetsAllowedFiguresWhenFiguresExistThenResultShouldBeEqualToExpected()
        {
            Result<Figure[]> actual = this.sut.GetAllowedFigures();
            var expected = new Result<Figure[]>(this.BuildAllowedFigures());
            actual.Should().BeSameResultAs(expected);
        }

        [Test]
        public void GivenPlayerGetsRandomFigureWhenFiguresExistThenResultShouldBeEqualToExpected()
        {
            var randomFigure = new Figure("1", "rock");
            A.CallTo(() => this.computerPlayerService.PickRandomFigure())
                .Returns(new Result<Figure>(randomFigure));
            Result<Figure> actual = this.sut.GetRandomFigure();
            var expected = new Result<Figure>(randomFigure);
            actual.Should().BeSameResultAs(expected);
        }

        private RuleSet BuildRuleSet()
        {
            Dictionary<string, Dictionary<string, bool>> rules = new Dictionary<string, Dictionary<string, bool>>
            {
                { "1", new Dictionary<string, bool> { { "2", true }, { "3", false } } },
                { "2", new Dictionary<string, bool> { { "1", false }, { "3", true } } },
                { "3", new Dictionary<string, bool> { { "1", true }, { "2", false } } }
            };
            return new RuleSet(rules);
        }

        private Figure[] BuildAllowedFigures()
        {
            return new[]
            {
                new Figure("1", "Rock"),
                new Figure("2", "Scissors"),
                new Figure("3", "Paper")
            };
        }
    }
}