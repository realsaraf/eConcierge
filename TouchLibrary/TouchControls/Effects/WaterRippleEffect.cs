using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using System.Windows;
using System.Diagnostics;

namespace TouchControls.Effects
{
	public class WaterRippleEffect
	{
		#region Fields

		public event EffectCompletedHandler EffectCompleted;
		private MeshGeometry3D _mesh;

		private int _xVertices;
		private int _yVertices;
		private WaterVertex[,] _vertexGrid;
		private DispatcherTimer _timer;

		private double _damping = 0.65;
		private double _updateInterval = 33;
		private double _refractionIndex = 1.33;

		private double _waterDepth = 1;
		private double _turbulence = 0.25;

		#endregion

		#region Properties

		public double Damping
		{
			get { return _damping; }
			set { _damping = value; }
		}

		public double RefractionIndex
		{
			get { return _refractionIndex; }
			set { _refractionIndex = value; }
		}

		public double WaterDepth
		{
			get { return _waterDepth; }
			set { _waterDepth = value; }
		}

		public double UpdateInterval
		{
			get { return _updateInterval; }
			set { _updateInterval = value; }
		}

		#endregion

		public WaterRippleEffect(int xVertices, int yVertices, MeshGeometry3D mesh)
		{
			_xVertices = xVertices;
			_yVertices = yVertices;
			_mesh = mesh;
			CreateVertexGrid();
		}

		public void PlayEffect()
		{
			_timer = new DispatcherTimer(DispatcherPriority.Normal);
			_timer.Interval = TimeSpan.FromMilliseconds(UpdateInterval);
			_timer.Tick += new EventHandler(UpdateWave);
			_timer.Start();
		}

		public void CancelEffect()
		{
			_timer.Stop();
			_timer = null;
			ResetMesh();
		}

		private void CreateVertexGrid()
		{
			_vertexGrid = new WaterVertex[_yVertices,_xVertices];
			for (int y = 0; y < _yVertices; y++)
			{
				for (int x = 0; x < _xVertices; x++)
				{
					int index = GetIndex(y, x);
					WaterVertex vertex = _vertexGrid[y, x];

					vertex.CurrentHeight = 0;
					vertex.PreviousHeight = 0;
					vertex.OriginalPosition = _mesh.Positions[index];
					vertex.OriginalTexture = _mesh.TextureCoordinates[index];
					vertex.Texture2 = _mesh.TextureCoordinates[index];
					_vertexGrid[y, x] = vertex;
				}
			}
		}

		private void UpdateWave(object sender, EventArgs args)
		{
			ProcessWater();
			UpdateGeometry();
			SwapHeights();

			if (IsWaveComplete())
			{
				_timer.Stop();
				ResetMesh();
				FireEffectCompleted();
			}
		}

		private void SwapHeights()
		{
			for (int y = 0; y < _yVertices; y++)
			{
				for (int x = 0; x < _xVertices; x++)
				{
					double temp = _vertexGrid[y, x].CurrentHeight;
					_vertexGrid[y, x].CurrentHeight = _vertexGrid[y, x].PreviousHeight;
					_vertexGrid[y, x].PreviousHeight = temp;
				}
			}
		}

		private void UpdateGeometry()
		{
			for (int y = 0; y < _yVertices; y++)
			{
				for (int x = 0; x < _xVertices; x++)
				{
					int index = GetIndex(y, x);
					Point3D point = _mesh.Positions[index];
					point.Z = _vertexGrid[y, x].CurrentHeight;
					_mesh.Positions[index] = point;

					_mesh.TextureCoordinates[index] = _vertexGrid[y, x].Texture2;
				}
			}
		}

		private void ProcessWater()
		{
			for (int y = 0; y < _yVertices; y++)
			{
				for (int x = 0; x < _xVertices; x++)
				{
					int xminus1 = (x - 1) < 0 ? 0 : x - 1;
					int xminus2 = (x - 2) < 0 ? 0 : x - 2;
					int yminus1 = (y - 1) < 0 ? 0 : y - 1;
					int yminus2 = (y - 2) < 0 ? 0 : y - 2;

					int xplus1 = (x + 1) > _xVertices - 1 ? _xVertices - 1 : x + 1;
					int xplus2 = (x + 2) > _xVertices - 1 ? _xVertices - 1 : x + 2;
					int yplus1 = (y + 1) > _yVertices - 1 ? _yVertices - 1 : y + 1;
					int yplus2 = (y + 2) > _yVertices - 1 ? _yVertices - 1 : y + 2;

					double newHeight =
						(_vertexGrid[y, xminus2].PreviousHeight +
						 _vertexGrid[y, xminus1].PreviousHeight +
						 _vertexGrid[y, xplus1].PreviousHeight +
						 _vertexGrid[y, xplus2].PreviousHeight +
						 _vertexGrid[yminus2, x].PreviousHeight +
						 _vertexGrid[yminus1, x].PreviousHeight +
						 _vertexGrid[yplus1, x].PreviousHeight +
						 _vertexGrid[yplus2, x].PreviousHeight +
						 _vertexGrid[yminus1, xminus1].PreviousHeight +
						 _vertexGrid[yminus1, xplus1].PreviousHeight +
						 _vertexGrid[yplus1, xminus1].PreviousHeight +
						 _vertexGrid[yplus1, xplus1].PreviousHeight)/6;

					newHeight -= _vertexGrid[y, x].CurrentHeight;
					newHeight *= Damping;
					if (newHeight < -1.0)
					{
						newHeight = -1.0;
					}
					if (newHeight > 1.0)
					{
						newHeight = 1.0;
					}

					_vertexGrid[y, x].CurrentHeight = newHeight;

					CalculateTexture(x, y);
				}
			}
		}

		private void ResetMesh()
		{
			for (int y = 0; y < _yVertices; y++)
			{
				for (int x = 0; x < _xVertices; x++)
				{
					int index = GetIndex(y, x);
					_mesh.Positions[index] = _vertexGrid[y, x].OriginalPosition;
					_mesh.TextureCoordinates[index] = _vertexGrid[y, x].OriginalTexture;
				}
			}

			Debug.WriteLine("---ResetMesh---\n" + _mesh.Positions);
		}

		private void CalculateTexture(int x, int y)
		{
			double xDiff = (x == _xVertices - 1)
			               	? 0
			               	: (_vertexGrid[y, x + 1].CurrentHeight -
			               	   _vertexGrid[y, x].CurrentHeight);
			double yDiff = (y == _yVertices - 1)
			               	? 0
			               	: (_vertexGrid[y + 1, x].CurrentHeight -
			               	   _vertexGrid[y, x].CurrentHeight);

			double xDisp = CalculateDisplacement(xDiff);
			double yDisp = CalculateDisplacement(yDiff);
			double u = 0;
			double v = 0;
			u = _vertexGrid[y, x].OriginalTexture.X + Math.Sign(xDiff)*xDisp;
			v = _vertexGrid[y, x].OriginalTexture.Y + Math.Sign(yDiff)*yDisp;

			if (u < 0)
			{
				u = 0;
			}
			if (u > 1)
			{
				u = 1;
			}
			if (v < 0)
			{
				v = 0;
			}
			if (v > 1)
			{
				v = 1;
			}
			_vertexGrid[y, x].Texture2 = new Point(u, v);
		}

		private double CalculateDisplacement(double heightDiff)
		{
			double angle = Math.Atan(heightDiff);
			double beamAngle = Math.Asin(Math.Sin(angle)/RefractionIndex);

			return Math.Tan(beamAngle)*(heightDiff + WaterDepth);
		}

		private void FireEffectCompleted()
		{
			if (EffectCompleted != null)
			{
				EffectCompleted();
			}
		}

		private bool IsWaveComplete()
		{
			bool complete = true;
			for (int y = 0; y < _yVertices; y++)
			{
				for (int x = 0; x < _xVertices; x++)
				{
					complete &= (_vertexGrid[y, x].PreviousHeight < 0.00001);
				}
			}

			return complete;
		}

		private int GetIndex(int row, int col)
		{
			return row*_xVertices + col;
		}

		public void PutTurbulence(Point pos)
		{
			_vertexGrid[(int) pos.Y, (int) pos.X].PreviousHeight = _turbulence;
		}

		private void Reset()
		{
			for (int y = 0; y < _yVertices; y++)
			{
				for (int x = 0; x < _xVertices; x++)
				{
					int index = GetIndex(y, x);
					WaterVertex vertex = _vertexGrid[y, x];

					_mesh.Positions[index] = vertex.OriginalPosition;
					_mesh.TextureCoordinates[index] = vertex.OriginalTexture;
				}
			}
		}

		private struct WaterVertex
		{
			public double CurrentHeight;
			public double PreviousHeight;
			public Point3D OriginalPosition;
			public Point OriginalTexture;
			public Point Texture2;
		}
	}
}