using System.Windows;
using System.Windows.Interactivity;

namespace ConcentrationDominos.Behaviors
{
    public class SetVisibilityAction
        : TriggerAction<UIElement>
    {
        public Visibility Value { get; set; }

        public bool ValueFromTrigger { get; set; }
            = false;

        protected override void Invoke(object parameter)
        {
            if (!(AssociatedObject is null))
                AssociatedObject.SetValue(UIElement.VisibilityProperty, ValueFromTrigger ? parameter : Value);
        }
    }
}
