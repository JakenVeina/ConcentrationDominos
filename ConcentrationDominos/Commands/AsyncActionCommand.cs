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
        public AsyncActionCommand(Func<CancellationToken, Task> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            CanExecute = ObservableValue.Create(true);
        }

        public AsyncActionCommand(Func<CancellationToken, Task> execute, IObservable<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
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

        public AsyncActionCommand(Func<CancellationToken, Task> execute, IObservableReadOnlyValue<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            CanExecute = canExecute ?? ObservableValue.Create(true);

            _canExecuteSubscription = canExecute?.Subscribe(x =>
                CanExecuteChanged?.Invoke(this, EventArgs.Empty));
        }

        public IObservableReadOnlyValue<bool> CanExecute { get; }

        public event EventHandler CanExecuteChanged;

        public void Dispose()
            => _canExecuteSubscription.Dispose();

        void ICommand.Execute(object parameter)
            => _execute.Invoke(CancellationToken.None);

        public Task ExecuteAsync(CancellationToken token)
            => _execute.Invoke(token);
        private readonly Func<CancellationToken, Task> _execute;

        bool ICommand.CanExecute(object parameter)
            => CanExecute.Value;

        private readonly IDisposable _canExecuteSubscription;
    }
}
