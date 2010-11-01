using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataAccessLayer;
using Infrasturcture;
using Infrasturcture.DTO;

namespace CustomControls.Transportation
{
    /// <summary>
    /// Interaction logic for MonorailDetail.xaml
    /// </summary>
    public partial class MonorailDetail : UserControl
    {
        private List<DTOMonorailDetail> _monorailDetails;

        public MonorailDetail()
        {
            InitializeComponent();
        }

        public int SetMonorail(int transportationId)
        {
            _monorailDetails = TransportationDAL.GetInstance().GetMonorailDetails(transportationId);
            PopulateMonorailDetail();
            return _monorailDetails.Count;
        }
        public void PopulateMonorailDetail(int pageIndex = 0)
        {
            DTOMonorailDetail detail = _monorailDetails[pageIndex];
            txbTitle.Text = detail.Title;
            txbDescription.Text = detail.Description;
            imgMonorail.Source = WpfUtil.BytesToImageSource(detail.Image);
        }
    }
}
