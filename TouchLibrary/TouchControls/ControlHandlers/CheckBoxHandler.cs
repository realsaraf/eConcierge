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

using System.Drawing;
using System.Windows.Controls;

namespace TouchControls.ControlHandlers
{
    /// <summary>
    /// Custom logic for handling multi-touch checkbox interation.
    /// Basically just switched the IsChecked when the Checkbox is tapped.
    /// </summary>
    public class CheckBoxHandler : ElementHandler
    {
        public override void Tap(PointF global, PointF relative)
        {
            var c = this.Source as CheckBox;
            if (c == null) return;

            // IsChecked is nullable, so this is the most readable way to handle
            if (c.IsChecked == true)
            {
                c.IsChecked = false;
            }
            else
            {
                c.IsChecked = true;
            }

            base.Tap(global, relative);
        }
    }
}
