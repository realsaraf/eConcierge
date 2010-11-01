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
using System.Windows;
using System.Windows.Input;
using System.Drawing;

using TouchFramework;
using TouchFramework.Config;

namespace TouchFramework.Tracking
{
    public class MouseTracking : FrameworkControl
    {
        PointF old;
        int moveTid = 5000;
        int redPoints = 10000;

        MouseConfiguration mouseConfig = null;

        public MouseTracking(FrameworkElement uiParent)
        {
            this.Owner = uiParent;
        }

        public override void ConfigureFramework(FrameworkConfiguration config)
        {
            if (!(config is MouseConfiguration)) throw new ArgumentException("Config must be MouseConfiguration");
            mouseConfig = config as MouseConfiguration;

            base.ConfigureFramework(config);
        }

        public override void Start()
        {
            mouseConfig.EventWindow.MouseMove += new MouseEventHandler(EventWindow_MouseMove);
            mouseConfig.EventWindow.MouseLeftButtonDown += new MouseButtonEventHandler(EventWindow_MouseLeftButtonDown);
            mouseConfig.EventWindow.MouseLeftButtonUp += new MouseButtonEventHandler(EventWindow_MouseLeftButtonUp);
            mouseConfig.EventWindow.MouseRightButtonDown += new MouseButtonEventHandler(EventWindow_MouseRightButtonDown);
        }
        
        void EventWindow_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            redPoints++;
            System.Windows.Point pos = e.GetPosition(mouseConfig.EventWindow);
            PointF p = new PointF((float)pos.X, (float)pos.Y);
            int id = redPoints;

            Touch t = new Touch(id, p);

            this.TouchAdded(t);
            this.ProcessUpdates();
        }

        void EventWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int id = moveTid;

            this.TouchRemoved(id);
            this.ProcessUpdates();

            // As this is not called every frame, simulate the frame after
            System.Threading.Thread.Sleep(10);
            this.ProcessUpdates();
        }

        void EventWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point pos = e.GetPosition(mouseConfig.EventWindow);
            PointF p = new PointF((float)pos.X, (float)pos.Y);
            
            moveTid++;
            int id = moveTid;

            old = p;

            var prop=new TouchProperties();
            prop.Acceleration = 1.0;
            prop.VelocityX = 1.0;
            prop.VelocityY = 1.0;

            Touch t = new Touch(id, p);
            t.Properties = prop;
            t.SetHeld();

            this.TouchAdded(t);
            this.ProcessUpdates();
        }

        void EventWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (!(e.LeftButton == MouseButtonState.Pressed)) return;

            System.Windows.Point pos = e.GetPosition(mouseConfig.EventWindow);
            PointF p = new PointF((float)pos.X, (float)pos.Y);

            int id = moveTid;

            var prop=new TouchProperties();
            prop.Acceleration = 1.0;
            prop.VelocityX = old.X / p.X;
            prop.VelocityY = old.Y / p.Y;
            if (double.IsNaN(prop.VelocityX)) prop.VelocityX = 0.0;
            if (double.IsNaN(prop.VelocityY)) prop.VelocityY = 0.0;

            if (p != old)
            {
                this.TouchUpdated(id, p, prop);
            }
            else
            {
                this.TouchHeld(id);
            }
            this.ProcessUpdates();

            old = p;
        }

        public override void Stop()
        {
            mouseConfig.EventWindow.MouseMove -= new MouseEventHandler(EventWindow_MouseMove);
            mouseConfig.EventWindow.MouseLeftButtonDown -= new MouseButtonEventHandler(EventWindow_MouseLeftButtonDown);
            mouseConfig.EventWindow.MouseLeftButtonUp -= new MouseButtonEventHandler(EventWindow_MouseLeftButtonUp);
            mouseConfig.EventWindow.MouseRightButtonDown -= new MouseButtonEventHandler(EventWindow_MouseRightButtonDown);
        }

        public override void ForceRefresh()
        {
            this.RemoveAllPoints();
            this.ProcessUpdates();
        }
    }
}
