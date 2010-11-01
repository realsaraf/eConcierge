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
using _3dTile;

namespace TouchControls.ControlHandlers
{
    /// <summary>
    /// Implements custom behaviour to handle the slide of a thumb on a slider.
    /// </summary>
    public class FlipTile3DHandler : ElementHandler
    {
        private PointF _lastPosition;
        public override void Slide(float x, float y)
        {
            var c = this.Source as FlipTile3D;
            if (c == null) return;
            UpdateLastPosition(x,y);
            c.ManualMove(Round(_lastPosition));
            base.Slide(x, y);
        }

        private void UpdateLastPosition(float x, float y)
        {
            _lastPosition.X = _lastPosition.X + x;
            _lastPosition.Y = _lastPosition.Y + y;
        }

        public override void Tap(PointF global, PointF relative)
        {
            var b = Source as FlipTile3D;
            if (b == null) return;
            b.ManualDown(Round(relative));
            base.Tap(global, relative);
        }

        public override void TouchDown(PointF global, PointF relative)
        {
            var b = Source as FlipTile3D;
            if (b == null) return;
            _lastPosition = relative;
            base.TouchDown(global, relative);
        }
        
        public System.Windows.Point Round(PointF value)
        {
            return new System.Windows.Point((int)(Math.Round(value.X)),
                             (int)(Math.Round(value.Y)));
        }

        public Point RoundD(PointF value)
        {
            return new Point((int)(Math.Round(value.X)),
                             (int)(Math.Round(value.Y)));
        }
    }
}
