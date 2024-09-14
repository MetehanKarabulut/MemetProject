using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemetProject.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public Product? product{ get; set; }
        public Cart? Cart{get; set;}
        public int Quantity { get; set; }
    }
}