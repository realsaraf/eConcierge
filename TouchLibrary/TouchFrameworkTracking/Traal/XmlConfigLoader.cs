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
using Mindstorm.Tracking.V1.Configuration;
using Mindstorm.Tracking.V1;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace TouchFramework.Tracking
{
    class XmlConfigLoader : ITrackingConfigurator
    {
        private string path;
        private XmlDocument doc = new XmlDocument();

        public void LoadFromFile(string path)
        {
            this.path = path;
            StreamReader sr = new StreamReader(path);
            String str = sr.ReadToEnd();
            sr.Close();
            this.doc.LoadXml(str);
        }


        public void Configure(ITrackingClient client)
        {
            XmlElement root = this.doc.DocumentElement;

            //Get Driver
            String nodepath;
            XmlNode node;
            XmlNodeList nodeList;

            nodepath = "//Configuration/Tracking/Cameras";
            node = root.SelectSingleNode(nodepath);
            int cameracount = Convert.ToInt32(node.Attributes["Count"].Value);
            float finvert = Convert.ToSingle(node.Attributes["Invert"].Value);

            try
            {
                String sdriver = node.Attributes["Driver"].Value;
                eCameraDriver driver = (eCameraDriver)Enum.Parse(typeof(eCameraDriver), sdriver);
                client.CameraSettings.SetDriver(driver);
            }
            catch
            {
                client.CameraSettings.SetDriver(eCameraDriver.Cmu);
            }

            client.CameraSettings.SetInvertImage(finvert >= 0.5);
            client.CameraSettings.Clear();

            for (int i = 0; i < cameracount; i++)
            {

                nodepath = "//Configuration/Tracking/Cameras/Camera[@ID='" + i + "']";
                node = root.SelectSingleNode(nodepath);

                
                client.CameraSettings.CreateCamera(i);
                CameraSettings cam = client.CameraSettings.GetCamera(i);

                //Size and fps
                XmlNode subnode = node.SelectSingleNode("Settings");

                int sx = Convert.ToInt32(subnode.Attributes["SizeX"].Value);
                int sy = Convert.ToInt32(subnode.Attributes["SizeY"].Value);
                int fps = Convert.ToInt32(subnode.Attributes["Fps"].Value);
                
                cam.SetSize(sx, sy);
                cam.SetFrameRate(fps);

                //Light settings
                subnode = node.SelectSingleNode("Light");
                float b = Convert.ToSingle(subnode.Attributes["Brightness"].Value);
                float g = Convert.ToSingle(subnode.Attributes["Gain"].Value);
                float s = Convert.ToSingle(subnode.Attributes["Shutter"].Value);
                cam.SetLight(b, s, g);

                
                //Stitch Settings
                subnode = node.SelectSingleNode("Stitch");
                int stitchx = Convert.ToInt32(subnode.Attributes["X"].Value);
                int stitchy = Convert.ToInt32(subnode.Attributes["Y"].Value);
                cam.SetOrigin(stitchx, stitchy);

                //Flip Settings (Keep for later)
                subnode = node.SelectSingleNode("Flip");
                double flipx = Convert.ToDouble(subnode.Attributes["X"].Value);
                double flipy = Convert.ToDouble(subnode.Attributes["Y"].Value);

                
                //Barrel settings
                nodepath = "//Configuration/Tracking/Barrel/Camera[@ID='" + i + "']";
                node = root.SelectSingleNode(nodepath);

                //Focal
                subnode = node.SelectSingleNode("Focal");

                float focalx = Convert.ToSingle(subnode.Attributes["X"].Value);
                float focaly = Convert.ToSingle(subnode.Attributes["Y"].Value);
                cam.Barrel.SetFocal(focalx, focaly);

                //Principal
                subnode = node.SelectSingleNode("Principal");
                float principalx = Convert.ToSingle(subnode.Attributes["X"].Value);
                float principaly = Convert.ToSingle(subnode.Attributes["Y"].Value);
                cam.Barrel.SetPrincipal(principalx, principaly);

                
                
                //Distortion
                subnode = node.SelectSingleNode("Distorsion");
                float d0 = Convert.ToSingle(subnode.Attributes["D0"].Value);
                float d1 = Convert.ToSingle(subnode.Attributes["D1"].Value);
                float d2 = Convert.ToSingle(subnode.Attributes["D2"].Value);
                float d3 = Convert.ToSingle(subnode.Attributes["D3"].Value);
                cam.Barrel.SetBarrel(d0, d1, d2, d3);

                //Border
                subnode = node.SelectSingleNode("Border");
                int border = Convert.ToInt32(subnode.Attributes["Size"].Value);
                cam.Barrel.SetBorder(border);

                //Perspective
                nodepath = "//Configuration/Tracking/Perspective/Camera[@ID='" + i + "']/Point";
                nodeList = root.SelectNodes(nodepath);

                List<float> origx = new List<float>();
                List<float> origy = new List<float>();
                List<float> screenx = new List<float>();
                List<float> screeny = new List<float>();

                for (int p = 0; p < nodeList.Count; p++)
                {
                    float ptorigx = Convert.ToSingle(nodeList[p].Attributes["OrigX"].Value);
                    float ptorigy = Convert.ToSingle(nodeList[p].Attributes["OrigY"].Value);


                    if (flipx >= 0.5)
                    {
                        ptorigx = sx - ptorigx;
                    }
                    if (flipy >= 0.5)
                    {
                        ptorigy = sy - ptorigy;
                    }

                    origx.Add(ptorigx);
                    origy.Add(ptorigy);

                    float ptscreenx = Convert.ToSingle(nodeList[p].Attributes["ScreenX"].Value);
                    float ptscreeny = Convert.ToSingle(nodeList[p].Attributes["ScreenY"].Value);

                    screenx.Add(ptscreenx);
                    screeny.Add(ptscreeny);
                }

                cam.Perspective.SetMapping(origx, origy, screenx, screeny);
            }


            
            //Barrel perspective enabled
            nodepath = "//Configuration/Tracking/Barrel";
            node = root.SelectSingleNode(nodepath);
            double enablebarrel = Convert.ToDouble(node.Attributes["Enabled"].Value);

            nodepath = "//Configuration/Tracking/Perspective";
            node = root.SelectSingleNode(nodepath);
            double enableperspective = Convert.ToDouble(node.Attributes["Enabled"].Value);

            client.CameraSettings.SetBarrelEnabled(enablebarrel >= 0.5);
            client.CameraSettings.SetPerspectiveEnabled(enableperspective >= 0.5);

            client.CameraSettings.Invalidate();


            
            //Background Settings
            nodepath = "//Configuration/Tracking/Background";
            node = root.SelectSingleNode(nodepath);

            client.BackgroundSettings.SetAbsolute(Convert.ToSingle(node.Attributes["Absolute"].Value) >= 0.5);
            client.BackgroundSettings.SetMotionMode(false);
            client.BackgroundSettings.SetEnabled(true);

            //Highpass settings
            nodepath = "//Configuration/Tracking/HighPass";
            node = root.SelectSingleNode(nodepath);

            client.HighpassSettings.SetEnabled(Convert.ToSingle(node.Attributes["Enabled"].Value) >= 0.5);
            client.HighpassSettings.SetKernelSize(Convert.ToSingle(node.Attributes["Force"].Value));
            client.HighpassSettings.SetScale(Convert.ToSingle(node.Attributes["Scale"].Value));
            client.HighpassSettings.EnableHQNoiseReduction(Convert.ToSingle(node.Attributes["HQ"].Value) >= 0.5);

            
            //Hotspots settings
            nodepath = "//Configuration/Tracking/HotSpots";
            node = root.SelectSingleNode(nodepath);

            client.HotspotSettings.SetEnabled(Convert.ToSingle(node.Attributes["Enabled"].Value) >= 0.5);
            client.HotspotSettings.SetMagnification(Convert.ToSingle(node.Attributes["BlowUp"].Value));
            client.HotspotSettings.SetThreshold(Convert.ToSingle(node.Attributes["Threshold"].Value));
            client.HotspotSettings.SetConfigurationMode(false);

            //Threshold settings
            nodepath = "//Configuration/Tracking/Threshold";
            node = root.SelectSingleNode(nodepath);

            String thrmode = node.Attributes["Mode"].Value;
            eThresholdMode ethrmode = (eThresholdMode)Enum.Parse(typeof(eThresholdMode), thrmode);
            client.ThresholdSettings.SetMode(ethrmode);
            client.ThresholdSettings.SetEnabled(Convert.ToSingle(node.Attributes["Enabled"].Value) >= 0.5);
            client.ThresholdSettings.SetOffset(Convert.ToSingle(node.Attributes["Offset"].Value));
            client.ThresholdSettings.SetScale(Convert.ToSingle(node.Attributes["Scale"].Value));

            //Blobs settings
            
            nodepath = "//Configuration/Tracking/Blobs";
            node = root.SelectSingleNode(nodepath);

            client.TrackingSettings.SetEnabled(true);
            client.TrackingSettings.SetMinSize(Convert.ToSingle(node.Attributes["MinSize"].Value));
            client.TrackingSettings.SetMaxSize(Convert.ToSingle(node.Attributes["MaxSize"].Value));
            client.TrackingSettings.SetMaxRejectDistance(Convert.ToSingle(node.Attributes["MaxRejectDist"].Value));
            client.TrackingSettings.SetMaxInvisible(Convert.ToSingle(node.Attributes["MaxInvisible"].Value));

            try
            {
                client.TrackingSettings.SetMinMovementThreshold(Convert.ToSingle(node.Attributes["MinMoveThreshold"].Value));
            }
            catch
            {
                client.TrackingSettings.SetMinMovementThreshold(0.0f);
            }

            try
            {
                client.TrackingSettings.SetMergeDistance(Convert.ToSingle(node.Attributes["MinMerge"].Value));
            }
            catch
            {
                client.TrackingSettings.SetMergeDistance(0.0f);
            }

            try
            {
                client.TrackingSettings.SetMaxHolds(Convert.ToInt32(node.Attributes["MaxHolds"].Value));
            }
            catch
            {
                client.TrackingSettings.SetMaxHolds(0);
            }
        }
    }
}
