using System.Drawing;

namespace Infrasturcture.TouchLibrary
{
    public interface IElementHandler
    {
        /// <summary>
        /// Raises the tap event as a routed event on the element.
        /// </summary>
        /// <param name="p">Relative point in element space</param>
        void Tap(PointF global, PointF relative);

        /// <summary>
        /// Checks if the element has a scroll viewer.  It it does, uses the scroll viewer
        /// to scroll the content by the number of pixels moved.
        /// X and Y are inverted so that the scroll works like the iPhone (i.e. the touched
        /// item sticks to your finger).  Works better without scrollbars shown.
        /// </summary>
        /// <param name="x">Change in the horizontal object relative movement center in pixels</param>
        /// <param name="y">Change in the vertical object relative movement center in pixels</param>
        void Scroll(float x, float y);

        /// <summary>
        /// Raises the Drag event as a routed event on the element.
        /// Override this to implement custom drag functionality.
        /// </summary>
        /// <param name="x">Horizontal coordinate in screen space of the center of movement.</param>
        /// <param name="y">Vertical coordinate in screen space of the center of movement.</param>
        void Drag(float x, float y);

        /// <summary>
        /// Raises the Drag event as a routed event on the element.
        /// Override this to implement custom CenterNotify behaviour.
        /// </summary>
        /// <param name="global">Center of movement relative to the whole canvas</param>
        /// <param name="relative">Center of movement relative to the object</param>
        void Drag(PointF global, PointF relative);

        /// <summary>
        /// Raises the Slide event as a routed event on the element.
        /// Overeride this to implement custom slide behaviour.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void Slide(float x, float y);

        /// <summary>
        /// Raises the TouchDown event as a routed event on the element.
        /// Override this to implement custom TouchDown behaviour.
        /// </summary>
        /// <param name="p">Point in element relative space of the touch center.</param>
        //public virtual void TouchDown(PointF p)
        void TouchDown(PointF global, PointF relative);

        /// <summary>
        /// Raises the TouchUp event as a routed event on the element.
        /// Override this to implement custom TouchUp behaviour.
        /// </summary>
        /// <param name="p">Last valid point in element relative space of the touch center.</param>
        void TouchUp(PointF global, PointF relative);
    }
}