using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CustomControls.Abstract;
using CustomControls.CategoryControl;
using Infrasturcture.TouchLibrary;

namespace CustomControls.CalendarControl
{
    /// <summary>
    /// Interaction logic for CalendarMain.xaml
    /// </summary>
    public partial class TouchCalendar : AnimatableControl, IMTouchControl
    {

        #region Declaration
        private int _currentMonth, _currentYear, _currentDay;
        private CalenderButtonState _buttonState;
        private double _cellWidth = 80;
        private double _fontSize = 12;
        private const int CELL_HEIGHT = 55;
        public event EventHandler<RegisterCellEventArgs> RegisterCalendarCellItem;
        public event EventHandler CalendarDayClick;
        private readonly List<CalendarEventViewer> _eventViewerControls = new List<CalendarEventViewer>();
        public IMTContainer Container { get; set; }
        public IFrameworkManger FrameworkManager { get; set; }
        private static TouchCalendar _calendar;
        private List<CalendarDayItem> _dayItems = new List<CalendarDayItem>();
        private readonly List<CalendarEventDetail> _eventDetailControls = new List<CalendarEventDetail>();

        public List<CalendarDayItem> DayItems
        {
            get
            {
                return _dayItems;
            }
        }

        public event EventHandler Closed;
        #endregion

        #region Constructor
        public static TouchCalendar GetInstance()
        {
            return _calendar ?? (_calendar = new TouchCalendar());
        }

        public TouchCalendar()
        {
            InitializeComponent();
            _currentMonth = DateTime.Now.Month;
            _currentYear = DateTime.Now.Year;
            _currentDay = DateTime.Now.Day;
            closeButton.Click += CloseButtonClick;
        }

        private void CloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        public void InitializeControl(IFrameworkManger frameworkManager, double left, double top)
        {
            FrameworkManager = frameworkManager;
            SetMonth();
            FrameworkManager.RegisterElement((IMTouchControl)closeButton, false, new[] { TouchAction.Tap });
            foreach (IMTouchControl button in GetTouchButtons())
            {
                FrameworkManager.RegisterElement(button, false, new[] { TouchAction.Tap });
            }
            FrameworkManager.AddControlWithAllGestures(this, left, top);
            RegisterUnRegisetrCellItem(true);
        }

        public void Close()
        {
            FrameworkManager.UnRegisterElement(closeButton);
            foreach (var button in GetTouchButtons())
            {
                FrameworkManager.UnRegisterElement(button);
            }
            RegisterUnRegisetrCellItem(false);
            _dayItems.Clear();
            FrameworkManager.RemoveControl(this);
            if (Closed != null)
            {
                Closed(null, null);
            }

        }

        public void CloseWithChildren()
        {
            FrameworkManager.UnRegisterElement(closeButton);
            foreach (var button in GetTouchButtons())
            {
                FrameworkManager.UnRegisterElement(button);
            }
            RegisterUnRegisetrCellItem(false);
            _dayItems.Clear();
            foreach (var calendarEventDetail in _eventDetailControls.ToArray())
            {
                calendarEventDetail.Close();
            }
            foreach (var eventViewerControl in _eventViewerControls.ToArray())
            {
                eventViewerControl.Close();
            }
            FrameworkManager.RemoveControl(this);
            if (Closed != null)
            {
                Closed(null, null);
            }

        }
        private void RegisterUnRegisetrCellItem(bool isRegister)
        {
            if (isRegister)
            {
                foreach (var dayItem in DayItems)
                {
                    FrameworkManager.RegisterElement((IMTouchControl)dayItem, false, new[] { TouchAction.Tap });
                }
            }
            else
            {
                foreach (var dayItem in DayItems)
                {
                    FrameworkManager.UnRegisterElement(dayItem);
                }
            }
        }
        #endregion

        private void SetMonth()
        {
            _cellWidth = 100;
            _fontSize = _cellWidth / 8.5;
            stkDayName.Visibility = Visibility.Visible;
            Sunday.Width = _cellWidth;
            Sunday.FontSize = _fontSize;
            btnLeft.IsEnabled = _currentMonth > 1;
            btnRight.IsEnabled = _currentMonth < 12;
            btnState.Content = GetMonthName((MonthNameType)_currentMonth) + ", " + _currentYear;
            _buttonState = CalenderButtonState.Day;
            PopulateDays();
        }
        private void SetYear()
        {
            _cellWidth = 150;
            _fontSize = _cellWidth / 7.5;
            stkDayName.Visibility = Visibility.Collapsed;

            btnRight.IsEnabled = btnLeft.IsEnabled = true;


            btnState.Content = _currentYear;
            _buttonState = CalenderButtonState.Month;
            PopulateMonths();
        }
        private void SetYearWithYearItem()
        {
            _cellWidth = 150;
            _fontSize = _cellWidth / 7.5;
            stkDayName.Visibility = Visibility.Collapsed;

            btnState.Content = _currentYear;
            _buttonState = CalenderButtonState.Year;
            PopulateYears();
        }

        private void FireRegisterCalendarCellItem(bool isRegister)
        {
            if (DayItems != null)
            {
                RegisterUnRegisetrCellItem(isRegister);
            }
        }

        private void PopulateYears()
        {
            FireRegisterCalendarCellItem(false);
            grd.Children.Clear();
            grd.RowDefinitions.Clear();
            grd.ColumnDefinitions.Clear();
            AddColumns(5);
            int year = _currentYear - 12;
            if (_dayItems != null)
                _dayItems.Clear();
            for (int row = 0; row < 5; row++)
            {
                grd.RowDefinitions.Add(new RowDefinition());
                for (int col = 0; col < 5; col++)
                {
                    var dayItem = new CalendarDayItem(2) { Width = _cellWidth, Height = CELL_HEIGHT };
                    dayItem.DayButton.Content = year;
                    dayItem.DayButton.IsChecked = year == _currentYear;
                    dayItem.DayButton.Tag = year;
                    dayItem.Click += DayButtonClick;
                    dayItem.SetValue(Grid.ColumnProperty, col);
                    dayItem.SetValue(Grid.RowProperty, row);
                    grd.Children.Add(dayItem);
                    year++;
                    _dayItems.Add(dayItem);
                }
            }
            FireRegisterCalendarCellItem(true);
        }

        private void PopulateMonths()
        {
            FireRegisterCalendarCellItem(false);
            grd.Children.Clear();
            grd.RowDefinitions.Clear();
            grd.ColumnDefinitions.Clear();
            AddColumns(4);
            int month = 1;
            if (_dayItems != null)
                _dayItems.Clear();
            for (int row = 0; row < 3; row++)
            {
                grd.RowDefinitions.Add(new RowDefinition());
                for (int col = 0; col < 4; col++)
                {
                    var dayItem = new CalendarDayItem(2) { Width = _cellWidth, Height = CELL_HEIGHT };
                    dayItem.DayButton.Content = GetMonthName((MonthNameType)month);
                    dayItem.DayButton.Tag = month;
                    dayItem.DayButton.IsChecked = month == _currentMonth;
                    dayItem.Click += DayButtonClick;
                    dayItem.SetValue(Grid.ColumnProperty, col);
                    dayItem.SetValue(Grid.RowProperty, row);
                    grd.Children.Add(dayItem);
                    month++;
                    _dayItems.Add(dayItem);
                }
            }
            FireRegisterCalendarCellItem(true);
        }

        private void PopulateDays()
        {
            FireRegisterCalendarCellItem(false);
            grd.Children.Clear();
            grd.RowDefinitions.Clear();
            grd.ColumnDefinitions.Clear();
            AddColumns(7);            
            var totalDays = DateTime.DaysInMonth(_currentYear, _currentMonth);
            var dateTime = new DateTime(_currentYear, _currentMonth, 1);
            int col = (int)dateTime.DayOfWeek, row = -1;
            var day = 1;
            if (_dayItems != null)
                _dayItems.Clear();
            while (day <= totalDays)
            {
                grd.RowDefinitions.Add(new RowDefinition());
                row++;
                int index = 0;
                while (index < 7)
                {
                    var dayItem = new CalendarDayItem(1.5) { Width = _cellWidth, Height = CELL_HEIGHT };
                    if (index == col && day <= totalDays)
                    {
                        dayItem.DayButton.Content = day;
                        dayItem.DayButton.IsChecked = day == _currentDay;
                        dayItem.Click += DayButtonClick;
                        _dayItems.Add(dayItem);
                        day++;
                        col++;
                    }
                    else
                    {
                        dayItem.DayButton.Visibility = Visibility.Hidden;
                    }
                    dayItem.SetValue(Grid.ColumnProperty, index);
                    dayItem.SetValue(Grid.RowProperty, row);
                    grd.Children.Add(dayItem);

                    index++;
                }
                col = 0;
            }
            FireRegisterCalendarCellItem(true);

        }

        private void AddColumns(int totalColumn)
        {
            for (int i = 0; i < totalColumn; i++)
            {
                grd.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        public DateTime SelectedDate
        {
            get
            {
                return new DateTime(_currentYear, _currentMonth, _currentDay);
            }
            set
            {
                _currentDay = value.Day;
                _currentMonth = value.Month;
                _currentYear = value.Year;
                SetMonth();
            }
        }

        private string GetMonthName(MonthNameType monthNameType)
        {
            switch (monthNameType)
            {
                case MonthNameType.JANUARY:
                    return "January";
                case MonthNameType.FEBRUARY:
                    return "February";
                case MonthNameType.MARCH:
                    return "March";
                case MonthNameType.APRIL:
                    return "April";
                case MonthNameType.MAY:
                    return "May";
                case MonthNameType.JUNE:
                    return "June";
                case MonthNameType.JULY:
                    return "July";
                case MonthNameType.AUGUST:
                    return "August";
                case MonthNameType.SEPTEMBER:
                    return "September";
                case MonthNameType.OCTOBER:
                    return "Octobor";
                case MonthNameType.NOVEMBER:
                    return "November";
                case MonthNameType.DECEMBER:
                    return "December";
                default:
                    return "January";
            }
        }

        #region Click Events

        void DayButtonClick(object sender, EventArgs eventArgs)
        {
            var btn = sender as RadioButton;
            switch (_buttonState)
            {
                case CalenderButtonState.Day:
                    _currentDay = Convert.ToInt32(btn.Content);
                    ShowEvent();
                    break;
                case CalenderButtonState.Month:
                    HandleMonthEvent((int)btn.Tag);
                    break;
                case CalenderButtonState.Year:
                    HandleYearEvent((int)btn.Tag);
                    break;
            }

        }

        private void ShowEvent()
        {
            var eventViewer = new CalendarEventViewer();
            eventViewer.EventDate = SelectedDate;
            var top = FrameworkManager.Canvas.ActualHeight / 2 - (eventViewer.Width / 4);
            var left = FrameworkManager.Canvas.ActualWidth / 2 - (eventViewer.Width / 2);
            eventViewer.InitializeControl(FrameworkManager, left, top);
            _eventViewerControls.Add(eventViewer);
            eventViewer.Closed += EventViewerClosed;
            eventViewer.DetailControlAdded += EventViewerDetailControlAdded;
        }

        void EventViewerDetailControlAdded(object sender, Infrasturcture.Global.Helpers.Events.DataEventArgs e)
        {
            var calendarEventDetail = e.Data as CalendarEventDetail;
            calendarEventDetail.Closed += CalendarEventDetailClosed;
            _eventDetailControls.Add(calendarEventDetail);
        }

        void CalendarEventDetailClosed(object sender, EventArgs e)
        {
            _eventDetailControls.Remove((CalendarEventDetail) sender);
        }

        void EventViewerClosed(object sender, EventArgs e)
        {
            _eventViewerControls.Remove((CalendarEventViewer)sender);
        }

        private void BtnLeftClick(object sender, RoutedEventArgs e)
        {
            switch (_buttonState)
            {
                case CalenderButtonState.Day:
                    _currentMonth--;
                    if (_currentMonth > 12)
                    {
                        _currentYear += _currentMonth - 12;
                        _currentMonth = _currentMonth % 12;
                    }

                    if (_currentMonth == 0)
                    {
                        _currentYear -= 1;
                        _currentMonth = 12;
                    }
                    SetMonth();
                    break;
                case CalenderButtonState.Month:
                    _currentYear--;
                    SetYear();
                    break;
            }

        }

        private void BtnRightClick(object sender, RoutedEventArgs e)
        {
            switch (_buttonState)
            {
                case CalenderButtonState.Day:
                    _currentMonth++;
                    if (_currentMonth > 12)
                    {
                        _currentYear += _currentMonth - 12;
                        _currentMonth = _currentMonth % 12;
                    }

                    if (_currentMonth == 0)
                    {
                        _currentYear -= 1;
                        _currentMonth = 12;
                    }
                    SetMonth();
                    break;
                case CalenderButtonState.Month:
                    _currentYear++;
                    SetYear();
                    break;
            }
        }

        private void BtnStateClick(object sender, RoutedEventArgs e)
        {
            switch (_buttonState)
            {
                case CalenderButtonState.Day:
                    SetYear();
                    break;
                case CalenderButtonState.Month:
                    SetYearWithYearItem();
                    break;
                default:
                    break;
            }
        }

        public IEnumerable<IMTouchControl> GetTouchButtons()
        {
            yield return btnLeft;
            yield return btnRight;
            yield return btnState;
        }

        #endregion

        private void HandleYearEvent(int year)
        {
            _currentYear = year;
            SetYear();
        }

        private void HandleMonthEvent(int month)
        {
            _currentMonth = month;
            SetMonth();
        }

        public override void Dispose()
        {
            _calendar = null;
        }
    }
}
