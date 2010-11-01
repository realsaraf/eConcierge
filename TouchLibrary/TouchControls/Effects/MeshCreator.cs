using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Windows.Media;

namespace TouchControls.Effects
{
	public static class MeshCreator
	{
		public static MeshGeometry3D CreateMesh(int xVertices, int yVertices)
		{
			Vector3DCollection normals = new Vector3DCollection();
			PointCollection textCoords = new PointCollection();
			for (int y = 0; y < yVertices; y++)
			{
				for (int x = 0; x < xVertices; x++)
				{
					// Normals
					Vector3D n1 = new Vector3D(0, 0, 1);
					normals.Add(n1);

					// Texture Coordinates
					textCoords.Add(GetTextureCoordinate(xVertices, yVertices, y, x));
				}
			}
			Int32Collection indices = GetTriangleIndices(xVertices, yVertices);

			MeshGeometry3D mesh = new MeshGeometry3D();
			mesh.Normals = normals;
			mesh.TriangleIndices = indices;
			mesh.TextureCoordinates = textCoords;
			return mesh;
		}

		private static Int32Collection GetTriangleIndices(int xVertices, int yVertices)
		{
			Int32Collection indices = new Int32Collection();
			for (int y = 0; y < yVertices - 1; y++)
			{
				for (int x = 0; x < xVertices - 1; x++)
				{
					int v1 = x + y*xVertices;
					int v2 = v1 + 1;
					int v3 = v1 + xVertices;
					int v4 = v3 + 1;
					indices.Add(v1);
					indices.Add(v3);
					indices.Add(v4);
					indices.Add(v1);
					indices.Add(v4);
					indices.Add(v2);
				}
			}
			return indices;
		}

		private static Point GetTextureCoordinate(int xVertices, int yVertices, int row, int col)
		{
			double blockWidth = 1.0D/(xVertices - 1);
			double blockHeight = 1.0D/(yVertices - 1);

			Point p = new Point();
			p.X = col*blockWidth;
			p.Y = row*blockHeight;
			return p;
		}
	}
}