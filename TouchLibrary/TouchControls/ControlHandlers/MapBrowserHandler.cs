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

namespace TouchControls.ControlHandlers
{
    /// <summary>
    /// Implements custom behaviour for ListBoxes such as item selection and scrolling.
    /// </summary>
    public class MapBrowserHandler : ElementHandler
    {
        public override void Tap(PointF global, PointF relative)
        {
            var mapBrowser = Source as MapBrowser;
            if (!mapBrowser.IsLocked) return;
            var chromBrowser = mapBrowser.ChromBrowser;
            var x = Int32.Parse(Math.Ceiling(relative.X).ToString());
            var y = Int32.Parse(Math.Ceiling(relative.Y).ToString());
            chromBrowser.RaiseMouseMove(x, y);
            chromBrowser.RaiseMouseClick();
        }

        public override void TouchDown(PointF global, PointF relative)
        {
            var mapBrowser = Source as MapBrowser;
            if (!mapBrowser.IsLocked) return;
            mapBrowser.TouchPoint = relative;
            var c = mapBrowser.ChromBrowser;
            var x = Int32.Parse(Math.Ceiling(relative.X).ToString());
            var y = Int32.Parse(Math.Ceiling(relative.Y).ToString());
            c.RaiseMouseMove(x, y);
            Thread.Sleep(150);
            c.RaiseMouseDown();
        }

        public override void TouchUp(PointF global, PointF relative)
        {
            var mapBrowser = Source as MapBrowser;
            if (!mapBrowser.IsLocked) return;
            var chromBrowser = mapBrowser.ChromBrowser;
            var x = Int32.Parse(Math.Ceiling(relative.X).ToString());
            var y = Int32.Parse(Math.Ceiling(relative.Y).ToString());
            chromBrowser.RaiseMouseMove(x, y);
            chromBrowser.RaiseMouseUp();
        }

        public override void Slide(float x, float y)
        {
            var mapBrowser = Source as MapBrowser;
            if (!mapBrowser.IsLocked) return;
            if (mapBrowser.Container.ObjectTouches.CurrTouchCount == 1 && mapBrowser.Container.ObjectTouches.PrevTouchCount == 1)
            {
                var chromBrowser = mapBrowser.ChromBrowser;
                if (chromBrowser == null) return;
                var newX = mapBrowser.TouchPoint.X + x;
                var newY = mapBrowser.TouchPoint.Y + y;
                mapBrowser.TouchPoint = new PointF(newX, newY);
                var xx = Int32.Parse(Math.Ceiling(newX).ToString());
                var yy = Int32.Parse(Math.Ceiling(newY).ToString());
                mapBrowser.CancelScale = true;
                chromBrowser.RaiseMouseMove(xx, yy);
            }
            base.Slide(x, y);
        }
    }
}


