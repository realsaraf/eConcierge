using System;
using System.Collections.Generic;
using System.IO;
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
using Infrasturcture.DTO;

namespace CustomControls.Transportation
{
    /// <summary>
    /// Interaction logic for TaxiDetail.xaml
    /// </summary>
    public partial class TaxiDetail : UserControl
    {
        private const int NO_OF_ITEMS_PER_COLUMN = 2;
        private List<DTOTaxiDetail> _taxiDetails;
        public TaxiDetail()
        {
            InitializeComponent();
        }
        public int SetTaxi(int transportationId)
        {
            _taxiDetails = TransportationDAL.GetInstance().GetTaxis(transportationId);
            PopulateTaxiDetail();
            return _taxiDetails.Count / 8;
        }
        public void PopulateTaxiDetail(int pageIndex = 0)
        {
            grdCategory.Children.Clear();
            grdCategory.RowDefinitions.Clear();
            grdCategory.ColumnDefinitions.Clear();


            AddColumns();
            int col = 0, row = -1;
            for (int index = pageIndex * 8; index < (pageIndex + 1) * 8; index++)
            {
                DTOTaxiDetail detail = _taxiDetails[index];

                if (col == 0)
                {
                    grdCategory.RowDefinitions.Add(new RowDefinition());
                    row++;
                }
                TaxiItem item = new TaxiItem();
                item.txbTitle.Text = detail.Title;
                item.txbDescription.Text = detail.Description;
                grdCategory.Children.Add(item);
                item.SetValue(Grid.ColumnProperty, col);
                item.SetValue(Grid.RowProperty, row);
                if (col == 0)
                    item.Margin = new Thickness(0, 10, 60, 0);
                else
                    item.Margin = new Thickness(0, 10, 0, 0);
                col++;
                if (col == NO_OF_ITEMS_PER_COLUMN) col = 0;
            }
        }

        private void AddColumns()
        {
            for (int col = 0; col < NO_OF_ITEMS_PER_COLUMN; col++)
            {
                grdCategory.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }
    }
}
