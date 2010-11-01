using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CustomControls.InheritedFrameworkControls;

namespace CustomControls.CategoryControl
{
    /// <summary>
    /// Interaction logic for ComboItems.xaml
    /// </summary>
    public partial class ComboItems : UserControl
    {
        public ComboItems()
        {
            InitializeComponent();
        }
        
        private string _selectedValue;
        public string SelectedValue
        {
            set
            {
                _selectedValue = value;
                foreach (var child in stkItems.Children)
                {
                    Button btn = child as Button;
                    if (btn.Tag.ToString().Equals(value))
                    {
                        Button_Click(btn, null);
                        break;
                    }

                }
            }
            get
            {
                return stkItems.Tag.ToString();
            }
        }

        public event EventHandler<SelectedItemEvntArgs> SelectionChanged;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
        public TouchButton AddItems(string text, string value)
        {
            TouchButton btn = new TouchButton();
            btn.Content = text;
            btn.Tag = value;
            btn.Height = 30;
            btn.Foreground = Brushes.White;
            btn.FontSize = 18;
            btn.HorizontalContentAlignment = HorizontalAlignment.Left;
            btn.Template = this.FindResource("ButtonBlankControlTemplate") as ControlTemplate;
            btn.Click += Button_Click;
            stkItems.Children.Add(btn);
            return btn;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Visibility == Visibility.Collapsed && e != null) return;
            Button btn = sender as Button;
            if (SelectionChanged != null)
            {
                stkItems.Tag = btn.Tag;
                SelectionChanged(sender, new SelectedItemEvntArgs() { SelectedText = btn.Content.ToString(), SelectedValue = btn.Tag.ToString() });
            }
            Grid parent = this.Parent as Grid;
            if (parent != null)
            {
                parent.Children.Remove(this);
            }

        }
    }
    public class SelectedItemEvntArgs : EventArgs
    {
        public string SelectedValue { get; set; }
        public string SelectedText { get; set; }
    }
}
