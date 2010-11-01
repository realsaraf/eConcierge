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

using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using System.Xml;

namespace TouchFramework.ControlHandlers
{
    /// <summary>
    /// Interaction logic for RssList.xaml
    /// </summary>
    public partial class RssList : UserControl
    {
        string url = string.Empty;

        public object SyncLock = new object();

        public string FeedUrl
        {
            get
            {
                lock (SyncLock)
                {
                    return url;
                }
            }
        }

        public RssList()
        {
            InitializeComponent();
        }

        public ListBox InternalList
        {
            get
            {
                return listBox1;
            }
        }

        public void Read(string url)
        {
            // Change the value of the feed query string to 'atom' to use Atom format.
            XmlReader reader = XmlReader.Create(url,
                  new XmlReaderSettings()
                  {
                      //MaxCharactersInDocument can be used to control the maximum amount of data 
                      //read from the reader and helps prevent OutOfMemoryException
                      //MaxCharactersInDocument = 1024 * 64
                  });


            SyndicationFeed feed = SyndicationFeed.Load(reader);
            label1.Content = feed.Title.Text;
            listBox1.ItemsSource = feed.Items;
        }
    }
}
