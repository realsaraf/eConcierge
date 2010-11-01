using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CustomControls.InheritedFrameworkControls;
using Helpers;
using Infrasturcture.Global.Controls;
using Infrasturcture.Global.Helpers.Events;
using Infrasturcture.TouchLibrary;
using TouchAction = Infrasturcture.TouchLibrary.TouchAction;

namespace CustomControls.MapLocation
{
    /// <summary>
    /// Interaction logic for MapSearchControl.xaml
    /// </summary>
    public partial class MapSearchControl : UserControl, IMTouchControl
    {
        public IMTContainer Container { get; set; }
        public IFrameworkManger FrameworkManager { get; set; }
        public event EventHandler ShowKeyBoardRequested;
        public event EventHandler HideKeyBoardRequested;
        public event EventHandler<DataEventArgs> SetSource;
        public event EventHandler<DataEventArgs> SetDestination;
        public List<LocationRadio> LocationRadios = new List<LocationRadio>();
        private const string SEARCH_LOCATION = "search location...";
        public event EventHandler Closed;

        public void InvokeClosed(EventArgs e)
        {
            EventHandler handler = Closed;
            if (handler != null) handler(this, e);
        }
        
        public LocationResult SelectedLocation
        {
            get { return (LocationResult)GetValue(SelectedLocationProperty); }
            set { SetValue(SelectedLocationProperty, value); }
        }

        public static readonly DependencyProperty SelectedLocationProperty =
            DependencyProperty.Register("SelectedLocation", typeof(LocationResult), typeof(MapSearchControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnSelectedLocationChanged)));

        private FiveButtonDiskControl.FiveButtonDiskControl _navigationMenuDisk;

        private static void OnSelectedLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var parent = d as MapSearchControl;
            var isEnabled = (e.NewValue != null);
            if (parent.SetSouceButton != null && parent.SetDestinationButton != null)
            {
                parent.SetSouceButton.IsEnabled = isEnabled;
                parent.SetDestinationButton.IsEnabled = isEnabled;
            }
        }


        public TouchButton SetSouceButton
        {
            get
            {
                return SourceButton;
            }
        }

        public TouchButton SetDestinationButton
        {
            get
            {
                return DestinationButton;
            }
        }

        public MapSearchControl()
        {
            InitializeComponent();
            SelectedLocation = null;
            searchBox.Text = SEARCH_LOCATION;
            menuButton.Checked += MenuButtonChecked;
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
            var left = translatePoint.X + menuButton.ActualWidth/2;
            _navigationMenuDisk.InitializeControl(FrameworkManager, left, top);
            _navigationMenuDisk.Expand();
            _navigationMenuDisk.Collapsed += NavigationMenuDiskCollapsed;
            _navigationMenuDisk.BottomRightButtonClick+=NavigationMenuDiskCloseButtonClick;
        }

        private void NavigationMenuDiskCloseButtonClick(object sender, EventArgs e)
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

        public void InitializeControl(IFrameworkManger frameworkManger, double left, double top)
        {
            FrameworkManager = frameworkManger;
            searchBox.Tag = FrameworkManager.RegisterElement(searchBox, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement((IMTouchControl)SetSouceButton, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement((IMTouchControl)SetDestinationButton, false, new[] { TouchAction.Tap }); 
            FrameworkManager.RegisterElement((IMTouchControl)menuButton, false, new[] { TouchAction.Tap });
            scrollViewer.Tag = FrameworkManager.RegisterElement(scrollViewer, false, new[] { TouchAction.ScrollY });
            SetSouceButton.Click += SetSouceButtonClick;
            SetDestinationButton.Click += SetDestinationButtonClick;
            FrameworkManager.AddControlWithAllNoGestures(this, left, top);
        }

        public void Close()
        {
            if(_navigationMenuDisk!=null)
                _navigationMenuDisk.Close();
            FrameworkManager.UnRegisterElement(((IMTContainer)scrollViewer.Tag).Id);
            FrameworkManager.UnRegisterElement(SetSouceButton);
            FrameworkManager.UnRegisterElement(SetDestinationButton);
            FrameworkManager.UnRegisterElement(menuButton);
            FrameworkManager.UnRegisterElement(((IMTContainer)searchBox.Tag).Id);
            foreach (var locationRadio in LocationRadios)
            {
                FrameworkManager.UnRegisterElement(locationRadio);
            }
            FrameworkManager.RemoveControl(this);
            InvokeClosed(new EventArgs());
        }

        private void CreateResults(IEnumerable<LocationResult> locationResults)
        {
            LocationRadios.Clear();
            container.Children.Clear();
            foreach (var locationResult in locationResults)
            {
                var locationRadio = new LocationRadio(locationResult);
                locationRadio.ScrollViewer = scrollViewer;
                container.Children.Add(locationRadio);
                LocationRadios.Add(locationRadio);
                locationRadio.Checked += LocationSelected;
            }
        }

        private void LocationSelected(object sender, EventArgs e)
        {
            var locationRadio = sender as LocationRadio;
            SelectedLocation = locationRadio.Location;
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            var tb = sender as TextBox;
            if (e.Key == Key.Enter && !string.IsNullOrEmpty(tb.Text))
            {
                SearchLocation(tb.Text);
            }
            if (e.Key == Key.Escape)
            {
                if (!tb.Text.Equals(SEARCH_LOCATION))
                {
                    tb.Foreground = Brushes.Gray;
                    tb.Text = SEARCH_LOCATION;
                }
            }
        }

        private void SearchLocation(string searchKey)
        {
            if (HideKeyBoardRequested != null)
            {
                HideKeyBoardRequested(this, new EventArgs());
            }

            var results = GMapUtil.GetLocations(searchKey);
            if (results != null && results.Count > 0)
            {
                CreateResults(results);
                foreach (var locationRadio in LocationRadios)
                {
                    FrameworkManager.RegisterElement((IMTouchControl)locationRadio, false, new[] { TouchAction.Tap, TouchAction.Slide });
                }
            }
            else
                ShowMessage("No Match", "There were no matching results for the search");
        }

        private void ShowMessage(string title, string message)
        {

        }

        private void SetDestinationButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedLocation == null)
            {
                ShowMessage("Empty", "Please select destination");
                return;
            }
            if (SetDestination != null)
                SetDestination(this, new DataEventArgs(SelectedLocation));
        }

        private void SetSouceButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedLocation == null)
            {
                ShowMessage("Empty", "Please select destination");
                return;
            }
            if (SetSource != null)
                SetSource(this, new DataEventArgs(SelectedLocation));
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (!tb.Text.Equals(SEARCH_LOCATION))
            {
                tb.Foreground = Brushes.Gray;
                tb.Text = SEARCH_LOCATION;
            }

            if (HideKeyBoardRequested != null)
            {
                HideKeyBoardRequested(sender, new EventArgs());
            }
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb.Text.Equals(SEARCH_LOCATION))
            {
                tb.Foreground = Brushes.Black;
                tb.Text = string.Empty;
            }

            if (ShowKeyBoardRequested != null)
            {
                ShowKeyBoardRequested(sender, new EventArgs());
            }
        }
        
    }
}