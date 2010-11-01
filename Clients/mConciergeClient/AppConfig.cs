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


using System.Configuration;

namespace mConcierge
{
    /// <summary>
    /// Loads data from App.config file for basic app configuration.
    /// </summary>
    internal static class AppConfig
    {
        const bool DEFAULT_FLIPX = false;
        const bool DEFAULT_FLIPY = false;
        const bool DEFAULT_CP = false;
        const int DEFAULT_PORT = 3333;
        const bool DEFAULT_FS = false;

        internal static bool FlipX = false;
        internal static bool FlipY = false;
        internal static string TrackingPath = string.Empty;
        internal static bool CorrectProjection = true;
        internal static int Port = 3333;
        internal static bool StartFullscreen = true;

        static internal void LoadAppConfig()
        {
            TrackingPath = GetConfigVal("trackingpath");

            bool parseOk = false;

            bool fX = false;
            parseOk = bool.TryParse(GetConfigVal("flipx"), out fX);
            FlipX = parseOk ? fX : DEFAULT_FLIPX;

            bool fY = false;
            parseOk = bool.TryParse(GetConfigVal("flipy"), out fY);
            FlipY = parseOk ? fY : DEFAULT_FLIPY;

            bool cp = false;
            parseOk = bool.TryParse(GetConfigVal("correctprojection"), out cp);
            CorrectProjection = parseOk ? cp : DEFAULT_CP;

            bool sf = false;
            parseOk = bool.TryParse(GetConfigVal("startfullscreen"), out sf);
            StartFullscreen = parseOk ? sf : DEFAULT_FS;

            int port = 3333;
            parseOk = int.TryParse(GetConfigVal("port"), out port);
            Port = parseOk ? port : DEFAULT_PORT;

        }

        static string GetConfigVal(string name)
        {
            string data = ConfigurationManager.AppSettings[name];
            if (data == null) return string.Empty; else return data;
        }
    }
}
