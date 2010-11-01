using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using CustomControls;
using Infrasturcture.TouchLibrary;
using TouchFramework;
using TouchFramework.Containers;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace mConcierge
{
    public class FrameworkManager : IFrameworkManager
    {
        private Dictionary<int, UIElement> _points = new Dictionary<int, UIElement>();
        private const TrackingHelper.TrackingType CurrentTrackingType = TrackingHelper.TrackingType.TUIO;
        private static readonly Random RandomGen = new Random();
        public Canvas Canvas { get; set; }
        public FrameworkControl Framework { get; set; }
        public FrameworkManager(Canvas canvas)
        {
            Canvas = canvas;
        }


        public void Initialize()
        {
            Framework = TrackingHelper.GetTracking(Canvas, CurrentTrackingType);
            Framework.OnProcessUpdates += DisplayPoints;
            Framework.Start();
            Framework.ForceRefresh();
        }

        public void PosAll()
        {
            foreach (MTContainer cont in Framework.Assigner.Elements.Values)
            {
                int difX = Canvas.ActualWidth > 200 ? 200 : 0;
                int difY = Canvas.ActualWidth > 200 ? 200 : 0;

                int x = RandomGen.Next(0, (int)Canvas.ActualWidth - difX);
                int y = RandomGen.Next(0, (int)Canvas.ActualHeight - difY);

                Canvas.SetTop(cont.WorkingObject, y);
                Canvas.SetLeft(cont.WorkingObject, x);
                cont.Reset();
                cont.StartX = x;
                cont.StartY = y;
            }
        }
        
        public void ReRegisterControl(IMTouchControl control, bool supportAllTouchAction, TouchAction[] touchActions)
        {
            UnRegisterControl(control);
            RegisterElement(control, supportAllTouchAction, touchActions);
        }

        public void UnRegisterControl(IMTouchControl control)
        {
            Framework.UnregisterElement(control.Container.Id);
            Framework.ForceRefresh();
        }

        public void UnRegisterControl(int containerId)
        {
            Framework.UnregisterElement(containerId);
            Framework.ForceRefresh();
        }

        public void RegisterElement(IMTouchControl control, bool supportAllTouches, TouchAction[] touchActions)
        {
            control.Container = RegisterElement((FrameworkElement)control, supportAllTouches, touchActions);
        }


        public IMTContainer RegisterElement(FrameworkElement control, bool supportAllTouches, TouchAction[] touchActions)
        {
            var prop = new ElementProperties();
            if (!supportAllTouches)
            {
                foreach (var touchAction in touchActions)
                {
                    prop.ElementSupport.AddSupport(touchAction);
                }
            }
            else
                prop.ElementSupport.AddSupportForAll();

            var cont = new MTSmoothContainerRev(control, Canvas, prop);
            Framework.RegisterElement(cont);
            return cont;
        }

        public void RegisterMediatorElement(IMTouchControl control, bool supportAllTouches, TouchAction[] touchActions)
        {
            var prop = new ElementProperties();
            if (!supportAllTouches)
            {
                foreach (var touchAction in touchActions)
                {
                    prop.ElementSupport.AddSupport(touchAction);
                }
            }
            else
                prop.ElementSupport.AddSupportForAll();

            var cont = new MTSmoothMediatorContainer((FrameworkElement)control, Canvas, prop);
            control.Container = cont;
            Framework.RegisterElement(cont);
            control.IsDisplayed = true;
        }

        #region DisplayPoints

        public void DisplayPoints()
        {
            foreach (int i in _points.Keys)
            {
                if (!Framework.AllTouches.Keys.Contains(i)) Canvas.Children.Remove(_points[i]);
            }
            foreach (Touch te in Framework.AllTouches.Values)
            {
                DisplayPoint(te.TouchId, te.TouchPoint);
            }
        }

        /// <summary>
        /// Goes through and removes all points from the screen.  I.e. all elipses created to represent touch points.
        /// </summary>
        public void RemovePoints()
        {
            foreach (UIElement e in _points.Values)
            {
                Canvas.Children.Remove(e);
            }
            _points = new Dictionary<int, UIElement>();
        }

        /// <summary>
        /// Displays a point on the screen in the specified location, with the specified colour.
        /// </summary>
        /// <param name="id">Id of the point.</param>
        /// <param name="p">Position of the point in screen coordinates.</param>
        public void DisplayPoint(int id, PointF p)
        {
            DisplayPoint(id, p, Colors.White);
        }

        /// <summary>
        /// Displays a point on the screen in the specified location, with the specified colour.
        /// </summary>
        /// <param name="id">Id of the point.</param>
        /// <param name="p">Position of the point in screen coordinates.</param>
        /// <param name="brushColor">The brush to use for the elipse.</param>
        public void DisplayPoint(int id, PointF p, Color brushColor)
        {
            Ellipse e = null;
            if (_points.ContainsKey(id))
            {
                e = _points[id] as Ellipse;
                e.RenderTransform = new TranslateTransform(p.X - 13, p.Y - 13);
            }

            if (e == null)
            {
                e = new Ellipse();

                var radialGradient = new RadialGradientBrush
                {
                    GradientOrigin = new Point(0.5, 0.5),
                    Center = new Point(0.5, 0.5),
                    RadiusX = 0.5,
                    RadiusY = 0.5
                };

                var shadow = Colors.Black;
                shadow.A = 30;
                radialGradient.GradientStops.Add(new GradientStop(shadow, 0.9));
                brushColor.A = 60;
                radialGradient.GradientStops.Add(new GradientStop(brushColor, 0.8));
                brushColor.A = 150;
                radialGradient.GradientStops.Add(new GradientStop(brushColor, 0.1));

                radialGradient.Freeze();

                e.Height = 26.0;
                e.Width = 26.0;
                e.Fill = radialGradient;

                int eZ = Framework.MaxZIndex + 100;
                e.IsHitTestVisible = false;
                e.RenderTransform = new TranslateTransform(p.X - 13, p.Y - 13);
                Canvas.Children.Add(e);
                Panel.SetZIndex(e, eZ < 600000 ? 600000 : eZ);
                _points.Add(id, e);
            }
        }
        #endregion

        public void Stop()
        {
            Framework.Stop();
        }
    }
}