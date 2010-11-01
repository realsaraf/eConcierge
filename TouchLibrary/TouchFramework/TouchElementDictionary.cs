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
using TouchFramework.Containers;

namespace TouchFramework
{
    /// <summary>
    /// Stores the relationship between touches and elements from a touch point of view.
    /// Allows you to find out which element any specific touch is related to.
    /// </summary>
    public class TouchElementDictionary : Dictionary<int, TouchElement>
    {
        public new void Add(int touchId, TouchElement element)
        {
            if (!ContainsKey(touchId))
                base.Add(touchId, element);
        }

        public void Remove(MTContainer element)
        {
            int[] vals = (from n in this
                          where n.Value.MTElement == element
                          select n.Key).ToArray();

            foreach (int i in vals) this.Remove(i);
        }

        public new TouchElement this[int touchId]
        {
            get
            {
                return base[touchId];
            }
            set
            {
                base[touchId] = value;
            }
        }
    }
}
