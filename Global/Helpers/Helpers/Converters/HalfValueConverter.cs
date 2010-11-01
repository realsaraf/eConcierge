using System;
using System.Windows.Data;

namespace Helpers.Converters
{
	public class HalfValueConverter : IValueConverter
	{
	    #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter != null)
                return ((double) value)/double.Parse(parameter.ToString());
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}