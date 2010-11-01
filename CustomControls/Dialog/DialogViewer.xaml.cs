using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Infrasturcture.Global.Controls.Dialog;
using Infrasturcture.Global.Helpers.Events;
using Infrasturcture.TouchLibrary;

namespace CustomControls.Dialog
{
    /// <summary>
    /// Interaction logic for DialogViewer.xaml
    /// </summary>
    public partial class DialogViewer : UserControl, IMTouchControl
    {
        public IMTContainer Container { get; set; }
        public event EventHandler Closed;
        public Button CloseButton
        {
            get
            {
                return closeButton;
            }
        }

        public IDialogContent DialogContent  
        {
            get { return (IDialogContent)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(IDialogContent), typeof(DialogViewer));

        public DialogViewer(IDialogContent dialogContent)
        {
            InitializeComponent();
            DataContext = this;
            DialogContent = dialogContent;
            closeButton.Click += CloseButtonClick;
            ((ADialogContent)DialogContent).Closed += DialogContentClosed;
        }

        void DialogContentClosed(object sender, DataEventArgs e)
        {
            OnClosed();
        }

        void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            ((ADialogContent)DialogContent).OnClosed(new DataEventArgs());
        }

        private void OnClosed()
        {
            if(Closed!=null)
                Closed(this,new EventArgs());
        }
        public Brush BackgroundBrush
        {
            set
            {
                bdrInner.Background = value;
            }
        }

        public Border Overlay { get; set; }
    }
}
