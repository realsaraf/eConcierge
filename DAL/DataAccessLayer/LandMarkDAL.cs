using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrasturcture;
using Infrasturcture.DTO;

namespace DataAccessLayer
{
    public class LandMarkDAL
    {
        private static LandMarkDAL _landMarkDAL;

        private LandMarkDAL()
        {
        }

        public static LandMarkDAL GetInstance()
        {
            if (_landMarkDAL == null)
                _landMarkDAL = new LandMarkDAL();

            return _landMarkDAL;
        }

        public List<DTOLandMark> GetLandMarks()
        {
            var landMarks = new List<DTOLandMark>();
            for (int i = 0; i < 16; i++)
            {
                var item = new DTOLandMark();
                item.Id = 1;
                item.Title = string.Format("Paris Las Vegas {0}", i + 1);
                item.Picture = WpfUtil.GetImage(i + 1, @"Images\POI\{0}.jpg");
                item.Address = "3655 Las Vegas Boulevard South, Las Vegas, NV";
                item.Telephone = "Phone-(877)603-4386";
                item.Description = "The air-conditioned, 38-storey hotel comprises a total of 2,916 rooms of which 295 are suites. Guests are welcomed in the lobby with a 24-hour reception, a safe, a cloakroom, and lift access. Facilities on offer here include a café, a newspaper stand, a hairdressing salon, a bar, a theatre, superb restaurants, and an Internet connection. Guests may also enjoy the 24-hour casino and shopping";
                landMarks.Add(item);
            }
            return landMarks;
        }
    }
}
