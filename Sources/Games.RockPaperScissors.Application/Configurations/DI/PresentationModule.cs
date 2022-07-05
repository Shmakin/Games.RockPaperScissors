using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentValidation;
using Games.RockPaperScissors.Domain;
using Games.RockPaperScissors.Domain.Rules;
using Games.RockPaperScissors.Presentation;
using Games.RockPaperScissors.Presentation.Controllers;
using Games.RockPaperScissors.Presentation.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Games.RockPaperScissors.Application.Configurations.DI
{
    public class PresentationModule : IDiModule
    {
        private delegate FigureFactory FigureFactoryResolver(string key);
        private delegate RequestValidator RequestValidatorResolver(string key);

        private readonly IServiceCollection serviceCollection;

        public PresentationModule(IServiceCollection serviceCollection)
        {
            this.serviceCollection = serviceCollection;
        }

        public void Register()
        {
            this.RegisterFigureFactoryResolver();

            this.serviceCollection.AddSingleton<RequestValidatorResolver>(provider => key =>
            {
                List<IValidator> validators = new List<IValidator>();
                GameSettings settings = (GameSettings)provider.GetService(typeof(GameSettings));
                (RuleSet ruleSet, Figure[] figures) currentGameConfig = GameRulesFactory.CreateGameRulesAndFigures(
                    settings.Games[key]);
                validators.Add(new PlayRequestValidator(currentGameConfig.figures));

                return new RequestValidator(validators);
            });

            this.RegisterRockPaperScissorsSpockLizardControllers();

            this.serviceCollection
                .AddControllers()
                .AddControllersAsServices();
        }

        private void RegisterRockPaperScissorsSpockLizardControllers()
        {
            string gameId = "rock-paper-scissors-lizard-spock";

            this.serviceCollection.AddSingleton(provider =>
            {
                DomainModule.GameServiceResolver gameServiceResolver = (DomainModule.GameServiceResolver)provider
                    .GetService(typeof(DomainModule.GameServiceResolver));
                FigureFactoryResolver figureFactoryResolver = (FigureFactoryResolver)provider
                    .GetService(typeof(FigureFactoryResolver));

                IMapper mapper = (IMapper)provider.GetService(typeof(IMapper));
                RequestValidatorResolver requestValidatorResolver = (RequestValidatorResolver)provider.GetService(typeof(RequestValidatorResolver));

                return new GameController(
                    gameServiceResolver(gameId),
                    figureFactoryResolver(gameId),
                    mapper,
                    requestValidatorResolver(gameId));
            });

            this.serviceCollection.AddSingleton(provider =>
            {
                DomainModule.GameServiceResolver gameServiceResolver = (DomainModule.GameServiceResolver)provider
                    .GetService(typeof(DomainModule.GameServiceResolver));
                IMapper mapper = (IMapper)provider.GetService(typeof(IMapper));

                return new ScoreboardController(
                    gameServiceResolver(gameId),
                    mapper);
            });
        }

        private void RegisterFigureFactoryResolver()
        {
            this.serviceCollection.AddSingleton<FigureFactoryResolver>(provider => key =>
            {
                GameSettings settings = (GameSettings)provider.GetService(typeof(GameSettings));
                (RuleSet ruleSet, Figure[] figures) currentGameConfig = GameRulesFactory.CreateGameRulesAndFigures(
                    settings.Games[key]);
                var dict = currentGameConfig.figures.ToDictionary(x => byte.Parse(x.Id), y => y.FigureName);
                return new FigureFactory(dict);
            });
        }
    }
}