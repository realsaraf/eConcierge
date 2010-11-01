using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CustomControls.Abstract;
using CustomControls.InheritedFrameworkControls;
using CustomControls.OptionControl;
using DataAccessLayer;
using Infrasturcture.DTO;
using Infrasturcture.Global.Helpers.Events;
using Infrasturcture.TouchLibrary;

namespace CustomControls.CalendarControl
{
    /// <summary>
    /// Interaction logic for EventViewer.xaml
    /// </summary>
    public partial class CalendarEventViewer : AnimatableControl, IMTouchControl
    {
        protected IFrameworkManger FrameworkManager { get; set; }
        public IMTContainer Container { get; set; }
        private const int ITEMS_PER_COLUMN = 3;
        public event EventHandler Closed;
        public event EventHandler<DataEventArgs> DetailControlAdded;
        private List<TouchButton> _eventButtons;
        public List<TouchButton> EventButtons
        {
            get { return _eventButtons; }
        }
        private int _totalDays;
        private DateTime _eventDate;
        public DateTime EventDate
        {
            set
            {
                txbDate.Text = value.Day.ToString();
                _eventDate = value;
                _totalDays = DateTime.DaysInMonth(value.Year, value.Month);
            }
            get
            {
                return new DateTime(_eventDate.Year, _eventDate.Month, Convert.ToInt32(txbDate.Text));
            }
        }
        
        public CalendarEventViewer()
        {
            InitializeComponent();
            closeButton.Click += CloseButtonClick;
            PopulateEventCategory();
        }

        public void InitializeControl(IFrameworkManger frameworkManger, double left, double top)
        {
            FrameworkManager = frameworkManger;
            FrameworkManager.RegisterElement((IMTouchControl)closeButton, false, new[] { TouchAction.Tap });
            foreach (IMTouchControl button in GetTouchButtons())
            {
                FrameworkManager.RegisterElement(button, false, new[] { TouchAction.Tap });
            }
            RegisterUnRegisterEventButton(true);
            FrameworkManager.AddControlWithAllGestures(this, left, top);
        }


        public void Close()
        {
            if (FrameworkManager == null) return;
            foreach (IMTouchControl button in GetTouchButtons())
            {
                FrameworkManager.UnRegisterElement(button);
            }
            RegisterUnRegisterEventButton(false);
            FrameworkManager.UnRegisterElement(closeButton);
            FrameworkManager.RemoveControl(this);
            if(Closed!=null)
                Closed(this,new EventArgs());
        }
        private void RegisterUnRegisterEventButton(bool isRegister)
        {
            if (isRegister)
            {
                foreach (TouchButton eventButton in EventButtons)
                {
                    FrameworkManager.RegisterElement((IMTouchControl)eventButton, false, new[] { TouchAction.Tap });
                    eventButton.Click += EventButtonClick;
                }
            }
            else
            {
                foreach (TouchButton eventButton in EventButtons)
                {
                    FrameworkManager.UnRegisterElement(eventButton);
                }
            }
        }

        void EventButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as TouchButton;
            var eventDetail = new CalendarEventDetail();
            eventDetail.EventDate = EventDate;
            eventDetail.CategoryId = Convert.ToInt32(button.Tag);
            var top = FrameworkManager.Canvas.ActualHeight / 2 - (eventDetail.Height / 2);
            var left = FrameworkManager.Canvas.ActualWidth / 2 - (eventDetail.Width / 2);
            eventDetail.InitializeControl(FrameworkManager, left, top);
            if(DetailControlAdded!=null)
                DetailControlAdded(this,new DataEventArgs(eventDetail));
            Close();
        }

        void CloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }
       
        private void PopulateEventCategory()
        {
            _eventButtons = new List<TouchButton>();
            var categoryList = CalendarDAL.GetInstance().GetCategories();
            AddColumns();
            int col = 0,row=-1;

            foreach (DTOEventCategory category in categoryList)
            {
                if(col == 0)
                {
                    grdCategory.RowDefinitions.Add(new RowDefinition());
                    row++;
                }
                TouchOptionItem item = new TouchOptionItem();
                item.CategoryText = category.Title;
                item.CateogoryButton.Tag = category.Id;
                grdCategory.Children.Add(item);
                item.SetValue(Grid.ColumnProperty, col);
                item.SetValue(Grid.RowProperty, row);
                item.Margin = new Thickness(15,5,15,5);
                _eventButtons.Add(item.CateogoryButton);
                col++;
                if (col == ITEMS_PER_COLUMN) col = 0;
            }
        }

        private void AddColumns()
        {
            for(int col=0;col<ITEMS_PER_COLUMN;col++)
            {
                grdCategory.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private void BtnLeftEventCategoryClick(object sender, RoutedEventArgs e)
        {
            int cDay = Convert.ToInt32(txbDate.Text);
            cDay--;
            SetDay(cDay);
        }

        private void SetDay(int cDay)
        {
            btnLeftEventCategory.IsEnabled = cDay != 1;
            btnRightEventCategory.IsEnabled = cDay != _totalDays;
            txbDate.Text = cDay.ToString();
        }

        private void BtnRightEventCategoryClick(object sender, RoutedEventArgs e)
        {
            var cDay = Convert.ToInt32(txbDate.Text);
            cDay++;
            SetDay(cDay);
        }
        public IEnumerable<IMTouchControl> GetTouchButtons()
        {
            yield return btnRightEventCategory;
            yield return btnLeftEventCategory;
        }

    }
}
