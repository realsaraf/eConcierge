using System.Windows;
using CustomControls.Abstract;
using Infrasturcture.TouchLibrary;

namespace CustomControls.Calendar
{
    /// <summary>
    /// Interaction logic for CalendarControl.xaml
    /// </summary>
    public partial class CalendarControl : AnimatableControl, IMTouchControl
    {
        private static CalendarControl _calendar;
        public static CalendarControl GetInstance()
        {
            return _calendar ?? (_calendar = new CalendarControl());
        }

        #region Properties
        
        public IMTContainer Container { get; set; }
        public bool IsDisplayed { get; set; }

        #endregion

        private CalendarControl()
        {
            InitializeComponent();
        }

        public FrameworkElement CloseButton
        {
            get
            {
                return CommandDisk;
            }
        }

        public override void Dispose()
        {
            _calendar = null;
        }
    }
}
