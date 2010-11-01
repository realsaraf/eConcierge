using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using CustomControls;

namespace mConciergeClient
{
    public class AnimationHelper
    {
        private static Canvas _canvas;

        public static void ParkHotelExplorer(bool restore, double width, Canvas canvas, double top, double left, double scale, double rotate)
        {
            _canvas = canvas;
            var hotelExplorer = HotelExplorer.GetInstance();
            hotelExplorer.IsFloating = !restore;
            hotelExplorer.HightLight(!restore);
            var ypos = restore ? top : Canvas.GetTop(hotelExplorer);
            var xpos = restore ? width - left - (hotelExplorer.ActualWidth * scale) : Canvas.GetLeft(hotelExplorer);
            var yTo = restore ? 55 : top;
            var xTo = restore ? 55 : width - left - (hotelExplorer.ActualWidth / scale);
            var zoomFrom = restore ? scale : 1;
            var zoomTo = restore ? 1 : scale;
            var rotateFrom = restore ? rotate : 0;
            var rotateTo = restore ? 0 : rotate;
            var yAnimation = new DoubleAnimation
                                 {
                                     From = ypos,
                                     To = yTo,
                                     Duration = new Duration(TimeSpan.FromMilliseconds(500))
                                 };
            var xAnimation = new DoubleAnimation
                                 {
                                     From = xpos,
                                     To = xTo,
                                     Duration = new Duration(TimeSpan.FromMilliseconds(1000))
                                 };

            hotelExplorer.BeginAnimation(Canvas.TopProperty, yAnimation);
            hotelExplorer.BeginAnimation(Canvas.LeftProperty, xAnimation);


            var zoom = new DoubleAnimation
                           {
                               From = zoomFrom,
                               To = zoomTo,
                               BeginTime = TimeSpan.FromMilliseconds(0),
                               Duration = new Duration(TimeSpan.FromMilliseconds(1000))
                           };

            var rotateAnimation = new DoubleAnimation
                             {
                                 From = rotateFrom,
                                 To = rotateTo,
                                 BeginTime = TimeSpan.FromMilliseconds(0),
                                 Duration = new Duration(TimeSpan.FromMilliseconds(1000))
                             };

            zoom.Completed += HotelZoomCompleted;

            var st = new ScaleTransform();
            var rt = new RotateTransform(rotateTo, 0, 0);

            var group = new TransformGroup();
            group.Children.Add(st);
            group.Children.Add(rt);

            hotelExplorer.Container.WorkingObject.RenderTransform = group;
            st.BeginAnimation(ScaleTransform.ScaleXProperty, zoom);
            st.BeginAnimation(ScaleTransform.ScaleYProperty, zoom);
            st.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
        }

        public static void ParkHotelExplorer(bool restore, double width, Canvas canvas, double pos = 40, double y = 40)
        {
            _canvas = canvas;
            var hotelExplorer = HotelExplorer.GetInstance();
            hotelExplorer.IsFloating = !restore;
            hotelExplorer.HightLight(!restore);
            var ypos = restore ? 40 : Canvas.GetTop(hotelExplorer);
            var xpos = restore ? width - 40 - (hotelExplorer.ActualWidth / 3) : Canvas.GetLeft(hotelExplorer);
            var yTo = restore ? 55 : 40;
            var xTo = restore ? 55 : width - 40 - (hotelExplorer.ActualWidth / 3);
            var zoomFrom = restore ? 0.40 : 1;
            var zoomTo = restore ? 1 : 0.40;
            var rotateFrom = restore ? 20 : 0;
            var rotateTo = restore ? 0 : 20;
            var yAnimation = new DoubleAnimation
            {
                From = ypos,
                To = yTo,
                Duration = new Duration(TimeSpan.FromMilliseconds(500))
            };
            var xAnimation = new DoubleAnimation
            {
                From = xpos,
                To = xTo,
                Duration = new Duration(TimeSpan.FromMilliseconds(1000))
            };

            hotelExplorer.BeginAnimation(Canvas.TopProperty, yAnimation);
            hotelExplorer.BeginAnimation(Canvas.LeftProperty, xAnimation);


            var zoom = new DoubleAnimation
            {
                From = zoomFrom,
                To = zoomTo,
                BeginTime = TimeSpan.FromMilliseconds(0),
                Duration = new Duration(TimeSpan.FromMilliseconds(1000))
            };

            var rotate = new DoubleAnimation
            {
                From = rotateFrom,
                To = rotateTo,
                BeginTime = TimeSpan.FromMilliseconds(0),
                Duration = new Duration(TimeSpan.FromMilliseconds(1000))
            };

            zoom.Completed += HotelZoomCompleted;

            var st = new ScaleTransform();
            var rt = new RotateTransform(rotateTo, 0, 0);

            var group = new TransformGroup();
            group.Children.Add(st);
            group.Children.Add(rt);

            hotelExplorer.RenderTransform = group;
            st.BeginAnimation(ScaleTransform.ScaleXProperty, zoom);
            st.BeginAnimation(ScaleTransform.ScaleYProperty, zoom);
            st.BeginAnimation(RotateTransform.AngleProperty, rotate);

        }

        static void HotelZoomCompleted(object sender, EventArgs e)
        {
            var hotelExplorer = HotelExplorer.GetInstance();
            hotelExplorer.Container.Reset();
            hotelExplorer.Container.StartX = (int)Canvas.GetLeft(hotelExplorer);
            hotelExplorer.Container.StartY = (int)Canvas.GetTop(hotelExplorer);
            if (hotelExplorer.Connector != null) return;
            if (hotelExplorer.IsFloating)
            {
                //hotelExplorer.Connector = ConnectControl(hotelExplorer, HotelVideoControl.GetInstance(), 0.33, 1);
            }
        }
    }
}