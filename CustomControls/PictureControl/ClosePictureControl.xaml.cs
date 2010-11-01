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
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Infrasturcture;
using Infrasturcture.TouchLibrary;
using UserControl = System.Windows.Controls.UserControl;

namespace CustomControls.PictureControl
{
    /// <summary>
    /// Interaction logic for VideoControl.xaml
    /// </summary>
    public partial class ClosePictureControl : UserControl, IMTouchControl
    {
        Image _image = new Image();
        private Timer timer = new Timer();
        private IFrameworkManger _frameworkManager;
        public event EventHandler Closed;
        #region Throw Events

        public void Close()
        {
            //timer.Start();
            //Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate()
            //{
            //    var sb = FindResource("ClosePictureStoryBoard") as Storyboard;
            //    sb.Begin();
            //    sb.Completed += SbCompleted;
            //});
            _frameworkManager.UnRegisterElement(closeButton);
            _frameworkManager.RemoveControl(this);
            if (Closed != null)
                Closed(this, new EventArgs());
        }

        void TimerTick(object sender, EventArgs e)
        {
            timer.Stop();
            if (Closed != null)
                Closed(this, new EventArgs());
        }

        void SbCompleted(object sender, EventArgs e)
        {
            _frameworkManager.UnRegisterElement(closeButton);
            _frameworkManager.RemoveControl(this);
            if (Closed != null)
                Closed(this, new EventArgs());
        }
        #endregion

        public ClosePictureControl()
        {
            InitializeComponent();
            closeButton.Click += CloseButtonClick;
            timer.Interval = 3000;
            timer.Tick += TimerTick;
        }

        void CloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        public string FilePath { get; set; }
        public int Id { get; set; }
        public IMTContainer Container { get; set; }

        public void Load(IFrameworkManger frameworkManager, double left, double top)
        {
            _frameworkManager = frameworkManager;
            _frameworkManager.RegisterElement((IMTouchControl)closeButton, false, new TouchAction[] { TouchAction.Tap });
            _frameworkManager.AddControlWithAllGestures(this, left, top);
        }

        public Rectangle SetImage(string path)
        {
            FilePath = path;
            BitmapSource bi = new BitmapImage(new Uri(path, UriKind.Relative));
            double aspectRatio = bi.Height / bi.Width;
            _image.Source = bi;
            Brush brush = new ImageBrush(_image.Source);
            rectangle.Fill = brush;
            rectangle.Height = this.Width * aspectRatio;
            RenderOptions.SetCachingHint(brush, CachingHint.Cache);
            RenderOptions.SetCacheInvalidationThresholdMinimum(brush, 0.5);
            RenderOptions.SetCacheInvalidationThresholdMaximum(brush, 2.0);
            return rectangle;
        }
        public Rectangle SetImage(byte[] imageData)
        {
            BitmapImage bi = WpfUtil.BytesToImageSource(imageData);
            double aspectRatio = bi.Height / bi.Width;
            _image.Source = bi;
            Brush brush = new ImageBrush(_image.Source);
            rectangle.Fill = brush;
            rectangle.Height = this.Width * aspectRatio;
            RenderOptions.SetCachingHint(brush, CachingHint.Cache);
            RenderOptions.SetCacheInvalidationThresholdMinimum(brush, 0.5);
            RenderOptions.SetCacheInvalidationThresholdMaximum(brush, 2.0);
            return rectangle;
        }
    }
}
