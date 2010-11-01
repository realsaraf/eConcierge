using System.Windows.Controls;
using CustomControls.InheritedFrameworkControls;

namespace CustomControls.OptionControl
{
    /// <summary>
    /// Interaction logic for TouchOptionItem.xaml
    /// </summary>
    public partial class TouchOptionItem : UserControl
    {
        public TouchOptionItem()
        {
            InitializeComponent();
        }

        public string CategoryText
        {
            set { txbCategory.Text = value; }
        }
        public TouchButton CateogoryButton
        {
            get
            {
                return btnEvent;
            }
        }

    }
}
