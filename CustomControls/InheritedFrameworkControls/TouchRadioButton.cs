using System.Windows.Controls;
using Infrasturcture.TouchLibrary;

namespace CustomControls.InheritedFrameworkControls
{
    public class TouchRadioButton : RadioButton, IMTouchControl
    {
        public IMTContainer Container { get; set; }
    }
}