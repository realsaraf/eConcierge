using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Infrasturcture.TouchLibrary;

namespace CustomControls.CircularButton
{
    /// <summary>
    /// Interaction logic for CircularCloseButtonControl.xaml
    /// </summary>
    public partial class CircularLockToggleButton : UserControl, IMTouchControl
    {
        public event EventHandler Tapped;
        public ToggleButton ToggleButton
        {
            get
            {
                return CommandToggleButton;
            }
        }
        public CircularLockToggleButton()
        {
            InitializeComponent();
            var imageBrush = new ImageBrush(new BitmapImage(new Uri(@"Images\lockButtonImage.png", UriKind.Relative)))
                                 {Stretch = Stretch.None};
            ToggleButton.Background = imageBrush;
        }
        
        public void OnTapped()
        {
            if(Tapped!=null)
                Tapped(this,new EventArgs());
        }

        public IMTContainer Container { get; set; }

        private void CommandToggleButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            var imageBrush = new ImageBrush(new BitmapImage(new Uri(@"Images\lockButtonLocked.png", UriKind.Relative)));
            imageBrush.Stretch = Stretch.None;
            ToggleButton.Background = imageBrush;
        }

        private void CommandToggleButton_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            var imageBrush = new ImageBrush(new BitmapImage(new Uri(@"Images\lockButtonImage.png", UriKind.Relative)));
            imageBrush.Stretch = Stretch.None;
            ToggleButton.Background = imageBrush;
        }
    }
}
