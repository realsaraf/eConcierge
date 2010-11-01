using System;
using System.Windows;
using CustomControls.LandMark;
using Infrasturcture.TouchLibrary;

namespace mConciergeClient
{
    public partial class MainWindow
    {
        private bool _skipLandmarkClose;
        void LandMarkUnchecked(object sender, RoutedEventArgs e)
        {
            if (!_skipLandmarkClose)
            {
                LandMarkControl.GetInstance().Close();
                _skipWeatherClose = false;
            }
        }
        
        private void LoadLandMark()
        {
            FrameworkManager.RegisterElement(LandMarkTool, false, new[] { TouchAction.Tap });
            LandMarkTool.Checked += LandMarkToolChecked;
        }

        void LandMarkToolChecked(object sender, RoutedEventArgs e)
        {
            ShowLandMark();
        }

        private void ShowLandMark()
        {
            var control = LandMarkControl.GetInstance();
            var top = canvas.ActualHeight / 2 - (control.Height / 2);
            var left = canvas.ActualWidth / 2 - (control.Width / 2);
            control.Load(FrameworkManager, left, top);
            control.Closed += ControlClosed;
        }

        void ControlClosed(object sender, EventArgs e)
        {
            _skipLandmarkClose = true;
            LandMarkTool.IsChecked = false;
        }
    }
}
