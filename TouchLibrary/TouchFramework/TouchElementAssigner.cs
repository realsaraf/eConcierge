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

using System.Windows;
using System.Drawing;
using Infrasturcture.TouchLibrary;

namespace TouchFramework
{
    /// <summary>
    /// Handles assignment of touches to elements based on the built-in WPF hittest logic.
    /// You can call functions of this class directly to wire in simulators into the WPF framework.
    /// </summary>
    public class TouchElementAssigner
    {
        public MTElementDictionary Elements = null;
        public TouchElementDictionary Touches = null;

        Ticker t;

        /// <summary>
        /// Constructor to create an assigner from a top-level parent which contains ALL the controls which will be assigned for.
        /// Hit test will only run for child controls of the parent object.
        /// </summary>
        /// <param name="UIParent">Parent UI object for which sub-controls will be evaluated for multi-touch.  This is often your Window or Canvas.</param>
        public TouchElementAssigner(FrameworkElement UIParent)
        {
            Elements = new MTElementDictionary(UIParent);
            Touches = new TouchElementDictionary();
            t = new Ticker(ref Elements);
        }

        public void TouchAdded(Touch t)
        {
            IMTContainer cont = Elements.HitTest(t.TouchPoint, true);
            if (cont == null) return;
            if (cont.IsBook() && !cont.IsHitTestVisible(t.TouchPoint))
            {
                var belowItemContainer = cont.GetBelowItem(t.TouchPoint, ++Elements.MaxZIndex);
                if (belowItemContainer != null)
                    cont = belowItemContainer;
                else
                    return;
            }
            cont.ObjectTouches.Add(t.TouchId, t);
            Touches.Add(t.TouchId, new TouchElement(t, cont));
        }

        public void TouchRemoved(int touchId)
        {
            // If touch is not related to an object, nothing we need to do here
            if (!Touches.ContainsKey(touchId)) return;

            IMTContainer cont = Touches[touchId].MTElement;
            cont.ObjectTouches.Remove(touchId);
            Touches.Remove(touchId);
        }

        public void TouchHeld(int touchId)
        {
            if (Touches.ContainsKey(touchId))
            {
                Touch curTouch = Touches[touchId].Touch;
                curTouch.SetHeld();
            }
        }

        public void TouchMoved(int touchId, PointF p, ITouchProperties prop)
        {
            if (Touches.ContainsKey(touchId))
            {
                Touch curTouch = Touches[touchId].Touch;
                curTouch.SetNewTouchPoint(p);
                curTouch.Properties = prop;
                curTouch.SetMoving();
            }
        }
    }
}
