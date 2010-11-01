using System;
using System.Windows;
using CustomControls.Dining;
using Infrasturcture.TouchLibrary;

namespace mConciergeClient
{
    public partial class MainWindow
    {
        private bool _skipDiningClose;
        void DiningUnchecked(object sender, RoutedEventArgs e)
        {
            if (!_skipDiningClose)
            {
                DiningControl.GetInstance().CloseWithChildren();
                _skipDiningClose = false;
            }
        }

        private void LoadDining()
        {
            FrameworkManager.RegisterElement(DiningTool, false, new[] { TouchAction.Tap });
            DiningTool.Checked += DiningToolChecked;
        }

        void DiningToolChecked(object sender, RoutedEventArgs e)
        {
            ShowDining();
        }

        private void ShowDining()
        {
                var control = DiningControl.GetInstance();
                var top = canvas.ActualHeight / 2 - (control.Height / 2);
                var left = canvas.ActualWidth / 2 - (control.Width / 2);
                control.Load(FrameworkManager, left, top);
                control.Closed += Dining_Closed;
                _skipDiningClose = false;
        }
        void Dining_Closed(object sender, EventArgs e)
        {
            _skipDiningClose = true;
            DiningTool.IsChecked = false;
        }

    }
}
