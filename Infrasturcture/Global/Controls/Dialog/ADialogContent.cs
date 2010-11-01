using System;
using System.Windows.Controls;
using Infrasturcture.Global.Helpers.Events;

namespace Infrasturcture.Global.Controls.Dialog
{
    public class ADialogContent : UserControl, IDialogContent
    {
        public event EventHandler<DataEventArgs> Closed;

        public void OnClosed(DataEventArgs dataEventArgs)
        {
            if (Closed != null)
                Closed(this, dataEventArgs);
        }
    }
}
