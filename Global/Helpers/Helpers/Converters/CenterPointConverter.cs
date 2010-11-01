using System;
using System.Windows;
using System.Windows.Data;

namespace Helpers.Converters
{
    public class CenterPointConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var center = (((double)value)) / 2;
            return new Point(center, center);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}