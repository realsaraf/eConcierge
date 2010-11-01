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
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TouchControls.ControlHandlers;
using TouchFramework.Helpers;

namespace TouchFramework.ControlHandlers
{
    /// <summary>
    /// Implements custom behaviour for ListBoxes such as item selection and scrolling.
    /// </summary>
    public class ListBoxHandler : ElementHandler
    {
        public override void Scroll(float x, float y)
        {
            base.Scroll(x, y);
        }

        public override void Tap(PointF global, PointF relative)
        {
            ListBox c = Source as ListBox;
            if (c == null) return;

            ListBoxItem item = findSelectedItem(c, new System.Windows.Point(relative.X, relative.Y));
            if (item != null) selectItem(c, item);

            base.Tap(global, relative);
        }

        void selectItem(ListBox c, ListBoxItem item)
        {
            if (c.SelectionMode == SelectionMode.Single)
            {
                c.SelectedItem = item;
            }
            else
            {
                if (!item.IsSelected) c.SelectedItems.Add(item); else c.SelectedItems.Remove(item);
            }
        }

        ListBoxItem findSelectedItem(ListBox c, System.Windows.Point p)
        {
            Visual findFrom = c.InputHitTest(p) as Visual;
            return findFrom.FindParent<ListBoxItem>() as ListBoxItem;
        }
    }
}


