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
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Collections;

#if !SILVERLIGHT
using System.Windows.Media.Media3D;
#endif

namespace Artefact.Animation
{
    /// <summary>
    /// Core class to construct and manage EaseObjects.
    /// </summary>
    public static class ArtefactAnimator
    {
        #region INFO - PUBLIC
        /// <summary>
        /// Original Creator of ArtefactAnimator.
        /// </summary>
        public const string Author = "Jesse Graupmann";
        /// <summary>
        /// Artefact
        /// </summary>
        public const string Link = "www.artefactgroup.com";
        /// <summary>
        /// Current version
        /// </summary>
        public const string Version = "2.0.4.3";
        /// <summary>
        /// Last month modified
        /// </summary>
        public const string Modified = "4/2010";
        #endregion

        #region PROPS - PUBLIC
        /// <summary>
        /// Event dispatched from each trigger of the CompositionTarget.Rendering.
        /// </summary>
        public static event VoidHandler Tick;

        /// <summary>
        /// Notification to all EaseObjects to stop easing values listed in the StopPropsEventHandler.
        /// </summary>
        public static event StopPropsEventHandler StopProps;

        /// <summary>
        /// Total number of Milliseconds ArtefactAnimator has been active - used for evaluating time percentage.
        /// </summary>
        public static double ElapsedMilliseconds
        {
            get { return Stopwatch.ElapsedMilliseconds; }
        }

        /// <summary>
        /// The DataTime ArtefactAnimator was started.
        /// </summary>
        public static DateTime StartTime
        {
            get { return Stopwatch.StartTime.Date; }
        }
        #endregion

        #region PROPS - PRIVATE
        private static int _i;
        private static readonly Stopwatch Stopwatch;
        #endregion

        #region CONSTRUCTOR
        static ArtefactAnimator()
        {
            #if SILVERLIGHT
            Debug.WriteLine("SILVERLIGHT VERSION: " + Version, typeof(ArtefactAnimator).ToString());
            #endif

            #if !SILVERLIGHT
            Debug.WriteLine("WPF VERSION: " + Version, typeof(ArtefactAnimator).ToString());
            #endif

            Stopwatch = new Stopwatch().Start();
            Stopwatch.Update += _Tick;
        }
        #endregion

        #region STOPPING EASE
        public static void StopEase(DependencyObject obj, params object[] props)
        {
            try
            {
                if (StopProps != null) StopProps(obj, GetNames(props));
            }
            catch (Exception error)
            {
                Debug.WriteLine(string.Format("StopEase({0}, {1}): {2}", obj, props, error), typeof(ArtefactAnimator).ToString());
            }
        }
        #endregion

        #region ADD EASE

        #region DEFAULT
        public static EaseObject AddEase(object obj, object props, object endValues, double time, PercentHandler ease, double delay)
        {
            // throw exception here?
            if (obj == null) return null;

            try
            {
                // construct ease object
                return new EaseObject(time, ease, delay)
                {
                    Target = obj,
                    Props = ValidatePropsAndValue(obj, props, endValues)
                }.Start();
            }
            catch (Exception error)
            {
                Debug.WriteLine(string.Format("[ERROR] - {1} - AddEase({0})", obj, error), typeof(ArtefactAnimator).ToString());
            }

            // return dummy EaseObject if failed to construct
            return new EaseObject(time, ease, delay);
        }
        #endregion

        #region OVERLOADS - AddEase
        public static EaseObject AddEase(object obj, object props, object endValues, double time, PercentHandler ease)
        {
            return AddEase(obj, props, endValues, time, ease, 0);
        }

        public static EaseObject AddEase(object obj, object props, object endValues, double time)
        {
            return AddEase(obj, props, endValues, time, null, 0);
        }

        public static EaseObject AddEase(object obj, object props, object endValues)
        {
            return AddEase(obj, props, endValues, 0, null, 0);
        }
        #endregion

        #region CUSTOM GETTER / SETTERS
        /**
        public static EaseObject AddEase(object obj, object props, object endValues, GetterSetter gettersGetters, double time, PercentHandler ease, double delay)
        {
            throw new NotImplementedException();
            return new EaseObject(time, ease, delay);
        }
        #region OVERLOADS
        #endregion
        */

        #endregion

        #endregion

        #region VALIDATION - GETTER SETTER DATA
        public static GetterSetterData ValidatePropAndValue(object obj, object prop, object endValue)
        {
            GetterSetterData g = null;
            GetterSetter gs = null;
            
            if (prop as DependencyProperty != null)
            {
                 
                if (AnimationTypes.GetterSettersByDependencyProperty.ContainsKey(prop as DependencyProperty))
                {
                    gs = AnimationTypes.GetterSettersByDependencyProperty[prop as DependencyProperty];
                }
                else
                {
                    try
                    {
                        var valueType = endValue.GetType();
                        if (AnimationTypes.GetterSettersByType.ContainsKey(valueType))
                        {
                            gs = AnimationTypes.GetterSettersByType[valueType];
                        }
                    }
                    catch (Exception error)
                    {
                        Debug.WriteLine("[ERROR] - " + error);
                        return null;
                    }
                }

                g = new GetterSetterData
                {
                    Name = GetName(prop),
                    Getter = gs != null ? gs.Getter : AnimationTypes.GetDoubleDependencyPropValue,
                    Setter = gs != null ? gs.Setter : AnimationTypes.SetDoubleDependencyPropValue,
                    Prop = prop,
                    ValueStart = endValue is int ? (double)((int)endValue) : endValue,
                };
                g.ValueEnd = g.ValueStart;
            }
            else if (prop as string != null)
            {
                var propStr = prop as string;
                if (AnimationTypes.Shortcuts.ContainsKey(propStr))
                {
                    // SHORTCUT
                    if (AnimationTypes.Shortcuts[propStr] as DependencyProperty != null)
                    {
                        var dp = AnimationTypes.Shortcuts[propStr] as DependencyProperty;
                        if (dp != null)
                            if (AnimationTypes.GetterSettersByDependencyProperty.ContainsKey(dp)) gs = AnimationTypes.GetterSettersByDependencyProperty[dp];
                    }

                    g = new GetterSetterData
                    {
                        Name = propStr,
                        Getter = gs != null ? gs.Getter : AnimationTypes.GetDoubleDependencyPropValue,
                        Setter = gs != null ? gs.Setter : AnimationTypes.SetDoubleDependencyPropValue,
                        Prop = AnimationTypes.Shortcuts[propStr],
                        ValueStart = endValue is int ? (double)((int)endValue) : endValue
                    };
                    g.ValueEnd = g.ValueStart;
                }
                else if (AnimationTypes.GetterSetterHash.ContainsKey(propStr))
                {
                    // GETTER / SETTER
                    g = new GetterSetterData
                    {
                        Name = propStr,
                        Getter = AnimationTypes.GetterSetterHash[propStr].Getter,
                        Setter = AnimationTypes.GetterSetterHash[propStr].Setter,
                        Prop = propStr,
                        ValueStart = endValue is int ? (double)((int)endValue) : endValue
                    };
                    g.ValueEnd = g.ValueStart;
                }
                else
                {
                    // SIMPLE DEPENDENCY PROPERTY - FROM STRING
                    if (AnimationTypes.Shortcuts.ContainsKey(propStr) && AnimationTypes.Shortcuts[propStr] as DependencyProperty != null)
                    {
                        var dp = AnimationTypes.Shortcuts[propStr] as DependencyProperty;
                        if (dp != null)
                        {
                            if (AnimationTypes.GetterSettersByDependencyProperty.ContainsKey(dp))
                            {
                                gs = AnimationTypes.GetterSettersByDependencyProperty[dp];
                                g = new GetterSetterData
                                {
                                    Name = GetName(prop),
                                    Getter = gs != null ? gs.Getter : AnimationTypes.GetDoubleDependencyPropValue,
                                    Setter = gs != null ? gs.Setter : AnimationTypes.SetDoubleDependencyPropValue,
                                    Prop = dp,
                                    ValueStart = endValue is int ? (double)((int)endValue) : endValue
                                };
                                g.ValueEnd = g.ValueStart;
                                return g;
                            }
                        }
                    }
                     
                    if (gs == null)
                    {
                        throw new Exception("String \"" + prop + "\" is not registered with ArtefactAnimator and therefore cannot be animated. EndValue=" + endValue);
                    } 
                }
            }
            return g;
        }
        public static object[] ConvertToObjectArray(object obj)
        {
            if( obj as object[] != null) return (object[])obj;
            List<object> list = new List<object>();
            var enumList = (IEnumerable)obj;
            
            foreach ( var item in enumList)
            {
                list.Add (item);
            }
            return list.ToArray();
        }

        public static Dictionary<string, GetterSetterData> ValidatePropsAndValue(object obj, object prop, object endValue)
        {
            var vList = new Dictionary<string, GetterSetterData>();
            GetterSetterData g = null;

            if (prop is Array)
            {
                // must be in object[] form
                var props = (prop is object[]) ? (object[])prop : ConvertToObjectArray(prop);
                var endValues = (endValue is object[]) ? (object[])endValue : ConvertToObjectArray(endValue);

                for (_i = 0; _i < props.Length; _i++)
                {
                    g = ValidatePropAndValue(obj, props[_i], endValues[_i]);
                    if (g != null) vList[g.Name] = g;
                }
            }
            else
            {
                g = ValidatePropAndValue(obj, prop, endValue);
                if (g != null) vList[g.Name] = g;
            } 

            return vList;
        } 
        #endregion

        #region SILVERLIGHT HELPERS
        private static string GetName(object prop)
        {
            // LOOK THROUGH KEYS
            var name = AnimationTypes.GetStringByProperty(prop as DependencyProperty);
            if (name != null) return name;

            // string
            name = prop as string;
            if (name != null) if (AnimationTypes.Shortcuts.ContainsKey(name)) return name;

            // fail
            return name;
        }

        /// <summary>
        /// Converts any objects in object[] to string by finding their registered name and keeps string values the same.
        /// </summary>
        /// <param name="props">An Array to get the values from.</param>
        /// <returns>All objects in object[] are returned as strings in a string[]</returns>
        public static string[] GetNames(object[] props)
        {
            if (props == null) return null;

            var nameList = new List<string>();
            foreach (var prop in props)
            {
                if (prop is String)
                {
                    nameList.Add(prop as string);
                }
                else
                {
                    var name = GetName(prop);
                    if (name != null) nameList.Add(name);
                }
            }
            return nameList.ToArray();
        }
        #endregion

        #region  SHORTCUT - HELPER
        /// <summary>
        /// Returns a casted Double from the current effective value of a dependency property from a System.Windows.DependencyObject.
        /// </summary>
        /// <param name="obj">The System.Windows.DependencyObject to retrieve the value from</param>
        /// <param name="prop">The System.Windows.DependencyProperty identifier of the property to retrieve the value for</param>
        /// <returns>Returns the current effective value.</returns>
        public static double GetDoubleDependencyProperty(this DependencyObject obj, DependencyProperty prop)
        {
            return (double)obj.GetValue(prop);
        }

        /// <summary>
        /// Sets the local value of a dependency property on a System.Windows.DependencyObject.
        /// </summary>
        /// <param name="obj">The System.Windows.DependencyObject to set the value on.</param>
        /// <param name="prop">The identifier of the dependency property to set.</param>
        /// <param name="value">The new local value.</param>
        public static void SetDependencyProperty(this DependencyObject obj, DependencyProperty prop, object value)
        {
            obj.SetValue(prop, value);
        }
        #endregion

        #region EXTENSIONS
        /// <summary>
        /// Returns the key of a Dictionary(string, object) by passing the value, or null if the key can't be found.
        /// </summary>
        /// <param name="hash">Dictionary(string, object) to search.</param>
        /// <param name="value">object value to search for.</param>
        /// <returns></returns>
        public static string GetKeyFromValue(this Dictionary<string, object> hash, object value)
        {
            return value == null ? null : (from data in hash where data.Value == value select data.Key).FirstOrDefault();
        }
        #endregion

        #region TICK
        private static void _Tick(Stopwatch stopwatch)
        {
            try
            {
                if (Tick != null) Tick();
            }
            catch (Exception error)
            {
                Debug.WriteLine("_Tick error:" + error, typeof(ArtefactAnimator).ToString());
            }
        }
        #endregion

        #region UTILS

        /// <summary>
        /// Get FrameworkElement.RenderTransform with Children in standard order [ ScaleTransform, SkewTransform, RotateTransform, TranslateTransform, ... ]
        /// </summary>
        public static TransformGroup NormalizeTransformGroup(FrameworkElement obj)
        {
            /*
                rect.RenderTransformOrigin = new Point(.5, .5);
                rect.RenderTransform = ArtefactAnimator.NormalizeTransformGroup(rect);

                ArtefactAnimator.AddEase(rect, new string[] { "X", "Y", "alpha" }, new double[] { 300, 300, .1 }, 5.0, ArtefactAnimator.WrapPennerEquation(PennerEquations.Equations.CubicEaseOut), 0.2);
                ArtefactAnimator.AddEase((rect.RenderTransform as TransformGroup).Children[TransformGroupIndexes.ScaleTransform], new string[] { "scalX", "scaleY" }, new double[] { 2, 2 }, 5.0, ArtefactAnimator.WrapPennerEquation(PennerEquations.Equations.CubicEaseOut), 0.2);
                ArtefactAnimator.AddEase((rect.RenderTransform as TransformGroup).Children[TransformGroupIndexes.SkewTransform], new string[] { "skewY" }, new double[] { -8.0 }, 5.0, ArtefactAnimator.WrapPennerEquation(PennerEquations.Equations.CubicEaseOut), 0.2);
                ArtefactAnimator.AddEase((rect.RenderTransform as TransformGroup).Children[TransformGroupIndexes.TranslateTransform], new string[] { "rotation" }, new double[] { 720 }, 5.0, ArtefactAnimator.WrapPennerEquation(PennerEquations.Equations.CubicEaseOut), 0.2);
            */

            var orgGroup = obj.RenderTransform as TransformGroup;
            var group = new TransformGroup();
            ScaleTransform scale = null;
            SkewTransform skew = null;
            RotateTransform rotate = null;
            TranslateTransform trans = null;

            if (orgGroup != null)
            {
                foreach (var child in orgGroup.Children)
                {
                    // add any existing transforms
                    if (child is ScaleTransform) scale = child as ScaleTransform;
                    else if (child is SkewTransform) skew = child as SkewTransform;
                    else if (child is RotateTransform) rotate = child as RotateTransform;
                    else if (child is TranslateTransform) trans = child as TranslateTransform;
                    else group.Children.Add(child);
                }
            }

            //  create missing values
            if (scale == null) scale = new ScaleTransform();
            if (skew == null) skew = new SkewTransform();
            if (rotate == null) rotate = new RotateTransform();
            if (trans == null) trans = new TranslateTransform();

            // set order close to Blend defaults
            group.Children.Insert((int)TransformGroupIndexes.ScaleTransform, scale);
            group.Children.Insert((int)TransformGroupIndexes.SkewTransform, skew);
            group.Children.Insert((int)TransformGroupIndexes.RotateTransform, rotate);
            group.Children.Insert((int)TransformGroupIndexes.TranslateTransform, trans);

            return group;
        }

        #if !SILVERLIGHT
        /// <summary>
        /// Get Model3D.Transform with Children in standard order [ TranslateTransform3D, ScaleTransform3D, RotateTransform3D, ... ]
        /// </summary>
        public static Transform3DGroup NormalizeTransform3DGroup(Transform3DGroup orgGroup)
        {
            var group = new Transform3DGroup();

            TranslateTransform3D trans = null;
            ScaleTransform3D scale = null;
            RotateTransform3D rotate = null;

            if (orgGroup != null)
            {
                foreach (var child in orgGroup.Children)
                {
                    // add any existing transforms | duplicates are removed

                    if (child is TranslateTransform3D) trans = child as TranslateTransform3D;
                    else if (child is ScaleTransform3D) scale = child as ScaleTransform3D;
                    else if (child is RotateTransform3D) rotate = child as RotateTransform3D;

                    //  store extra transforms

                    else group.Children.Add(child);
                }
            }

            //  create missing values
            if (trans == null) trans = new TranslateTransform3D(0, 0, 0);
            if (scale == null) scale = new ScaleTransform3D(1, 1, 1);
            if (rotate == null) rotate = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0));


            // set order close to Blend defaults
            group.Children.Insert((int)Transform3DGroupIndexes.TranslateTransform3D, trans);
            group.Children.Insert((int)Transform3DGroupIndexes.ScaleTransform3D, scale);
            group.Children.Insert((int)Transform3DGroupIndexes.RotateTransform3D, rotate);

            return group;
        }
        #endif

        #endregion
    }
}