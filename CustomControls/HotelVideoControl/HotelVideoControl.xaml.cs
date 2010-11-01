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
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Windows.Threading;
using CustomControls.InheritedFrameworkControls;
using Infrasturcture.TouchLibrary;
using Brush = System.Windows.Media.Brush;
using Image = System.Windows.Controls.Image;
using Rectangle = System.Windows.Shapes.Rectangle;
using Timer = System.Windows.Forms.Timer;
using UserControl = System.Windows.Controls.UserControl;

namespace CustomControls.HotelVideoControl
{
    /// <summary>
    /// Interaction logic for VideoControl.xaml
    /// </summary>
    public partial class HotelVideoControl : UserControl, IMTouchControl
    {
        Timer timer = new Timer();
        TimeSpan oldPosition;
        public event MediaOpenedHandler OnPositionChanged;
        public delegate void MediaOpenedHandler(double milliseconds);
        MediaPlayer player = new MediaPlayer();
        DrawingVisual videoVisual = new DrawingVisual();
        bool playing = false;
        bool inPreview = false;
        bool loaded = false;
        double dispHeight = 0.0;
        double dispWidth = 0.0;
        public IMTContainer Container { get; set; }
        private bool AutoChange;
        public IMTouchControl TrackBar
        {
            get
            {
                return Slider;
            }
            set
            {
                Slider = value as TrackBar;
            }
        }
        public event EventHandler Closed;
        private IFrameworkManger _frameworkManager;

        public HotelVideoControl()
        {
            player.MediaOpened += player_MediaOpened;
            player.MediaEnded += player_MediaEnded;
            InitializeComponent();
            timer.Interval = 1;
            timer.Tick += timer_Tick;
            rectangle1.MouseDown += Rectangle1MouseDown;
            Slider.GotMouseCapture += Slider_GotMouseCapture;
            Slider.LostMouseCapture += Slider_LostMouseCapture;
            closeButton.Click += CloseButtonClick;
        }

        public void Load(IFrameworkManger frameworkManager, double left, double top)
        {
            _frameworkManager = frameworkManager;
            _frameworkManager.RegisterElement(TrackBar, false, new[] { TouchAction.Slide, TouchAction.Tap });
            _frameworkManager.RegisterElement((IMTouchControl)closeButton, false, new[] { TouchAction.Tap });
            _frameworkManager.AddControlWithAllGestures(this, left, top);
        }

        void SbCompleted(object sender, EventArgs e)
        {
            Cleanup();
            _frameworkManager.UnRegisterElement(TrackBar);
            _frameworkManager.UnRegisterElement(closeButton);
            _frameworkManager.RemoveControl(this);
            if(Closed!=null)
                Closed(this,new EventArgs());
        }

        private void Cleanup()
        {
            player.Close();
        }

        void CloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        public void Close()
        {
            Cleanup();
            _frameworkManager.UnRegisterElement(TrackBar);
            _frameworkManager.UnRegisterElement(closeButton);
            _frameworkManager.RemoveControl(this);
            if (Closed != null)
                Closed(this, new EventArgs());
        }
        
        public bool IsSliderTouched(PointF relative)
        {
            return true;
        }

        void Slider_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UnTouchSlider();
        }

        public void UnTouchSlider()
        {
            IsSliderCaptured = false;
            AutoChange = false;
            SeekTo(Slider.Value);
        }

        void Slider_GotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TouchSlider();
        }

        public void TouchSlider()
        {
            IsSliderCaptured = true;
        }

        void Rectangle1MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PlayVideo();
        }

        public Rectangle SetVideo(string path)
        {
            player.Open(new Uri(path, UriKind.RelativeOrAbsolute));
            return rectangle1;
        }

        public void PlayVideo()
        {
            if (!loaded) return;
            if (inPreview) { renderVideo(); inPreview = false; }
            playing = !playing;
            if (playing)
            {
                timer.Start();
                this.oldPosition = this.player.Position;
                player.Play();
            }
            else
            {
                this.timer.Stop();
                player.Pause();
            }
        }

        bool IsSliderCaptured = false;

        void timer_Tick(object sender, EventArgs e)
        {
            if (this.player.Position != this.oldPosition && !IsSliderCaptured)
            {
                AutoChange = true;
                Slider.Value = player.Position.TotalMilliseconds/player.NaturalDuration.TimeSpan.TotalMilliseconds;
                AutoChange = false;
            }
                
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!AutoChange && !IsSliderCaptured)
            {
                SeekTo(e.NewValue);
            }
        }

        private void SeekTo(double milliseconds)
        {
            int postion = (int)(player.NaturalDuration.TimeSpan.TotalMilliseconds * milliseconds);
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, postion);
            this.player.Position = ts;
        }

        void player_MediaOpened(object sender, EventArgs e)
        {
            initVideo();
        }

        void player_MediaEnded(object sender, EventArgs e)
        {
            playing = false;
            initVideo();
        }

        void initVideo()
        {
            calcDisplay();
            rectangle1.Height = dispHeight;

            startPreviewPlayer();

            BackgroundWorker wk = new BackgroundWorker();
            wk.DoWork += wk_DoWork;
            wk.RunWorkerCompleted += wk_RunWorkerCompleted;
            wk.RunWorkerAsync();
        }

        void wk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            renderPreview();
            stopPlayer();

            loaded = true;
            inPreview = true;
        }

        void wk_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
        }

        void renderVideo()
        {
            DrawingContext context = videoVisual.RenderOpen();
            context.DrawVideo(player, new Rect(0, 0, dispWidth, dispHeight));
            context.Close();

            Brush brush = new VisualBrush(videoVisual);
            rectangle1.Fill = brush;

            RenderOptions.SetCachingHint(brush, CachingHint.Cache);
            RenderOptions.SetCacheInvalidationThresholdMinimum(brush, 0.5);
            RenderOptions.SetCacheInvalidationThresholdMaximum(brush, 2.0);
        }

        void renderPreview()
        {
            BitmapImage playIcon = getPlayIcon();
            Rect iconPos = calcIconCenterPosition(playIcon);

            DrawingContext context = videoVisual.RenderOpen();
            context.DrawVideo(player, new Rect(0, 0, dispWidth, dispHeight));
            context.DrawImage(playIcon, iconPos);
            context.Close();

            RenderTargetBitmap target = new RenderTargetBitmap((int)dispWidth, (int)dispHeight, 1 / 100, 1 / 100, PixelFormats.Pbgra32);
            target.Render(videoVisual);
            BitmapFrame frame = BitmapFrame.Create(target).GetAsFrozen() as BitmapFrame;

            Image img = new Image();
            img.Source = frame;
            Brush brush = new VisualBrush(img);
            rectangle1.Fill = brush;
        }

        void stopPlayer()
        {
            player.Stop();
        }

        void startPreviewPlayer()
        {
            player.Position = TimeSpan.FromSeconds(1);
            player.Play();
        }

        BitmapImage getPlayIcon()
        {
            BitmapImage bi = new BitmapImage(new Uri(@"pack://application:,,,/TouchControls;component/play.png"));
            return bi;
        }

        Rect calcIconCenterPosition(BitmapImage icon)
        {
            double xpos = (dispWidth - icon.Width) / 2;
            double ypos = (dispHeight - icon.Height) / 2;
            return new Rect(xpos, ypos, icon.Width, icon.Height);
        }
        
        void calcDisplay()
        {
            double width = Convert.ToDouble(player.NaturalVideoWidth);
            double height = Convert.ToDouble(player.NaturalVideoHeight);

            double ratio = height / width;

            dispHeight = rectangle1.Width * ratio;
            dispWidth = rectangle1.Width;
        }
    }
}
