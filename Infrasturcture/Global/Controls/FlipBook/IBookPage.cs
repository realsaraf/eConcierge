using System.Windows;
using Infrasturcture.Global.Controls.FlipBook;

namespace Infrasturcture.FlipBook
{
    public interface IBookPage
    {
        CornerOrigin? GetCorner(IBook book, Point point);
        void LetGoPage(IBook itemsControl, Point point);
        void MoveGrabPoint(IBookPage selectedPage, Point point);
    }
}
