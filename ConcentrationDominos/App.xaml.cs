using System;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;

using ConcentrationDominos.Models;
using ConcentrationDominos.Gameplay;
using ConcentrationDominos.Views;
using ConcentrationDominos.ViewModels;

namespace ConcentrationDominos
{
    public partial class App
        : Application
    {
        private void OnExit(object sender, ExitEventArgs e)
        {
            if (_gameView.IsLoaded)
                _gameView.Close();

            foreach (var behavior in _serviceProvider.GetServices<IBehavior>())
                behavior.Stop();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            _serviceProvider = new ServiceCollection()
                .AddSingleton<SystemClockBehavior>()
                .AddSingleton<ISystemClock>(x => x.GetRequiredService<SystemClockBehavior>())
                .AddSingleton<IBehavior>(x => x.GetRequiredService<SystemClockBehavior>())
                .AddSingleton<IBehavior, GameBuildingBehavior>()
                .AddSingleton<IBehavior, GameClockBehavior>()
                .AddSingleton<IGameplayService, GameplayService>()
                .AddSingleton<Random>()
                .AddSingleton<GameStateModel>()
                .AddTransient<IGameBoardTileViewModel, GameBoardTileViewModel>()
                .AddTransient<IGameBoardViewModel, GameBoardViewModel>()
                .AddTransient<IGameSettingsViewModel, GameSettingsViewModel>()
                .AddTransient<IGameViewModel, GameViewModel>()
                .BuildServiceProvider();

            _serviceProvider.GetService<GameStateModel>()
                .UpdateInterval = TimeSpan.FromSeconds(1.0 / 60.0);

            foreach (var behavior in _serviceProvider.GetServices<IBehavior>())
                behavior.Start();

            _serviceProvider.GetService<GameStateModel>()
                .Settings.Value = new GameSettingsModel(
                    dominoSetType: DominoSetType.DoubleSix,
                    memoryInterval: TimeSpan.FromSeconds(1));

            _gameView = new GameView()
            {
                DataContext = _serviceProvider.GetRequiredService<IGameViewModel>()
            };
            _gameView.Show();
        }

        private GameView _gameView;

        private IServiceProvider _serviceProvider;
    }
}
