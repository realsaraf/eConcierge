using CustomControls.HotelVideoControl;
using TouchFramework.ControlHandlers;

namespace TouchControls.ControlHandlers
{
    public class HotelVideoControlHandler:ElementHandler
    {
        public override void Tap(System.Drawing.PointF global, System.Drawing.PointF relative)
        {
            var hotelVideoControl = Source as HotelVideoControl;
            hotelVideoControl.PlayVideo();
            base.Tap(global, relative);
        }
       
    }
}
