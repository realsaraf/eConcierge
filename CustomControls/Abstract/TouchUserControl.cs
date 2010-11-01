using System;
using System.Windows.Controls;
using Infrasturcture.TouchLibrary;

namespace CustomControls.Abstract
{
    public class TouchUserControl:UserControl, IDisposable
    {
        private IFrameworkManager _frameworkManager;
        private IMTContainer _container;
        public event EventHandler Tapped;

        protected TouchUserControl(IFrameworkManager frameworkManager)
        {
            _frameworkManager = frameworkManager;
            _container = _frameworkManager.RegisterElement(this, false, new TouchAction[] {TouchAction.Tap});
        }

        public void Dispose()
        {
            _frameworkManager.UnRegisterControl(_container.Id);
            _frameworkManager = null;
            _container = null;
        }

        public void OnTapped()
        {
            if (Tapped != null)
                Tapped(this, new EventArgs());
        }
    }
}