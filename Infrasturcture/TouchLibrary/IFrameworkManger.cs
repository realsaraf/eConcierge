using System.Windows;
using System.Windows.Controls;

namespace Infrasturcture.TouchLibrary
{
    public interface IFrameworkManger
    {
        IMTContainer RegisterElement(IMTouchControl control, bool supportAllTouches, TouchAction[] touchActions);
        IMTContainer RegisterElement(FrameworkElement control, bool supportAllTouches, TouchAction[] touchActions);
        void UnRegisterElement(int containerId);
        void UnRegisterElement(IMTouchControl control);
        void RemoveControl(IMTouchControl control);
        Canvas Canvas { get; set; }
        void HideKeyBoard();
        void ShowKeyBoard();
        IMTContainer AddControl(IMTouchControl control, TouchAction[] touchActions, double left, double top);
        IMTContainer AddControlWithAllGestures(IMTouchControl control, double left, double top);
        void AddControlWithAllNoGestures(IMTouchControl control, double left, double top);
    }
}