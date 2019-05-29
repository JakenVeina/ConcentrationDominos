using System;
using System.Reactive;
using System.Windows;

namespace ConcentrationDominos.Behaviors
{
    public class ObservableUnitTrigger
        : ObservableTrigger<Unit>
    {
        public override IObservable<Unit> Source
        {
            get => (IObservable<Unit>) GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        public static readonly DependencyProperty SourceProperty
            = DependencyProperty.Register(
                name: nameof(Source),
                propertyType: typeof(IObservable<Unit>),
                ownerType: typeof(ObservableTrigger<Unit>),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null,
                    propertyChangedCallback: OnSourceChanged));
    }
}
