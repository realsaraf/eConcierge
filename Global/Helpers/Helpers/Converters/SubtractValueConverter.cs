using System;
using System.Windows.Data;

namespace Helpers.Converters
{
	public class SubtractValueConverter : IValueConverter
	{
	    #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((double) value) - double.Parse(parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}