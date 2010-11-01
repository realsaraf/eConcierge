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
using TouchControls.ControlHandlers;


namespace TouchFramework.ControlHandlers
{
    /// <summary>
    /// Implements custom behaviour to handle the slide of a thumb on a slider.
    /// </summary>
    public class AppCategoriesHandler : ElementHandler
    {
        public override void Slide(float x, float y)
        {
            AppCategories c = this.Source as AppCategories;
            if (c == null) return;
            PointF myPoint = new PointF();
            myPoint.X = x;
            myPoint.Y = y;
           // c.ManualMove(Round(myPoint));
            base.Slide(x, y);
        }


        //public override void TouchDown(PointF p)
        public override void TouchDown(PointF global, PointF relative)
        {
            AppCategories b = Source as AppCategories;
            if (b == null) return;
            bool hittest = false;
            int Cat_Id = 0;
            //if (p.Y < 165)
            if (relative.Y < 165)
            {
                int catIncr = 0;
                for (int cat = 1; cat <= 7; cat++)
                {
                    catIncr += 185;
                   // catIncr = catIncr + 185;
                    //if (p.X < catIncr)
                    if (relative.X < catIncr)
                    {
                        Cat_Id = cat;
                        hittest = true;
                        break;
                    }

                }

                if (hittest == true)
                {
                    b.LoadCategory(Cat_Id);
                }

            }


               // PointF screenPos = new PointF();
            //screenPos.X = (int)Math.Round(loc.X * screenArea.Width) + screenArea.Left;
           // screenPos.Y = (int)Math.Round(loc.Y * screenArea.Width) + screenArea.Top;
            //return screenPos;
           // b.ManualDown(Round(p));
            base.TouchDown(global, relative);
            //base.TouchDown(p);
        }

        public override void Drag(PointF global, PointF relative)
        {
            AppCategories b = Source as AppCategories;
            if (b == null) return;

          //  b.ManualMove(Round(relative));

            base.Drag(global, relative);
        }

        public override void TouchUp(PointF global, PointF relative)
        {
            AppCategories b = Source as AppCategories;
            if (b == null) return;

            //b.ManualUp(Round(p));
            base.TouchUp(global, relative);
        }

        // Convert a PointF object into a Point object using rounding conversion.
        public static System.Windows.Point Round(PointF value)
        {
            return new System.Windows.Point((int)(Math.Round(value.X)),
                             (int)(Math.Round(value.Y)));
        }

        
    }
}
