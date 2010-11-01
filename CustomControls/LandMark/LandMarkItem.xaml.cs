using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CustomControls.InheritedFrameworkControls;
using Infrasturcture;
using Infrasturcture.DTO;
using Infrasturcture.TouchLibrary;

namespace CustomControls.LandMark
{
    /// <summary>
    /// Interaction logic for TouchOptionItem.xaml
    /// </summary>
    public partial class LandMarkItem : UserControl, IMTouchControl
    {
        public DTOLandMark LandMark { get; set; }
        public IMTContainer Container { get; set; }
        public BitmapImage Picture { get; set; }
        public string Title { get; set; }
        public event EventHandler Click;
        public LandMarkItem(DTOLandMark landMark)
        {
            LandMark = landMark;
            InitializeComponent();
            DataContext = this;
            Picture = WpfUtil.BytesToImageSource(landMark.Picture);
            Title = landMark.Title;
        }

        public void Tapped()
        {
            if(Click!=null)
                Click(this,new EventArgs());
        }
    }
}
