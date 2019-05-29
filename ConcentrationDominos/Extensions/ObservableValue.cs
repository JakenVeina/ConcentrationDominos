using System.Reactive.Subjects;

namespace System.Reactive
{
    public interface IObservableValue<T>
        : IObservableReadOnlyValue<T>
    {
        new T Value { get; set; }
    }

    public static class ObservableValue
    {
        public static ObservableValue<T> Create<T>(T initialValue = default)
            => new ObservableValue<T>(initialValue);
    }

    public class ObservableValue<T>
        : IObservableValue<T>
    {
        public ObservableValue(T initialValue = default)
        {
            _subject = new BehaviorSubject<T>(initialValue);
        }

        public T Value
        {
            get => _subject.Value;
            set => _subject.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<T> observer)
            => _subject.Subscribe(observer);

        private readonly BehaviorSubject<T> _subject;
    }
}
