using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using CjcAwesomiumWrapper;
using CustomControls.Abstract;
using CustomControls.CircularButton;
using CustomControls.MapLocation;
using Helpers;
using Helpers.Extensions;
using Infrasturcture.Global.Controls;
using Infrasturcture.Global.Helpers.Events;
using Infrasturcture.TouchLibrary;
using WpfKb.Controls;
using TextBox = System.Windows.Controls.TextBox;
using Timer = System.Windows.Forms.Timer;
using TouchAction = Infrasturcture.TouchLibrary.TouchAction;
using WebBrowser = Cjc.ChromiumBrowser.WebBrowser;

namespace TouchControls
{
    /// <summary>
    /// Interaction logic for ChromBrowser.xaml
    /// </summary>
    public partial class MapBrowser : UserControl, IMTouchControl
    {
        #region Properties
        private readonly string _url;
        private readonly Timer _timer;
        private bool _skip;

        public event EventHandler MenuChecked;
        public event EventHandler MenuUnChecked;
        public IMTContainer Container { get; set; }
        public bool MouseHandled { get; set; }
        public bool IsLocked { get; set; }
        public WebBrowser ChromBrowser;
        private double _left;
        private double _top;
        public event EventHandler Closed;
        private MapDirectionStepsView _directionsControl;
        private MapSearchControl _mapSearchControl;
        public CircularToggleButton LockButton
        {
            get
            {
                return lockButton;
            }
        }
        public IFrameworkManger FrameworkManager { get; set; }
        public PointF TouchPoint { get; set; }
        public bool CancelScale { get; set; }



        public string SourceButtonText
        {
            get { return (string)GetValue(SourceButtonTextProperty); }
            set { SetValue(SourceButtonTextProperty, value); }
        }

        public static readonly DependencyProperty SourceButtonTextProperty =
            DependencyProperty.Register("SourceButtonText", typeof(string), typeof(MapBrowser), new UIPropertyMetadata("Set Origin"));



        public string DestinationButtonText
        {
            get { return (string)GetValue(DestinationButtonTextProperty); }
            set { SetValue(DestinationButtonTextProperty, value); }
        }

        public static readonly DependencyProperty DestinationButtonTextProperty =
            DependencyProperty.Register("DestinationButtonText", typeof(string), typeof(MapBrowser), new UIPropertyMetadata("Set Destination"));


        #endregion

        public MapBrowser(string url, double width, double height)
        {
            Width = width;
            Height = height;
            _timer = new Timer { Interval = 300 };
            _timer.Tick += TimerTick;
            _url = url;
            InitializeComponent();
            Loaded += MapBrowser_Loaded;
            ChromBrowser = new WebBrowser
                               {
                                   Clip = BrowserContainer.Clip,
                                   Focusable = true,
                                   EnableAsyncRendering = false,
                                   Width = width,
                                   Height = height,
                                   Source = _url,
                                   Cursor=Cursors.None
                               };
            ChromBrowser.Navigate(_url);
            Loaded += ChromBrowser_Loaded;
            ChromBrowser.Ready += ChromBrowser_Ready;
            BrowserContainer.Children.Add(ChromBrowser);
            LockButton.Tag = "Lock";

            LockButton.Checked += LockButtonChecked;
            LockButton.UnChecked += LockButtonChecked;

            closeButton.Click += CloseButtonClick;

            setSourceButton.Click += SetSourceButtonClick;
            setDestinationButton.Click += SetDestinationButtonClick;
            getDirectionsButton.Click += GetDirectionsButtonClick;
            searchLocationButton.Click += SearchLocationButtonClick;
            DataContext = this;
        }

        public void InvokeClosed(EventArgs e)
        {
            EventHandler handler = Closed;
            if (handler != null) handler(this, e);
        }

        void CloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        void MapBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            Container.Reset();
            Container.StartX = _left.ToInt();
            Container.StartY = _top.ToInt();
        }

        void SetSourceButtonClick(object sender, RoutedEventArgs e)
        {
            if (SourceButtonText.Equals("Set Origin"))
            {
                SetSource(null);
                SourceButtonText = "Remove Origin";
            }
            else
            {
                RemoveSource();
                SourceButtonText = "Set Origin";
            }
            
        }

        void SetDestinationButtonClick(object sender, RoutedEventArgs e)
        {
            if (DestinationButtonText.Equals("Set Destination"))
            {
                SetDestination(null);
                DestinationButtonText = "Remove Destination";
            }
            else
            {
                RemoveDestination();
                DestinationButtonText = "Set Destination";
            }
        }

        void GetDirectionsButtonClick(object sender, RoutedEventArgs e)
        {
            var origin = GetOrigin();
            var destination = GetDestination();
            var directionSteps = GMapUtil.GetDirections(origin, destination);
            if (directionSteps != null && directionSteps.Steps.Count > 0)
            {
                if (_directionsControl != null)
                    _directionsControl.Close();
                var height = FrameworkManager.Canvas.ActualHeight;
                var mapDirectionStepsView = new MapDirectionStepsView { Height = height-200 };
                mapDirectionStepsView.InitializeControl(FrameworkManager, 0, 0);
                mapDirectionStepsView.InitializeControlData(directionSteps);
                _directionsControl = mapDirectionStepsView;
                _directionsControl.Closed += DirectionsControlClosed;
                GetDirections();
            }
        }

        void DirectionsControlClosed(object sender, EventArgs e)
        {
            _directionsControl = null;
        }

        void SearchLocationButtonClick(object sender, RoutedEventArgs e)
        {
            if (_mapSearchControl != null)
                _mapSearchControl.Close();
            var height = FrameworkManager.Canvas.ActualHeight;
            _mapSearchControl = new MapSearchControl { Height = height - 200 };
            _mapSearchControl.InitializeControl(FrameworkManager, 0, 0);
            _mapSearchControl.ShowKeyBoardRequested += MapSearchControlShowKeyBoardRequested;
            _mapSearchControl.HideKeyBoardRequested += MapSearchControlHideKeyBoardRequested;
            _mapSearchControl.SetDestination += MapSearchControlSetDestination;
            _mapSearchControl.Closed += MapSearchControlClosed;
            _mapSearchControl.SetSource += MapSearchControlSetSource;

        }

        void MapSearchControlClosed(object sender, EventArgs e)
        {
            _mapSearchControl = null;
        }

        void MapSearchControlSetSource(object sender, DataEventArgs e)
        {
            _mapSearchControl.Close();
            _mapSearchControl = null;
            SetSource((LocationResult)e.Data);
        }

        void MapSearchControlSetDestination(object sender, DataEventArgs e)
        {
            _mapSearchControl.Close();
            _mapSearchControl = null;
            SetDestination((LocationResult)e.Data);
        }

        void MapSearchControlHideKeyBoardRequested(object sender, EventArgs e)
        {
            FrameworkManager.HideKeyBoard();
        }

        void MapSearchControlShowKeyBoardRequested(object sender, EventArgs e)
        {
            FrameworkManager.ShowKeyBoard();
        }

        

        void ChromBrowser_Ready(object sender, EventArgs e)
        {
            loadingBlock.Visibility = Visibility.Collapsed;
            loadingBlock.Width = loadingBlock.Height = 0;
            Container.Reset();
            Container.StartX = _left.ToInt();
            Container.StartY = _top.ToInt();
        }

        private void LockButtonChecked(object sender, EventArgs eventArgs)
        {
            var button = sender as CircularToggleButton;
            switch (button.Tag.ToString())
            {
                case "Lock":
                    IsLocked = (bool)button.IsChecked;
                    Container.Locked = IsLocked;
                    break;
            }
        }

        public void Load(IFrameworkManger frameworkManger, double left, double top)
        {
            _top = top;
            _left = left;
            FrameworkManager = frameworkManger;
            FrameworkManager.RegisterElement((IMTouchControl)LockButton, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement((IMTouchControl)closeButton, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement((IMTouchControl)setSourceButton, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement((IMTouchControl)setDestinationButton, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement((IMTouchControl)getDirectionsButton, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement((IMTouchControl)searchLocationButton, false, new[] { TouchAction.Tap });
            FrameworkManager.AddControlWithAllGestures(this, left, top);
        }


        public void Close()
        {
            FrameworkManager.UnRegisterElement(setSourceButton);
            FrameworkManager.UnRegisterElement(setDestinationButton);
            FrameworkManager.UnRegisterElement(getDirectionsButton);
            FrameworkManager.UnRegisterElement(searchLocationButton);
            FrameworkManager.UnRegisterElement(LockButton);
            FrameworkManager.UnRegisterElement(closeButton);
            FrameworkManager.RemoveControl(this);
            InvokeClosed(new EventArgs());
            if(_mapSearchControl!=null)
                _mapSearchControl.Close();
            if(_directionsControl!=null)
                _directionsControl.Close();
        }

        void TimerTick(object sender, EventArgs e)
        {
            _skip = false;
            CancelScale = false;
            _timer.Stop();
        }

        void ChromBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            Container.OnApplyingTransforms += OnApplyingTransforms;
            Container.Reset();
            Container.StartX = _left.ToInt();
            Container.StartY = _top.ToInt();
        }

        private void OnApplyingTransforms(object sender, EventArgs e)
        {
            if (IsLocked && !CancelScale)
            {
                var scaler = Container.Transforms.Children.OfType<ScaleTransform>().LastOrDefault();
                this.Cursor = null;
                CancelScale = (scaler != null && scaler.ScaleX != 0);
                if (scaler != null && !_skip)
                {
                    if(scaler.ScaleX!=1)
                    {
                        var action = scaler.ScaleX > 1 ? "ZoomIn()" : "ZoomOut()";
                        _skip = true;
                        _timer.Start();
                        ChromBrowser.ExecuteJavascript(action, "");
                    }
                }
                Container.CancelTransforms = true;
            }
            else
            {
                Container.CancelTransforms = CancelScale;
                CancelScale = false;
            }
        }

        #region Scripts Calls
        private void RemoveDestination()
        {
            ChromBrowser.ExecuteJavascript(string.Format("RemoveDestination()"), "");
        }

        private void RemoveSource()
        {
            ChromBrowser.ExecuteJavascript(string.Format("RemoveSource()"), "");
        }

        public void SetSource(LocationResult selectedLocation)
        {
            lockButton.IsChecked = true;
            if (selectedLocation == null)
                ChromBrowser.ExecuteJavascript(string.Format("AddSource()"), "");
            else
                ChromBrowser.ExecuteJavascript(string.Format("SetSource({0},{1})", selectedLocation.Latitude, selectedLocation.Longitude), "");
        }

        public void SetDestination(LocationResult selectedLocation)
        {
            lockButton.IsChecked = true;
            if (selectedLocation == null)
                ChromBrowser.ExecuteJavascript(string.Format("AddDestination()"), "");
            else
                ChromBrowser.ExecuteJavascript(string.Format("SetDestination({0},{1})", selectedLocation.Latitude, selectedLocation.Longitude), "");
        }

        public void GetDirections()
        {
            ChromBrowser.ExecuteJavascript("GetDirection()", "");
        }
        
        public string GetOrigin()
        {
            var value = ChromBrowser.ExecuteJavascriptWithResult("GetOrigin()","");
            var origin = value.ToString();
            if(!string.IsNullOrEmpty(origin))
            {
                origin = origin.Replace("(", "");
                origin = origin.Replace(")", "");
                origin = origin.Replace(" ", "");
            }
            return origin;
        }

        public string GetDestination()
        {
            var value = ChromBrowser.ExecuteJavascriptWithResult("GetDestination()", "");
            var destination = value.ToString();
            if (!string.IsNullOrEmpty(destination))
            {
                destination = destination.Replace("(", "");
                destination = destination.Replace(")", "");
                destination = destination.Replace(" ", "");
            }
            return destination;
        }
    
        #endregion
    }
}
