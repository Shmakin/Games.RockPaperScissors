using System.Collections.Generic;
using Games.RockPaperScissors.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Games.RockPaperScissors.Application.Configurations.DI
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private delegate IComputerPlayerService ComputerPlayerServiceResolver(string key);

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddDefaultPolicy(
                builder =>
                {
                    // todo: remove hardcode
                    builder.WithOrigins("https://codechallenge.boohma.com")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }));

            List<IDiModule> diModules = new List<IDiModule>
            {
                new DomainModule(services),
                new ExternalModule(services),
                new SettingsModule(this.configuration, services),
                new PresentationModule(services),
                new MappingModule(services)
            };

            foreach (IDiModule diModule in diModules)
            {
                diModule.Register();
            }
        }

        public void Configure(IApplicationBuilder builder, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                builder.UseDeveloperExceptionPage();
            }

            builder.UseCors();
            builder.UseRouting();
            builder.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}