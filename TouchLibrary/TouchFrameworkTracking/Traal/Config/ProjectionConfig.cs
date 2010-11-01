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
using System.Text;
using System.IO;
using System.Xml.XPath;

namespace TouchFramework.Tracking
{
    public class ProjectionConfig
    {
        public float OffsetX
        {
            get;
            set;
        }
        public float OffsetY
        {
            get;
            set;
        }
        public float ScaleX
        {
            get;
            set;
        }
        public float ScaleY
        {
            get;
            set;
        }

        const float DEFAULT_SCALEX = 1f;
        const float DEFAULT_SCALEY = 1f;
        const float DEFAULT_OFFSETX = 0f;
        const float DEFAULT_OFFSETY = 0f;

        public static ProjectionConfig LoadDefaults()
        {
            ProjectionConfig projection = new ProjectionConfig();
            projection.OffsetX = DEFAULT_OFFSETX;
            projection.OffsetY = DEFAULT_OFFSETY;
            projection.ScaleX = DEFAULT_SCALEX;
            projection.ScaleY = DEFAULT_SCALEY;
            return projection;
        }

        public static ProjectionConfig Load(string userSettingsPath)
        {
            if (!File.Exists(userSettingsPath)) return null;

            XPathDocument doc = new XPathDocument(userSettingsPath);
            XPathNavigator nav = doc.CreateNavigator();

            XPathNavigator posxA = nav.SelectSingleNode("//Projection/Screens/Screen/@PosX");
            XPathNavigator posyA = nav.SelectSingleNode("//Projection/Screens/Screen/@PosY");
            XPathNavigator scalexA = nav.SelectSingleNode("//Projection/Screens/Screen/@ScaleX");
            XPathNavigator scaleyA = nav.SelectSingleNode("//Projection/Screens/Screen/@ScaleY");

            ProjectionConfig projection = new ProjectionConfig();
            projection.OffsetX = float.Parse(posxA.Value);
            projection.OffsetY = float.Parse(posyA.Value) * -1;
            projection.ScaleX = float.Parse(scalexA.Value);
            projection.ScaleY = float.Parse(scaleyA.Value);
            return projection;
        }
    }
}
