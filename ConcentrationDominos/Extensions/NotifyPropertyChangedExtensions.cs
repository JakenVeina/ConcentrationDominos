using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reflection;

namespace System.Reactive.Linq
{
    public static class NotifyPropertyChangedExtensions
    {
        public static IObservable<TProperty> ObserveProperty<TOwner, TProperty>(this TOwner owner, Expression<Func<TOwner, TProperty>> propertyExpression)
            where TOwner : INotifyPropertyChanged
        {
            if (owner is null)
                throw new ArgumentNullException(nameof(owner));

            if (propertyExpression is null)
                throw new ArgumentNullException(nameof(propertyExpression));

            if (!(propertyExpression.Body is MemberExpression memberExpression)
                || (memberExpression.Member.MemberType != MemberTypes.Property))
                throw new ArgumentException($"Expression {propertyExpression.ToString()} does not describe a property upon {typeof(TOwner).FullName}", nameof(propertyExpression));

            var propertyGetter = propertyExpression.Compile();

            return Observable.Create<TProperty>(observer =>
            {
                if (observer is null)
                    throw new ArgumentNullException(nameof(observer));

                observer.OnNext(propertyGetter.Invoke(owner));

                var handler = new PropertyChangedEventHandler((sender, e) =>
                {
                    if (e.PropertyName == memberExpression.Member.Name)
                        observer.OnNext(propertyGetter.Invoke(owner));
                });

                owner.PropertyChanged += handler;

                return Disposable.Create(() => owner.PropertyChanged -= handler);
            });
        }

        public static IObservable<TProperty> WhenPropertyChanged<TOwner, TProperty>(this TOwner owner, Expression<Func<TOwner, TProperty>> propertyExpression)
            where TOwner : INotifyPropertyChanged
        {
            if (owner is null)
                throw new ArgumentNullException(nameof(owner));

            if (propertyExpression is null)
                throw new ArgumentNullException(nameof(propertyExpression));

            if (!(propertyExpression.Body is MemberExpression memberExpression)
                || (memberExpression.Member.MemberType != MemberTypes.Property))
                throw new ArgumentException($"Expression {propertyExpression.ToString()} does not describe a property upon {typeof(TOwner).FullName}", nameof(propertyExpression));

            var propertyGetter = propertyExpression.Compile();

            return Observable.Create<TProperty>(observer =>
            {
                if (observer is null)
                    throw new ArgumentNullException(nameof(observer));

                var handler = new PropertyChangedEventHandler((sender, e) =>
                {
                    if (e.PropertyName == memberExpression.Member.Name)
                        observer.OnNext(propertyGetter.Invoke(owner));
                });

                owner.PropertyChanged += handler;

                return Disposable.Create(() => owner.PropertyChanged -= handler);
            });
        }
    }
}
