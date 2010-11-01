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
using System.Text;

namespace TouchFramework.Helpers
{
    /// <summary>
    /// Some basic math functions.
    /// </summary>
    public static class ExtMaths
    {
        public static float RadiansToDegrees(float radians)
        {
            float res = radians * (float)(180 / Math.PI);
            return res;
        }
        public static float DegreesToRadians(float degrees)
        {
            return degrees * (float)(Math.PI / 180);
        }
        public static float GetHypotenuse(float a, float b)
        {
            return (float)Math.Sqrt(a * a + b * b);
        }
    }
}
