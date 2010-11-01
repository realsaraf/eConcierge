using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;

namespace TouchControls.Effects
{
    public class LineEvaluator
    {
        private Point3D _point1;

        public Point3D Point1
        {
            get { return _point1; }
            set { _point1 = value; }
        }

        private Point3D _point2;

        public Point3D Point2
        {
            get { return _point2; }
            set { _point2 = value; }
        }

        public Point3D Evaluate(double t)
        {
            Point3D pos = new Point3D();
            pos.X = Point2.X * t + (1 - t) * Point1.X;
            pos.Y = Point2.Y * t + (1 - t) * Point1.Y;
            pos.Z = Point2.Z * t + (1 - t) * Point1.Z;

            return pos;
        }
    }
}