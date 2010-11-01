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
using System.Windows.Media;
using System.Windows.Media.Animation;

using System.Windows;
using System.Windows.Shapes;
using System.Drawing;
using System.Timers;
using System.IO;
using System.Diagnostics;
using TouchFramework.Containers;

namespace TouchFramework
{
    /// <summary>
    /// Wraps any FrameworkElement object with a controlling interface which stores touch information and 
    /// processes actions based on the touches present.
    /// </summary>
    public class Ticker : IDisposable
    {
        object sync = new object();
        Timer timer;

        delegate void InvokeDelegate();

        MTElementDictionary eles;

        public Ticker(ref MTElementDictionary elements)
        {
            eles = elements;
            this.timer = new Timer();
            this.timer.Interval = 3;
            this.timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            this.timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (eles.SyncRoot)
            {
                try {

                    foreach (MTContainer cont in eles.Values.ToArray())
                    {
                        cont.Tick();
                    }
                }
                catch (Exception) // catches without assigning to a variable
                {
                    
                }



            }
        }

        #region IDisposable Members

        protected bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Cleanup();
                }
                disposed = true;
            }
        }

        private void Cleanup()
        {
            timer.Stop();
            timer.Close();
        }

        #endregion

        ~Ticker()
        {
            Dispose(false);
        }
    }
}

