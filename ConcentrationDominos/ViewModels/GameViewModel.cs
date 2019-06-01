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
        object CurrentNavigationTarget { get; }

        IGameBoardViewModel GameBoard { get; }

        bool IsComplete { get; }

        bool IsIdle { get; }

        IActionCommand NavigateToInstructionsCommand { get; }

        IActionCommand NavigateToSettingsCommand { get; }

        IActionCommand PauseCommand { get; }

        IActionCommand ResetCommand { get; }

        TimeSpan Runtime { get; }

        IActionCommand StartCommand { get; }

        IActionCommand UnpauseCommand { get; }
    }

    public class GameViewModel
        : ViewModelBase,
            IGameViewModel
    {
        public GameViewModel(
            IGameBoardViewModel gameBoard,
            IGameInstructionsViewModel instructions,
            IGameplayService gameplayService,
            IGameSettingsViewModel settings,
            GameStateModel gameState,
            INavigationService navigationService)
        {
            _instructions = instructions;
            _navigationService = navigationService;
            _settings = settings;
            _subscriptions = new CompositeDisposable();

            navigationService.CurrentTarget
                .Subscribe(x => CurrentNavigationTarget = x)
                .DisposeWith(_subscriptions);

            GameBoard = gameBoard
                .DisposeWith(_subscriptions);

            gameState.State
                .Subscribe(x =>
                {
                    IsComplete = x == GameState.Completed;
                    IsIdle = x == GameState.Idle;
                })
                .DisposeWith(_subscriptions);

            NavigateToInstructionsCommand = new ActionCommand(
                    execute: () => _navigationService.NavigateTo(_instructions),
                    canExecute: navigationService.CanNavigateTo)
                .DisposeWith(_subscriptions);

            NavigateToSettingsCommand = new ActionCommand(
                    execute: () => _navigationService.NavigateTo(_settings),
                    canExecute: navigationService.CanNavigateTo)
                .DisposeWith(_subscriptions);

            PauseCommand = new ActionCommand(
                    execute: gameplayService.Pause,
                    canExecute: gameplayService.CanPause)
                .DisposeWith(_subscriptions);

            ResetCommand = new ActionCommand(
                    execute: gameplayService.Reset,
                    canExecute: gameplayService.CanReset)
                .DisposeWith(_subscriptions);

            gameState.Runtime
                .Subscribe(x => Runtime = x)
                .DisposeWith(_subscriptions);

            StartCommand = new ActionCommand(
                    execute: gameplayService.Start,
                    canExecute: gameplayService.CanStart)
                .DisposeWith(_subscriptions);

            UnpauseCommand = new ActionCommand(
                    execute: gameplayService.Unpause,
                    canExecute: gameplayService.CanUnpause)
                .DisposeWith(_subscriptions);
        }

        public object CurrentNavigationTarget
        {
            get => _currentNavigationTarget;
            private set => TrySetProperty(ref _currentNavigationTarget, value);
        }
        private object _currentNavigationTarget;

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

        public IActionCommand NavigateToInstructionsCommand { get; }

        public IActionCommand NavigateToSettingsCommand { get; }

        public IActionCommand PauseCommand { get; }

        public IActionCommand ResetCommand { get; }

        public TimeSpan Runtime
        {
            get => _runtime;
            private set => TrySetProperty(ref _runtime, value);
        }
        private TimeSpan _runtime;

        public IActionCommand StartCommand { get; }

        public IActionCommand UnpauseCommand { get; }

        public void Dispose()
            => _subscriptions.Dispose();

        private readonly IGameInstructionsViewModel _instructions;

        private readonly INavigationService _navigationService;

        private readonly IGameSettingsViewModel _settings;

        private readonly CompositeDisposable _subscriptions;
    }
}
