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
using Infrasturcture.TouchLibrary;

namespace TouchFramework
{
    /// <summary>
    /// Handles simple data about a touch including status, properties and ID
    /// </summary>
    public class Touch : ITouch
    {
        public int TouchId{ get; set;}
        bool _moving = false;

        /// <summary>
        /// Holds detailed information about this touch (e.g. Velocity).
        /// </summary>
        public ITouchProperties Properties{ get; set;}

        /// <summary>
        /// Constructor for the touch object.
        /// </summary>
        /// <param name="Id">Id of the touch.</param>
        /// <param name="point">Point of the touch location.</param>
        public Touch(int Id, PointF point)
        {
            TouchId = Id;
            TouchPoint = point;
        }

        public PointF TouchPoint
        {
            get;
            set;
        }

        public void SetMoving()
        {
            _moving = true;
        }
        public void SetHeld()
        {
            _moving = false;
        }

        public bool IsMoving
        {
            get
            {
                return _moving;
            }
        }
        
        /// <summary>
        /// Updates the touch point to a new one without messing up other internal variables.
        /// Use this function when a touch point has moved.
        /// </summary>
        /// <param name="p">New point of the touch location.</param>
        public void SetNewTouchPoint(PointF p)
        {
            TouchPoint = p;
        }
    }
}
