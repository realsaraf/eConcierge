using System.Windows.Controls;
using Infrasturcture.TouchLibrary;

namespace CustomControls.InheritedFrameworkControls
{
    public class TrackBar:Slider, IMTouchControl
    {
        public IMTContainer Container { get; set; }
    }
}