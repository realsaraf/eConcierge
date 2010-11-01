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
using System.Windows;
using System.Drawing;

using Mindstorm.Tracking.V1;
using Mindstorm.Tracking.V1.Configuration;

using TouchFramework;
using TouchFramework.Config;

namespace TouchFramework.Tracking
{
    public class TraalTracking : FrameworkControl
    {
        public double ScreenWidth = SystemParameters.PrimaryScreenWidth;
        public double ScreenHeight = SystemParameters.PrimaryScreenHeight;

        TraalConfiguration traalConfig = null;

        TrackingClient trackingClient = new TrackingClient();
        
        bool stopping = false;
        
        public TraalTracking(FrameworkElement uiParent)
        {
            this.Owner = uiParent;
        }

        public override void ConfigureFramework(FrameworkConfiguration config)
        {
            if (!(config is TraalConfiguration)) throw new ArgumentException("Config must be TraalConfiguration");
            traalConfig = config as TraalConfiguration;

            setTrackingConfig(trackingClient, traalConfig.TrackingConfigPath, traalConfig.UseVfwDriver);
            
            base.ConfigureFramework(config);
        }

        void setTrackingConfig(TrackingClient client, string trackingConfigPath, bool useVfw)
        {
            XmlConfigLoader tconf = new XmlConfigLoader();
            tconf.LoadFromFile(trackingConfigPath);
            tconf.Configure(client);
            if (useVfw) client.CameraSettings.SetDriver(eCameraDriver.Vfw);
        }

        public override void Start()
        {
            trackingClient.TouchAdded += new TouchEventHandler(_client_TouchAdded);
            trackingClient.TouchHeld += new TouchEventHandler(_client_TouchHeld);
            trackingClient.TouchRemoved += new TouchEventHandler(_client_TouchRemoved);
            trackingClient.TouchUpdated += new TouchEventHandler(_client_TouchUpdated);
            trackingClient.FrameProcessed += new FrameProcessedEventHandler(_client_FrameProcessed); 
            
            trackingClient.Start();
        }

        public override void Stop()
        {
            stopping = true;
            if (trackingClient == null) return;

            trackingClient.TouchAdded -= new TouchEventHandler(_client_TouchAdded);
            trackingClient.TouchHeld -= new TouchEventHandler(_client_TouchHeld);
            trackingClient.TouchRemoved -= new TouchEventHandler(_client_TouchRemoved);
            trackingClient.TouchUpdated -= new TouchEventHandler(_client_TouchUpdated);
            trackingClient.FrameProcessed -= new FrameProcessedEventHandler(_client_FrameProcessed);

            trackingClient.Stop();
        }

        public override void ForceRefresh()
        {
            trackingClient.BackgroundSettings.ClearModel();
        }

        void _client_FrameProcessed(double duration)
        {
            if (stopping) return;

            this.ProcessUpdates();
        }

        void _client_TouchUpdated(ITouchData data)
        {
            if (stopping) return;
            
            Touch t = buildTouch(data);
            this.TouchUpdated(t.TouchId, t.TouchPoint, t.Properties);
        }

        void _client_TouchRemoved(ITouchData data)
        {
            if (stopping) return;

            this.TouchRemoved(data.Id);
        }

        void _client_TouchHeld(ITouchData data)
        {
            if (stopping) return;

            this.TouchHeld(data.Id);
        }

        void _client_TouchAdded(ITouchData data)
        {
            if (stopping) return;
            
            Touch t = buildTouch(data);
            this.TouchAdded(t);
        }



        Touch buildTouch(ITouchData rawTouch)
        {
            TouchProperties prop=new TouchProperties();
            prop.Acceleration = rawTouch.Acceleration;
            prop.VelocityX = rawTouch.VelocityX;
            prop.VelocityY = rawTouch.VelocityY;

            PointF p = getTouchPoint(rawTouch);

            Touch t = new Touch(rawTouch.Id, p);
            t.Properties = prop;

            return t;
        }

        PointF getTouchPoint(ITouchData data)
        {
            float x1 = getScreenPoint((float)data.PositionX,
                traalConfig.Projection.ScaleX,
                traalConfig.Projection.OffsetX,
                traalConfig.Alignment.FlipX,
                traalConfig.CorrectProjection,
                ScreenWidth);
            float y1 = getScreenPoint((float)data.PositionY,
                traalConfig.Projection.ScaleY,
                traalConfig.Projection.OffsetY,
                traalConfig.Alignment.FlipY,
                traalConfig.CorrectProjection,
                ScreenHeight);

            PointF t = new PointF(x1, y1);
            return t;
        }

        float getScreenPoint(float xOrY, float scale, float offset, bool flip, bool correctProjection, double screenDimension)
        {
            if (flip) xOrY = PointTransform.Flip(xOrY, 0f);
            if (correctProjection) xOrY = PointTransform.ScaleOffset(xOrY, scale, offset);
            xOrY = PointTransform.ScaleZeroToOne(xOrY);
            xOrY *= (float)screenDimension;
            return xOrY;
        }
    }
}
