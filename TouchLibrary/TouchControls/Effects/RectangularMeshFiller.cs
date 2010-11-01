using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;

namespace TouchControls.Effects
{
	public class RectangularMeshFiller
	{
		public Point3DCollection FillMesh(int xVertices, int yVertices, double aspect)
		{
			LineEvaluator hLine = new LineEvaluator();
			hLine.Point1 = new Point3D(-aspect/2, 0, 0);
			hLine.Point2 = new Point3D(aspect/2, 0, 0);

			LineEvaluator vLine = new LineEvaluator();
			vLine.Point1 = new Point3D(0, 0.5, 0);
			vLine.Point2 = new Point3D(0, -0.5, 0);

			Point3DCollection positions = new Point3DCollection();
			for (int y = 0; y < yVertices; y++)
			{
				double vT = (double) y/(yVertices - 1);

				Point3D vPoint = vLine.Evaluate(vT);
				for (int x = 0; x < xVertices; x++)
				{
					double hT = (double) x/(xVertices - 1);
					Point3D hPoint = hLine.Evaluate(hT);

					positions.Add(new Point3D(hPoint.X, vPoint.Y, 0));
				}
			}
			return positions;
		}
	}
}