using System;
using System.Globalization;
using System.Windows.Data;

namespace Helpers.Converters
{
    public class MultiplyValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var multiplicant = Double.Parse(parameter.ToString());
            return ((double)value) * multiplicant;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
