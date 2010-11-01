using System;
using System.Windows;
using CustomControls.Weather;
using Infrasturcture.TouchLibrary;

namespace mConciergeClient
{
    public partial class MainWindow
    {
        private bool _skipWeatherClose;
        void WeatherUnchecked(object sender, RoutedEventArgs e)
        {
            if (!_skipWeatherClose)
            {
                WeatherControl.GetInstance().Close();
                _skipWeatherClose = false;
            }
        }

        private void LoadWeather()
        {
            FrameworkManager.RegisterElement(WeatherTool, false, new[] { TouchAction.Tap });
            WeatherTool.Checked += WeatherToolChecked;
        }

        void WeatherToolChecked(object sender, RoutedEventArgs e)
        {
            ShowWeather();
        }

        private void ShowWeather()
        {
            var control = WeatherControl.GetInstance();
            var top = canvas.ActualHeight/2 - (control.Height/2);
            var left = canvas.ActualWidth/2 - (control.Width/2);
            control.Load(FrameworkManager, left, top);
            control.Closed += WeatherControlClosed;
        }

        void WeatherControlClosed(object sender, EventArgs e)
        {
            _skipWeatherClose = true;
            WeatherTool.IsChecked = false;
        }
    }
}
