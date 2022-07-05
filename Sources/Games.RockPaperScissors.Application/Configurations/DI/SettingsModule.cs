using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Games.RockPaperScissors.Application.Configurations.DI
{
    public class SettingsModule : IDiModule
    {
        private readonly IConfiguration configuration;
        private readonly IServiceCollection services;

        public SettingsModule(
            IConfiguration configuration,
            IServiceCollection services)
        {
            this.configuration = configuration;
            this.services = services;
        }

        public void Register()
        {
            this.services.AddSingleton(this.configuration.GetSection("GameSettings").Get<GameSettings>());
            this.services.AddSingleton(this.configuration.GetSection("ServiceSettings").Get<ServiceSettings>());
        }
    }
}