using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Games.RockPaperScissors.Domain;
using Games.RockPaperScissors.Domain.Games;
using Games.RockPaperScissors.Presentation.Controllers;
using Games.RockPaperScissors.Presentation.Requests;
using Games.RockPaperScissors.Presentation.Responses;
using Games.RockPaperScissors.Presentation.Validators;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Games.RockPaperScissors.Presentation.Tests
{
    [TestFixture]
    public class GameControllerFixture
    {
        private GameController sut;
        private IGameService gameService;
        private IMapper mapper;
        private IRequestValidator requestValidator;

        [SetUp]
        public void SetUp()
        {
            var figureFactory = new FigureFactory(
                new Dictionary<byte, string>
                {
                    { 1, "rock" },
                    { 2, "paper" },
                    { 3, "scissors" }
                });

            this.gameService = A.Fake<IGameService>();
            this.mapper = A.Fake<IMapper>();
            this.requestValidator = A.Fake<IRequestValidator>();

            this.sut = new GameController(
                this.gameService,
                figureFactory,
                this.mapper,
                this.requestValidator);
        }

        public class TestCasesData
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new NUnit.Framework.TestCaseData(
                        new PlayRequest { PlayerFigureId = 1 },
                        new Result<Unit>(Unit.Default),
                        new Result<GameOutcome>(new GameOutcome(
                            new Figure("1", "rock"),
                            new Figure("2", "paper"),
                            GameCondition.Loose)),
                        typeof(OkObjectResult));

                    yield return new NUnit.Framework.TestCaseData(
                        new PlayRequest { PlayerFigureId = 1 },
                        new Result<Unit>(new Exception("test")),
                        new Result<GameOutcome>(new GameOutcome(
                            new Figure("1", "rock"),
                            new Figure("2", "paper"),
                            GameCondition.Loose)),
                        typeof(StatusCodeResult));

                    yield return new NUnit.Framework.TestCaseData(
                        new PlayRequest { PlayerFigureId = 1 },
                        new Result<Unit>(Unit.Default),
                        new Result<GameOutcome>(new Exception("test")),
                        typeof(StatusCodeResult));
                }
            }
        }

        [TestCaseSource(typeof(TestCasesData), nameof(TestCasesData.TestCases))]
        public void GivenPlayerAndControllerWhenPlayerCallsPlayGameThenResultShouldBeEqualToExpected(
            PlayRequest playRequest,
            Result<Unit> validationResult,
            Result<GameOutcome> playResult,
            Type expected)
        {
            A.CallTo(() => this.requestValidator.Validate(A<PlayRequest>._))
                .Returns(validationResult);
            A.CallTo(() => this.gameService.Play(A<Figure>._))
                .Returns(playResult);


            Type actual = this.sut.Play(playRequest).GetType();
            actual.Should().Be(expected);
        }

        [Test]
        public void GivenPlayerAndControllerWhenPlayerCallsGetAllowedFiguresThenResultShouldBeEqualToExpected()
        {
            A.CallTo(() => this.gameService.GetAllowedFigures())
                .Returns(new Result<Figure[]>(new[] { new Figure("1", "rock") }));
            Type actual = this.sut.GetAllFigures().GetType();
            Type expected = typeof(OkObjectResult);
            actual.Should().Be(expected);
        }

        [Test]
        public void GivenPlayerAndControllerWhenPlayerCallsGetRandomFigureThenResultShouldBeEqualToExpected()
        {
            A.CallTo(() => this.gameService.GetRandomFigure())
                .Returns(new Result<Figure>(new Figure("1", "rock")));
            Type actual = this.sut.GetRandomFigure().GetType();
            Type expected = typeof(OkObjectResult);
            actual.Should().Be(expected);
        }
    }
}