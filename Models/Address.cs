using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemetProject.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public User? User { get; set; }
        public required string AddressTitle { get; set; }
        public required string FullAddress { get; set; }
    }
}