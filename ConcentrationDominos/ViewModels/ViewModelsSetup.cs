using Microsoft.Extensions.DependencyInjection;

namespace ConcentrationDominos.ViewModels
{
    public static class ViewModelsSetup
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services)
            => services
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<IGameInstructionsViewModel, GameInstructionsViewModel>()
                .AddSingleton<IGameBoardViewModel, GameBoardViewModel>()
                .AddTransient<IGameBoardTileViewModel, GameBoardTileViewModel>()
                .AddTransient<IGameSettingsViewModel, GameSettingsViewModel>()
                .AddTransient<IGameViewModel, GameViewModel>();
    }
}
