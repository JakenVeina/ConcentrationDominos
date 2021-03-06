﻿using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using ConcentrationDominos.Models;

namespace ConcentrationDominos.Gameplay
{
    public interface IGameplayService
    {
        IObservable<bool> CanPause { get; }

        IObservable<bool> CanReset { get; }

        IObservable<bool> CanStart { get; }

        IObservable<bool> CanUnpause { get; }

        Task FlipTileAsync(GameBoardTileModel tile, CancellationToken cancellationToken);

        IObservable<bool> ObserveCanFlipTile(IObservable<GameBoardTileModel> tile);

        void Pause();

        void Reset();

        void Start();

        void Unpause();
    }

    public class GameplayService
        : IGameplayService
    {
        public GameplayService(
            GameStateModel gameState,
            Random random)
        {
            _gameState = gameState;
            _random = random;

            CanPause = _gameState.State
                .Select(x => x == GameState.Running);

            CanReset = _gameState.State
                .Select(x => (x != GameState.Waiting) && (x != GameState.Idle));

            CanStart = _gameState.State
                .Select(x => x == GameState.Idle);

            CanUnpause = _gameState.State
                .Select(x => x == GameState.Paused);
        }

        public IObservable<bool> CanPause { get; }

        public IObservable<bool> CanReset { get; }

        public IObservable<bool> CanStart { get; }

        public IObservable<bool> CanUnpause { get; }

        public async Task FlipTileAsync(GameBoardTileModel tile, CancellationToken cancellationToken)
        {
            tile.IsFaceUp.Value = true;

            _gameState.State.Value = GameState.Waiting;
            await Task.Delay(_gameState.Settings.Value.MemoryInterval, cancellationToken);
            _gameState.State.Value = GameState.Running;

            var faceupTiles = _gameState.GameBoard.Value
                .Tiles
                .Where(x => !(x.Content.Value is null) && x.IsFaceUp.Value)
                .ToArray();

            if (faceupTiles.Length < 2)
                return;

            var firstDomino = faceupTiles[0].Content.Value;
            var secondDomino = faceupTiles[1].Content.Value;

            var totalValue = firstDomino.FirstSuit
                + firstDomino.SecondSuit
                + secondDomino.FirstSuit
                + secondDomino.SecondSuit;

            var targetValue = (2 * (int)_gameState.DominoSet.Value.Type);

            if (totalValue == targetValue)
            {
                foreach(var faceupTile in faceupTiles)
                    faceupTile.Content.Value = null;

                var remainingTiles = _gameState.GameBoard.Value
                        .Tiles
                        .Where(x => !(x.Content.Value is null))
                        .ToArray();

                if(remainingTiles.Length < 2)
                {
                    _gameState.State.Value = GameState.Completed;

                    foreach(var remainingTile in remainingTiles)
                        remainingTile.IsFaceUp.Value = true;
                }
            }
            else
            {
                foreach (var faceupTile in faceupTiles)
                    faceupTile.IsFaceUp.Value = false;
            }
        }

        public IObservable<bool> ObserveCanFlipTile(IObservable<GameBoardTileModel> tile)
            => Observable.CombineLatest(
                    _gameState.State,
                    tile,
                    (state, x) => (state, tile: x))
                .Select(x => (x.tile is null)
                    ? Observable.Return(false)
                    : Observable.CombineLatest(
                        x.tile.Content,
                        x.tile.IsFaceUp,
                        (content, isFaceUp) => (x.state == GameState.Running) && !(content is null) && !isFaceUp))
                .SelectLatest();

        public void Pause()
            => _gameState.State.Value = GameState.Paused;

        public void Reset()
        {
            _gameState.State.Value = GameState.Idle;

            var mappings = _gameState.GameBoard.Value
                .Tiles
                .Zip(_gameState.DominoSet.Value
                        .Dominos
                        .OrderBy(x => _random.Next()),
                    (tile, domino) => (tile, domino));

            foreach (var (tile, domino) in mappings)
            {
                tile.Content.Value = domino;
                tile.IsFaceUp.Value = false;
                tile.IsRotated.Value = (_random.Next() % 2) == 0;
            }
        }

        public void Start()
            => _gameState.State.Value = GameState.Running;

        public void Unpause()
            => _gameState.State.Value = GameState.Running;

        private readonly GameStateModel _gameState;

        private readonly Random _random;
    }
}
