using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CustomControls.Abstract;
using CustomControls.CircularButton;
using InfoStrat.VE;
using Infrasturcture.TouchLibrary;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Label = System.Windows.Controls.Label;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Point = System.Windows.Point;

namespace CustomControls.MapControl
{
    /// <summary>
    /// Interaction logic for MapTest.xaml
    /// </summary>
    public partial class MapControl : AnimatableControl, IMTouchControl
    {
        protected bool? Locked { get; set; }

        public VEMap Map
        {
            get
            {
                return veMap;
            }
        }
        public CircularLockToggleButton LockButton
        {
            get
            {
                return lockButton;
            }
        }
        public CircularLockToggleButton MenuButton
        {
            get
            {
                return menuButton;
            }
        }
        private static MapControl _mapControl;
        private bool _pinTouched;
        private VEPushPin _currentPin;

        private MapControl()
        {
            InitializeComponent();
            SetUpMap();
            Loaded += MapControl_Loaded;
            menuButton.CommandToggleButton.Checked += MenuToggleButton_Checked;
            menuButton.CommandToggleButton.Unchecked += MenuToggleButton_Checked;
            lockButton.CommandToggleButton.Checked += CommandToggleButton_Checked;
            lockButton.CommandToggleButton.Unchecked += CommandToggleButton_Checked;
        }





        void GlobeControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (_pinTouched && _currentPin != null)
            {
                var translatedPosition = new Point(e.X, e.Y);
                var pos = translatedPosition;
                var point = new Point(pos.X, pos.Y);
                var location = Map.PointToLatLong(point);
                if (location != null)
                {
                    _currentPin.Latitude = location.Latitude;
                    _currentPin.Longitude = location.Longitude;
                }
            }
        }

        void GlobeControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var point = e.Location;
            foreach (var item in Map.Items.OfType<VEPushPin>())
            {
                if (item.IsMouseDirectlyOver)
                {
                    _currentPin = item;
                    break;
                }
            }
        }

        private void MenuToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            var currentPoint = new Point(ActualWidth / 2, ActualHeight / 2);
            CreateMarker(Map.PointToLatLong(currentPoint), "MapPushPinStyle");
        }

        private void CreateMarker(VELatLong currentLocation, string pushpinstyle)
        {
            var newPin = new VEPushPin(currentLocation);
            newPin.Style = (Style)FindResource(pushpinstyle);
            newPin.Content = new Label
                                 {
                                     Content = new Border() { Width = 40, Height = 40, Background = new ImageBrush(new BitmapImage(new Uri(@"blueMarker.png", UriKind.Relative))) },
                                     HorizontalAlignment = HorizontalAlignment.Center,
                                     FontSize = 20
                                 };
            //newPin.Click += VEPushPin_Click;
            newPin.PreviewMouseDown += newPin_PreviewMouseDown;
            newPin.MouseMove += newPin_MouseMove;
            //newPin.PreviewMouseUp += NewPinMouseUp;
            //newPin.PreviewMouseMove += NewPinMouseMove;
            Map.Items.Add(newPin);
        }

        void newPin_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //_currentPin = sender as VEPushPin;
            //_pinTouched = true;
        }

        void newPin_MouseMove(object sender, MouseEventArgs e)
        {
            //if (_pinTouched && _currentPin != null)
            //{
            //    AnimateUtility.AnimateElementPoint(_currentPin,VEPushPin.DisplayLatitudeProperty,e.)
            //}
            //if (_pinTouched && _currentPin != null)
            //{
            //    var translatedPosition = e.GetPosition(Map);
            //    var pos = translatedPosition;
            //    var point = new Point(pos.X, pos.Y);
            //    var location = Map.PointToLatLong(point);
            //    if (location != null)
            //    {
            //        _currentPin.Latitude = location.Latitude;
            //        _currentPin.Longitude = location.Longitude;
            //    }
            //}
        }

        void NewPinMouseDown(object sender, MouseButtonEventArgs e)
        {
            _currentPin = sender as VEPushPin;
            _pinTouched = true;
        }

        void CommandToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Locked = ((ToggleButton)sender).IsChecked;
        }

        void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            //PolyInfo polylineStyle = PolyInfo.DefaultPolyline;
            //polylineStyle.LineWidth = 4;
            //polylineStyle.AltitudeMode = AltitudeMode.FromGround;

            //var startLocation = new GeocodeLocation() { Latitude = 40.644632134160233, Longitude = -74.103277135907049 };
            //var endLocation = new GeocodeLocation() { Latitude = 40.708518028284395, Longitude = -74.017532940244919 };
            //var results = GetRoute(startLocation, endLocation);

            //var lla = new List<LatLonAlt>();
            //foreach (var item in results.Legs[0].Itinerary)
            //{
            //    lla.Add(new LatLonAlt(item.Location.Latitude*Math.PI/180.0, item.Location.Longitude*Math.PI/180.0, 0));
            //}

            //Map.GlobeControl.Host.Geometry.AddGeometry(new PolylineGeometry("route", Guid.NewGuid().ToString(), null, lla.ToArray(), PolylineGeometry.PolylineFormat.Polyline2D, polylineStyle));



            ((IMTContainer)Container).OnApplyingTransforms += MapControl_OnApplyingTransforms;
        }

        public static RouteResult GetRoute(GeocodeLocation loc1, GeocodeLocation loc2)
        {
            try
            {
                var routeService = new RouteService();
                var req = new RouteRequest();
                var mreq = new MajorRoutesRequest();
                // Set the credentials using a valid Virtual Earth token
                string token = "AolKu2V0H2w28BS3ZiaImboX02IOwKIS_7P_EhRQ0-cNuGAWqovQ83ISPMjXC_0b";// GetToken();
                req.Credentials = new Credentials();
                req.Credentials.Token = token;

                var wps = new Waypoint[2];
                wps[0] = new Waypoint
                             {
                                 Location = new Location
                                                {
                                                    Latitude = loc1.Latitude,
                                                    Longitude = loc1.Longitude
                                                }
                             };
                wps[1] = new Waypoint
                             {
                                 Location = new Location
                                                {
                                                    Latitude = loc2.Latitude,
                                                    Longitude = loc2.Longitude
                                                }
                             };

                req.Waypoints = wps;

                req.Options = new RouteOptions
                                  {
                                      Optimization = RouteOptimization.MinimizeDistance,
                                      RoutePathType = RoutePathType.Points,
                                      Mode = TravelMode.Driving
                                  };

                req.UserProfile = new UserProfile
                                      {
                                          DistanceUnit = DistanceUnit.Kilometer
                                      };

                RouteResponse response = routeService.CalculateRoute(req);

                if (response.Result != null)
                    return response.Result;

            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        void MapControl_OnApplyingTransforms(object sender, EventArgs e)
        {
            if (Locked != null && (bool)Locked)
            {
                var translater = Container.Transforms.Children.OfType<TranslateTransform>().LastOrDefault();
                var scaler = Container.Transforms.Children.OfType<ScaleTransform>().LastOrDefault();
                if (translater != null)
                {
                    veMap.DoMapMove(translater.Value.OffsetX * 5, translater.Value.OffsetY * 5, new Point((ActualWidth / 2) - 50, (ActualWidth / 2) - 50));
                }
                if (scaler != null)
                {
                    if (scaler.ScaleX > 1)
                        veMap.DoMapZoom(scaler.ScaleX * 7, new Point(scaler.CenterX, scaler.CenterY));
                    if (scaler.ScaleX < 1)
                        veMap.DoMapZoom(scaler.ScaleX * -7, new Point(scaler.CenterX, scaler.CenterY));
                }
                Container.CancelTransforms = true;
            }
            Container.CancelTransforms = false;
        }

        public static MapControl GetInstance()
        {
            return _mapControl ?? (_mapControl = new MapControl());
        }

        public override void Dispose()
        {
            veMap.Dispose();
            _mapControl = null;
        }
        private void SetUpMap()
        {
            //Create the VE Map
            //map = new VEMap
            //{
            //    Width = 800,
            //    Height = 800,
            //    Margin = new Thickness(5),
            //    VerticalAlignment = VerticalAlignment.Top,
            //    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            //    MapStyle = VEMapStyle.Hybrid,
            //    LatLong = new Point(38.9444195081574, -77.0630161230201),
            //    Clip = new EllipseGeometry
            //    {
            //        RadiusX = 400,
            //        RadiusY = 400,
            //        Center = new Point(400, 400)
            //    }
            //};

            //Ceeate a default pin location (my house)
            var newPin = new VEPushPin(new VELatLong(50.826958333333337, -0.16388055555555556));
            newPin.SetResourceReference(VEPushPin.StyleProperty, "PushPinStyle");
            newPin.Content = new Label
            {
                Content = "Waiting",
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 20
            };
            newPin.Click += VEPushPin_Click;
            Map.Items.Add(newPin);


            ////I do not like doing this with indexes that may change, but
            ////I had no choice as I wanted map to be exactly 4th child, and 
            ////when setting up map in XAML it would sometimes loose intellisense
            //mainGrid.Children.Insert(4, map);
        }

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            Map.DoMapZoom(1000);
        }

        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            Map.DoMapZoom(-1000);
        }

        private void BtnRoad_Click(object sender, RoutedEventArgs e)
        {
            Map.MapStyle = InfoStrat.VE.VEMapStyle.Road;
        }
        private void BtnAerial_Click(object sender, RoutedEventArgs e)
        {
            Map.MapStyle = InfoStrat.VE.VEMapStyle.Aerial;
        }
        private void BtnHybrid_Click(object sender, RoutedEventArgs e)
        {
            Map.MapStyle = InfoStrat.VE.VEMapStyle.Hybrid;
        }

        private void VEPushPin_Click(object sender, VEPushPinClickedEventArgs e)
        {
            Point? ptTest = Map.LatLongToPoint(Map.VELatLong);
            VELatLong latTest = Map.PointToLatLong(ptTest);

            //VEPushPin pin = sender as VEPushPin;
            //Map.FlyTo(pin.LatLong, -90, 0, 300, null);
        }

        private void btnPanUp_Click(object sender, RoutedEventArgs e)
        {
            Map.DoMapMove(0, 1000);
        }

        private void btnPanDown_Click(object sender, RoutedEventArgs e)
        {
            Map.DoMapMove(0, -1000);
        }

        private void btnPanLeft_Click(object sender, RoutedEventArgs e)
        {
            Map.DoMapMove(1000, 0);
        }

        private void btnPanRight_Click(object sender, RoutedEventArgs e)
        {
            Map.DoMapMove(-1000, 0);
        }

        public IMTContainer Container { get; set; }

        public void Zoom(PointF relative)
        {
            veMap.DoMapZoom(200, new System.Windows.Point(relative.X, relative.Y));
        }

        public void ChangeMapStyle(string name)
        {
            switch (name)
            {
                case "Road":
                    veMap.MapStyle = VEMapStyle.Road;
                    break;
                case "Aerial":
                    veMap.MapStyle = VEMapStyle.Aerial;
                    break;
                case "Hybrid":
                    veMap.MapStyle = VEMapStyle.Hybrid;
                    break;
            }
        }
    }
}
