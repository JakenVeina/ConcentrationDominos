using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using ConcentrationDominos.Models;

namespace ConcentrationDominos.ViewModels
{
    public interface IGameBoardViewModel
        : INotifyPropertyChanged,
            IDisposable
    {
        ushort Height { get; }

        bool IsInteractable { get; }

        ImmutableArray<IGameBoardTileViewModel> Tiles { get; }

        ushort Width { get; }
    }

    public class GameBoardViewModel
        : ViewModelBase,
            IGameBoardViewModel
    {
        public GameBoardViewModel(
            GameStateModel gameState,
            IServiceProvider serviceProvider)
        {
            _subscriptions = new CompositeDisposable();

            gameState.GameBoard
                .Select(x => x.Height)
                .Subscribe(x => Height = x)
                .DisposeWith(_subscriptions);

            gameState.State
                .Select(x => x == GameState.Running)
                .Subscribe(x => IsInteractable = x)
                .DisposeWith(_subscriptions);

            _tiles = ImmutableArray<IGameBoardTileViewModel>.Empty;
            gameState.GameBoard
                .Subscribe(x =>
                {
                    foreach (var tile in _tiles)
                        tile.Dispose();

                    Tiles = x.Tiles
                        .Select(y =>
                        {
                            var viewModel = serviceProvider.GetRequiredService<IGameBoardTileViewModel>();

                            viewModel.ColIndex = y.ColIndex;
                            viewModel.RowIndex = y.RowIndex;
    
                            return viewModel;
                        })
                        .ToImmutableArray();
                })
                .DisposeWith(_subscriptions);

            gameState.GameBoard
                .Select(x => x.Width)
                .Subscribe(x => Width = x)
                .DisposeWith(_subscriptions);
        }

        public ushort Height
        {
            get => _height;
            set => TrySetProperty(ref _height, value);
        }
        private ushort _height;

        public bool IsInteractable
        {
            get => _isInteractable;
            set => TrySetProperty(ref _isInteractable, value);
        }
        private bool _isInteractable;

        public ImmutableArray<IGameBoardTileViewModel> Tiles
        {
            get => _tiles;
            set => TrySetProperty(ref _tiles, value);
        }
        private ImmutableArray<IGameBoardTileViewModel> _tiles;

        public ushort Width
        {
            get => _width;
            set => TrySetProperty(ref _width, value);
        }
        private ushort _width;

        public void Dispose()
            => _subscriptions.Dispose();

        private readonly CompositeDisposable _subscriptions;
    }
}
