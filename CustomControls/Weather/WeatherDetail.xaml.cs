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
using CustomControls.Abstract;
using Infrasturcture.TouchLibrary;
using TouchFramework.Events;
using TouchAction = Infrasturcture.TouchLibrary.TouchAction;

namespace CustomControls.Weather
{
    /// <summary>
    /// Interaction logic for WeatherDetail.xaml
    /// </summary>
    public partial class WeatherDetail : AnimatableControl, IMTouchControl
    {
        private List<WeatherElement> _weatherData;
        private int _currentPagerIndex;
        public IFrameworkManger FrameworkManager { get; set; }

        public WeatherDetail()
        {
            InitializeComponent();
            pager.sld.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sld_ValueChanged);

        }

        void sld_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int newValue = Convert.ToInt32(e.NewValue);
            if (newValue == _currentPagerIndex + 1 || newValue == _currentPagerIndex - 1)
            {
                _currentPagerIndex = newValue;
                SetWeatherProperties(_weatherData[newValue]);
            }

        }

        private void SetWeatherProperties(WeatherElement weather)
        {
            txbDay.Text = _currentPagerIndex == 0 ? "Today" : weather.Day;
            txbTempHight.Text = string.Format("{0}°F", weather.High);
            txbTempLow.Text = string.Format("{0}°F", weather.Low);
            txbCondition.Text = weather.Condition;
            imgWeather.Source = weather.Icon;// new BitmapImage(new Uri(string.Format(@"D:\Saraf's Project\MTouch\Docs\Concierge mockup\weather\icon{0}.png", (_currentPagerIndex + 2)), UriKind.Absolute));

        }

        public IMTContainer Container { get; set; }

        public void InitializeControl(IFrameworkManger frameworkManager, List<WeatherElement> weatherData, int index)
        {
            FrameworkManager = frameworkManager;
            _weatherData = weatherData;
            _currentPagerIndex = index;
            FrameworkManager.RegisterElement(pager.sld as IMTouchControl, false, new[] { TouchAction.Slide });
            FrameworkManager.RegisterElement((IMTouchControl)CommandDisk, false, new[] { TouchAction.Tap });
            CommandDisk.AddHandler(MTEvents.TapEvent, new RoutedEventHandler(WeatherCloseTapEvent));
            pager.sld.Maximum = _weatherData.Count - 1;
            pager.sld.Minimum = 0;
            pager.sld.Value = _currentPagerIndex;
            SetWeatherProperties(_weatherData[_currentPagerIndex]);
            FrameworkManager.AddControlWithAllGestures(this, 20, 20);

        }
        private void WeatherCloseTapEvent(object sender, RoutedEventArgs e)
        {
            FrameworkManager.Canvas.Children.Remove(this);
        }
    }
}
