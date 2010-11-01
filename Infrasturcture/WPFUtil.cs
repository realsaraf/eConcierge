using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Infrasturcture
{
    public static class WpfUtil
    {
        public static BitmapImage BytesToImageSource(Byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        public static byte[] GetImage(int i, string path)
        {
            var fs = new FileStream(string.Format(path, i), FileMode.Open, FileAccess.Read);
            var imageData = new byte[fs.Length];
            fs.Read(imageData, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            return imageData;
        }

        public static ImageSource GetImageSource(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }
    }
}
