/*
    Copyright © 2009 Jesse Graupmann
    All rights reserved.

    Redistribution and use in source and binary forms, with or without 
    modification, are permitted provided that the following conditions 
    are met:

        * Redistributions of source code must retain the above copyright
          notice, this list of conditions and the following disclaimer.
        * Redistributions in binary form must reproduce the above copyright 
          notice, this list of conditions and the following disclaimer 
          in the documentation and/or other materials provided with the 
          distribution.
        * Neither the name of the author nor the names of contributors 
          may be used to endorse or promote products derived from this 
          software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
    "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT 
    LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS 
    FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE 
    COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
    INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
    BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
    LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
    CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
    LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
    ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
    POSSIBILITY OF SUCH DAMAGE.
*/



using System;
using System.Linq;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;

namespace Artefact.Animation
{
    public static class ArtefactAnimatorExtensions
    {
        // Extension Methods
        public static EaseObject AlphaTo(this UIElement obj, double alpha, double time, PercentHandler transition, double delay)
        {
            ArtefactAnimator.StopEase(obj, new string[] { AnimationTypes.Alpha, AnimationTypes.AutoAlpha, AnimationTypes.AutoAlphaCollapsed });
            return ArtefactAnimator.AddEase(obj, AnimationTypes.Alpha, alpha, time, transition, delay);
        }

        //  instant
        public static void AlphaTo(this UIElement obj, double alpha)
        {
            ArtefactAnimator.StopEase(obj, new string[] { AnimationTypes.Alpha, AnimationTypes.AutoAlpha, AnimationTypes.AutoAlphaCollapsed });
            obj.Opacity = alpha;
        }

        public static EaseObject AutoAlphaTo(this UIElement obj, double alpha, double time, PercentHandler transition, double delay)
        {
            ArtefactAnimator.StopEase(obj, new string[] { AnimationTypes.Alpha, AnimationTypes.AutoAlpha, AnimationTypes.AutoAlphaCollapsed });
            return ArtefactAnimator.AddEase(obj, AnimationTypes.AutoAlpha, alpha, time, transition, delay);
        }
        //  instant
        public static void AutoAlphaTo(this UIElement obj, double alpha)
        {
            ArtefactAnimator.StopEase(obj, new string[] { AnimationTypes.Alpha, AnimationTypes.AutoAlpha, AnimationTypes.AutoAlphaCollapsed });
            obj.Opacity = alpha;
            #if !SILVERLIGHT
                obj.Visibility = alpha > 0 ? Visibility.Visible : Visibility.Hidden;
            #endif
            #if SILVERLIGHT
                obj.Visibility = alpha > 0 ? Visibility.Visible : Visibility.Collapsed;
            #endif
        }

        public static EaseObject AutoAlphaCollapsedTo(this UIElement obj, double alpha, double time, PercentHandler transition, double delay)
        {
            ArtefactAnimator.StopEase(obj, new string[] { AnimationTypes.Alpha, AnimationTypes.AutoAlpha, AnimationTypes.AutoAlphaCollapsed });
            return ArtefactAnimator.AddEase(obj, AnimationTypes.AutoAlphaCollapsed, alpha, time, transition, delay);
        }
        //  instant
        public static void AutoAlphaCollapsedTo(this UIElement obj, double alpha)
        {
            ArtefactAnimator.StopEase(obj, new string[] { AnimationTypes.Alpha, AnimationTypes.AutoAlpha, AnimationTypes.AutoAlphaCollapsed });
            obj.Opacity = alpha;
            obj.Visibility = alpha > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public static EaseObject XTo(this UIElement obj, double X, double time, PercentHandler transition, double delay)
        {
            try
            {
                return ArtefactAnimator.AddEase(obj, AnimationTypes.X, X, time, transition, delay);
            }
            catch (Exception error) { Debug.WriteLine("[ERROR] ArtefactAnimatorExtensions - XTo - " + error); return null; }
        }
        //  instant
        public static void XTo(this UIElement obj, double x) { Canvas.SetLeft(obj, x); }

        public static EaseObject YTo(this UIElement obj, double y, double time, PercentHandler transition, double delay)
        {
            try
            {
                return ArtefactAnimator.AddEase(obj, AnimationTypes.Y, y, time, transition, delay);
            }
            catch (Exception error) { Debug.WriteLine("[ERROR] ArtefactAnimatorExtensions - YTo - " + error); return null; }
        }
        //  instant
        public static void YTo(this UIElement obj, double y) { Canvas.SetTop(obj, y); }

        public static EaseObject SlideTo(this UIElement obj, double x, double y, double time, PercentHandler transition, double delay)
        {
            try
            {
                return ArtefactAnimator.AddEase(obj, new[] { AnimationTypes.X, AnimationTypes.Y }, new object[] { x, y }, time, transition, delay);
            }
            catch (Exception error) { Debug.WriteLine("[ERROR] ArtefactAnimatorExtensions - SlideTo - " + error); return null; }
        }
        //  instant
        public static void SlideTo(this UIElement obj, double x, double y) { Canvas.SetLeft(obj, x); Canvas.SetTop(obj, y); }

        public static EaseObject HeightTo(this UIElement obj, double height, double time, PercentHandler transition, double delay)
        {
            try
            {
                return ArtefactAnimator.AddEase(obj, AnimationTypes.Height, height, time, transition, delay);
            }
            catch (Exception error) { Debug.WriteLine("[ERROR] ArtefactAnimatorExtensions - HeightTo - " + error); return null; }
        }
        //  instant
        public static void HeightTo(this UIElement obj, double height) { ((FrameworkElement) obj).Height = height; }


        public static EaseObject WidthTo(this UIElement obj, double width, double time, PercentHandler transition, double delay)
        {
            try
            {
                return ArtefactAnimator.AddEase(obj, AnimationTypes.Width, width, time, transition, delay);
            }
            catch (Exception error) { Debug.WriteLine("[ERROR] ArtefactAnimatorExtensions - WidthTo - " + error); return null; }
        }
        //  instant
        public static void WidthTo(this UIElement obj, double width) { ((FrameworkElement) obj).Width = width; }

        public static EaseObject DimensionsTo(this UIElement obj, double width, double height, double time, PercentHandler transition, double delay)
        {
            try
            {
                return ArtefactAnimator.AddEase(obj, new[] { AnimationTypes.Width, AnimationTypes.Height }, new object[] { width, height }, time, transition, delay);
            }
            catch (Exception error) { Debug.WriteLine("[ERROR] ArtefactAnimatorExtensions - DimensionsTo - " + error); return null; }
        }
        //  instant
        public static void DimensionsTo(this UIElement obj, double width, double height) { ((FrameworkElement) obj).Width = width; ((FrameworkElement) obj).Height = height; }

        public static EaseObject ScaleTo(this UIElement obj, double scaleX, double scaleY, double time, PercentHandler transition, double delay)
        {
            try
            {
                return ArtefactAnimator.AddEase(((TransformGroup) obj.RenderTransform).Children[(int)TransformGroupIndexes.ScaleTransform], new object[] { AnimationTypes.ScaleX, AnimationTypes.ScaleY }, new object[] { scaleX, scaleY }, time, transition, delay);
            }
            catch (Exception error) { Debug.WriteLine("[ERROR] ArtefactAnimatorExtensions - ScaleTo - " + error); return null; }
        }

        // instant
        public static EaseObject ScaleTo(this UIElement obj, double scaleX, double scaleY)
        {
            try
            {
                return ArtefactAnimator.AddEase(((TransformGroup)obj.RenderTransform).Children[(int)TransformGroupIndexes.ScaleTransform], new object[] { AnimationTypes.ScaleX, AnimationTypes.ScaleY }, new object[] { scaleX, scaleY }, 0, null, 0).Finish();
            }
            catch (Exception error) { Debug.WriteLine("[ERROR] ArtefactAnimatorExtensions - ScaleTo - " + error); return null; }
        }

        public static EaseObject RotateTo(this UIElement obj, double rotation, double time, PercentHandler transition, double delay)
        {
            try
            {
                return ArtefactAnimator.AddEase(((TransformGroup) obj.RenderTransform).Children[(int)TransformGroupIndexes.RotateTransform], AnimationTypes.Rotation, rotation, time, transition, delay);
            }
            catch (Exception error) { Debug.WriteLine("[ERROR] ArtefactAnimatorExtensions - RotateTo - " + error); return null; }
        }

        // instant
        public static EaseObject RotateTo(this UIElement obj, double rotation)
        {
            try
            {
                return ArtefactAnimator.AddEase(((TransformGroup)obj.RenderTransform).Children[(int)TransformGroupIndexes.RotateTransform], AnimationTypes.Rotation, rotation, 0, null, 0).Finish();
            }
            catch (Exception error) { Debug.WriteLine("[ERROR] ArtefactAnimatorExtensions - RotateTo - " + error); return null; }
        }

        public static EaseObject SkewTo(this UIElement obj, double skewX, double skewY, double time, PercentHandler transition, double delay)
        {
            try
            {
                return ArtefactAnimator.AddEase(((TransformGroup) obj.RenderTransform).Children[(int)TransformGroupIndexes.SkewTransform], new object[] { AnimationTypes.SkewX, AnimationTypes.SkewY }, new object[] { skewX, skewY }, time, transition, delay);
            }
            catch (Exception error) { Debug.WriteLine("[ERROR] ArtefactAnimatorExtensions - SkewTo - " + error); return null; }
        }

        // instant
        public static EaseObject SkewTo(this UIElement obj, double skewX, double skewY)
        {
            try
            {
                return ArtefactAnimator.AddEase(((TransformGroup)obj.RenderTransform).Children[(int)TransformGroupIndexes.SkewTransform], new object[] { AnimationTypes.SkewX, AnimationTypes.SkewY }, new object[] { skewX, skewY }, 0, null, 0).Finish();
            }
            catch (Exception error) { Debug.WriteLine("[ERROR] ArtefactAnimatorExtensions - SkewTo - " + error); return null; }
        }

        public static EaseObject OffsetTo(this UIElement obj, double offsetX, double offsetY, double time, PercentHandler transition, double delay)
        {
            try
            {
                return ArtefactAnimator.AddEase(((TransformGroup) obj.RenderTransform).Children[(int)TransformGroupIndexes.TranslateTransform], new object[] { AnimationTypes.OffsetX, AnimationTypes.OffsetY }, new object[] { offsetX, offsetY }, time, transition, delay);
            }
            catch (Exception error) { Debug.WriteLine("[ERROR] ArtefactAnimatorExtensions - OffsetTo - " + error); return null; }
        }

        // instant
        public static EaseObject OffsetTo(this UIElement obj, double offsetX, double offsetY)
        {
            try
            {
                return ArtefactAnimator.AddEase(((TransformGroup)obj.RenderTransform).Children[(int)TransformGroupIndexes.TranslateTransform], new object[] { AnimationTypes.OffsetX, AnimationTypes.OffsetY }, new object[] { offsetX, offsetY }, 0, null, 0).Finish();
            }
            catch (Exception error) { Debug.WriteLine("[ERROR] ArtefactAnimatorExtensions - OffsetTo - " + error); return null; }
        }


        // UTILS



        /// <summary>
        /// Get FrameworkElement.RenderTransform with Children in standard order [ ScaleTransform, SkewTransform, RotateTransform, TranslateTransform, ... ]
        /// </summary>
        public static TransformGroup NormalizedTransformGroup(this FrameworkElement element)
        {
            return ArtefactAnimator.NormalizeTransformGroup(element);
        }

        /// <summary>
        /// Sets and Returns FrameworkElement.RenderTransform with Children in standard order [ ScaleTransform, SkewTransform, RotateTransform, TranslateTransform, ... ]
        /// </summary>
        public static TransformGroup NormalizeTransformGroup(this FrameworkElement element)
        {
            TransformGroup tg = ArtefactAnimator.NormalizeTransformGroup(element);
            element.RenderTransform = tg;
            return tg;
        }

        /// <summary>
        /// T can be ScaleTransform, SkewTransform, RotateTransform, or TranslateTransform. If not found, the elements transform will be normalized and the function will run again.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public static T GetNormalizedTransform<T>(this FrameworkElement element) where T : Transform
        {
            var transformGroup = (TransformGroup)element.RenderTransform;
            if (transformGroup != null)
            {
                var transform = transformGroup.Children.OfType<T>().FirstOrDefault();
                if (transform != null) return transform;
            }

            // transform group is null or can't find type
            element.NormalizeTransformGroup();
            return element.GetNormalizedTransform<T>();
        }

        #if !SILVERLIGHT
        /// <summary>
        /// Get Model3D.Transform with Children in standard order [ TranslateTransform3D, ScaleTransform3D, RotateTransform3D, ... ]
        /// </summary>
        public static Transform3DGroup NormalizedTransform3DGroup(this Model3D obj)
        {
            return ArtefactAnimator.NormalizeTransform3DGroup(obj.Transform as Transform3DGroup);
        }

        /// <summary>
        /// Sets and Returns Model3D.Transform with Children in standard order [ TranslateTransform3D, ScaleTransform3D, RotateTransform3D, ... ]
        /// </summary>
        public static Transform3DGroup NormalizeTransform3DGroup(this Model3D obj)
        {
            obj.Transform = ArtefactAnimator.NormalizeTransform3DGroup(obj.Transform as Transform3DGroup);
            return obj.Transform as Transform3DGroup;
        }

        /// <summary>
        /// Get Model3D.Transform with Children in standard order [ TranslateTransform3D, ScaleTransform3D, RotateTransform3D, ... ]
        /// </summary>
        public static Transform3DGroup NormalizedTransform3DGroup(this Camera obj)
        {
            return ArtefactAnimator.NormalizeTransform3DGroup(obj.Transform as Transform3DGroup);
        }

        /// <summary>
        /// Sets and Returns Model3D.Transform with Children in standard order [ TranslateTransform3D, ScaleTransform3D, RotateTransform3D, ... ]
        /// </summary>
        public static Transform3DGroup NormalizeTransform3DGroup(this Camera obj)
        {
            obj.Transform = ArtefactAnimator.NormalizeTransform3DGroup(obj.Transform as Transform3DGroup);
            return obj.Transform as Transform3DGroup;
        }
        #endif

        // EASE OBJECTS

        public static EaseObject OnComplete(this EaseObject obj, EaseObjectHandler handler) { if (obj != null && handler != null) obj.Complete += handler; return obj; }
        public static EaseObject OnUpdate(this EaseObject obj, EaseObjectHandler handler) { if (obj != null && handler != null) obj.Update += handler; return obj; }
        public static EaseObject OnBegin(this EaseObject obj, EaseObjectHandler handler) { if (obj != null && handler != null) obj.Begin += handler; return obj; }
        public static EaseObject OnStopped(this EaseObject obj, EaseObjectHandler handler) { if (obj != null && handler != null) obj.Stopped += handler; return obj; }
        public static EaseObject Callback(this EaseObject obj, EventHandler handler) { if (obj != null && handler != null) obj.Complete += (eo, p) => { if (handler != null) handler(eo, EventArgs.Empty); }; return obj; }
    }
}
