using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CustomControls.PictureControl;
using Infrasturcture.TouchLibrary;
using TouchControls;
using TouchFramework.Events;
using TouchAction = Infrasturcture.TouchLibrary.TouchAction;

namespace mConciergeClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties
        protected FrameworkManger FrameworkManager { get; set; }
        private readonly double _screenHeight = SystemParameters.PrimaryScreenHeight;
        private readonly double _screenWidth = SystemParameters.PrimaryScreenWidth;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            FrameworkManager = new FrameworkManger(MainCanvas, FeatureToolPanel);
            Closing += OnClosing;
            InitializeTheme();
            Mouse.OverrideCursor=Cursors.None;
        }

        void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CloseApp();
        }

        private void InitializeTheme()
        {
            var currentTheme = ConfigurationManager.AppSettings["Theme"];
            var resourceDictionary = new ResourceDictionary
                          {
                              Source = new Uri(String.Format("Themes/Styles/{0}.xaml", currentTheme), UriKind.Relative)
                          };
            Resources.MergedDictionaries.Add(resourceDictionary);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkManager.Initialize();
            InitializeCanvas();
            LoadHotelExplorer();
            LoadLandMark();
            LoadNavigation();
            LoadCalendar();
            LoadTransportation();
            LoadDining();
            LoadWeather();
            InitializePhotoViewer();
            CreateCloseButton();
        }

        private void InitializePhotoViewer()
        {
            SliderCanvas.Width = Width / 2;
            SliderCanvas.Height = Height;
            SliderCanvas.Background = Brushes.Black;
        }

        private void CreateCloseButton()
        {
            var i = new Image();
            BitmapSource bi = new BitmapImage(new Uri(@"images\red_x.png", UriKind.Relative));
            i.Source = bi;
            Canvas.SetLeft(i, Width - 50);
            canvas.Children.Add(i);
            FrameworkManager.RegisterElement(i, false, new[] { TouchAction.Tap });
            i.AddHandler(MTEvents.TouchDownEvent, new RoutedEventHandler(ShutdownEvent));
        }

        private void InitializeCanvas()
        {
            Width = _screenWidth;
            Height = _screenHeight;
            Left = 0;
            Top = 0;
            ResizeMode = ResizeMode.NoResize;
            WindowStyle = WindowStyle.None;
            MainCanvas.Width = Width;
            MainCanvas.Height = Height;
        }

        public ClosePictureControl AddPhoto(byte[] imageData, TouchAction[] touchActions)
        {
            var photo = new ClosePictureControl();
            var image = photo.SetImage(imageData);
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
            var top = canvas.ActualHeight / 2 ;
            var left = canvas.ActualWidth / 2 ;
            photo.Load(FrameworkManager, left, top);
            return photo;
        }
        public ClosePictureControl AddPhoto(string path, TouchAction[] touchActions)
        {
            var photo = new ClosePictureControl();
            var image = photo.SetImage(path);
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
            var top = canvas.ActualHeight / 2;
            var left = canvas.ActualWidth / 2;
            photo.Load(FrameworkManager, left, top);
            return photo;
        }

        public int GetInt(double value)
        {
            return Int32.Parse(Math.Ceiling(value).ToString());
        }

        private void ShutdownEvent(object sender, RoutedEventArgs e)
        {
            CloseApp();
        }

        private void CloseApp()
        {
            FrameworkManager.Stop();
            Environment.Exit(0);
        }

        void ControlAnimationCompleted(object sender, EventArgs e)
        {
            var control = sender as IMTouchControl;
            control.Container.Reset();
            var translatedPoint = TranslatePoint(new Point(0, 0), canvas);
            control.Container.StartX = GetInt(translatedPoint.X);
            control.Container.StartY = GetInt(translatedPoint.Y);
        }

        private void ShowMessage(string title, string message)
        {
            var msg = new MTouchMessageBox(title, message);
            FrameworkManager.RegisterElement((IMTouchControl)msg, true, null);
            FrameworkManager.RegisterElement(msg.OkButton, false, new[] { TouchAction.Tap });
            Canvas.SetLeft(msg, canvas.ActualWidth / 5);
            Canvas.SetTop(msg, canvas.ActualHeight / 5);
            canvas.Children.Add(msg);
            msg.OkClick += MsgOkClick;
        }

        void MsgOkClick(object sender, EventArgs e)
        {
            var msg = sender as MTouchMessageBox;
            FrameworkManager.UnRegisterElement(msg.OkButton);
            FrameworkManager.RemoveControl(msg);
        }
    }
}