using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using CustomControls.Abstract;
using Infrasturcture;
using Infrasturcture.TouchLibrary;

namespace CustomControls.Weather
{
    /// <summary>
    /// Interaction logic for CalendarControl.xaml
    /// </summary>
    public partial class WeatherControl : AnimatableControl, IMTouchControl
    {
        private static WeatherControl _weather;
        private List<WeatherElement> _weathersData;
        public event EventHandler Closed;
        public IFrameworkManger FrameworkManager { get; set; }
        private List<WeatherDay> _weatherDayControls = new List<WeatherDay>();
        public static WeatherControl GetInstance()
        {
            return _weather ?? (_weather = new WeatherControl());
        }

        #region Properties
        
        public IMTContainer Container { get; set; }

        #endregion

        private WeatherControl()
        {
            InitializeComponent();
            closeButton.Click += CloseButtonClick;
        }

        void CloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        public void Load(IFrameworkManger frameworkManager, double left, double top)
        {
            FrameworkManager = frameworkManager;
            FrameworkManager.RegisterElement((IMTouchControl)closeButton, false, new[] { TouchAction.Tap });
            FrameworkManager.AddControlWithAllGestures(this, left, top);
        }

        public void Close()
        {
            FrameworkManager.UnRegisterElement(closeButton);
            FrameworkManager.RemoveControl(this);
            _weatherDayControls.Clear();
            stkDays.Children.Clear();
            if(Closed!=null)
                Closed(this,new EventArgs());
        }

        private void AnimatableControl_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateWeather();
        }

        private void PopulateWeather()
        {
            _weathersData =  WeatherHelper.GetData("New York");
            var index = 1;
            var isToday = true;
            foreach (var weather in _weathersData)
            {
                var weatherDay = new WeatherDay();
                if (isToday)
                {
                    weatherDay.txbDay.Text = "Today";
                    isToday = false;
                }
                else
                {
                    weatherDay.txbDay.Text = weather.Day;
                }
                weatherDay.imgWeather.Source = WpfUtil.GetImageSource(string.Format(@"Images\{0}.png", GetWeatherImagePath(weather.Condition)));
                weatherDay.btnDay.Tag = index - 1;
                weatherDay.txbTemparture.Text = string.Format("{0}°F", weather.High);
                weatherDay.Margin = new Thickness(10, 0, 10, 0);
                stkDays.Children.Add(weatherDay);
                _weatherDayControls.Add(weatherDay);
                index++;
            }
        }
        private string GetWeatherImagePath(string condition)
        {
            switch (condition)
            {
                case "Clear":
                    return "icon4";
                case "Cloudy":
                    return "icon3";
                case "Fog":
                    return "icon5";
                case "Haze":
                    return "icon9";
                case "Light Rain":
                    return "icon10";
                case "Mostly Cloudy":
                    return "icon7";
                case "Overcast":
                    return "icon6";
                case "Partly Cloudy":
                    return "icon3";
                case "Rain ":
                    return "rainy";
                case "Rain Showers ":
                    return "rainy";
                case "Showers":
                    return "icon8";
                case "Thunderstorm":
                    return "icon7";
                case "Chance of Showers":
                    return "icon9";
                case "Chance of Snow":
                    return "icon12";
                case "Chance of Storm":
                    return "icon3";
                case "Mostly Sunny":
                    return "icon2";
                case "Partly Sunny":
                    return "icon9";
                case "Scattered Showers":
                    return "rainy";
                case "Sunny":
                    return "icon4";
            }
            return "icon4";
        }
    }
}
