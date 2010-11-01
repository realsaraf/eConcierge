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

using System.Drawing;
using System.Windows;
using CustomControls.BasicVideoControl;

namespace TouchControls.ControlHandlers
{
    /// <summary>
    /// Custom logic for handling multi-touch button interation
    /// </summary>
    public class VideoControlHandler : ElementHandler
    {
        public override void Tap(PointF global, PointF relative)
        {
            var c = Source as VideoControl;
            if (c == null) return;
            var control = c.InputHitTest(new System.Windows.Point(relative.X, relative.Y));
            if (control != null)
            {
                if(!c.MenuDisk.RaiseMouseClick((FrameworkElement) control))
                    c.PlayVideo();
            }
            base.Tap(global, relative);
        }

        public override void TouchDown(PointF global, PointF relative)
        {
            var c = Source as VideoControl;
            if (c == null) return;
            var control = c.InputHitTest(new System.Windows.Point(relative.X, relative.Y));
            if (control != null)
            {
                c.MenuDisk.RaiseMouseMove((FrameworkElement)control);
                c.MenuDisk.RaiseMouseDown((FrameworkElement)control);
            }
            base.TouchDown(global, relative);
        }

        public override void TouchUp(PointF global, PointF relative)
        {
            var c = Source as VideoControl;
            if (c == null) return;
            var control = c.InputHitTest(new System.Windows.Point(relative.X, relative.Y));
            if (control != null)
            {
                c.MenuDisk.RaiseMouseMove((FrameworkElement)control);
                c.MenuDisk.RaiseMouseUp((FrameworkElement)control);
            }
            base.TouchDown(global, relative);
        }
    }
}
