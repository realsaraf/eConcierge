using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace TouchControls.ControlHandlers
{
   public class ScrollViewerHandler : ElementHandler
    {
        public override void Slide(float x, float y)
        {
            var c = this.Source as ScrollViewer;
            if (c == null) return;

            //double diff = c.Maximum - c.Minimum;
            //double m = c.Orientation == Orientation.Horizontal ? x : y;
            //double sz = c.Orientation == Orientation.Horizontal ? c.RenderSize.Width : c.RenderSize.Height;

            //double movePercent = ((double)m / sz) * 100;
            //double valMove = (diff / 100) * movePercent;

            //c.Value += valMove;
            c.ScrollToVerticalOffset(c.VerticalOffset + y);

            base.Slide(x, y);
        }
    }
}

