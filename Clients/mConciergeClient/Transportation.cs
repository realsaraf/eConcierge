using System;
using System.Collections.Generic;
using System.Windows;
using CustomControls.Transportation;
using Infrasturcture.TouchLibrary;

namespace mConciergeClient
{
    public partial class MainWindow
    {
        private List<TransportationDetail> _transportationDetailControls = new List<TransportationDetail>();
        private bool _skipTransportationClose;
        private TransportationControl _transportationControl;

        void TransportationUnchecked(object sender, RoutedEventArgs e)
        {
            if (!_skipTransportationClose)
            {
                if (_transportationControl != null)
                    _transportationControl.Close();
                foreach (var transportationDetailControl in _transportationDetailControls.ToArray())
                {
                    transportationDetailControl.Close();
                }
                _skipTransportationClose = false;
            }
        }

        private void LoadTransportation()
        {
            FrameworkManager.RegisterElement(TransportationTool, false, new[] { TouchAction.Tap });
            TransportationTool.Checked += TransportationToolChecked;
        }

        void TransportationToolChecked(object sender, RoutedEventArgs e)
        {
            ShowTransportation();
        }

        private void ShowTransportation()
        {
            _transportationControl = new TransportationControl();
            var top = canvas.ActualHeight / 2 - (_transportationControl.Height / 2);
            var left = canvas.ActualWidth / 2 - (_transportationControl.Width / 2);
            _transportationControl.Load(FrameworkManager, left, top);
            _skipTransportationClose = false;
            _transportationControl.Closed += Transporation_Closed;
            _transportationControl.DetailControlAdded += ControlDetailControlAdded;
        }

        void ControlDetailControlAdded(object sender, Infrasturcture.Global.Helpers.Events.DataEventArgs e)
        {
            _transportationControl.Close();
            var transportationDetail = e.Data as TransportationDetail;
            _transportationDetailControls.Add(transportationDetail);
            transportationDetail.Closed += TransportationDetailClosed;
        }

        void TransportationDetailClosed(object sender, EventArgs e)
        {
            _transportationDetailControls.Remove((TransportationDetail) sender);
        }

        void Transporation_Closed(object sender, EventArgs e)
        {
            _skipTransportationClose = true;
            TransportationTool.IsChecked = false;
        }
    }
}