using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using CustomControls.HotelVideoControl;
using TouchFramework.ControlHandlers;
using TouchFramework.Helpers;

namespace TouchControls.ControlHandlers
{
    public class TrackBarHandler : ElementHandler
    {
        public override void Slide(float x, float y)
        {
            Slider c = this.Source as Slider;
            if (c == null) return;

            double diff = c.Maximum - c.Minimum;
            double m = c.Orientation == Orientation.Horizontal ? x : y;
            double sz = c.Orientation == Orientation.Horizontal ? c.RenderSize.Width : c.RenderSize.Height;

            double movePercent = ((double)m / sz) * 100;
            double valMove = (diff / 100) * movePercent;

            c.Value += valMove;

            base.Slide(x, y);
        }

        public override void TouchDown(System.Drawing.PointF global, System.Drawing.PointF relative)
        {
            var videoControl = Source.FindParent<HotelVideoControl>() as HotelVideoControl;
            if (videoControl == null) return;
            videoControl.TouchSlider();
            base.TouchDown(global, relative);
        }

        public override void TouchUp(System.Drawing.PointF global, System.Drawing.PointF relative)
        {
            var videoControl = Source.FindParent<HotelVideoControl>() as HotelVideoControl;
            if (videoControl == null) return;
            videoControl.UnTouchSlider();
            base.TouchUp(global, relative);
        }
    }
}
