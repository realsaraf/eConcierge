using System;
using System.Windows.Controls;
using Infrasturcture.TouchLibrary;

namespace CustomControls.CircularButton
{
    /// <summary>
    /// Interaction logic for CircularCloseButtonControl.xaml
    /// </summary>
    public partial class CircularCloseButtonControl : UserControl, IMTouchControl
    {
        public IMTContainer Container { get; set; }
        public event EventHandler Click;
        public Button Button
        {
            get
            {
                return CommandButton;
            }
        }

        public event EventHandler Tapped;
        public CircularCloseButtonControl()
        {
            InitializeComponent();
            CommandButton.Click += CommandButton_Click;
        }

        void CommandButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }
        
        public void OnTapped()
        {
            if(Tapped!=null)
                Tapped(this,new EventArgs());
        }

    }
}
