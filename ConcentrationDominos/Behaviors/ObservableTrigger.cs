using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Interactivity;

namespace ConcentrationDominos.Behaviors
{
    /// <summary>
    /// Describes the base implementation of a trigger, which listens for events upon an <see cref="IObservable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of observable value.</typeparam>
    public abstract class ObservableTrigger<T>
        : TriggerBase<DependencyObject>
    {
        /// <summary>
        /// The source <see cref="IObservable{T}"/> whose values will be passed to actions owned by this trigger.
        /// </summary>
        public abstract IObservable<T> Source { get; set; }

        /// <summary>
        /// A <see cref="DependencyPropertyChangedEventHandler"/> to be attached to a backing <see cref="DependencyProperty"/> for <see cref="Source"/>.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> whose <see cref="Source"/> value has changed.</param>
        /// <param name="e">The set of args describing the change in value of <see cref="Source"/>.</param>
        internal protected static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = d as ObservableTrigger<T>;

            @this._subscription?.Dispose();
            @this._subscription = @this.Source?.Subscribe(x => @this.InvokeActions(x));
        }

        private IDisposable _subscription;
    }
}
