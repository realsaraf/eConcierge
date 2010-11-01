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
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;

namespace Artefact.Animation
{
    public static class EaseHelper
    {
        #region NUMBERS
        /// <summary>
        /// Returns new double by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static double EaseValue(double startValue, double endValue, double percent)
        {
            return startValue == endValue ? endValue : (startValue + ((endValue - startValue) * percent));
        }

        /// <summary>
        /// Returns new int by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static int EaseValue(int startValue, int endValue, double percent)
        {
            return startValue == endValue ? endValue : (int)(startValue + ((endValue - startValue) * percent));
        }

        /// <summary>
        /// Returns new decimal by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static decimal EaseValue(decimal startValue, decimal endValue, double percent)
        {
            return startValue == endValue ? endValue : (decimal)(((double)startValue) + (((double)(endValue - startValue) * percent)));
        }

        /// <summary>
        /// Returns new byte by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static byte EaseValue(byte startValue, byte endValue, double percent)
        {
            return startValue == endValue ? endValue : (byte)(startValue + (((endValue - startValue) * percent)));
        }
        #endregion

        #region COLORS
        /// <summary>
        /// Returns new Color with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static Color EaseValue(Color startValue, Color endValue, double percent)
        {
            return new Color
            {
                A = EaseValue(startValue.A, endValue.A, percent),
                R = EaseValue(startValue.R, endValue.R, percent),
                G = EaseValue(startValue.G, endValue.G, percent),
                B = EaseValue(startValue.B, endValue.B, percent)
            };
        }
        #endregion

        #region BRUSHES
        /// <summary>
        /// Returns new SolidColorBrush with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static SolidColorBrush EaseValue(SolidColorBrush startValue, SolidColorBrush endValue, double percent)
        {
            return new SolidColorBrush
            {
                Opacity = EaseValue(startValue.Opacity, endValue.Opacity, percent),
                Color = EaseValue(startValue.Color, endValue.Color, percent),
            };
        }

        /// <summary>
        /// Returns new LinearGradientBrush with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static LinearGradientBrush EaseValue(LinearGradientBrush startValue, LinearGradientBrush endValue, double percent)
        {
            return new LinearGradientBrush
            {
                SpreadMethod = endValue.SpreadMethod,
                MappingMode = endValue.MappingMode,
                ColorInterpolationMode = endValue.ColorInterpolationMode,
                Opacity = EaseValue(startValue.Opacity, endValue.Opacity, percent),
                GradientStops = EaseValue(startValue.GradientStops, endValue.GradientStops, percent),
                EndPoint = EaseValue(startValue.EndPoint, endValue.EndPoint, percent),
                StartPoint = EaseValue(startValue.StartPoint, endValue.StartPoint, percent),
            };
        }

        /// <summary>
        /// Returns new RadialGradientBrush with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static RadialGradientBrush EaseValue(RadialGradientBrush startValue, RadialGradientBrush endValue, double percent)
        {
            return new RadialGradientBrush
            {
                SpreadMethod = endValue.SpreadMethod,
                MappingMode = endValue.MappingMode,
                ColorInterpolationMode = endValue.ColorInterpolationMode,
                Opacity = EaseValue(startValue.Opacity, endValue.Opacity, percent),
                RadiusX = EaseValue(startValue.RadiusX, endValue.RadiusX, percent),
                RadiusY = EaseValue(startValue.RadiusY, endValue.RadiusY, percent),
                Center = EaseValue(startValue.Center, endValue.Center, percent),
                GradientOrigin = EaseValue(startValue.GradientOrigin, endValue.GradientOrigin, percent),
                GradientStops = EaseValue(startValue.GradientStops, endValue.GradientStops, percent)
            };
        }

        /// <summary>
        /// Returns new GradientStopCollection with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static GradientStopCollection EaseValue(GradientStopCollection startValue, GradientStopCollection endValue, double percent)
        {
            if (endValue == null) return endValue;

            var collection = new GradientStopCollection();
            for (var i = 0; i < endValue.Count; i++)
            {
                var stop = new GradientStop();
                if (i < startValue.Count)
                {
                    stop.Offset = EaseValue(startValue[i].Offset, endValue[i].Offset, percent);
                    stop.Color = EaseValue(startValue[i].Color, endValue[i].Color, percent);
                }
                else
                {
                    stop.Offset = endValue[i].Offset;
                    stop.Color = endValue[i].Color;
                }
                collection.Add(stop);
            }
            return collection;
        }


        #endregion

        #region POINTS
        /// <summary>
        /// Returns new Point with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static Point EaseValue(Point startValue, Point endValue, double percent)
        {
            return new Point(EaseValue(startValue.X, endValue.X, percent), EaseValue(startValue.Y, endValue.Y, percent));
        }

        /// <summary>
        /// Returns new Rect with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static Rect EaseValue(Rect startValue, Rect endValue, double percent)
        {
            return new Rect(    EaseValue(startValue.X, endValue.X, percent), 
                                EaseValue(startValue.Y, endValue.Y, percent),
                                EaseValue(startValue.Width, endValue.Width, percent),
                                EaseValue(startValue.Height, endValue.Height, percent));
        }

        /// <summary>
        /// Returns new Matrix with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static Matrix EaseValue(Matrix startValue, Matrix endValue, double percent)
        {
            return new Matrix(  EaseValue(startValue.M11, endValue.M11, percent),
                                EaseValue(startValue.M12, endValue.M12, percent),
                                EaseValue(startValue.M21, endValue.M21, percent),
                                EaseValue(startValue.M22, endValue.M22, percent),
                                EaseValue(startValue.OffsetX, endValue.OffsetX, percent),
                                EaseValue(startValue.OffsetY, endValue.OffsetY, percent));
        }

        /// <summary>
        /// Returns new Matrix3D with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static Matrix3D EaseValue(Matrix3D startValue, Matrix3D endValue, double percent)
        {
            return new Matrix3D(EaseValue(startValue.M11, endValue.M11, percent),
                                EaseValue(startValue.M12, endValue.M12, percent),
                                EaseValue(startValue.M13, endValue.M13, percent),
                                EaseValue(startValue.M14, endValue.M14, percent),
                                EaseValue(startValue.M21, endValue.M21, percent),
                                EaseValue(startValue.M22, endValue.M22, percent),
                                EaseValue(startValue.M23, endValue.M23, percent),
                                EaseValue(startValue.M24, endValue.M24, percent),
                                EaseValue(startValue.M31, endValue.M31, percent),
                                EaseValue(startValue.M32, endValue.M32, percent),
                                EaseValue(startValue.M33, endValue.M33, percent),
                                EaseValue(startValue.M34, endValue.M34, percent),
                                EaseValue(startValue.OffsetX, endValue.OffsetX, percent),
                                EaseValue(startValue.OffsetY, endValue.OffsetY, percent),
                                EaseValue(startValue.OffsetZ, endValue.OffsetZ, percent),
                                EaseValue(startValue.M44, endValue.M44, percent));
        }
        #endregion

        #region BORDERS
        /// <summary>
        /// Returns new CornerRadius with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static CornerRadius EaseValue(CornerRadius startValue, CornerRadius endValue, double percent)
        {
            return new CornerRadius(    EaseValue(startValue.TopLeft, endValue.TopLeft, percent), 
                                        EaseValue(startValue.TopRight, endValue.TopRight, percent), 
                                        EaseValue(startValue.BottomLeft, endValue.BottomLeft, percent), 
                                        EaseValue(startValue.BottomRight, endValue.BottomRight, percent) );
        }
        #endregion

        #region THICKNESS / LENGTH
        /// <summary>
        /// Returns new Thickness with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static Thickness EaseValue(Thickness startValue, Thickness endValue, double percent)
        {
            return new Thickness(       EaseValue(startValue.Left, endValue.Left, percent),
                                        EaseValue(startValue.Right, endValue.Right, percent),
                                        EaseValue(startValue.Top, endValue.Top, percent),
                                        EaseValue(startValue.Bottom, endValue.Bottom, percent));
        }

        /// <summary>
        /// Returns new GridLength with positive values eased from startValue to endValue using a time percentage 0 -> 1. GridUnitType is determined by the endValue.
        /// </summary>
        public static GridLength EaseValue(GridLength startValue, GridLength endValue, double percent)
        {
            return new GridLength(Math.Max(0,EaseValue(startValue.Value, endValue.Value, percent)), endValue.GridUnitType);
        }
        
        #endregion

        #region EFFECTS
        /// <summary>
        /// Returns new BlurEffect with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static BlurEffect EaseValue(BlurEffect startValue, BlurEffect endValue, double percent)
        {
            if (startValue == null) startValue = new BlurEffect { Radius = 0 };
            if (endValue == null) endValue = new BlurEffect { Radius = 0 };
           
            return new BlurEffect {  Radius = EaseValue(startValue.Radius,endValue.Radius,percent)   };
        }

        /// <summary>
        /// Returns new DropShadowEffect with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static DropShadowEffect EaseValue(DropShadowEffect startValue, DropShadowEffect endValue, double percent)
        {
            if (startValue == null) startValue = new DropShadowEffect { Color = Color.FromArgb(0, 0, 0, 0), Opacity = 0 };
            if (endValue == null) endValue = new DropShadowEffect { Color = Color.FromArgb(0, 0, 0, 0), Opacity = 0 };

            return new DropShadowEffect {   BlurRadius = EaseValue(startValue.BlurRadius, endValue.BlurRadius, percent), 
                                            Color = EaseValue(startValue.Color, endValue.Color, percent),
                                            Direction = EaseValue(startValue.Direction, endValue.Direction, percent),
                                            Opacity = EaseValue(startValue.Opacity, endValue.Opacity, percent),
                                            ShadowDepth = EaseValue(startValue.ShadowDepth, endValue.ShadowDepth, percent)
            };
        }
        #endregion
    }
}
