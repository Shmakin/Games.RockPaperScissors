using AutoMapper;
using FluentValidation;
using Games.RockPaperScissors.Domain;
using Games.RockPaperScissors.Domain.Games;
using Games.RockPaperScissors.Presentation.Requests;
using Games.RockPaperScissors.Presentation.Responses;
using Games.RockPaperScissors.Presentation.Validators;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace Games.RockPaperScissors.Presentation.Controllers
{
    [Route("rpssl")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService gameService;
        private readonly FigureFactory figureFactory;
        private readonly IMapper mapper;
        private readonly IRequestValidator requestValidator;

        public GameController(
            IGameService gameService,
            FigureFactory figureFactory,
            IMapper mapper,
            IRequestValidator requestValidator)
        {
            this.gameService = gameService;
            this.figureFactory = figureFactory;
            this.mapper = mapper;
            this.requestValidator = requestValidator;
        }

        [HttpPost("play")]
        public ActionResult Play(PlayRequest playRequest)
        {
            Result<GameOutcome> result =
                this.requestValidator.Validate(playRequest).Bind(_ =>
                this.figureFactory.Create(playRequest).Bind(figure =>
                this.gameService.Play(figure)));

            return result.Match(
                Succ: gameOutcome => this.Ok(this.mapper.Map<GameResultResponse>(gameOutcome)),
                Fail: exception =>
                {
                    switch (exception)
                    {
                        case ValidationException:
                            return this.ValidationProblem();
                        default:
                            return this.StatusCode(500);
                    }
                });
        }

        [HttpGet("choice")]
        public ActionResult GetRandomFigure()
        {
            Result<Figure> result = this.gameService.GetRandomFigure();

            return result.Match(
                Succ: randomFigure => this.Ok(this.mapper.Map<GetRandomFigureResponse>(randomFigure)),
                Fail: exception =>
                {
                    switch (exception)
                    {
                        case ValidationException:
                            return this.ValidationProblem();
                        default:
                            return this.StatusCode(500);
                    }
                });
        }

        [HttpGet("choices")]
        public ActionResult GetAllFigures()
        {
            Result<Figure[]> result = this.gameService.GetAllowedFigures();

            return result.Match(
                Succ: figures => this.Ok(this.mapper.Map<AllowedFigureResponse[]>(figures)),
                Fail: exception =>
                {
                    switch (exception)
                    {
                        case ValidationException:
                            return this.ValidationProblem();
                        default:
                            return this.StatusCode(500);
                    }
                });
        }
    }
}