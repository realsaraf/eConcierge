using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace CustomControls.MenuDiskControl
{
    /// <summary>
    /// Interaction logic for ProgressControl.xaml
    /// </summary>
    public partial class ProgressControl : UserControl
    {
        public ProgressControl()
        {
            this.InitializeComponent();
        }

        public void Start()
        {
            Visibility = Visibility.Visible;
            var spinStoryBoard = (Storyboard)FindResource("spinStoryBoard");
            spinStoryBoard.Begin();
        }

        public void Stop()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal,
                              new Action(
                                        delegate
                                        {
                                            Visibility = Visibility.Hidden;
                                            var spinStoryBoard = (Storyboard)FindResource("spinStoryBoard");
                                            spinStoryBoard.Stop();
                                        }
                                        )
                            );
        }
    }
}