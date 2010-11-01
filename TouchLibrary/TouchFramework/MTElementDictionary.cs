/*
TouchFramework connects touch tracking from a tracking engine to WPF controls 
allow scaling, rotation, movement and other multi-touch behaviours.

Copyright 2009 - Mindstorm Limited (reg. 05071596)

Author - Simon Lerpiniere

This file is part of TouchFramework.

TouchFramework is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

TouchFramework is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser Public License for more details.

You should have received a copy of the GNU Lesser Public License
along with TouchFramework.  If not, see <http://www.gnu.org/licenses/>.

If you have any questions regarding this library, or would like to purchase 
a commercial licence, please contact Mindstorm via www.mindstorm.com.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

using System.Windows;
using System.Windows.Media;
using System.Drawing;
using Infrasturcture.TouchLibrary;
using TouchFramework.Containers;

namespace TouchFramework
{
    /// <summary>
    /// Stores the relationship between Elements and touches from an element point of view.
    /// Allows you to find out which touches are related to a specific element.
    /// </summary>
    public class MTElementDictionary : Dictionary<int, MTContainer>
    {
        public readonly FrameworkElement Parent;
        public int MaxZIndex = 0;

        public object SyncRoot = new object();

        /// <summary>
        /// Constructor for MTElementDictionary.
        /// </summary>
        /// <param name="uiParent">The parent object of this MTElementDictionary.  This is usually your canvas or window.</param>
        public MTElementDictionary(FrameworkElement uiParent)
        {
            this.Parent = uiParent;
        }

        /// <summary>
        /// Adds a container to this dictionary.
        /// </summary>
        /// <param name="toAdd">The fully initialised container.</param>
        public void Add(MTContainer toAdd)
        {
            lock (SyncRoot)
            {
                if (!this.Keys.Contains(toAdd.GetHashCode()))
                {
                    this.Add(toAdd.GetHashCode(), toAdd);
                    MaxZIndex++;
                    Panel.SetZIndex(toAdd.WorkingObject, MaxZIndex);
                }
            }
        }


        /// <summary>
        /// Gets an element container based on the passed FrameworkElement
        /// </summary>
        /// <param name="referenceElement">The framework element you want the container for.</param>
        /// <returns>Container for the framework element passed or null if not found.</returns>
        public MTContainer this[FrameworkElement referenceElement]
        {
            get
            {
                if (referenceElement == null) throw new ArgumentNullException("referenceElement must not be null");
                if (!this.ContainsKey(referenceElement.GetHashCode())) return null;
                return this[referenceElement.GetHashCode()];
            }
        }

        internal MTContainer HitTest(PointF point)
        {
            return HitTest(point, false);
        }
        internal MTContainer HitTest(PointF point, bool isNew)
        {
            DependencyObject e = null;
            HitTestResult r = VisualTreeHelper.HitTest(this.Parent, new System.Windows.Point(point.X, point.Y));
            if (r != null) e = r.VisualHit as DependencyObject;
            if (e == null)
            {
                return null;
            }

            int hashCode = searchForHash(e);

            MTContainer cont = null;

            if (hashCode != 0)
            {
                cont = this[hashCode];

                if (isNew && cont.Supports(TouchAction.SelectToFront))
                {
                    MaxZIndex++;
                    Panel.SetZIndex(cont.WorkingObject, MaxZIndex);
                }
            }
            
            return cont;
        }

        /// <summary>
        /// Tells all objects in this dictionary to act on their touch data.
        /// I.e. moving/resizing/scaling or whatever based on their implemented logic.
        /// </summary>
        public void Action()
        {
            foreach (int key in this.Keys.ToArray())
            {
                if (this.ContainsKey(key))
                {
                    var mtContainer = this[key];
                    
                    mtContainer.ActOnTouches();
                }
            }
        }

        int searchForHash(DependencyObject e)
        {
            if (e == null) return 0;
            int hashCode = e.GetHashCode();
            if (!this.ContainsKey(hashCode))
            {
                hashCode = 0;
                DependencyObject p = VisualTreeHelper.GetParent(e);
                if (p != null && !(p is Window))
                {
                    hashCode = searchForHash(p);
                }
            }
            return hashCode;
        }
    }
}
