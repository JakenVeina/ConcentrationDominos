using System;
using System.Globalization;
using System.Windows.Data;
using ConcentrationDominos.Models;
using Humanizer;

namespace ConcentrationDominos.Formatting
{
    public class DominoSetHumanizer
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (value is null) || ((value as string) == string.Empty)
                ? null
                : ((DominoSetType)value).Humanize(LetterCasing.Title);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
