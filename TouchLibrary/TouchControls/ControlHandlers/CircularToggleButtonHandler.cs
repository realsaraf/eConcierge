using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CustomControls.CircularButton;
using TouchFramework.ControlHandlers;

namespace TouchControls.ControlHandlers
{
    public class CircularToggleButtonHandler : ElementHandler
    {
        public override void Tap(PointF global, PointF relative)
        {
            var c = Source as CircularToggleButton;
            if (c == null) return;
            c.IsChecked = !c.IsChecked;
            base.Tap(global, relative);
        }
    }
}
