using System;
using System.Globalization;
using System.Windows.Data;

using Humanizer;

namespace ConcentrationDominos.Formatting
{
    public class TimeSpanHumanizer
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ((TimeSpan?)value)?.Humanize();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
