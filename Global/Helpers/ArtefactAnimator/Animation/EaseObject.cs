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



//#define debug

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Artefact.Animation
{
    public class EaseObject : IEaseObject
    {
        #region EASE STATES
        [Flags]
        public enum EaseStates
        {
            IsRunning = 0x01,
            IsFirstRun = 0x02,
            IsDelayed = 0x04,
            All = IsRunning | IsFirstRun | IsDelayed
        }
        #endregion

        #region EASE OBJECT COUNTS
        public static int EaseObjectCount { get; private set; }
        public static int EaseObjectRunningCount { get; private set; }
        public readonly int EaseObjectIndex;
        #endregion

        #region EVENTS

        /// <summary>
        /// Ease has completed without being stopped
        /// </summary>
        public event EaseObjectHandler Complete;

        /// <summary>
        /// Ease percentages have beeen updated
        /// </summary>
        public event EaseObjectHandler Update;

        /// <summary>
        /// Ease has started. Usefull if using a delay.
        /// </summary>
        public event EaseObjectHandler Begin;

        /// <summary>
        /// Ease has been stop using Stop();
        /// </summary>
        public event EaseObjectHandler Stopped;

        #endregion

        #region PROPS

        public int ActiveCount { get; private set; }
        private double _timeElapsed;

        /// <summary>
        /// Custom user data to add to EaseObject
        /// </summary>
        public object Data;

        /// <summary>
        /// Duration of wait in milliseconds
        /// </summary>
        public double Delay;

        /// <summary>
        /// PercentHandler take one double value which is determined by the (ellapsed Time/total Time)
        /// </summary>
        public PercentHandler Ease;

        /// <summary>
        /// Object with currently easing properties -> see Props
        /// </summary>
        public object Target;

        /// <summary>
        /// Collection of GetterSetterData. IsActive determines if the property is currently being animated. 
        /// When all props have their IsActive state set to false, the EaseObject will stop.
        /// </summary>
        public Dictionary<string, GetterSetterData> Props;

        /// <summary>
        /// Duration of ease in milliseconds
        /// </summary>
        public double Time;

        /// <summary>
        /// Time in milliseconds taken from ArtefactAnimator.ElapsedMilliseconds to determine elapsed milliseconds
        /// </summary>
        public double TimeStarted { get; private set; }

        /// <summary>
        /// Currently subscribed to ArtefactAnimator and updating each tick
        /// </summary>
        public bool IsRunning
        {
            get { return ((flags & IS_RUNNING) == IS_RUNNING); }
        }

        /// <summary>
        /// Waiting for delay to ellapse before starting
        /// </summary>
        public bool IsDelayed
        {
            get { return ((flags & IS_DELAYED) == IS_DELAYED); }
        }

        /// <summary>
        /// Percentage of time determined from (ellapsed Time/total Time)
        /// </summary>
        public double PercentTime { get; private set; }

        /// <summary>
        /// Value of PercentTime having passed through Ease.
        /// </summary>
        public double PercentEase { get; private set; }

        #endregion

        #region FLAGS

        public const int IS_DELAYED = 0x04;
        public const int IS_FIRSTRUN = 0x02;
        public const int IS_RUNNING = 0x01;
        private int flags = IS_FIRSTRUN;

        #endregion

        #region CONSTRUCTOR
        public EaseObject(double time, PercentHandler ease, double delay)
        {
            // help with reporting
            EaseObjectIndex = ++EaseObjectCount;
            ++EaseObjectRunningCount;

            if (!double.IsNaN(time) && time > 0) Time = time*1000;
            if (!double.IsNaN(delay) && delay > 0) Delay = delay*1000;
            if (delay > 0 && ((flags & IS_DELAYED) != IS_DELAYED)) flags += IS_DELAYED;
            Ease = ease;
        }
        #endregion

        #region DESCRUCTION

        ~EaseObject()
        {
            // help with reporting
            --EaseObjectRunningCount;
        }

        #endregion

        #region IEaseObject Members | START/STOP/FINISH/TICK...

        public EaseObject Start()
        { 
            if ((flags & IS_FIRSTRUN) != IS_FIRSTRUN) flags += IS_FIRSTRUN;
            PercentTime = PercentEase = 0;
            TimeStarted = ArtefactAnimator.ElapsedMilliseconds;
            foreach (GetterSetterData data in Props.Values) data.IsActive = true;

            if ((flags & IS_RUNNING) != IS_RUNNING)
            {
                flags += IS_RUNNING;
                ArtefactAnimator.Tick += Tick;
                ArtefactAnimator.StopProps += ArtefactAnimator_StopProps;
                ++EaseObjectRunningCount;
            }

            return this;
        }


        public EaseObject Stop()
        {
            if ((flags & IS_RUNNING) == IS_RUNNING)
            {
                flags -= IS_RUNNING;
                ArtefactAnimator.Tick -= Tick;
                ArtefactAnimator.StopProps -= ArtefactAnimator_StopProps;
                --EaseObjectRunningCount;

                foreach (GetterSetterData data in Props.Values) if (data.IsActive) UnRegisterDependencyProperty(data.Name);

                if (Stopped != null) Stopped(this, PercentEase);
            }

            return this;
        }

        public EaseObject Finish()
        {
            // complete
            PercentTime = PercentEase = 1;
            // one final update
            ActiveCount = 0;
            foreach (GetterSetterData data in Props.Values)
            {
                if (!data.IsActive) continue;
                ActiveCount++;
                if (!data.IsActive) continue;
                try
                {
                    data.Setter(Target, data, PercentEase);
                }
                catch (Exception error)
                {
                    Debug.WriteLine("[ERROR] - EaseObject failed to apply Setter in Finish() - " + error);
                }
                 
                UnRegisterDependencyProperty(data.Name);
                data.IsActive = false;
            }

            if (ActiveCount > 0)
            {
                if (Update != null) Update(this, PercentEase);
                if (Complete != null) Complete(this, PercentEase);
            }
            return Stop();
        }

        public void Tick()
        {
            if((flags & IS_RUNNING) != IS_RUNNING ||    // Ease object is not running
                Props.Count <= 0)                       // Without properties to ease, there is no way to tell when to stop.
            {
                #if debug
                Debug.WriteLine(string.Format("{0} {1}/{2} - [ERROR] - Not Currently Running or missing props to ease -> stopping esae object now.", this, inx, EaseObjectCount));
                #endif

                Stop();
                return;
            }
             
            // Calculate ellapsed time
            _timeElapsed = ArtefactAnimator.ElapsedMilliseconds - TimeStarted;


            if ((flags & IS_DELAYED) == IS_DELAYED) // Check delay
            {
                if (_timeElapsed < Delay) // wait
                {
                    #if debug
                    Debug.WriteLine(string.Format("{0} - [DEBUG] - Delayed{3} - {1}<{2}.", this, _timeElapsed, Delay, inx));
                    #endif

                    return;
                }

                // continue
                flags -= IS_DELAYED;
                TimeStarted = ArtefactAnimator.ElapsedMilliseconds - (_timeElapsed - Delay);
                _timeElapsed = 0;
            }

            // first time running - set begining values
            if ((flags & IS_FIRSTRUN) == IS_FIRSTRUN)
            {
                flags -= IS_FIRSTRUN;
                ResetBeginingValues();
                if (Begin != null) Begin(this, PercentEase);
            }


            if (Time < _timeElapsed)
            {
                Finish();
                return;
            }

            // update
            PercentTime = (_timeElapsed)/Time;
            PercentEase = Ease == null ? PercentTime : Ease(PercentTime);
            ActiveCount = 0;

            foreach (GetterSetterData data in Props.Values)
            {
                if (!data.IsActive) continue;
                ActiveCount++;
                try
                {
                    data.Setter(Target, data, PercentEase);
                }
                catch(Exception error)
                {
                   data.IsActive = false;
                   ActiveCount--;
                   Debug.WriteLine("[ERROR] - EaseObject failed to apply Setter in Update() - " + GetterSetterData.Describe(data) + "\n" + error); 
                }
            }

            if (ActiveCount > 0)
            {
                if (Update != null) Update(this, PercentEase);
            }
            else
            {
                Stop(); // Nothing happened
            }

        }

        public void StopProps(params string[] propNames)
        {
            if (propNames == null || propNames.Length <= 0) // Remove everything in ease object
                foreach (var propName in Props.Keys) StopProps(propName);
            
            else
                foreach (var propName in propNames) StopProps(propName);
        }
        #endregion

        #region EASE REGISTRATION

        public static readonly Dictionary<object, Dictionary<string, IEaseObject>> ActiveReg =
            new Dictionary<object, Dictionary<string, IEaseObject>>();

        public void UnRegisterDependencyProperty(string propName)
        {
            try
            {
                // if (DEBUG ) Debug.WriteLine("\t UN-Registering for " + inx + "   " + propName);
                if (ActiveReg.ContainsKey(Target))
                {
                    if (ActiveReg[Target].ContainsKey(propName))
                    {
                        if (ActiveReg[Target][propName] == this)
                        {
                            StopProps(propName);              // remove prop from active easing
   
                            ActiveReg[Target].Remove(propName);    // remove link to ease object

                            if (ActiveReg[Target].Count <= 0)      // nothing to ease on this object
                                ActiveReg.Remove(Target);
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine(string.Format("{0} - [WARN] - Can't UnRegisterDependencyProperty({1}) - {2}", this,
                                              propName, error));
            }
        }

        private void ArtefactAnimator_StopProps(object obj, string[] propNames)
        {
            if (Target == obj) StopProps(propNames);
        }

        public void RegisterDependencyProperty(string propName)
        { 
            if (ActiveReg.ContainsKey(Target))
            {
                // has object
                if (ActiveReg[Target].ContainsKey(propName))
                {
                    // has prop - unregister if not this
                    var eo = ActiveReg[Target][propName];
                    if (eo != this)
                    {
                        if (eo != null) eo.StopProps(propName);
                    }
                }

                // set active all this
                ActiveReg[Target][propName] = this;
            }
            else
            {
                // add all needed info
                ActiveReg[Target] = new Dictionary<string, IEaseObject> {{propName, this}};
            }

            // make double sure that easing is active
            // Props[propName].IsActive = true;
            // if (DEBUG ) Debug.WriteLine("\t Registered for " + inx + "   " + Props[propName].IsActive + "    " + propName + "    " + (ActiveReg[obj][propName] == this));
        }

        #endregion

        private void ResetBeginingValues()
        {
            try
            {
                foreach (GetterSetterData data in Props.Values)
                {
                    if (!data.IsActive) continue;
                    try
                    {
                        data.ValueStart = data.Getter(Target, data);
                        RegisterDependencyProperty(data.Name);
                    }
                    catch (Exception error)
                    {
                        data.IsActive = false;
                        Debug.WriteLine(string.Format("{0} - [ERROR] - Failed to ResetBeginingValue. Setting Prop InActive. - {1} - \n{2}", this, GetterSetterData.Describe(data), error));
                    }
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine(string.Format("{0} - [ERROR] - ResetBeginingValues - {1}", this, error));
            }
        }

        public void StopProps(string propName)
        {
            if (Props.ContainsKey(propName))Props[propName].IsActive = false;
        }
    }
}