using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemetProject.Models
{
    public class User
    {
        public int UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EMail { get; set; }
        public required string Password { get; set; }
        public required string PhoneNumber { get; set; }
        public ICollection<Address>? UserAddress { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public required string Role { get; set; }

    }
}