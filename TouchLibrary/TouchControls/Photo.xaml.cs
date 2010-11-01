/*
TouchFramework connects touch tracking from a tracking engine to WPF controls 
allow scaling, rotation, movement and other multi-touch behaviours.

Copyright 2009 - Mindstorm Limited (reg. 05071596)

Author - Simon Lerpiniere

This file is part of TouchFramework.

TouchFramework is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

TouchFramework is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser Public License for more details.

You should have received a copy of the GNU Lesser Public License
along with TouchFramework.  If not, see <http://www.gnu.org/licenses/>.

If you have any questions regarding this library, or would like to purchase 
a commercial licence, please contact Mindstorm via www.mindstorm.com.
*/


using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Infrasturcture.TouchLibrary;
using TouchFramework;

namespace TouchControls
{
    /// <summary>
    /// Interaction logic for Photo.xaml
    /// </summary>
    public partial class Photo : UserControl
    {

        public Ellipse MenuBtn;
        public int Id;
        //public FrameworkControl framework;
        public IMTContainer container;

        public Photo()
        {
            InitializeComponent();
        }

        public Image SetPicture(string path)
        {
            BitmapSource bi = new BitmapImage(new Uri(path));

            double aspectRatio = bi.Width / bi.Height;
            //Uncomment this to enable automatic resize of pictures
            //bi = Photo.GenerateBitmapSource(bi, 500 * aspectRatio, 500);
            image2.Source = bi;
            bi.Freeze();
           //// MenuBtn = this.MenuButton;
            this.Width = bi.Width;
            this.Height = bi.Height;
            //this.MenuButton.AddHandler(MTEvents.TouchDownEvent, new RoutedEventHandler(CloseObject));
            return image2;
        }

        void CloseObject(object sender, RoutedEventArgs e)
        {
            //String helloworld = new String[8];
            int mynumber = new int();
            //framework.UnregisterElement(Id);

        }

        public static BitmapSource GenerateBitmapSource(ImageSource img, double renderWidth, double renderHeight)
        {
            var dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawImage(img, new Rect(0, 0, renderWidth, renderHeight));
            }
            var bmp = new RenderTargetBitmap((int)renderWidth, (int)renderHeight, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(dv);

      
           
            return bmp;
        }
    }
}
