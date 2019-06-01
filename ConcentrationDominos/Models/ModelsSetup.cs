using Microsoft.Extensions.DependencyInjection;

namespace ConcentrationDominos.Models
{
    public static class ModelsSetup
    {
        public static IServiceCollection AddModels(this IServiceCollection services)
            => services
                .AddSingleton<GameStateModel>();
    }
}
