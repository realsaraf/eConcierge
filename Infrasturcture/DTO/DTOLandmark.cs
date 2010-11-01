using System;
using System.Collections.Generic;
using System.IO;

namespace Infrasturcture.DTO
{
    public class DTOLandMark
    {
        public int Id { get; set; }
        public byte[] Picture { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
    }
}