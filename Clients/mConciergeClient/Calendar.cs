using System.Windows;
using CustomControls.CalendarControl;
using Infrasturcture.TouchLibrary;

namespace mConciergeClient
{
    public partial class MainWindow
    {
        private bool _skipCalendarClose;
        void CalendarUnchecked(object sender, RoutedEventArgs e)
        {
            if (!_skipCalendarClose)
            {
                TouchCalendar.GetInstance().CloseWithChildren();
                _skipCalendarClose = false;
            }
        }

        private void LoadCalendar()
        {
            FrameworkManager.RegisterElement(CalendarTool, false, new[] { TouchAction.Tap });
            CalendarTool.Checked += CalendarToolChecked;
        }

        void CalendarToolChecked(object sender, RoutedEventArgs e)
        {
            ShowCalendar();
        }

        private void ShowCalendar()
        {
            var calendarControl = TouchCalendar.GetInstance();
            var top = canvas.ActualHeight / 2 - 300;
            var left = canvas.ActualWidth / 2 - 300;
            calendarControl.InitializeControl(FrameworkManager, left, top);
            calendarControl.Closed += CalendarControlClosed;
            _skipCalendarClose = false;
        }

        void CalendarControlClosed(object sender, System.EventArgs e)
        {
            _skipCalendarClose = true;
            CalendarTool.IsChecked = false;
        }
    }
}