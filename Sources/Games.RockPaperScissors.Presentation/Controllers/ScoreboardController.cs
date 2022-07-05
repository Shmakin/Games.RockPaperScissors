using AutoMapper;
using FluentValidation;
using Games.RockPaperScissors.Domain.Games;
using Games.RockPaperScissors.Domain.Scoreboards;
using Games.RockPaperScissors.Presentation.Responses;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace Games.RockPaperScissors.Presentation.Controllers
{
    [Route("scoreboard")]
    [ApiController]
    public class ScoreboardController : ControllerBase
    {
        private readonly IGameService gameService;
        private readonly IMapper mapper;

        public ScoreboardController(
            IGameService gameService,
            IMapper mapper)
        {
            this.gameService = gameService;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetScoreboard()
        {
            // todo: remove hardcode
            Result<Scoreboard> result = this.gameService.GetScoreboard(10);

            return result.Match(
                Succ: scoreboard => this.Ok(this.mapper.Map<ScoreboardResponse[]>(scoreboard)),
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