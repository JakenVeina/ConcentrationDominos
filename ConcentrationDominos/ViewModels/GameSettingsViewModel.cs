using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;

using ConcentrationDominos.Gameplay;
using ConcentrationDominos.Models;

namespace ConcentrationDominos.ViewModels
{
    public interface IGameSettingsViewModel
        : INotifyPropertyChanged,
            IDisposable
    {
        DominoSetType DominoSetType { get; }

        IReadOnlyList<DominoSetType> DominoSetTypes { get; }

        TimeSpan MemoryInterval { get; }

        IReadOnlyList<TimeSpan> MemoryIntervals { get; }

        IActionCommand SaveCommand { get; }

        IActionCommand UndoCommand { get; }
    }

    public class GameSettingsViewModel
        : ViewModelBase,
            IGameSettingsViewModel
    {
        public GameSettingsViewModel(
            GameStateModel gameState,
            INavigationService navigationService)
        {
            _gameState = gameState;
            _navigationService = navigationService;

            _subscriptions = new CompositeDisposable();

            SaveCommand = new ActionCommand(
                    execute: () =>
                    {
                        _gameState.Settings.Value = new GameSettingsModel(
                            _dominoSetType,
                            _memoryInterval);

                        _navigationService.NavigateTo(null);
                    },
                    canExecute: Observable.CombineLatest(
                        _gameState.Settings,
                        this.ObserveProperty(x => x.DominoSetType),
                        this.ObserveProperty(x => x.MemoryInterval),
                        navigationService.CanNavigateTo,
                        (settings, dominoSetType, memoryInterval, canNavigateTo) =>
                            canNavigateTo
                                && ((dominoSetType != settings.DominoSetType)
                                    || (memoryInterval != settings.MemoryInterval))))
                .DisposeWith(_subscriptions);

            UndoCommand = new ActionCommand(
                    execute: () =>
                    {
                        ReloadSettings();

                        _navigationService.NavigateTo(null);
                    })
                .DisposeWith(_subscriptions);

            ReloadSettings();
        }

        public DominoSetType DominoSetType
        {
            get => _dominoSetType;
            set => TrySetProperty(ref _dominoSetType, value);
        }
        private DominoSetType _dominoSetType;

        public IReadOnlyList<DominoSetType> DominoSetTypes
            => _dominoSetTypes;
        private static readonly DominoSetType[] _dominoSetTypes
            = Enum.GetValues(typeof(DominoSetType))
                .Cast<DominoSetType>()
                .Where(x => x != DominoSetType.Empty)
                .OrderBy(x => (int)x)
                .ToArray();

        public TimeSpan MemoryInterval
        {
            get => _memoryInterval;
            set => TrySetProperty(ref _memoryInterval, value);
        }
        private TimeSpan _memoryInterval;

        public IReadOnlyList<TimeSpan> MemoryIntervals
            => _memoryIntervals;
        private static readonly TimeSpan[] _memoryIntervals
            = new[]
            {
                TimeSpan.Zero,
                TimeSpan.FromSeconds(0.5),
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(5)
            };

        public IActionCommand SaveCommand { get; }

        public IActionCommand UndoCommand { get; }

        public void Dispose()
            => _subscriptions.Dispose();

        private void ReloadSettings()
        {
            DominoSetType = _gameState.Settings.Value.DominoSetType;
            MemoryInterval = _gameState.Settings.Value.MemoryInterval;
        }

        private readonly GameStateModel _gameState;

        private readonly INavigationService _navigationService;

        private readonly CompositeDisposable _subscriptions;
    }
}
