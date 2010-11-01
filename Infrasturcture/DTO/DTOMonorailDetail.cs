using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrasturcture.DTO
{
    public class DTOMonorailDetail
    {
        public int Id { get; set; }

        public int TransportationId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }
    }
}
