using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace System.Windows.Input
{
    public interface IAsyncActionCommand
        : ICommand
    {
        Task ExecuteAsync(CancellationToken token);

        new IObservableReadOnlyValue<bool> CanExecute { get; }
    }

    public class AsyncActionCommand
        : IAsyncActionCommand,
            IDisposable
    {
        public AsyncActionCommand(Func<CancellationToken, Task> executeAsync)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            CanExecute = ObservableValue.Create(true);
        }

        public AsyncActionCommand(Func<CancellationToken, Task> executeAsync, IObservable<bool> canExecute)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            if (canExecute is null)
                throw new ArgumentNullException(nameof(canExecute));

            var canExecuteValue = ObservableValue.Create(false);

            CanExecute = canExecuteValue;

            _canExecuteSubscription = canExecute?.Subscribe(x =>
            {
                canExecuteValue.Value = x;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            });
        }

        public AsyncActionCommand(Func<CancellationToken, Task> executeAsync, IObservableReadOnlyValue<bool> canExecute)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            CanExecute = canExecute ?? ObservableValue.Create(true);

            _canExecuteSubscription = canExecute?.Subscribe(x =>
                CanExecuteChanged?.Invoke(this, EventArgs.Empty));
        }

        public IObservableReadOnlyValue<bool> CanExecute { get; }

        public event EventHandler CanExecuteChanged;

        public void Dispose()
            => _canExecuteSubscription.Dispose();

        void ICommand.Execute(object parameter)
            => _executeAsync.Invoke(CancellationToken.None);

        public Task ExecuteAsync(CancellationToken token)
            => _executeAsync.Invoke(token);
        private readonly Func<CancellationToken, Task> _executeAsync;

        bool ICommand.CanExecute(object parameter)
            => CanExecute.Value;

        private readonly IDisposable _canExecuteSubscription;
    }
}
