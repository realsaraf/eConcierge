using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Infrasturcture.TouchLibrary
{
    public interface IMTContainer
    {
        PointF Position { get; }
        event EventHandler<EventArgs> OnApplyingTransforms;
        Panel TopContainer { get; set; }
        int StartX { get; set; }
        int StartY { get; set; }
        ITouchDictionary ObjectTouches { get; set; }
        int Id { get; }
        TransformGroup Transforms { get; set; }
        Matrix OldTranform { get; set; }
        FrameworkElement WorkingObject { get; set; }
        bool CancelTransforms { get; set; }
        float StartScale { get; set; }
        void Reset();
        bool IsBook();
        bool IsHitTestVisible(PointF touchPoint);
        IMTContainer GetBelowItem(PointF touchPoint, int i);
        bool Locked { get; set; }
    }
}
