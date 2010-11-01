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
using System.Drawing;

namespace TouchFramework.Helpers
{
    /// <summary>
    /// Handles basic calculation logic on points or sets of points.
    /// </summary>
    public class PointTransform
    {
        /// <summary>
        /// Calculates the center of the axis aligned bounding box of a set of points.
        /// </summary>
        /// <param name="points">Points to calculate bounding box center for.</param>
        /// <returns>Point which is the center of the axis aligned bounding box.</returns>
        public static PointF GetBoxCenterPoint(PointF[] points)
        {
            float minX = 0f;
            float minY = 0f;
            float maxX = 0f;
            float maxY = 0f;
            float cenX = 0f;
            float cenY = 0f;
            bool firstPass = true;

            for (int i = 0; i < points.Length; i++)
            {
                PointF p = points[i];

                minX = p.X < minX || firstPass ? p.X : minX;
                minY = p.Y < minY || firstPass ? p.Y : minY;
                maxX = p.X > maxX || firstPass ? p.X : maxX;
                maxY = p.Y > maxY || firstPass ? p.Y : maxY;
                firstPass = false;
            }
            cenX = minX + ((maxX - minX) / 2);
            cenY = minY + ((maxY - minY) / 2);

            return new PointF(cenX, cenY);
        }

        /// <summary>
        /// Calculates a simple center point of the passed points by averaging all of the x and y values.
        /// </summary>
        /// <param name="points">Points to calculate the center for.</param>
        /// <returns>Point which is roughly the center of the points.</returns>
        public static PointF GetAvgCenterPoint(PointF[] points)
        {
            float avgX = 0f;
            float avgY = 0f;

            for (int i = 0; i < points.Length; i++)
            {
                PointF p = points[i];

                avgX += p.X;
                avgY += p.Y;
            }
            avgX /= points.Length;
            avgY /= points.Length;

            return new PointF(avgX, avgY);
        }

        /// <summary>
        /// Gets the average distance of the passed points from the center.
        /// </summary>
        /// <param name="center">Center to calculate distance from.</param>
        /// <param name="points">Points to average distance for.</param>
        /// <returns></returns>
        public static float GetAvgDistanceFromCenter(PointF center, PointF[] points)
        {
            if (points.Length == 0) return 0f;

            float avgDist = 0f;

            for (int i = 0; i < points.Length; i++)
            {
                PointF p = points[i];
                avgDist += PointTransform.GetDistance(center, p);
            }
            avgDist /= points.Length;

            return avgDist;
        }
        /// <summary>
        /// Gets the distance of the passed point from the center.
        /// </summary>
        /// <param name="center">Center to calculate distance from.</param>
        /// <param name="points">Point to distance for.</param>
        /// <returns></returns>
        public static float GetDistanceFromCenter(PointF center, PointF point)
        {
            float dist = PointTransform.GetDistance(center, point);
            return dist;
        }
        /// <summary>
        /// Returns the angle in degrees from the passed point in comparision 
        /// to the x-axis of the centerpoint passed.
        /// </summary>
        /// <param name="centerPoint">Center point for the x-axis to calculate the angle against.</param>
        /// <param name="point">The point to compare against the x-axis.</param>
        /// <returns>Degrees between the point and the center point.</returns>
        public static float GetAngle(PointF centerPoint, PointF point)
        {
            return ExtMaths.RadiansToDegrees(PointTransform.GetAngleRadiansTan(centerPoint, point));
        }
        /// <summary>
        /// Returns the angle in radians from the passed point in comparision 
        /// to the x-axis of the centerpoint passed.
        /// </summary>
        /// <param name="centerPoint">Center point for the x-axis to calculate the angle against.</param>
        /// <param name="point">The point to compare against the x-axis.</param>
        /// <returns>Radians between the to point and the x-axis of the from point.</returns>
        public static float GetAngleRadiansTan(PointF from, PointF to)
        {
            if (from == to) return 0f;

            float dX = to.X - from.X;
            float dY = to.Y - from.Y;

            float angle = (float)Math.Atan2(dY, dX);

            return angle;
        }
        /// <summary>
        /// Calculates the distance between 2 points.
        /// </summary>
        /// <param name="from">First point to compare.</param>
        /// <param name="to">Point to compare to.</param>
        /// <returns></returns>
        public static float GetDistance(PointF from, PointF to)
        {
            float a = from.X - to.X;
            float b = from.Y - to.Y;
            return ExtMaths.GetHypotenuse(a, b);
        }

        /// <summary>
        /// Converts a point from 0 to 1 range to screen space including accounting for the top and left offset.
        /// </summary>
        /// <param name="screenArea">Rectangle defining the screen area.</param>
        /// <param name="loc">The point to calculate.</param>
        /// <returns>The point in screen space.</returns>
        public static PointF GetScreenPos(Rectangle screenArea, PointF loc)
        {
            PointF screenPos = new PointF();
            screenPos.X = (int)Math.Round(loc.X * screenArea.Width) + screenArea.Left;
            screenPos.Y = (int)Math.Round(loc.Y * screenArea.Width) + screenArea.Top;
            return screenPos;
        }

        /// <summary>
        /// Converts a point from -1 to +1 (3D style) to 0 to 1 (2D style).
        /// </summary>
        /// <param name="minusOneToPlusOne">Point in -1 to +1 space.</param>
        /// <returns>Converted point in 0 to 1 space.</returns>
        public static float ScaleZeroToOne(float minusOneToPlusOne)
        {
            return (float)(minusOneToPlusOne / 2f + 0.5f);
        }
        /// <summary>
        /// Flips a value to the opposite position.  Call this for y to flip y or x to flip x.
        /// </summary>
        /// <param name="value">x or y co-ordinate to flip.</param>
        /// <param name="middle">The center point of the space you are using, e.g. 0.5 on a 0 to 1 space.</param>
        /// <returns></returns>
        public static float Flip(float value, float middle)
        {
            return middle + (middle - value);
        }
        /// <summary>
        /// Flips a value to the opposite position for 0 to 1 space values.  Call this for y to flip y or x to flip x.
        /// </summary>
        /// <param name="value">x or y co-ordinate to flip.</param>
        /// <returns></returns>
        public static float Flip(float zeroToOne)
        {
            return 0.5f + (0.5f - zeroToOne);
        }
        /// <summary>
        /// Scales and offset's a value.  Use for matching screen points to surface points.
        /// </summary>
        /// <param name="value">x or y co-ordinate to adjust.</param>
        /// <param name="scale">The scale as a multiplier.</param>
        /// <param name="offset">The offset as a fixed value in the point space.</param>
        /// <returns></returns>
        public static float ScaleOffset(float value, float scale, float offset)
        {
            return (value / scale) + offset;
        }
        /// <summary>
        /// Scales a value.  Use for matching screen points to surface points.
        /// </summary>
        /// <param name="value">x or y co-ordinate to adjust.</param>
        /// <param name="scale">The scale as a multiplier.</param>
        /// <returns></returns>
        public static float Scale(float value, float scale)
        {
            return value / scale;
        }
        /// <summary>
        /// Offset's a value.  Use for matching screen points to surface points.
        /// </summary>
        /// <param name="value">x or y co-ordinate to adjust.</param>
        /// <param name="offset">The offset as a fixed value in the point space.</param>
        /// <returns></returns>
        public static float Offset(float value, float offset)
        {
            return value + offset;
        }
    }
}
