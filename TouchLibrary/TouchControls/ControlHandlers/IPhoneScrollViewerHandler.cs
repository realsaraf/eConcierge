using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace TouchControls.ControlHandlers
{
   public class IPhoneScrollViewerHandler : ElementHandler
    {
        public override void Slide(float x, float y)
        {
            var c = this.Source as ScrollViewer;
            if (c == null) return;
            c.ScrollToVerticalOffset(c.VerticalOffset - y);
            c.ScrollToHorizontalOffset(c.HorizontalOffset - x);
            base.Slide(x, y);
        }
    }
}

