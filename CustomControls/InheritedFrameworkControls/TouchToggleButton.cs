using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Infrasturcture.TouchLibrary;

namespace CustomControls.InheritedFrameworkControls
{
    public class TouchToggleButton : ToggleButton, IMTouchControl
    {
        public IMTContainer Container { get; set; }
    }
}