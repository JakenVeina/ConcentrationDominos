using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using ConcentrationDominos.Models;

namespace ConcentrationDominos.Gameplay
{
    public interface INavigationService
    {
        IObservable<bool> CanNavigateTo { get; }

        IObservable<object> CurrentTarget { get; }

        void NavigateTo(object target);
    }

    public class NavigationService
        : INavigationService,
            IDisposable
    {
        public NavigationService(
            GameStateModel gameState)
        {
            _gameState = gameState;

            CanNavigateTo = gameState.State
                .Select(x => x != GameState.Waiting);

            _currentTarget = new BehaviorSubject<object>(null);
        }

        public IObservable<bool> CanNavigateTo { get; }

        public IObservable<object> CurrentTarget
            => _currentTarget;
        private readonly BehaviorSubject<object> _currentTarget;

        public void Dispose()
            => _currentTarget.Dispose();

        public void NavigateTo(object target)
        {
            if (_gameState.State.Value == GameState.Running)
                _gameState.State.Value = GameState.Paused;

            _currentTarget.OnNext(target);
        }

        private readonly GameStateModel _gameState;
    }
}
