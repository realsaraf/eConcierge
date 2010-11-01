using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrasturcture;

namespace DataAccessLayer
{
    public class PhotoGalleryDAL
    {
        private static PhotoGalleryDAL _photoGalleryDAL;
        public static PhotoGalleryDAL GetInstance()
        {
            return _photoGalleryDAL ?? (_photoGalleryDAL = new PhotoGalleryDAL());
        }
        public List<byte[]> GetImages()
        {
            var images = new List<byte[]>();
            for (int i = 1; i <= 6; i++)
            {
                images.Add(WpfUtil.GetImage(i, @"Images\Dining\{0}.jpg"));
            }
            return images;
        }
    }
}
