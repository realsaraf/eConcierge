using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Helpers.Converters;
using Infrasturcture.TouchLibrary;

namespace CustomControls.CircularButton
{
    /// <summary>
    /// Interaction logic for CircularCloseButtonControl.xaml
    /// </summary>
    public partial class CircularToggleButton : UserControl, IMTouchControl
    {
        #region Properties
        public IMTContainer Container { get; set; }

        public event EventHandler Checked;
        public event EventHandler UnChecked;
        public bool IsChecked
        {
            get
            {
                return (bool) CommandToggleButton.IsChecked;
            }
            set
            {
                CommandToggleButton.IsChecked = value;
            }
        }


        [TypeConverter(typeof(PathToImageSourceConverter))] 
        public ImageSource CheckedImage
        {
            get { return (ImageSource)GetValue(CheckedImageProperty); }
            set { SetValue(CheckedImageProperty, value); }
        }

        public static readonly DependencyProperty CheckedImageProperty =
            DependencyProperty.Register("CheckedImage", typeof(ImageSource), typeof(CircularToggleButton));

        [TypeConverter(typeof(PathToImageSourceConverter))] 
        public ImageSource UncheckedImage
        {
            get { return (ImageSource)GetValue(UncheckedImageProperty); }
            set { SetValue(UncheckedImageProperty, value); }
        }

        public static readonly DependencyProperty UncheckedImageProperty =
            DependencyProperty.Register("UncheckedImage", typeof(ImageSource), typeof(CircularToggleButton));

        #endregion

        public CircularToggleButton()
        {
            InitializeComponent();
            Loaded += CircularToggleButtonLoaded;
            
        }

        void CircularToggleButtonLoaded(object sender, RoutedEventArgs e)
        {
            var imageBrush = new ImageBrush(UncheckedImage) { Stretch = Stretch.None };
            CommandToggleButton.Background = imageBrush;
        }

        private void CommandToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            var imageBrush = new ImageBrush(CheckedImage) {Stretch = Stretch.None};
            CommandToggleButton.Background = imageBrush;
            if(Checked!=null)
                Checked(this,new EventArgs());
        }

        private void CommandToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            var imageBrush = new ImageBrush(UncheckedImage) {Stretch = Stretch.None};
            CommandToggleButton.Background = imageBrush;
            if (UnChecked != null)
                UnChecked(this, new EventArgs());
        }
    }
}
