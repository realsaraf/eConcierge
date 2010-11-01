using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrasturcture;
using Infrasturcture.DTO;

namespace DataAccessLayer
{
    public class DiningDAL
    {
        private static DiningDAL _diningDAL;

        private DiningDAL()
        {
        }

        public static DiningDAL GetInstance()
        {
            if (_diningDAL == null)
                _diningDAL = new DiningDAL();

            return _diningDAL;
        }

        public List<DTODiningCategory> GetCategories()
        {
            var categories = new List<DTODiningCategory>();
            categories.Add(new DTODiningCategory { Id = 1, Title = "Restaurants By Category" });
            categories.Add(new DTODiningCategory { Id = 2, Title = "Nightclubs" });
            categories.Add(new DTODiningCategory { Id = 3, Title = "Restaurants By Proximity" });
            categories.Add(new DTODiningCategory { Id = 4, Title = "All Bars" });
            categories.Add(new DTODiningCategory { Id = 5, Title = "Reommended Restaurants" });
            categories.Add(new DTODiningCategory { Id = 6, Title = "Bars By Proximity" });
            categories.Add(new DTODiningCategory { Id = 7, Title = "Dinner Theatre" });
            categories.Add(new DTODiningCategory { Id = 8, Title = "Recommended Bars" });
            return categories;
        }

        public List<DTODiningSubCategory> GetSubCategories(int categoryId)
        {
            var categories = new List<DTODiningSubCategory>();
            categories.Add(new DTODiningSubCategory { Id = 1, Title = "Mexican / Tex-Mex" + categoryId });
            categories.Add(new DTODiningSubCategory { Id = 2, Title = "Fast Food" });
            categories.Add(new DTODiningSubCategory { Id = 3, Title = "Indian" });
            categories.Add(new DTODiningSubCategory { Id = 4, Title = "Italian" });
            categories.Add(new DTODiningSubCategory { Id = 5, Title = "Asian" });
            categories.Add(new DTODiningSubCategory { Id = 6, Title = "Mediteranean" });
            categories.Add(new DTODiningSubCategory { Id = 7, Title = "Vegetarian / Vegan" });
            categories.Add(new DTODiningSubCategory { Id = 8, Title = "Cafeterias" });
            return categories;
        }

        public List<DTODining> GetDinings(int subCategoryId)
        {
            var dinings = new List<DTODining>();
            for (var i = 0; i < 9; i++)
            {
                var dining = new DTODining();
                dining.Title = "Teru Sushi @ Las Vegas Hilton" + i;
                dining.Description = @"Authentic Japanese sushi with Las Vegas inspriration comes alive at the Las Vegas Hilton. Teru Sushi features more than 40 types of fresh sushi flown in daily from waters around the world. You will find Chef Suichi performing his artistry every evening the restaurant is open to provide creative, and delicious sushi." + i;
                dining.Location = @"Las Vegas Hilton / 3000 Paradise Rd / Las Vegas, NV Reservations: (702) 732-5111" + i;
                dining.Photo = WpfUtil.GetImage(i + 1, @"Images\Dining\{0}.jpg");
                dinings.Add(dining);
            }
            return dinings;
        }
    }
}
