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
using System.Runtime.InteropServices;
using WpfKb.Controls;

namespace TouchControls.ControlHandlers
{
    /// <summary>
    /// Implements custom behaviour to handle the slide of a thumb on a slider.
    /// </summary>
    public class KeyboardHandler : ElementHandler
    {

        private const UInt32 WM_LBUTTONDOWN = 0x201;
        private const UInt32 WM_LBUTTONUP = 0x202;


        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;

        [DllImport("user32.dll")]
        private static extern void mouse_event(
        UInt32 dwFlags, // motion and click options
        UInt32 dx, // horizontal position or change
        UInt32 dy, // vertical position or change
        UInt32 dwData, // wheel movement
        IntPtr dwExtraInfo // application-defined information
        );


        public override void Slide(float x, float y)
        {
            FloatingTouchScreenKeyboard c = this.Source as FloatingTouchScreenKeyboard;
            if (c == null) return;
            PointF myPoint = new PointF();
            myPoint.X = x;
            myPoint.Y = y;
            //c.ManualMove(Round(myPoint));
            base.Slide(x, y);
        }


        //public override void TouchDown(PointF p)
        public override void TouchDown(PointF global, PointF relative)
        {
            FloatingTouchScreenKeyboard b = Source as FloatingTouchScreenKeyboard;
            if (b == null) return;
            b.IsKeyboardShown = true;
            SendMouseDown(Round(global));

            // PointF screenPos = new PointF();
            //screenPos.X = (int)Math.Round(loc.X * screenArea.Width) + screenArea.Left;
            // screenPos.Y = (int)Math.Round(loc.Y * screenArea.Width) + screenArea.Top;
            //return screenPos;
            //b.ManualDown(Round(p));
            //b.ManualDown(Round(relative));

            //base.TouchDown(p);
            base.TouchDown(global, relative);
        }

        public override void Drag(PointF global, PointF relative)
        {
            FloatingTouchScreenKeyboard b = Source as FloatingTouchScreenKeyboard;
            if (b == null) return;

            // b.ManualMove(Round(relative));

            base.Drag(global, relative);
        }

        public override void TouchUp(PointF global, PointF relative)
        {
            FloatingTouchScreenKeyboard b = Source as FloatingTouchScreenKeyboard;
            if (b == null) return;
            SendMouseUp(Round(global));
            //b.ManualUp(Round(p));
            base.TouchUp(global, relative);
        }

        // Convert a PointF object into a Point object using rounding conversion.
        public static System.Drawing.Point Round(PointF value)
        {
            return new System.Drawing.Point((int)(Math.Round(value.X)),
                             (int)(Math.Round(value.Y)));
        }

        public static void SendMouseClick(System.Drawing.Point location)
        {
            //this.Cursor = new Cursor(Cursor.Current.Handle);
            System.Windows.Forms.Cursor.Position = location;
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
        }

        public static void SendMouseDown(System.Drawing.Point location)
        {
            //this.Cursor = new Cursor(Cursor.Current.Handle);
            System.Windows.Forms.Cursor.Position = location;
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
        }

        public static void SendMouseUp(System.Drawing.Point location)
        {
            //this.Cursor = new Cursor(Cursor.Current.Handle);
            System.Windows.Forms.Cursor.Position = location;
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
        }



    }
}
