using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using CustomControls.Abstract;
using Helpers.Extensions;
using Infrasturcture.TouchLibrary;
using WebBrowser = Cjc.ChromiumBrowser.WebBrowser;

namespace TouchControls
{
    /// <summary>
    /// Interaction logic for ChromBrowser.xaml
    /// </summary>
    public partial class ChromBrowser : AnimatableControl, IMTouchControl
    {
        private readonly string _url;
        public WebBrowser newBrowser;

        public ChromBrowser(string url, double width, double height)
        {
            _url = url;
            InitializeComponent();
            newBrowser = new WebBrowser();
            newBrowser.SetValue(Grid.RowProperty, 1);
            newBrowser.Focusable = true;
            newBrowser.EnableAsyncRendering = false;
            newBrowser.Source = _url;
            newBrowser.Width = width;
            newBrowser.Height = height;
            //newBrowser.Navigate(GetUrl(newBrowser.Source));
            newBrowser.Ready += NewBrowserReady;
            newBrowser.SizeChanged += NewBrowserSizeChanged;
            newBrowser.Navigate(_url);
            BrowserContainer.Children.Add(newBrowser);
            Loaded += ChromBrowser_Loaded;
        }

        void ChromBrowser_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ((IMTContainer)Container).OnApplyingTransforms += OnApplyingTransforms;
        }

        private void OnApplyingTransforms(object sender, EventArgs e)
        {
            var translater = Container.Transforms.Children.OfType<TranslateTransform>().LastOrDefault();
            var scaler = Container.Transforms.Children.OfType<ScaleTransform>().LastOrDefault();
            if (translater != null)
            {
                
                //newBrowser
                //veMap.DoMapMove(translater.Value.OffsetX * 5, translater.Value.OffsetY * 5, new System.Windows.Point((ActualWidth / 2) - 50, (ActualWidth / 2) - 50));
            }
            if (scaler != null)
            {
                if (scaler.ScaleX > 1)
                {
                    newBrowser.InjectMouseWheel(1);
                }
                if (scaler.ScaleX < 1)
                    newBrowser.InjectMouseWheel(-1);
            }
        }


        private static string GetUrl(string address)
        {
            var source = address.Trim();
            return source.Contains(":") ? source : "http://" + source;
        }

        private void NewBrowserReady(object sender, EventArgs e)
        {
            ((WebBrowser)sender).Navigate(_url);
            newBrowser.Loaded -= NewBrowserReady;
        }


        private void NewBrowserSizeChanged(object sender, EventArgs e)
        {
            var browser = sender as WebBrowser;
            //Width = browser.ActualWidth;
            //Height = browser.ActualHeight;
            //OuterFrame.Width = browser.ActualWidth + 70;
            //OuterFrame.Height = browser.ActualHeight + 70;
        }

        public IMTContainer Container { get; set; }
    }
}
