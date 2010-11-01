using System.Collections.Generic;
using Infrasturcture;

namespace DataAccessLayer
{
    public class AccommodationDAL
    {
        private static AccommodationDAL _accommodationDAL;
        public static AccommodationDAL GetInstance()
        {
            return _accommodationDAL ?? (_accommodationDAL = new AccommodationDAL());
        }
        public List<byte[]> GetImages()
        {
            var images = new List<byte[]>();
            for(int i=1;i<=6;i++)
            {
                images.Add(WpfUtil.GetImage(i, @"Images\Accommodation\{0}.jpg"));
            }
            return images;
        }

    }
}
