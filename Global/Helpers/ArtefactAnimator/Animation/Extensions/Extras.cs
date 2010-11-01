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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Artefact.Animation.Extensions
{
    public static class AnimationBlurEffects
    {
        /// <summary>
        /// DropShadowEffect.BlurRadiusProperty
        /// </summary>
        public const string BlurRadius = "dropshadoweffect_blurradius";

        /// <summary>
        /// DropShadowEffect.ShadowDepthProperty
        /// </summary>
        public const string ShadowDepth = "dropshadoweffect_shadowdepth";

        /// <summary>
        /// DropShadowEffect.Opacity
        /// </summary>
        public const string Opacity = "dropshadoweffect_opacity";    
  
        /// <summary>
        /// DropShadowEffect.Color
        /// </summary>
        public const string Color = "dropshadoweffect_color";       
 
        /// <summary>
        /// DropShadowEffect.Direction
        /// </summary>
        public const string Direction = "dropshadoweffect_direction";    

        /// <summary>
        /// DropShadowEffect
        /// </summary>
        public const string DropShadowEffect = "dropshadoweffect";

        public static Dictionary<string, object> Shortcuts = new Dictionary<string, object>
        {
             { BlurRadius, System.Windows.Media.Effects.DropShadowEffect.BlurRadiusProperty }
            ,{ ShadowDepth, System.Windows.Media.Effects.DropShadowEffect.ShadowDepthProperty }
            ,{ Opacity, System.Windows.Media.Effects.DropShadowEffect.OpacityProperty }
            ,{ Direction, System.Windows.Media.Effects.DropShadowEffect.DirectionProperty }
        };

        public static void EaseValue(DropShadowEffect current, DropShadowEffect startValue, DropShadowEffect endValue, double percent)
        {
            if ( current.BlurRadius != endValue.BlurRadius) current.BlurRadius  = EaseHelper.EaseValue(startValue.BlurRadius, endValue.BlurRadius, percent);
            if (current.Opacity != endValue.Opacity) current.Opacity = EaseHelper.EaseValue(startValue.Opacity, endValue.Opacity, percent);
            if (current.Color != endValue.Color) current.Color = EaseHelper.EaseValue(startValue.Color, endValue.Color, percent);
            if (current.Direction != endValue.Direction) current.Direction = EaseHelper.EaseValue(startValue.Direction, endValue.Direction, percent);
            if (current.ShadowDepth != endValue.ShadowDepth) current.ShadowDepth = EaseHelper.EaseValue(startValue.ShadowDepth, endValue.ShadowDepth, percent);
        }

        private static readonly Dictionary<string, GetterSetter> GetterSetterHash = new Dictionary<string, GetterSetter>
        {
            // ALPHA
            {DropShadowEffect, new GetterSetter {
                Getter = (obj, data)=> obj,
                Setter = (obj, data, per)=> 
                            EaseValue((DropShadowEffect)obj, (DropShadowEffect)data.ValueStart, (DropShadowEffect)data.ValueEnd, per )
            }}
        };

        public static void Init()
        {
            return;
            // SHORTCUTS
            foreach (var data in Shortcuts)
            {
                AnimationTypes.RegisterShortcut(data.Key, data.Value);
            }

            // GETTER SETTERS
            foreach (var data in GetterSetterHash)
            {
                AnimationTypes.RegisterGetterSetter(data.Key, data.Value);
            }
        }

        // EXTENSIONS

        public static DropShadowEffect Copy ( this DropShadowEffect effect )
        {
            if (effect == null) return new DropShadowEffect();
            return new DropShadowEffect
            {
                BlurRadius  = effect.BlurRadius,
                Color       = effect.Color,
                Direction   = effect.Direction,
                Opacity     = effect.Opacity,
                ShadowDepth = effect.ShadowDepth
            };
        }
    }

    #if SILVERLIGHT
    public static class Animation3DEffects
    {
        /// <summary>
        /// PlaneProjection.RotationXProperty 
        /// </summary>
        public const string RotationX = "rotationx";

        /// <summary>
        /// PlaneProjection.RotationYProperty 
        /// </summary>
        public const string RotationY = "rotationy";
        
        /// <summary>
        /// PlaneProjection.RotationZProperty 
        /// </summary>
        public const string RotationZ = "rotationz";

        /// <summary>
        /// PlaneProjection.CenterOfRotationXProperty 
        /// </summary>
        public const string CenterOfRotationX = "centerofrotationx";

        /// <summary>
        /// PlaneProjection.CenterOfRotationYProperty 
        /// </summary>
        public const string CenterOfRotationY = "centerofrotationy";

        /// <summary>
        /// PlaneProjection.CenterOfRotationZProperty 
        /// </summary>
        public const string CenterOfRotationZ = "centerofrotationz";

        /// <summary>
        /// PlaneProjection.LocalOffsetXProperty 
        /// </summary>
        public const string LocalOffsetX = "localoffsetx";

        /// <summary>
        /// PlaneProjection.LocalOffsetYProperty 
        /// </summary>
        public const string LocalOffsetY = "localoffsety";

        /// <summary>
        /// PlaneProjection.LocalOffsetZProperty 
        /// </summary>
        public const string LocalOffsetZ = "localoffsetz";

        /// <summary>
        /// PlaneProjection.GlobalOffsetXProperty 
        /// </summary>
        public const string GlobalOffsetX = "globaloffsetx";

        /// <summary>
        /// PlaneProjection.GlobalOffsetYProperty 
        /// </summary>
        public const string GlobalOffsetY = "globaloffsety";

        /// <summary>
        /// PlaneProjection.GlobalOffsetZProperty 
        /// </summary>
        public const string GlobalOffsetZ = "globaloffsetz";

        /// <summary>
        /// PlaneProjection.ProjectionMatrix
        /// </summary>
        public const string ProjectionMatrix = "projectionmatrix";

        /// <summary>
        /// (PlaneProjection) UIElement.Projection  
        /// </summary>
        public const string Projection = "planeprojectobj";

        public static Dictionary<string, object> Shortcuts = new Dictionary<string, object>()
        {
             { RotationX, PlaneProjection.RotationXProperty }
            ,{ RotationY, PlaneProjection.RotationYProperty }
            ,{ RotationZ, PlaneProjection.RotationZProperty }
            ,{ CenterOfRotationX, PlaneProjection.CenterOfRotationXProperty }
            ,{ CenterOfRotationY, PlaneProjection.CenterOfRotationYProperty }
            ,{ CenterOfRotationZ, PlaneProjection.CenterOfRotationZProperty }
            ,{ LocalOffsetX, PlaneProjection.LocalOffsetXProperty }
            ,{ LocalOffsetY, PlaneProjection.LocalOffsetYProperty }
            ,{ LocalOffsetZ, PlaneProjection.LocalOffsetZProperty }
            ,{ GlobalOffsetX, PlaneProjection.GlobalOffsetXProperty }
            ,{ GlobalOffsetY, PlaneProjection.GlobalOffsetYProperty }
            ,{ GlobalOffsetZ, PlaneProjection.GlobalOffsetZProperty }
        };

        public static void EaseValue(Matrix3D current, Matrix3D startValue, Matrix3D endValue, double percent)
        {
            if (current.M11 != endValue.M11) EaseHelper.EaseValue(startValue.M11, endValue.M11, percent); 
            if (current.M12 != endValue.M12) EaseHelper.EaseValue(startValue.M12, endValue.M12, percent); 
            if (current.M13 != endValue.M13) EaseHelper.EaseValue(startValue.M13, endValue.M13, percent); 
            if (current.M14 != endValue.M14) EaseHelper.EaseValue(startValue.M14, endValue.M14, percent); 
            if (current.M21 != endValue.M21) EaseHelper.EaseValue(startValue.M21, endValue.M21, percent); 
            if (current.M22 != endValue.M22) EaseHelper.EaseValue(startValue.M22, endValue.M22, percent); 
            if (current.M23 != endValue.M23) EaseHelper.EaseValue(startValue.M23, endValue.M23, percent); 
            if (current.M24 != endValue.M24) EaseHelper.EaseValue(startValue.M24, endValue.M24, percent); 
            if (current.M31 != endValue.M31) EaseHelper.EaseValue(startValue.M31, endValue.M31, percent); 
            if (current.M32 != endValue.M32) EaseHelper.EaseValue(startValue.M32, endValue.M32, percent); 
            if (current.M33 != endValue.M33) EaseHelper.EaseValue(startValue.M33, endValue.M33, percent); 
            if (current.M34 != endValue.M34) EaseHelper.EaseValue(startValue.M34, endValue.M34, percent); 
            if (current.M44 != endValue.M44) EaseHelper.EaseValue(startValue.M44, endValue.M44, percent); 
            if (current.OffsetX != endValue.OffsetX) EaseHelper.EaseValue(startValue.OffsetX, endValue.OffsetX, percent); 
            if (current.OffsetY != endValue.OffsetY) EaseHelper.EaseValue(startValue.OffsetY, endValue.OffsetY, percent);
            if (current.OffsetZ != endValue.OffsetZ) EaseHelper.EaseValue(startValue.OffsetZ, endValue.OffsetZ, percent); 
        }

        public static void EaseValue(PlaneProjection current, PlaneProjection startValue, PlaneProjection endValue, double percent)
        {
            if (current.RotationX != endValue.RotationX) current.RotationX = EaseHelper.EaseValue(startValue.RotationX, endValue.RotationX, percent);
            if (current.RotationY != endValue.RotationY) current.RotationY = EaseHelper.EaseValue(startValue.RotationY, endValue.RotationY, percent);
            if (current.RotationZ != endValue.RotationZ) current.RotationZ = EaseHelper.EaseValue(startValue.RotationZ, endValue.RotationZ, percent);
            if (current.LocalOffsetX != endValue.LocalOffsetX) current.LocalOffsetX = EaseHelper.EaseValue(startValue.LocalOffsetX, endValue.LocalOffsetX, percent);
            if (current.LocalOffsetY != endValue.LocalOffsetY) current.LocalOffsetY = EaseHelper.EaseValue(startValue.LocalOffsetY, endValue.LocalOffsetY, percent);
            if (current.LocalOffsetZ != endValue.LocalOffsetZ) current.LocalOffsetZ = EaseHelper.EaseValue(startValue.LocalOffsetZ, endValue.LocalOffsetZ, percent);
            if (current.GlobalOffsetX != endValue.GlobalOffsetX) current.GlobalOffsetX = EaseHelper.EaseValue(startValue.GlobalOffsetX, endValue.GlobalOffsetX, percent);
            if (current.GlobalOffsetY != endValue.GlobalOffsetY) current.GlobalOffsetY = EaseHelper.EaseValue(startValue.GlobalOffsetY, endValue.GlobalOffsetY, percent);
            if (current.GlobalOffsetZ != endValue.GlobalOffsetZ) current.GlobalOffsetZ = EaseHelper.EaseValue(startValue.GlobalOffsetZ, endValue.GlobalOffsetZ, percent);
            if (current.CenterOfRotationX != endValue.CenterOfRotationX) current.CenterOfRotationX = EaseHelper.EaseValue(startValue.CenterOfRotationX, endValue.CenterOfRotationX, percent);
            if (current.CenterOfRotationY != endValue.CenterOfRotationY) current.CenterOfRotationY = EaseHelper.EaseValue(startValue.CenterOfRotationY, endValue.CenterOfRotationY, percent);
            if (current.CenterOfRotationZ != endValue.CenterOfRotationZ) current.CenterOfRotationZ = EaseHelper.EaseValue(startValue.CenterOfRotationZ, endValue.CenterOfRotationZ, percent);

            //  NOT TEST
            //if (current.ProjectionMatrix != endValue.ProjectionMatrix) EaseValue(current.ProjectionMatrix, startValue.ProjectionMatrix, endValue.ProjectionMatrix, percent);
        }

        private static readonly Dictionary<string, GetterSetter> GetterSetterHash = new Dictionary<string, GetterSetter>
        {
            // (PlaneProjection) UIElement.Projection
            {Projection, new GetterSetter { 
                Getter = (obj, data)=> ((UIElement) obj).Projection ?? new PlaneProjection(),
                Setter = (obj, data, per)=>
                {
                    var element = ((UIElement) obj);
                    if ( element.Projection == null) element.Projection = new PlaneProjection();

                    EaseValue((PlaneProjection)element.Projection, (PlaneProjection)data.ValueStart, (PlaneProjection)data.ValueEnd, per);
                 }
            }},

            // PlaneProjection.ProjectionMatrix - NOT TESTED
            {ProjectionMatrix, new GetterSetter { 
                Getter = (obj, data)=> ((PlaneProjection) obj).ProjectionMatrix,
                Setter = (obj, data, per)=>
                {
                    var element = ((PlaneProjection) obj);
                    EaseValue(element.ProjectionMatrix, (Matrix3D)data.ValueStart, (Matrix3D)data.ValueEnd, per);
                 }
            }}
        };


        public static void Init()
        {
            try
            {
                // SHORTCUTS
                foreach (var data in Shortcuts)
                {
                    AnimationTypes.RegisterShortcut(data.Key, data.Value);
                }

                // GETTER SETTERS
                foreach (var data in GetterSetterHash)
                {
                    AnimationTypes.RegisterGetterSetter(data.Key, data.Value);
                }
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("[ERROR] - {0} - Init - {1}", typeof(Animation3DEffects), error));
            }
        }
    }
    #endif
}
