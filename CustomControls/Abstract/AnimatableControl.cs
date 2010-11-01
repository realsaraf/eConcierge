using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CustomControls.Abstract
{
    public class AnimatableControl : UserControl, IAnimatableControl
    {
        private Canvas _canvas;
        public event EventHandler AnimationCompleted;

        public AnimatableControl()
        {
             
        }
        public void AnimateTo(bool restore, Canvas canvas, double top, double left, double scale, double rotate, int animationDuration = 1000)
        {
            _canvas = canvas;
            HightLight(!restore);
            var xpos = restore ? left : Canvas.GetLeft(this);
            var ypos = restore ? top : Canvas.GetTop(this);
            var xTo = restore ? left - (this.ActualWidth / 2) : left - (this.ActualWidth * scale / 2);
            var yTo = restore ? top - (this.ActualHeight / 2) : top - (this.ActualHeight * scale / 2);
            var zoomFrom = restore ? scale : 1;
            var zoomTo = restore ? 1 : scale;
            var rotateFrom = restore ? rotate : 0;
            var rotateTo = restore ? 0 : rotate;

            var yAnimation = new DoubleAnimation
                                 {
                                     From = ypos,
                                     To = yTo,
                                     Duration = new Duration(TimeSpan.FromMilliseconds(animationDuration))
                                 };
            var xAnimation = new DoubleAnimation
                                 {
                                     From = xpos,
                                     To = xTo,
                                     Duration = new Duration(TimeSpan.FromMilliseconds(animationDuration))
                                 };

            this.BeginAnimation(Canvas.TopProperty, yAnimation);
            this.BeginAnimation(Canvas.LeftProperty, xAnimation);


            var zoom = new DoubleAnimation
                           {
                               From = zoomFrom,
                               To = zoomTo,
                               BeginTime = TimeSpan.FromMilliseconds(0),
                               Duration = new Duration(TimeSpan.FromMilliseconds(animationDuration))
                           };

            var rotateAnimation = new DoubleAnimation
                                      {
                                          From = rotateFrom,
                                          To = rotateTo,
                                          BeginTime = TimeSpan.FromMilliseconds(0),
                                          Duration = new Duration(TimeSpan.FromMilliseconds(animationDuration))
                                      };

            zoom.Completed += AnimateToCompleted;

            var st = new ScaleTransform();
            var rt = new RotateTransform(rotateTo, 0, 0);

            var group = new TransformGroup();
            group.Children.Add(st);
            group.Children.Add(rt);

            this.RenderTransform = group;
            st.BeginAnimation(ScaleTransform.ScaleXProperty, zoom);
            st.BeginAnimation(ScaleTransform.ScaleYProperty, zoom);
            rt.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
        }

        public virtual void HightLight(bool doHighlight) { }

        public virtual void AnimateToCompleted(object sender, EventArgs e)
        {
            if (AnimationCompleted != null)
                AnimationCompleted(this, new EventArgs());
        }
        
        public void AnimatedDownToCenter(Canvas canvas)
        {
            AnimateTo(false, canvas, canvas.ActualHeight / 2, canvas.ActualWidth / 2, 0, 0);
        }

        public virtual void Dispose()
        {
            
        }
    }
}