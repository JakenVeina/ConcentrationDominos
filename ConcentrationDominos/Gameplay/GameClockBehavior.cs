using System;
using System.Diagnostics;
using System.Reactive.Linq;

using ConcentrationDominos.Models;

namespace ConcentrationDominos.Gameplay
{
    public class GameClockBehavior
        : IBehavior
    {
        public GameClockBehavior(
            GameStateModel gameState,
            ISystemClock systemClock)
        {
            _gameState = gameState;
            _runtimeStopwatch = new Stopwatch();
            _systemClock = systemClock;
        }

        public void Start()
        {
            _registration = Observable.CombineLatest(
                    _gameState.State,
                    _systemClock.UtcNow,
                    (state, utcNow) => state)
                .Subscribe(state =>
                {
                    switch(state)
                    {
                        case GameState.Idle:
                            _runtimeStopwatch.Reset();
                            break;

                        case GameState.Running:
                        case GameState.Waiting:
                            _runtimeStopwatch.Start();
                            break;

                        case GameState.Paused:
                        case GameState.Completed:
                            _runtimeStopwatch.Stop();
                            break;
                    }

                    _gameState.Runtime.Value = _runtimeStopwatch.Elapsed;
                });
        }

        public void Stop()
        {
            _registration.Dispose();
            _registration = null;
        }

        private readonly GameStateModel _gameState;

        private readonly Stopwatch _runtimeStopwatch;

        private readonly ISystemClock _systemClock;

        private IDisposable _registration;
    }
}
