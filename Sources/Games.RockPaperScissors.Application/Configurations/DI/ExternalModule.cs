using AutoMapper;
using Games.RockPaperScissors.Domain;
using Games.RockPaperScissors.Domain.Scoreboards;
using Games.RockPaperScissors.External;
using Games.RockPaperScissors.External.Scoreboards;
using Microsoft.Extensions.DependencyInjection;

namespace Games.RockPaperScissors.Application.Configurations.DI
{
    public class ExternalModule : IDiModule
    {
        private readonly IServiceCollection services;

        public ExternalModule(IServiceCollection services)
        {
            this.services = services;
        }

        public void Register()
        {
            this.RegisterRandomGateway(this.services);
            this.RegisterScoreboardRepository(this.services);
        }

        private void RegisterRandomGateway(IServiceCollection services)
        {
            services.AddSingleton<IRandomGateway, RandomGateway>(provider =>
            {
                ServiceSettings settings = (ServiceSettings)provider.GetService(typeof(ServiceSettings));
                return new RandomGateway(settings.ApiForRandomValueConnectionString);
            });
        }

        private void RegisterScoreboardRepository(IServiceCollection services)
        {
            services.AddSingleton<IScoreboardRepository, ScoreboardRepository>(provider =>
            {
                IMapper mapper = (IMapper)provider.GetService(typeof(IMapper));
                return new ScoreboardRepository(mapper, "Data Source=Application.db;Cache=Shared");
            });
        }
    }
}