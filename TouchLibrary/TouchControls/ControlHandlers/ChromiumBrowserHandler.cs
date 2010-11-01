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
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using WebBrowser = Cjc.ChromiumBrowser.WebBrowser;

namespace TouchControls.ControlHandlers
{
    /// <summary>
    /// Implements custom behaviour for ListBoxes such as item selection and scrolling.
    /// </summary>
    public class ChromiumBrowserHandler : ElementHandler
    {
        public ChromiumBrowserHandler()
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
            var c = Source as WebBrowser;
            if (c == null) return;
            var x = Int32.Parse(Math.Ceiling(relative.X).ToString());
            var y = Int32.Parse(Math.Ceiling(relative.Y).ToString());
            if (TapCount == 2)
            {
                TapCount = 0;
                c.RaiseMouseMove(x, y);
                c.RaiseMouseClick();
            }
            
        }

        public override void TouchDown(PointF global, PointF relative)
        {
            var c = Source as WebBrowser;
            if (c == null) return;
            var x = Int32.Parse(Math.Ceiling(relative.X).ToString());
            var y = Int32.Parse(Math.Ceiling(relative.Y).ToString());
            c.RaiseMouseMove(x, y);
            Thread.Sleep(150);
            c.RaiseMouseDown();
           // base.TouchDown(global, relative);
        }

        public override void TouchUp(PointF global, PointF relative)
        {
            var c = Source as WebBrowser;
            if (c == null) return;
            var x = Int32.Parse(Math.Ceiling(relative.X).ToString());
            var y = Int32.Parse(Math.Ceiling(relative.Y).ToString());
            c.RaiseMouseMove(x, y);
            c.RaiseMouseUp();
            //base.TouchUp(global, relative);
        }

        public override void Scroll(float x, float y)
        {
            var c = Source as WebBrowser;
            if (c == null) return;
            c.InjectMouseWheel(Int32.Parse(Math.Ceiling(y).ToString()));
            //base.Scroll(x, y);
        }
    }
}


