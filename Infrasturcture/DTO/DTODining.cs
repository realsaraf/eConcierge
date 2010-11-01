using System;
using System.Collections.Generic;
using System.IO;

namespace Infrasturcture.DTO
{
    public class DTODining
    {
        public int Id { get; set; }

        public int SubCategoryId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public byte[] Photo { get; set; }

        public string Location { get; set; }
    }
}