using System.Collections.Generic;
using System.Windows;
using System.Windows.Interactivity;

namespace ConcentrationDominos.Behaviors
{
    public class VisibilityController
        : Behavior<UIElement>
    {
        public bool? IsCollapsed
        {
            get => (bool?)GetValue(IsCollapsedProperty);
            set => SetValue(IsCollapsedProperty, value);
        }
        public static readonly DependencyProperty IsCollapsedProperty
            = DependencyProperty.Register(
                name: IsCollapsedPropertyName,
                propertyType: typeof(bool?),
                ownerType: typeof(VisibilityController),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null,
                    propertyChangedCallback: OnIsCollapsedChanged));
        private const string IsCollapsedPropertyName
            = "IsCollapsed";

        public bool? IsHidden
        {
            get => (bool?)GetValue(IsHiddenProperty);
            set => SetValue(IsHiddenProperty, value);
        }
        public static readonly DependencyProperty IsHiddenProperty
            = DependencyProperty.Register(
                name: IsHiddenPropertyName,
                propertyType: typeof(bool?),
                ownerType: typeof(VisibilityController),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null,
                    propertyChangedCallback: OnIsHiddenChanged));
        private const string IsHiddenPropertyName
            = "IsHidden";

        public bool? IsVisible
        {
            get => (bool?)GetValue(IsVisibleProperty);
            set => SetValue(IsVisibleProperty, value);
        }
        public static readonly DependencyProperty IsVisibleProperty
            = DependencyProperty.Register(
                name: IsVisiblePropertyName,
                propertyType: typeof(bool?),
                ownerType: typeof(VisibilityController),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null,
                    propertyChangedCallback: OnIsVisibleChanged));
        private const string IsVisiblePropertyName
            = "IsVisible";

        private static void OnIsCollapsedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => (d as VisibilityController).UpdateVisibility();

        private static void OnIsHiddenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => (d as VisibilityController).UpdateVisibility();

        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => (d as VisibilityController).UpdateVisibility();

        private void UpdateVisibility()
        {
            if(!(AssociatedObject is null))
            {
                if (_visibilityMap.TryGetValue((IsCollapsed, IsHidden, IsVisible), out var visibility))
                    AssociatedObject.Visibility = visibility;
                else
                    AssociatedObject.ClearValue(UIElement.VisibilityProperty);
            }
        }

        private static readonly Dictionary<(bool? isCollapsed, bool? isHidden, bool? isVisible), Visibility> _visibilityMap
            = new Dictionary<(bool? isCollapsed, bool? isHidden, bool? isVisible), Visibility>() {
                { (null,  null,  false), Visibility.Hidden  },
                { (null,  null,  true),  Visibility.Visible },
                { (null,  false, null),  Visibility.Visible },
                { (null,  false, true),  Visibility.Visible },
                { (null,  true,  null),  Visibility.Hidden },
                { (null,  true,  false), Visibility.Hidden },
                { (false, null,  null),  Visibility.Visible },
                { (false, null,  false), Visibility.Hidden },
                { (false, null,  true),  Visibility.Visible },
                { (false, false, null),  Visibility.Visible },
                { (false, false, true),  Visibility.Visible },
                { (false, true,  null),  Visibility.Hidden },
                { (false, true,  false), Visibility.Hidden },
                { (true,  null,  null),  Visibility.Collapsed },
                { (true,  null,  false), Visibility.Collapsed },
                { (true,  null,  true),  Visibility.Collapsed },
                { (true,  false, null),  Visibility.Collapsed },
                { (true,  false, false), Visibility.Collapsed },
                { (true,  false, true),  Visibility.Collapsed },
                { (true,  true,  null),  Visibility.Collapsed },
                { (true,  true,  false), Visibility.Collapsed }
            };
    }
}
