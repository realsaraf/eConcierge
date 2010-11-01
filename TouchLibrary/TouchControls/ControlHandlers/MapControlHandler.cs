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
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using CustomControls.MapControl;
using TouchFramework.ControlHandlers;
using Point = System.Windows.Point;

namespace TouchControls.ControlHandlers
{
    /// <summary>
    /// Implements custom behaviour for selecting and scrolling a textbox.
    /// </summary>
    public class MapControlHandler : ElementHandler
    {
        public MapControlHandler()
        {
            Timer.Interval = 2000;
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        void Timer_Tick(object sender, System.EventArgs e)
        {
            TapCount = 0;
        }
        private Timer Timer = new Timer();
        private int _tapCount;
        public int TapCount
        {
            get
            {
                return _tapCount;
            } 
            set
            {
                _tapCount = value;
            }
        }
        public override void Tap(PointF global, PointF relative)
        {
            TapCount++;
            var mapControl = Source as MapControl;
            if (mapControl == null) return;
            var control = mapControl.InputHitTest(new Point(relative.X, relative.Y)) as FrameworkElement;
            while (control != null && !(control is Viewbox) && control.Tag == null)
            {
                control = control.Parent as FrameworkElement;
            }
            if (control != null && !(control is Viewbox) && control.Tag.ToString().Contains("MapStyle"))
            {
                mapControl.ChangeMapStyle(control.Tag.ToString().Replace("MapStyle",""));
            }
            else
            {
                while (control != null && !(control is Viewbox))
                {
                    control = control.Parent as FrameworkElement;
                }
                if((control is Viewbox) && TapCount==2)
                {
                    TapCount = 0;
                    mapControl.Map.DoMapZoom(300,new Point(relative.X,relative.Y));
                }
            }
        }

        public override void TouchDown(PointF global, PointF relative)
        {
            var mapControl = Source as MapControl;
            if (mapControl == null) return;
            var control = mapControl.InputHitTest(new Point(relative.X, relative.Y)) as FrameworkElement;
            while (control != null && !(control is Viewbox))
            {
                control = control.Parent as FrameworkElement;
            }
            if (control is Viewbox)
            {
                base.TouchDown(global, relative);
            }
        }

        public override void TouchUp(PointF global, PointF relative)
        {
            var mapControl = Source as MapControl;
            if (mapControl == null) return;
            var control = mapControl.InputHitTest(new Point(relative.X, relative.Y)) as FrameworkElement;
            while (control != null && !(control is Viewbox))
            {
                control = control.Parent as FrameworkElement;
            }
            if (control is Viewbox)
            {
                base.TouchUp(global, relative);
            }
        }

        public override void Drag(PointF global, PointF relative)
        {
            var mapControl = Source as MapControl;
            if (mapControl == null) return;
            var control = mapControl.InputHitTest(new Point(relative.X, relative.Y)) as FrameworkElement;
            while (control != null && !(control is Viewbox))
            {
                control = control.Parent as FrameworkElement;
            }
            if (control is Viewbox)
            {
                base.Drag(global, relative);
            }
        }
    }
}
