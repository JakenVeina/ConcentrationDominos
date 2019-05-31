using System;
using System.Collections.Generic;

using ConcentrationDominos.Models;

namespace ConcentrationDominos.Gameplay
{
    public class GameBuildingBehavior
        : IBehavior
    {
        public GameBuildingBehavior(
            IGameplayService gameplayService,
            GameStateModel gameState)
        {
            _gameplayService = gameplayService;
            _gameState = gameState;
        }

        public void Start()
        {
            _registration = _gameState.Settings
                .Subscribe(settings =>
                {
                    if (_gameState.DominoSet.Value.Type != settings.DominoSetType)
                    {
                        _gameState.DominoSet.Value = new DominoSetModel(settings.DominoSetType);

                        var gameBoardSize = _gameBoardSizeMap[settings.DominoSetType];

                        _gameState.GameBoard.Value = new GameBoardModel(gameBoardSize.width, gameBoardSize.height);

                        _gameplayService.Reset();
                    }
                });
        }

        public void Stop()
        {
            _registration.Dispose();
            _registration = null;
        }

        private static readonly Dictionary<DominoSetType, (ushort width, ushort height)> _gameBoardSizeMap
            = new Dictionary<DominoSetType, (ushort width, ushort height)>()
            {
                { DominoSetType.Empty,        (0,  0) },
                { DominoSetType.DoubleOne,    (3,  1) },
                { DominoSetType.DoubleTwo,    (3,  2) },
                { DominoSetType.DoubleThree,  (5,  2) },
                { DominoSetType.DoubleFour,   (5,  3) },
                { DominoSetType.DoubleFive,   (7,  3) },
                { DominoSetType.DoubleSix,    (7,  4) },
                { DominoSetType.DoubleSeven,  (9,  4) },
                { DominoSetType.DoubleEight,  (9,  5) },
                { DominoSetType.DoubleNine,   (11, 5) },
                { DominoSetType.DoubleTen,    (11, 6) },
                { DominoSetType.DoubleEleven, (13, 6) },
                { DominoSetType.DoubleTwelve, (13, 7) }
            };

        private readonly IGameplayService _gameplayService;

        private readonly GameStateModel _gameState;

        private IDisposable _registration;
    }
}
