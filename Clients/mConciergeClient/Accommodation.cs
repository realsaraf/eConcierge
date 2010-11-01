using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using CustomControls;
using CustomControls.HotelaccommodationControl;
using CustomControls.PictureControl;
using DataAccessLayer;
using Helpers.Extensions;
using Infrasturcture.Global.Helpers.Events;
using Infrasturcture.TouchLibrary;

namespace mConciergeClient
{
    public partial class MainWindow
    {
        readonly List<ClosePictureControl> _photos = new List<ClosePictureControl>();
        readonly List<AccommPhoto> _accomPhotos = new List<AccommPhoto>();
        readonly List<Line> _connectors = new List<Line>();
        private double _centerX;
        private double _centerY;
        const double SCALE = 0.5;

        private void ShowHotelAccommodation()
        {
            const double photoControlGridSize = 4.5;
            var canvasWidth = canvas.ActualWidth;
            var canvasHeight = canvas.ActualHeight;
            _centerX = (canvasWidth / 2);
            _centerY = (canvasHeight / 2);
            var photoRadius = GetPhotoRadius(canvasWidth, canvasHeight, photoControlGridSize);
            ResizeHotelExplorer(SCALE, _centerX, _centerY);
            FrameworkManager.ReRegisterControl(HotelExplorer.GetInstance(), false, new[] { TouchAction.Tap });

            var imagesData = AccommodationDAL.GetInstance().GetImages();
            AddImage(photoRadius, photoRadius, _centerY - (photoRadius / 2), imagesData[0], _centerX, _centerY);
            AddImage(photoRadius, canvasWidth - (photoRadius * 2), _centerY - (photoRadius / 2), imagesData[1], _centerX, _centerY);
            AddImage(photoRadius, _centerX - (photoRadius * 2), photoRadius, imagesData[2], _centerX, _centerY);
            AddImage(photoRadius, _centerX + photoRadius, photoRadius, imagesData[3], _centerX, _centerY);
            AddImage(photoRadius, _centerX - (photoRadius * 2), canvasHeight - (photoRadius * 2), imagesData[4], _centerX, _centerY);
            AddImage(photoRadius, _centerX + photoRadius, canvasHeight - (photoRadius * 2), imagesData[5], _centerX, _centerY);
        }

        private void ResizeHotelExplorer(double scale, double centerX, double centerY)
        {
            if (_hotelExplorer != null)
            {
                _hotelExplorer.Width = scale == 1 ? 716 : _hotelExplorer.ActualWidth * scale;
                _hotelExplorer.Height = scale == 1 ? 468 : _hotelExplorer.ActualHeight * scale;
                var left = centerX - _hotelExplorer.Width / 2;
                Canvas.SetLeft(_hotelExplorer, left);
                var top = centerY - _hotelExplorer.Height / 2;
                Canvas.SetTop(_hotelExplorer, top);
                _hotelExplorer.HightLight(scale!=1);
                _hotelExplorer.Container.Reset();
                _hotelExplorer.Container.StartX = left.ToInt();
                _hotelExplorer.Container.StartY = top.ToInt();
            }
        }

        private static double GetPhotoRadius(double canvasWidth, double canvasHeight, double photoControlGridSize)
        {
            var canvasWidthExcludingExplorer = (canvasWidth - (HotelExplorer.GetInstance().ActualWidth * SCALE));
            var canvasHeightExcludingExplorer = (canvasHeight - (HotelExplorer.GetInstance().ActualHeight * SCALE));
            var photoRadiusHorizontal = canvasWidthExcludingExplorer / photoControlGridSize;
            var photoRadiusVertical = canvasHeightExcludingExplorer / photoControlGridSize;
            return photoRadiusHorizontal < photoRadiusVertical ? photoRadiusHorizontal : photoRadiusVertical;
        }

        private void AddImage(double photoRadius, double left, double top, byte[] imageData, double centerX, double centerY)
        {
            var accommPhoto = new AccommPhoto { ImageData = imageData };
            accommPhoto.Width = accommPhoto.Height = photoRadius;
            Canvas.SetTop(accommPhoto, top);
            Canvas.SetLeft(accommPhoto, left);

            FrameworkManager.RegisterElement((IMTouchControl)accommPhoto, false, new[] { TouchAction.Tap, TouchAction.Move, TouchAction.SelectToFront, TouchAction.Slide });
            accommPhoto.Tapped += AccomodationPhotoTappedHandler;
            var line = AddLine(GetPhotoCenter(accommPhoto).X, GetPhotoCenter(accommPhoto).Y, centerX, centerY);
            accommPhoto.Tag = line;
            _connectors.Add(line);
            accommPhoto.Dragged += AccommPhotoDragged;
            MainCanvas.Children.Add(accommPhoto);
            Panel.SetZIndex(accommPhoto, Panel.GetZIndex(line) + 1);
            _accomPhotos.Add(accommPhoto);
        }

        void AccommPhotoDragged(object sender, PointEventArgs e)
        {
            var photo = sender as AccommPhoto;
            var line = photo.Tag as Line;
            var photoPoint = photo.TranslatePoint(new Point(0, 0), canvas);
            line.X1 = photoPoint.X + (photo.ActualWidth / 2.0);
            line.Y1 = photoPoint.Y + (photo.ActualHeight / 2.0);
        }

        private void AccomodationPhotoTappedHandler(object sender, RoutedEventArgs e)
        {
            var accommPhoto = sender as AccommPhoto;
            if (accommPhoto.IsDisplayed) return;
            var photo = AddPhoto(accommPhoto.ImageData, null);
            photo.Closed += PictureCloseClick;
            photo.Tag = accommPhoto;
            _photos.Add(photo);
        }


        private static Point GetPhotoCenter(AccommPhoto accommPhoto)
        {
            return new Point(Canvas.GetLeft(accommPhoto) + accommPhoto.Width / 2.0, Canvas.GetTop(accommPhoto) + accommPhoto.Height / 2.0);
        }


        private Line AddLine(double x1, double y1, double x2, double y2)
        {
            var line = new Line
                           {
                               StrokeThickness = 3,
                               Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC19914")),
                               X1 = x1,
                               Y1 = y1,
                               X2 = x2,
                               Y2 = y2
                           };
            canvas.Children.Add(line);
            Panel.SetZIndex(line, Panel.GetZIndex(HotelExplorer.GetInstance()) - 1);
            return line;
        }

        void PictureCloseClick(object sender, EventArgs e)
        {
            var pictureControl = sender as ClosePictureControl;
            _photos.Remove(pictureControl);
        }

        public void RemovePhotos()
        {
            foreach (var photo in _photos.ToArray())
            {
                photo.Close();
            }
        }

        private void RemoveAccomodationPhotos()
        {
            foreach (var accomPhoto in _accomPhotos)
            {
                canvas.Children.Remove((Line)accomPhoto.Tag);
                FrameworkManager.RemoveControl(accomPhoto);
            }
            
            _accomPhotos.Clear();
        }

        private void UnloadAccommodation()
        {
            foreach (var accomPhoto in _accomPhotos)
            {
                canvas.Children.Remove((Line)accomPhoto.Tag);
                FrameworkManager.RemoveControl(accomPhoto);
            }
            _accomPhotos.Clear();
            FrameworkManager.ReRegisterControl(HotelExplorer.GetInstance(), true, null);
            ResizeHotelExplorer(1, _centerX, _centerY);
        }

        private void UnloadAccommodationWithChildren()
        {
            foreach (var accomPhoto in _accomPhotos)
            {
                canvas.Children.Remove((Line)accomPhoto.Tag);
                FrameworkManager.RemoveControl(accomPhoto);
            }
            RemovePhotos();
            _accomPhotos.Clear();
            FrameworkManager.ReRegisterControl(HotelExplorer.GetInstance(), true, null);
            ResizeHotelExplorer(1, _centerX, _centerY);
        }

        private void MoveAccomodationConnectors(DataEventArgs dataEventArgs)
        {
            var values = dataEventArgs.Data as float[];
            var point = _hotelExplorer.TranslatePoint(new Point(0, 0), MainCanvas);
            if(values!=null)
            {
                foreach (var line in _connectors)
                {
                    line.X2 = point.X + _hotelExplorer.Width/2;
                    line.Y2 = point.Y + _hotelExplorer.Height/2;
                }
            }
        }
    }
}
