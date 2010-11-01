using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfoStrat.VE;
using Infrasturcture.TouchLibrary;

namespace CustomControls.MapControl
{
    public class VEMapControl : VEMap, IMTouchControl
    {
        public IMTContainer Container { get; set; }

        public MapControl Parent { get; set; }

        public VEMapControl()
        {
        }
    }
}
