using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using Infrasturcture.Global.Controls;

namespace Helpers
{
    public class GMapUtil
    {
        #region GetDirections
        public static DirectionSteps GetDirections(string origin, string destination)
        {
            var requestUrl = string.Format("http://maps.google.com/maps/api/directions/xml?origin={0}&destination={1}&sensor=false", origin, destination);
            try
            {
                var client = new WebClient();
                var result = client.DownloadString(requestUrl);
                return ParseDirectionResults(result);
            }
            catch (Exception)
            {
                return null;
            }

        }

        private static DirectionSteps ParseDirectionResults(string result)
        {
            DirectionSteps directionSteps = null;
            var xmlDoc = new XmlDocument {InnerXml = result};
            if (xmlDoc.HasChildNodes)
            {
                var directionsResponseNode = xmlDoc.SelectSingleNode("DirectionsResponse");
                if (directionsResponseNode != null)
                {
                    var statusNode = directionsResponseNode.SelectSingleNode("status");
                    if (statusNode != null && statusNode.InnerText.Equals("OK"))
                    {
                        var legs = directionsResponseNode.SelectNodes("route/leg");
                        if (legs != null && legs.Count > 0)
                        {
                            directionSteps = new DirectionSteps();
                            foreach (XmlNode leg in legs)
                            {
                                int stepCount = 1;
                                var stepNodes = leg.SelectNodes("step");
                                var steps = new List<DirectionStep>();
                                foreach (XmlNode stepNode in stepNodes)
                                {
                                    var directionStep = new DirectionStep();
                                    directionStep.Index = stepCount++;
                                    directionStep.Distance = stepNode.SelectSingleNode("distance/text").InnerText;
                                    directionStep.Duration = stepNode.SelectSingleNode("duration/text").InnerText;
                                    directionStep.Description = Regex.Replace(stepNode.SelectSingleNode("html_instructions").InnerText, "<[^<]+?>", "");
                                    steps.Add(directionStep);
                                }
                                directionSteps.OriginAddress = leg.SelectSingleNode("start_address").InnerText;
                                directionSteps.DestinationAddress = leg.SelectSingleNode("end_address").InnerText;
                                directionSteps.TotalDistance = leg.SelectSingleNode("distance/text").InnerText;
                                directionSteps.TotalDuration = leg.SelectSingleNode("duration/text").InnerText;
                                directionSteps.Steps = steps;
                            }
                        }
                    }
                }
            }
            return directionSteps;
        }

        #endregion

        #region SearchLocation
        public static List<LocationResult> GetLocations(string searchKey)
        {
            var requestUrl = string.Format("http://maps.google.com/maps/api/geocode/xml?address={0}&sensor=true", searchKey.Replace(" ", "+"));
            try
            {
                var client = new WebClient();
                var result = client.DownloadString(requestUrl);
                var results = ParseLocationResults(result);
                return results;
            }
            catch (Exception)
            {
                return null;
            }
        }


        private static List<LocationResult> ParseLocationResults(string result)
        {
            var xmlDoc = new XmlDocument();
            var results = new List<LocationResult>(); ;
            xmlDoc.InnerXml = result;
            if (xmlDoc.HasChildNodes)
            {
                var geocodeResponseNode = xmlDoc.SelectSingleNode("GeocodeResponse");
                if (geocodeResponseNode != null)
                {
                    var statusNode = geocodeResponseNode.SelectSingleNode("status");
                    if (statusNode != null && statusNode.InnerText.Equals("OK"))
                    {
                        var resultNodes = geocodeResponseNode.SelectNodes("result");
                        if (resultNodes != null && resultNodes.Count > 0)
                        {
                            int index = 1;
                            foreach (XmlNode resultNode in resultNodes)
                            {

                                var locationResult = CreateLocationResult(resultNode, index++);
                                if (locationResult != null)
                                    results.Add(locationResult);
                            }
                        }
                    }

                }
            }
            return results;
        }

        private static LocationResult CreateLocationResult(XmlNode resultNode, int index)
        {
            var locationResult = new LocationResult();
            try
            {
                var formattedAddressNode = resultNode.SelectSingleNode("formatted_address");
                var geometryNode = resultNode.SelectSingleNode("geometry");
                var locationNode = geometryNode.SelectSingleNode("location");
                var latitude = locationNode.SelectSingleNode("lat").InnerText;
                var longitude = locationNode.SelectSingleNode("lng").InnerText;

                locationResult.FormattedAddress = formattedAddressNode != null ? formattedAddressNode.InnerText : string.Empty;
                locationResult.Latitude = Double.Parse(latitude);
                locationResult.Longitude = Double.Parse(longitude);
                locationResult.Index = index;
            }
            catch
            {
                return null;
            }
            return locationResult;
        }
        #endregion
    }
}