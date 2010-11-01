using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using CustomControls.InheritedFrameworkControls;
using Helpers.Converters;
using Infrasturcture.TouchLibrary;

namespace CustomControls.FiveButtonDiskControl
{
    public partial class FiveButtonDiskControl : UserControl, IMTouchControl
    {
        #region Properties
        [TypeConverter(typeof(PathToImageSourceConverter))]
        public string TopLeftImage
        {
            get { return (string)GetValue(TopLeftImageProperty); }
            set { SetValue(TopLeftImageProperty, value); }
        }

        public static readonly DependencyProperty TopLeftImageProperty =
            DependencyProperty.Register("TopLeftImage", typeof(string), typeof(FiveButtonDiskControl));

        [TypeConverter(typeof(PathToImageSourceConverter))]
        public string TopRightImage
        {
            get { return (string)GetValue(TopRightImageProperty); }
            set { SetValue(TopRightImageProperty, value); }
        }

        public static readonly DependencyProperty TopRightImageProperty =
            DependencyProperty.Register("TopRightImage", typeof(string), typeof(FiveButtonDiskControl));

        [TypeConverter(typeof(PathToImageSourceConverter))]
        public string BottomLeftImage
        {
            get { return (string)GetValue(BottomLeftImageProperty); }
            set { SetValue(BottomLeftImageProperty, value); }
        }

        public static readonly DependencyProperty BottomLeftImageProperty =
            DependencyProperty.Register("BottomLeftImage", typeof(string), typeof(FiveButtonDiskControl));

        [TypeConverter(typeof(PathToImageSourceConverter))]
        public string BottomRightImage
        {
            get { return (string)GetValue(BottomRightImageProperty); }
            set { SetValue(BottomRightImageProperty, value); }
        }

        public static readonly DependencyProperty BottomRightImageProperty =
            DependencyProperty.Register("BottomRightImage", typeof(string), typeof(FiveButtonDiskControl));


        public string TopLeftText
        {
            get { return (string)GetValue(TopLeftTextProperty); }
            set { SetValue(TopLeftTextProperty, value); }
        }

        public static readonly DependencyProperty TopLeftTextProperty =
            DependencyProperty.Register("TopLeftText", typeof(string), typeof(FiveButtonDiskControl));


        public string TopRightText
        {
            get { return (string)GetValue(TopRightTextProperty); }
            set { SetValue(TopRightTextProperty, value); }
        }

        public static readonly DependencyProperty TopRightTextProperty =
            DependencyProperty.Register("TopRightText", typeof(string), typeof(FiveButtonDiskControl));


        public string BottomLeftText
        {
            get { return (string)GetValue(BottomLeftTextProperty); }
            set { SetValue(BottomLeftTextProperty, value); }
        }

        public static readonly DependencyProperty BottomLeftTextProperty =
            DependencyProperty.Register("BottomLeftText", typeof(string), typeof(FiveButtonDiskControl));


        public string BottomRightText
        {
            get { return (string)GetValue(BottomRightTextProperty); }
            set { SetValue(BottomRightTextProperty, value); }
        }

        public static readonly DependencyProperty BottomRightTextProperty =
            DependencyProperty.Register("BottomRightText", typeof(string), typeof(FiveButtonDiskControl));

        public TouchButton TopLeftButton
        {
            get
            {
                return topLeftButton;
            }
        }

        public TouchButton TopRightButton
        {
            get
            {
                return topRightButton;
            }
        }

        public TouchButton BottomLeftButton
        {
            get
            {
                return bottomLeftButton;
            }
        }

        public TouchButton BottomRightButton
        {
            get
            {
                return bottomRightButton;
            }
        }

        public TouchButton ExpandButton
        {
            get
            {
                return expandButton;
            }
        }

        public TouchButton CollapseButton
        {
            get
            {
                return collapseButton;
            }
        }
        #endregion

        public readonly double ActualSize = 337.5;
        private readonly Storyboard _collapseStoryboard;
        private readonly Storyboard _expandStoryboard;
        public event EventHandler TopRightButtonClick;
        public event EventHandler BottomRightButtonClick;
        public event EventHandler BottomLeftButtonClick;
        public event EventHandler TopLeftButtonClick;
        public event EventHandler Collapsed;
        public IMTContainer Container { get; set; }
        protected IFrameworkManger FrameworkManager { get; set; }

        public FiveButtonDiskControl()
        {
            DataContext = this;
            InitializeComponent();
            expandButton.Click += ExpandButtonClick;
            topRightButton.Click += TopRightButtonOnClick;
            topLeftButton.Click += TopLeftButtonOnClick;
            bottomRightButton.Click += BottomRightButtonOnClick;
            bottomLeftButton.Click += BottomLeftButtonOnClick;
            _collapseStoryboard= (Storyboard)FindResource("CollapseStoryBoard");
            _expandStoryboard = (Storyboard)FindResource("ExpandStoryBoard");
            _collapseStoryboard.Completed += CollapseStoryboardCompleted;
        }

        void CollapseStoryboardCompleted(object sender, EventArgs e)
        {
            if(Collapsed!=null)
                Collapsed(this,new EventArgs());
        }


        public void InitializeControl(IFrameworkManger frameworkManager, double left, double top)
        {
            FrameworkManager = frameworkManager;
            FrameworkManager.RegisterElement((IMTouchControl)TopLeftButton, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement((IMTouchControl)TopRightButton, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement((IMTouchControl)BottomLeftButton, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement((IMTouchControl)BottomRightButton, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement((IMTouchControl)CollapseButton, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement((IMTouchControl)ExpandButton, false, new[] { TouchAction.Tap });
            FrameworkManager.AddControlWithAllGestures(this, left, top);
        }

        public void Close()
        {
            FrameworkManager.UnRegisterElement(ExpandButton);
            FrameworkManager.UnRegisterElement(CollapseButton);
            FrameworkManager.UnRegisterElement(TopLeftButton);
            FrameworkManager.UnRegisterElement(TopRightButton);
            FrameworkManager.UnRegisterElement(BottomLeftButton);
            FrameworkManager.UnRegisterElement(BottomRightButton);
            FrameworkManager.RemoveControl(this);
        }

        public void InitializeControlData(string topLeftImage, string bottomLeftImage, string bottomRightImage, string topRightImage, string topLeftText, string bottomLeftText, string bottomRightText, string topRightText)
        {
            TopLeftText = topLeftText;
            TopRightText = topRightText;
            BottomLeftText = bottomLeftText;
            BottomRightText = bottomRightText;

            TopLeftImage = topLeftImage;
            TopRightImage = topRightImage;
            BottomLeftImage = bottomLeftImage;
            BottomRightImage = bottomRightImage;
        }

        public void Expand()
        {
            _expandStoryboard.Begin();
        }

        public void Collapse()
        {
            _collapseStoryboard.Begin();
        }

        void BottomLeftButtonOnClick(object sender, RoutedEventArgs e)
        {
            OnBottomLeftClick(new EventArgs());
        }


        void BottomRightButtonOnClick(object sender, RoutedEventArgs e)
        {
            OnBottomRightClick(new EventArgs());
        }

        void TopLeftButtonOnClick(object sender, RoutedEventArgs e)
        {
            OnTopLeftClick(new EventArgs());
        }

        static void ExpandButtonClick(object sender, RoutedEventArgs e)
        {
        }

        void TopRightButtonOnClick(object sender, RoutedEventArgs e)
        {
            OnTopRightClick(new EventArgs());
        }

        protected virtual void OnTopRightClick(EventArgs e)
        {
            if (TopRightButtonClick != null)
                TopRightButtonClick(this, e);
        }

        protected virtual void OnBottomLeftClick(EventArgs e)
        {
            if (BottomLeftButtonClick != null)
                BottomLeftButtonClick(this, e);
        }

        protected virtual void OnBottomRightClick(EventArgs e)
        {
            if (BottomRightButtonClick != null)
                BottomRightButtonClick(this, e);
        }

        protected virtual void OnTopLeftClick(EventArgs e)
        {
            if (TopLeftButtonClick != null)
                TopLeftButtonClick(this, e);
        }

        public void RaiseClose()
        {
            if (TopRightButtonClick != null)
                TopRightButtonClick(this, new EventArgs());
        }
    }
}