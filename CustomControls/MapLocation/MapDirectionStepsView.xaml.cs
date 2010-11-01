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
using Helpers;
using Infrasturcture.TouchLibrary;
using TouchAction = Infrasturcture.TouchLibrary.TouchAction;

namespace CustomControls.MapLocation
{
    /// <summary>
    /// Interaction logic for MapDirectionStepsView.xaml
    /// </summary>
    public partial class MapDirectionStepsView : UserControl, IMTouchControl
    {
        private FiveButtonDiskControl.FiveButtonDiskControl _navigationMenuDisk;
        public IMTContainer Container { get; set; }
        public IFrameworkManger FrameworkManager { get; set; }
        public event EventHandler Closed;

        public void InvokeClosed(EventArgs e)
        {
            EventHandler handler = Closed;
            if (handler != null) handler(this, e);
        }

        public MapDirectionStepsView()
        {
            InitializeComponent();
            menuButton.Checked += MenuButtonChecked;
        }
        
        public void InitializeControlData(DirectionSteps directionSteps)
        {
            foreach (var step in directionSteps.Steps)
            {
                var mapdirectionStepControl = new MapDirectionStepControl { DataContext = step };
                stepsContainer.Children.Add(mapdirectionStepControl);
            }
            DataContext = directionSteps;
        }

        void MenuButtonChecked(object sender, EventArgs e)
        {
            _navigationMenuDisk = new FiveButtonDiskControl.FiveButtonDiskControl();
            _navigationMenuDisk.InitializeControlData
                (
                "Images/print.png",
                "Images/textToPhone.png",
                "Images/close.png",
                "Images/bluetooth.png",
                "Print",
                "",
                "Close",
                "BlueTooth"
                );
            var translatePoint = menuButton.TranslatePoint(new Point(0, 0), FrameworkManager.Canvas);
            var top = translatePoint.Y + menuButton.ActualHeight / 2;
            var left = translatePoint.X + menuButton.ActualWidth / 2;
            _navigationMenuDisk.InitializeControl(FrameworkManager, left, top);
            _navigationMenuDisk.Expand();
            _navigationMenuDisk.Collapsed += NavigationMenuDiskCollapsed;
            _navigationMenuDisk.BottomRightButtonClick += NavigationMenuDiskCloseButtonClick;
        }

        void NavigationMenuDiskCloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        void NavigationMenuDiskCollapsed(object sender, EventArgs e)
        {
            _navigationMenuDisk.Collapsed -= NavigationMenuDiskCollapsed;
            _navigationMenuDisk.Close();
            _navigationMenuDisk = null;
            menuButton.IsChecked = false;
        }

        public void Close()
        {
            if(_navigationMenuDisk!=null)
                _navigationMenuDisk.Close();
            FrameworkManager.UnRegisterElement(((IMTContainer)scrollViewer.Tag).Id);
            FrameworkManager.UnRegisterElement(menuButton);
            FrameworkManager.RemoveControl(this);
            InvokeClosed(new EventArgs());
        }

        public void InitializeControl(IFrameworkManger frameworkManager, double left, double top)
        {
            FrameworkManager = frameworkManager;
            FrameworkManager.RegisterElement((IMTouchControl)menuButton, false, new[] { TouchAction.Tap });
            scrollViewer.Tag = FrameworkManager.RegisterElement(scrollViewer, false, new[] { TouchAction.Slide });
            FrameworkManager.AddControlWithAllNoGestures(this, left, top);
        }

    }
}
