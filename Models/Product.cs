using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemetProject.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public required string Description { get; set; }
        public int Price { get; set; }
        public bool StockControl { get; set; }
        public Category? Category { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public byte[]? ImageData { get; set; }
        
    }
}