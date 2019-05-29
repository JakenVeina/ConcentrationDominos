using System.Reactive;

namespace System.Windows.Input
{
    public interface IActionCommand
        : ICommand
    {
        void Execute();

        new IObservableReadOnlyValue<bool> CanExecute { get; }
    }

    public class ActionCommand
        : IActionCommand,
            IDisposable
    {
        public ActionCommand(Action execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            CanExecute = ObservableValue.Create(true);
        }

        public ActionCommand(Action execute, IObservable<bool> canExecute)
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

        public ActionCommand(Action execute, IObservableReadOnlyValue<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            CanExecute = canExecute ?? ObservableValue.Create(true);

            _canExecuteSubscription = canExecute?.Subscribe(x => 
                CanExecuteChanged?.Invoke(this, EventArgs.Empty));
        }

        public IObservableReadOnlyValue<bool> CanExecute { get; }

        public event EventHandler CanExecuteChanged;

        bool ICommand.CanExecute(object parameter)
            => CanExecute.Value;

        public void Dispose()
            => _canExecuteSubscription?.Dispose();

        public void Execute()
            => _execute();
        private readonly Action _execute;

        void ICommand.Execute(object parameter)
            => _execute();

        private readonly IDisposable _canExecuteSubscription;
    }
}
