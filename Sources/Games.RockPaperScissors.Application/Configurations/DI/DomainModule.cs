using Games.RockPaperScissors.Domain;
using Games.RockPaperScissors.Domain.Games;
using Games.RockPaperScissors.Domain.Rules;
using Games.RockPaperScissors.Domain.Scoreboards;
using Microsoft.Extensions.DependencyInjection;

namespace Games.RockPaperScissors.Application.Configurations.DI
{
    public class DomainModule : IDiModule
    {
        private delegate IComputerPlayerService ComputerPlayerServiceResolver(string key);

        public delegate IGameService GameServiceResolver(string key);

        private readonly IServiceCollection services;

        public DomainModule(IServiceCollection services)
        {
            this.services = services;
        }

        public void Register()
        {
            this.RegisterComputerPlayerServiceResolver(this.services);
            this.RegisterGameServiceResolver(this.services);
        }

        private void RegisterComputerPlayerServiceResolver(IServiceCollection services)
        {
            services.AddSingleton<ComputerPlayerServiceResolver>(provider => key =>
            {
                GameSettings settings = (GameSettings)provider.GetService(typeof(GameSettings));
                IRandomGateway randomGateway = (IRandomGateway)provider.GetService(typeof(IRandomGateway));
                (RuleSet ruleSet, Figure[] figures) currentGameConfig = GameRulesFactory.CreateGameRulesAndFigures(
                    settings.Games[key]);
                GameStrategy currentGameStrategy = GameRulesFactory.CreateGameStrategyByString(settings.Games[key].ComputerStrategy);
                return new ComputerPlayerService(
                    randomGateway,
                    currentGameStrategy,
                    currentGameConfig.ruleSet,
                    currentGameConfig.figures);
            });
        }

        private void RegisterGameServiceResolver(IServiceCollection services)
        {
            services.AddSingleton<GameServiceResolver>(provider => key =>
            {
                GameSettings settings = (GameSettings)provider.GetService(typeof(GameSettings));
                IScoreboardRepository scoreboardRepository = (IScoreboardRepository)provider.GetService(typeof(IScoreboardRepository));
                ComputerPlayerServiceResolver computerPlayerService = (ComputerPlayerServiceResolver)provider.GetService(typeof(ComputerPlayerServiceResolver));

                (RuleSet ruleSet, Figure[] figures) currentGameConfig = GameRulesFactory.CreateGameRulesAndFigures(settings.Games[key]);
                return new GameService(
                    key,
                    currentGameConfig.ruleSet,
                    currentGameConfig.figures,
                    computerPlayerService(key),
                    scoreboardRepository);
            });
        }
    }
}