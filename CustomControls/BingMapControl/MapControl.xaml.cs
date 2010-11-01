using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using InfoStrat.VE;
using Infrasturcture.TouchLibrary;

namespace BingMapControl
{
    /// <summary>
    /// Interaction logic for MapTest.xaml
    /// </summary>
    public partial class MapControl : UserControl, IMTouchControl
    {
        public VEMap map
        {
            get
            {
                return null; // veMap;
            }
        }
        private static MapControl _mapControl;

        private MapControl()
        {
            InitializeComponent();
            SetUpMap();
            Loaded += MapControl_Loaded;
        }

        void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            ((IMTSmoothContainer)Container).OnApplyingTransforms += MapControl_OnApplyingTransforms;
        }

        void MapControl_OnApplyingTransforms(object sender, EventArgs e)
        {
            var translater = Container.Transforms.Children.OfType<TranslateTransform>().LastOrDefault();
            var scaler = Container.Transforms.Children.OfType<ScaleTransform>().LastOrDefault();
            if (translater != null)
            {
                map.DoMapMove(translater.Value.OffsetX * 5, translater.Value.OffsetY * 5, new System.Windows.Point((ActualWidth / 2) - 50, (ActualWidth / 2) - 50));
            }
            if (scaler != null)
            {
                if (scaler.ScaleX > 1)
                    map.DoMapZoom(scaler.ScaleX * 7, new System.Windows.Point(scaler.CenterX, scaler.CenterY));
                if (scaler.ScaleX < 1)
                    map.DoMapZoom(scaler.ScaleX * -7, new System.Windows.Point(scaler.CenterX, scaler.CenterY));
            }
        }

        public static MapControl GetInstance()
        {
            return _mapControl ?? (_mapControl = new MapControl());
        }

        public void Dispose()
        {
            map.Dispose();
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

            ////Ceeate a default pin location (my house)
            //var newPin = new VEPushPin(new VELatLong(50.826958333333337, -0.16388055555555556));
            //newPin.SetResourceReference(VEPushPin.StyleProperty, "PushPinStyle");
            //newPin.Content = new Label
            //{
            //    Content = "Waiting",
            //    HorizontalAlignment = HorizontalAlignment.Center,
            //    FontSize = 20
            //};
            //newPin.Click += VEPushPin_Click;
            //map.Items.Add(newPin);


            ////I do not like doing this with indexes that may change, but
            ////I had no choice as I wanted map to be exactly 4th child, and 
            ////when setting up map in XAML it would sometimes loose intellisense
            //mainGrid.Children.Insert(4, map);
        }

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            map.DoMapZoom(1000);
        }

        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            map.DoMapZoom(-1000);
        }

        private void BtnRoad_Click(object sender, RoutedEventArgs e)
        {
            map.MapStyle = InfoStrat.VE.VEMapStyle.Road;
        }
        private void BtnAerial_Click(object sender, RoutedEventArgs e)
        {
            map.MapStyle = InfoStrat.VE.VEMapStyle.Aerial;
        }
        private void BtnHybrid_Click(object sender, RoutedEventArgs e)
        {
            map.MapStyle = InfoStrat.VE.VEMapStyle.Hybrid;
        }

        private void VEPushPin_Click(object sender, VEPushPinClickedEventArgs e)
        {
            VEPushPin pin = sender as VEPushPin;
            map.FlyTo(pin.LatLong, -90, 0, 300, null);
        }

        private void btnPanUp_Click(object sender, RoutedEventArgs e)
        {
            map.DoMapMove(0, 1000);
        }

        private void btnPanDown_Click(object sender, RoutedEventArgs e)
        {
            map.DoMapMove(0, -1000);
        }

        private void btnPanLeft_Click(object sender, RoutedEventArgs e)
        {
            map.DoMapMove(1000, 0);
        }

        private void btnPanRight_Click(object sender, RoutedEventArgs e)
        {
            map.DoMapMove(-1000, 0);
        }

        public IMTContainer Container { get; set; }

        public void Zoom(PointF relative)
        {
            map.DoMapZoom(200, new System.Windows.Point(relative.X, relative.Y));
        }

        public void ChangeMapStyle(string name)
        {
            switch (name)
            {
                    case "Road":
                    map.MapStyle = VEMapStyle.Road;
                    break;
                    case "Aerial":
                    map.MapStyle = VEMapStyle.Aerial;
                    break;
                    case "Hybrid":
                    map.MapStyle = VEMapStyle.Hybrid;
                    break;
            }
        }
    }
}
