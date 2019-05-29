using System;
using System.Diagnostics;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

using ConcentrationDominos.Models;

namespace ConcentrationDominos.Gameplay
{
    public interface ISystemClock
    {
        IObservableReadOnlyValue<DateTimeOffset> UtcNow { get; }
    }

    public class SystemClockBehavior
        : ISystemClock,
            IBehavior            
    {
        public SystemClockBehavior(
            GameStateModel gameState)
        {
            _gameState = gameState;

            _utcNow = new ObservableValue<DateTimeOffset>(DateTimeOffset.Now);
        }

        public IObservableReadOnlyValue<DateTimeOffset> UtcNow
            => _utcNow;
        private ObservableValue<DateTimeOffset> _utcNow;

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            RunAsync(_cancellationTokenSource.Token);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

        private async void RunAsync(CancellationToken cancellationToken)
        {
            var stopwatch = new Stopwatch();

            while (true)
            {
                stopwatch.Restart();

                _utcNow.Value = DateTimeOffset.UtcNow;

                stopwatch.Stop();

                var remaining = _gameState.UpdateInterval - stopwatch.Elapsed;
                if (remaining > TimeSpan.Zero)
                    await Task.Delay(remaining, cancellationToken);
                else
                    await Task.Yield();
            }
        }

        private CancellationTokenSource _cancellationTokenSource;

        private readonly GameStateModel _gameState;
    }
}
