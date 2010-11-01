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
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TouchControls.ControlHandlers;
using TouchFramework;
using TouchFramework.Helpers;



namespace TouchFramework.ControlHandlers
{
    /// <summary>
    /// Custom logic for handling multi-touch button interation
    /// </summary>
    public class AppDataHandler : ElementHandler
    {


        //public override void TouchDown(PointF p)
        public override void TouchDown(PointF global, PointF relative)
        {
            AppData s = Source as AppData;
            Canvas parent = s.Parent as Canvas;
            //if (p.X < 20 && p.Y < 20)
            if (relative.X > s.image2.Source.Width && relative.Y < 55)
            {
                s.RunApp();
            }
            else if (relative.X > s.image2.Source.Width && relative.Y > 150)
            {
                s.Visibility = Visibility.Hidden;
            }


            base.TouchDown(global, relative);

            //base.TouchDown(p);
        }

        public override void Drag(PointF global, PointF relative)
        {
    
            base.Drag(global, relative);
        }

        public override void TouchUp(PointF global, PointF relative)
        {

            base.TouchUp(global, relative);
        }
        

    }
}
