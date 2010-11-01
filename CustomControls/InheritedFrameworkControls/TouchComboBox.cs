using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Infrasturcture.TouchLibrary;

namespace CustomControls.InheritedFrameworkControls
{
    public class TouchComboBox : ComboBox, IMTouchControl
    {
        public IMTContainer Container { get; set; }
    }
}
