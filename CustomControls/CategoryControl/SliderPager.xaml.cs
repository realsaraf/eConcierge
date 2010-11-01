using System.Windows;
using System.Windows.Controls;

namespace CustomControls.CategoryControl
{
    /// <summary>
    /// Interaction logic for SliderPager.xaml
    /// </summary>
    public partial class SliderPager : UserControl
    {
        public SliderPager()
        {
            InitializeComponent();
            sld.Minimum = 0;
            sld.Maximum = 10;
            
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
          
        }
    }
}
