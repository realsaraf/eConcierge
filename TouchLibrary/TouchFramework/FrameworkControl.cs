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
using System.Drawing;
using System.Windows;
using System.Threading;
using System.Windows.Threading;
using Infrasturcture.TouchLibrary;
using TouchFramework.Config;
using TouchFramework.Containers;

namespace TouchFramework
{
    /// <summary>
    /// Builds up a dictionary of touches and matches them to elements registered.
    /// </summary>
    /// <remarks>
    /// Each time a touch event happens, call the appropriate function (Added/Updated/...).
    /// If you are using a tracking system that returns data as a dictionary, enumerate the dictionary
    /// and call the required functions for each update.
    /// </remarks>
    public abstract class FrameworkControl
    {
        /// <summary>
        /// The parent UI element which holds all elements this controller will manage.
        /// </summary>
        public FrameworkElement Owner = null;

        /// <summary>
        /// Manages assignment of touches to objects
        /// </summary>
        public TouchElementAssigner Assigner = null;

        /// <summary>
        /// Holds a dictionary of all touches related to this controller.
        /// </summary>
        public TouchDictionary AllTouches = new TouchDictionary();

        public delegate void ProcessUpdatesDelegate();
        
        /// <summary>
        /// Called after the ProcessUpdates function completes.  
        /// Can be used to perform actions based on the current touch information or element final positions.
        /// </summary>
        public ProcessUpdatesDelegate OnProcessUpdates = null;

        delegate void InvokeDelegate();
        object syncRoot = new object();

        protected bool IsConfigured = false;
        int uiThreadId = 0;

        /// <summary>
        /// Configures the required things needed for the control to run.
        /// </summary>
        /// <param name="owner">The UI element holding all the registered elements for this controller.</param>
        /// <param name="uiManagedThreadId">The thread ID of the UI Thread.</param>
        /// <remarks>
        /// UIThreadID is used to make sure that UI functions are always
        /// called on the UI thread.  WPF really doesn't like you doing cross-thread calls to UI
        /// element functions (the HitTest for example).
        /// </remarks>
        public virtual void ConfigureFramework(FrameworkConfiguration config)
        {
            this.Owner = config.Owner;
            uiThreadId = config.UIManagedThreadId;
            Assigner = new TouchElementAssigner(config.Owner);
            IsConfigured = true;
        }

        protected void CheckConfigured()
        {
            if (!IsConfigured) throw new NotConfiguredException();
        }

        /// <summary>
        /// Wires up everything to start tracking and starts the process.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Stops tracking including un-wiring events etc...
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Looks at a new touch and uses the ElementAssigner to match it to registered elements.
        /// Puts the touch into the AllTouches dictionary regardless of whether it matches an item or not.
        /// </summary>
        /// <param name="t">New touch object.</param>
        protected void TouchAdded(Touch t)
        {
            CheckConfigured();

            // This might update the UI, so need to be on UI Thread
            if (Thread.CurrentThread.ManagedThreadId != uiThreadId)
            {
                // this.Owner.Dispatcher.Invoke(new TouchAddDelegate(TouchAdded), new object[] { t });
                this.Owner.Dispatcher.Invoke((InvokeDelegate)delegate() { TouchAdded(t); });
                return;
            }

            // Even though we are on the same thread, we still need to lock 
            // as the AllTouches collection is public.
            lock (syncRoot)
            {
                Assigner.TouchAdded(t);
                AllTouches.Add(t.TouchId, t);
            }
        }

        /// <summary>
        /// Updates a touch position.  Does not change the object the touch is related to by doing a hittest.
        /// </summary>
        /// <remarks>
        /// Touch is fixed to one element when added, and should never change.  This
        /// gives the best responsiveness for resizing and rotating, as the fingers move outside of the
        /// element area.
        /// </remarks>
        /// <param name="touchId">Unique ID for the touch.</param>
        /// <param name="touchPoint">The coordinates of the touch in screen space.</param>
        /// <param name="props">Extended properties of the touch.</param>
        protected void TouchUpdated(int touchId, PointF touchPoint, ITouchProperties props)
        {
            CheckConfigured();

            // Potential to be called from another thread, so need to lock
            lock (syncRoot)
            {
                Assigner.TouchMoved(touchId, touchPoint, props);

                if (AllTouches.ContainsKey(touchId))
                {
                    AllTouches[touchId].TouchPoint = touchPoint;
                }
            }
        }

        /// <summary>
        /// Removes a touch from all touch collections where it is present, including it's association with
        /// any element.
        /// </summary>
        /// <param name="touchId"></param>
        protected void TouchRemoved(int touchId)
        {
            CheckConfigured();

            // Potential to be called from another thread, so need to lock
            lock (syncRoot)
            {
                Assigner.TouchRemoved(touchId);
                AllTouches.Remove(touchId);
            }
        }

        /// <summary>
        /// If an object is still present in the same position, this is called to
        /// mark the object as static.  
        /// </summary>
        /// <remarks>
        /// Static points result in a different center
        /// of action for resize and rotate etc...
        /// </remarks>
        /// <param name="touchId"></param>
        protected void TouchHeld(int touchId)
        {
            CheckConfigured();

            // Potential to be called from another thread, so need to lock
            lock (syncRoot)
            {
                Assigner.TouchHeld(touchId);
            }
        }

        /// <summary>
        /// Tells all elements to act on their touch information.  Whether it be resize, rotate or whatever.
        /// Call this once you have done all your adding/updating/holding/...
        /// </summary>
        protected void ProcessUpdates()
        {
            CheckConfigured();

            // This might update the UI, so need to be on UI Thread
            if (Thread.CurrentThread.ManagedThreadId != uiThreadId)
            {
                this.Owner.Dispatcher.Invoke((InvokeDelegate)delegate() { this.ProcessUpdates(); });
                return;
            }

            // Even though we are on the same thread, we still need to lock 
            // as the AllTouches collection is public.
            lock (syncRoot)
            {
                Assigner.Elements.Action();
                if (OnProcessUpdates != null) OnProcessUpdates();
            }
        }

        /// <summary>
        /// Forces a refresh of tracking points.  Can be used to clear background etc...
        /// </summary>
        public abstract void ForceRefresh();

        /// <summary>
        /// Removes all points from all collections. 
        /// BEWARE: trying to remove the points a 2nd time will cause an exception.  USE WITH CARE!
        /// </summary>
        public virtual void RemoveAllPoints()
        {
            int[] ids = new int[AllTouches.Count];
            AllTouches.Keys.CopyTo(ids, 0);

            for (int i = 0; i < ids.Length; i++)
            {
                this.TouchRemoved(ids[i]);
            }            
        }

        /// <summary>
        /// Gives you the Max ZIndex of all elements registered.
        /// </summary>
        /// <remarks>
        /// As touch events can change elements index this is ideal for placing something on top of everything.
        /// </remarks>
        public int MaxZIndex
        {
            get
            {
                return Assigner.Elements.MaxZIndex;
            }
        }

        /// <summary>
        /// Registers any FrameworkElement with the TouchFramework ready to accept
        /// touch information.
        /// </summary>
        /// <param name="ele">FrameworkElement object to track touches for.</param>
        /// <returns></returns>
        public void RegisterElement(MTContainer cont)
        {
            this.Assigner.Elements.Add(cont);
        }

        /// <summary>
        /// Un-registers any FrameworkElement with the TouchFramework ready to accept
        /// touch information.
        /// </summary>
        /// <param name="ele">FrameworkElement object to track touches for.</param>
        /// <returns></returns>
        public void UnregisterElement(int id)
        {
            var cont = this.Assigner.Elements[id];

            this.Assigner.Touches.Remove(cont);
            this.Assigner.Elements.Remove(id);

            // Dispose the container
            if (cont is IDisposable) ((IDisposable)cont).Dispose();
        }

        /// <summary>
        /// Clears all registered framework elements and disposes the containers
        /// </summary>
        /// <returns></returns>
        public void UnregisterAllElements()
        {
            // Dispose all the containers
            foreach (var cont in this.Assigner.Elements.Values)
            {
                if (cont is IDisposable) ((IDisposable)cont).Dispose();
            }

            this.Assigner.Touches.Clear();
            this.Assigner.Elements.Clear();
        }
    }
}