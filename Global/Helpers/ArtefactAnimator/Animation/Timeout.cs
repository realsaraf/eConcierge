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
using System.Diagnostics;

namespace Artefact.Animation
{
    public static class TimeoutExtensions
    {
        public static Timeout OnComplete(this Timeout watch, TimeoutHandler handler)
        {
            watch.Complete += handler;
            return watch;
        }

        public static Timeout OnUpdate(this Timeout watch, StopwatchHandler handler)
        {
            watch.Update += handler;
            return watch;
        }

        public static Timeout OnBegin(this Timeout watch, StopwatchHandler handler)
        {
            watch.Begin += handler;
            return watch;
        }

        public static Timeout OnStopped(this Timeout watch, StopwatchHandler handler)
        {
            watch.Stopped += handler;
            return watch;
        }

        public static Timeout OnPaused(this Timeout watch, StopwatchHandler handler)
        {
            watch.Paused += handler;
            return watch;
        }

        public static Timeout OnResumed(this Timeout watch, StopwatchHandler handler)
        {
            watch.Resumed += handler;
            return watch;
        }
    }

    public class Timeout : Stopwatch
    {
        public event TimeoutHandler Complete;

        /// <summary>
        /// Duration to wait for event trigger in milliseconds
        /// </summary>
        public double Time
        {
            get;
            set;
        }

        
        // ________________________________________________________  CONSTRUCTION

        /// <summary>
        /// Triggers Complete event after Time has elapsed. 
        /// </summary>
        /// <param name="time">Time in Milliseconds</param>
        public Timeout(double milliseconds)
        {
            Time = milliseconds;
            Tick = _Tick;
        }

        /// <summary>
        /// Triggers Complete event after Time has elapsed. 
        /// </summary>
        /// <param name="time">Time in Milliseconds</param>
        /// <param name="autoStart">If true, Start() is called in constructor</param>
        public Timeout(double milliseconds, bool autoStart)
        {
            Time = milliseconds;
            Tick = _Tick;
            if (autoStart) Start();
        }

        // ________________________________________________________  EVENTS


        /// <summary>
        /// Tick to handle timer event. Calculates elapsed time and determines if Timeout is complete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal new void _Tick(object sender, EventArgs e)
        {
            try
            {
                ElapsedMilliseconds = (DateTime.Now - StartTime).TotalMilliseconds;
                OnUpdate(this);
                if (ElapsedMilliseconds > Time)
                {
                    Finish();
                    if (Complete != null) Complete(this);
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine("_Tick error:" + error, typeof(Timeout).ToString());
            }
        }
    }
}
