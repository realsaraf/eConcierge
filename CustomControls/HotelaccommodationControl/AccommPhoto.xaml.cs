using System;
using System.Collections.Generic;
using System.Drawing;
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
using Infrasturcture;
using Infrasturcture.TouchLibrary;
using Point = System.Windows.Point;

namespace CustomControls.HotelaccommodationControl
{
    /// <summary>
    /// Interaction logic for AccommPhoto.xaml
    /// </summary>
    public partial class AccommPhoto : AnimatableControl, IMTouchControl
    {
        public AccommPhoto()
        {
            InitializeComponent();
        }

        public IMTContainer Container
        {
            get; set;
        }

        private byte[] _imageData;
        public byte[] ImageData
        {
            set
            {
               
                _imageData = value;
                imgAccomm.Source = WpfUtil.BytesToImageSource(value);
            }
            get
            {
                return _imageData;
            }
        }

        public bool IsDisplayed { get; set; }

        public void OnTapped()
        {
            if(Tapped!=null)
                Tapped(this,new RoutedEventArgs());
        }

        public event EventHandler<RoutedEventArgs> Tapped;
        public event EventHandler<PointEventArgs> Dragged;


        public void OnDrag(PointF pointF)
        {
            if(Dragged != null)
            {
                Dragged(this, new PointEventArgs() {PointF = pointF});
            }
        }
    }
    public class PointEventArgs:RoutedEventArgs
    {
        public PointF PointF { get; set; }
    }
}
