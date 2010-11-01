using System.Windows;
using Infrasturcture.FlipBook;
using Infrasturcture.TouchLibrary;

namespace Infrasturcture.Global.Controls.FlipBook
{
    public interface IBook
    {
        void SetSelectedPages();
        void CheckSheets(IBookPage bookPage);
        Size RenderSize { get; set; }
        IMTContainer Container { get; set; }
        bool IsHitTestVisible { get; set; }
        void AnimateToNextPage(bool fromTop, int duration);
        void AnimateToPreviousPage(bool fromTop, int duration);
        object InputHitTest(Point relative);
        void TransformToVisual(object visual);
    }
}
