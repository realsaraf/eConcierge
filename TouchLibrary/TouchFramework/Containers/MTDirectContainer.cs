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
using Infrasturcture.TouchLibrary;
using TouchFramework.Containers;

namespace TouchFramework
{
    /// <summary>
    /// Wraps any FrameworkElement object with a controlling interface which stores touch information and 
    /// processes actions based on the touches present.
    /// </summary>
    public class MTDirectContainer : MTContainer
    {
        delegate void InvokeDelegate();

        /// <summary>
        /// Constructor for MTElementContainer.
        /// </summary>
        /// <param name="createFrom">The FrameworkElement this container is going to store touches for and manipulate.</param>
        public MTDirectContainer(FrameworkElement createFrom, Panel cont, ElementProperties props)
            : base(createFrom, cont, props)
        {
        }

        /// <summary>
        /// Performs a rendertransform applying the scale to the working object.
        /// </summary>
        /// <param name="scaleFactor">Value to multiply the object's width and height by</param>
        /// <param name="centerPoint">Point in screen space for the center of the scale operation</param>
        public override void Scale(float scaleFactor, PointF centerPoint)
        {
            if (!Supports(TouchAction.Resize)) return;
            ScaleTransform sc = new ScaleTransform(scaleFactor, scaleFactor, centerPoint.X - StartX, centerPoint.Y - StartY);
            AddTransform(sc);

            this.ApplyTransforms();
        }

        /// <summary>
        /// Performs a rendertransform moving the object from it's current position (after all previous tranforms).
        /// </summary>
        /// <param name="offsetX">Number of pixels to move the object on the x axis.</param>
        /// <param name="offsetY">Number of pixels to move the object on the y axis.</param>
        public override void Move(float offsetX, float offsetY)
        {
            if (!Supports(TouchAction.Move)) return;
            TranslateTransform tt = new TranslateTransform(offsetX, offsetY);
            AddTransform(tt);

            this.ApplyTransforms();
        }

        /// <summary>
        /// Performs a rendertransform rotating the object around the center point provided from it's current rotation (after all previous transforms).
        /// </summary>
        /// <param name="angle">The angle to rotate by.</param>
        /// <param name="centerPoint">Point in screen space for the center of the scale operation.</param>
        public override void Rotate(float angle, PointF centerPoint)
        {
            if (!Supports(TouchAction.Rotate)) return;
            RotateTransform rt = new RotateTransform(angle, centerPoint.X - StartX, centerPoint.Y - StartY);
            AddTransform(rt);

            this.ApplyTransforms();
        }

        /// <summary>
        /// Combines a scale, rotate and move operation for simplicity
        /// </summary>
        /// <param name="angle">The angle to rotate by.</param>
        /// <param name="scaleFactor">Value to multiply the object's width and height by.</param>
        /// <param name="offsetX">Number of pixels to move the object on the x axis.</param>
        /// <param name="offsetY">Number of pixels to move the object on the y axis.</param>
        /// <param name="centerPoint">Point in screen space for the center of the scale operation.</param>
        public override void ScaleRotateMove(float angle, float scaleFactor, float offsetX, float offsetY, PointF centerPoint)
        {
            ScaleTransform sc = new ScaleTransform(scaleFactor, scaleFactor, centerPoint.X - StartX, centerPoint.Y - StartY);
            TranslateTransform tt = new TranslateTransform(offsetX, offsetY);
            RotateTransform rt = new RotateTransform(angle, centerPoint.X - StartX, centerPoint.Y - StartY);

            if (Locked)
                if (ClearTransformsNext) ResetTransforms();

            if (Supports(TouchAction.Resize)) this.AddTransform(sc);
            if (Supports(TouchAction.Move)) this.AddTransform(tt);
            if (Supports(TouchAction.Rotate)) this.AddTransform(rt);

            this.ApplyTransforms();
        }

        protected override void Cleanup()
        {
            // Nothing to cleanup here
        }
    }
}