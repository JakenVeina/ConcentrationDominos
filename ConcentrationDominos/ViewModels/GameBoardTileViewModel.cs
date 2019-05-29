using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;

using ConcentrationDominos.Models;
using ConcentrationDominos.Gameplay;

namespace ConcentrationDominos.ViewModels
{
    public interface IGameBoardTileViewModel
        : INotifyPropertyChanged,
            IDisposable
    {
        ushort ColIndex { get; set; }

        ushort? FirstSuit { get; }

        bool IsEmpty { get; }

        bool? IsFaceUp { get; }

        ushort RowIndex { get; set; }

        ushort? SecondSuit { get; }

        IAsyncActionCommand FlipCommand { get; }
    }

    public class GameBoardTileViewModel
        : ViewModelBase,
            IGameBoardTileViewModel
    {
        public GameBoardTileViewModel(
            IGameplayService gameplayService,
            GameStateModel gameState)
        {
            _subscriptions = new CompositeDisposable();

            var cachedModel = Observable.CombineLatest(
                    this.ObserveProperty(x => x.ColIndex),
                    this.ObserveProperty(x => x.RowIndex),
                    gameState.GameBoard,
                    (colIndex, rowIndex, gameBoard) => gameBoard
                        .Tiles
                        .FirstOrDefault(x => (x.ColIndex == colIndex) && (x.RowIndex == rowIndex)))
                .ToCached()
                .DisposeWith(_subscriptions);

            var modelContent = cachedModel
                .Select(x => x?.Content
                    ?? Observable.Return<DominoModel>(null))
                .SelectLatest();

            Observable.CombineLatest(
                    modelContent
                        .Select(x => x?.FirstSuit),
                    modelContent
                        .Select(x => x?.SecondSuit),
                    cachedModel
                        .Select(x => x?.IsRotated),
                    (firstSuit, secondSuit, isRotated) => (firstSuit, secondSuit, isRotated))
                .Subscribe(x =>
                {
                    if(!(x.isRotated is null))
                    {
                        if(x.isRotated.Value)
                        {
                            FirstSuit = x.secondSuit;
                            SecondSuit = x.firstSuit;
                        }
                        else
                        {
                            FirstSuit = x.firstSuit;
                            SecondSuit = x.secondSuit;
                        }
                    }
                })
                .DisposeWith(_subscriptions);

            modelContent
                .Select(x => x is null)
                .Subscribe(x => IsEmpty = x)
                .DisposeWith(_subscriptions);

            cachedModel
                .Select(x => x?.IsFaceUp?.Select(y => (bool?)y)
                    ?? Observable.Return<bool?>(null))
                .SelectLatest()
                .Subscribe(x => IsFaceUp = x)
                .DisposeWith(_subscriptions);

            FlipCommand = new AsyncActionCommand(
                    cancellationToken => gameplayService.FlipTileAsync(cachedModel.Value, cancellationToken),
                    gameplayService.ObserveCanFlipTile(cachedModel))
                .DisposeWith(_subscriptions);
        }

        public ushort ColIndex
        {
            get => _colIndex;
            set => TrySetProperty(ref _colIndex, value);
        }
        private ushort _colIndex;

        public ushort? FirstSuit
        {
            get => _firstSuit;
            private set => TrySetProperty(ref _firstSuit, value);
        }
        private ushort? _firstSuit;

        public bool IsEmpty
        {
            get => _isEmpty;
            private set => TrySetProperty(ref _isEmpty, value);
        }
        private bool _isEmpty;

        public bool? IsFaceUp
        {
            get => _isFaceUp;
            private set => TrySetProperty(ref _isFaceUp, value);
        }
        private bool? _isFaceUp;

        public ushort RowIndex
        {
            get => _rowIndex;
            set => TrySetProperty(ref _rowIndex, value);
        }
        private ushort _rowIndex;

        public ushort? SecondSuit
        {
            get => _secondSuit;
            private set => TrySetProperty(ref _secondSuit, value);
        }
        private ushort? _secondSuit;

        public IAsyncActionCommand FlipCommand { get; }

        public void Dispose()
            => _subscriptions.Dispose();

        private readonly CompositeDisposable _subscriptions;
    }
}
