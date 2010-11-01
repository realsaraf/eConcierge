using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using Artefact.Animation;
using CustomControls.Abstract;
using CustomControls.TouchCombo;
using DataAccessLayer;
using Infrasturcture;
using Infrasturcture.DTO;
using Infrasturcture.TouchLibrary;
using WPFMitsuControls;
using Image = System.Windows.Controls.Image;
using Path = System.IO.Path;
using TouchAction = Infrasturcture.TouchLibrary.TouchAction;

namespace CustomControls.Dining
{
    /// <summary>
    /// Interaction logic for DiningDetail.xaml
    /// </summary>
    public partial class DiningDetail : AnimatableControl, IMTouchControl
    {
        public IMTContainer Container { get; set; }
        public IFrameworkManger FrameworkManager { get; set; }
        private string _subCategoryId;
        private List<DTODining> _dinings;
        private int _currentPagerIndex;
        private string _categoryId;
        private Book _menuBook;
        public event EventHandler Closed;


        public DiningDetail()
        {
            InitializeComponent();
            categoryCombo.SelectionChanged += CategoryComboSelectionChanged;
            pager.ValueChanged += SldValueChanged;
            closeButton.Click += CloseButtonClick;
            menuButton.Click += MenuButtonClick;
        }

        void CategoryComboSelectionChanged(object sender, Infrasturcture.Global.Helpers.Events.DataEventArgs e)
        {
            SetDiningProperties();
        }

        private void MenuButtonClick(object sender, RoutedEventArgs e)
        {
            if(_menuBook!=null) return;

            _menuBook = new Book();
            MemoryStream sr = null;
            ParserContext pc = null;
            const string xaml = "<DataTemplate><Border BorderThickness=\"0\" BorderBrush=\"Gray\" Background=\"Transparent\"><ContentControl Content=\"{Binding .}\" /></Border></DataTemplate>";
            sr = new MemoryStream(Encoding.ASCII.GetBytes(xaml));
            pc = new ParserContext();
            pc.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            pc.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            var datatemplate = (DataTemplate)XamlReader.Load(sr, pc);
            _menuBook.ItemTemplate = datatemplate;
            var folderName = Directory.GetCurrentDirectory() + "\\Menus\\El Sol Menu";
            var fileNames = Directory.GetFiles(folderName);
            foreach (var fileName in fileNames)
            {

                if (IsImageExt(Path.GetExtension(fileName)))
                {
                    var i = new Image();
                    BitmapSource bi = new BitmapImage(new Uri(fileName));

                    i.Source = bi;
                    bi.Freeze();
                    _menuBook.Items.Add(i);
                    _menuBook.Width = bi.Width * 2;
                    _menuBook.Height = bi.Height;
                }
            }

            var cont = FrameworkManager.AddControlWithAllGestures(_menuBook,100,30);
            //_menuBook.ScaleTo(.5, .5);
            //cont.StartScale = 0.5f;
        }

        bool IsImageExt(string ext)
        {
            string[] exts = { ".jpg", ".png", ".gif", ".tiff", ".bmp", ".jpeg" };
            return exts.Contains(ext.ToLower());
        }

        void CloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        public void Load(IFrameworkManger frameworkManger, string subCategoryId, string categoryId)
        {
            _categoryId = categoryId;
            _subCategoryId = subCategoryId;
            FrameworkManager = frameworkManger;
            FrameworkManager.RegisterElement((IMTouchControl)pager, false, new[] { TouchAction.Slide });
            FrameworkManager.RegisterElement((IMTouchControl)closeButton, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement((IMTouchControl)menuButton, false, new[] { TouchAction.Tap });
            categoryCombo.Initialize(FrameworkManager, GetCategoryComboItems());
            categoryCombo.SelectedItem = _subCategoryId;
            FrameworkManager.AddControlWithAllGestures(this, 20, 20);
            SetDiningProperties();
        }

        private List<TouchComboBoxItem> GetCategoryComboItems()
        {
            var categoryComboItems = new List<TouchComboBoxItem>();
            var categoryList = DiningDAL.GetInstance().GetSubCategories(Convert.ToInt32(_categoryId));
            foreach (var category in categoryList)
            {
                var categoryComboItem = new TouchComboBoxItem();
                categoryComboItem.DisplayText = category.Title;
                categoryComboItem.Item = category.Id;
                categoryComboItems.Add(categoryComboItem);
            }
            return categoryComboItems;
        }

        public void Close()
        {
            if (FrameworkManager == null) return;
            FrameworkManager.UnRegisterElement(pager);
            FrameworkManager.UnRegisterElement(closeButton);
            FrameworkManager.UnRegisterElement(menuButton);
            categoryCombo.Close();
            if(_menuBook!=null)
                FrameworkManager.RemoveControl(_menuBook);
            if (Closed != null)
                Closed(this, new EventArgs());
            FrameworkManager.RemoveControl(this);
        }

        void SldValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int newValue = Convert.ToInt32(e.NewValue);
            if (newValue == _currentPagerIndex + 1 || newValue == _currentPagerIndex - 1)
            {
                _currentPagerIndex = newValue;
                SetDiningProperties(_dinings[newValue]);
            }

        }

        private void SetDiningProperties()
        {
            _dinings = DiningDAL.GetInstance().GetDinings(Convert.ToInt32(categoryCombo.SelectedItem));
            _currentPagerIndex = 0;
            pager.Minimum = 0;
            pager.Maximum = _dinings.Count - 1;
            pager.Value = 0;
            SetDiningProperties(_dinings[0]);
        }

        private void SetDiningProperties(DTODining dining)
        {
            imgEvent.Source = WpfUtil.BytesToImageSource(dining.Photo);
            txbTitle.Text = dining.Title;
            txbDescription.Text = dining.Description;
            txbFooter.Text = dining.Location;
        }
    }
}
