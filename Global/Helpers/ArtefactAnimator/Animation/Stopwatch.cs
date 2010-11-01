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
using System.Windows.Media;

namespace Artefact.Animation
{
    public static class StopwatchExtensions
    {
        public static Stopwatch OnUpdate(this Stopwatch watch, StopwatchHandler handler)
        {
            watch.Update += handler;
            return watch;
        }

        public static Stopwatch OnBegin(this Stopwatch watch, StopwatchHandler handler)
        {
            watch.Begin += handler;
            return watch;
        }

        public static Stopwatch OnStopped(this Stopwatch watch, StopwatchHandler handler)
        {
            watch.Stopped += handler;
            return watch;
        }

        public static Stopwatch OnPaused(this Stopwatch watch, StopwatchHandler handler)
        {
            watch.Paused += handler;
            return watch;
        }

        public static Stopwatch OnResumed(this Stopwatch watch, StopwatchHandler handler)
        {
            watch.Resumed += handler;
            return watch;
        }
    }

    public class Stopwatch
    {
        internal EventHandler Tick;

        public Stopwatch()
        {
            Tick = _Tick;
        }

        public Stopwatch(bool autoStart)
        {
            Tick = _Tick;
            if (autoStart) Start();
        }

        public double ElapsedMilliseconds { get; internal set; }

        public DateTime StartTime { get; private set; }
        public DateTime PauseTime { get; private set; }

        public bool IsPaused { get; private set; }
        public bool IsRunning { get; private set; }

        public event StopwatchHandler Update;
        public event StopwatchHandler Begin;
        public event StopwatchHandler Stopped;
        public event StopwatchHandler Paused;
        public event StopwatchHandler Resumed;


        public Stopwatch Restart()
        {
            Finish();
            Start();
            return this;
        }

        public Stopwatch Start(bool toRestartIfRunning)
        {
            Finish();
            Start();
            return this;
        }

        public Stopwatch Start()
        {
            if (IsPaused) IsPaused = false;
            if (!IsRunning)
            {
                IsRunning = true;
                StartTime = DateTime.Now;
                CompositionTarget.Rendering += Tick;
                if (Begin != null) Begin(this);
            }
            return this;
        }

        public Stopwatch Pause()
        {
            if (IsRunning && !IsPaused)
            {
                IsPaused = true;
                CompositionTarget.Rendering -= Tick;
                PauseTime = DateTime.Now;
                if (Paused != null) Paused(this);
            }

            return this;
        }

        public Stopwatch Resume()
        {
            if (IsRunning && IsPaused)
            {
                if (PauseTime != null)
                {
                    StartTime.Add(PauseTime.Subtract(DateTime.Now));
                }
                IsPaused = false;
                CompositionTarget.Rendering += Tick;
                if (Resumed != null) Resumed(this);
            }

            return this;
        }

        internal void OnUpdate(Stopwatch sender)
        {
            if (Update != null) Update(sender);
        }

        public Stopwatch Stop()
        {
            if (IsRunning)
            {
                Finish();
                if (Stopped != null) Stopped(this);
            }

            return this;
        }

        public Stopwatch Finish()
        {
            if (IsPaused) IsPaused = false;
            if (IsRunning)
            {
                IsRunning = false;
                CompositionTarget.Rendering -= Tick;
            }

            return this;
        }

        #region TICK
        internal void _Tick(object sender, EventArgs e)
        {
            try
            {
                ElapsedMilliseconds = (DateTime.Now - StartTime).TotalMilliseconds;
                if (Update != null) Update(this);
            }
            catch (Exception error)
            {
                Debug.WriteLine("_Tick error:" + error, typeof(Stopwatch).ToString());
            }
        }
        #endregion
    }
}