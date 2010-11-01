using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using CustomControls.Abstract;
using CustomControls.TouchCombo;
using DataAccessLayer;
using Infrasturcture.Global.Helpers.Events;
using Infrasturcture.TouchLibrary;
using TouchAction = Infrasturcture.TouchLibrary.TouchAction;

namespace CustomControls.Transportation
{
    /// <summary>
    /// Interaction logic for TransportationDetail.xaml
    /// </summary>
    public partial class TransportationDetail : AnimatableControl, IMTouchControl
    {
        private TaxiDetail _taxiDetail;
        private MonorailDetail _monorailDetail;
        public IFrameworkManger FrameworkManager { get; set; }
        private int _currentPagerIndex;
        public event EventHandler Closed;

        private TaxiDetail TaxiDetail
        {
            get
            {
                return _taxiDetail ?? (_taxiDetail = new TaxiDetail());

            }
        }

        private MonorailDetail MonorailDetail
        {
            get
            {
                return _monorailDetail ?? (_monorailDetail = new MonorailDetail());

            }
        }

        public IMTContainer Container { get; set; }

        public string CategoryId { get; set; }

        public TransportationDetail()
        {
            InitializeComponent();
            categoryCombo.SelectionChanged += ComboItems_SelectionChanged;
            pager.sld.ValueChanged += SldValueChanged;
            closeButton.Click += CloseButtonClicked;
        }

        public void Load(IFrameworkManger frameworkManger, double left, double top)
        {
            FrameworkManager = frameworkManger;
            FrameworkManager.RegisterElement((IMTouchControl)closeButton, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement(pager.sld as IMTouchControl, false, new[] { TouchAction.Slide });
            FrameworkManager.RegisterElement(categoryCombo as IMTouchControl, false, new[] { TouchAction.Tap });
            categoryCombo.Initialize(FrameworkManager, GetCategoryComboItems());
            categoryCombo.SetSelectedIndex(0);
            FrameworkManager.AddControlWithAllGestures(this, left, top);
            PopulateSubCategory();
        }

        private List<TouchComboBoxItem> GetCategoryComboItems()
        {
            var categoryComboItems = new List<TouchComboBoxItem>();
            var categoryList = TransportationDAL.GetInstance().GetCategories();
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
            FrameworkManager.UnRegisterElement(pager.sld);
            categoryCombo.Close();
            FrameworkManager.RemoveControl(this);
            if(Closed!=null)
                Closed(this,new EventArgs());
        }

        void SldValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int newValue = Convert.ToInt32(e.NewValue);
            if (newValue == _currentPagerIndex + 1 || newValue == _currentPagerIndex - 1)
            {
                _currentPagerIndex = newValue;
                if (IsTaxi(Convert.ToInt32(CategoryId)))
                    TaxiDetail.PopulateTaxiDetail(_currentPagerIndex);
                else if (IsMonorail(Convert.ToInt32(CategoryId)))
                {
                    MonorailDetail.PopulateMonorailDetail(_currentPagerIndex);
                }
            }

        }

        void ComboItems_SelectionChanged(object sender, DataEventArgs dataEventArgs)
        {
            CategoryId = categoryCombo.SelectedItem.ToString();
            PopulateSubCategory();
        }


        private void PopulateSubCategory()
        {
            int id = Convert.ToInt32(CategoryId);
            stkBody.Children.Clear();
            if (IsTaxi(id))
            {
                pager.sld.Maximum = TaxiDetail.SetTaxi(id) - 1;
                TaxiDetail.PopulateTaxiDetail();
                _currentPagerIndex = 0;
                stkBody.Children.Add(TaxiDetail);
                btnSeeMap.Visibility = Visibility.Collapsed;
            }
            else if (IsMonorail(id))
            {
                pager.sld.Maximum = MonorailDetail.SetMonorail(id) - 1;
                MonorailDetail.Background = Brushes.SkyBlue;
                _currentPagerIndex = 0;
                stkBody.Children.Add(MonorailDetail);
                btnSeeMap.Visibility = Visibility.Visible;
            }

        }

        private bool IsMonorail(int id)
        {
            return id == 1 || id == 2 || id == 5;
        }

        private bool IsTaxi(int id)
        {
            return id == 3 || id == 4 || id == 6;
        }
    }
}
