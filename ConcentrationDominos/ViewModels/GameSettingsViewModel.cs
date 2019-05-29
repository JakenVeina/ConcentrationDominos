using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

using ConcentrationDominos.Gameplay;
using ConcentrationDominos.Models;

namespace ConcentrationDominos.ViewModels
{
    public interface IGameSettingsViewModel
        : INotifyPropertyChanged,
            IDisposable
    {
        IObservable<Unit> ChangesRequested { get; }

        DominoSetType DominoSetType { get; }

        IReadOnlyList<DominoSetType> DominoSetTypes { get; }

        TimeSpan MemoryInterval { get; }

        IReadOnlyList<TimeSpan> MemoryIntervals { get; }

        IObservable<Unit> OperationCompleted { get; }

        IActionCommand SaveCommand { get; }

        IActionCommand UndoCommand { get; }
    }

    public class GameSettingsViewModel
        : ViewModelBase,
            IGameSettingsViewModel
    {
        public GameSettingsViewModel(
            IGameplayService gameplayService,
            GameStateModel gameState)
        {
            _gameState = gameState;

            _subscriptions = new CompositeDisposable();

            ChangesRequested = gameplayService.SettingsChangeRequested;
            gameplayService.SettingsChangeRequested
                .Subscribe(x => ReloadSettings())
                .DisposeWith(_subscriptions);

            DominoSetTypes = Enum.GetValues(typeof(DominoSetType))
                .Cast<DominoSetType>()
                .Where(x => x != DominoSetType.Empty)
                .OrderBy(x => (int)x)
                .ToArray();

            MemoryIntervals = new[]
            {
                TimeSpan.Zero,
                TimeSpan.FromSeconds(0.5),
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(5)
            };

            _operationCompleted = new Subject<Unit>();

            SaveCommand = new ActionCommand(
                    execute: () =>
                    {
                        _gameState.Settings.Value = new GameSettingsModel(
                            _dominoSetType,
                            _memoryInterval);
                        _operationCompleted.OnNext(Unit.Default);
                    },
                    canExecute: Observable.CombineLatest(
                        _gameState.Settings,
                        this.ObserveProperty(x => x.DominoSetType),
                        this.ObserveProperty(x => x.MemoryInterval),
                        (settings, dominoSetType, memoryInterval) =>
                            (dominoSetType != settings.DominoSetType)
                            || (memoryInterval != settings.MemoryInterval)))
                .DisposeWith(_subscriptions);

            UndoCommand = new ActionCommand(
                    execute: () =>
                    {
                        ReloadSettings();
                        _operationCompleted.OnNext(Unit.Default);
                    })
                .DisposeWith(_subscriptions);
        }

        public IObservable<Unit> ChangesRequested { get; }

        public DominoSetType DominoSetType
        {
            get => _dominoSetType;
            set => TrySetProperty(ref _dominoSetType, value);
        }
        private DominoSetType _dominoSetType;

        public IReadOnlyList<DominoSetType> DominoSetTypes { get; }

        public TimeSpan MemoryInterval
        {
            get => _memoryInterval;
            set => TrySetProperty(ref _memoryInterval, value);
        }
        private TimeSpan _memoryInterval;

        public IObservable<Unit> OperationCompleted
            => _operationCompleted;
        private readonly Subject<Unit> _operationCompleted;

        public IReadOnlyList<TimeSpan> MemoryIntervals { get; }

        public IActionCommand SaveCommand { get; }

        public IActionCommand UndoCommand { get; }

        public void Dispose()
        {
            _subscriptions.Dispose();
            _operationCompleted.Dispose();
        }

        private void ReloadSettings()
        {
            DominoSetType = _gameState.Settings.Value.DominoSetType;
            MemoryInterval = _gameState.Settings.Value.MemoryInterval;
        }

        private readonly GameStateModel _gameState;

        private readonly CompositeDisposable _subscriptions;
    }
}
