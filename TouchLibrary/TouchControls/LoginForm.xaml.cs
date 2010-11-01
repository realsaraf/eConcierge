using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CustomControls.PictureControl;
using Infrasturcture.TouchLibrary;

namespace TouchControls
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : UserControl, IMTouchControl
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public string UserName
        {
            get
            {
                return userNameBox.Text;
            }
        }

        public string Password
        {
            get
            {
                return passwordBox.Password;
            }
        }

        public string Message
        {
            get
            {
                return messageBox.Text;
            }
        }

        public event EventHandler SubmitClick;
        public event EventHandler CancelClick;
        public IMTContainer Container { get; set; }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (SubmitClick != null)
                SubmitClick(this, e);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (CancelClick != null)
                CancelClick(this, e);
        }

        public void RaiseMouseClick(FrameworkElement control)
        {
            var p = VisualTreeHelper.GetParent(control);
            while (p != null)
            {
                if (((FrameworkElement)p).Name == "loginFormBorder")
                    break;
                if (p is Button || p is TextBox || p is PasswordBox)
                {
                    break;
                }
                p = VisualTreeHelper.GetParent(p);
            }

            if (p is PasswordBox)
            {
                var passwordBox = p as PasswordBox;
                passwordBox.Focus();
            }
            if (p is TextBox)
            {
                var textBox = p as TextBox;
                textBox.Focus();
            }
            if (p is Button)
            {
                var button = p as Button;
                switch (button.Name)
                {
                    case "btnSubmit":
                        SubmitClick(this, new EventArgs());
                        break;
                    case "btnCancel":
                        CancelClick(this, new EventArgs());
                        break;
                    default:
                        break;
                }
            }
        }

        public PictureControl PictureControl { get; set; }
    }
}
