using System;
using System.Windows.Controls;
using Infrasturcture.TouchLibrary;

namespace CustomControls.MapLocation
{
	/// <summary>
	/// Interaction logic for MapDirectionStepControl.xaml
	/// </summary>
	public partial class MapDirectionStepControl : UserControl, IMTouchControl
	{
        public IMTContainer Container { get; set; }

		public MapDirectionStepControl()
		{
			this.InitializeComponent();
		}
	}
}