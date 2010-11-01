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
using CustomControls.CalendarControl;
using CustomControls.CategoryControl;
using CustomControls.InheritedFrameworkControls;
using CustomControls.OptionControl;
using CustomControls.TouchCombo;
using DataAccessLayer;
using Infrasturcture.DTO;
using Infrasturcture.Global.Helpers.Events;
using Infrasturcture.TouchLibrary;
using TouchFramework.Events;
using TouchAction = Infrasturcture.TouchLibrary.TouchAction;

namespace CustomControls.Dining
{
    /// <summary>
    /// Interaction logic for DiningSubCateogryControl.xaml
    /// </summary>
    public partial class DiningSubCateogryControl : AnimatableControl, IMTouchControl
    {
        private const int NO_OF_ITEMS_PER_COLUMN = 2;
        private readonly List<IMTouchControl> _subCategoryItems = new List<IMTouchControl>();
        public IFrameworkManger FrameworkManager { get; set; }
        private string _categoryId;
        private readonly List<TouchButton> _comboItems = new List<TouchButton>();
        public event EventHandler Closed;
        public event EventHandler<DataEventArgs> DetailControlAdded;

        public void InvokeDetailControlAdded(DataEventArgs e)
        {
            EventHandler<DataEventArgs> handler = DetailControlAdded;
            if (handler != null) handler(this, e);
        }

        #region Properties
        
        public IMTContainer Container { get; set; }

        #endregion

        public DiningSubCateogryControl()
        {
            InitializeComponent();
            categoryCombo.SelectionChanged += ComboItems_SelectionChanged;
            closeButton.Click += CloseButtonClicked;     
        }
        
        public void InitializeControl(IFrameworkManger frameworkManger, string categoryId)
        {
            FrameworkManager = frameworkManger;
            FrameworkManager.RegisterElement(categoryCombo as IMTouchControl, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement((IMTouchControl)closeButton, false, new[] { TouchAction.Tap });
            foreach (TouchButton item in _comboItems)
            {
                FrameworkManager.RegisterElement(item as IMTouchControl, false, new[] { TouchAction.Tap });

            }
            _categoryId = categoryId;
            categoryCombo.Initialize(FrameworkManager, GetCategoryComboItems());
            categoryCombo.SelectedItem = _categoryId;
            FrameworkManager.AddControlWithAllGestures(this, 20, 20);
            PopulateSubCategory();
        }

        private List<TouchComboBoxItem> GetCategoryComboItems()
        {
            var categoryComboItems = new List<TouchComboBoxItem>();
            var categoryList = DiningDAL.GetInstance().GetCategories();
            foreach (var category in categoryList)
            {
                var categoryComboItem = new TouchComboBoxItem();
                categoryComboItem.DisplayText = category.Title;
                categoryComboItem.Item = category.Id;
                categoryComboItems.Add(categoryComboItem);
            }
            return categoryComboItems;
        }

        private void CloseButtonClicked(object sender, EventArgs e)
        {
            Close();
        }

        public void Close()
        {
            FrameworkManager.UnRegisterElement(categoryCombo);
            FrameworkManager.UnRegisterElement(closeButton);
            FrameworkManager.RemoveControl(this);
            if(Closed!=null)
                Closed(this,new EventArgs());
        }

        void ComboItems_SelectionChanged(object sender, DataEventArgs dataEventArgs)
        {
            _categoryId = categoryCombo.SelectedItem.ToString();
            PopulateSubCategory();
        }
      
        private void PopulateSubCategory()
        {
            grdCategory.Children.Clear();
            grdCategory.RowDefinitions.Clear();
            grdCategory.ColumnDefinitions.Clear();
            if(_subCategoryItems.Count > 0)
            {
                foreach (var item in _subCategoryItems)
                {
                    FrameworkManager.UnRegisterElement(item);
                }
                _subCategoryItems.Clear();
            }
            var categoryList = DiningDAL.GetInstance().GetSubCategories(Convert.ToInt32(_categoryId));
            AddColumns();
            int col = 0, row = -1;

            foreach (var category in categoryList)
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
                item.Margin = new Thickness(15, 10, 15, 10);
                _subCategoryItems.Add(item.CateogoryButton);
                FrameworkManager.RegisterElement(item.CateogoryButton as IMTouchControl, false, new[] { TouchAction.Tap });
                item.CateogoryButton.Click += CateogoryButton_Click;
                col++;
                if (col == NO_OF_ITEMS_PER_COLUMN) col = 0;
            }
        }


        void CateogoryButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (TouchButton)sender;
            var diningDetail = new DiningDetail();
            InvokeDetailControlAdded(new DataEventArgs(diningDetail));
            var subCategoryId = button.Tag.ToString();
            Close();
            diningDetail.Load(FrameworkManager, subCategoryId,_categoryId);
        }

        private void AddColumns()
        {
            for (int col = 0; col < NO_OF_ITEMS_PER_COLUMN; col++)
            {
                grdCategory.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

    }
}
