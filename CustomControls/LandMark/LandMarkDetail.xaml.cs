using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using CustomControls.Abstract;
using CustomControls.CategoryControl;
using CustomControls.Dining;
using CustomControls.InheritedFrameworkControls;
using Infrasturcture;
using Infrasturcture.DTO;
using Infrasturcture.TouchLibrary;
using TouchFramework.Events;
using TouchAction = Infrasturcture.TouchLibrary.TouchAction;

namespace CustomControls.LandMark
{
    /// <summary>
    /// Interaction logic for DiningDetail.xaml
    /// </summary>
    public partial class LandMarkDetail : AnimatableControl, IMTouchControl
    {
        public IMTContainer Container { get; set; }
        public IFrameworkManger FrameworkManager { get; set; }
        public BitmapImage Picture  { get; set; }
        public event EventHandler Closed;
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        
        public LandMarkDetail(DTOLandMark landMark)
        {
            InitializeComponent();
            Picture = WpfUtil.BytesToImageSource(landMark.Picture);
            Title = landMark.Title;
            Description = landMark.Description;
            Address = landMark.Address;
            Telephone = landMark.Telephone;
            closeButton.Click += CloseButtonClick;
            DataContext = this;
        }

        public void Load(IFrameworkManger frameworkManger, double left, double top)
        {
            FrameworkManager = frameworkManger;
            FrameworkManager.RegisterElement((IMTouchControl)closeButton, false, new[] { TouchAction.Tap });
            FrameworkManager.AddControlWithAllGestures(this, left, top);
        }

        void CloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        public void Close()
        {
            FrameworkManager.UnRegisterElement(closeButton);
            FrameworkManager.RemoveControl(this);
            if(Closed!=null)
                Closed(this,new EventArgs());
        }
    }
}
