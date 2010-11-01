/*
TouchFramework connects touch tracking from a tracking engine to WPF controls 
allow scaling, rotation, movement and other multi-touch behaviours.

Copyright 2009 - Mindstorm Limited (reg. 05071596)

Author - Simon Lerpiniere

This file is part of TouchFramework.

TouchFramework is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

TouchFramework is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser Public License for more details.

You should have received a copy of the GNU Lesser Public License
along with TouchFramework.  If not, see <http://www.gnu.org/licenses/>.

If you have any questions regarding this library, or would like to purchase 
a commercial licence, please contact Mindstorm via www.mindstorm.com.
*/

using System.Drawing;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Controls;
using _3dTile;
using CustomControls;
using CustomControls.BasicVideoControl;
using CustomControls.CalendarControl;
using CustomControls.CircularButton;
using CustomControls.HotelaccommodationControl;
using CustomControls.HotelVideoControl;
using CustomControls.InheritedFrameworkControls;
using CustomControls.LandMark;
using CustomControls.MapControl;
using CustomControls.MapLocation;
using CustomControls.PictureControl;
using Infrasturcture._3DTile;
using Infrasturcture.FlipBook;
using Infrasturcture.Global.Controls.FlipBook;
using Infrasturcture.TouchLibrary;
using TouchFramework.ControlHandlers;
using TouchFramework.Helpers;
using TouchFramework.Events;
using WpfKb.Controls;
using WPFMitsuControls;

//using WpfKb.Controls;


namespace TouchControls.ControlHandlers
{
    /// <summary>
    /// Provides default behaviour which is applied to all multi-touch element.
    /// Derive from this class to create custom behaviors for different types of element.
    /// Note: Currently the static GetHandler method needs to be modified to support additional added handlers.
    /// </summary>
    public class ElementHandler : IElementHandler
    {
        public static ElementHandler GetHandler(FrameworkElement source, Panel cont, IMTContainer mTContainer)
        {
            ElementHandler handler = null;
            if (source is TextBox)
            {
                handler = new TextBoxHandler() as ElementHandler;
            }
            else if (source is LandMarkItem)
            {
                handler = new LandmarkItemHandler();
            }
            else if (source is TouchRadioButton)
            {
                handler = new TouchRadioButtonHandler();
            }
            else if (source is IPhoneScrollViewer)
            {
                handler = new IPhoneScrollViewerHandler();
            }
            else if (source is LocationRadio)
            {
                handler = new LocationRadioButtonHandler();
            }
            else if (source is CalendarDayItem)
            {
                handler = new DayItemHandler();
            }
            else if (source is TouchButton)
            {
                handler = new TouchButtonHandler();
            }
            else if (source is Button)
            {
                handler = new ButtonHandler() as ElementHandler;
            }
            else if (source is CheckBox)
            {
                handler = new CheckBoxHandler() as ElementHandler;
            }
            else if (source is TrackBar)
            {
                handler = new TrackBarHandler();
            }
            else if (source is Slider)
            {
                handler = new SliderHandler() as ElementHandler;
            }
            else if (source is ListBox)
            {
                handler = new ListBoxHandler() as ElementHandler;
            }
            else if (source is ComboBox)
            {
                handler = new ComboBoxHandler() as ElementHandler;
            }
            else if (source is RssList)
            {
                handler = new RssListHandler() as ElementHandler;
            }
            else if (source is VideoControl)
            {
                handler = new VideoControlHandler() as ElementHandler;
            }
            else if (source is FlipTile3D)
            {
                handler = new FlipTile3DHandler() as ElementHandler;
            }
            else if (source is AppCategories)
            {
                handler = new AppCategoriesHandler() as ElementHandler;
            }
            else if (source is AppData)
            {
                handler = new AppDataHandler() as ElementHandler;
            }
            else if(source is AccommPhoto)
            {
                handler = new AccomPhotoHandler();
            }
           else if (source is Photo)
            {
                handler = new PhotoHandler();
            }
            else if (source is PictureControl)
            {
                handler = new PictureControlHandler();
            }
            else if (source is ClosePictureControl)
            {
                handler = new ClosePictureControlHandler();
            }
            else if (source is Book)
            {
                handler = new BookHandler() as ElementHandler;
            }
            else if (source is ChromBrowser)
            {
                handler = new WebBrowserHandler();
            }
            else if (source is Cjc.ChromiumBrowser.WebBrowser)
            {
                handler = new ChromiumBrowserHandler();
            }
            else if (source is FloatingTouchScreenKeyboard)
            {
                handler = new KeyboardHandler();
            }
            else if (source is HotelVideoControl)
            {
                handler = new HotelVideoControlHandler();
            }
            else if (source is HotelExplorer)
            {
                handler = new HotelExplorerHandler();
            }
            else if (source is CircularToggleButton)
            {
                handler = new CircularToggleButtonHandler();
            }
            else if (source is ToggleButton)
            {
                handler = new ToggleButtonHandler();
            }
            else if (source is CircularCloseButtonControl)
            {
                handler = new CircularCloseButtonControlHandler();
            }
            else if (source is MapControl)
            {
                handler = new MapControlHandler();
            }
            else if (source is MapBrowser)
            {
                handler = new MapBrowserHandler();
            }
            else if (source is ScrollViewer)
            {
                handler = new ScrollViewerHandler();
            }
            else
            {
                handler = new ButtonHandler();
            }
            handler.Source = source;
            handler.Container = cont;
            _containter = mTContainer;
            return handler;
        }



        public bool HasHitTest(PointF pointF)
        {
            var b = Source as IBook;
            if (b == null)
                return true;
            b.SetSelectedPages();
            var cont = b.Container;
            var cc = cont.TopContainer.InputHitTest(new System.Windows.Point(pointF.X,pointF.Y));
            if (!(cc is Canvas))
            {
                var bp = (cc as Visual).FindParent<IBookPage>() as IBookPage;
                if (bp != null)
                {
                    b.CheckSheets(bp);
                    return true;
                }
            }
            return false;
        }

        IBookPage findSelectedPage(IBook c, System.Windows.Point relative)
        {
            var findFrom = c.InputHitTest(relative);
            return (findFrom as Visual).FindParent<IBookPage>() as IBookPage;
        }

        public FrameworkElement Source = null;
        public Panel Container = null;
        private static IMTContainer _containter;

        /// <summary>
        /// Raises the tap event as a routed event on the element.
        /// </summary>
        /// <param name="p">Relative point in element space</param>
        public virtual void Tap(PointF global, PointF relative)
        {
            this.Source.RaiseEvent(new RoutedEventArgs(MTEvents.TapEvent, Source));
        }

        /// <summary>
        /// Checks if the element has a scroll viewer.  It it does, uses the scroll viewer
        /// to scroll the content by the number of pixels moved.
        /// X and Y are inverted so that the scroll works like the iPhone (i.e. the touched
        /// item sticks to your finger).  Works better without scrollbars shown.
        /// </summary>
        /// <param name="x">Change in the horizontal object relative movement center in pixels</param>
        /// <param name="y">Change in the vertical object relative movement center in pixels</param>
        public virtual void Scroll(float x, float y)
        {
            // This sets it so that we scroll by pixels rather than by lines
            if (ScrollViewer.GetCanContentScroll(Source)) ScrollViewer.SetCanContentScroll(Source, false);

            // Find the scrollviewer and scroll it if there is one
            ScrollViewer scroll = Source.FindChild<ScrollViewer>() as ScrollViewer;
            if (scroll != null)
            {
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset + (x * -1));
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset + (y * -1));
            }

            Source.RaiseEvent(new RoutedEventArgs(MTEvents.ScrollEvent, Source));
        }

        /// <summary>
        /// Raises the Drag event as a routed event on the element.
        /// Override this to implement custom drag functionality.
        /// </summary>
        /// <param name="x">Horizontal coordinate in screen space of the center of movement.</param>
        /// <param name="y">Vertical coordinate in screen space of the center of movement.</param>
        public virtual void Drag(float x, float y)
        {
            Source.RaiseEvent(new RoutedEventArgs(MTEvents.DragEvent, Source));
        }

        /// <summary>
        /// Raises the Drag event as a routed event on the element.
        /// Override this to implement custom CenterNotify behaviour.
        /// </summary>
        /// <param name="global">Center of movement relative to the whole canvas</param>
        /// <param name="relative">Center of movement relative to the object</param>
        public virtual void Drag(PointF global, PointF relative)
        {
            this.Drag(global.X, global.Y);
        }

        /// <summary>
        /// Raises the Slide event as a routed event on the element.
        /// Overeride this to implement custom slide behaviour.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public virtual void Slide(float x, float y)
        {
            Source.RaiseEvent(new RoutedEventArgs(MTEvents.SlideEvent, Source));
        }

        /// <summary>
        /// Raises the TouchDown event as a routed event on the element.
        /// Override this to implement custom TouchDown behaviour.
        /// </summary>
        /// <param name="p">Point in element relative space of the touch center.</param>
        //public virtual void TouchDown(PointF p)
        public virtual void TouchDown(PointF global, PointF relative)
        {
            Source.RaiseEvent(new RoutedEventArgs(MTEvents.TouchDownEvent, Source));
        }

        /// <summary>
        /// Raises the TouchUp event as a routed event on the element.
        /// Override this to implement custom TouchUp behaviour.
        /// </summary>
        /// <param name="p">Last valid point in element relative space of the touch center.</param>
        public virtual void TouchUp(PointF global, PointF relative)
        {
            Source.RaiseEvent(new RoutedEventArgs(MTEvents.TouchUpEvent, Source));
        }

        public bool IsBook()
        {
            return Source is IBook;
        }

        public IMTContainer BelowItemContainer(PointF touchPoint, int maxZIndex)
        {
            var book = Source as IBook;
            book.IsHitTestVisible = false;
            var cont = book.Container;
            var cc = cont.TopContainer.InputHitTest(new System.Windows.Point(touchPoint.X, touchPoint.Y));
            book.IsHitTestVisible = true;
            if (!(cc is Canvas))
            {
                var bp = (cc as Visual).FindParent<IMTouchControl>() as FrameworkElement;
                if (bp != null)
                {
                    Canvas.SetZIndex(bp,maxZIndex);
                    return ((IMTouchControl)bp).Container;
                }
            }
            return null;
        }
    }
}
