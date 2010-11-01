using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchHelpers.Helper;
using System.Drawing;

namespace TouchHelpers.Events
{
    public class PointFEventArgs : EventArgs
    {
        public PointF Location { get; set; }
    }
}
