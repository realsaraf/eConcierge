using System;
using System.Windows;
using CustomControls.FiveButtonDiskControl;
using CustomControls.MapLocation;
using TouchControls;
using Application = System.Windows.Forms.Application;
using TouchAction = Infrasturcture.TouchLibrary.TouchAction;

namespace mConciergeClient
{
    public partial class MainWindow
    {
        private FiveButtonDiskControl _navigationMenuDisk;
        private MapBrowser _mapBrowser;
        private bool _skipnavigationClose;

        private void LoadNavigation()
        {
            FrameworkManager.RegisterElement(NavigationTool, false, new[] { TouchAction.Tap });
            NavigationTool.Checked += NavigationToolChecked;
        }

        void NavigationToolChecked(object sender, RoutedEventArgs e)
        {
            ShowHotelNavigation();
        }

        void NavigationUnchecked(object sender, RoutedEventArgs e)
        {
            if (!_skipnavigationClose)
            {
                if (_mapBrowser != null)
                    _mapBrowser.Close();
                if (_navigationMenuDisk != null)
                    _navigationMenuDisk.Close();
                _skipnavigationClose = false;
            }
            
        }

        private void ShowHotelNavigation()
        {
            var size = (canvas.ActualHeight < canvas.ActualWidth ? canvas.ActualHeight : canvas.ActualWidth) - 150;
            var path = string.Format("file://{0}/{1}", Application.StartupPath, "mapbackend.html");
            var left = (Width / 2) - (size / 2);
            _mapBrowser = new MapBrowser(path, size, size);
            _mapBrowser.Load(FrameworkManager, left, 50);
            _mapBrowser.MenuChecked += MapMenuButtonChecked;
            _mapBrowser.MenuUnChecked += MapMenuButtonUnchecked;
            _mapBrowser.Closed += MapBrowserClosed;
        }

        void MapBrowserClosed(object sender, EventArgs e)
        {
            _skipnavigationClose = true;
            NavigationTool.IsChecked = false;
        }

        private void MapMenuButtonUnchecked(object sender, EventArgs eventArgs)
        {
            _navigationMenuDisk.Close();
            _navigationMenuDisk = null;
        }

        private void MapMenuButtonChecked(object sender, EventArgs eventArgs)
        {
            _navigationMenuDisk = new FiveButtonDiskControl();
            var top = _navigationMenuDisk.ActualSize / 2 + 10;
            var left = Width - 30 - (_navigationMenuDisk.ActualSize / 2);
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

            _navigationMenuDisk.InitializeControl(FrameworkManager, left, top);
            _navigationMenuDisk.TopRightButtonClick += MapSearchRequested;
            _navigationMenuDisk.BottomRightButtonClick += MapDirectionRequested;
            _navigationMenuDisk.TopLeftButtonClick += SetSourceButtonClick;
            _navigationMenuDisk.BottomLeftButtonClick += SetDestinationButtonClick;
            _navigationMenuDisk.Expand();
        }

        void SetDestinationButtonClick(object sender, EventArgs e)
        {
        }

        void SetSourceButtonClick(object sender, EventArgs e)
        {
        }

        private void MapDirectionRequested(object sender, EventArgs e)
        {

        }

        void MapSearchRequested(object sender, EventArgs eventArgs)
        {

        }
    }
}