using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Interactivity;

namespace ConcentrationDominos.Behaviors
{
    public abstract class ObservableTrigger<T>
        : TriggerBase<DependencyObject>
    {
        public abstract IObservable<T> Source { get; set; }

        internal protected static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = d as ObservableTrigger<T>;

            @this._subscription?.Dispose();
            @this._subscription = @this.Source?.Subscribe(x => @this.InvokeActions(x));
        }

        private IDisposable _subscription;
    }
}
