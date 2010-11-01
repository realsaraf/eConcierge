using System;
using System.Collections.Generic;
using System.IO;

namespace Infrasturcture.DTO
{
    public class DTOEvent
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public byte[] Photo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Location { get; set; }
    }
}