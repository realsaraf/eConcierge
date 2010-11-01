using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Helpers.Converters
{
    public class PathToImageSourceConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return new BitmapImage(new Uri(value.ToString(), UriKind.Relative));
        }
    }
}