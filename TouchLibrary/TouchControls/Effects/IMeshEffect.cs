using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace TouchControls.Effects
{
	public delegate void EffectCompletedHandler();

	public interface IMeshEffect
	{
		event EffectCompletedHandler EffectCompleted;

		void PlayEffect();

		int MeshDivisions { get; set; }

		MeshGeometry3D Mesh { get; set; }

		void Reset();
	}
}