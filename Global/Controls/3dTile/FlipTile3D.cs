using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Infrasturcture._3DTile;
using Microsoft.Samples.KMoore.WPFSamples.FlipTile3D;

namespace _3dTile
{
	public class FlipTile3D : WrapperElement<Viewport3D>, IFlipTile3D
	{


        public Window mainWindow;
        public int SelectedTileIndex;

		public FlipTile3D()
			: base(new Viewport3D())
		{

			Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(Setup3D));

			m_listener.Rendering += tick;

			Unloaded += (sender, e) => m_listener.StopListening();
		}

	    public FlipTile3D(List<byte[]> pictures):this()
	    {
	        _materials = GetSamplePictures(pictures);
	    }


		#region render/layout overrides

		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(this.RenderSize));
		}

		#endregion



		#region mouse overrides

        public void ManualMove(Point position)
        {
            _lastMouse = position;

        }
        public void ManualDown(Point position)
        {
            _lastMouse = position;
            SetClickedItem(_lastMouse);
            _isFlipped = !_isFlipped;
        }

        public void ManualChoseItem(Point position)
        {
            _isFlipped = !_isFlipped;
            if (_isFlipped)
            {
                SetClickedItem(position);
            }
        }
        public void ManualLeave()
        {
            _lastMouse = new Point(double.NaN, double.NaN);
        }

		protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
		{
			_lastMouse = e.GetPosition(this);
		}
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			_lastMouse = new Point(double.NaN, double.NaN);
		}
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			_isFlipped = !_isFlipped;
			if (_isFlipped)
			{
				SetClickedItem(e.GetPosition(this));
			}
		}


		#endregion

		#region private methods

		private void SetClickedItem(Point point)
		{
			//move point to center
			point -= (new Vector(this.ActualWidth / 2, this.ActualHeight / 2));

			//flip it
			point.Y *= -1;

			//scale it to 1x1 dimentions
			double scale = Math.Min(this.ActualHeight, this.ActualWidth) / 2;
			point = (Point)(((Vector)point) / scale);

			//set it up so that bottomLeft = 0,0 & topRight = 1,1 (positive Y is up)
			point = (Point)(((Vector)point + new Vector(1, 1)) * .5);

			//scale up so that the point coordinates align w/ the x/y scale
			point.Y *= c_yCount;
			point.X *= c_xCount;

			//now we have the indicies of the x & y of the tile clicked
			int yIndex = (int)Math.Floor(point.Y);
			int xIndex = (int)Math.Floor(point.X);

			int tileIndex = yIndex * c_xCount + xIndex;

            int tilecount = _tiles.Count();
            if (tileIndex < tilecount && tileIndex >= 0)
            {

                //SelectedTileIndex = tileIndex;
                _backMaterial.Brush = _tiles[tileIndex].DiffuseMaterial.Brush;
                //_backMaterial.Brush = Brushes.Black;

               // //var currWindow = mainWindow as MediaTable;
               //// currWindow.AddNewBrushToImage(_tiles[tileIndex].DiffuseMaterial.Brush as ImageBrush);
           
            }
		}

		private void Setup3D()
		{

			//perf improvement. Clipping in 3D is expensive. Avoid if you can!
			WrappedElement.ClipToBounds = false;

			PerspectiveCamera camera = new PerspectiveCamera(
					new Point3D(0, 0, 3.73), //position
					new Vector3D(0, 0, -1), //lookDirection
					new Vector3D(0, 1, 0), //upDirection
					30 //FOV
					);

			WrappedElement.Camera = camera;

			Model3DGroup everything = new Model3DGroup();

			Model3DGroup lights = new Model3DGroup();
			DirectionalLight whiteLight = new DirectionalLight(Colors.White, new Vector3D(0, 0, -1));
			lights.Children.Add(whiteLight);

			everything.Children.Add(lights);

			ModelVisual3D model = new ModelVisual3D();

			double tileSizeX = 2.0 / c_xCount;
			double startX = -((double)c_xCount) / 2 * tileSizeX + tileSizeX / 2;
			double startY = -((double)c_yCount) / 2 * tileSizeX + tileSizeX / 2;

			int index;

			Size tileTextureSize = new Size(1.0 / c_xCount, 1.0 / c_yCount);

			//so, tiles are numbers, left-to-right (ascending x), bottom-to-top (ascending y)
			for (int y = 0; y < c_yCount; y++)
			{
				for (int x = 0; x < c_xCount; x++)
				{
					index = y * c_xCount + x;

					Rect backTextureCoordinates = new Rect(
							x * tileTextureSize.Width,

							// this will give you a headache. Exists since we are going 
						// from bottom bottomLeft of 3D space (negative Y is down), 
						// but texture coor are negative Y is up
							1 - y * tileTextureSize.Height - tileTextureSize.Height,

							tileTextureSize.Width, tileTextureSize.Height);

					_tiles[index] = new TileData();
					_tiles[index].Setup3DItem(
							everything,
							getMaterial(index),
							new Size(tileSizeX, tileSizeX),
							new Point(startX + x * tileSizeX, startY + y * tileSizeX),
							_backMaterial,
							backTextureCoordinates);
				}
			}

			model.Content = everything;

			WrappedElement.Children.Add(model);

			//start the per-frame tick for the physics
			m_listener.StartListening();
		}


		private void tick(object sender, EventArgs e)
		{
			Vector mouseFixed = fixMouse(_lastMouse, this.RenderSize);
			for (int i = 0; i < _tiles.Length; i++)
			{
				//TODO: unwire Render event if nothing happens
				_tiles[i].TickData(mouseFixed, _isFlipped);
			}
		}

		private static Vector fixMouse(Point mouse, Size size)
		{

			Debug.Assert(size.Width >= 0 && size.Height >= 0);
			double scale = Math.Max(size.Width, size.Height) / 2;

			// Translate y going down to y going up
			mouse.Y = -mouse.Y + size.Height;

			mouse.Y -= size.Height / 2;
			mouse.X -= size.Width / 2;

			Vector v = new Vector(mouse.X, mouse.Y);

			v /= scale;

			return v;
		}


		#endregion

		#region Implementation

		private DiffuseMaterial getMaterial(int index)
		{
			return _materials[index % _materials.Count];
		}

		//private readonly IList<DiffuseMaterial> _materials = GetSamplePictures();
	    private readonly IList<DiffuseMaterial> _materials;

		private static IList<DiffuseMaterial> GetSamplePictures()
		{
			IList<DiffuseMaterial> materials;

			IList<string> files = Helpers.GetPicturePaths();
			if (files.Count > 0)
			{
				materials = new List<DiffuseMaterial>();

				foreach (string file in files)
				{
					Uri uri = new Uri(file);

					BitmapImage bitmapImage = new BitmapImage();
					bitmapImage.BeginInit();
					bitmapImage.UriSource = uri;
					bitmapImage.DecodePixelWidth = 320;
					bitmapImage.DecodePixelHeight = 240;
					bitmapImage.EndInit();

					bitmapImage.Freeze();

					ImageBrush imageBrush = new ImageBrush(bitmapImage);
					imageBrush.Stretch = Stretch.UniformToFill;
					imageBrush.ViewportUnits = BrushMappingMode.Absolute;
					imageBrush.Freeze();

					DiffuseMaterial diffuseMaterial = new DiffuseMaterial(imageBrush);
					materials.Add(diffuseMaterial);
				}
			}
			else
			{
				Brush[] brushes = new Brush[] { 
                    Brushes.LightBlue, 
                    Brushes.Pink, 
                    Brushes.LightGray, 
                    Brushes.Yellow, 
                    Brushes.Orange, 
                    Brushes.LightGreen };

				DiffuseMaterial[] materialsArray =
						brushes.Select(brush => new DiffuseMaterial(brush)).ToArray();

				materials = materialsArray;
			}

			return materials;
		}

        private static IList<DiffuseMaterial> GetSamplePictures(List<byte[]> pictures)
        {
            

             IList<DiffuseMaterial> materials = new List<DiffuseMaterial>();

            if(pictures.Count > 0)
            {
                foreach (var pic in pictures)
                {
                    MemoryStream stream = new MemoryStream(pic);
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = stream; 
                    bitmapImage.DecodePixelWidth = 320;
                    bitmapImage.DecodePixelHeight = 240;
                    bitmapImage.EndInit();

                    bitmapImage.Freeze();

                    ImageBrush imageBrush = new ImageBrush(bitmapImage);
                    imageBrush.Stretch = Stretch.UniformToFill;
                    imageBrush.ViewportUnits = BrushMappingMode.Absolute;
                    imageBrush.Freeze();

                    DiffuseMaterial diffuseMaterial = new DiffuseMaterial(imageBrush);
                    materials.Add(diffuseMaterial);
                }
            }
            else
            {
                Brush[] brushes = new Brush[] { 
                    Brushes.LightBlue, 
                    Brushes.Pink, 
                    Brushes.LightGray, 
                    Brushes.Yellow, 
                    Brushes.Orange, 
                    Brushes.LightGreen };

                DiffuseMaterial[] materialsArray =
                        brushes.Select(brush => new DiffuseMaterial(brush)).ToArray();

                materials = materialsArray;
            }

            return materials;
        }



		private Point _lastMouse = new Point(double.NaN, double.NaN);
		private bool _isFlipped;

		private readonly TileData[] _tiles = new TileData[c_xCount * c_yCount];
		private readonly DiffuseMaterial _backMaterial = new DiffuseMaterial();
		private readonly CompositionTargetRenderingListener m_listener =
				new CompositionTargetRenderingListener();

		private const int c_xCount = 7, c_yCount = 6;

		#endregion
	}

}