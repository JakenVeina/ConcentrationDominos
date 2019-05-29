namespace System.Reactive.Disposables
{
    public static class DisposableExtensions
    {
        public static T DisposeWith<T>(this T disposable, CompositeDisposable disposables)
            where T : IDisposable
        {
            if (disposable == null)
                throw new ArgumentNullException(nameof(disposable));

            if (disposables is null)
                throw new ArgumentNullException(nameof(disposables));

            disposables.Add(disposable);

            return disposable;
        }
    }
}
