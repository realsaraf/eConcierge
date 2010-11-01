using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Infrasturcture;
using Infrasturcture.DTO;

namespace DataAccessLayer
{
    public class CalendarDAL
    {
        private static CalendarDAL _calendarDAL;

        private CalendarDAL()
        {
        }

        public static CalendarDAL GetInstance()
        {
            if (_calendarDAL == null)
                _calendarDAL = new CalendarDAL();

            return _calendarDAL;
        }

        public List<DTOEvent> Events(int categoryId, DateTime date)
        {
            var events = new List<DTOEvent>();
            for (int i = 0; i < 8; i++)
            {
                var dtoEvent = new DTOEvent();
                dtoEvent.Title = "This is event title" + i;
                dtoEvent.Description = "Descreiption of events" + i;
                dtoEvent.StartDate = DateTime.Now.AddDays(-2);
                dtoEvent.EndDate = DateTime.Now.AddDays(2);
                dtoEvent.Location = "New York" + i;
                var imageData = WpfUtil.GetImage(i + 1, @"Images\Events\{0}.jpg");
                dtoEvent.Photo = imageData;
                events.Add(dtoEvent);
            }
            return events;
        }

        public List<DTOEventCategory> GetCategories()
        {
            var categories = new List<DTOEventCategory>();
            categories.Add(new DTOEventCategory { Id = 1, Title = "Dining" });
            categories.Add(new DTOEventCategory { Id = 2, Title = "Gambling" });
            categories.Add(new DTOEventCategory { Id = 3, Title = "Tours" });
            categories.Add(new DTOEventCategory { Id = 4, Title = "Theatre" });
            categories.Add(new DTOEventCategory { Id = 5, Title = "Movies" });
            categories.Add(new DTOEventCategory { Id = 6, Title = "Shopping" });
            categories.Add(new DTOEventCategory { Id = 7, Title = "Sports Event" });
            categories.Add(new DTOEventCategory { Id = 8, Title = "Adult" });
            categories.Add(new DTOEventCategory { Id = 9, Title = "Bars" });
            categories.Add(new DTOEventCategory { Id = 10, Title = "Pool" });
            categories.Add(new DTOEventCategory { Id = 11, Title = "Golf" });
            categories.Add(new DTOEventCategory { Id = 13, Title = "All" });
            return categories;
        }
    }
}
