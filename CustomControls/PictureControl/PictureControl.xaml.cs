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
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Infrasturcture.TouchLibrary;

namespace CustomControls.PictureControl
{
    /// <summary>
    /// Interaction logic for VideoControl.xaml
    /// </summary>
    public partial class PictureControl : UserControl,IMTouchControl
    {
        Image _image = new Image();
        public MenuDiskControl.MenuDiskControl MenuDisk
        {
            get
            {
                return CommandDisk;
            }
        }

        #region Throw Events
        public event EventHandler CloseClick;
        public event EventHandler EditClick;
        public event EventHandler CopyClick;
        public event EventHandler EmailClick;
        public event EventHandler FacebookClick;
        public event EventHandler TwitterClick;

        protected virtual void OnCloseClick(EventArgs e)
        {
            CloseClick(this, e);
        }

        protected virtual void OnCopyClick(EventArgs e)
        {
            CopyClick(this, e);
        }

        protected virtual void OnEditClick(EventArgs e)
        {
            EditClick(this, e);
        }

        protected virtual void OnEmailClick(EventArgs e)
        {
            EmailClick(this, e);
        }

        protected virtual void OnFacebookClick(EventArgs e)
        {
            FacebookClick(this, e);
        }

        protected virtual void OnTwitterClick(EventArgs e)
        {
            TwitterClick(this, e);
        }
        private void RegisterDiskEvents()
        {
            CommandDisk.CloseClick += CommandDisk_CloseClick;
            CommandDisk.EditClick += CommandDisk_EditClick;
            CommandDisk.CopyClick += CommandDisk_CopyClick;
            CommandDisk.EmailClick += CommandDisk_EmailClick;
            CommandDisk.FacebookClick += CommandDisk_FacebookClick;
            CommandDisk.TwitterClick += CommandDisk_TwitterClick;
        }

        void CommandDisk_TwitterClick(object sender, EventArgs e)
        {
            if (TwitterClick != null)
                OnTwitterClick(e);
        }

        void CommandDisk_FacebookClick(object sender, EventArgs e)
        {
            if (FacebookClick != null)
                OnFacebookClick(e);
        }

        void CommandDisk_EmailClick(object sender, EventArgs e)
        {

            if (EmailClick != null)
                OnEmailClick(new EventArgs());
        }

        void CommandDisk_CopyClick(object sender, EventArgs e)
        {
            if (CopyClick != null)
                OnCopyClick(new EventArgs());
        }

        void CommandDisk_EditClick(object sender, EventArgs e)
        {
            if (EditClick != null)
                OnEditClick(new EventArgs());
        }

        void CommandDisk_CloseClick(object sender, EventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate()
            {
                var sb = FindResource("ClosePictureStoryBoard") as Storyboard;
                sb.Begin();
                sb.Completed += SbCompleted;
            });
        }

        void SbCompleted(object sender, EventArgs e)
        {
            if (CloseClick != null)
                OnCloseClick(new EventArgs());
        }
        #endregion

        public PictureControl()
        {
            InitializeComponent();
            RegisterDiskEvents();
        }

        public Rectangle SetImage(string path)
        {
            FilePath = path;
            MenuDisk.FilePath = path;
            MenuDisk.MediaType = MediaType.Photo;
            BitmapSource bi = new BitmapImage(new Uri(path, UriKind.Relative));
            double aspectRatio = bi.Height / bi.Width;
            _image.Source = bi;
            Brush brush = new ImageBrush(_image.Source);
            rectangle1.Fill = brush;
            rectangle1.Height = this.Width * aspectRatio;
            RenderOptions.SetCachingHint(brush, CachingHint.Cache);
            RenderOptions.SetCacheInvalidationThresholdMinimum(brush, 0.5);
            RenderOptions.SetCacheInvalidationThresholdMaximum(brush, 2.0);
            return rectangle1;
        }

        public string FilePath { get; set; }
        //public FrameworkControl framework { get; set; }

        public int Id { get; set; }

        //public MTSmoothContainer FacebookAuthenticationContainer { get; set; }

        public IMTContainer Container { get; set; }
    }
}
