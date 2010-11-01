using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Infrasturcture.TouchLibrary;

namespace CustomControls.MenuDiskControl
{
    public partial class MenuDiskControl
    {
        public event EventHandler CloseClick;
        public event EventHandler EditClick;
        public event EventHandler CopyClick;
        public event EventHandler EmailClick;
        public event EventHandler FacebookClick;
        public event EventHandler TwitterClick;
        public ProgressControl FbProgressControl
        {
            get
            {
                return FacebookProgress;
            }
        }

        public ProgressControl TwitterProgressControl
        {
            get
            {
                return TwitterProgress;
            }
        }

        public MenuDiskControl()
        {
            InitializeComponent();
            CommandButton.Click += CommandButtonClick;
            CloseButton.Click += CloseButton_Click;
            EmailButton.Click += EmailButton_Click;
            EditButton.Click += EditButton_Click;
            CopyButton.Click += CopyButton_Click;
            FacebookButton.Click += FacebookButton_Click;
            TwitterButton.Click += TwitterButton_Click;
        }

        void TwitterButton_Click(object sender, RoutedEventArgs e)
        {
            OnTwitterClick(new ShareButtonEventArgs { FilePath = FilePath, MediaType = MediaType });
        }

        public string FilePath { get; set; }

        public MediaType MediaType { get; set; }

        void FacebookButton_Click(object sender, RoutedEventArgs e)
        {
            OnFacebookClick(new ShareButtonEventArgs { FilePath = FilePath, MediaType = MediaType });
        }

        void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            OnCopyClick(new EventArgs());
        }


        void EditButton_Click(object sender, RoutedEventArgs e)
        {
            OnEditClick(new EventArgs());
        }

        void EmailButton_Click(object sender, RoutedEventArgs e)
        {
            OnEmailClick(new EventArgs());
        }

        void CommandButtonClick(object sender, RoutedEventArgs e)
        {
        }

        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            OnCloseClick(new EventArgs());
        }

        protected virtual void OnCloseClick(EventArgs e)
        {
            if (CloseClick != null)
                CloseClick(this, e);
        }

        protected virtual void OnCopyClick(EventArgs e)
        {
            if (CopyClick != null)
                CopyClick(this, e);
        }

        protected virtual void OnEditClick(EventArgs e)
        {
            if (EditClick != null)
                EditClick(this, e);
        }

        protected virtual void OnEmailClick(EventArgs e)
        {
            if (shareGrid.Opacity == 0)
            {
                var commandShareDownStory = FindResource("ShareButtonMouseDown") as Storyboard;
                commandShareDownStory.Begin();
            }
            else
            {
                var commandShareDownStory = FindResource("ShareHideButtonMouseDown") as Storyboard;
                commandShareDownStory.Begin();
            }

            if (EmailClick != null)
                EmailClick(this, e);
        }

        protected virtual void OnFacebookClick(EventArgs e)
        {
            if (FacebookClick != null)
                FacebookClick(this, e);
        }

        protected virtual void OnTwitterClick(EventArgs e)
        {
            if (TwitterClick != null)
                TwitterClick(this, e);
        }

        public bool RaiseMouseClick(FrameworkElement control)
        {
            bool isCommandClicked = false;
            var controlBelow = GetControlBelow(control);
            if (controlBelow is Button)
            {
                var button = controlBelow as Button;
                switch (button.Name)
                {
                    case "CloseButton":
                        isCommandClicked = true;
                        OnCloseClick(new EventArgs());
                        break;
                    case "CopyButton":
                        isCommandClicked = true;
                        OnCopyClick(new EventArgs());
                        break;
                    case "EditButton":
                        isCommandClicked = true;
                        OnEditClick(new EventArgs());
                        break;
                    case "EmailButton":
                        isCommandClicked = true;
                        OnEmailClick(new EventArgs());
                        break;
                    case "CommandButton":
                        isCommandClicked = true;
                        var commandMouseDownStory = FindResource("CommandButtonMouseDown") as Storyboard;
                        commandMouseDownStory.Begin();
                        break;
                    case "MenuButton":
                        isCommandClicked = true;
                        var menuMouseDownStory = FindResource("MenuButtonMouseDown") as Storyboard;
                        menuMouseDownStory.Begin();
                        break;
                    case "FacebookButton":
                        isCommandClicked = true;
                        OnFacebookClick(new ShareButtonEventArgs() { FilePath = FilePath, MediaType = MediaType });
                        break;
                    case "TwitterButton":
                        isCommandClicked = true;
                        OnTwitterClick(new ShareButtonEventArgs() { FilePath = FilePath, MediaType = MediaType });
                        break;
                    default:
                        break;
                }
            }
            return isCommandClicked;
        }

        private DependencyObject GetControlBelow(FrameworkElement control)
        {
            var p = VisualTreeHelper.GetParent(control);
            while (p != null)
            {
                if (((FrameworkElement)p).Name == "LayoutRoot")
                    break;
                if (p is Button)
                {
                    break;
                }
                p = VisualTreeHelper.GetParent(p);
            }
            return p;
        }

        public void RaiseClose()
        {
            if (CloseClick != null)
                CloseClick(this, new EventArgs());
        }

        public void RaiseMouseMove(FrameworkElement control)
        {
            bool isCommandClicked = false;
            var controlBelow = GetControlBelow(control);
            if (controlBelow is Button)
            {
                var button = controlBelow as Button;
                button.Focus();
            }
        }

        public void RaiseMouseDown(FrameworkElement control)
        {
            var controlBelow = GetControlBelow(control);
            if (controlBelow is Button)
            {
                var button = controlBelow as Button;
                button.CaptureMouse();
            }
        }

        public void RaiseMouseUp(FrameworkElement control)
        {
            var controlBelow = GetControlBelow(control);
            if (controlBelow is Button)
            {
                var button = controlBelow as Button;
                button.ReleaseMouseCapture();
            }
        }
    }
}