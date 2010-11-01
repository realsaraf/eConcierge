using System;
using System.Windows.Controls;
using CustomControls.InheritedFrameworkControls;
using Infrasturcture.Global.Controls;
using Infrasturcture.TouchLibrary;

namespace CustomControls.MapLocation
{
    /// <summary>
    /// Interaction logic for LocationRadio.xaml
    /// </summary>
    public partial class LocationRadio : UserControl, IMTouchControl
    {
        private LocationResult _location;
        public event EventHandler Checked;
        public LocationRadio(LocationResult locationResult)
        {
            this.InitializeComponent();
            Location = locationResult;
            radioButton.Checked += ButtonChecked;
        }

        void ButtonChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if(Checked !=null)
                Checked(this, new EventArgs());
        }

        public RadioButton Radio
        {
            get
            {
                return radioButton;
            }
        }

        public LocationResult Location
        {
            get
            {
                return _location;
            }
            set
            {
                radioButton.DataContext = value;
                _location = value;
            }
        }

        public IPhoneScrollViewer ScrollViewer { get; set; }

        public IMTContainer Container { get; set; }
    }
}