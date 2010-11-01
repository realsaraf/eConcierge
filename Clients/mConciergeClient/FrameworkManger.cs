using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Helpers.Extensions;
using Infrasturcture.TouchLibrary;
using mConcierge;
using TouchFramework;
using TouchFramework.Containers;
using WpfKb.Controls;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace mConciergeClient
{
    public class FrameworkManger : IFrameworkManger
    {
        private readonly DockPanel _toolDock;
        private Dictionary<int, UIElement> _points = new Dictionary<int, UIElement>();
        private const TrackingHelper.TrackingType CURRENT_TRACKING_TYPE = TrackingHelper.TrackingType.TUIO;
        private static readonly Random RandomGen = new Random();
        private FloatingTouchScreenKeyboard _keyboard;
        public Canvas Canvas { get; set; }
        private int _maxZIndex;

        public FrameworkControl Framework { get; set; }
        public FrameworkManger(Canvas canvas, DockPanel toolDock)
        {
            _toolDock = toolDock;
            Canvas = canvas;
        }


        public void Initialize()
        {
            Framework = TrackingHelper.GetTracking(Canvas, CURRENT_TRACKING_TYPE);
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
            UnRegisterElement(control);
            RegisterElement(control, supportAllTouchAction, touchActions);
        }

        public IMTContainer RegisterElement(IMTouchControl control, bool supportAllTouches, TouchAction[] touchActions)
        {
            control.Container = RegisterElement((FrameworkElement)control, supportAllTouches, touchActions);
            return control.Container;
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
            {
                prop.ElementSupport.AddSupportForAll();
            }

            var cont = new MTSmoothContainerRev(control, Canvas, prop);
            Framework.RegisterElement(cont);
            return cont;
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

        public void UnRegisterElement(int containerId)
        {
            Framework.UnregisterElement(containerId);
            Framework.ForceRefresh();
        }

        public void UnRegisterElement(IMTouchControl control)
        {
            if (control.Container == null) return;
            Framework.UnregisterElement(control.Container.Id);
            Framework.ForceRefresh();
        }

        public void RemoveControl(IMTouchControl control)
        {
            UnRegisterElement(control);
            Canvas.Children.Remove((FrameworkElement)control);
        }

        public void HideKeyBoard()
        {
            if (_keyboard == null) return;

            UnRegisterElement(_keyboard);
            Canvas.Children.Remove(_keyboard);
            _keyboard = null;
        }

        public void ShowKeyBoard()
        {
            _keyboard = new FloatingTouchScreenKeyboard
            {
                Name = "Keyboard",
                Width = 900,
                Height = 400,
                AreAnimationsEnabled = true
            };
            RegisterElement((IMTouchControl)_keyboard, true, null);
            Canvas.SetZIndex(_keyboard, 600000);
            Canvas.Children.Add(_keyboard);
        }

        private void AddToCanvas(UIElement control)
        {
            Canvas.Children.Add(control);
        }

        public IMTContainer AddControl(IMTouchControl control, TouchAction[] touchActions, double left, double top)
        {
            AddToCanvas((UIElement) control);
            var cont = RegisterElement(control, false, touchActions);
            Canvas.SetLeft((UIElement)control, left);
            Canvas.SetTop((UIElement)control, top);
            control.Container.Reset();
            control.Container.StartX = left.ToInt();
            control.Container.StartY = top.ToInt();
            return cont;
        }

        public IMTContainer AddControlWithAllGestures(IMTouchControl control, double left, double top)
        {
            AddToCanvas((UIElement)control);
            var cont = RegisterElement(control, true, null);
            Canvas.SetLeft((UIElement)control, left);
            Canvas.SetTop((UIElement)control, top);
            control.Container.Reset();
            control.Container.StartX = left.ToInt();
            control.Container.StartY = top.ToInt(); 
            return cont;
        }

        public void AddControlWithAllNoGestures(IMTouchControl control, double left, double top)
        {
            AddToCanvas((UIElement)control);
            Canvas.SetLeft((UIElement)control, left);
            Canvas.SetTop((UIElement)control, top);
        }
    }
}