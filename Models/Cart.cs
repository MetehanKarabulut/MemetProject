using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemetProject.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public User User{ get; set; }
        public string CreatedDate { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}