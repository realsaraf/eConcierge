using InfoStrat.VE;
using Infrasturcture.TouchLibrary;

namespace BingMapControl
{
    public class VEMapControl : VEMap, IMTouchControl
    {
        public IMTContainer Container { get; set; }

        public MapControl Parent { get; set; }
    }
}
