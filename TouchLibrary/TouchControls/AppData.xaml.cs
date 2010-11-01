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
using System.Windows.Media.Animation;
using Infrasturcture.TouchLibrary;

//Recompile using TouchExample;

//using TouchFramework.Helpers;

namespace TouchFramework.ControlHandlers
{
    /// <summary>
    /// Interaction logic for Photo.xaml
    /// </summary>
    public partial class AppData : UserControl
    {
        public String app_name;
        public String app_file;
        public String app_description;
        public String app_thumb;
        public String app_image;
        public String app_type;

        public Ellipse MenuBtn;
        public IMTContainer container;

        //Recompile  public AppLauncher MainWindow;

        public Image openbutton;
        public Image closebutton;




        public AppData()
        {
            InitializeComponent();

            BitmapSource openimage = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\images\\Open.png"));
            Open.Source = openimage;
            openimage.Freeze();
            openbutton = Open;
            //Open.AddHandler(MTEvents.TouchDownEvent, new RoutedEventHandler(ShutdownEvent));

            BitmapSource infoimage = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\images\\Info.png"));
            Info.Source = infoimage;
            infoimage.Freeze();

            BitmapSource closeimage = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\images\\Close.png"));
            Close.Source = closeimage;
            closeimage.Freeze();
            closebutton = Close;
 
        }

        public void UpdateData()
        {
            textBlock1.Text = app_description;

            if (app_type == "system")
            {
                Open.Visibility = System.Windows.Visibility.Hidden;
                Info.Visibility = System.Windows.Visibility.Hidden;
                Close.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public void RunApp()
        {
            //Recompile  MainWindow.RunApp(app_file);
           
  

        }

        public Image SetPicture(string path)
        {
            if (path != "")
            {

                BitmapSource bi = new BitmapImage(new Uri(path));

                double aspectRatio = bi.Width / bi.Height;
                //Uncomment this to enable automatic resize of pictures
                //bi = Photo.GenerateBitmapSource(bi, 500 * aspectRatio, 500);
                image2.Source = bi;
                bi.Freeze();
                //MenuBtn = this.MenuButton;
                //this.Width = bi.Width;
                // this.Height = bi.Height;
                // this.MenuButton.AddHandler(MTEvents.TouchDownEvent, new RoutedEventHandler(CloseObject));
                return image2;
            }
            return null;
        }

        void CloseObject(object sender, RoutedEventArgs e)
        {
            //String helloworld = new String[8];
            int mynumber = new int();

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

        private double DURATION = 0;
        private double DownMoveTo = 0;
        private double incrementby = 1;
        public void StartDownAnimation(double duration, double downmoveto, double delay, double fallposition)
        {
            const double initialScaleFactor = 0.05d;
            var random = new Random();
            double ypos = random.Next(100, 680);
            var AppAnimation = new DoubleAnimation();
            AppAnimation.From = 0;
            AppAnimation.To = ypos;
            AppAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            BeginAnimation(Canvas.TopProperty, AppAnimation);



            var zoom = new DoubleAnimation();
            zoom.From = initialScaleFactor;
            zoom.To = 1d;
            zoom.BeginTime = TimeSpan.FromMilliseconds(0);
            zoom.Duration = new Duration(TimeSpan.FromMilliseconds(1000));
            zoom.Completed += new EventHandler(zoom_Completed);
            var tt = new TranslateTransform();
            var st = new ScaleTransform();

            var group = new TransformGroup();
            group.Children.Add(st);
            group.Children.Add(tt);

            this.RenderTransform = group;
            st.BeginAnimation(ScaleTransform.ScaleXProperty, zoom);
            st.BeginAnimation(ScaleTransform.ScaleYProperty, zoom);
            
        }

        void zoom_Completed(object sender, EventArgs e)
        {
            this.container.Reset();
            this.container.StartX = (int)Canvas.GetLeft(this);
            this.container.StartY = (int)Canvas.GetTop(this);
        }
    }
}
