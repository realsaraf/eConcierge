using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Diagnostics;
using System.Windows.Media;

namespace Microsoft.Samples.KMoore.WPFSamples.FlipTile3D
{
	internal class TileData
	{
		public bool TickData(Vector lastMouse, bool isFlipped)
		{
			bool somethingChanged = false;

			//active means nothing in the "flipped" mode
			bool isActiveItem = isActive(_locationDesired, _size, (Point)lastMouse) && !isFlipped;
			bool goodMouse = !isEmptyPoint((Point)lastMouse);

			#region rotation

			Quaternion rotationTarget = new Quaternion(new Vector3D(1, 0, 0), 0);

			//apply forces
			rotationTarget.Normalize();
			_rotationCurrent.Normalize();


			double angle = 0;
			Vector3D axis = new Vector3D(0, 0, 1);
			if (!double.IsNaN(lastMouse.X) && !isFlipped)
			{
				Point3D mouse = new Point3D(lastMouse.X, lastMouse.Y, 1);
				Vector3D line = mouse - _locationCurrent;
				Vector3D straight = new Vector3D(0, 0, 1);

				angle = Vector3D.AngleBetween(line, straight);
				axis = Vector3D.CrossProduct(line, straight);
			}
			Quaternion rotationForceTowardsMouse = new Quaternion(axis, -angle);

			Quaternion rotationForceToDesired = rotationTarget - _rotationCurrent;

			Quaternion rotationForce = rotationForceToDesired + rotationForceTowardsMouse;


			_rotationVelocity *= new Quaternion(rotationForce.Axis, rotationForce.Angle * .2);

			//dampenning
			_rotationVelocity = new Quaternion(_rotationVelocity.Axis, _rotationVelocity.Angle * (_weird - .3));

			//apply terminal velocity
			_rotationVelocity = new Quaternion(_rotationVelocity.Axis, _rotationVelocity.Angle);

			_rotationVelocity.Normalize();

			//apply to position
			_rotationCurrent *= _rotationVelocity;
			_rotationCurrent.Normalize();

			//see if there is any real difference between what we calculated and what actually exists
			if (AnyDiff(_quaternionRotation3D.Quaternion.Axis, _rotationCurrent.Axis, Diff) ||
					AnyDiff(_quaternionRotation3D.Quaternion.Angle, _rotationCurrent.Angle, Diff))
			{
				//if the angles are both ~0, the axis may be way off but the result is basically the same
				//check for this and forget animating in this case
				if (AnyDiff(_quaternionRotation3D.Quaternion.Angle, 0, Diff) || AnyDiff(_rotationCurrent.Angle, 0, Diff))
				{
					_quaternionRotation3D.Quaternion = _rotationCurrent;
					somethingChanged = true;
				}
			}


			#endregion

			#region flip
			double verticalFlipTarget = isFlipped ? 180 : 0;
			double verticalFlipCurrent = _verticalFlipRotation.Angle;

			//force
			double verticalFlipForce = verticalFlipTarget - verticalFlipCurrent;

			//velocity
			_flipVerticalVelocity += .3 * verticalFlipForce;

			//dampening
			_flipVerticalVelocity *= (_weird - .3);

			//terminal velocity
			_flipVerticalVelocity = limitDouble(_flipVerticalVelocity, 10);

			//apply
			verticalFlipCurrent += _flipVerticalVelocity;

			if (AnyDiff(verticalFlipCurrent, _verticalFlipRotation.Angle, Diff) && AnyDiff(_flipVerticalVelocity, 0, Diff))
			{
				_verticalFlipRotation.Angle = verticalFlipCurrent;
			}

			#endregion

			#region scale
			if (isActiveItem && !isFlipped)
			{
				this._scaleDesired = 2;
			}
			else
			{
				this._scaleDesired = 1;
			}

			double scaleForce = this._scaleDesired - this._scaleCurrent;
			this._scaleVelocity += .1 * scaleForce;
			//dampening
			this._scaleVelocity *= .8;
			//terminal velocity
			this._scaleVelocity = limitDouble(this._scaleVelocity, .05);
			this._scaleCurrent += this._scaleVelocity;

			if (AnyDiff(_scaleTransform.ScaleX, _scaleCurrent, Diff) || AnyDiff(_scaleTransform.ScaleY, _scaleCurrent, Diff))
			{
				this._scaleTransform.ScaleX = this._scaleCurrent;
				this._scaleTransform.ScaleY = this._scaleCurrent;
				somethingChanged = true;
			}

			#endregion

			#region location
			Vector3D locationForce;

			//apply forces
			if (isActiveItem)
			{
				_locationDesired.Z = .1;
			}
			else
			{
				_locationDesired.Z = 0;
			}
			locationForce = _locationDesired - _locationCurrent;

			//only repel the non-active items
			if (!isActiveItem && goodMouse && !isFlipped)
			{
				locationForce += .025 * invertVector(this.CurrentLocationVector - new Vector3D(lastMouse.X, lastMouse.Y, 0));
			}

			_locationVelocity += .1 * locationForce;

			//apply dampenning
			_locationVelocity *= (_weird - .3);

			//apply terminal velocity
			_locationVelocity = limitVector3D(_locationVelocity, .3);

			//apply velocity to location
			_locationCurrent += _locationVelocity;

			if ((GetVector(_translate) - (Vector3D)_locationCurrent).Length > Diff)
			{
				_translate.OffsetX = _locationCurrent.X;
				_translate.OffsetY = _locationCurrent.Y;
				_translate.OffsetZ = _locationCurrent.Z;
				somethingChanged = true;
			}
			#endregion

			return somethingChanged;
		}

		private static bool AnyDiff(double d1, double d2, double diff)
		{
			Debug.Assert(AreGoodNumbers(d1, d2, diff));
			Debug.Assert(diff >= 0);
			return Math.Abs(d1 - d2) > diff;
		}
		private static bool AnyDiff(Vector3D v1, Vector3D v2, double diff)
		{
			Debug.Assert(IsGoodNumber(diff));
			Debug.Assert(diff >= 0);
			double angleBetween = Vector3D.AngleBetween(v1, v2);
			return angleBetween > diff;
		}

		private static bool IsGoodNumber(double d)
		{
			return !double.IsNaN(d) && !double.IsInfinity(d);
		}
		private static bool AreGoodNumbers(params double[] d)
		{
			for (int i = 0; i < d.Length; i++)
			{
				if (!IsGoodNumber(d[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static Vector3D GetVector(TranslateTransform3D transform)
		{
			return new Vector3D(transform.OffsetX, transform.OffsetY, transform.OffsetZ);
		}

		public void Setup3DItem(Model3DGroup targetGroup, DiffuseMaterial diffuseMaterialBrushPair,
				Size size, Point center, Material backMaterial, Rect backTextureCoordinates)
		{
			_locationDesired = new Point3D(center.X, center.Y, 0);
			_locationCurrent = new Point3D(0, 0, Rnd.NextDouble() * 10 - 20);
			_size = size;

			Point3D topLeft = new Point3D(-size.Width / 2, size.Height / 2, 0);
			Point3D topRight = new Point3D(size.Width / 2, size.Height / 2, 0);
			Point3D bottomLeft = new Point3D(-size.Width / 2, -size.Height / 2, 0);
			Point3D bottomRight = new Point3D(size.Width / 2, -size.Height / 2, 0);

			DiffuseMaterial = diffuseMaterialBrushPair;

			_quad.Children.Add(
					CreateTile(
							diffuseMaterialBrushPair,
							backMaterial,
							_borderMaterial,
							new Size3D(size.Width, size.Height, .01),
							backTextureCoordinates));

			Transform3DGroup group = new Transform3DGroup();

			group.Children.Add(new RotateTransform3D(_verticalFlipRotation));
			group.Children.Add(new RotateTransform3D(this._quaternionRotation3D));

			group.Children.Add(_scaleTransform);
			group.Children.Add(_translate);

			_quad.Transform = group;

			targetGroup.Children.Add(_quad);
		}

		private static Model3DGroup CreateTile(Material frontMaterial, Material backMaterial, Material sideMaterial,
				Size3D size, Rect backMaterialCoordiantes)
		{
			//these are represent half the width, height, depth of the quads, since everything is from zero
			double w = size.X / 2;
			double h = size.Y / 2;
			double d = size.Z / 2;

			//front
			GeometryModel3D front = GetQuad(
					new Point3D(-w, -h, d),
					new Point3D(w, -h, d),
					new Point3D(w, h, d),
					new Point3D(-w, h, d),
					frontMaterial);

			//back
			GeometryModel3D back = GetQuad(
					new Point3D(-w, -h, d),
					new Point3D(w, -h, d),
					new Point3D(w, h, d),
					new Point3D(-w, h, d),
					backMaterial, backMaterialCoordiantes);

			RotateTransform3D backRotate =
					new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 180), new Point3D());
			RotateTransform3D backFlip =
					new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 180), new Point3D());

			Transform3DGroup backTransformGroup = new Transform3DGroup();
			backTransformGroup.Children.Add(backRotate);
			backTransformGroup.Children.Add(backFlip);

			back.Transform = backTransformGroup;

			GeometryModel3D bottom, left, top, right;
			//sides
			{
				//right
				right = GetQuad(
						new Point3D(w, -h, d),
						new Point3D(w, -h, -d),
						new Point3D(w, h, -d),
						new Point3D(w, h, d), sideMaterial);


				//left
				left = GetQuad(
						new Point3D(-w, -h, -d),
						new Point3D(-w, -h, d),
						new Point3D(-w, h, d),
						new Point3D(-w, h, -d), sideMaterial);


				//top
				top = GetQuad(
						new Point3D(-w, h, d),
						new Point3D(w, h, d),
						new Point3D(w, h, -d),
						new Point3D(-w, h, -d), sideMaterial);


				//bottom
				bottom = GetQuad(
						new Point3D(-w, -h, -d),
						new Point3D(w, -h, -d),
						new Point3D(w, -h, d),
						new Point3D(-w, -h, d), sideMaterial);

			}

			Model3DGroup group = new Model3DGroup();
			group.Children.Add(front);
			group.Children.Add(back);
			group.Children.Add(right);
			group.Children.Add(left);
			group.Children.Add(bottom);
			group.Children.Add(top);

			return group;
		}

		private static GeometryModel3D GetQuad(
				Point3D bottomLeft, Point3D bottomRight, Point3D topRight, Point3D topLeft,
				Material material)
		{
			return GetQuad(bottomLeft, bottomRight, topRight, topLeft, material, new Rect(0, 0, 1, 1));
		}

		private static GeometryModel3D GetQuad(
				Point3D bottomLeft, Point3D bottomRight, Point3D topRight, Point3D topLeft,
				Material material, Rect textureCoordinates)
		{

			MeshGeometry3D mesh = new MeshGeometry3D();
			mesh.Positions.Add(bottomLeft);
			mesh.Positions.Add(bottomRight);
			mesh.Positions.Add(topRight);
			mesh.Positions.Add(topLeft);

			mesh.TriangleIndices.Add(0);
			mesh.TriangleIndices.Add(1);
			mesh.TriangleIndices.Add(2);

			mesh.TriangleIndices.Add(2);
			mesh.TriangleIndices.Add(3);
			mesh.TriangleIndices.Add(0);

			mesh.TextureCoordinates.Add(textureCoordinates.BottomLeft);
			mesh.TextureCoordinates.Add(textureCoordinates.BottomRight);
			mesh.TextureCoordinates.Add(textureCoordinates.TopRight);
			mesh.TextureCoordinates.Add(textureCoordinates.TopLeft);

			GeometryModel3D gm3d = new GeometryModel3D(mesh, material);
			gm3d.BackMaterial = material;

			return gm3d;
		}

		private static bool isActive(Point3D center, Size size, Point mouse)
		{
			Point bottomLeft = new Point(center.X - size.Width / 2, center.Y - size.Height / 2);
			Point topRight = new Point(center.X + size.Width / 2, center.Y + size.Height / 2);

			Vector blDiff = mouse - bottomLeft;
			Vector trDiff = topRight - mouse;

			return blDiff.X >= 0 && blDiff.Y >= 0 && trDiff.X >= 0 && trDiff.Y >= 0;
		}

		private static bool isEmptyPoint(Point point)
		{
			return (double.IsNaN(point.X) && double.IsNaN(point.Y));
		}

		private static Vector3D invertVector(Vector3D input)
		{
			double invertLength = 1 / input.Length;
			input.Normalize();
			return invertLength * input;
		}

		private static Vector3D limitVector3D(Vector3D input, double max)
		{
			Debug.Assert(max > 0);
			Debug.Assert(!double.IsPositiveInfinity(max));
			Debug.Assert(!double.IsNaN(input.Length));

			if (input.Length > max)
			{
				input.Normalize();
				return input * max;
			}
			else
			{
				return input;
			}
		}

		private static double limitDouble(double input, double max)
		{
			Debug.Assert(max >= 0);

			if (Math.Abs(input) > max)
			{
				return Math.Sign(input) * max;
			}
			else
			{
				return input;
			}
		}

		#region fields
		private Point3D _locationDesired;
		private Size _size;

		private Point3D _locationCurrent;
		private Vector3D _locationVelocity;

		private double _scaleDesired = 1;
		private double _scaleCurrent = 1;
		private double _scaleVelocity = 0;

		private readonly double _weird = Rnd.NextDouble() * .1 + .85;

		private Vector3D CurrentLocationVector
		{
			get
			{
				return new Vector3D(_locationCurrent.X, _locationCurrent.Y, _locationCurrent.Z);
			}
		}

		private Quaternion _rotationCurrent = new Quaternion();
		private Quaternion _rotationVelocity = new Quaternion();

		private double _flipVerticalVelocity = 0;

		private readonly Model3DGroup _quad = new Model3DGroup();

		private readonly TranslateTransform3D _translate = new TranslateTransform3D();
		private readonly QuaternionRotation3D _quaternionRotation3D = new QuaternionRotation3D();
		private readonly AxisAngleRotation3D _verticalFlipRotation = new AxisAngleRotation3D(new Vector3D(1, 0, 0), Rnd.NextDouble() * 360 + 720);
		private readonly ScaleTransform3D _scaleTransform = new ScaleTransform3D();

		private readonly DiffuseMaterial _borderMaterial = new DiffuseMaterial(Brushes.Black);

		internal DiffuseMaterial DiffuseMaterial;

		#endregion

		internal const double Diff = .000001;
		internal static readonly Random Rnd = new Random();
	}
}
