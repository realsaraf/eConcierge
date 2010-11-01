using System.Windows;

namespace Infrasturcture.TouchLibrary
{
    public interface IFrameworkManager
    {
        IMTContainer RegisterElement(FrameworkElement control, bool supportAllTouches, TouchAction[] touchActions);
        void UnRegisterControl(IMTouchControl control);
        void UnRegisterControl(int containerId);
    }
}