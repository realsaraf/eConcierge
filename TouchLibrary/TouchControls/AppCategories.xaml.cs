using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace TouchFramework.ControlHandlers
{
    /// <summary>
    /// Interaction logic for AppCategories.xaml
    /// </summary>
    public partial class AppCategories : UserControl
    {
        public Image NavigationMenu;
        //Recompile       public AppLauncher MainWindow;
        public AppCategories()
        {
            InitializeComponent();

            var i = new Image();
            BitmapSource bi = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\images\\BackgroundmTouch.png"));
            i.Source = bi;
            canvas1.Children.Add(i);

            var TB = new Image();
            BitmapSource tb = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\images\\TubesBottom.png"));
            TB.Source = tb;
            canvas1.Children.Add(TB);

            NavigationMenu = new Image();
            BitmapSource tf = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\images\\TubesFront.png"));
            NavigationMenu.Source = tf;
            canvas1.Children.Add(NavigationMenu);
        }

        public void LoadCategory(int Cat_Id)
        {

            //MainWindow.LoadCategory(Cat_Id);
            /*
            MainWindow.applications = new List<AppData>();
            XmlDocument doc = new XmlDocument();
            doc.Load("applications.xml");
            XmlElement root = doc.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("//category[" + Cat_Id + "]/app"); // You can filter elements here using XPath
            //for (int i = 0; i < nodelist.Count; i++)
            int i = 0;

            foreach (XmlNode node in nodes)
            {
                // Get the node's attributes
                XmlAttributeCollection attCol = node.Attributes;
                String name = attCol["name"].Value;
                String file= attCol["file"].Value;
                String description = attCol["description"].Value;
                String thumb = attCol["thumb"].Value;
                String image = attCol["image"].Value;



               MainWindow.AddApplication(name, file, description, thumb, image, i++);
 */

/*
                AppData openApp = new AppData();
                openApp.app_name = name;
                openApp.app_file = file;
                //openApp.app_description = description;
                openApp.app_thumb = thumb;
                openApp.app_image = image;
                openApp.UpdateData();
                openApp.SetPicture(System.IO.Directory.GetCurrentDirectory() + thumb);

                Canvas parentcanvas = this.Parent as Canvas;
                parentcanvas.Children.Add(openApp);
                
                Random random = new Random();
                 int xpos = random.Next(50, 1230);
                 int ypos = random.Next(150, 670);

                Canvas.SetLeft(openApp, xpos);
                Canvas.SetTop(openApp, ypos);
*/
               

                //Canvas.SetLeft(openApp, 1000);
                //ArtefactAnimator.AddEase ( object, properties, values, time, ease, delay );
                //ArtefactAnimator.AddEase(NavigationMenu, Canvas.TopProperty, 500, 3, AnimationTransitions.CubicEaseOut, 0); 
              
 //           }

            
            //MainWindow.StartAnimations();
 

        }
    }
}
