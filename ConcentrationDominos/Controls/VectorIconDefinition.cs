using System.Windows;
using System.Windows.Media;

namespace ConcentrationDominos.Controls
{
    public class VectorIconDefinition
        : DependencyObject
    {
        public Geometry Geometry
        {
            get => (Geometry)GetValue(GeometryProperty);
            set => SetValue(GeometryProperty, value);
        }
        private static readonly DependencyProperty GeometryProperty
            = DependencyProperty.Register(
                name: nameof(Geometry),
                propertyType: typeof(Geometry),
                ownerType: typeof(VectorIcon));

        public double NativeHeight
        {
            get => (double)GetValue(NativeHeightProperty);
            set => SetValue(NativeHeightProperty, value);
        }
        private static readonly DependencyProperty NativeHeightProperty
            = DependencyProperty.Register(
                name: nameof(NativeHeight),
                propertyType: typeof(double),
                ownerType: typeof(VectorIcon));

        public double NativeWidth
        {
            get => (double)GetValue(NativeWidthProperty);
            set => SetValue(NativeWidthProperty, value);
        }
        private static readonly DependencyProperty NativeWidthProperty
            = DependencyProperty.Register(
                name: nameof(NativeWidth),
                propertyType: typeof(double),
                ownerType: typeof(VectorIcon));
    }
}
