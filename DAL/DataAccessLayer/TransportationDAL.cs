using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrasturcture;
using Infrasturcture.DTO;

namespace DataAccessLayer
{
    public class TransportationDAL
    {
        private static TransportationDAL _transportationDAL;

        private TransportationDAL()
        {
        }

        public static TransportationDAL GetInstance()
        {
            if (_transportationDAL == null)
                _transportationDAL = new TransportationDAL();

            return _transportationDAL;
        }

        public List<DTOTransportationCategory> GetCategories()
        {
            var categories = new List<DTOTransportationCategory>();
            categories.Add(new DTOTransportationCategory { Id = 1, Title = "Airport Shuttles" });
            categories.Add(new DTOTransportationCategory { Id = 2, Title = "Parking Garages" });
            categories.Add(new DTOTransportationCategory { Id = 3, Title = "Buses" });
            categories.Add(new DTOTransportationCategory { Id = 4, Title = "Rental Cars" });
            categories.Add(new DTOTransportationCategory { Id = 5, Title = "Monorail" });
            categories.Add(new DTOTransportationCategory { Id = 6, Title = "Taxis" });
            return categories;
        }

        public List<DTOTaxiDetail> GetTaxis(int transportationId)
        {
            var taxis = new List<DTOTaxiDetail>();
            for (int i = 1; i <= 5; i++)
            {

                taxis.Add(GetTaxi(1, "Henderson Taxi Company" + i, "(702) 384-2322"));
                taxis.Add(GetTaxi(1, "Lucky Cab Co." + i, "(702) 384-1212"));
                taxis.Add(GetTaxi(1, "Nells Cab Company" + i, "(702) 384-3221"));
                taxis.Add(GetTaxi(1, "Ace Union Cab Company" + i, "(702) 384-4343"));
                taxis.Add(GetTaxi(1, "Western Cab Company" + i, "(702) 384-8181"));
                taxis.Add(GetTaxi(1, "Delux Taxi Cab Service" + i, "(702) 384-7227"));
                taxis.Add(GetTaxi(1, "Desert Cab Company" + i, "(702) 384-9801"));
                taxis.Add(GetTaxi(1, "Navana Cab company" + i, "(702) 384-1076"));
            }
            return taxis;
        }

        public List<DTOMonorailDetail> GetMonorailDetails(int transportationId)
        {
            var details = new List<DTOMonorailDetail>();
            for (var i = 0; i < 9; i++)
            {
                var detail = new DTOMonorailDetail();
                detail.Title = "Las Vegas Monorail" + i;
                detail.Description = @"The Las Vegas monorails runs from the MGM Grand to the Sahara\n"
                                       + "hotel. The route has seven stations. The Las Vegas monorails runs\n" 
                                       + "from the MGM Grand to the Sahara. The route has seven stations\n"
                                        + "The Las Vegas monorails runs from the MGM Grand to the Sahara.\n"
                                        + "hotel. The route has seven stations." + i;

                detail.Image = WpfUtil.GetImage(i + 1, @"Images\Dining\{0}.jpg");
                details.Add(detail);
            }
            return details;
        }

        public DTOTaxiDetail GetTaxi(int id, string title, string description)
        {
            var taxi = new DTOTaxiDetail();
            taxi.Id = id;
            taxi.Title = title;
            taxi.Description = description;
            return taxi;
        }

    }
}
