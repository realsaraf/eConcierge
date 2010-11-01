using Infrasturcture.Global.Helpers.Events;

namespace Infrasturcture.Global.Controls.Dialog
{
    public interface IDialogContent
    {
        void OnClosed(DataEventArgs dataEventArgs);
    }
}