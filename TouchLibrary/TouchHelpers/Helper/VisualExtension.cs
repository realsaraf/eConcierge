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
using System.Windows.Media;

namespace TouchFramework.Helpers
{
    /// <summary>
    /// Extends visuals with better searching functionality.
    /// </summary>
    public static class VisualExtensions
    {
        public static Visual FindChild<T>(this Visual myVisual)
        {
            Visual childVisual = null;
            int i = 0;
            while (i < VisualTreeHelper.GetChildrenCount(myVisual) && !(childVisual is T))
            {
                // Retrieve child visual at specified index value.
                childVisual = (Visual)VisualTreeHelper.GetChild(myVisual, i);
                // If it's not the one we want, start going through the children
                if (!(childVisual is T)) childVisual = FindChild<T>(childVisual);
                // Move on to the next index (after we've been through all the children!)
                i++;
            }
            // Return whatever we've got
            return childVisual;
        }

        public static Visual FindParent<T>(this Visual myVisual)
        {
            if (myVisual == null) return null;

            // Retrieve child visual at specified index value.
            Visual parentVisual = (Visual)VisualTreeHelper.GetParent(myVisual);
            
            // If it's not the one we want, start going through the children
            if (!(parentVisual is T)) parentVisual = FindParent<T>(parentVisual);
            
            // Return whatever we've got
            return parentVisual;
        }
    }
}
