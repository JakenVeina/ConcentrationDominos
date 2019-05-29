namespace System.Reactive
{
    public interface ICachedObservable<T>
        : IObservable<T>,
            IDisposable
    {
        bool HasValue { get; }

        T Value { get; }
    }

    internal class CachedObservable<T>
        : ObservableBase<T>,
            ICachedObservable<T>
    {
        public CachedObservable(IObservable<T> source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            _registration = source.Subscribe(x =>
            {
                Value = x;
                HasValue = true;
            });

            _source = source;
        }

        public bool HasValue { get; private set; }
            = false;

        public T Value
        {
            get => HasValue
                ? _value
                : throw new InvalidOperationException("The cache is currently empty");
            private set => _value = value;
        }
        private T _value;

        public void Dispose()
            => _registration.Dispose();

        protected override IDisposable SubscribeCore(IObserver<T> observer)
            => _source.Subscribe(observer);

        private readonly IDisposable _registration;

        private readonly IObservable<T> _source;
    }
}
