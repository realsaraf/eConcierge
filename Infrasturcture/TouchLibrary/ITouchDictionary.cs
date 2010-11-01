using System.Drawing;

namespace Infrasturcture.TouchLibrary
{
    public interface ITouchDictionary
    {
        void Add(int touchId, ITouch touch);
        float MoveX { get; set; }
        float MoveY { get; set; }
        bool JustTouched { get;}
        bool Lifted { get; }
        bool Tapped { get;}
        PointF ActionCenter { get;}
        PointF MoveCenter { get; set; }
        bool Changed { get; set; }
        bool TwoOrMoreTouch { get;}
        bool OneOrMoreLifted { get;}
        int CurrTouchCount { get; }
        int PrevTouchCount { get; }
        float GetAngleChanged();
        float GetDistanceChangeRatio();
        void CalculateChanges();
        void Remove(int touchId);
    }
}
