using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace CustomControls.Weather
{

    public class WeatherHelper
    {
        public static List<WeatherElement> GetData(string location)
        {

            List<WeatherElement> elements = new List<WeatherElement>();

            if (!string.IsNullOrEmpty(location))
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.google.com/ig/api?weather=" + location);


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                XDocument doc = XDocument.Load(response.GetResponseStream());
                if (doc.Root.Element("problem_cause") == null)
                {
                    var x = from c in doc.Root.Element("weather").Elements() where c.Name == "forecast_conditions" select c;

                    foreach (XElement element in x)
                    {
                        //image = null;
                        WeatherElement welement = new WeatherElement();
                        welement.Condition = element.Element("condition").Attribute("data").Value;
                        welement.Day = element.Element("day_of_week").Attribute("data").Value;
                        welement.High = element.Element("high").Attribute("data").Value;
                        welement.Low = element.Element("low").Attribute("data").Value;
                        WeatherElement temp = elements.FirstOrDefault(e => e.Condition.Equals(welement.Condition));
                        if (temp != null)
                        {
                            welement.Icon = temp.Icon;
                        }
                        else
                        {
                            welement.Icon = new BitmapImage(new Uri("http://www.google.com/" + element.Element("icon").Attribute("data").Value));
                        }
                        elements.Add(welement);

                    }
                }
            }
            return elements;

        }
    }

    public class WeatherElement
    {
        public BitmapImage Icon { get; set; }
        public string Day { get; set; }
        public string Low { get; set; }
        public string High { get; set; }
        public string Condition { get; set; }
    }
}
