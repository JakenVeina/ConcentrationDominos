namespace System.Reactive
{
    public interface IObservableReadOnlyValue<T>
        : IObservable<T>
    {
        T Value { get; }
    }
}
