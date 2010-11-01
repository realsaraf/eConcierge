using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Infrasturcture.TouchLibrary;
using TouchFramework;
using TouchFramework.ControlHandlers;

namespace TouchControls
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class MTouchMessageBox : UserControl, IMTouchControl
    {
        public MTouchMessageBox(string title, string message)
        {
            InitializeComponent();
            txtTitle.Text = title;
            txtMessage.Text = message;
        }

        public event EventHandler OkClick;
        public IMTContainer Container { get; set; }

        public IMTouchControl OkButton
        {
            get
            {
                return btnOk;
            }
        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            if (OkClick != null)
                OkClick(this, e);
        }
    }
}
