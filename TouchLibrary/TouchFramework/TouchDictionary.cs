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
using System.Drawing;
using System.IO;
using Infrasturcture.TouchLibrary;

namespace TouchFramework
{
    /// <summary>
    /// Stores a set of touches (often a set for a specific element).
    /// Provides evalulation of touch information for angles, distance and center point calculations.
    /// </summary>
    public class TouchDictionary : Dictionary<int,  ITouch>, ITouchDictionary
    {
        const int KEEP_NUMPOINTS = 15;

        public int PrevTouchCount { get; private set; }
        public int CurrTouchCount { get; private set; }
        float prevDistance = 0f;
        float currDistance = 0f;
        float prevAngle = 0f;
        float currAngle = 0f;
        bool collectionChanged = false;

        double velocityX = 0;
        double velocityY = 0;
        double oldVelocityX = 0;
        double oldVelocityY = 0;

        int[] idsMoving = new int[0];
        int[] prevIdsMoving = new int[0];
        int[] numPointsSet = new int[KEEP_NUMPOINTS];
        PointF[] pointsStatic;
        PointF[] pointsAll;
        PointF staticCenter = PointF.Empty;
        PointF fixedCenter = new PointF(0f, 0f);
        PointF prevMoveCenter = new PointF(0f, 0f);

        /// <summary>
        /// Returns the screen space co-ordinates of the movement centerpoint.
        /// Useful for checking if a movement is within an object space.
        /// </summary>
        private PointF _moveCenter;
        public PointF MoveCenter
        {
            get
            {
                if (_moveCenter == null)
                    _moveCenter = new PointF(0,0);
                return _moveCenter;
            }
            set
            {
                _moveCenter = value;
            }
        }
        /// <summary>
        /// Calculated amount of move X.
        /// </summary>
        public float MoveX { get; set; }
        /// <summary>
        /// Calculated amount of move Y.
        /// </summary>
        public float MoveY { get; set; }
        /// <summary>
        /// Calculated amount of move X from the last frame.
        /// </summary>
        public float LastMoveX = 0f;
        /// <summary>
        /// Calculated amount of move Y from the last frame.
        /// </summary>
        public float LastMoveY = 0f;
        /// <summary>
        /// Where or not something has changed in this collection (e.g. object added or removed).
        /// </summary>
        public bool Changed { get; set; }

        /// <summary>
        /// The center of the action for scale, rotate etc...
        /// </summary>
        private PointF _actionCenter;
        public PointF ActionCenter
        {
            get
            {
                if (_actionCenter == null)
                    _actionCenter = new PointF();
                return _actionCenter;
            }
            set
            {
                _actionCenter = value;
            }
        }
            
        /// <summary>
        /// Adds a new touch object to this dictionary.
        /// </summary>
        /// <param name="touchId">Id of the touch being added.</param>
        /// <param name="touch">Touch object to add.</param>
        public new void Add(int touchId, ITouch touch)
        {
            collectionChanged = true;
            if (!base.ContainsKey(touchId))
                base.Add(touchId, touch);
        }

        /// <summary>
        /// Removes a touch object from the dictionary.
        /// </summary>
        /// <param name="touchId">Id of the touch object to remove.</param>
        public new void Remove(int touchId)
        {
            collectionChanged = true;
            base.Remove(touchId);
        }

        /// <summary>
        /// Indexer for getting touch objects.
        /// </summary>
        /// <param name="touchId">Id of the touch.</param>
        /// <returns>Touch object requested.</returns>
        public new ITouch this[int touchId]
        {
            get
            {
                return base[touchId];
            }
            set
            {
                base[touchId] = value;
            }
        }

        /// <summary>
        /// The X velocity calculated from the last frame.
        /// Use this for calculating flick velocity etc...
        /// </summary>
        public double LastVelocityX
        {
            get
            {
                return oldVelocityX;
            }
        }

        /// <summary>
        /// The Y velocity calculated from the last frame.
        /// Use this for calculating flick velocity etc...
        /// </summary>
        public double LastVelocityY
        {
            get
            {
                return oldVelocityY;
            }
        }

        /// <summary>
        /// Returns true if all fingers were removed from the object this frame.
        /// </summary>
        public bool Lifted
        {
            get
            {
                return (CurrTouchCount == 0 && PrevTouchCount != 0);
            }
        }

        /// <summary>
        /// Uses the history of past number of touches to calculate is the object has been tapped.
        /// A tap is defined by a touch held for 1 or 2 frame only, no longer.
        /// </summary>
        public bool Tapped
        {
            get
            {
                bool ret = false;
                int i = 2;
                while (!ret && i < numPointsSet.Length)
                {
                    ret = (numPointsSet[i] == 0);
                    i++;
                }

                ret = ret && 
                    numPointsSet[0] == 0 &&
                    numPointsSet[1] >= 1;

                return ret;
            }
        }

        /// <summary>
        /// Returns true if all fingers were removed from the object this frame.
        /// </summary>
        public bool OneOrMoreLifted
        {
            get
            {
                return CurrTouchCount < PrevTouchCount && PrevTouchCount > 1;
            }
        }

        /// <summary>
        /// Returns true if there were no fingers touching this object last frame,
        /// but there is this one.
        /// </summary>
        public bool JustTouched
        {
            get
            {
                return (CurrTouchCount != 0 && PrevTouchCount == 0);
            }
        }

        /// <summary>
        /// Returns true if there were less than 2 fingers touching this object last frame,
        /// but there is this one.
        /// </summary>
        public bool TwoOrMoreTouch
        {
            get
            {
                return (CurrTouchCount >= 2 && PrevTouchCount < 2);
            }
        }

        /// <summary>
        /// Calculates the angle of change to be used for rotate.
        /// </summary>
        /// <returns>The difference between the old angle and new.</returns>
        public float GetAngleChanged()
        {
            float res = currAngle - prevAngle;
            if (float.IsNaN(res)) res = 0f;
            return res;
        }

        /// <summary>
        /// Gets a value to be used for scaling the object.
        /// </summary>
        /// <returns>The division of the current distance by the previous distance</returns>
        public float GetDistanceChangeRatio()
        {
            if (prevDistance == 0f) return 1f;
            return currDistance / prevDistance;
        }

        /// <summary>
        /// Updates the previous information with the current information and gets the new information.
        /// Stores the new values for analysis of changes and sets a flag for whether or not something has changed.
        /// </summary>
        public void CalculateChanges()
        {
            updatePreviousData();
            calcCenterPoints();

            CurrTouchCount = this.Count;
            numPointsSet.Shift();
            numPointsSet[0] = CurrTouchCount;

            float newCurrDistance = this.getDistanceFromCenter(fixedCenter);
            float newCurrAngle = this.getAngle(fixedCenter);

            currDistance = newCurrDistance;
            currAngle = newCurrAngle;

            // If we have lost or added a finger, don't move, scale or rotate this frame
            if (collectionChanged)
            {
                prevDistance = currDistance;
                prevAngle = currAngle;
                prevMoveCenter = MoveCenter;
            }
            
            MoveX = MoveCenter.X - prevMoveCenter.X;
            MoveY = MoveCenter.Y - prevMoveCenter.Y;

            calcVelocity();

            Changed =
                (currAngle != prevAngle) ||
                (currDistance != prevDistance) ||
                (MoveX != 0f) ||
                (MoveY != 0f) ||
                collectionChanged;

            collectionChanged = false;
        }

        /// <summary>
        /// Stores all current data in previous data vars ready for new data to populate current vars.
        /// </summary>
        void updatePreviousData()
        {
            prevIdsMoving = (int[])idsMoving.Clone();
            PrevTouchCount = CurrTouchCount;
            prevDistance = currDistance;
            prevAngle = currAngle;
            LastMoveX = MoveX;
            LastMoveY = MoveY;
        }

        /// <summary>
        /// Wrapper to calculate all required center points and groups.
        /// </summary>
        void calcCenterPoints()
        {
            calcPointGroups();
            calcFixedCenterPoint();
            calcStaticCenterPoint();
            calcActionCenterPoint();
            calcMovementCenterPoint();
        }

        /// <summary>
        /// Calculates the axis-aligned bounding box of all points
        /// </summary>
        void calcFixedCenterPoint()
        {
            if (pointsAll.Length != 0) fixedCenter = PointTransform.GetBoxCenterPoint(pointsAll);
        }

        /// <summary>
        /// Calculates the axis-aligned bounding box of the static points
        /// </summary>
        void calcStaticCenterPoint()
        {
            if (pointsStatic.Length != 0) staticCenter = PointTransform.GetBoxCenterPoint(pointsStatic);
        }

        /// <summary>
        /// Selects sets of points as PointF arrays to be used for calculations.
        /// </summary>
        void calcPointGroups()
        {
            pointsAll = (from entry in this
                            select entry.Value.TouchPoint).ToArray<PointF>();

            pointsStatic = (from entry in this
                          where (entry.Value.IsMoving == false)
                          select entry.Value.TouchPoint).ToArray<PointF>();

            idsMoving = (from entry in this
                            where (entry.Value.IsMoving == true)
                            select entry.Value.TouchId).ToArray<int>();
        }

        /// <summary>
        /// Calculates the movement centerpoint and updates the old one.
        /// Will always set the old one to the current one unless all fingers are moving.
        /// This results in the x/y difference being zero and stopping movement.
        /// </summary>
        void calcMovementCenterPoint()
        {
            prevMoveCenter = MoveCenter;
            MoveCenter = fixedCenter;
            if (pointsStatic.Length != 0 || this.Count == 0) prevMoveCenter = MoveCenter;
        }

        /// <summary>
        /// Calculates the center point for the source of any scale or rotate.
        /// This is the center of the static fingers (if any) otherwise it's the center of all fingers.
        /// </summary>
        void calcActionCenterPoint()
        {
            if (pointsStatic.Length > 0)
            {
                ActionCenter = staticCenter;
            }
            else
            {
                ActionCenter = fixedCenter;
            }
        }
        

        /// <summary>
        /// Calculates the velocity of just one point.  
        /// Since most actions based on velocity are based on all fingers moving the the same direction, 
        /// it's unlikely there will be much velocity difference between the fingers.
        /// </summary>
        void calcVelocity()
        {
            oldVelocityX = velocityX;
            oldVelocityY = velocityY;

            if (this.Count == 0) return;
            ITouch pTouch = this.First().Value;

            velocityX = pTouch.Properties.VelocityX;
            velocityY = pTouch.Properties.VelocityY;
        }

        /// <summary>
        /// Calculates the distance of just one finger from the center point.
        /// Since the centerpoint used is the fixed center point all fingers are roughly equidistant.
        /// Since this is called every frame, this is a nice optimisation.
        /// </summary>
        /// <param name="center">The center of the axis-aligned bounding box of all fingers</param>
        /// <returns></returns>
        float getDistanceFromCenter(PointF center)
        {
            if (this.Count == 0) return 0f;
            PointF pTouch = this.First().Value.TouchPoint;

            return PointTransform.GetDistanceFromCenter(center, pTouch);
        }

        /// <summary>
        /// Calculates the angle against the x-axis of just one finger from the center point.
        /// Since the centerpoint used is the fixed center point using just one finger gives you the 
        /// most reliable angle calculation for calculating angle changed.  It's also a nice optimisation.  
        /// There is some edge cases where this is not ideal, but they are unlikely to be encountered 
        /// under normal usage.
        /// </summary>
        /// <param name="center">The center of the axis-aligned bounding box of all fingers</param>
        /// <returns></returns>
        float getAngle(PointF center)
        {
            if (this.Count == 0) return 0f;
            PointF pTouch = this.First().Value.TouchPoint;

            return PointTransform.GetAngle(center, pTouch);
        }
    }
}
