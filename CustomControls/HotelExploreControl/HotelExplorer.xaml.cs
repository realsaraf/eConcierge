using System;
using System.Windows;
using System.Windows.Controls;
using CustomControls.Abstract;
using CustomControls.InheritedFrameworkControls;
using Infrasturcture.Global.Helpers.Events;
using Infrasturcture.TouchLibrary;

namespace CustomControls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class HotelExplorer : AnimatableControl, IMTouchControl
    {
        private static HotelExplorer _hotelExplorer;
        public event EventHandler OnDoAnimateToOrigin;
        public event EventHandler<DataEventArgs> Drag;
        public event EventHandler Closed;
        public HotelExplorer()
        {
            InitializeComponent();
            RegisterEvents();
        }

        public static HotelExplorer GetInstance()
        {
            if (_hotelExplorer == null)
                _hotelExplorer = new HotelExplorer();
            return _hotelExplorer;
        }
        
        public static IMTContainer GetContainer()
        {
            return _hotelExplorer.Container;
        }

        public override void HightLight(bool shouldHighLight)
        {
            var thickness = shouldHighLight ? 3 : 0;
            OuterRim.BorderThickness = new Thickness(thickness);
        }

        private void RegisterEvents()
        {
            ((RadioButton)TourVideoOption).Checked += OptionChecked;
            ((RadioButton)HotelMapOption).Checked += OptionChecked;
            ((RadioButton)PhotoGalleryOption).Checked += OptionChecked;
            ((RadioButton)AccomodationsOption).Checked += OptionChecked;

            ((RadioButton)TourVideoOption).Unchecked += OptionUnChecked;
            ((RadioButton)HotelMapOption).Unchecked += OptionUnChecked; 
            ((RadioButton)PhotoGalleryOption).Unchecked += OptionUnChecked;
            ((RadioButton)AccomodationsOption).Unchecked += OptionUnChecked;

            closeButton.Click += CloseButtonClick;
        }

        void CloseButtonClick(object sender, EventArgs e)
        {
            if(Closed!=null)
                Closed(this,new EventArgs());
        }

        private void UnRegisterEvents()
        {
            ((RadioButton)TourVideoOption).Checked -= OptionChecked;
            ((RadioButton)HotelMapOption).Checked -= OptionChecked;
            ((RadioButton)PhotoGalleryOption).Checked -= OptionChecked;
            ((RadioButton)AccomodationsOption).Checked -= OptionChecked;

            ((RadioButton)TourVideoOption).Unchecked -= OptionUnChecked;
            ((RadioButton)HotelMapOption).Unchecked -= OptionUnChecked;
            ((RadioButton)PhotoGalleryOption).Unchecked -= OptionUnChecked;
            ((RadioButton)AccomodationsOption).Unchecked -= OptionUnChecked;
        }

        private void OptionUnChecked(object sender, RoutedEventArgs e)
        {
            if (OptionUnUnChecked != null)
                OptionUnUnChecked(sender, e);
        }

        public void ClearChoices()
        {
            ((RadioButton)TourVideoOption).IsChecked = false;
            ((RadioButton)HotelMapOption).IsChecked = false;
            ((RadioButton)PhotoGalleryOption).IsChecked = false;
            ((RadioButton)AccomodationsOption).IsChecked = false;

        }
        
        void OptionChecked(object sender, RoutedEventArgs e)
        {
            if (OptionClicked != null)
                OptionClicked(sender, e);
        }

        public IMTContainer Container { get; set; }
        public event EventHandler OptionClicked;
        public event EventHandler OptionUnUnChecked;

        public IMTouchControl TourVideoOption
        {
            get
            {
                return TourVideo;
            }
            set
            {
                TourVideo = value as TouchRadioButton;
            }
        }

        public IMTouchControl AccomodationsOption
        {
            get
            {
                return Accomodations;
            }
            set
            {
                Accomodations = value as TouchRadioButton;
            }
        }

        public IMTouchControl HotelMapOption
        {
            get
            {
                return HotelMap;
            }
            set
            {
                HotelMap = value as TouchRadioButton;
            }
        }

        public IMTouchControl PhotoGalleryOption
        {
            get
            {
                return PhotoGallery;
            }
            set
            {
                PhotoGallery = value as TouchRadioButton;
            }
        }

        public bool IsFloating { get; set; }

        public UIElement Connector { get; set; }

        public IMTouchControl CurrentDisplayedControl { get; set; }

        public IMTouchControl CloseButton
        {
            get
            {
                return closeButton;
            }
            
        }

        public void MoveToOrigin()
        {
            if(OnDoAnimateToOrigin!=null)
            {
                OnDoAnimateToOrigin(this,new EventArgs());
            }
        }

        public override void Dispose()
        {
            UnRegisterEvents();
            _hotelExplorer = null;
        }

        public void Dragged(float x, float y)
        {
            var data = new float[2];
            data[0] = x;
            data[1] = y;
            if (Drag != null)
                Drag(this, new DataEventArgs(data));
        }
    }
}
