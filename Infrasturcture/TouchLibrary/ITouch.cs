using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Infrasturcture.TouchLibrary
{
    public interface ITouch
    {
        PointF TouchPoint { get; set; }
        ITouchProperties Properties { get; set; }
        bool IsMoving { get; }
        int TouchId { get; set; }
    }
}
