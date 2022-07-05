using System.Collections.Generic;
using AutoMapper;
using Games.RockPaperScissors.Application.Configurations.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace Games.RockPaperScissors.Application.Configurations.DI
{
    public class MappingModule : IDiModule
    {
        private readonly IServiceCollection serviceCollection;

        public MappingModule(IServiceCollection serviceCollection)
        {
            this.serviceCollection = serviceCollection;
        }

        public void Register()
        {
            this.serviceCollection.AddSingleton<IMapper>(provider =>
            {
                return new Mapper(
                    new MapperConfiguration(
                        expression => expression.AddProfiles(
                            new List<Profile>
                            {
                                new PresentationMapping(),
                                new ExternalMapping()
                            })));
            });
        }
    }
}