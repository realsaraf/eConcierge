using System;
using System.Windows;
using System.Windows.Controls;
using CustomControls.InheritedFrameworkControls;
using Infrasturcture.TouchLibrary;

namespace CustomControls.CalendarControl
{
    /// <summary>
    /// Interaction logic for DayItem.xaml
    /// </summary>
    public partial class CalendarDayItem : UserControl, IMTouchControl
    {
        public event EventHandler Click;
        public IMTContainer Container { get; set; }

        public CalendarDayItem(double fontRatio)
        {
            InitializeComponent();
            FontRatio = fontRatio;
            DayButton.Click += DayButton_Click;
        }

        void DayButton_Click(object sender, RoutedEventArgs e)
        {
            if (Click != null)
                Click(sender, e);
        }

        public TouchRadioButton DayButton
        {
            get { return btn; }
        }

        public double FontRatio { get; set; }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            btn.FontSize = Height / FontRatio;
        }
    }
}
