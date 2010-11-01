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
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;

namespace Artefact.Animation
{
    public static class AnimationTypes
    {
        #region DEPENDENCY PROPERTY -> GETTER / SETTER NAMES
        #region (DIRECT)

        /// <summary>
        /// Sets RowDefinition.Height to a new GridLength and applies eased value with GridUnitType.Pixel
        /// </summary>
        public const string RowHeightPixels = "rowheightpixels";

        /// <summary>
        /// Sets ColumnDefinition.Width to a new GridLength and applies eased value with GridUnitType.Pixel
        /// </summary>
        public const string ColumWidthPixels = "columwidthpixels";

        /// <summary>
        /// Sets RowDefinition.Height to a new GridLength and applies eased value with GridUnitType.Star
        /// </summary>
        public const string RowHeightStar = "rowheightstar";

        /// <summary>
        /// Sets ColumnDefinition.Width to a new GridLength and applies eased value with GridUnitType.Star
        /// </summary>
        public const string ColumWidthStar = "columwidthstar";
        #endregion

        #region (INDIRECT)

        /// <summary>
        /// Sets FrameworkElement.Margin.Top by creating a new Thickness and setting the FrameworkElement.Margin property
        /// </summary>
        public const string MarginTop = "margintop";

        /// <summary>
        /// Sets FrameworkElement.Margin.Bottom by creating a new Thickness and setting the FrameworkElement.Margin property
        /// </summary>
        public const string MarginBottom = "marginbottom";

        /// <summary>
        /// Sets FrameworkElement.Margin.Left by creating a new Thickness and setting the FrameworkElement.Margin property
        /// </summary>
        public const string MarginLeft = "marginleft";

        /// <summary>
        /// Sets FrameworkElement.Margin.Rigth by creating a new Thickness and setting the FrameworkElement.Margin property
        /// </summary>
        public const string MarginRight = "marginright";
        #endregion
        #endregion

        #region DEPENDENCY PROPERTY NAMES
        /// <summary>
        /// Affects the Canvas.Left property of a UIElement
        /// </summary>
        public const string X = "x";

        /// <summary>
        /// Affects the Canvas.Top property of a UIElement
        /// </summary>
        public const string Y = "y";

        /// <summary>
        /// Affects the Canvas.Width property of a UIElement
        /// </summary>
        public const string Width = "width";

        /// <summary>
        /// Affects the Canvas.Height property of a UIElement
        /// </summary>
        public const string Height = "height";

        /// <summary>
        /// Affects the Canvas.ActualWidth property of a UIElement
        /// </summary>
        public const string ActualWidth = "actualwidth";

        /// <summary>
        /// Affects the Canvas.Height property of a UIElement
        /// </summary>
        public const string ActualHeight = "actualheight";

        /// <summary>
        /// Affects the Canvas.Opacity property of a UIElement
        /// </summary>
        public const string Alpha = "alpha";

        /// <summary>
        /// Affects the Canvas.Opacity property of a UIElement. When in WPF, 0 will set the Visibility of an element to Visibility.Hidden - Silverlight uses Visibility.Collapsed.
        /// </summary>
        public const string AutoAlpha = "autoalpha";

        /// <summary>
        /// Affects the Canvas.Opacity property of a UIElement. 0 will set the Visibility of an element to Visibility.Collapsed.
        /// </summary>
        public const string AutoAlphaCollapsed = "autoalphacollapsed";

        /// <summary>
        /// Applies to the ScaleTransform ScaleXProperty
        /// </summary>
        public const string ScaleX = "scalex";

        /// <summary>
        /// Applies to the ScaleTransform ScaleYProperty
        /// </summary>
        public const string ScaleY = "scaley";

        /// <summary>
        /// Applies to the SkewTransform AngleXProperty
        /// </summary>
        public const string SkewX = "skewx";

        /// <summary>
        /// Applies to the SkewTransform AngleYProperty
        /// </summary>
        public const string SkewY = "skewy";

        /// <summary>
        /// Applies to the RotateTransform AngleProperty
        /// </summary>
        public const string Rotation = "rotation";

        /// <summary>
        /// Applies to the TranslateTransform OffsetX
        /// </summary>
        public const string OffsetX = "offsetx";

        /// <summary>
        /// Applies to the TranslateTransform OffsetY
        /// </summary>
        public const string OffsetY = "offsety";

        /// <summary> 
        /// Eases the A,B,R,G values on a Color object to the values of a new Color Object.
        /// </summary>
        public const string ColorObj = "colorobj";
        #endregion

        #region GETTERSETTER
        /// <summary>
        /// Some values will throw errors
        /// </summary>
        public static readonly GetterSetter PositiveDoubleGetterSetter = new GetterSetter
        {
            Getter = GetPositiveDoubleDependencyPropValue,
            Setter = SetPositiveDependencyPropValue
        };
        #endregion

        #region GETTERS / SETTERS
        public static object GetDependencyPropValue(object obj, GetterSetterData data)
        {
            return ((DependencyObject)obj).GetValue(data.Prop as DependencyProperty);
        }

        public static object GetDoubleDependencyPropValue(object obj, GetterSetterData data)
        {
            return ((DependencyObject)obj).GetValue(data.Prop as DependencyProperty);
        }

        public static void SetDoubleDependencyPropValue(object obj, GetterSetterData data, double percent)
        {
            if (data != null) ((DependencyObject)obj).SetValue(data.Prop as DependencyProperty, EaseHelper.EaseValue((double)data.ValueStart, (double)data.ValueEnd, percent));
        }

        public static object GetPositiveDoubleDependencyPropValue(object obj, GetterSetterData data)
        {
            return ((DependencyObject)obj).GetValue(data.Prop as DependencyProperty);
        }

        public static void SetPositiveDependencyPropValue(object obj, GetterSetterData data, double percent)
        {
            if (data != null) ((DependencyObject)obj).SetValue(data.Prop as DependencyProperty, Math.Max(0D, EaseHelper.EaseValue((double)data.ValueStart, (double)data.ValueEnd, percent)));
        }
        #endregion

        #region GETTER / SETTER DATA
        /// <summary>
        /// String -> DependencyProperty Lookup
        /// </summary>
        public static readonly Dictionary<string, object> Shortcuts = new Dictionary<string, object>
        {
            { X, Canvas.LeftProperty }
           ,{ Y, Canvas.TopProperty }
           ,{ Width, FrameworkElement.WidthProperty }
           ,{ Height, FrameworkElement.HeightProperty }
           ,{ ActualWidth, FrameworkElement.ActualWidthProperty }
           ,{ ActualHeight, FrameworkElement.ActualHeightProperty }
           ,{ Alpha, UIElement.OpacityProperty } // works with WPF and Silverlight
           ,{ ScaleX, ScaleTransform.ScaleXProperty } 
           ,{ ScaleY, ScaleTransform.ScaleYProperty } 
           ,{ SkewX, SkewTransform.AngleXProperty } 
           ,{ SkewY, SkewTransform.AngleYProperty } 
           ,{ Rotation, RotateTransform.AngleProperty } 
           ,{ OffsetX, TranslateTransform.XProperty } 
           ,{ OffsetY, TranslateTransform.YProperty } 
        };

        /// <summary>
        /// String -> GetterSetter Lookup
        /// </summary>
        public static readonly Dictionary<string, GetterSetter> GetterSetterHash = new Dictionary<string, GetterSetter>
        {
               #region ALPHA
              {AutoAlpha, new GetterSetter {
                                 Getter = (obj, data)=> ((FrameworkElement) obj).Opacity,
                                 Setter = (obj, data, percent)=>
                                 { 
                                    ((FrameworkElement) obj).Opacity = Math.Max(0,Math.Min(1,EaseHelper.EaseValue((double)data.ValueStart, (double)data.ValueEnd, percent)));  
                                    if (((FrameworkElement) obj).Opacity > 0)
                                    {
                                        ((FrameworkElement) obj).Visibility = Visibility.Visible;
                                    }
                                    else if ( ((FrameworkElement) obj).Visibility == Visibility.Visible) 
                                    {
                                        #if SILVERLIGHT
                                        (obj as FrameworkElement).Visibility = Visibility.Collapsed;
                                        #endif
                                        #if !SILVERLIGHT
                                        ((FrameworkElement) obj).Visibility = Visibility.Hidden;
                                        #endif                                                
                                    }
                                }}
                                },

              {AutoAlphaCollapsed, new GetterSetter {  
                                Getter = (obj, data)=> ((FrameworkElement) obj).Opacity,
                                Setter = (obj, data, percent)=>
                                { 
                                    ((FrameworkElement) obj).Opacity = Math.Max(0,Math.Min(1,EaseHelper.EaseValue((double)data.ValueStart, (double)data.ValueEnd, percent)));  
                                    if (((FrameworkElement) obj).Opacity > 0)
                                    {
                                        ((FrameworkElement) obj).Visibility = Visibility.Visible;
                                    }
                                    else if ( ((FrameworkElement) obj).Visibility != Visibility.Collapsed) 
                                    {
                                        #if SILVERLIGHT
                                        (obj as FrameworkElement).Visibility = Visibility.Collapsed;
                                        #endif
                                        #if !SILVERLIGHT
                                        ((FrameworkElement) obj).Visibility = Visibility.Collapsed;
                                        #endif                                                
                                    }
                                }}
                                }
                #endregion

               #region GRID
              //    pixel
              ,{RowHeightPixels, new GetterSetter {  
                            Getter = (obj, data)=> ((RowDefinition) obj).Height.Value,
                            Setter = (obj, data, percent)=>{  ((RowDefinition) obj).Height = new GridLength(Math.Max(0,EaseHelper.EaseValue((double)data.ValueStart, (double)data.ValueEnd, percent)), GridUnitType.Pixel); } 
                            }}

              ,{ColumWidthPixels, new GetterSetter {  
                            Getter = (obj, data)=> ((ColumnDefinition) obj).Width.Value,
                            Setter = (obj, data, percent)=>{((ColumnDefinition) obj).Width = new GridLength(Math.Max(0,EaseHelper.EaseValue((double)data.ValueStart, (double)data.ValueEnd, percent)), GridUnitType.Pixel); }
                            }}

               //    star                     
              ,{RowHeightStar, new GetterSetter {  
                             Getter = (obj, data)=> ((RowDefinition) obj).Height.Value,
                             Setter = (obj, data, percent)=>{ ((RowDefinition) obj).Height = new GridLength(Math.Max(0,EaseHelper.EaseValue((double)data.ValueStart, (double)data.ValueEnd, percent)), GridUnitType.Star); }
                            }}
              ,{ColumWidthStar, new GetterSetter { 
                             Getter = (obj, data)=> ((ColumnDefinition) obj).Width.Value,
                             Setter = (obj, data, percent)=>{((ColumnDefinition) obj).Width = new GridLength(Math.Max(0,EaseHelper.EaseValue((double)data.ValueStart, (double)data.ValueEnd, percent)), GridUnitType.Star); }
                            }}
               #endregion

               #region MARGIN

              ,{MarginTop, new GetterSetter {  
                             Getter = (obj, data)=> ((FrameworkElement) obj).Margin.Top,
                             Setter = (obj, data, percent)=>{ ((FrameworkElement) obj).Margin = new Thickness(((FrameworkElement) obj).Margin.Left, EaseHelper.EaseValue((double)data.ValueStart, (double)data.ValueEnd, percent), (obj as FrameworkElement).Margin.Right, (obj as FrameworkElement).Margin.Bottom); }
                            }}
              ,{MarginBottom, new GetterSetter {
                             Getter = (obj, data)=> ((FrameworkElement) obj).Margin.Bottom,
                             Setter = (obj, data, percent)=>{ ((FrameworkElement) obj).Margin = new Thickness(((FrameworkElement) obj).Margin.Left, (obj as FrameworkElement).Margin.Top, (obj as FrameworkElement).Margin.Right, EaseHelper.EaseValue((double)data.ValueStart, (double)data.ValueEnd, percent) ); }
                            }}
              ,{MarginLeft,new GetterSetter { 
                             Getter = (obj, data)=> ((FrameworkElement) obj).Margin.Left,
                             Setter = (obj, data, percent)=>{ ((FrameworkElement) obj).Margin = new Thickness( EaseHelper.EaseValue((double)data.ValueStart, (double)data.ValueEnd, percent), (obj as FrameworkElement).Margin.Top, (obj as FrameworkElement).Margin.Right, (obj as FrameworkElement).Margin.Bottom ); }
                            }}
              ,{MarginRight, new GetterSetter {
                             Getter = (obj, data)=> ((FrameworkElement) obj).Margin.Right,
                             Setter = (obj, data, percent)=>{ ((FrameworkElement) obj).Margin = new Thickness(((FrameworkElement) obj).Margin.Left, (obj as FrameworkElement).Margin.Top, EaseHelper.EaseValue((double)data.ValueStart, (double)data.ValueEnd, percent), (obj as FrameworkElement).Margin.Bottom ); }
                            }},
               #endregion

               #region COLOR
              {ColorObj, new GetterSetter
              {
                     Getter = (obj, data)=> obj,Setter = (obj, data, percent)=>
                    {
                        var c = EaseHelper.EaseValue((Color)data.ValueStart, (Color)data.ValueEnd, percent );
                        var o = (Color)obj;
                        o.A = c.A;
                        o.B = c.B;
                        o.R = c.R;
                        o.G = c.G;
                    }
                }},  
                #endregion
        };

        /// <summary>
        /// Type -> GetterSetter Lookup
        /// </summary>
        public static readonly Dictionary<Type, GetterSetter> GetterSettersByType = new Dictionary<Type, GetterSetter>
        {
                #region NUMBERS

                {   typeof(Double), 
                    new GetterSetter
                    {
                        Getter = GetDependencyPropValue,
                        Setter = (obj, data, percent)=> ((DependencyObject)obj).SetValue((DependencyProperty)data.Prop, EaseHelper.EaseValue((double) data.ValueStart, (double) data.ValueEnd,percent))
                    }
                },

                {   typeof(int), 
                    new GetterSetter
                    {
                        Getter = GetDependencyPropValue,
                        Setter = (obj, data, percent)=> ((DependencyObject)obj).SetValue((DependencyProperty)data.Prop, EaseHelper.EaseValue((int) data.ValueStart, (int) data.ValueEnd,percent))
                    }
                },

                {   typeof(decimal), 
                    new GetterSetter
                    {
                        Getter = GetDependencyPropValue,
                        Setter = (obj, data, percent)=> ((DependencyObject)obj).SetValue((DependencyProperty)data.Prop, EaseHelper.EaseValue((decimal) data.ValueStart, (decimal) data.ValueEnd,percent))
                    }
                },
                #endregion

                #region POINTS
                {   typeof(Point), 
                    new GetterSetter
                    {
                        Getter = GetDependencyPropValue,
                        Setter = (obj, data, percent)=> ((DependencyObject)obj).SetValue((DependencyProperty)data.Prop, EaseHelper.EaseValue((Point) data.ValueStart, (Point) data.ValueEnd,percent))
                    }
                },

                {   typeof(Rect), 
                    new GetterSetter
                    {
                        Getter = GetDependencyPropValue,
                        Setter = (obj, data, percent)=> ((DependencyObject)obj).SetValue((DependencyProperty)data.Prop, EaseHelper.EaseValue((Rect) data.ValueStart, (Rect) data.ValueEnd,percent))
                    }
                },

                {   typeof(Matrix), 
                    new GetterSetter
                    {
                        Getter = GetDependencyPropValue,
                        Setter = (obj, data, percent)=> ((DependencyObject)obj).SetValue((DependencyProperty)data.Prop, EaseHelper.EaseValue((Matrix) data.ValueStart, (Matrix) data.ValueEnd,percent))
                    }
                },

                {   typeof(Matrix3D), 
                    new GetterSetter
                    {
                        Getter = GetDependencyPropValue,
                        Setter = (obj, data, percent)=> ((DependencyObject)obj).SetValue((DependencyProperty)data.Prop, EaseHelper.EaseValue((Matrix3D) data.ValueStart, (Matrix3D) data.ValueEnd,percent))
                    }
                },
                #endregion

                #region BRUSHES
                {   typeof(SolidColorBrush), 
                    new GetterSetter
                    {
                        Getter = GetDependencyPropValue,
                        Setter = (obj, data, percent)=> ((DependencyObject)obj).SetValue((DependencyProperty)data.Prop, EaseHelper.EaseValue((SolidColorBrush) data.ValueStart, (SolidColorBrush) data.ValueEnd,percent))
                    }
                },

                {   typeof(LinearGradientBrush), 
                    new GetterSetter
                    {
                        Getter = GetDependencyPropValue,
                        Setter = (obj, data, percent)=> ((DependencyObject)obj).SetValue((DependencyProperty)data.Prop, EaseHelper.EaseValue((LinearGradientBrush) data.ValueStart, (LinearGradientBrush) data.ValueEnd,percent))
                    }
                },

                {   typeof(RadialGradientBrush), 
                    new GetterSetter
                    {
                        Getter = GetDependencyPropValue,
                        Setter = (obj, data, percent)=> ((DependencyObject)obj).SetValue((DependencyProperty)data.Prop, EaseHelper.EaseValue((RadialGradientBrush) data.ValueStart, (RadialGradientBrush) data.ValueEnd,percent))
                    }
                },
                #endregion

                #region BORDERS
                
                {   typeof(CornerRadius), 
                    new GetterSetter
                    {
                        Getter = GetDependencyPropValue,
                        Setter = (obj, data, percent)=> ((DependencyObject)obj).SetValue((DependencyProperty)data.Prop, EaseHelper.EaseValue((CornerRadius) data.ValueStart, (CornerRadius) data.ValueEnd,percent))
                    }
                },

                #endregion

                #region THICKNESS / LENGTH
                
                {   typeof(Thickness), 
                    new GetterSetter
                    {
                        Getter = GetDependencyPropValue,
                        Setter = (obj, data, percent)=> ((DependencyObject)obj).SetValue((DependencyProperty)data.Prop, EaseHelper.EaseValue((Thickness) data.ValueStart, (Thickness) data.ValueEnd,percent))
                    }
                },

                {   typeof(GridLength), 
                    new GetterSetter
                    {
                        Getter = GetDependencyPropValue,
                        Setter = (obj, data, percent)=> ((DependencyObject)obj).SetValue((DependencyProperty)data.Prop, EaseHelper.EaseValue((GridLength) data.ValueStart, (GridLength) data.ValueEnd,percent))
                    }
                },
                #endregion    

                #region EFFECTS
                 {  typeof(BlurEffect), 
                    new GetterSetter
                    {
                        Getter = GetDependencyPropValue,
                        Setter = (obj, data, percent)=> ((DependencyObject)obj).SetValue((DependencyProperty)data.Prop, EaseHelper.EaseValue((BlurEffect) data.ValueStart, (BlurEffect) data.ValueEnd,percent))
                    }
                },

                 {  typeof(DropShadowEffect), 
                    new GetterSetter
                    {
                        Getter = GetDependencyPropValue,
                        Setter = (obj, data, percent)=> ((DependencyObject)obj).SetValue((DependencyProperty)data.Prop, EaseHelper.EaseValue((DropShadowEffect) data.ValueStart, (DropShadowEffect) data.ValueEnd,percent))
                    }
                },
                #endregion

                #region COLOR
                 {  typeof(Color), 
                    new GetterSetter
                    {
                        Getter = GetDependencyPropValue,
                        Setter = (obj, data, percent)=> ((DependencyObject)obj).SetValue((DependencyProperty)data.Prop, EaseHelper.EaseValue((Color) data.ValueStart, (Color) data.ValueEnd,percent))
                    }
                },

                #endregion
        };
        
        /// <summary>
        /// DependencyProperty -> GetterSetter Lookup
        /// Allows you to pick an alternate getter/setter for any Dependency Property.
        /// </summary>
        public static readonly Dictionary<DependencyProperty, GetterSetter> GetterSettersByDependencyProperty = new Dictionary<DependencyProperty, GetterSetter>
        {
           { FrameworkElement.WidthProperty, PositiveDoubleGetterSetter },
           { FrameworkElement.HeightProperty, PositiveDoubleGetterSetter },
           { FrameworkElement.ActualWidthProperty, PositiveDoubleGetterSetter },
           { FrameworkElement.ActualHeightProperty, PositiveDoubleGetterSetter },
        };

 
        #endregion

        #region GETTER/SETTER REGISTRATION
        /// <summary>
        /// Links DependencyProperty to GetterSetter.
        /// </summary>
        /// <returns>Registration Success. False means the prop is already registered.</returns>
        public static bool RegisterDependencyPropertyForGetterSetter(DependencyProperty prop, GetterSetter getterSetter)
        {
            if (GetterSettersByDependencyProperty.ContainsKey(prop))
            {
                Debug.WriteLine(typeof(AnimationTypes) + "  [WARN] Can't Register Property, key already exists for: " + prop);
                return false;
            }
            GetterSettersByDependencyProperty[prop] = getterSetter;
            return true;
        }

        /// <summary>
        /// Links Type to GetterSetter.
        /// </summary>
        /// <returns>Registration Success. False means the propType is already registered.</returns>
        public static bool RegisterGetterSetterForType(Type propType, GetterSetter getterSetter)
        {
            if (GetterSettersByType.ContainsKey(propType))
            {
                Debug.WriteLine(typeof(AnimationTypes) + "  [WARN] Can't Register Property, key already exists for: " + propType);
                return false;
            }
            GetterSettersByType[propType] = getterSetter;
            return true;
        }
            
        /// <summary>
        /// Link string name to DependencyProperty used in GetterSetters.
        /// </summary>
        /// <returns>Registration Success. False means the propName is already registered.</returns>
        public static bool RegisterShortcut(string propName, object value)
        {
            if (Shortcuts.ContainsKey(propName))
            {
                Debug.WriteLine(typeof(AnimationTypes) + "  [WARN] Can't Register Property, key already exists for: " + propName);
                return false;
            }
            Shortcuts[propName] = value;
            return true;
        }

        /// <summary>
        /// Link GetterSetter to string name
        /// </summary>
        /// <returns>Registration Success. False means the propName is already registered.</returns>
        public static bool RegisterGetterSetter(string propName, GetterSetter value)
        {
            if (GetterSetterHash.ContainsKey(propName))
            {
                Debug.WriteLine(typeof(AnimationTypes) + "  [WARN] Can't Register GetterSetter, key already exists for: " + propName);
                return false;
            }
            GetterSetterHash[propName] = value;
            return true;
        }

        private static readonly Dictionary<string, object> Unknowns = new Dictionary<string, object>();
        #endregion

        #region GETTER / SETTER LOOKUP HELPERS
        public static string GetStringByProperty(DependencyProperty prop)
        {
            var key = Shortcuts.GetKeyFromValue(prop);

            // Silverlight does not contain a public Name property on DependencyProperty, so we'll make a unique look-up.
            #if (SILVERLIGHT)
            if (key == null)
            {
                key = Unknowns.GetKeyFromValue(prop);
                if (key == null)
                {
                    key = Guid.NewGuid().ToString();
                    Unknowns[key] = prop;
                }
            }
            return key;
            #endif

            #if (!SILVERLIGHT)
            return key ?? prop.ToString();
            #endif
        }

        /// <summary>
        /// Returns DependencyProperty as registered by the string name in Shortcuts or from the Unknowns list if not found.
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static DependencyProperty GetPropertyByString(string propName)
        {
            object prop;
            Shortcuts.TryGetValue(propName, out prop);
            if (prop == null) Unknowns.TryGetValue(propName, out prop);
            return prop as DependencyProperty;
        }
        #endregion
    }
}