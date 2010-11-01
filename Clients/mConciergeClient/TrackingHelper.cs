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

using System.Threading;
using System.Windows;
using TouchFramework;
using TouchFramework.Config;
using TouchFramework.Tracking;

namespace mConcierge
{
    /// <summary>
    /// Helper class for loading the correct FrameworkControl class based on the type of tracking needed
    /// and the correct config for that tracking system.
    /// </summary>
    internal class TrackingHelper
    {
        internal enum TrackingType
        {
            Mouse = 0,
            TUIO,
            Traal
        }

        internal static FrameworkControl GetTracking(FrameworkElement uiParent, TrackingType trType)
        {
            switch (trType)
            {
                case TrackingType.Mouse:
                    return TrackingHelper.GetMouseTracking(uiParent);
                case TrackingType.TUIO:
                    return TrackingHelper.GetTuioTracking(uiParent);
                case TrackingType.Traal:
                    return TrackingHelper.GetTraalTracking(uiParent);
                default:
                    return TrackingHelper.GetMouseTracking(uiParent);
            }
        }

        internal static FrameworkControl GetMouseTracking(FrameworkElement uiParent)
        {
            FrameworkControl tmpFrame = new MouseTracking(uiParent);
            FrameworkConfiguration conf = GetMouseConfig(uiParent);
            tmpFrame.ConfigureFramework(conf);
            return tmpFrame;
        }

        internal static FrameworkControl GetTraalTracking(FrameworkElement uiParent)
        {
            FrameworkControl tmpFrame = new TraalTracking(uiParent);
            FrameworkConfiguration conf = GetTraalConfig(uiParent);
            tmpFrame.ConfigureFramework(conf);
            return tmpFrame;
        }

        internal static FrameworkControl GetTuioTracking(FrameworkElement uiParent)
        {
            FrameworkControl tmpFrame = new TuioTracking(uiParent);
            FrameworkConfiguration conf = GetTuioConfig(uiParent);
            tmpFrame.ConfigureFramework(conf);
            return tmpFrame;
        }

        internal static FrameworkConfiguration GetMouseConfig(FrameworkElement uiParent)
        {
            var conf = new MouseConfiguration
            {
                Owner = uiParent,
                UIManagedThreadId = Thread.CurrentThread.ManagedThreadId,
                EventWindow = uiParent
            };
            return conf;
        }

        internal static FrameworkConfiguration GetTraalConfig(FrameworkElement uiParent)
        {
            AppConfig.LoadAppConfig();

            ProjectionConfig proj = ProjectionConfig.Load(AppConfig.TrackingPath);
            if (proj == null) proj = ProjectionConfig.LoadDefaults();

            var align = new AlignConfig()
            {
                FlipX = AppConfig.FlipX,
                FlipY = AppConfig.FlipY
            };

            var conf = new TraalConfiguration
            {
                Owner = uiParent,
                UIManagedThreadId = Thread.CurrentThread.ManagedThreadId,
                CorrectProjection = AppConfig.CorrectProjection,
                UseVfwDriver = false,
                Alignment = align,
                Projection = proj,
                TrackingConfigPath = AppConfig.TrackingPath,
            };

            return conf;
        }

        internal static FrameworkConfiguration GetTuioConfig(FrameworkElement uiParent)
        {
            AppConfig.LoadAppConfig();

            ProjectionConfig proj = ProjectionConfig.Load(AppConfig.TrackingPath);
            if (proj == null) proj = ProjectionConfig.LoadDefaults();

            var align = new AlignConfig()
            {
                FlipX = AppConfig.FlipX,
                FlipY = AppConfig.FlipY
            };

            var conf = new TuioConfiguration
            {
                Owner = uiParent,
                UIManagedThreadId = Thread.CurrentThread.ManagedThreadId,
                CorrectProjection = AppConfig.CorrectProjection,
                Alignment = align,
                Projection = proj,
                Port = AppConfig.Port,
            };

            return conf;
        }
    }
}
