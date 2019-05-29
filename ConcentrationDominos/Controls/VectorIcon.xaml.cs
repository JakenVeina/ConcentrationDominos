using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConcentrationDominos.Controls
{
    public partial class VectorIcon
        : UserControl
    {
        public VectorIcon()
        {
            InitializeComponent();
        }

        public Brush Brush
        {
            get => (Brush)GetValue(BrushProperty);
            set => SetValue(BrushProperty, value);
        }
        private static readonly DependencyProperty BrushProperty
            = DependencyProperty.Register(
                name: nameof(Brush),
                propertyType: typeof(Brush),
                ownerType: typeof(VectorIcon));

        public VectorIconDefinition Definition
        {
            get => (VectorIconDefinition)GetValue(DefinitionProperty);
            set => SetValue(DefinitionProperty, value);
        }
        private static readonly DependencyProperty DefinitionProperty
            = DependencyProperty.Register(
                name: nameof(Definition),
                propertyType: typeof(VectorIconDefinition),
                ownerType: typeof(VectorIcon));
    }
}
