using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemetProject.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public User? User { get; set; }
        public required DateTime OrderDate { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
        public Payment? Payment { get; set; }
        public int TotalAmount { get; set; }
        public required string Status { get; set; }

    }
}