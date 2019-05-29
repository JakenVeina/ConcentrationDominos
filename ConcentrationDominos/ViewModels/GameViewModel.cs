using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;

using ConcentrationDominos.Models;
using ConcentrationDominos.Gameplay;

namespace ConcentrationDominos.ViewModels
{
    public interface IGameViewModel
         : INotifyPropertyChanged,
            IDisposable
    {
        IGameBoardViewModel GameBoard { get; }

        bool IsComplete { get; }

        bool IsIdle { get; }

        IActionCommand PauseCommand { get; }

        IActionCommand RestartCommand { get; }

        IActionCommand RequestSettingsChangeCommand { get; }

        TimeSpan Runtime { get; }

        IGameSettingsViewModel Settings { get; }

        IActionCommand UnpauseCommand { get; }
    }

    public class GameViewModel
        : ViewModelBase,
            IGameViewModel
    {
        public GameViewModel(
            IGameBoardViewModel gameBoard,
            IGameplayService gameplayService,
            GameStateModel gameState,
            IGameSettingsViewModel settings)
        {
            _subscriptions = new CompositeDisposable();

            GameBoard = gameBoard
                .DisposeWith(_subscriptions);

            gameState.State
                .Subscribe(x =>
                {
                    IsComplete = x == GameState.Completed;
                    IsIdle = x == GameState.Idle;
                })
                .DisposeWith(_subscriptions);

            PauseCommand = new ActionCommand(
                    execute: gameplayService.Pause,
                    canExecute: gameplayService.CanPause)
                .DisposeWith(_subscriptions);

            RestartCommand = new ActionCommand(
                    execute: gameplayService.Restart,
                    canExecute: gameplayService.CanRestart)
                .DisposeWith(_subscriptions);

            RequestSettingsChangeCommand = new ActionCommand(
                    execute: gameplayService.RequestSettingsChange,
                    canExecute: gameplayService.CanRequestSettingsChange)
                .DisposeWith(_subscriptions);

            gameState.Runtime
                .Subscribe(x => Runtime = x)
                .DisposeWith(_subscriptions);

            Settings = settings;

            UnpauseCommand = new ActionCommand(
                    execute: gameplayService.Unpause,
                    canExecute: gameplayService.CanUnpause)
                .DisposeWith(_subscriptions);
        }

        public IGameBoardViewModel GameBoard { get; }

        public bool IsComplete
        {
            get => _isComplete;
            private set => TrySetProperty(ref _isComplete, value);
        }
        private bool _isComplete;

        public bool IsIdle
        {
            get => _isIdle;
            private set => TrySetProperty(ref _isIdle, value);
        }
        private bool _isIdle;

        public IActionCommand PauseCommand { get; }

        public IActionCommand RestartCommand { get; }

        public IActionCommand RequestSettingsChangeCommand { get; }

        public TimeSpan Runtime
        {
            get => _runtime;
            private set => TrySetProperty(ref _runtime, value);
        }
        private TimeSpan _runtime;

        public IGameSettingsViewModel Settings { get; }

        public IActionCommand UnpauseCommand { get; }

        public void Dispose()
            => _subscriptions.Dispose();

        private readonly CompositeDisposable _subscriptions;
    }
}
