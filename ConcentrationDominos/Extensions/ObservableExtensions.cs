using System.Reactive.Disposables;

namespace System.Reactive.Linq
{
    public static class ObservableExtensions
    {
        public static IObservable<T> SelectLatest<T>(this IObservable<IObservable<T>> sources)
            => Observable.Create<T>(observer =>
            {
                IDisposable sourceRegistration = null;
                var sourcesRegistration = sources
                    .Subscribe(x =>
                    {
                        sourceRegistration?.Dispose();
                        sourceRegistration = x.Subscribe(observer);
                    });

                return Disposable.Create(() =>
                {
                    sourceRegistration?.Dispose();
                    sourcesRegistration.Dispose();
                });
            });

        public static ICachedObservable<T> ToCached<T>(this IObservable<T> source)
            => new CachedObservable<T>(source);

        public static IDisposable PublishTo<T>(this IObservable<T> source, IObservableValue<T> value)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            return source.Subscribe(x => value.Value = x);
        }
    }
}
