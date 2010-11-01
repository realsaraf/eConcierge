using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace TouchControls.ControlHandlers
{
    public class ComboBoxHandler : ElementHandler
    {
        public override void Tap(PointF global, PointF relative)
        {
            ComboBox c = Source as ComboBox;
            if (c == null) return;

            //ListBoxItem item = findSelectedItem(c, new System.Windows.Point(relative.X, relative.Y));
            //if (item != null) selectItem(c, item);
            c.IsDropDownOpen = !c.IsDropDownOpen; 

            base.Tap(global, relative);
        }
    }
}
