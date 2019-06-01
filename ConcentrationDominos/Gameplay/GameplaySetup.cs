using Microsoft.Extensions.DependencyInjection;

namespace ConcentrationDominos.Gameplay
{
    public static class GameplaySetup
    {
        public static IServiceCollection AddGameplay(this IServiceCollection services)
            => services
                .AddSingleton<SystemClockBehavior>()
                .AddSingleton<ISystemClock>(x => x.GetRequiredService<SystemClockBehavior>())
                .AddSingleton<IBehavior>(x => x.GetRequiredService<SystemClockBehavior>())
                .AddSingleton<IBehavior, GameBuildingBehavior>()
                .AddSingleton<IBehavior, GameClockBehavior>()
                .AddSingleton<IGameplayService, GameplayService>();
    }
}
