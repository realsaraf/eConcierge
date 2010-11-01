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

  
 * WPF EASING EQUATIONS * Credit/Thanks:
 * 
 * Darren David - The easing equations in c#
 *   (http://code.google.com/p/wpf-animation/) [See License.txt for license info]
 *   
 * Robert Penner - The easing equations we all know and love 
 *   (http://robertpenner.com/easing/) [See License.txt for license info]
 * 
 * Lee Brimelow - initial port of Penner's equations to WPF 
 *   (http://thewpfblog.com/?p=12)
 * 
 * Zeh Fernando - additional equations (out/in) from 
 *   caurina.transitions.Tweener (http://code.google.com/p/tweener/)
 *   [See License.txt for license info]
 */




#if !SILVERLIGHT
using System; 
#endif

#if SILVERLIGHT
using System.Windows.Media.Animation;
#endif

namespace Artefact.Animation
{
    public static class AnimationTransitions
    {
        private const double B = 0;
        private const double C = 1;
        private const double D = 1;

        #if SILVERLIGHT
        // BACK
        private static readonly BackEase _BackEaseIn = new BackEase { EasingMode = EasingMode.EaseIn };
        private static readonly BackEase _BackEaseInOut = new BackEase { EasingMode = EasingMode.EaseInOut };
        private static readonly BackEase _BackEaseOut = new BackEase { EasingMode = EasingMode.EaseOut };
        public static readonly PercentHandler BackEaseIn = _BackEaseIn.Ease;
        public static readonly PercentHandler BackEaseInOut = _BackEaseInOut.Ease;
        public static readonly PercentHandler BackEaseOut = _BackEaseOut.Ease;

        // BOUNCE
        private static readonly BounceEase _BounceEaseIn = new BounceEase { EasingMode = EasingMode.EaseIn };
        private static readonly BounceEase _BounceEaseInOut = new BounceEase { EasingMode = EasingMode.EaseInOut };
        private static readonly BounceEase _BounceEaseOut = new BounceEase { EasingMode = EasingMode.EaseOut };
        public static readonly PercentHandler BounceEaseIn = _BounceEaseIn.Ease;
        public static readonly PercentHandler BounceEaseInOut = _BounceEaseInOut.Ease;
        public static readonly PercentHandler BounceEaseOut = _BounceEaseOut.Ease;

        // BOUNCE
        private static readonly CircleEase _CircleEaseIn = new CircleEase { EasingMode = EasingMode.EaseIn };
        private static readonly CircleEase _CircleEaseInOut = new CircleEase { EasingMode = EasingMode.EaseInOut };
        private static readonly CircleEase _CircleEaseOut = new CircleEase { EasingMode = EasingMode.EaseOut };
        public static readonly PercentHandler CircleEaseIn = _CircleEaseIn.Ease;
        public static readonly PercentHandler CircleEaseInOut = _CircleEaseInOut.Ease;
        public static readonly PercentHandler CircleEaseOut = _CircleEaseOut.Ease;

        // CUBIC
        private static readonly CubicEase _CubicEaseIn = new CubicEase { EasingMode = EasingMode.EaseIn };
        private static readonly CubicEase _CubicEaseInOut = new CubicEase { EasingMode = EasingMode.EaseInOut };
        private static readonly CubicEase _CubicEaseOut = new CubicEase { EasingMode = EasingMode.EaseOut };
        public static readonly PercentHandler CubicEaseIn = _CubicEaseIn.Ease;
        public static readonly PercentHandler CubicEaseInOut = _CubicEaseInOut.Ease;
        public static readonly PercentHandler CubicEaseOut = _CubicEaseOut.Ease;

        // ELASTIC
        private static readonly ElasticEase _ElasticEaseIn = new ElasticEase { EasingMode = EasingMode.EaseIn };
        private static readonly ElasticEase _ElasticEaseInOut = new ElasticEase { EasingMode = EasingMode.EaseInOut };
        private static readonly ElasticEase _ElasticEaseOut = new ElasticEase { EasingMode = EasingMode.EaseOut };
        public static readonly PercentHandler ElasticEaseIn = _ElasticEaseIn.Ease;
        public static readonly PercentHandler ElasticEaseInOut = _ElasticEaseInOut.Ease;
        public static readonly PercentHandler ElasticEaseOut = _ElasticEaseOut.Ease;

        // EXPONENTIAL
        private static readonly ExponentialEase _ExponentialEaseIn = new ExponentialEase { EasingMode = EasingMode.EaseIn };
        private static readonly ExponentialEase _ExponentialEaseInOut = new ExponentialEase { EasingMode = EasingMode.EaseInOut };
        private static readonly ExponentialEase _ExponentialEaseOut = new ExponentialEase { EasingMode = EasingMode.EaseOut };
        public static readonly PercentHandler ExponentialEaseIn = _ExponentialEaseIn.Ease;
        public static readonly PercentHandler ExponentialEaseInOut = _ExponentialEaseInOut.Ease;
        public static readonly PercentHandler ExponentialEaseOut = _ExponentialEaseOut.Ease;

        // POWER
        private static readonly PowerEase _PowerEaseIn = new PowerEase { EasingMode = EasingMode.EaseIn };
        private static readonly PowerEase _PowerEaseInOut = new PowerEase { EasingMode = EasingMode.EaseInOut };
        private static readonly PowerEase _PowerEaseOut = new PowerEase { EasingMode = EasingMode.EaseOut };
        public static readonly PercentHandler PowerEaseIn = _PowerEaseIn.Ease;
        public static readonly PercentHandler PowerEaseInOut = _PowerEaseInOut.Ease;
        public static readonly PercentHandler PowerEaseOut = _PowerEaseOut.Ease;


        // QUADRATIC
        private static readonly QuadraticEase _QuadraticEaseIn = new QuadraticEase { EasingMode = EasingMode.EaseIn };
        private static readonly QuadraticEase _QuadraticEaseInOut = new QuadraticEase { EasingMode = EasingMode.EaseInOut };
        private static readonly QuadraticEase _QuadraticEaseOut = new QuadraticEase { EasingMode = EasingMode.EaseOut };
        public static readonly PercentHandler QuadraticEaseIn = _QuadraticEaseIn.Ease;
        public static readonly PercentHandler QuadraticEaseInOut = _QuadraticEaseInOut.Ease;
        public static readonly PercentHandler QuadraticEaseOut = _QuadraticEaseOut.Ease;

        // QUARTIC
        private static readonly QuarticEase _QuarticEaseIn = new QuarticEase { EasingMode = EasingMode.EaseIn };
        private static readonly QuarticEase _QuarticEaseInOut = new QuarticEase { EasingMode = EasingMode.EaseInOut };
        private static readonly QuarticEase _QuarticEaseOut = new QuarticEase { EasingMode = EasingMode.EaseOut };
        public static readonly PercentHandler QuarticEaseIn = _QuarticEaseIn.Ease;
        public static readonly PercentHandler QuarticEaseInOut = _QuarticEaseInOut.Ease;
        public static readonly PercentHandler QuarticEaseOut = _QuarticEaseOut.Ease;

        // QUINTIC
        private static readonly QuinticEase _QuinticEaseIn = new QuinticEase { EasingMode = EasingMode.EaseIn };
        private static readonly QuinticEase _QuinticEaseInOut = new QuinticEase { EasingMode = EasingMode.EaseInOut };
        private static readonly QuinticEase _QuinticEaseOut = new QuinticEase { EasingMode = EasingMode.EaseOut };
        public static readonly PercentHandler QuinticEaseIn = _QuinticEaseIn.Ease;
        public static readonly PercentHandler QuinticEaseInOut = _QuinticEaseInOut.Ease;
        public static readonly PercentHandler QuinticEaseOut = _QuinticEaseOut.Ease;

        // SINE
        private static readonly SineEase _SineEaseIn = new SineEase { EasingMode = EasingMode.EaseIn };
        private static readonly SineEase _SineEaseInOut = new SineEase { EasingMode = EasingMode.EaseInOut };
        private static readonly SineEase _SineEaseOut = new SineEase { EasingMode = EasingMode.EaseOut };
        public static readonly PercentHandler SineEaseIn = _SineEaseIn.Ease;
        public static readonly PercentHandler SineEaseInOut = _SineEaseInOut.Ease;
        public static readonly PercentHandler SineEaseOut = _SineEaseOut.Ease;
        #endif

        #if !SILVERLIGHT
        public static readonly PercentHandler QuadEaseOut = percent => PennerDoubleAnimation.QuadEaseOut(percent, B, C, D);
        public static readonly PercentHandler QuadEaseIn = percent => PennerDoubleAnimation.QuadEaseIn(percent, B, C, D);
        public static readonly PercentHandler QuadEaseInOut = percent => PennerDoubleAnimation.QuadEaseInOut(percent, B, C, D);
        public static readonly PercentHandler QuadEaseOutIn = percent => PennerDoubleAnimation.QuadEaseOutIn(percent, B, C, D);
        public static readonly PercentHandler ExpoEaseOut = percent => PennerDoubleAnimation.ExpoEaseOut(percent, B, C, D);
        public static readonly PercentHandler ExpoEaseIn = percent => PennerDoubleAnimation.ExpoEaseIn(percent, B, C, D);
        public static readonly PercentHandler ExpoEaseInOut = percent => PennerDoubleAnimation.ExpoEaseInOut(percent, B, C, D);
        public static readonly PercentHandler ExpoEaseOutIn = percent => PennerDoubleAnimation.ExpoEaseOutIn(percent, B, C, D);
        public static readonly PercentHandler CubicEaseOut = percent => PennerDoubleAnimation.CubicEaseOut(percent, B, C, D);
        public static readonly PercentHandler CubicEaseIn = percent => PennerDoubleAnimation.CubicEaseIn(percent, B, C, D);
        public static readonly PercentHandler CubicEaseInOut = percent => PennerDoubleAnimation.CubicEaseInOut(percent, B, C, D);
        public static readonly PercentHandler CubicEaseOutIn = percent => PennerDoubleAnimation.CubicEaseOutIn(percent, B, C, D);
        public static readonly PercentHandler QuartEaseOut = percent => PennerDoubleAnimation.QuartEaseOut(percent, B, C, D);
        public static readonly PercentHandler QuartEaseIn = percent => PennerDoubleAnimation.QuartEaseIn(percent, B, C, D);
        public static readonly PercentHandler QuartEaseInOut = percent => PennerDoubleAnimation.QuartEaseInOut(percent, B, C, D);
        public static readonly PercentHandler QuartEaseOutIn = percent => PennerDoubleAnimation.QuartEaseOutIn(percent, B, C, D);
        public static readonly PercentHandler QuintEaseOut = percent => PennerDoubleAnimation.QuintEaseOut(percent, B, C, D);
        public static readonly PercentHandler QuintEaseIn = percent => PennerDoubleAnimation.QuintEaseIn(percent, B, C, D);
        public static readonly PercentHandler QuintEaseInOut = percent => PennerDoubleAnimation.QuintEaseInOut(percent, B, C, D);
        public static readonly PercentHandler QuintEaseOutIn = percent => PennerDoubleAnimation.QuintEaseOutIn(percent, B, C, D);
        public static readonly PercentHandler CircEaseOut = percent => PennerDoubleAnimation.CircEaseOut(percent, B, C, D);
        public static readonly PercentHandler CircEaseIn = percent => PennerDoubleAnimation.CircEaseIn(percent, B, C, D);
        public static readonly PercentHandler CircEaseInOut = percent => PennerDoubleAnimation.CircEaseInOut(percent, B, C, D);
        public static readonly PercentHandler CircEaseOutIn = percent => PennerDoubleAnimation.CircEaseOutIn(percent, B, C, D);
        public static readonly PercentHandler SineEaseOut = percent => PennerDoubleAnimation.SineEaseOut(percent, B, C, D);
        public static readonly PercentHandler SineEaseIn = percent => PennerDoubleAnimation.SineEaseIn(percent, B, C, D);
        public static readonly PercentHandler SineEaseInOut = percent => PennerDoubleAnimation.SineEaseInOut(percent, B, C, D);
        public static readonly PercentHandler SineEaseOutIn = percent => PennerDoubleAnimation.SineEaseOutIn(percent, B, C, D);
        public static readonly PercentHandler ElasticEaseOut = percent => PennerDoubleAnimation.ElasticEaseOut(percent, B, C, D);
        public static readonly PercentHandler ElasticEaseIn = percent => PennerDoubleAnimation.ElasticEaseIn(percent, B, C, D);
        public static readonly PercentHandler ElasticEaseInOut = percent => PennerDoubleAnimation.ElasticEaseInOut(percent, B, C, D);
        public static readonly PercentHandler ElasticEaseOutIn = percent => PennerDoubleAnimation.ElasticEaseOutIn(percent, B, C, D);
        public static readonly PercentHandler BounceEaseOut = percent => PennerDoubleAnimation.BounceEaseOut(percent, B, C, D);
        public static readonly PercentHandler BounceEaseIn = percent => PennerDoubleAnimation.BounceEaseIn(percent, B, C, D);
        public static readonly PercentHandler BounceEaseInOut = percent => PennerDoubleAnimation.BounceEaseInOut(percent, B, C, D);
        public static readonly PercentHandler BounceEaseOutIn = percent => PennerDoubleAnimation.BounceEaseOutIn(percent, B, C, D);
        public static readonly PercentHandler BackEaseOut = percent => PennerDoubleAnimation.BackEaseOut(percent, B, C, D);
        public static readonly PercentHandler BackEaseIn = percent => PennerDoubleAnimation.BackEaseIn(percent, B, C, D);
        public static readonly PercentHandler BackEaseInOut = percent => PennerDoubleAnimation.BackEaseInOut(percent, B, C, D);
        public static readonly PercentHandler BackEaseOutIn = percent => PennerDoubleAnimation.BackEaseOutIn(percent, B, C, D);  
        #endif
    }

    #if !SILVERLIGHT
    public class PennerDoubleAnimation
    {
        #region Equations

        // These methods are all public to enable reflection in GetCurrentValueCore.

        #region Linear

        /// <summary>
        /// Easing equation function for a simple linear tweening, with no easing.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double Linear(double t, double b, double c, double d)
        {
            return c * t / d + b;
        }

        #endregion

        #region Expo

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double ExpoEaseOut(double t, double b, double c, double d)
        {
            return (t == d) ? b + c : c * (-Math.Pow(2, -10 * t / d) + 1) + b;
        }

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double ExpoEaseIn(double t, double b, double c, double d)
        {
            return (t == 0) ? b : c * Math.Pow(2, 10 * (t / d - 1)) + b;
        }

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double ExpoEaseInOut(double t, double b, double c, double d)
        {
            if (t == 0)
                return b;

            if (t == d)
                return b + c;

            if ((t /= d / 2) < 1)
                return c / 2 * Math.Pow(2, 10 * (t - 1)) + b;

            return c / 2 * (-Math.Pow(2, -10 * --t) + 2) + b;
        }

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double ExpoEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return ExpoEaseOut(t * 2, b, c / 2, d);

            return ExpoEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #region Circular

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double CircEaseOut(double t, double b, double c, double d)
        {
            return c * Math.Sqrt(1 - (t = t / d - 1) * t) + b;
        }

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double CircEaseIn(double t, double b, double c, double d)
        {
            return -c * (Math.Sqrt(1 - (t /= d) * t) - 1) + b;
        }

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double CircEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d / 2) < 1)
                return -c / 2 * (Math.Sqrt(1 - t * t) - 1) + b;

            return c / 2 * (Math.Sqrt(1 - (t -= 2) * t) + 1) + b;
        }

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double CircEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return CircEaseOut(t * 2, b, c / 2, d);

            return CircEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #region Quad

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuadEaseOut(double t, double b, double c, double d)
        {
            return -c * (t /= d) * (t - 2) + b;
        }

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuadEaseIn(double t, double b, double c, double d)
        {
            return c * (t /= d) * t + b;
        }

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuadEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d / 2) < 1)
                return c / 2 * t * t + b;

            return -c / 2 * ((--t) * (t - 2) - 1) + b;
        }

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuadEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return QuadEaseOut(t * 2, b, c / 2, d);

            return QuadEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #region Sine

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double SineEaseOut(double t, double b, double c, double d)
        {
            return c * Math.Sin(t / d * (Math.PI / 2)) + b;
        }

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double SineEaseIn(double t, double b, double c, double d)
        {
            return -c * Math.Cos(t / d * (Math.PI / 2)) + c + b;
        }

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double SineEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d / 2) < 1)
                return c / 2 * (Math.Sin(Math.PI * t / 2)) + b;

            return -c / 2 * (Math.Cos(Math.PI * --t / 2) - 2) + b;
        }

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing in/out: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double SineEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return SineEaseOut(t * 2, b, c / 2, d);

            return SineEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #region Cubic

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double CubicEaseOut(double t, double b, double c, double d)
        {
            return c * ((t = t / d - 1) * t * t + 1) + b;
        }

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double CubicEaseIn(double t, double b, double c, double d)
        {
            return c * (t /= d) * t * t + b;
        }

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double CubicEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d / 2) < 1)
                return c / 2 * t * t * t + b;

            return c / 2 * ((t -= 2) * t * t + 2) + b;
        }

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double CubicEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return CubicEaseOut(t * 2, b, c / 2, d);

            return CubicEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #region Quartic

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuartEaseOut(double t, double b, double c, double d)
        {
            return -c * ((t = t / d - 1) * t * t * t - 1) + b;
        }

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuartEaseIn(double t, double b, double c, double d)
        {
            return c * (t /= d) * t * t * t + b;
        }

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuartEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d / 2) < 1)
                return c / 2 * t * t * t * t + b;

            return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
        }

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuartEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return QuartEaseOut(t * 2, b, c / 2, d);

            return QuartEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #region Quintic

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuintEaseOut(double t, double b, double c, double d)
        {
            return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
        }

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuintEaseIn(double t, double b, double c, double d)
        {
            return c * (t /= d) * t * t * t * t + b;
        }

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuintEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d / 2) < 1)
                return c / 2 * t * t * t * t * t + b;
            return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
        }

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuintEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return QuintEaseOut(t * 2, b, c / 2, d);
            return QuintEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #region Elastic

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double ElasticEaseOut(double t, double b, double c, double d)
        {
            if ((t /= d) == 1)
                return b + c;

            double p = d * .3;
            double s = p / 4;

            return (c * Math.Pow(2, -10 * t) * Math.Sin((t * d - s) * (2 * Math.PI) / p) + c + b);
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double ElasticEaseIn(double t, double b, double c, double d)
        {
            if ((t /= d) == 1)
                return b + c;

            double p = d * .3;
            double s = p / 4;

            return -(c * Math.Pow(2, 10 * (t -= 1)) * Math.Sin((t * d - s) * (2 * Math.PI) / p)) + b;
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double ElasticEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d / 2) == 2)
                return b + c;

            double p = d * (.3 * 1.5);
            double s = p / 4;

            if (t < 1)
                return -.5 * (c * Math.Pow(2, 10 * (t -= 1)) * Math.Sin((t * d - s) * (2 * Math.PI) / p)) + b;
            return c * Math.Pow(2, -10 * (t -= 1)) * Math.Sin((t * d - s) * (2 * Math.PI) / p) * .5 + c + b;
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double ElasticEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return ElasticEaseOut(t * 2, b, c / 2, d);
            return ElasticEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #region Bounce

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BounceEaseOut(double t, double b, double c, double d)
        {
            if ((t /= d) < (1 / 2.75))
                return c * (7.5625 * t * t) + b;
            else if (t < (2 / 2.75))
                return c * (7.5625 * (t -= (1.5 / 2.75)) * t + .75) + b;
            else if (t < (2.5 / 2.75))
                return c * (7.5625 * (t -= (2.25 / 2.75)) * t + .9375) + b;
            else
                return c * (7.5625 * (t -= (2.625 / 2.75)) * t + .984375) + b;
        }

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BounceEaseIn(double t, double b, double c, double d)
        {
            return c - BounceEaseOut(d - t, 0, c, d) + b;
        }

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BounceEaseInOut(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return BounceEaseIn(t * 2, 0, c, d) * .5 + b;
            else
                return BounceEaseOut(t * 2 - d, 0, c, d) * .5 + c * .5 + b;
        }

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BounceEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return BounceEaseOut(t * 2, b, c / 2, d);
            return BounceEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #region Back

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BackEaseOut(double t, double b, double c, double d)
        {
            return c * ((t = t / d - 1) * t * ((1.70158 + 1) * t + 1.70158) + 1) + b;
        }

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BackEaseIn(double t, double b, double c, double d)
        {
            return c * (t /= d) * t * ((1.70158 + 1) * t - 1.70158) + b;
        }

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BackEaseInOut(double t, double b, double c, double d)
        {
            double s = 1.70158;
            if ((t /= d / 2) < 1)
                return c / 2 * (t * t * (((s *= (1.525)) + 1) * t - s)) + b;
            return c / 2 * ((t -= 2) * t * (((s *= (1.525)) + 1) * t + s) + 2) + b;
        }

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BackEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return BackEaseOut(t * 2, b, c / 2, d);
            return BackEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #endregion
    }
    #endif 
}
