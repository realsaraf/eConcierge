/*
TouchFramework connects touch tracking from a tracking engine to WPF controls 
allow scaling, rotation, movement and other multi-touch behaviours.

Copyright 2009 - Mindstorm Limited (reg. 05071596)

Author - Julien Vulliet

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
using System.Runtime.InteropServices;
using System.Drawing;

namespace TouchFramework
{
    /// <summary>
    /// Uses an OS timer to accurately provide linear filtering of a variable from one value to another.
    /// </summary>
    public sealed class LinearFilter
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static extern int GetTickCount();

        #region Fields
        /// <summary>
        /// The volicity for dampening movements is the last positive velocity value from of the past N frames.
        /// </summary>
        const int NUM_FRAMES_VELAVG = 3;
        
        //position inputs (origin and target)
        float origin = 0;
        float target;

        //How long to animate
        int delay = 100;
        
        //time internals
        int currenttime;
        int elapsed;
        bool isfiltering = false;

        //Position and velocity outputs
        float position;
        float velocity;
        float previousposition;
        float previousvelocity;
        float cumulativeposition;
        float[] velocitySet = new float[NUM_FRAMES_VELAVG];
        #endregion

        #region Properties
        public int Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        public float PreviousPosition
        {
            get { return previousposition; }
        }

        public float PreviousVelocity
        {
            get { return previousvelocity; }
        }

        public float Position
        {
            get { return position; }
        }

        public float Velocity
        {
            get { return velocity; }
        }

        public float CumulativePosition
        {
            get { return cumulativeposition; }
        }

        public float LastVelocityFromSet
        {
            get
            {
                int i = 0;
                float val = 0f;
                while (i < velocitySet.Length && val == 0f)
                {
                    val = velocitySet[i];
                    i++;
                }
                return val;
            }
        }

        public bool IsFiltering
        {
            get { return this.isfiltering; }
            set { this.isfiltering = value; }
        }

        public void Stop()
        {
            this.isfiltering = false;
        }
        #endregion

        public void Reset(float origin, float target)
        {
            this.currenttime = GetTickCount(); 
            this.position = origin;
            this.previousposition = origin;
            this.velocity = 0;
            this.previousvelocity = 0;
            this.cumulativeposition = 0;
            this.origin = origin;
            this.target = target;
            elapsed = 0;

            this.isfiltering = true;
        }

        public float Target
        {
            set
            {
                this.currenttime = GetTickCount(); 
                this.origin = position;
                this.previousposition = origin;
                this.velocity = 0;
                this.previousvelocity = 0;
                this.cumulativeposition = 0;
                target = value;
                elapsed = 0;
            }
            get
            {
                return target;
            }
        }

        public void StepIfFiltering()
        {
            if (!this.isfiltering) return;
            this.Step();
        }

        public void Step()
        {
            int newtime = GetTickCount();
            int timestep = newtime - this.currenttime;

            this.previousposition = position;
            this.previousvelocity = velocity;

            if (timestep > 0)
            {
                //Increment elapsed time
                elapsed += timestep;

                if (elapsed >= delay)
                {
                    //If we finished, we make sure to be on target
                    elapsed = delay;
                    this.isfiltering = false;
                    this.position = target;
                    this.velocity = 0;

                    resetVelocitySet();
                }
                else
                {
                    if (this.target != this.origin)
                    {
                        float ratio = ((float)timestep / this.Delay) * (this.target - this.origin);
                        
                        this.position += ratio;
                        this.velocity = ratio;
                        this.cumulativeposition += this.position;

                        velocitySet.Shift();

                        velocitySet[0] = ratio;
                    }
                    else
                    {
                        this.position = this.target;
                        this.velocity = 0;
                        
                        resetVelocitySet();
                    }
                }
            }

            this.currenttime = newtime;
        }

        void resetVelocitySet()
        {
            velocitySet = new float[10];
        }
    }

    /// <summary>
    /// Implements 2 linear filters to provide filtering of a point in x and y.
    /// </summary>
    public class LinearFilter2d
    {
        #region Fields
        LinearFilter filterx = new LinearFilter();
        LinearFilter filtery = new LinearFilter();

        int delay = 200;
        #endregion

        #region Properties
        public int Delay
        {
            get { return delay; }
            set 
            { 
                delay = value;
                filtery.Delay = value;
                filterx.Delay = value;
            }
        }

        public PointF Position
        {
            get { return new PointF(this.filterx.Position,this.filtery.Position); }
        }

        public PointF Velocity
        {
            get { return new PointF(this.filterx.Velocity, this.filtery.Velocity); }
        }

        public PointF CumulativePosition
        {
            get { return new PointF(this.filterx.CumulativePosition, this.filtery.CumulativePosition); }
        }

        public PointF LastVelocityFromSet
        {
            get { return new PointF(this.filterx.LastVelocityFromSet, this.filtery.LastVelocityFromSet); }
        }

        public bool IsFiltering
        {
            get { return filterx.IsFiltering && filtery.IsFiltering; }
            set { filterx.IsFiltering = value; filtery.IsFiltering = value; }
        }
        #endregion

        public void Reset(PointF origin, PointF target)
        {
            this.filterx.Reset(origin.X, target.X);
            this.filtery.Reset(origin.Y, target.Y);
            this.Target = target;
        }

        public PointF Target
        {
            set
            {
                this.filterx.Target = value.X;
                this.filtery.Target = value.Y;
            }
            get
            {
                return new PointF(this.filterx.Target, this.filtery.Target);
            }
        }

        public void StepIfFiltering()
        {
            this.filterx.StepIfFiltering();
            this.filtery.StepIfFiltering();
        }

        public void Step()
        {
            this.filterx.Step();
            this.filtery.Step();
        }

        public void Stop()
        {
            this.filterx.Stop();
            this.filtery.Stop();
        }
    }
}
