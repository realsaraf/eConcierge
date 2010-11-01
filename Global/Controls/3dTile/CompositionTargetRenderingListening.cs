/*
The MIT License

Copyright (c) 2008 Kevin Moore (http://j832.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
using System.Windows.Media;
using System.Windows.Threading;
using System.Diagnostics;
using System.Windows;
//using J832.Common;

namespace Microsoft.Samples.KMoore.WPFSamples.FlipTile3D
{
	public class CompositionTargetRenderingListener : DispatcherObject, IDisposable
	{
		public CompositionTargetRenderingListener() { }

		public void StartListening()
		{
			requireAccessAndNotDisposed();

			if (!m_isListening)
			{
				m_isListening = true;
				CompositionTarget.Rendering += compositionTarget_Rendering;
			}
		}

		public void StopListening()
		{
			requireAccessAndNotDisposed();

			if (m_isListening)
			{
				m_isListening = false;
				CompositionTarget.Rendering -= compositionTarget_Rendering;
			}
		}

		public void WireParentLoadedUnloaded(FrameworkElement parent)
		{
			requireAccessAndNotDisposed();
			Util.RequireNotNull(parent, "parent");

			parent.Loaded += delegate(object sender, RoutedEventArgs e)
			{
				this.StartListening();
			};

			parent.Unloaded += delegate(object sender, RoutedEventArgs e)
			{
				this.StopListening();
			};
		}

		public bool IsDisposed
		{
			get
			{
				VerifyAccess();
				return m_disposed;
			}
		}

		public event EventHandler Rendering;

		protected virtual void OnRendering(EventArgs args)
		{
			requireAccessAndNotDisposed();

			EventHandler handler = Rendering;
			if (handler != null)
			{
				handler(this, args);
			}
		}

		public void Dispose()
		{
			requireAccessAndNotDisposed();
			StopListening();

			Delegate[] invocationlist = Rendering.GetInvocationList();
			foreach (Delegate d in invocationlist)
			{
				Rendering -= (EventHandler)d;
			}

			m_disposed = true;
		}

		#region Implementation

		[DebuggerStepThrough]
		private void requireAccessAndNotDisposed()
		{
			VerifyAccess();
			if (m_disposed)
			{
				throw new ObjectDisposedException(string.Empty);
			}
		}

		private void compositionTarget_Rendering(object sender, EventArgs e)
		{
			OnRendering(e);
		}

		private bool m_isListening;
		private bool m_disposed;

		#endregion

	}

}