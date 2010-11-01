using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using _3dTile;
using Artefact.Animation;
using CustomControls;
using CustomControls.HotelVideoControl;
using CustomControls.PictureControl;
using DataAccessLayer;
using Infrasturcture.TouchLibrary;

namespace mConciergeClient
{
    public partial class MainWindow
    {
        private bool _skipHeClosedEvent;
        private HotelVideoControl _hotelVideoControl;
        private HotelExplorer _hotelExplorer;
        private ClosePictureControl _hotelMap;

        private void LoadHotelExplorer()
        {
            LoadSlider();
            FrameworkManager.RegisterElement(mConciergeTool, false, new[] { TouchAction.Tap });
            mConciergeTool.Checked += HotelExplorerToolSelected;
        }

        void HotelExplorerToolSelected(object sender, RoutedEventArgs e)
        {
            _skipHeClosedEvent = false;
            ShowHotelExplorer();
        }

        private void HotelExplorerUnchecked(object sender, RoutedEventArgs e)
        {
            if (!_skipHeClosedEvent)
            {
                RemoveHotelExplorer();
                _skipHeClosedEvent = false;
            }
        }

        private void RemoveHotelExplorer()
        {
            if (_hotelExplorer != null)
            {
                _hotelExplorer.Drag -= HotelExplorerDrag;
                _hotelExplorer.ClearChoices();
                _hotelExplorer.OptionClicked -= HotelExplorerOptionClicked;
                _hotelExplorer.OptionUnUnChecked -= HotelExplorerOptionUnChecked;
                FrameworkManager.UnRegisterElement(_hotelExplorer.TourVideoOption);
                FrameworkManager.UnRegisterElement(_hotelExplorer.AccomodationsOption);
                FrameworkManager.UnRegisterElement(_hotelExplorer.HotelMapOption);
                FrameworkManager.UnRegisterElement(_hotelExplorer.PhotoGalleryOption);
                FrameworkManager.UnRegisterElement(_hotelExplorer.CloseButton);
                FrameworkManager.RemoveControl(_hotelExplorer);
                _hotelExplorer.Closed -= HotelExplorerClosed;
                _skipHeClosedEvent = true;
                mConciergeTool.IsChecked = false;
            }
        }

        private void ShowHotelExplorer()
        {
            _hotelExplorer = new HotelExplorer();
            _hotelExplorer.Drag += HotelExplorerDrag;
            var top = canvas.ActualHeight / 2 - (_hotelExplorer.Height / 2);
            var left = canvas.ActualWidth / 2 - (_hotelExplorer.Width / 2);
            _hotelExplorer.OptionClicked += HotelExplorerOptionClicked;
            _hotelExplorer.OptionUnUnChecked += HotelExplorerOptionUnChecked;
            FrameworkManager.RegisterElement(_hotelExplorer.TourVideoOption, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement(_hotelExplorer.AccomodationsOption, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement(_hotelExplorer.HotelMapOption, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement(_hotelExplorer.PhotoGalleryOption, false, new[] { TouchAction.Tap });
            FrameworkManager.RegisterElement(_hotelExplorer.CloseButton, false, new[] { TouchAction.Tap });
            FrameworkManager.AddControlWithAllGestures(_hotelExplorer, left, top);
            _hotelExplorer.Closed += HotelExplorerClosed;
        }

        void HotelExplorerClosed(object sender, EventArgs e)
        {
            _hotelExplorer.Drag -= HotelExplorerDrag;
            _hotelExplorer.Closed -= HotelExplorerClosed;
            _hotelExplorer.OptionClicked -= HotelExplorerOptionClicked;
            _hotelExplorer.OptionUnUnChecked -= HotelExplorerOptionUnChecked;
            FrameworkManager.UnRegisterElement(_hotelExplorer.TourVideoOption);
            FrameworkManager.UnRegisterElement(_hotelExplorer.AccomodationsOption);
            FrameworkManager.UnRegisterElement(_hotelExplorer.HotelMapOption);
            FrameworkManager.UnRegisterElement(_hotelExplorer.PhotoGalleryOption);
            FrameworkManager.UnRegisterElement(_hotelExplorer.CloseButton);
            RemoveAccomodationPhotos(); 
            FrameworkManager.RemoveControl(_hotelExplorer);
            _skipHeClosedEvent = true;
            mConciergeTool.IsChecked = false;
        }

        void HotelExplorerDrag(object sender, Infrasturcture.Global.Helpers.Events.DataEventArgs e)
        {
            MoveAccomodationConnectors(e);
        }

        void HotelExplorerOptionClicked(object sender, EventArgs e)
        {
            var control = sender as RadioButton;
            if (control != null)
                switch (control.Tag.ToString())
                {
                    case "TourVideo":
                        RemoveHotelExplorer();
                        ShowHotelIntroVideo();
                        break;
                    case "Accomodations":
                        ShowHotelAccommodation();
                        break;
                    case "HotelMap":
                        RemoveHotelExplorer();
                        ShowHotelMap();
                        break;
                    case "PhotoGallery":
                        RemoveHotelExplorer();
                        ShowPhotoViewer(true);
                        break;
                }
        }

        private void HotelExplorerOptionUnChecked(object sender, EventArgs e)
        {
            var control = sender as RadioButton;
            if (control != null)
                switch (control.Tag.ToString())
                {
                    case "TourVideo":
                        if (_hotelVideoControl != null)
                            _hotelVideoControl.Close();
                        break;
                    case "Accomodations":
                        UnloadAccommodation();
                        break;
                    case "HotelMap":
                        if(_hotelMap!=null)
                            _hotelMap.Close();
                        break;
                    case "PhotoGallery":
                        ShowPhotoViewer(false);
                        break;
                }
        }



        void LoadSlider()
        {
            Canvas.SetLeft(SliderCanvas, -((Width / 2) + 50));
            var myPicturesChoser = new FlipTile3D(PhotoGalleryDAL.GetInstance().GetImages()) {mainWindow = this, Width = Width/2, Height = Height};
            SliderCanvas.Children.Add(myPicturesChoser);
            myPicturesChoser.Tag = FrameworkManager.RegisterElement(myPicturesChoser, false, new[] { TouchAction.Tap, TouchAction.Slide });
            SliderCanvas.Visibility = Visibility.Visible;
        }

        void ShowPhotoViewer(bool open)
        {
            if (open)
            {
                ArtefactAnimator.AddEase(SliderCanvas, AnimationTypes.X, 0, 1, AnimationTransitions.CubicEaseOut, 0);
            }
            else
            {
                ArtefactAnimator.AddEase(SliderCanvas, AnimationTypes.X, -((Width / 2) + 50), 1, AnimationTransitions.CubicEaseOut, 0);
            }
        }

        private void ShowHotelMap()
        {
            _hotelMap = AddPhoto(@"Images\hotelFloorMap.png", new[] { TouchAction.Resize, TouchAction.Move, TouchAction.Slide, TouchAction.Flick, TouchAction.SelectToFront });
            _hotelMap.Closed += HotelMapClosed;
        }

        private void HotelMapClosed(object sender, EventArgs e)
        {
            _hotelMap = null;
            HotelExplorer.GetInstance().ClearChoices();
        }

        private void ShowHotelIntroVideo()
        {
            _hotelVideoControl = new HotelVideoControl();
            _hotelVideoControl.SetVideo(ConfigurationManager.AppSettings["IntroVideoPath"]);
            _hotelVideoControl.Load(FrameworkManager, 200, (Height / 2) - 400);
            _hotelVideoControl.Closed += HotelVideoControlCloseClick;
        }

        private void HotelVideoControlCloseClick(object sender, EventArgs e)
        {
            _hotelVideoControl = null;
            HotelExplorer.GetInstance().ClearChoices();
        }
    }
}