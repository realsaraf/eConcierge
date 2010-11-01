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
using CustomControls.InheritedFrameworkControls;
using Infrasturcture.Global.Helpers.Events;
using Infrasturcture.TouchLibrary;
using TouchAction = Infrasturcture.TouchLibrary.TouchAction;

namespace CustomControls.TouchCombo
{
    /// <summary>
    /// Interaction logic for TouchComboBox.xaml
    /// </summary>
    public partial class TouchComboBox : UserControl, IMTouchControl
    {
        private static bool _skipOnchanged;
        private IFrameworkManger _frameworkManger;
        public IMTContainer Container { get; set; }
        private List<TouchRadioButton> _options = new List<TouchRadioButton>();
        public event EventHandler<DataEventArgs> SelectionChanged;

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(TouchComboBox), new FrameworkPropertyMetadata(OnDpPropertyChanged));

        private static void OnDpPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var combo = d as TouchComboBox;
            var id = e.NewValue.ToString();
            var item = combo._items.Single(i => i.Item.ToString().Equals(id));
            combo.SelectedTitle = item.DisplayText;
        }


        public string SelectedTitle
        {
            get { return (string)GetValue(SelectedTitleProperty); }
            set { SetValue(SelectedTitleProperty, value); }
        }

        private int _selectedIndex;

        public void SetSelectedIndex(int value)
        {
            _selectedIndex = value;
            var item = _items.First();
            SelectedItem = item.Item;

        }

        public int GetSelectedIndex()
        {
            return _selectedIndex;
        }

        public static readonly DependencyProperty SelectedTitleProperty =
            DependencyProperty.Register("SelectedTitle", typeof(string), typeof(TouchComboBox));

        private List<TouchComboBoxItem> _items;

        public TouchComboBox()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void Initialize(IFrameworkManger frameworkManger, List<TouchComboBoxItem> items)
        {
            _items = items;
            _frameworkManger = frameworkManger;
            _frameworkManger.RegisterElement((IMTouchControl) titleToggleButton, false, new TouchAction[] {TouchAction.Tap});
            itemsContainer.Children.Clear();
            foreach (var item in items)
            {
                var touchButton = new TouchRadioButton();
                touchButton.Content = item.Content;
                touchButton.Tag = item;
                touchButton.Foreground = Brushes.White;
                touchButton.FontWeight = FontWeights.Bold;
                touchButton.Style = (Style) FindResource("ComboTouchRadioButtonStyle");
                touchButton.Content = item.DisplayText;
                touchButton.Click += TouchButtonClick;
                itemsContainer.Children.Add(touchButton);
                _frameworkManger.RegisterElement((IMTouchControl)touchButton, false, new TouchAction[] {TouchAction.Tap});
                _options.Add(touchButton);
            }
        }

        public void Close()
        {
            _frameworkManger.UnRegisterElement(titleToggleButton);
            foreach (var touchButton in _options)
            {
                _frameworkManger.UnRegisterElement(touchButton);
            }
        }

        void TouchButtonClick(object sender, RoutedEventArgs e)
        {
            var item = ((TouchComboBoxItem)(sender as TouchRadioButton).Tag);
            SelectedItem = item.Item;
            SelectedTitle = item.DisplayText;
            titleToggleButton.IsChecked = false;
            if(SelectionChanged!=null)
                SelectionChanged(this,new DataEventArgs(SelectedItem));
        }


        private void TitleToggleButtonChecked(object sender, RoutedEventArgs e)
        {
            border.Background =new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4E0000"));
            border.BorderThickness=new Thickness(3);
            optionsBorder.Width = Double.NaN;
            optionsBorder.Height = Double.NaN;
        }

        private void TitleToggleButtonUnchecked(object sender, RoutedEventArgs e)
        {
            border.Background = Brushes.Transparent;
            border.BorderThickness = new Thickness(0);
            optionsBorder.Width = 0;
            optionsBorder.Height = 0;
        }

    }
}
