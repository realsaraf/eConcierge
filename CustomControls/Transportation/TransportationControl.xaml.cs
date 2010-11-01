using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CustomControls.Abstract;
using CustomControls.CalendarControl;
using CustomControls.CategoryControl;
using CustomControls.InheritedFrameworkControls;
using CustomControls.OptionControl;
using DataAccessLayer;
using Infrasturcture.DTO;
using Infrasturcture.Global.Helpers.Events;
using Infrasturcture.TouchLibrary;

namespace CustomControls.Transportation
{
    /// <summary>
    /// Interaction logic for CalendarControl.xaml
    /// </summary>
    public partial class TransportationControl : AnimatableControl, IMTouchControl
    {
        private List<TouchButton> _transportationButtons;
        private const int NO_OF_ITEM_IN_COLUMN = 2;
        public event EventHandler Closed;
        public IFrameworkManger FrameworkManager { get; set; }
        private static TransportationControl _transportation;
        public event EventHandler<DataEventArgs> DetailControlAdded;
        public void InvokeDetailControlAdded(DataEventArgs e)
        {
            EventHandler<DataEventArgs> handler = DetailControlAdded;
            if (handler != null) handler(this, e);
        }
        
        #region Properties

        public IMTContainer Container { get; set; }

        #endregion

        public TransportationControl()
        {
            InitializeComponent();
            PopulateEventCategory();
            closeButton.Click += CloseButtonClicked;
        }

        public void Load(IFrameworkManger frameworkManger, double left, double top)
        {
            FrameworkManager = frameworkManger;
            FrameworkManager.RegisterElement((IMTouchControl)closeButton, false, new[] { TouchAction.Tap });
            foreach (TouchButton eventButton in _transportationButtons)
            {
                FrameworkManager.RegisterElement(eventButton as IMTouchControl, false, new[] { TouchAction.Tap });
            }
            FrameworkManager.AddControlWithAllGestures(this,left,top);
        }

        private void CloseButtonClicked(object sender, EventArgs e)
        {
            Close();
        }

        public void Close()
        {
            FrameworkManager.UnRegisterElement(closeButton);
            foreach (TouchButton eventButton in _transportationButtons)
            {
                FrameworkManager.UnRegisterElement(eventButton);
            }
            FrameworkManager.RemoveControl(this);
            
            if(Closed != null)
            {
                Closed(null, null);
            }
        }

        private void PopulateEventCategory()
        {
            _transportationButtons = new List<TouchButton>();
            var categoryList = TransportationDAL.GetInstance().GetCategories();
            AddColumns();
            int col = 0, row = -1;

            foreach (DTOTransportationCategory category in categoryList)
            {
                if (col == 0)
                {
                    grdCategory.RowDefinitions.Add(new RowDefinition());
                    row++;
                }
                var item = new TouchOptionItem();
                item.CategoryText = category.Title;
                item.CateogoryButton.Tag = category.Id;
                grdCategory.Children.Add(item);
                item.SetValue(Grid.ColumnProperty, col);
                item.SetValue(Grid.RowProperty, row);
                if (col == 0)
                    item.Margin = new Thickness(10, 5, 10, 0);
                else
                    item.Margin = new Thickness(100, 5, 10, 0);

                _transportationButtons.Add(item.CateogoryButton);
                item.CateogoryButton.Click += CateogoryButton_Click;
                col++;
                if (col == NO_OF_ITEM_IN_COLUMN) col = 0;
            }
        }

        void CateogoryButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (TouchButton)sender;
            if (button == null) return;
            var transportationDetail = new TransportationDetail();
            var categoryId = button.Tag.ToString();
            transportationDetail.CategoryId = categoryId;
            InvokeDetailControlAdded(new DataEventArgs(transportationDetail));
            var top = FrameworkManager.Canvas.ActualHeight / 2 - (transportationDetail.Height / 2);
            var left = FrameworkManager.Canvas.ActualWidth / 2 - (transportationDetail.Width / 2);
            transportationDetail.Load(FrameworkManager, left, top);

        }

        private void AddColumns()
        {
            for (int col = 0; col < NO_OF_ITEM_IN_COLUMN; col++)
            {
                grdCategory.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }
    }
}
