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
using System.Windows;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using TouchFramework.Config;
using TouchFramework.Tracking.Tuio;
using System.Drawing;

namespace TouchFramework.Tracking
{
    /// <summary>
    /// Tracking implementation for receiving TUIO data and acting upon the events with WPF.
    /// </summary>
    public class TuioTracking : FrameworkControl
    {
        #region Fields
        TuioConfiguration config;
        UdpClient udpreceiver;
        Thread thr;
        bool isrunning;
        Dictionary<int, Tuio2DCursor> current = new Dictionary<int, Tuio2DCursor>();
        object m_lock = new object();
        #endregion

        #region Properties
        public double ScreenWidth = SystemParameters.PrimaryScreenWidth;
        public double ScreenHeight = SystemParameters.PrimaryScreenHeight;
        #endregion

        public TuioTracking(FrameworkElement uiParent)
        {
            this.Owner = uiParent;
        }

        public override void ConfigureFramework(FrameworkConfiguration config)
        {
            if (!(config is TuioConfiguration)) throw new ArgumentException("Config must be TraalConfiguration");

            this.config = config as TuioConfiguration;
                
            base.ConfigureFramework(config);
        }

        public override void Start()
        {
            if (!isrunning)
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, this.config.Port);
                this.udpreceiver = new UdpClient(endpoint);

                this.isrunning = true;
                this.thr = new Thread(new ThreadStart(this.receive));
                this.thr.Start();
            }
        }

        public override void Stop()
        {
            if (isrunning)
            {
                isrunning = false;
                close();
            }
        }

        void close()
        {
            try
            {
                //Might throw an exception which is meaningless when we are shutting down
                this.udpreceiver.Close();
            }
            catch
            {

            }
        }

        public override void ForceRefresh()
        {
            // For tuio this is only useful to remove stuck points after a TUIO server restart
            lock (m_lock)
            {
                this.current.Clear();
            }
        }

        void receive()
        {
            try
            {
                receiveData();
            }
            catch
            {
                // Ignore errors for now
            }
            finally
            {
                // Try to stop cleanly on termination of the blocking receivedata function
                this.Stop();
            }
        }

        void receiveData()
        {
            while (isrunning)
            {
                IPEndPoint ip = null;

                byte[] buffer = this.udpreceiver.Receive(ref ip);
                OSCBundle bundle = OSCPacket.Unpack(buffer) as OSCBundle;

                if (bundle != null)
                {
                    //Not currently checked, we probably should!
                    //int fseq = TuioParser.GetSequenceNumber(bundle);
                    
                    List<int> alivecursors = TuioParser.GetAliveCursors(bundle);
                    Dictionary<int, Tuio2DCursor> newcursors = TuioParser.GetCursors(bundle);
                    
                    // Remove the deleted ones
                    removeNotAlive(alivecursors);

                    //Process held/updated items
                    updateSetCursors(newcursors, alivecursors);

                    //Process new items
                    addNewCursors(newcursors);

                    this.ProcessUpdates();
                }
            }
        }

        void addNewCursors(Dictionary<int, Tuio2DCursor> sets)
        {
            // Get all the cursors we've not got
            var result = (from entry in sets
                          where (!this.current.ContainsKey(entry.Key))
                          select entry.Value);

            // Add them
            foreach (Tuio2DCursor cur in result)
                this.TouchAdded(cur);
        }

        void updateSetCursors(Dictionary<int, Tuio2DCursor> sets, List<int> alive)
        {
            foreach (int curid in alive)
            {
                //Held cursor
                if (!sets.ContainsKey(curid) && this.current.ContainsKey(curid))
                {
                    this.TouchHeld(curid);
                }
                else
                {
                    if (sets.ContainsKey(curid) && this.current.ContainsKey(curid))
                    {
                        Tuio2DCursor cur = sets[curid];
                        if (cur.IsEqual(this.current[curid]))
                        {
                            //Call touchheld if same value
                            this.TouchHeld(curid);
                        }
                        else
                        {
                            this.TouchUpdated(cur);
                        }
                    }
                }
            }
        }

        void removeNotAlive(List<int> alive)
        {
            // Get all the ones to delete
            var result = (from entry in this.current
                          where (!alive.Contains(entry.Key))
                          select entry.Key).ToArray<int>();

            // Delete them
            foreach (int i in result)
                this.TouchRemoved(i);
        }

        protected void TouchUpdated(Tuio2DCursor cur)
        {
            this.current[cur.SessionID] = cur;
            Touch t = buildTouch(cur);
            this.TouchUpdated(t.TouchId, t.TouchPoint, t.Properties);
        }

        protected void TouchAdded(Tuio2DCursor cur)
        {
            this.current.Add(cur.SessionID, cur);
            Touch t = buildTouch(cur);
            this.TouchAdded(t);
        }

        protected new void TouchRemoved(int touchId)
        {
            this.current.Remove(touchId);
            base.TouchRemoved(touchId);
        }

        Touch buildTouch(Tuio2DCursor cursor)
        {
            TouchProperties prop=new TouchProperties();
            prop.Acceleration = cursor.Acceleration;
            prop.VelocityX = cursor.VelocityX;
            prop.VelocityY = cursor.VelocityY;

            PointF p = getTouchPoint(cursor);


            Touch t = new Touch(cursor.SessionID, p);
            t.Properties = prop;

            return t;
        }

        PointF getTouchPoint(Tuio2DCursor data)
        {
            float x1 = getScreenPoint((float)data.PositionX,
                this.config.Projection.ScaleX,
                this.config.Projection.OffsetX,
                this.config.Alignment.FlipX,
                this.config.CorrectProjection,
                ScreenWidth);
            float y1 = getScreenPoint((float)data.PositionY,
                this.config.Projection.ScaleY,
                this.config.Projection.OffsetY,
                this.config.Alignment.FlipY,
                this.config.CorrectProjection,
                ScreenHeight);

            PointF t = new PointF(x1, y1);
            return t;
        }

        float getScreenPoint(float xOrY, float scale, float offset, bool flip, bool correctProjection, double screenDimension)
        {
            if (flip) xOrY = PointTransform.Flip(xOrY, 0.5f);
            float projSpace = xOrY * 2f - 1f;
            if (correctProjection) projSpace = PointTransform.ScaleOffset(projSpace, scale, offset);
            xOrY = PointTransform.ScaleZeroToOne(projSpace);
            xOrY *= (float)screenDimension;
            return xOrY;
        }
    }
}
